// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxAggregateTests(ExecutionShape shape, DelegateForm delegates)
    : AggregateTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxAggregateTests_SynchronousCompletion()
    : AsyncRxAggregateTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxAggregateTests_ForcedYield()
    : AsyncRxAggregateTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxAggregateTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxAggregateTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxAggregateTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxAggregateTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
