// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

/// <summary>The <c>While</c> runner, on the execution-shape axis only.</summary>
/// <remarks>
/// AsyncRx.NET's <c>While</c> with a <see cref="ValueTask"/> condition resubscribes from the
/// completion handler and grows the stack with each iteration of a synchronously completing
/// source, as the library's own comment says, so <c>LoopTest1</c> and <c>LoopTest2</c>'s claim
/// that every iteration runs at one stack depth fails under
/// <see cref="DelegateForm.Asynchronous"/>. That form is a deferred library item, and this runner
/// does not take it.
/// </remarks>
public abstract class AsyncRxWhileTests(ExecutionShape shape, DelegateForm delegates)
    : WhileTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape, delegates);
}

[TestClass]
public sealed class AsyncRxWhileTests_SynchronousCompletion()
    : AsyncRxWhileTests(ExecutionShape.SynchronousCompletion, DelegateForm.Synchronous);

[TestClass]
public sealed class AsyncRxWhileTests_ForcedYield()
    : AsyncRxWhileTests(ExecutionShape.ForcedYield, DelegateForm.Synchronous);
