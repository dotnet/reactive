// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if STRESS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Concurrency;
using System.Threading;
using System.Reactive.Linq;

namespace ReactiveTests.Stress.Linq
{
    public class Replay
    {
        /// <summary>
        /// Tests the Replay operator with different schedulers, supporting ISchedulerLongRunning and otherwise.
        /// Stresses the ScheduledObserver implementation with its counting logic.
        /// </summary>
        public static void DifferentSchedulers()
        {
            while (true)
            {
                for (int i = 100; i <= 10000; i *= 10)
                {
                    foreach (var s in new IScheduler[] { Scheduler.Default, TaskPoolScheduler.Default, ThreadPoolScheduler.Instance })
                    {
                        foreach (var b in new[] { true, false })
                        {
                            var t = b ? s : s.DisableOptimizations();

                            var e = new ManualResetEvent(false);
                            var xs = Observable.Range(0, i, TaskPoolScheduler.Default.DisableOptimizations()).Do(_ => { }, () => e.Set());

                            var ys = xs.Replay(t);

                            var f = new ManualResetEvent(false);
                            var r = new List<int>();
                            ys.Subscribe(r.Add, () => f.Set());

                            ys.Connect();
                            
                            e.WaitOne();
                            f.WaitOne();

                            if (!r.SequenceEqual(Enumerable.Range(0, i)))
                                throw new Exception();

                            Console.Write(".");
                        }
                    }
                }
            }
        }
    }
}
#endif