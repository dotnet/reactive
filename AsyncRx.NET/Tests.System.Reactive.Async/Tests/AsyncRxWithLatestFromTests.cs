// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxWithLatestFromTests(ExecutionShape shape, DelegateForm delegates)
    : WithLatestFromTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxWithLatestFromTests_SynchronousCompletion()
    : AsyncRxWithLatestFromTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxWithLatestFromTests_ForcedYield()
    : AsyncRxWithLatestFromTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxWithLatestFromTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxWithLatestFromTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxWithLatestFromTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxWithLatestFromTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
