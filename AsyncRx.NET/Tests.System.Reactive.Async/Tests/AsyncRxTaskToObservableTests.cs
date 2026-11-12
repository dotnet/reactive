// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

/// <summary>
/// The shared <c>TaskToObservableTests</c> on AsyncRx.NET, plus the argument checks over its own
/// task conversion.
/// </summary>
/// <remarks>
/// The two argument checks over <c>TaskAsyncObservableExtensions</c>, the equivalents of the
/// originals over <c>TaskObservableExtensions</c> (a static class the code-generated stratum does
/// not cover): a null task, a null scheduler and a null observer are refused. The subscribe
/// refusal is awaited, so a check that surfaces as a faulted task and one thrown synchronously
/// both pass.
/// </remarks>
public abstract class AsyncRxTaskToObservableTests(ExecutionShape shape) : TaskToObservableTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape);

    private readonly Task<int> _doneTask = Task.FromResult(42);

    [TestMethod]
    public async Task TaskToObservable_NonVoid_ArgumentChecking()
    {
        var s = ImmediateAsyncScheduler.Instance;

        Assert.ThrowsExactly<ArgumentNullException>(
            () => TaskAsyncObservableExtensions.ToAsyncObservable((Task<int>)null!));

        Assert.ThrowsExactly<ArgumentNullException>(
            () => TaskAsyncObservableExtensions.ToAsyncObservable((Task<int>)null!, s));
        Assert.ThrowsExactly<ArgumentNullException>(
            () => TaskAsyncObservableExtensions.ToAsyncObservable(_doneTask, null!));

        var tcs = new TaskCompletionSource<int>();
        var task = tcs.Task;
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => await task.ToAsyncObservable().SubscribeAsync(null!));
    }

    [TestMethod]
    public async Task TaskToObservable_Void_ArgumentChecking()
    {
        var s = ImmediateAsyncScheduler.Instance;

        Assert.ThrowsExactly<ArgumentNullException>(
            () => TaskAsyncObservableExtensions.ToAsyncObservable(null!));

        Assert.ThrowsExactly<ArgumentNullException>(
            () => TaskAsyncObservableExtensions.ToAsyncObservable(null!, s));
        Assert.ThrowsExactly<ArgumentNullException>(
            () => TaskAsyncObservableExtensions.ToAsyncObservable((Task)_doneTask, null!));

        var tcs = new TaskCompletionSource<int>();
        Task task = tcs.Task;
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => await task.ToAsyncObservable().SubscribeAsync(null!));
    }
}

[TestClass]
public sealed class AsyncRxTaskToObservableTests_SynchronousCompletion() : AsyncRxTaskToObservableTests(ExecutionShape.SynchronousCompletion);

[TestClass]
public sealed class AsyncRxTaskToObservableTests_ForcedYield() : AsyncRxTaskToObservableTests(ExecutionShape.ForcedYield);
