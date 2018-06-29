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
    public class CountTest : ReactiveTest
    {
        [Fact]
        public void Count_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Count(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Count(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Count(DummyObservable<int>.Instance, default));
        }

        [Fact]
        public void Count_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Count()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Count_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Count()
            );

            res.Messages.AssertEqual(
                OnNext(250, 1),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Count_Some()
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
                xs.Count()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Count_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Count()
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Count_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Count()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

#if !NO_PERF && !NO_THREAD
        [Fact]
        public void Count_InjectOverflow()
        {
            var xs = Observable.Return(42, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<int>(xs, int.MaxValue).Count();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }
#endif

        [Fact]
        public void Count_Predicate_Empty_True()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Count(_ => true)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Count_Predicate_Empty_False()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Count(_ => false)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Count_Predicate_Return_True()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Count(_ => true)
            );

            res.Messages.AssertEqual(
                OnNext(250, 1),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Count_Predicate_Return_False()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Count(_ => false)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Count_Predicate_Some_All()
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
                xs.Count(x => x < 10)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Count_Predicate_Some_None()
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
                xs.Count(x => x > 10)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Count_Predicate_Some_Even()
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
                xs.Count(x => x % 2 == 0)
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Count_Predicate_Throw_True()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Count(_ => true)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Count_Predicate_Throw_False()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Count(_ => false)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Count_Predicate_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Count(_ => true)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Count_Predicate_PredicateThrows()
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
                xs.Count(x =>
                {
                    if (x == 3)
                    {
                        throw ex;
                    }

                    return true;
                })
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

#if !NO_PERF && !NO_THREAD && !CRIPPLED_REFLECTION

        [Fact]
        public void Count_Predicate_InjectOverflow()
        {
            var xs = Observable.Return(42, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<int>(xs, int.MaxValue).Count(_ => true);

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }
#endif

    }

#if !CRIPPLED_REFLECTION || NETCOREAPP1_1 || NETCOREAPP1_0
    internal class OverflowInjection<T> : IObservable<T>
    {
        private readonly IObservable<T> _source;
        private readonly object _initialCount;

        public OverflowInjection(IObservable<T> source, object initialCount)
        {
            _source = source;
            _initialCount = initialCount;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            var f = observer.GetType().GetField("_count", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            f.SetValue(observer, _initialCount);

            return _source.Subscribe(observer);
        }
    }
#endif
}
