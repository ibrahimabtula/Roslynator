﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Roslynator.CSharp.Syntax.SyntaxInfoHelpers;

namespace Roslynator.CSharp.Syntax
{
    public struct SimpleAssignmentExpressionInfo : IEquatable<SimpleAssignmentExpressionInfo>
    {
        private SimpleAssignmentExpressionInfo(
            AssignmentExpressionSyntax assignmentExpression,
            ExpressionSyntax left,
            ExpressionSyntax right)
        {
            AssignmentExpression = assignmentExpression;
            Left = left;
            Right = right;
        }

        private static SimpleAssignmentExpressionInfo Default { get; } = new SimpleAssignmentExpressionInfo();

        public AssignmentExpressionSyntax AssignmentExpression { get; }

        public ExpressionSyntax Left { get; }

        public ExpressionSyntax Right { get; }

        public SyntaxToken OperatorToken
        {
            get { return AssignmentExpression?.OperatorToken ?? default(SyntaxToken); }
        }

        public bool Success
        {
            get { return AssignmentExpression != null; }
        }

        internal static SimpleAssignmentExpressionInfo Create(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return CreateCore(Walk(node, walkDownParentheses) as AssignmentExpressionSyntax, walkDownParentheses, allowMissing);
        }

        internal static SimpleAssignmentExpressionInfo Create(
            AssignmentExpressionSyntax assignmentExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return CreateCore(assignmentExpression, walkDownParentheses, allowMissing);
        }

        internal static SimpleAssignmentExpressionInfo CreateCore(
            AssignmentExpressionSyntax assignmentExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            if (assignmentExpression?.Kind() != SyntaxKind.SimpleAssignmentExpression)
                return Default;

            ExpressionSyntax left = WalkAndCheck(assignmentExpression.Left, allowMissing, walkDownParentheses);

            if (left == null)
                return Default;

            ExpressionSyntax right = WalkAndCheck(assignmentExpression.Right, allowMissing, walkDownParentheses);

            if (right == null)
                return Default;

            return new SimpleAssignmentExpressionInfo(assignmentExpression, left, right);
        }

        public override string ToString()
        {
            return AssignmentExpression?.ToString() ?? base.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is SimpleAssignmentExpressionInfo other && Equals(other);
        }

        public bool Equals(SimpleAssignmentExpressionInfo other)
        {
            return EqualityComparer<AssignmentExpressionSyntax>.Default.Equals(AssignmentExpression, other.AssignmentExpression);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<AssignmentExpressionSyntax>.Default.GetHashCode(AssignmentExpression);
        }

        public static bool operator ==(SimpleAssignmentExpressionInfo info1, SimpleAssignmentExpressionInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(SimpleAssignmentExpressionInfo info1, SimpleAssignmentExpressionInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
