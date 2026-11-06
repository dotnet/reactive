// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxToDictionaryTests(ExecutionShape shape, DelegateForm delegates)
    : ToDictionaryTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxToDictionaryTests_SynchronousCompletion()
    : AsyncRxToDictionaryTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxToDictionaryTests_ForcedYield()
    : AsyncRxToDictionaryTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxToDictionaryTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxToDictionaryTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxToDictionaryTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxToDictionaryTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
