// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxSelectTests(ExecutionShape shape, DelegateForm delegates)
    : SelectTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxSelectTests_SynchronousCompletion()
    : AsyncRxSelectTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxSelectTests_ForcedYield()
    : AsyncRxSelectTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxSelectTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxSelectTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxSelectTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxSelectTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
