// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class RangeTest : ReactiveTest
    {

        [Fact]
        public void Range_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Range(0, 0, null));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Range(0, -1, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Range(int.MaxValue, 2, DummyScheduler.Instance));
        }

        [Fact]
        public void Range_Zero()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Range(0, 0, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(201)
            );
        }

        [Fact]
        public void Range_One()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Range(0, 1, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 0),
                OnCompleted<int>(202)
            );
        }

        [Fact]
        public void Range_Five()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Range(10, 5, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 10),
                OnNext(202, 11),
                OnNext(203, 12),
                OnNext(204, 13),
                OnNext(205, 14),
                OnCompleted<int>(206)
            );
        }

        [Fact]
        public void Range_Boundaries()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Range(int.MaxValue, 1, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, int.MaxValue),
                OnCompleted<int>(202)
            );
        }

        [Fact]
        public void Range_Dispose()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Range(-10, 5, scheduler),
                204
            );

            res.Messages.AssertEqual(
                OnNext(201, -10),
                OnNext(202, -9),
                OnNext(203, -8)
            );
        }

        [Fact]
        public void Range_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Range(0, -1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Range(int.MaxValue, 2));
        }

        [Fact]
        public void Range_Default()
        {
            for (var i = 0; i < 100; i++)
            {
                Observable.Range(100, 100).AssertEqual(Observable.Range(100, 100, DefaultScheduler.Instance));
            }
        }

#if !NO_PERF
        [Fact]
        public void Range_LongRunning1()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Range(0, 100, s);

            var lst = new List<int>();
            var done = false;
            xs.Subscribe(x => { lst.Add(x); }, () => done = true);

            end.WaitOne();

            Assert.True(lst.SequenceEqual(Enumerable.Range(0, 100)));
            Assert.True(done);
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public void Range_LongRunning2()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Range(0, int.MaxValue, s);

            var lst = new List<int>();
            var d = xs.Subscribe(x => { lst.Add(x); });

            start.WaitOne();

            while (lst.Count < 100)
            {
                ;
            }

            d.Dispose();
            end.WaitOne();

            Assert.True(true);
        }

        [Fact]
        public void Range_LongRunning_Empty()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var scheduler = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Range(5, 0, scheduler);

            var lst = new List<int>();
            xs.ForEach(lst.Add);

            Assert.True(lst.SequenceEqual(Enumerable.Range(5, 0)));
        }

        [Fact]
        public void Range_LongRunning_Regular()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var scheduler = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Range(5, 17, scheduler);

            var lst = new List<int>();
            xs.ForEach(lst.Add);

            Assert.True(lst.SequenceEqual(Enumerable.Range(5, 17)));
        }

        [Fact]
        public void Range_LongRunning_Boundaries()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var scheduler = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Range(int.MaxValue, 1, scheduler);

            var lst = new List<int>();
            xs.ForEach(lst.Add);

            Assert.True(lst.SequenceEqual(Enumerable.Range(int.MaxValue, 1)));
        }
#endif

    }
}
