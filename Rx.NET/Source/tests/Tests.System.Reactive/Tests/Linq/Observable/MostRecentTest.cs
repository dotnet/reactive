// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class MostRecentTest : ReactiveTest
    {

        [Fact]
        public void MostRecent_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MostRecent(default, 1));
        }

        [Fact]
        public void MostRecent1()
        {
            var evt = new AutoResetEvent(false);
            var nxt = new AutoResetEvent(false);
            var src = Observable.Create<int>(obs =>
            {
                Task.Run(() =>
                {
                    evt.WaitOne();
                    obs.OnNext(1);
                    nxt.Set();
                    evt.WaitOne();
                    obs.OnNext(2);
                    nxt.Set();
                    evt.WaitOne();
                    obs.OnCompleted();
                    nxt.Set();
                });

                return () => { };
            });

            var res = src.MostRecent(42).GetEnumerator();

            Assert.True(res.MoveNext());
            Assert.Equal(42, res.Current);
            Assert.True(res.MoveNext());
            Assert.Equal(42, res.Current);

            for (var i = 1; i <= 2; i++)
            {
                evt.Set();
                nxt.WaitOne();
                Assert.True(res.MoveNext());
                Assert.Equal(i, res.Current);
                Assert.True(res.MoveNext());
                Assert.Equal(i, res.Current);
            }

            evt.Set();
            nxt.WaitOne();
            Assert.False(res.MoveNext());
        }

        [Fact]
        public void MostRecent2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnNext(270, 7),
                OnNext(280, 8),
                OnNext(290, 9),
                OnCompleted<int>(300)
            );

            var res = xs.MostRecent(0);

            var e1 = default(IEnumerator<int>);
            scheduler.ScheduleAbsolute(200, () =>
            {
                e1 = res.GetEnumerator();
            });

            var o1 = new List<int>();
            scheduler.ScheduleAbsolute(205, () =>
            {
                Assert.True(e1.MoveNext());
                o1.Add(e1.Current);
            });
            scheduler.ScheduleAbsolute(232, () =>
            {
                Assert.True(e1.MoveNext());
                o1.Add(e1.Current);
            });
            scheduler.ScheduleAbsolute(234, () =>
            {
                Assert.True(e1.MoveNext());
                o1.Add(e1.Current);
            });
            scheduler.ScheduleAbsolute(265, () =>
            {
                Assert.True(e1.MoveNext());
                o1.Add(e1.Current);
            });

            scheduler.ScheduleAbsolute(285, () => e1.Dispose());

            var e2 = default(IEnumerator);
            scheduler.ScheduleAbsolute(255, () =>
            {
                e2 = ((IEnumerable)res).GetEnumerator();
            });

            var o2 = new List<int>();
            scheduler.ScheduleAbsolute(258, () =>
            {
                Assert.True(e2.MoveNext());
                o2.Add((int)e2.Current);
            });
            scheduler.ScheduleAbsolute(262, () =>
            {
                Assert.True(e2.MoveNext());
                o2.Add((int)e2.Current);
            });
            scheduler.ScheduleAbsolute(264, () =>
            {
                Assert.True(e2.MoveNext());
                o2.Add((int)e2.Current);
            });
            scheduler.ScheduleAbsolute(275, () =>
            {
                Assert.True(e2.MoveNext());
                o2.Add((int)e2.Current);
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 285),
                Subscribe(255, 300)
            );

            o1.AssertEqual(0, 3, 3, 6);
            o2.AssertEqual(0, 6, 6, 7);
        }

        [Fact]
        public void MostRecent_Error()
        {
            var ex = new Exception();

            var evt = new AutoResetEvent(false);
            var nxt = new AutoResetEvent(false);
            var src = Observable.Create<int>(obs =>
            {
                Task.Run(() =>
                {
                    evt.WaitOne();
                    obs.OnNext(1);
                    nxt.Set();
                    evt.WaitOne();
                    obs.OnError(ex);
                    nxt.Set();
                });

                return () => { };
            });

            var res = src.MostRecent(42).GetEnumerator();

            Assert.True(res.MoveNext());
            Assert.Equal(42, res.Current);
            Assert.True(res.MoveNext());
            Assert.Equal(42, res.Current);

            evt.Set();
            nxt.WaitOne();
            Assert.True(res.MoveNext());
            Assert.Equal(1, res.Current);
            Assert.True(res.MoveNext());
            Assert.Equal(1, res.Current);

            evt.Set();
            nxt.WaitOne();

            ReactiveAssert.Throws(ex, () => res.MoveNext());
        }

    }
}
