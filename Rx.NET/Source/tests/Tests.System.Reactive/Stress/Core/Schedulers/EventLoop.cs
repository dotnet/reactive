// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if STRESS
using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ReactiveTests.Stress.Schedulers
{
    /// <summary>
    /// Test for <see href="https://rx.codeplex.com/workitem/37">work item #37</see>.
    /// </summary>
    public static class EventLoop
    {
        private static readonly FieldInfo semaphore = typeof(EventLoopScheduler).GetField("_evt", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void NoSemaphoreFullException()
        {
            var failed = new TaskCompletionSource<int>();

            using (var scheduler = new EventLoopScheduler())
            {
                Assert.Equal(0, scheduler.CurrentCount());

                var maxCount = Environment.ProcessorCount;

                using (Enumerable.Range(1, maxCount)
                    .Select(_ => scheduler.SchedulePeriodic(TimeSpan.Zero, () =>
                    {
                        var count = scheduler.CurrentCount();

                        if (count > maxCount)
                            failed.SetResult(count);
                    }))
                    .Aggregate(
                        new CompositeDisposable(),
                        (c, d) =>
                        {
                            c.Add(d);
                            return c;
                        }))
                {
                    if (failed.Task.Wait(TimeSpan.FromSeconds(10)))
                    {
                        Assert.Fail("Semaphore count is too high: {0}", failed.Task.Result);
                    }
                }
            }
        }

        private static int CurrentCount(this EventLoopScheduler scheduler)
        {
            return ((SemaphoreSlim)semaphore.GetValue(scheduler)).CurrentCount;
        }
    }
}
#endif