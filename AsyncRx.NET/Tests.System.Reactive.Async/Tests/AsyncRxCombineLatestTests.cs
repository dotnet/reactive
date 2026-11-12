// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxCombineLatestTests(ExecutionShape shape, DelegateForm delegates)
    : CombineLatestTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxCombineLatestTests_SynchronousCompletion()
    : AsyncRxCombineLatestTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxCombineLatestTests_ForcedYield()
    : AsyncRxCombineLatestTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxCombineLatestTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxCombineLatestTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxCombineLatestTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxCombineLatestTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
