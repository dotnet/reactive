// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxIfTests(ExecutionShape shape, DelegateForm delegates)
    : IfTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxIfTests_SynchronousCompletion()
    : AsyncRxIfTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxIfTests_ForcedYield()
    : AsyncRxIfTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxIfTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxIfTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxIfTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxIfTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
