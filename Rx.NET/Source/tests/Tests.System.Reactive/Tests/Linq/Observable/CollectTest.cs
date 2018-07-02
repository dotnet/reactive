// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class CollectTest : ReactiveTest
    {

        [Fact]
        public void Collect_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Collect(default(IObservable<int>), () => 0, (x, y) => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Collect(someObservable, default(Func<int>), (x, y) => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Collect(someObservable, () => 0, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Collect(default(IObservable<int>), () => 0, (x, y) => x, x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Collect(someObservable, default(Func<int>), (x, y) => x, x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Collect(someObservable, () => 0, default, x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Collect(someObservable, () => 0, (x, y) => x, default));
        }

        [Fact]
        public void Collect_Regular1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnNext(600, 6),
                OnNext(700, 7),
                OnNext(800, 8),
                OnCompleted<int>(900)
            );

            var ys = xs.Collect(() => 0, (x, y) => x + y);
            var e = default(IEnumerator<int>);

            var res = new List<int>();

            var log = new Action(() =>
            {
                Assert.True(e.MoveNext());
                res.Add(e.Current);
            });

            scheduler.ScheduleAbsolute(250, () => { e = ys.GetEnumerator(); });
            scheduler.ScheduleAbsolute(270, log);
            scheduler.ScheduleAbsolute(310, log);
            scheduler.ScheduleAbsolute(450, log);
            scheduler.ScheduleAbsolute(470, log);
            scheduler.ScheduleAbsolute(750, log);
            scheduler.ScheduleAbsolute(850, log);
            scheduler.ScheduleAbsolute(950, log);
            scheduler.ScheduleAbsolute(980, () => Assert.False(e.MoveNext()));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(250, 900)
            );

            Assert.Equal(7, res.Count);
            Assert.Equal(res[0], new int[] { }.Sum());
            Assert.Equal(res[1], new int[] { 3 }.Sum());
            Assert.Equal(res[2], new int[] { 4 }.Sum());
            Assert.Equal(res[3], new int[] { }.Sum());
            Assert.Equal(res[4], new int[] { 5, 6, 7 }.Sum());
            Assert.Equal(res[5], new int[] { 8 }.Sum());
            Assert.Equal(res[6], new int[] { }.Sum());
        }

        [Fact]
        public void Collect_Regular2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnNext(600, 6),
                OnNext(700, 7),
                OnNext(800, 8),
                OnCompleted<int>(900)
            );

            var ys = xs.Collect(() => 0, (x, y) => x + y);
            var e = default(IEnumerator<int>);

            var res = new List<int>();

            var log = new Action(() =>
            {
                Assert.True(e.MoveNext());
                res.Add(e.Current);
            });

            scheduler.ScheduleAbsolute(250, () => { e = ys.GetEnumerator(); });
            scheduler.ScheduleAbsolute(550, log);
            scheduler.ScheduleAbsolute(950, log);
            scheduler.ScheduleAbsolute(980, () => Assert.False(e.MoveNext()));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(250, 900)
            );

            Assert.Equal(2, res.Count);
            Assert.Equal(res[0], new int[] { 3, 4, 5 }.Sum());
            Assert.Equal(res[1], new int[] { 6, 7, 8 }.Sum());
        }

        [Fact]
        public void Collect_InitialCollectorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnCompleted<int>(500)
            );

            var ex = new Exception();
            var ys = xs.Collect<int, int>(() => { throw ex; }, (x, y) => x + y);

            var ex_ = default(Exception);

            scheduler.ScheduleAbsolute(250, () =>
            {
                try
                {
                    ys.GetEnumerator();
                }
                catch (Exception err)
                {
                    ex_ = err;
                }
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
            );

            Assert.Same(ex_, ex);
        }

        [Fact]
        public void Collect_SecondCollectorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnCompleted<int>(500)
            );

            var ex = new Exception();
            var n = 0;
            var ys = xs.Collect(() =>
            {
                if (n++ == 0)
                {
                    return 0;
                }
                else
                {
                    throw ex;
                }
            }, (x, y) => x + y);
            var e = default(IEnumerator<int>);

            var ex_ = default(Exception);

            scheduler.ScheduleAbsolute(250, () => e = ys.GetEnumerator());
            scheduler.ScheduleAbsolute(350, () =>
            {
                try
                {
                    e.MoveNext();
                }
                catch (Exception err)
                {
                    ex_ = err;
                }
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(250, 350)
            );

            Assert.Same(ex_, ex);
        }

        [Fact]
        public void Collect_NewCollectorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnCompleted<int>(500)
            );

            var ex = new Exception();
            var ys = xs.Collect(() => 0, (x, y) => x + y, x => { throw ex; });
            var e = default(IEnumerator<int>);

            var ex_ = default(Exception);

            scheduler.ScheduleAbsolute(250, () => e = ys.GetEnumerator());
            scheduler.ScheduleAbsolute(350, () =>
            {
                try
                {
                    e.MoveNext();
                }
                catch (Exception err)
                {
                    ex_ = err;
                }
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(250, 350)
            );

            Assert.Same(ex_, ex);
        }

        [Fact]
        public void Collect_MergeThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnCompleted<int>(500)
            );

            var ex = new Exception();
            var ys = xs.Collect(() => 0, (x, y) => { throw ex; });
            var e = default(IEnumerator<int>);

            var ex_ = default(Exception);

            scheduler.ScheduleAbsolute(250, () => { e = ys.GetEnumerator(); });
            scheduler.ScheduleAbsolute(350, () =>
            {
                try
                {
                    e.MoveNext();
                }
                catch (Exception err)
                {
                    ex_ = err;
                }
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(250, 300)
            );

            Assert.Same(ex_, ex);
        }

    }
}
