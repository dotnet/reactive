// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxLastOrDefaultAsyncTests(ExecutionShape shape, DelegateForm delegates)
    : LastOrDefaultAsyncTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxLastOrDefaultAsyncTests_SynchronousCompletion()
    : AsyncRxLastOrDefaultAsyncTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxLastOrDefaultAsyncTests_ForcedYield()
    : AsyncRxLastOrDefaultAsyncTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxLastOrDefaultAsyncTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxLastOrDefaultAsyncTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxLastOrDefaultAsyncTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxLastOrDefaultAsyncTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
