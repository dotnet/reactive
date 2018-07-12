// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{

    public class VirtualSchedulerTest
    {
        private class VirtualSchedulerTestScheduler : VirtualTimeScheduler<string, char>
        {
            public VirtualSchedulerTestScheduler()
            {
            }

            public VirtualSchedulerTestScheduler(string initialClock, IComparer<string> comparer)
                : base(initialClock, comparer)
            {
            }

            protected override string Add(string absolute, char relative)
            {
                return (absolute ?? string.Empty) + relative;
            }

            protected override DateTimeOffset ToDateTimeOffset(string absolute)
            {
                return new DateTimeOffset((absolute ?? string.Empty).Length, TimeSpan.Zero);
            }

            protected override char ToRelative(TimeSpan timeSpan)
            {
                return (char)(timeSpan.Ticks % char.MaxValue);
            }
        }

        [Fact]
        public void Virtual_Now()
        {
            var res = new VirtualSchedulerTestScheduler().Now - DateTime.Now;
            Assert.True(res.Seconds < 1);
        }
#if !NO_THREAD
        [Fact]
        public void Virtual_ScheduleAction()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            var scheduler = new VirtualSchedulerTestScheduler();
            scheduler.Schedule(() => { Assert.Equal(id, Thread.CurrentThread.ManagedThreadId); ran = true; });
            scheduler.Start();
            Assert.True(ran);
        }
#endif

        [Fact]
        public void Virtual_ScheduleActionError()
        {
            var ex = new Exception();

            try
            {
                var scheduler = new VirtualSchedulerTestScheduler();
                scheduler.Schedule(() => { throw ex; });
                scheduler.Start();
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.Same(e, ex);
            }
        }

        [Fact]
        public void Virtual_InitialAndComparer_Now()
        {
            var s = new VirtualSchedulerTestScheduler("Bar", Comparer<string>.Default);
            Assert.Equal(3, s.Now.Ticks);
        }

        [Fact]
        public void Virtual_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new VirtualSchedulerTestScheduler("", null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new VirtualSchedulerTestScheduler().ScheduleRelative(0, 'a', null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new VirtualSchedulerTestScheduler().ScheduleAbsolute(0, "", null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new VirtualSchedulerTestScheduler().Schedule(0, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => new VirtualSchedulerTestScheduler().Schedule(0, TimeSpan.Zero, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => new VirtualSchedulerTestScheduler().Schedule(0, DateTimeOffset.UtcNow, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => VirtualTimeSchedulerExtensions.ScheduleAbsolute(default(VirtualSchedulerTestScheduler), "", () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => VirtualTimeSchedulerExtensions.ScheduleAbsolute(new VirtualSchedulerTestScheduler(), "", default));
            ReactiveAssert.Throws<ArgumentNullException>(() => VirtualTimeSchedulerExtensions.ScheduleRelative(default(VirtualSchedulerTestScheduler), 'a', () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => VirtualTimeSchedulerExtensions.ScheduleRelative(new VirtualSchedulerTestScheduler(), 'a', default));
        }

        [Fact]
        public void Historical_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new HistoricalScheduler(DateTime.Now, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => new HistoricalScheduler().ScheduleAbsolute(42, DateTime.Now, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => new HistoricalScheduler().ScheduleRelative(42, TimeSpan.FromSeconds(1), default));
        }

#if !NO_THREAD
        [Fact(Skip = "Ignored")]
        public void Virtual_ScheduleActionDue()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            var sw = new Stopwatch();
            sw.Start();
            var scheduler = new VirtualSchedulerTestScheduler();
            scheduler.Schedule(TimeSpan.FromSeconds(0.2), () => { sw.Stop(); Assert.Equal(id, Thread.CurrentThread.ManagedThreadId); ran = true; });
            scheduler.Start();
            Assert.True(ran, "ran");
            Assert.True(sw.ElapsedMilliseconds > 180, "due " + sw.ElapsedMilliseconds);
        }
#endif

        [Fact]
        [Trait("SkipCI", "true")]
        public void Virtual_ThreadSafety()
        {
            for (var i = 0; i < 10; i++)
            {
                var scheduler = new TestScheduler();
                var seq = Observable.Never<string>();
                var disposable = default(IDisposable);

                var sync = 2;

                Task.Run(() =>
                {
                    if (Interlocked.Decrement(ref sync) != 0)
                    {
                        while (Volatile.Read(ref sync) != 0)
                        {
                            ;
                        }
                    }

                    Task.Delay(10).Wait();

                    disposable = seq.Timeout(TimeSpan.FromSeconds(5), scheduler).Subscribe(s => { });
                });

                var watch = scheduler.StartStopwatch();
                try
                {
                    if (Interlocked.Decrement(ref sync) != 0)
                    {
                        while (Volatile.Read(ref sync) != 0)
                        {
                            ;
                        }
                    }

                    var d = default(IDisposable);
                    while (watch.Elapsed < TimeSpan.FromSeconds(100))
                    {
                        d = Volatile.Read(ref disposable);
                        scheduler.AdvanceBy(50);
                    }

                    if (d != null)
                    {
                        throw new Exception("Should have thrown!");
                    }
                }
                catch (TimeoutException)
                {
                }
                catch (Exception ex)
                {
                    Assert.True(false, string.Format("Virtual time {0}, exception {1}", watch.Elapsed, ex));
                }
                disposable?.Dispose();
            }
        }
    }
}
