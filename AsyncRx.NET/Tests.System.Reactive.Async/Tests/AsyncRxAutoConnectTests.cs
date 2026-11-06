// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxAutoConnectTests(ExecutionShape shape, DelegateForm delegates)
    : AutoConnectTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxAutoConnectTests_SynchronousCompletion()
    : AsyncRxAutoConnectTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxAutoConnectTests_ForcedYield()
    : AsyncRxAutoConnectTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxAutoConnectTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxAutoConnectTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxAutoConnectTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxAutoConnectTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
