// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxFirstOrDefaultAsyncTests(ExecutionShape shape, DelegateForm delegates)
    : FirstOrDefaultAsyncTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxFirstOrDefaultAsyncTests_SynchronousCompletion()
    : AsyncRxFirstOrDefaultAsyncTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxFirstOrDefaultAsyncTests_ForcedYield()
    : AsyncRxFirstOrDefaultAsyncTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxFirstOrDefaultAsyncTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxFirstOrDefaultAsyncTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxFirstOrDefaultAsyncTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxFirstOrDefaultAsyncTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
