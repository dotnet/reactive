// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared.Scenarios;

// Rx.NET marks its blocking operators obsolete, pointing at the async forms; these tests use
// them as the originals do under a project-wide suppression.
#pragma warning disable CS0618

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>AppendPrependTests</c> on Rx.NET, plus the two real-time originals.
/// </summary>
/// <remarks>
/// <c>AppendPrepend_SchedulerRecursive</c> prepends on an <c>EventLoopScheduler</c> and
/// <c>AppendPrepend_SchedulerLongRunning</c> on <c>NewThreadScheduler</c>, both of which only
/// Rx.NET has, and block on <c>First</c>; as the originals but for MSTest's assertions and the
/// static scheduler named in full, with the originals' <c>[DoNotParallelize]</c> (their comments
/// say they ran slow or hung under parallel execution).
/// </remarks>
[TestClass]
public sealed class RxAppendPrependTests : AppendPrependTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    [DoNotParallelize]
    public void AppendPrepend_SchedulerRecursive()
    {
        using var scheduler = new EventLoopScheduler();

        var source = Observable.Range(1, 1000).DelaySubscription(TimeSpan.FromMilliseconds(100), global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance);

        var result = source.Prepend(0, scheduler).Prepend(-1, scheduler)
            .Timeout(TimeSpan.FromSeconds(5))
            .ToList().First();

        Assert.HasCount(1002, result);

        var j = 0;
        for (var i = -1; i <= 1000; i++)
        {
            Assert.AreEqual(i, result[j++]);
        }
    }

    [TestMethod]
    [DoNotParallelize]
    public void AppendPrepend_SchedulerLongRunning()
    {
        var scheduler = NewThreadScheduler.Default;
        Assert.IsTrue(scheduler is ISchedulerLongRunning, "Not a long-running scheduler!");

        var source = Observable.Range(1, 1000).DelaySubscription(TimeSpan.FromMilliseconds(100), global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance);

        var result = source.Prepend(0, scheduler).Prepend(-1, scheduler)
            .Timeout(TimeSpan.FromSeconds(5))
            .ToList().First();

        Assert.HasCount(1002, result);

        var j = 0;
        for (var i = -1; i <= 1000; i++)
        {
            Assert.AreEqual(i, result[j++]);
        }
    }
}
