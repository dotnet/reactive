// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxCaseTests(ExecutionShape shape, DelegateForm delegates)
    : CaseTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxCaseTests_SynchronousCompletion()
    : AsyncRxCaseTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxCaseTests_ForcedYield()
    : AsyncRxCaseTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxCaseTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxCaseTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxCaseTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxCaseTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
