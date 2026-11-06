// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxMinByTests(ExecutionShape shape, DelegateForm delegates)
    : MinByTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxMinByTests_SynchronousCompletion()
    : AsyncRxMinByTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxMinByTests_ForcedYield()
    : AsyncRxMinByTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxMinByTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxMinByTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxMinByTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxMinByTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
