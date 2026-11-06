// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxSingleOrDefaultAsyncTests(ExecutionShape shape, DelegateForm delegates)
    : SingleOrDefaultAsyncTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxSingleOrDefaultAsyncTests_SynchronousCompletion()
    : AsyncRxSingleOrDefaultAsyncTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxSingleOrDefaultAsyncTests_ForcedYield()
    : AsyncRxSingleOrDefaultAsyncTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxSingleOrDefaultAsyncTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxSingleOrDefaultAsyncTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxSingleOrDefaultAsyncTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxSingleOrDefaultAsyncTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
