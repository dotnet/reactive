// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxDistinctTests(ExecutionShape shape, DelegateForm delegates)
    : DistinctTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxDistinctTests_SynchronousCompletion()
    : AsyncRxDistinctTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxDistinctTests_ForcedYield()
    : AsyncRxDistinctTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxDistinctTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxDistinctTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxDistinctTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxDistinctTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
