// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxMaxTests(ExecutionShape shape, DelegateForm delegates)
    : MaxTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxMaxTests_SynchronousCompletion()
    : AsyncRxMaxTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxMaxTests_ForcedYield()
    : AsyncRxMaxTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxMaxTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxMaxTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxMaxTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxMaxTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
