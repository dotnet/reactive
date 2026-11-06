// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxAnyTests(ExecutionShape shape, DelegateForm delegates)
    : AnyTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxAnyTests_SynchronousCompletion()
    : AsyncRxAnyTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxAnyTests_ForcedYield()
    : AsyncRxAnyTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxAnyTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxAnyTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxAnyTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxAnyTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
