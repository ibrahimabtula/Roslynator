﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CodeFixes;

namespace Roslynator.CSharp.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MethodDeclarationOrLocalFunctionStatementCodeFixProvider))]
    [Shared]
    public class MethodDeclarationOrLocalFunctionStatementCodeFixProvider : BaseCodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(CompilerDiagnosticIdentifiers.BodyCannotBeIteratorBlockBecauseTypeIsNotIteratorInterfaceType); }
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            if (!Settings.IsCodeFixEnabled(CodeFixIdentifiers.ChangeMethodReturnType))
                return;

            SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

            if (!TryFindFirstAncestorOrSelf(root, context.Span, out SyntaxNode node, predicate: f => f.IsKind(SyntaxKind.MethodDeclaration, SyntaxKind.LocalFunctionStatement)))
                return;

            foreach (Diagnostic diagnostic in context.Diagnostics)
            {
                switch (diagnostic.Id)
                {
                    case CompilerDiagnosticIdentifiers.BodyCannotBeIteratorBlockBecauseTypeIsNotIteratorInterfaceType:
                        {
                            if (!Settings.IsCodeFixEnabled(CodeFixIdentifiers.ChangeMethodReturnType))
                                break;

                            BlockSyntax body = (node is MethodDeclarationSyntax methodDeclaration)
                                ? methodDeclaration.Body
                                : ((LocalFunctionStatementSyntax)node).Body;

                            Debug.Assert(body != null, node.ToString());

                            if (body == null)
                                break;

                            SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

                            ITypeSymbol typeSymbol = null;
                            HashSet<ITypeSymbol> typeSymbols = null;
                            INamedTypeSymbol ienumerableOfTSymbol = null;

                            foreach (SyntaxNode descendant in body.DescendantNodes(descendIntoChildren: f => !CSharpFacts.IsFunction(f.Kind())))
                            {
                                if (!descendant.IsKind(SyntaxKind.YieldReturnStatement))
                                    continue;

                                var yieldReturn = (YieldStatementSyntax)descendant;

                                ExpressionSyntax expression = yieldReturn.Expression;

                                if (expression == null)
                                    continue;

                                var namedTypeSymbol = semanticModel.GetTypeSymbol(expression, context.CancellationToken) as INamedTypeSymbol;

                                if (namedTypeSymbol?.IsErrorType() != false)
                                    continue;

                                if (typeSymbol == null)
                                {
                                    typeSymbol = namedTypeSymbol;
                                }
                                else
                                {
                                    if (typeSymbols == null)
                                    {
                                        typeSymbols = new HashSet<ITypeSymbol>() { typeSymbol };
                                    }

                                    if (!typeSymbols.Add(namedTypeSymbol))
                                        continue;
                                }

                                if (ienumerableOfTSymbol == null)
                                    ienumerableOfTSymbol = semanticModel.GetTypeByMetadataName(MetadataNames.System_Collections_Generic_IEnumerable_T);

                                CodeFixRegistrator.ChangeReturnType(
                                    context,
                                    diagnostic,
                                    node,
                                    ienumerableOfTSymbol.Construct(namedTypeSymbol),
                                    semanticModel,
                                    additionalKey: SymbolDisplay.ToMinimalDisplayString(namedTypeSymbol, semanticModel, node.SpanStart, SymbolDisplayFormats.Default));
                            }

                            break;
                        }
                }
            }
        }
    }
}
