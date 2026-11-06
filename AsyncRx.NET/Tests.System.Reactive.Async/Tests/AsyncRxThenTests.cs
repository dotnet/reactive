// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxThenTests(ExecutionShape shape, DelegateForm delegates)
    : ThenTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxThenTests_SynchronousCompletion()
    : AsyncRxThenTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxThenTests_ForcedYield()
    : AsyncRxThenTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxThenTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxThenTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxThenTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxThenTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
