﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Roslynator.Tests;
using Xunit;

#pragma warning disable RCS1090

namespace Roslynator.CSharp.CodeFixes.Tests
{
    public class CS1983ReturnTypeOfAsyncMethodMustBeVoidOrTaskOrTaskOfTTests : AbstractCSharpCompilerCodeFixVerifier
    {
        public override string DiagnosticId { get; } = CompilerDiagnosticIdentifiers.ReturnTypeOfAsyncMethodMustBeVoidOrTaskOrTaskOfT;

        public override CodeFixProvider FixProvider { get; } = new ReturnTypeOfAsyncMethodMustBeVoidOrTaskOrTaskOfTCodeFixProvider();

        public override CodeVerificationOptions Options { get; } = CodeVerificationOptions.Default.AddAllowedCompilerDiagnosticId(CompilerDiagnosticIdentifiers.SinceMethodIsAsyncMethodThatReturnsTaskReturnKeywordMustNotBeFollowedByObjectExpression);

        [Fact]
        public async Task Test_Task()
        {
            await VerifyFixAsync(@"
using System.Threading.Tasks;

public class Foo
{
    public async object Bar()
    {
        await DoAsync().ConfigureAwait(false);

        return await GetAsync().ConfigureAwait(false);
    }

    public async object Bar2()
    {
        await DoAsync().ConfigureAwait(false);
    }

    public void Bar3()
    {
        async object LocalBar()
        {
            await DoAsync().ConfigureAwait(false);

            return await GetAsync().ConfigureAwait(false);
        }

        async object LocalBar2()
        {
            await DoAsync().ConfigureAwait(false);
        }
    }

    public Task<object> GetAsync()
    {
        return Task.FromResult(default(object));
    }

    public Task DoAsync()
    {
        return Task.CompletedTask;
    }
}
", @"
using System.Threading.Tasks;

public class Foo
{
    public async Task Bar()
    {
        await DoAsync().ConfigureAwait(false);

        return await GetAsync().ConfigureAwait(false);
    }

    public async Task Bar2()
    {
        await DoAsync().ConfigureAwait(false);
    }

    public void Bar3()
    {
        async Task LocalBar()
        {
            await DoAsync().ConfigureAwait(false);

            return await GetAsync().ConfigureAwait(false);
        }

        async Task LocalBar2()
        {
            await DoAsync().ConfigureAwait(false);
        }
    }

    public Task<object> GetAsync()
    {
        return Task.FromResult(default(object));
    }

    public Task DoAsync()
    {
        return Task.CompletedTask;
    }
}
", EquivalenceKey.Create(DiagnosticId, "Task"));
        }

        [Fact]
        public async Task Test_TaskOfT()
        {
            await VerifyFixAsync(@"
using System.Threading.Tasks;

public class Foo
{
    public async object Bar()
    {
        object x = await GetAsync().ConfigureAwait(false);

        return await GetAsync().ConfigureAwait(false);
    }

    public async object Bar2()
    {
        await DoAsync().ConfigureAwait(false);

        return await GetAsync().ConfigureAwait(false);
    }

    public async object Bar3() => await GetAsync().ConfigureAwait(false);

    public void Bar4()
    {
        async object LocalBar()
        {
            object x = await GetAsync().ConfigureAwait(false);

            return await GetAsync().ConfigureAwait(false);
        }

        async object LocalBar2()
        {
            await DoAsync().ConfigureAwait(false);

            return await GetAsync().ConfigureAwait(false);
        }

        async object LocalBar3() => await GetAsync().ConfigureAwait(false);
    }

    public Task<object> GetAsync()
    {
        return Task.FromResult(default(object));
    }

    public Task DoAsync()
    {
        return Task.CompletedTask;
    }
}
", @"
using System.Threading.Tasks;

public class Foo
{
    public async Task<object> Bar()
    {
        object x = await GetAsync().ConfigureAwait(false);

        return await GetAsync().ConfigureAwait(false);
    }

    public async Task<object> Bar2()
    {
        await DoAsync().ConfigureAwait(false);

        return await GetAsync().ConfigureAwait(false);
    }

    public async Task<object> Bar3() => await GetAsync().ConfigureAwait(false);

    public void Bar4()
    {
        async Task<object> LocalBar()
        {
            object x = await GetAsync().ConfigureAwait(false);

            return await GetAsync().ConfigureAwait(false);
        }

        async Task<object> LocalBar2()
        {
            await DoAsync().ConfigureAwait(false);

            return await GetAsync().ConfigureAwait(false);
        }

        async Task<object> LocalBar3() => await GetAsync().ConfigureAwait(false);
    }

    public Task<object> GetAsync()
    {
        return Task.FromResult(default(object));
    }

    public Task DoAsync()
    {
        return Task.CompletedTask;
    }
}
", EquivalenceKey.Create(DiagnosticId, "TaskOfT"));
        }

        [Fact]
        public async Task TestNoFix()
        {
            const string source = @"
using System.Threading.Tasks;

public class Foo
{
    public async X Bar()
    {
        await DoAsync().ConfigureAwait(false);

        return await GetAsync().ConfigureAwait(false);
    }

    public async X Bar2() => await GetAsync().ConfigureAwait(false);

    public void LocalFunction()
    {
        async X LocalBar()
        {
            await DoAsync().ConfigureAwait(false);

            return await GetAsync().ConfigureAwait(false);
        }

        async X LocalBar2() => await GetAsync().ConfigureAwait(false);
    }

    public Task<object> GetAsync()
    {
        return Task.FromResult(default(object));
    }

    public Task DoAsync()
    {
        return Task.CompletedTask;
    }
}
";
            await VerifyNoFixAsync(source, EquivalenceKey.Create(DiagnosticId, "Task"));

            await VerifyNoFixAsync(source, EquivalenceKey.Create(DiagnosticId, "TaskOfT"));
        }
    }
}
