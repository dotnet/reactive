// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>Sample</c> scenarios on Rx.NET, plus its <c>DisableOptimizations</c> test.
/// </summary>
/// <remarks>
/// <c>Sample_DefaultScheduler_PeriodicDisabled</c> is the original: it samples on a scheduler
/// stripped of <c>ISchedulerPeriodic</c>, so that Rx.NET's timed <c>Sample</c> takes its
/// recursive-scheduling fallback. AsyncRx.NET has one way of scheduling a periodic timer and no
/// <c>DisableOptimizations</c>, so the test is Rx.NET's own. It awaits the result through
/// <c>ToTask</c> where the original blocks on <c>ToEnumerable</c>.
/// </remarks>
[TestClass]
public sealed class RxSampleTests : SampleTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public async Task Sample_DefaultScheduler_PeriodicDisabled()
    {
        var res = await Observable.Return(42)
            .Sample(
                TimeSpan.FromMilliseconds(1),
                global::System.Reactive.Concurrency.Scheduler.Default.DisableOptimizations())
            .ToTask();

        Assert.AreEqual(42, res);
    }
}
