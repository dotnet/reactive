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
    public class ChunkifyTest : ReactiveTest
    {

        [Fact]
        public void Chunkify_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Chunkify(default(IObservable<int>)));
        }

        [Fact]
        public void Chunkify_Regular1()
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

            var ys = xs.Chunkify();
            var e = default(IEnumerator<IList<int>>);

            var res = new List<IList<int>>();

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
            Assert.True(res[0].SequenceEqual(new int[] { }));
            Assert.True(res[1].SequenceEqual(new int[] { 3 }));
            Assert.True(res[2].SequenceEqual(new int[] { 4 }));
            Assert.True(res[3].SequenceEqual(new int[] { }));
            Assert.True(res[4].SequenceEqual(new int[] { 5, 6, 7 }));
            Assert.True(res[5].SequenceEqual(new int[] { 8 }));
            Assert.True(res[6].SequenceEqual(new int[] { }));
        }

        [Fact]
        public void Chunkify_Regular2()
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

            var ys = xs.Chunkify();
            var e = default(IEnumerator<IList<int>>);

            var res = new List<IList<int>>();

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
            Assert.True(res[0].SequenceEqual(new int[] { 3, 4, 5 }));
            Assert.True(res[1].SequenceEqual(new int[] { 6, 7, 8 }));
        }

        [Fact]
        public void Chunkify_Error()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnNext(600, 6),
                OnError<int>(700, ex)
            );

            var ys = xs.Chunkify();
            var e = default(IEnumerator<IList<int>>);

            var res = new List<IList<int>>();

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
            scheduler.ScheduleAbsolute(750, () =>
            {
                try
                {
                    e.MoveNext();
                    Assert.True(false);
                }
                catch (Exception error)
                {
                    Assert.Same(ex, error);
                }
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(250, 700)
            );

            Assert.Equal(4, res.Count);
            Assert.True(res[0].SequenceEqual(new int[] { }));
            Assert.True(res[1].SequenceEqual(new int[] { 3 }));
            Assert.True(res[2].SequenceEqual(new int[] { 4 }));
            Assert.True(res[3].SequenceEqual(new int[] { }));
        }

    }
}
