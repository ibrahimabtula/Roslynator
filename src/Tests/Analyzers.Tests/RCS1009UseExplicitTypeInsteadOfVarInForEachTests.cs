﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

#pragma warning disable RCS1090

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1009UseExplicitTypeInsteadOfVarInForEachTests : AbstractCSharpCodeFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.UseExplicitTypeInsteadOfVarInForEach;

        public override DiagnosticAnalyzer Analyzer { get; } = new UseExplicitTypeInForEachAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new UseExplicitTypeInsteadOfVarInForEachCodeFixProvider();

        [Fact]
        public async Task TestDiagnostic()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
using System.Collections.Generic;

class C
{
    public void M()
    {
        var items = new List<DateTime>();

        foreach ([|var|] item in items)
        {
        }
    }
}
", @"
using System;
using System.Collections.Generic;

class C
{
    public void M()
    {
        var items = new List<DateTime>();

        foreach (DateTime item in items)
        {
        }
    }
}
");
        }

        [Fact]
        public async Task TestNoDiagnostic()
        {
            await VerifyNoDiagnosticAsync(@"
using System;
using System.Collections.Generic;

class C
{
    public void M()
    {
        var items = new List<DateTime>();

        foreach (DateTime item in items)
        {
        }
    }
}");
        }
    }
}
