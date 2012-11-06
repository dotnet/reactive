// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class EventLoopSchedulerTest
    {
        [TestMethod]
        public void EventLoop_ArgumentChecking()
        {
            var el = new EventLoopScheduler();

            ReactiveAssert.Throws<ArgumentNullException>(() => new EventLoopScheduler(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => el.Schedule<int>(42, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => el.Schedule<int>(42, DateTimeOffset.Now, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => el.Schedule<int>(42, TimeSpan.Zero, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => el.SchedulePeriodic(42, TimeSpan.FromSeconds(1), default(Func<int, int>)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => el.SchedulePeriodic(42, TimeSpan.FromSeconds(-1), _ => _));
        }

        [TestMethod]
        public void EventLoop_Now()
        {
            var res = new EventLoopScheduler().Now - DateTime.Now;
            Assert.IsTrue(res.Seconds < 1);
        }

        [TestMethod]
        public void EventLoop_ScheduleAction()
        {
            var ran = false;
            var gate = new Semaphore(0, 1);
            var el = new EventLoopScheduler();
            el.Schedule(() => { ran = true;
                                  gate.Release(); });
            Assert.IsTrue(gate.WaitOne(TimeSpan.FromSeconds(2)));
            Assert.IsTrue(ran);
        }

        [TestMethod]
        public void EventLoop_DifferentThread()
        {
            var id = default(int);
            var gate = new Semaphore(0, 1);
            var el = new EventLoopScheduler();
            el.Schedule(() =>
            {
                id = Thread.CurrentThread.ManagedThreadId;
                gate.Release();
            });
            Assert.IsTrue(gate.WaitOne(TimeSpan.FromSeconds(2)));
            Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, id);
        }

        [TestMethod]
        public void EventLoop_ScheduleOrderedActions()
        {
            var results = new List<int>();
            var gate = new Semaphore(0, 1);
            var el = new EventLoopScheduler();
            el.Schedule(() => results.Add(0));
            el.Schedule(() =>
            {
                results.Add(1);
                gate.Release();
            });
            Assert.IsTrue(gate.WaitOne(TimeSpan.FromSeconds(2)));
            results.AssertEqual(0, 1);
        }

        [TestMethod]
        public void EventLoop_SchedulerDisposed()
        {
            var d = 0;
            var e = new ManualResetEvent(false);
            var f = new ManualResetEvent(false);

            var results = new List<int>();
            var el = new EventLoopScheduler();
            el.Schedule(() => results.Add(0));
            el.Schedule(() =>
            {
                el.Dispose();
                e.Set();

                results.Add(1);

                try
                {
                    el.Schedule(() => { throw new Exception("Should be disposed!"); });
                    f.Set();
                }
                catch (ObjectDisposedException)
                {
                    // BREAKING CHANGE v2 > v1.x - New exception behavior.
                    Interlocked.Increment(ref d);
                    f.Set();
                }
            });

            e.WaitOne();

            try
            {
                el.Schedule(() => results.Add(2));
            }
            catch (ObjectDisposedException)
            {
                // BREAKING CHANGE v2 > v1.x - New exception behavior.
                Interlocked.Increment(ref d);
            }

            f.WaitOne();

            results.AssertEqual(0, 1);

            Assert.AreEqual(2, d);
        }

        [TestMethod]
        public void EventLoop_ScheduleTimeOrderedActions()
        {
            var results = new List<int>();
            var gate = new Semaphore(0, 1);
            var el = new EventLoopScheduler();
            el.Schedule(TimeSpan.FromMilliseconds(50), () => results.Add(1));
            el.Schedule(TimeSpan.FromMilliseconds(100), () =>
                        {
                            results.Add(0);
                            gate.Release();
                        });

            Assert.IsTrue(gate.WaitOne(TimeSpan.FromSeconds(2)));
            results.AssertEqual(1, 0);
        }

        [TestMethod]
        public void EventLoop_ScheduleOrderedAndTimedActions()
        {
            var results = new List<int>();
            var gate = new Semaphore(0, 1);
            var el = new EventLoopScheduler();
            el.Schedule(() => results.Add(1));
            el.Schedule(TimeSpan.FromMilliseconds(100), () =>
            {
                results.Add(0);
                gate.Release();
            });

            Assert.IsTrue(gate.WaitOne(TimeSpan.FromSeconds(2)));
            results.AssertEqual(1, 0);
        }

        [TestMethod]
        public void EventLoop_ScheduleTimeOrderedInFlightActions()
        {            
            var results = new List<int>();
            var gate = new Semaphore(0, 1);
            var el = new EventLoopScheduler();

            el.Schedule(TimeSpan.FromMilliseconds(100), () =>
                        {
                            results.Add(0);
                            el.Schedule(TimeSpan.FromMilliseconds(50), () => results.Add(1));
                            el.Schedule(TimeSpan.FromMilliseconds(100), ()=>
                            {
                                results.Add(2);
                                gate.Release();
                            });
                        });

            Assert.IsTrue(gate.WaitOne(TimeSpan.FromSeconds(2)));
            results.AssertEqual(0, 1, 2);
        }


        [TestMethod]
        public void EventLoop_ScheduleTimeAndOrderedInFlightActions()
        {
            var results = new List<int>();
            var gate = new Semaphore(0, 1);
            var el = new EventLoopScheduler();

            el.Schedule(TimeSpan.FromMilliseconds(100), () =>
            {
                results.Add(0);
                el.Schedule(() => results.Add(4));
                el.Schedule(TimeSpan.FromMilliseconds(50), () => results.Add(1));
                el.Schedule(TimeSpan.FromMilliseconds(100), () =>
                {
                    results.Add(2);
                    gate.Release();
                });
            });

            Assert.IsTrue(gate.WaitOne(TimeSpan.FromSeconds(2)));
            results.AssertEqual(0, 4, 1, 2);
        }       

        [TestMethod]
        public void EventLoop_ScheduleActionNested()
        {
            var ran = false;
            var el = new EventLoopScheduler();
            var gate = new Semaphore(0, 1);
            el.Schedule(() => el.Schedule(() => { ran = true;
                                                  gate.Release(); }));
            Assert.IsTrue(gate.WaitOne(TimeSpan.FromSeconds(2)));
            Assert.IsTrue(ran);
        }

#if !SILVERLIGHT
        [TestMethod]
        [Ignore]
        public void EventLoop_ScheduleActionDue()
        {
            var ran = false;
            var el = new EventLoopScheduler();
            var sw = new Stopwatch();
            var gate = new Semaphore(0, 1);
            sw.Start();
            el.Schedule(TimeSpan.FromSeconds(0.2), () => {
                                  ran = true; 
                                  sw.Stop();
                                  gate.Release();
                              });
            Assert.IsTrue(gate.WaitOne(TimeSpan.FromSeconds(2)));
            Assert.IsTrue(ran, "ran");
            Assert.IsTrue(sw.ElapsedMilliseconds > 180, "due " + sw.ElapsedMilliseconds);
        }

        [TestMethod]
        [Ignore]
        public void EventLoop_ScheduleActionDueNested()
        {
            var ran = false;
            var el = new EventLoopScheduler();
            var gate = new Semaphore(0, 1);

            var sw = new Stopwatch();
            sw.Start();
            el.Schedule(TimeSpan.FromSeconds(0.2), () =>
            {
                sw.Stop();
                sw.Start();
                el.Schedule(TimeSpan.FromSeconds(0.2), () =>
                {
                    sw.Stop();
                    ran = true;
                    gate.Release();
                });
            });

            Assert.IsTrue(gate.WaitOne(TimeSpan.FromSeconds(2)));
            Assert.IsTrue(ran, "ran");
            Assert.IsTrue(sw.ElapsedMilliseconds > 380, "due " + sw.ElapsedMilliseconds);
        }
#endif

#if !NO_PERF
#if !NO_STOPWATCH
        [TestMethod]
        public void Stopwatch()
        {
            StopwatchTest.Run(new EventLoopScheduler());
        }
#endif
#endif

#if !NO_CDS
        [TestMethod]
        public void EventLoop_Immediate()
        {
            var M = 1000;
            var N = 4;

            for (int i = 0; i < N; i++)
            {
                for (int j = 1; j <= M; j *= 10)
                {
                    using (var e = new EventLoopScheduler())
                    {
                        var cd = new CountdownEvent(j);

                        for (int k = 0; k < j; k++)
                            e.Schedule(() => cd.Signal());

                        if (!cd.Wait(10000))
                            Assert.Fail("j = " + j);
                    }
                }
            }
        }

        [TestMethod]
        public void EventLoop_TimeCollisions()
        {
            var M = 1000;
            var N = 4;

            for (int i = 0; i < N; i++)
            {
                for (int j = 1; j <= M; j *= 10) 
                {
                    using (var e = new EventLoopScheduler())
                    {
                        var cd = new CountdownEvent(j);

                        for (int k = 0; k < j; k++)
                            e.Schedule(TimeSpan.FromMilliseconds(100), () => cd.Signal());

                        if (!cd.Wait(10000))
                            Assert.Fail("j = " + j);
                    }
                }
            }
        }

        [TestMethod]
        public void EventLoop_Spread()
        {
            var M = 1000;
            var N = 4;

            for (int i = 0; i < N; i++)
            {
                for (int j = 1; j <= M; j *= 10)
                {
                    using (var e = new EventLoopScheduler())
                    {
                        var cd = new CountdownEvent(j);

                        for (int k = 0; k < j; k++)
                            e.Schedule(TimeSpan.FromMilliseconds(k), () => cd.Signal());

                        if (!cd.Wait(10000))
                            Assert.Fail("j = " + j);
                    }
                }
            }
        }
#endif

        [TestMethod]
        public void EventLoop_Periodic()
        {
            var n = 0;
            
            using (var s = new EventLoopScheduler())
            {
                var e = new ManualResetEvent(false);

                var d = s.SchedulePeriodic(TimeSpan.FromMilliseconds(25), () =>
                {
                    if (Interlocked.Increment(ref n) == 10)
                        e.Set();
                });
                
                if (!e.WaitOne(10000))
                    Assert.Fail();
                
                d.Dispose();
            }
        }
    }
}
