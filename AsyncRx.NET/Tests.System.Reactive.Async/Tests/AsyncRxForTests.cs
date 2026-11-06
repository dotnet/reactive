// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxForTests(ExecutionShape shape, DelegateForm delegates)
    : ForTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxForTests_SynchronousCompletion()
    : AsyncRxForTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxForTests_ForcedYield()
    : AsyncRxForTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxForTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxForTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxForTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxForTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
