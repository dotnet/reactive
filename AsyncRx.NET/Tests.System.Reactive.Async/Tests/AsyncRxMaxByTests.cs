// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxMaxByTests(ExecutionShape shape, DelegateForm delegates)
    : MaxByTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxMaxByTests_SynchronousCompletion()
    : AsyncRxMaxByTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxMaxByTests_ForcedYield()
    : AsyncRxMaxByTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxMaxByTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxMaxByTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxMaxByTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxMaxByTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
