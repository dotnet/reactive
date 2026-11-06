// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Subjects;

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

/// <summary>
/// The shared <c>ReplaySubjectTests</c> on AsyncRx.NET, plus the argument checks over its own
/// subjects.
/// </summary>
/// <remarks>
/// The three argument-checking originals over <c>SequentialReplayAsyncSubject</c>, each
/// constructor form as the original has it: a null observer, error or scheduler and a negative
/// buffer size or window are refused as the original expects, and zero is allowed.
/// </remarks>
public abstract class AsyncRxReplaySubjectTests(ExecutionShape shape) : ReplaySubjectTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape);

    [TestMethod]
    public async Task Subscribe_ArgumentChecking()
    {
        var scheduler = ImmediateAsyncScheduler.Instance;

        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => await new SequentialReplayAsyncSubject<int>().SubscribeAsync(null!));
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => await new SequentialReplayAsyncSubject<int>(1).SubscribeAsync(null!));
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => await new SequentialReplayAsyncSubject<int>(2).SubscribeAsync(null!));
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => await new SequentialReplayAsyncSubject<int>(scheduler)
                .SubscribeAsync(null!));
    }

    [TestMethod]
    public async Task OnError_ArgumentChecking()
    {
        var scheduler = ImmediateAsyncScheduler.Instance;

        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => await new SequentialReplayAsyncSubject<int>().OnErrorAsync(null!));
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => await new SequentialReplayAsyncSubject<int>(1).OnErrorAsync(null!));
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => await new SequentialReplayAsyncSubject<int>(2).OnErrorAsync(null!));
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => await new SequentialReplayAsyncSubject<int>(scheduler).OnErrorAsync(null!));
    }

    [TestMethod]
    public void Constructor_ArgumentChecking()
    {
        var scheduler = ImmediateAsyncScheduler.Instance;
        var negative = TimeSpan.FromTicks(-1);

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => new SequentialReplayAsyncSubject<int>(-1));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => new SequentialReplayAsyncSubject<int>(-1, scheduler));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => new SequentialReplayAsyncSubject<int>(-1, TimeSpan.Zero));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => new SequentialReplayAsyncSubject<int>(-1, TimeSpan.Zero, scheduler));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => new SequentialReplayAsyncSubject<int>(negative));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => new SequentialReplayAsyncSubject<int>(negative, scheduler));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => new SequentialReplayAsyncSubject<int>(0, negative));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => new SequentialReplayAsyncSubject<int>(0, negative, scheduler));

        Assert.ThrowsExactly<ArgumentNullException>(
            () => new SequentialReplayAsyncSubject<int>(null!));
        Assert.ThrowsExactly<ArgumentNullException>(
            () => new SequentialReplayAsyncSubject<int>(0, null!));
        Assert.ThrowsExactly<ArgumentNullException>(
            () => new SequentialReplayAsyncSubject<int>(TimeSpan.Zero, null!));
        Assert.ThrowsExactly<ArgumentNullException>(
            () => new SequentialReplayAsyncSubject<int>(0, TimeSpan.Zero, null!));

        // zero allowed
        _ = new SequentialReplayAsyncSubject<int>(0);
        _ = new SequentialReplayAsyncSubject<int>(TimeSpan.Zero);
        _ = new SequentialReplayAsyncSubject<int>(0, TimeSpan.Zero);
        _ = new SequentialReplayAsyncSubject<int>(0, scheduler);
        _ = new SequentialReplayAsyncSubject<int>(TimeSpan.Zero, scheduler);
        _ = new SequentialReplayAsyncSubject<int>(0, TimeSpan.Zero, scheduler);
    }
}

[TestClass]
public sealed class AsyncRxReplaySubjectTests_SynchronousCompletion() : AsyncRxReplaySubjectTests(ExecutionShape.SynchronousCompletion);

[TestClass]
public sealed class AsyncRxReplaySubjectTests_ForcedYield() : AsyncRxReplaySubjectTests(ExecutionShape.ForcedYield);
