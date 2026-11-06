// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxSumTests(ExecutionShape shape, DelegateForm delegates)
    : SumTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxSumTests_SynchronousCompletion()
    : AsyncRxSumTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxSumTests_ForcedYield()
    : AsyncRxSumTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxSumTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxSumTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxSumTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxSumTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
