﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Xunit;

#pragma warning disable RCS1090

namespace Roslynator.CSharp.Refactorings.Tests
{
    public class RR0159SwapBinaryOperandsTests : AbstractCSharpCodeRefactoringVerifier
    {
        public override string RefactoringId { get; } = RefactoringIdentifiers.SwapBinaryOperands;

        [Theory]
        [InlineData("f &[||]& f2", "f2 && f")]
        [InlineData("f |[||]| f2", "f2 || f")]
        [InlineData("i =[||]= j", "j == i")]
        [InlineData("i ![||]= j", "j != i")]
        [InlineData("i [||]> j", "j < i")]
        [InlineData("i >[||]= j", "j <= i")]
        [InlineData("i [||]< j", "j > i")]
        [InlineData("i <[||]= j", "j >= i")]
        public async Task Test(string fromData, string toData)
        {
            await VerifyRefactoringAsync(@"
class C
{
    void M()
    {
        bool f = false;
        bool f2 = false;
        int i = 0;
        int j = 0;
            
        if ([||]) { }
    }
}
", fromData, toData, RefactoringId);
        }

        [Theory]
        [InlineData("i [||]+ j", "j + i")]
        [InlineData("i [||]* j", "j * i")]
        public async Task Test_AddMultiply(string fromData, string toData)
        {
            await VerifyRefactoringAsync(@"
class C
{
    void M(int i, int j)
    {
        int k = [||];
    }
}
", fromData, toData, RefactoringId);
        }

        [Fact]
        public async Task TestNoRefactoring()
        {
            await VerifyNoRefactoringAsync(@"
class C
{
    void M(object x, bool f)
    {
        if (x =[||]= null) { }
        if (x ![||]= null) { }
        if (f =[||]= false) { }
        if (f =[||]= true) { }
    }
}
", RefactoringId);
        }
    }
}
