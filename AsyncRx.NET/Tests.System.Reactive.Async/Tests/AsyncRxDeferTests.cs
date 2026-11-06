// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxDeferTests(ExecutionShape shape, DelegateForm delegates)
    : DeferTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxDeferTests_SynchronousCompletion()
    : AsyncRxDeferTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxDeferTests_ForcedYield()
    : AsyncRxDeferTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxDeferTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxDeferTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxDeferTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxDeferTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
