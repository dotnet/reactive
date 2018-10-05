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
    public class LatestTest : ReactiveTest
    {

        [Fact]
        public void Latest_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Latest(default(IObservable<int>)));
        }

        [Fact]
        public void Latest1()
        {
            var disposed = false;
            var evt = new AutoResetEvent(false);
            var src = Observable.Create<int>(obs =>
            {
                Task.Run(() =>
                {
                    evt.WaitOne();
                    obs.OnNext(1);
                    evt.WaitOne();
                    obs.OnNext(2);
                    evt.WaitOne();
                    obs.OnCompleted();
                });

                return () => { disposed = true; };
            });

            var res = src.Latest().GetEnumerator();

            Task.Run(async () =>
            {
                await Task.Delay(250);
                evt.Set();
            });

            Assert.True(res.MoveNext());
            Assert.Equal(1, res.Current);

            evt.Set();
            Assert.True(res.MoveNext());
            Assert.Equal(2, ((IEnumerator)res).Current);

            evt.Set();
            Assert.False(res.MoveNext());

            ReactiveAssert.Throws<NotSupportedException>(() => res.Reset());

            res.Dispose();
            //ReactiveAssert.Throws<ObjectDisposedException>(() => res.MoveNext());
            Assert.True(disposed);
        }

        [Fact]
        public void Latest2()
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

            var res = xs.Latest();

            var e1 = default(IEnumerator<int>);
            scheduler.ScheduleAbsolute(205, () =>
            {
                e1 = res.GetEnumerator();
            });

            var o1 = new List<int>();
            scheduler.ScheduleAbsolute(235, () =>
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
            scheduler.ScheduleAbsolute(265, () =>
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
                Subscribe(205, 285),
                Subscribe(255, 300)
            );

            o1.AssertEqual(3, 6);
            o2.AssertEqual(6, 7);
        }

        [Fact]
        public void Latest_Error()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var ex = new Exception();

            var evt = new AutoResetEvent(false);
            var src = Observable.Create<int>(obs =>
            {
                Task.Run(() =>
                {
                    evt.WaitOne();
                    obs.OnNext(1);
                    evt.WaitOne();
                    obs.OnError(ex);
                });

                return () => { };
            });

            var res = src.Latest().GetEnumerator();

            Task.Run(async () =>
            {
                await Task.Delay(250);
                evt.Set();
            });

            Assert.True(res.MoveNext());
            Assert.Equal(1, res.Current);

            evt.Set();

            ReactiveAssert.Throws(ex, () => res.MoveNext());
        }

    }
}
