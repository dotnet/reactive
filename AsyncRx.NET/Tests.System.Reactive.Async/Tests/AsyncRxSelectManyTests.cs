// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxSelectManyTests(ExecutionShape shape, DelegateForm delegates)
    : SelectManyTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxSelectManyTests_SynchronousCompletion()
    : AsyncRxSelectManyTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxSelectManyTests_ForcedYield()
    : AsyncRxSelectManyTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxSelectManyTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxSelectManyTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxSelectManyTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxSelectManyTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
