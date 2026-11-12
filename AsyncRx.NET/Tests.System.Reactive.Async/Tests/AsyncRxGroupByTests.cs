// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxGroupByTests(ExecutionShape shape, DelegateForm delegates)
    : GroupByTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxGroupByTests_SynchronousCompletion()
    : AsyncRxGroupByTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxGroupByTests_ForcedYield()
    : AsyncRxGroupByTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxGroupByTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxGroupByTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxGroupByTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxGroupByTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
