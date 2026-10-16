// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>ObserveOn</c> scenarios on Rx.NET, plus three over its own schedulers.
/// </summary>
/// <remarks>
/// <c>ObserveOn_EventLoop_Long</c>, <c>ObserveOn_LongRunning_SameThread</c> and
/// <c>ObserveOn_LongRunning_DisableOptimizations</c> are the originals: they exercise
/// <c>EventLoopScheduler</c> and the task pool scheduler's <c>ISchedulerLongRunning</c> path,
/// with and without <c>DisableOptimizations</c>, none of which AsyncRx.NET has, so they are
/// written against Rx.NET directly.
/// </remarks>
[TestClass]
public sealed class RxObserveOnTests : ObserveOnTests
{
    private static readonly TimeSpan MaxWaitTime = TimeSpan.FromSeconds(10);

    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void ObserveOn_EventLoop_Long()
    {
        using var scheduler1 = new EventLoopScheduler();
        var n = 1_000_000;
        var cde = new CountdownEvent(1);

        Observable.Range(1, n).ObserveOn(scheduler1)
            .Subscribe(v => { }, () => cde.Signal());

        Assert.IsTrue(cde.Wait(MaxWaitTime), "Timeout!");
    }

    [TestMethod]
    public void ObserveOn_LongRunning_SameThread()
    {
        var scheduler = TaskPoolScheduler.Default;
        Assert.IsNotNull(scheduler.AsLongRunning());

        var n = 1_000_000;
        var threads = new HashSet<long>();
        var cde = new CountdownEvent(1);

        Observable.Range(1, n)
            .ObserveOn(scheduler)
            .Subscribe(
                v => threads.Add(Environment.CurrentManagedThreadId),
                e => cde.Signal(),
                () => cde.Signal()
            );

        Assert.IsTrue(cde.Wait(MaxWaitTime), "Timeout!");
        Assert.HasCount(1, threads);
    }

    [TestMethod]
    public void ObserveOn_LongRunning_DisableOptimizations()
    {
        var scheduler = TaskPoolScheduler.Default.DisableOptimizations();
        Assert.IsNull(scheduler.AsLongRunning());

        var n = 1_000_000;
        var threads = new HashSet<long>();
        var cde = new CountdownEvent(1);

        Observable.Range(1, n)
            .ObserveOn(scheduler)
            .Subscribe(
                v => threads.Add(Environment.CurrentManagedThreadId),
                e => cde.Signal(),
                () => cde.Signal()
            );

        Assert.IsTrue(cde.Wait(MaxWaitTime), "Timeout!");
        Assert.IsTrue(threads.Count >= 1);
    }
}
