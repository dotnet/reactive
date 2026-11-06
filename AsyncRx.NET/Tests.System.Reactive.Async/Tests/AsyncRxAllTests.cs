// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxAllTests(ExecutionShape shape, DelegateForm delegates)
    : AllTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxAllTests_SynchronousCompletion()
    : AsyncRxAllTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxAllTests_ForcedYield()
    : AsyncRxAllTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxAllTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxAllTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxAllTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxAllTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
