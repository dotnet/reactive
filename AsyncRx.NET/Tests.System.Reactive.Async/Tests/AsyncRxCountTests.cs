// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxCountTests(ExecutionShape shape, DelegateForm delegates)
    : CountTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxCountTests_SynchronousCompletion()
    : AsyncRxCountTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxCountTests_ForcedYield()
    : AsyncRxCountTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxCountTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxCountTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxCountTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxCountTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
