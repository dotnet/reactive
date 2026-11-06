// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxPublishTests(ExecutionShape shape, DelegateForm delegates)
    : PublishTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxPublishTests_SynchronousCompletion()
    : AsyncRxPublishTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxPublishTests_ForcedYield()
    : AsyncRxPublishTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxPublishTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxPublishTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxPublishTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxPublishTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
