// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class HistoricalSchedulerTest
    {
        public DateTimeOffset Time(int i)
        {
            return new DateTimeOffset(1979, 10, 31, 4, 30, 15, TimeSpan.Zero).AddDays(i);
        }

        [TestMethod]
        public void Ctor()
        {
            var s = new HistoricalScheduler();

            Assert.AreEqual(DateTimeOffset.MinValue, s.Clock);
            Assert.AreEqual(DateTimeOffset.MinValue, s.Now);
            Assert.AreEqual(false, s.IsEnabled);
        }

        [TestMethod]
        public void Start_Stop()
        {
            var s = new HistoricalScheduler();

            var list = new List<Timestamped<int>>();

            var ts = TimeSpan.FromHours(1);

            s.Schedule(Time(0), () => list.Add(new Timestamped<int>(1, s.Now)));
            s.Schedule(Time(1), () => list.Add(new Timestamped<int>(2, s.Now)));
            s.Schedule(Time(2), () => s.Stop());
            s.Schedule(Time(3), () => list.Add(new Timestamped<int>(3, s.Now)));
            s.Schedule(Time(4), () => s.Stop());
            s.Schedule(Time(5), () => s.Start());
            s.Schedule(Time(6), () => list.Add(new Timestamped<int>(4, s.Now)));

            s.Start();

            Assert.AreEqual(Time(2), s.Now);
            Assert.AreEqual(Time(2), s.Clock);

            s.Start();

            Assert.AreEqual(Time(4), s.Now);
            Assert.AreEqual(Time(4), s.Clock);

            s.Start();

            Assert.AreEqual(Time(6), s.Now);
            Assert.AreEqual(Time(6), s.Clock);

            s.Start();

            Assert.AreEqual(Time(6), s.Now);
            Assert.AreEqual(Time(6), s.Clock);

            list.AssertEqual(
                new Timestamped<int>(1, Time(0)),
                new Timestamped<int>(2, Time(1)),
                new Timestamped<int>(3, Time(3)),
                new Timestamped<int>(4, Time(6))
            );
        }

        [TestMethod]
        public void Order()
        {
            var s = new HistoricalScheduler();

            var list = new List<Timestamped<int>>();

            s.Schedule(Time(2), () => list.Add(new Timestamped<int>(2, s.Now)));

            s.Schedule(Time(3), () => list.Add(new Timestamped<int>(3, s.Now)));

            s.Schedule(Time(1), () => list.Add(new Timestamped<int>(0, s.Now)));
            s.Schedule(Time(1), () => list.Add(new Timestamped<int>(1, s.Now)));

            s.Start();

            list.AssertEqual(
                new Timestamped<int>(0, Time(1)),
                new Timestamped<int>(1, Time(1)),
                new Timestamped<int>(2, Time(2)),
                new Timestamped<int>(3, Time(3))
            );
        }

        [TestMethod]
        public void Cancellation()
        {
            var s = new HistoricalScheduler();

            var list = new List<Timestamped<int>>();

            var d = s.Schedule(Time(2), () => list.Add(new Timestamped<int>(2, s.Now)));

            s.Schedule(Time(1), () =>
                {
                    list.Add(new Timestamped<int>(0, s.Now));
                    d.Dispose();
                });

            s.Start();

            list.AssertEqual(
                new Timestamped<int>(0, Time(1))
            );
        }

        [TestMethod]
        public void AdvanceTo_ArgumentChecking()
        {
            var now = DateTimeOffset.Now;

            var s = new HistoricalScheduler(now);

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => s.AdvanceTo(now.Subtract(TimeSpan.FromSeconds(1))));
        }

        [TestMethod]
        public void AdvanceTo()
        {
            var s = new HistoricalScheduler();

            var list = new List<Timestamped<int>>();

            s.Schedule(Time(0), () => list.Add(new Timestamped<int>(0, s.Now)));
            s.Schedule(Time(1), () => list.Add(new Timestamped<int>(1, s.Now)));
            s.Schedule(Time(2), () => list.Add(new Timestamped<int>(2, s.Now)));
            s.Schedule(Time(10), () => list.Add(new Timestamped<int>(10, s.Now)));
            s.Schedule(Time(11), () => list.Add(new Timestamped<int>(11, s.Now)));

            s.AdvanceTo(Time(8));

            Assert.AreEqual(Time(8), s.Now);
            Assert.AreEqual(Time(8), s.Clock);

            list.AssertEqual(
                new Timestamped<int>(0, Time(0)),
                new Timestamped<int>(1, Time(1)),
                new Timestamped<int>(2, Time(2))
            );

            s.AdvanceTo(Time(8));

            Assert.AreEqual(Time(8), s.Now);
            Assert.AreEqual(Time(8), s.Clock);

            list.AssertEqual(
                new Timestamped<int>(0, Time(0)),
                new Timestamped<int>(1, Time(1)),
                new Timestamped<int>(2, Time(2))
            );

            s.Schedule(Time(7), () => list.Add(new Timestamped<int>(7, s.Now)));
            s.Schedule(Time(8), () => list.Add(new Timestamped<int>(8, s.Now)));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => s.AdvanceTo(Time(4)));

            Assert.AreEqual(Time(8), s.Now);
            Assert.AreEqual(Time(8), s.Clock);

            list.AssertEqual(
                new Timestamped<int>(0, Time(0)),
                new Timestamped<int>(1, Time(1)),
                new Timestamped<int>(2, Time(2))
            );

            s.AdvanceTo(Time(10));

            Assert.AreEqual(Time(10), s.Now);
            Assert.AreEqual(Time(10), s.Clock);

            list.AssertEqual(
                new Timestamped<int>(0, Time(0)),
                new Timestamped<int>(1, Time(1)),
                new Timestamped<int>(2, Time(2)),
                new Timestamped<int>(7, Time(8)),
                new Timestamped<int>(8, Time(8)),
                new Timestamped<int>(10, Time(10))
            );

            s.AdvanceTo(Time(100));

            Assert.AreEqual(Time(100), s.Now);
            Assert.AreEqual(Time(100), s.Clock);

            list.AssertEqual(
                new Timestamped<int>(0, Time(0)),
                new Timestamped<int>(1, Time(1)),
                new Timestamped<int>(2, Time(2)),
                new Timestamped<int>(7, Time(8)),
                new Timestamped<int>(8, Time(8)),
                new Timestamped<int>(10, Time(10)),
                new Timestamped<int>(11, Time(11))
            );
        }

        [TestMethod]
        public void AdvanceBy_ArgumentChecking()
        {
            var s = new HistoricalScheduler();

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => s.AdvanceBy(TimeSpan.FromSeconds(-1)));
        }

        [TestMethod]
        public void AdvanceBy()
        {
            var s = new HistoricalScheduler();

            var list = new List<Timestamped<int>>();

            s.Schedule(Time(0), () => list.Add(new Timestamped<int>(0, s.Now)));
            s.Schedule(Time(1), () => list.Add(new Timestamped<int>(1, s.Now)));
            s.Schedule(Time(2), () => list.Add(new Timestamped<int>(2, s.Now)));
            s.Schedule(Time(10), () => list.Add(new Timestamped<int>(10, s.Now)));
            s.Schedule(Time(11), () => list.Add(new Timestamped<int>(11, s.Now)));

            s.AdvanceBy(Time(8) - s.Now);

            Assert.AreEqual(Time(8), s.Now);
            Assert.AreEqual(Time(8), s.Clock);

            list.AssertEqual(
                new Timestamped<int>(0, Time(0)),
                new Timestamped<int>(1, Time(1)),
                new Timestamped<int>(2, Time(2))
            );

            s.Schedule(Time(7), () => list.Add(new Timestamped<int>(7, s.Now)));
            s.Schedule(Time(8), () => list.Add(new Timestamped<int>(8, s.Now)));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => s.AdvanceBy(TimeSpan.FromDays(-4)));

            Assert.AreEqual(Time(8), s.Now);
            Assert.AreEqual(Time(8), s.Clock);

            list.AssertEqual(
                new Timestamped<int>(0, Time(0)),
                new Timestamped<int>(1, Time(1)),
                new Timestamped<int>(2, Time(2))
            );

            s.AdvanceBy(TimeSpan.Zero);

            Assert.AreEqual(Time(8), s.Now);
            Assert.AreEqual(Time(8), s.Clock);

            list.AssertEqual(
                new Timestamped<int>(0, Time(0)),
                new Timestamped<int>(1, Time(1)),
                new Timestamped<int>(2, Time(2))
            );

            s.AdvanceBy(TimeSpan.FromDays(2));

            Assert.AreEqual(Time(10), s.Now);
            Assert.AreEqual(Time(10), s.Clock);

            list.AssertEqual(
                new Timestamped<int>(0, Time(0)),
                new Timestamped<int>(1, Time(1)),
                new Timestamped<int>(2, Time(2)),
                new Timestamped<int>(7, Time(8)),
                new Timestamped<int>(8, Time(8)),
                new Timestamped<int>(10, Time(10))
            );

            s.AdvanceBy(TimeSpan.FromDays(90));

            Assert.AreEqual(Time(100), s.Now);
            Assert.AreEqual(Time(100), s.Clock);

            list.AssertEqual(
                new Timestamped<int>(0, Time(0)),
                new Timestamped<int>(1, Time(1)),
                new Timestamped<int>(2, Time(2)),
                new Timestamped<int>(7, Time(8)),
                new Timestamped<int>(8, Time(8)),
                new Timestamped<int>(10, Time(10)),
                new Timestamped<int>(11, Time(11))
            );
        }

        [TestMethod]
        public void IsEnabled()
        {
            var s = new HistoricalScheduler();

            Assert.AreEqual(false, s.IsEnabled);

            s.Schedule(() =>
            {
                Assert.AreEqual(true, s.IsEnabled);
                s.Stop();
                Assert.AreEqual(false, s.IsEnabled);
            });

            Assert.AreEqual(false, s.IsEnabled);

            s.Start();

            Assert.AreEqual(false, s.IsEnabled);
        }

        [TestMethod]
        public void No_Nested_AdvanceBy()
        {
            var s = new HistoricalScheduler();

            s.Schedule(() => s.AdvanceBy(TimeSpan.FromSeconds(1)));

            ReactiveAssert.Throws<InvalidOperationException>(() => s.Start());
        }

        [TestMethod]
        public void No_Nested_AdvanceTo()
        {
            var s = new HistoricalScheduler();

            s.Schedule(() => s.AdvanceTo(DateTimeOffset.Now.AddDays(1)));

            ReactiveAssert.Throws<InvalidOperationException>(() => s.Start());
        }

        [TestMethod]
        public void Sleep_ArgumentChecking()
        {
            var s = new HistoricalScheduler(DateTimeOffset.Now);

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => s.Sleep(TimeSpan.FromSeconds(-1)));
        }

        [TestMethod]
        public void Sleep1()
        {
            var now = new DateTimeOffset(1983, 2, 11, 12, 0, 0, TimeSpan.Zero);

            var s = new HistoricalScheduler(now);

            s.Sleep(TimeSpan.FromDays(1));

            Assert.AreEqual(now + TimeSpan.FromDays(1), s.Clock);
        }

        [TestMethod]
        public void Sleep2()
        {
            var s = new HistoricalScheduler();

            var n = 0;

            s.Schedule(s.Now.AddMinutes(1), rec =>
            {
                s.Sleep(TimeSpan.FromMinutes(3));
                n++;

                rec(s.Now.AddMinutes(1));
            });

            s.AdvanceTo(s.Now + TimeSpan.FromMinutes(5));

            Assert.AreEqual(2, n);
        }

        [TestMethod]
        public void WithComparer_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new HistoricalScheduler(DateTimeOffset.Now, null));
        }

        [TestMethod]
        public void WithComparer()
        {
            var now = DateTimeOffset.Now;

            var s = new HistoricalScheduler(now, new ReverseComparer<DateTimeOffset>(Comparer<DateTimeOffset>.Default));

            var res = new List<int>();

            s.Schedule(now - TimeSpan.FromSeconds(1), () => res.Add(1));
            s.Schedule(now - TimeSpan.FromSeconds(2), () => res.Add(2));

            s.Start();

            Assert.IsTrue(new[] { 1, 2 }.SequenceEqual(res));
        }

        class ReverseComparer<T> : IComparer<T>
        {
            private readonly IComparer<T> _comparer;

            public ReverseComparer(IComparer<T> comparer)
            {
                _comparer = comparer;
            }

            public int Compare(T x, T y)
            {
                return -_comparer.Compare(x, y);
            }
        }
    }
}
