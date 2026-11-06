// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxSingleAsyncTests(ExecutionShape shape, DelegateForm delegates)
    : SingleAsyncTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxSingleAsyncTests_SynchronousCompletion()
    : AsyncRxSingleAsyncTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxSingleAsyncTests_ForcedYield()
    : AsyncRxSingleAsyncTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxSingleAsyncTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxSingleAsyncTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxSingleAsyncTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxSingleAsyncTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
