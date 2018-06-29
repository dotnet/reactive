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
    public class ToObservableTest : ReactiveTest
    {

        [Fact]
        public void EnumerableToObservable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToObservable((IEnumerable<int>)null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToObservable(DummyEnumerable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToObservable(DummyEnumerable<int>.Instance, DummyScheduler.Instance).Subscribe(null));
            ReactiveAssert.Throws<NullReferenceException>(() => Observable.ToObservable(NullEnumeratorEnumerable<int>.Instance, Scheduler.CurrentThread).Subscribe());
        }

        [Fact]
        public void EnumerableToObservable_Complete()
        {
            var scheduler = new TestScheduler();

            var e = new MockEnumerable<int>(scheduler,
                new[] { 3, 1, 2, 4 }
            );

            var results = scheduler.Start(() =>
                e.ToObservable(scheduler)
            );

            results.Messages.AssertEqual(
                OnNext(201, 3),
                OnNext(202, 1),
                OnNext(203, 2),
                OnNext(204, 4),
                OnCompleted<int>(205)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 205)
            );
        }

        [Fact]
        public void EnumerableToObservable_Dispose()
        {
            var scheduler = new TestScheduler();

            var e = new MockEnumerable<int>(scheduler,
                new[] { 3, 1, 2, 4 }
            );

            var results = scheduler.Start(() =>
                e.ToObservable(scheduler),
                203
            );

            results.Messages.AssertEqual(
                OnNext(201, 3),
                OnNext(202, 1)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 203)
            );
        }

        [Fact]
        public void EnumerableToObservable_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var e = new MockEnumerable<int>(scheduler,
                EnumerableToObservable_Error_Core(ex)
            );

            var results = scheduler.Start(() =>
                e.ToObservable(scheduler)
            );

            results.Messages.AssertEqual(
                OnNext(201, 1),
                OnNext(202, 2),
                OnError<int>(203, ex)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 203)
            );
        }

        [Fact]
        public void EnumerableToObservable_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToObservable((IEnumerable<int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToObservable(DummyEnumerable<int>.Instance).Subscribe(null));
        }

        [Fact]
        public void EnumerableToObservable_Default()
        {
            var xs = new[] { 4, 3, 1, 5, 9, 2 };

            xs.ToObservable().AssertEqual(xs.ToObservable(DefaultScheduler.Instance));
        }

#if !NO_PERF
        [Fact]
        public void EnumerableToObservable_LongRunning_Complete()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var scheduler = new TestLongRunningScheduler(x => start = x, x => end = x);

            var e = new[] { 3, 1, 2, 4 };

            var results = e.ToObservable(scheduler);

            var lst = new List<int>();
            results.Subscribe(lst.Add);

            start.WaitOne();
            end.WaitOne();

            Assert.True(e.SequenceEqual(lst));
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public void EnumerableToObservable_LongRunning_Dispose()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var scheduler = new TestLongRunningScheduler(x => start = x, x => end = x);

            var e = Enumerable.Range(0, int.MaxValue);

            var results = e.ToObservable(scheduler);

            var lst = new List<int>();
            var d = results.Subscribe(lst.Add);

            start.WaitOne();

            while (lst.Count < 100)
            {
                ;
            }

            d.Dispose();
            end.WaitOne();

            Assert.True(e.Take(100).SequenceEqual(lst.Take(100)));
        }

        [Fact]
        public void EnumerableToObservable_LongRunning_Error()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var scheduler = new TestLongRunningScheduler(x => start = x, x => end = x);

            var ex = new Exception();
            var e = EnumerableToObservable_Error_Core(ex);

            var results = e.ToObservable(scheduler);

            var lst = new List<int>();
            var err = default(Exception);
            results.Subscribe(lst.Add, ex_ => err = ex_);

            start.WaitOne();
            end.WaitOne();

            Assert.True(new[] { 1, 2 }.SequenceEqual(lst));
            Assert.Same(ex, err);
        }
#endif

        private static IEnumerable<int> EnumerableToObservable_Error_Core(Exception ex)
        {
            yield return 1;
            yield return 2;
            throw ex;
        }

        [Fact]
        public void EnumerableToObservable_GetEnumeratorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = new RogueEnumerable<int>(ex);

            var res = scheduler.Start(() =>
                xs.ToObservable(scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

    }
}
