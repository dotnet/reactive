// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxLastAsyncTests(ExecutionShape shape, DelegateForm delegates)
    : LastAsyncTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxLastAsyncTests_SynchronousCompletion()
    : AsyncRxLastAsyncTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxLastAsyncTests_ForcedYield()
    : AsyncRxLastAsyncTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxLastAsyncTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxLastAsyncTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxLastAsyncTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxLastAsyncTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
