// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxZipTests(ExecutionShape shape, DelegateForm delegates)
    : ZipTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxZipTests_SynchronousCompletion()
    : AsyncRxZipTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxZipTests_ForcedYield()
    : AsyncRxZipTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxZipTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxZipTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxZipTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxZipTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
