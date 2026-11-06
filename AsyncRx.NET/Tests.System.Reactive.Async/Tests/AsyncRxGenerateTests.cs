// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxGenerateTests(ExecutionShape shape, DelegateForm delegates)
    : GenerateTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxGenerateTests_SynchronousCompletion()
    : AsyncRxGenerateTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxGenerateTests_ForcedYield()
    : AsyncRxGenerateTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxGenerateTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxGenerateTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxGenerateTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxGenerateTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
