// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class NextTest : ReactiveTest
    {

        [Fact]
        public void Next_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Next(default(IObservable<int>)));
        }

        [Fact]
        public void Next1()
        {
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

                return () => { };
            });

            var res = src.Next().GetEnumerator();

            void release() => Task.Run(async () =>
            {
                await Task.Delay(250);
                evt.Set();
            });

            release();
            Assert.True(res.MoveNext());
            Assert.Equal(1, res.Current);

            release();
            Assert.True(res.MoveNext());
            Assert.Equal(2, res.Current);

            release();
            Assert.False(res.MoveNext());
        }


        [Fact]
        public void Next2()
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

            var res = xs.Next();

            var e1 = default(IEnumerator<int>);
            scheduler.ScheduleAbsolute(200, () =>
            {
                e1 = res.GetEnumerator();
            });

            scheduler.ScheduleAbsolute(285, () => e1.Dispose());

            var e2 = default(IEnumerator);
            scheduler.ScheduleAbsolute(255, () =>
            {
                e2 = ((IEnumerable)res).GetEnumerator();
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 285),
                Subscribe(255, 300)
            );
        }

        [Fact]
        public void Next_DoesNotBlock()
        {
            var evt = new ManualResetEvent(false);

            var xs = Observable.Empty<int>().Do(_ => { }, () => evt.Set());

            var e = xs.Next().GetEnumerator();

            evt.WaitOne();

            Assert.False(e.MoveNext());
        }

        [Fact]
        public void Next_SomeResults()
        {
            var xs = Observable.Range(0, 100, Scheduler.Default);

            var res = xs.Next().ToList();

            Assert.True(res.All(x => x < 100));
            Assert.True(res.Count == res.Distinct().Count());
        }

#if !NO_THREAD
        [Fact]
        public void Next_Error()
        {
            var ex = new Exception();

            var evt = new AutoResetEvent(false);
            var src = Observable.Create<int>(obs =>
            {
                new Thread(() =>
                {
                    evt.WaitOne();
                    obs.OnNext(1);
                    evt.WaitOne();
                    obs.OnError(ex);
                }).Start();

                return () => { };
            });

            var res = src.Next().GetEnumerator();

            void release() => new Thread(() =>
            {
                Thread.Sleep(250);
                evt.Set();
            }).Start();

            release();
            Assert.True(res.MoveNext());
            Assert.Equal(1, res.Current);

            release();

            ReactiveAssert.Throws(ex, () => res.MoveNext());
        }
#endif

    }
}
