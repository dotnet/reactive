// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class LongCountTest : ReactiveTest
    {

        [Fact]
        public void LongCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LongCount(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LongCount(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LongCount(DummyObservable<int>.Instance, default));
        }

        [Fact]
        public void LongCount_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void LongCount_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount()
            );

            res.Messages.AssertEqual(
                OnNext(250, 1L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void LongCount_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void LongCount_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.LongCount()
            );

            res.Messages.AssertEqual(
                OnError<long>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void LongCount_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.LongCount()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

#if !NO_PERF && !NO_THREAD
        [Fact]
        public void LongCount_InjectOverflow()
        {
            var xs = Observable.Return(42, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<int>(xs, long.MaxValue).LongCount();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }
#endif

        [Fact]
        public void LongCount_Predicate_Empty_True()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(_ => true)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void LongCount_Predicate_Empty_False()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(_ => false)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void LongCount_Predicate_Return_True()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(_ => true)
            );

            res.Messages.AssertEqual(
                OnNext(250, 1L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void LongCount_Predicate_Return_False()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(_ => false)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void LongCount_Predicate_Some_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(x => x < 10)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void LongCount_Predicate_Some_None()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(x => x > 10)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void LongCount_Predicate_Some_Even()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(x => x % 2 == 0)
            );

            res.Messages.AssertEqual(
                OnNext(250, 2L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void LongCount_Predicate_Throw_True()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(_ => true)
            );

            res.Messages.AssertEqual(
                OnError<long>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void LongCount_Predicate_Throw_False()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(_ => false)
            );

            res.Messages.AssertEqual(
                OnError<long>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void LongCount_Predicate_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(_ => true)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void LongCount_Predicate_PredicateThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.LongCount(x =>
                {
                    if (x == 3)
                    {
                        throw ex;
                    }

                    return true;
                })
            );

            res.Messages.AssertEqual(
                OnError<long>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

#if !NO_PERF && !NO_THREAD && !CRIPPLED_REFLECTION
        [Fact]
        public void LongCount_Predicate_InjectOverflow()
        {
            var xs = Observable.Return(42, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<int>(xs, long.MaxValue).LongCount(_ => true);

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }
#endif

    }
}
