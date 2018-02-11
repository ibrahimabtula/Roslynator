﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Syntax
{
    //TODO: pub
    internal struct ModifiersInfo : IEquatable<ModifiersInfo>
    {
        private readonly ModifierFlags _flags;

        private ModifiersInfo(SyntaxNode node, SyntaxTokenList modifiers)
        {
            Node = node;
            Modifiers = modifiers;
            _flags = GetModifierFlags(modifiers);
        }

        private static ModifiersInfo Default { get; } = new ModifiersInfo();

        public SyntaxNode Node { get; }

        public SyntaxTokenList Modifiers { get; }

        //TODO: pub
        internal bool Any(ModifierFlags flags) => _flags.Any(flags);

        //TODO: pub
        internal bool All(ModifierFlags flags) => _flags.All(flags);

        //TODO: ren
        public bool HasNew => Any(ModifierFlags.New);

        public bool HasPublic => Any(ModifierFlags.Public);

        public bool HasPrivate => Any(ModifierFlags.Private);

        public bool HasProtected => Any(ModifierFlags.Protected);

        public bool HasInternal => Any(ModifierFlags.Internal);

        public bool HasConst => Any(ModifierFlags.Const);

        public bool HasStatic => Any(ModifierFlags.Static);

        public bool HasVirtual => Any(ModifierFlags.Virtual);

        public bool HasSealed => Any(ModifierFlags.Sealed);

        public bool HasOverride => Any(ModifierFlags.Override);

        public bool HasAbstract => Any(ModifierFlags.Abstract);

        public bool HasReadOnly => Any(ModifierFlags.ReadOnly);

        public bool HasExtern => Any(ModifierFlags.Extern);

        public bool HasUnsafe => Any(ModifierFlags.Unsafe);

        public bool HasVolatile => Any(ModifierFlags.Volatile);

        public bool HasAsync => Any(ModifierFlags.Async);

        public bool HasPartial => Any(ModifierFlags.Partial);

        public bool HasRef => Any(ModifierFlags.Ref);

        public bool HasOut => Any(ModifierFlags.Out);

        public bool HasIn => Any(ModifierFlags.In);

        public bool HasParams => Any(ModifierFlags.Params);

        public bool HasAbstractOrVirtualOrOverride
        {
            get { return Any(ModifierFlags.AbstractVirtualOverride); }
        }

        public Accessibility Accessibility
        {
            get
            {
                if (HasPublic)
                    return Accessibility.Public;

                if (HasInternal)
                    return (HasProtected) ? Accessibility.ProtectedOrInternal : Accessibility.Internal;

                if (HasProtected)
                    return (HasPrivate) ? Accessibility.ProtectedAndInternal : Accessibility.Protected;

                if (HasPrivate)
                    return Accessibility.Private;

                return Accessibility.NotApplicable;
            }
        }

        public bool Success
        {
            get { return Node != null; }
        }

        internal static ModifiersInfo Create(SyntaxNode node)
        {
            if (node == null)
                return Default;

            return new ModifiersInfo(node, node.GetModifiers());
        }

        internal static ModifiersInfo Create(ClassDeclarationSyntax classDeclaration)
        {
            if (classDeclaration == null)
                return Default;

            return new ModifiersInfo(classDeclaration, classDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(ConstructorDeclarationSyntax constructorDeclaration)
        {
            if (constructorDeclaration == null)
                return Default;

            return new ModifiersInfo(constructorDeclaration, constructorDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
        {
            if (conversionOperatorDeclaration == null)
                return Default;

            return new ModifiersInfo(conversionOperatorDeclaration, conversionOperatorDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(DelegateDeclarationSyntax delegateDeclaration)
        {
            if (delegateDeclaration == null)
                return Default;

            return new ModifiersInfo(delegateDeclaration, delegateDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(DestructorDeclarationSyntax destructorDeclaration)
        {
            if (destructorDeclaration == null)
                return Default;

            return new ModifiersInfo(destructorDeclaration, destructorDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(EnumDeclarationSyntax enumDeclaration)
        {
            if (enumDeclaration == null)
                return Default;

            return new ModifiersInfo(enumDeclaration, enumDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(EventDeclarationSyntax eventDeclaration)
        {
            if (eventDeclaration == null)
                return Default;

            return new ModifiersInfo(eventDeclaration, eventDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            if (eventFieldDeclaration == null)
                return Default;

            return new ModifiersInfo(eventFieldDeclaration, eventFieldDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(FieldDeclarationSyntax fieldDeclaration)
        {
            if (fieldDeclaration == null)
                return Default;

            return new ModifiersInfo(fieldDeclaration, fieldDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(IndexerDeclarationSyntax indexerDeclaration)
        {
            if (indexerDeclaration == null)
                return Default;

            return new ModifiersInfo(indexerDeclaration, indexerDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            if (interfaceDeclaration == null)
                return Default;

            return new ModifiersInfo(interfaceDeclaration, interfaceDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration == null)
                return Default;

            return new ModifiersInfo(methodDeclaration, methodDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(OperatorDeclarationSyntax operatorDeclaration)
        {
            if (operatorDeclaration == null)
                return Default;

            return new ModifiersInfo(operatorDeclaration, operatorDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(PropertyDeclarationSyntax propertyDeclaration)
        {
            if (propertyDeclaration == null)
                return Default;

            return new ModifiersInfo(propertyDeclaration, propertyDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(StructDeclarationSyntax structDeclaration)
        {
            if (structDeclaration == null)
                return Default;

            return new ModifiersInfo(structDeclaration, structDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(IncompleteMemberSyntax incompleteMember)
        {
            if (incompleteMember == null)
                return Default;

            return new ModifiersInfo(incompleteMember, incompleteMember.Modifiers);
        }

        internal static ModifiersInfo Create(AccessorDeclarationSyntax accessorDeclaration)
        {
            if (accessorDeclaration == null)
                return Default;

            return new ModifiersInfo(accessorDeclaration, accessorDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(LocalDeclarationStatementSyntax localDeclarationStatement)
        {
            if (localDeclarationStatement == null)
                return Default;

            return new ModifiersInfo(localDeclarationStatement, localDeclarationStatement.Modifiers);
        }

        internal static ModifiersInfo Create(LocalFunctionStatementSyntax localFunctionStatement)
        {
            if (localFunctionStatement == null)
                return Default;

            return new ModifiersInfo(localFunctionStatement, localFunctionStatement.Modifiers);
        }

        internal static ModifiersInfo Create(ParameterSyntax parameter)
        {
            if (parameter == null)
                return Default;

            return new ModifiersInfo(parameter, parameter.Modifiers);
        }

        private static ModifierFlags GetModifierFlags(SyntaxTokenList modifiers)
        {
            var flags = ModifierFlags.None;

            for (int i = 0; i < modifiers.Count; i++)
            {
                switch (modifiers[i].Kind())
                {
                    case SyntaxKind.PublicKeyword:
                        {
                            flags |= ModifierFlags.Public;
                            break;
                        }
                    case SyntaxKind.PrivateKeyword:
                        {
                            flags |= ModifierFlags.Private;
                            break;
                        }
                    case SyntaxKind.InternalKeyword:
                        {
                            flags |= ModifierFlags.Internal;
                            break;
                        }
                    case SyntaxKind.ProtectedKeyword:
                        {
                            flags |= ModifierFlags.Protected;
                            break;
                        }
                    case SyntaxKind.StaticKeyword:
                        {
                            flags |= ModifierFlags.Static;
                            break;
                        }
                    case SyntaxKind.ReadOnlyKeyword:
                        {
                            flags |= ModifierFlags.ReadOnly;
                            break;
                        }
                    case SyntaxKind.SealedKeyword:
                        {
                            flags |= ModifierFlags.Sealed;
                            break;
                        }
                    case SyntaxKind.ConstKeyword:
                        {
                            flags |= ModifierFlags.Const;
                            break;
                        }
                    case SyntaxKind.VolatileKeyword:
                        {
                            flags |= ModifierFlags.Volatile;
                            break;
                        }
                    case SyntaxKind.NewKeyword:
                        {
                            flags |= ModifierFlags.New;
                            break;
                        }
                    case SyntaxKind.OverrideKeyword:
                        {
                            flags |= ModifierFlags.Override;
                            break;
                        }
                    case SyntaxKind.AbstractKeyword:
                        {
                            flags |= ModifierFlags.Abstract;
                            break;
                        }
                    case SyntaxKind.VirtualKeyword:
                        {
                            flags |= ModifierFlags.Virtual;
                            break;
                        }
                    case SyntaxKind.RefKeyword:
                        {
                            flags |= ModifierFlags.Ref;
                            break;
                        }
                    case SyntaxKind.OutKeyword:
                        {
                            flags |= ModifierFlags.Out;
                            break;
                        }
                    case SyntaxKind.InKeyword:
                        {
                            flags |= ModifierFlags.In;
                            break;
                        }
                    case SyntaxKind.ParamsKeyword:
                        {
                            flags |= ModifierFlags.Params;
                            break;
                        }
                    case SyntaxKind.UnsafeKeyword:
                        {
                            flags |= ModifierFlags.Unsafe;
                            break;
                        }
                    case SyntaxKind.PartialKeyword:
                        {
                            flags |= ModifierFlags.Partial;
                            break;
                        }
                    case SyntaxKind.AsyncKeyword:
                        {
                            flags |= ModifierFlags.Async;
                            break;
                        }
                    default:
                        {
                            Debug.Fail(modifiers[i].Kind().ToString());
                            break;
                        }
                }
            }

            return flags;
        }

        public override string ToString()
        {
            return Node?.ToString() ?? base.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is ModifiersInfo other && Equals(other);
        }

        public bool Equals(ModifiersInfo other)
        {
            return EqualityComparer<SyntaxNode>.Default.Equals(Node, other.Node);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<SyntaxNode>.Default.GetHashCode(Node);
        }

        public static bool operator ==(ModifiersInfo info1, ModifiersInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(ModifiersInfo info1, ModifiersInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
