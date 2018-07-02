// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class DistinctUntilChangedTest : ReactiveTest
    {

        [Fact]
        public void DistinctUntilChanged_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged(null, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged(someObservable, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged<int, int>(null, _ => _));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged<int, int>(someObservable, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged(someObservable, _ => _, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged<int, int>(null, _ => _, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged(someObservable, null, EqualityComparer<int>.Default));
        }

        [Fact]
        public void DistinctUntilChanged_Never()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Never<int>();

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged()
            );

            res.Messages.AssertEqual(
            );
        }

        [Fact]
        public void DistinctUntilChanged_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged()
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void DistinctUntilChanged_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged()
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void DistinctUntilChanged_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();
            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged()
            );

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void DistinctUntilChanged_AllChanges()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void DistinctUntilChanged_AllSame()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 2),
                OnNext(230, 2),
                OnNext(240, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void DistinctUntilChanged_SomeChanges()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2), //*
                OnNext(215, 3), //*
                OnNext(220, 3),
                OnNext(225, 2), //*
                OnNext(230, 2),
                OnNext(230, 1), //*
                OnNext(240, 2), //*
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(215, 3),
                OnNext(225, 2),
                OnNext(230, 1),
                OnNext(240, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void DistinctUntilChanged_Comparer_AllEqual()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged(new FuncComparer<int>((x, y) => true))
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void DistinctUntilChanged_Comparer_AllDifferent()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 2),
                OnNext(230, 2),
                OnNext(240, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged(new FuncComparer<int>((x, y) => false))
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 2),
                OnNext(230, 2),
                OnNext(240, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void DistinctUntilChanged_KeySelector_Div2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2), //*
                OnNext(220, 4),
                OnNext(230, 3), //*
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged(x => x % 2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(230, 3),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        private class FuncComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _equals;

            public FuncComparer(Func<T, T, bool> equals)
            {
                _equals = equals;
            }

            public bool Equals(T x, T y)
            {
                return _equals(x, y);
            }

            public int GetHashCode(T obj)
            {
                return 0;
            }
        }

        [Fact]
        public void DistinctUntilChanged_KeySelectorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged(new Func<int, int>(x => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [Fact]
        public void DistinctUntilChanged_ComparerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged(new ThrowComparer<int>(ex))
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnError<int>(220, ex)
            );
        }

        private class ThrowComparer<T> : IEqualityComparer<T>
        {
            private readonly Exception _ex;

            public ThrowComparer(Exception ex)
            {
                _ex = ex;
            }

            public bool Equals(T x, T y)
            {
                throw _ex;
            }

            public int GetHashCode(T obj)
            {
                return 0;
            }
        }

        [Fact]
        public void DistinctUntilChanged_KeySelector_Comparer()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2), // * key = 1    % 3 = 1
                OnNext(220, 8), //   key = 4    % 3 = 1   same
                OnNext(230, 2), //   key = 1    % 3 = 1   same
                OnNext(240, 5), // * key = 2    % 3 = 2
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged(x => x / 2, new FuncComparer<int>((x, y) => x % 3 == y % 3))
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

    }
}
