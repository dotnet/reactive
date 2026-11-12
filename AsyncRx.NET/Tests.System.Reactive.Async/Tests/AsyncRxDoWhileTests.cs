// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxDoWhileTests(ExecutionShape shape, DelegateForm delegates)
    : DoWhileTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxDoWhileTests_SynchronousCompletion()
    : AsyncRxDoWhileTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxDoWhileTests_ForcedYield()
    : AsyncRxDoWhileTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxDoWhileTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxDoWhileTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxDoWhileTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxDoWhileTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
