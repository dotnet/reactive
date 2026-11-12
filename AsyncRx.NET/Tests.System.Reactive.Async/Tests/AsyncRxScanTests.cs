// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

public abstract class AsyncRxScanTests(ExecutionShape shape, DelegateForm delegates)
    : ScanTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxScanTests_SynchronousCompletion()
    : AsyncRxScanTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxScanTests_ForcedYield()
    : AsyncRxScanTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxScanTests_SynchronousCompletion_AsynchronousDelegates()
    : AsyncRxScanTests(ExecutionShape.SynchronousCompletion, DelegateForm.Asynchronous);

[TestClass]
public sealed class AsyncRxScanTests_ForcedYield_AsynchronousDelegates()
    : AsyncRxScanTests(ExecutionShape.ForcedYield, DelegateForm.Asynchronous);
