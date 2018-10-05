// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class GetEnumeratorTest : ReactiveTest
    {

        [Fact]
        public void GetEnumerator_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GetEnumerator(default(IObservable<int>)));
        }

        [Fact]
        public void GetEnumerator_Regular1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 2),
                OnNext(20, 3),
                OnNext(30, 5),
                OnNext(40, 7),
                OnCompleted<int>(50)
            );

            var res = default(IEnumerator<int>);

            scheduler.ScheduleAbsolute(default(object), 100, (self, _) => { res = xs.GetEnumerator(); return Disposable.Empty; });

            var hasNext = new List<bool>();
            var vals = new List<Tuple<long, int>>();
            for (long i = 200; i <= 250; i += 10)
            {
                var t = i;
                scheduler.ScheduleAbsolute(default(object), t, (self, _) =>
                {
                    var b = res.MoveNext();
                    hasNext.Add(b);
                    if (b)
                    {
                        vals.Add(new Tuple<long, int>(scheduler.Clock, res.Current));
                    }

                    return Disposable.Empty;
                });
            }

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 150)
            );

            Assert.Equal(6, hasNext.Count);
            Assert.True(hasNext.Take(4).All(_ => _));
            Assert.True(hasNext.Skip(4).All(_ => !_));

            Assert.Equal(4, vals.Count);
            Assert.True(vals[0].Item1 == 200 && vals[0].Item2 == 2);
            Assert.True(vals[1].Item1 == 210 && vals[1].Item2 == 3);
            Assert.True(vals[2].Item1 == 220 && vals[2].Item2 == 5);
            Assert.True(vals[3].Item1 == 230 && vals[3].Item2 == 7);
        }

        [Fact]
        public void GetEnumerator_Regular2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 2),
                OnNext(30, 3),
                OnNext(50, 5),
                OnNext(70, 7),
                OnCompleted<int>(90)
            );

            var res = default(IEnumerator<int>);

            scheduler.ScheduleAbsolute(default(object), 100, (self, _) => { res = xs.GetEnumerator(); return Disposable.Empty; });

            var hasNext = new List<bool>();
            var vals = new List<Tuple<long, int>>();
            for (long i = 120; i <= 220; i += 20)
            {
                var t = i;
                scheduler.ScheduleAbsolute(default(object), t, (self, _) =>
                {
                    var b = res.MoveNext();
                    hasNext.Add(b);
                    if (b)
                    {
                        vals.Add(new Tuple<long, int>(scheduler.Clock, res.Current));
                    }

                    return Disposable.Empty;
                });
            }

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 190)
            );

            Assert.Equal(6, hasNext.Count);
            Assert.True(hasNext.Take(4).All(_ => _));
            Assert.True(hasNext.Skip(4).All(_ => !_));

            Assert.Equal(4, vals.Count);
            Assert.True(vals[0].Item1 == 120 && vals[0].Item2 == 2);
            Assert.True(vals[1].Item1 == 140 && vals[1].Item2 == 3);
            Assert.True(vals[2].Item1 == 160 && vals[2].Item2 == 5);
            Assert.True(vals[3].Item1 == 180 && vals[3].Item2 == 7);
        }

        [Fact]
        public void GetEnumerator_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 2),
                OnNext(30, 3),
                OnNext(50, 5),
                OnNext(70, 7),
                OnCompleted<int>(90)
            );

            var res = default(IEnumerator<int>);

            scheduler.ScheduleAbsolute(default(object), 100, (self, _) => { res = xs.GetEnumerator(); return Disposable.Empty; });

            scheduler.ScheduleAbsolute(default(object), 140, (self, _) =>
            {
                Assert.True(res.MoveNext());
                Assert.Equal(2, res.Current);

                Assert.True(res.MoveNext());
                Assert.Equal(3, res.Current);

                res.Dispose();

                return Disposable.Empty;
            });

            scheduler.ScheduleAbsolute(default(object), 160, (self, _) =>
            {
                ReactiveAssert.Throws<ObjectDisposedException>(() => res.MoveNext());
                return Disposable.Empty;
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 140)
            );
        }

    }
}
