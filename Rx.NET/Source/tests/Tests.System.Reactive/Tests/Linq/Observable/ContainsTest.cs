// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class ContainsTest : ReactiveTest
    {

        [Fact]
        public void Contains_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Contains(default, 42));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Contains(default, 42, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Contains(DummyObservable<int>.Instance, 42, null));
        }

        [Fact]
        public void Contains_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Contains(42)
            );

            res.Messages.AssertEqual(
                OnNext(250, false),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Contains_ReturnPositive()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Contains(2)
            );

            res.Messages.AssertEqual(
                OnNext(210, true),
                OnCompleted<bool>(210)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Contains_ReturnNegative()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Contains(-2)
            );

            res.Messages.AssertEqual(
                OnNext(250, false),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Contains_SomePositive()
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
                xs.Contains(3)
            );

            res.Messages.AssertEqual(
                OnNext(220, true),
                OnCompleted<bool>(220)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void Contains_SomeNegative()
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
                xs.Contains(-3)
            );

            res.Messages.AssertEqual(
                OnNext(250, false),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Contains_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Contains(42)
            );

            res.Messages.AssertEqual(
                OnError<bool>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Contains_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Contains(42)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Contains_ComparerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2)
            );

            var res = scheduler.Start(() =>
                xs.Contains(42, new ContainsComparerThrows())
            );

            res.Messages.AssertEqual(
                OnError<bool>(210, e => e is NotImplementedException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        private class ContainsComparerThrows : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode(int obj)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void Contains_ComparerContainsValue()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 3),
                OnNext(220, 4),
                OnNext(230, 8),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Contains(42, new ContainsComparerMod2())
            );

            res.Messages.AssertEqual(
                OnNext(220, true),
                OnCompleted<bool>(220)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void Contains_ComparerDoesNotContainValue()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 4),
                OnNext(230, 8),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Contains(21, new ContainsComparerMod2())
            );

            res.Messages.AssertEqual(
                OnNext(250, false),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        private class ContainsComparerMod2 : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x % 2 == y % 2;
            }

            public int GetHashCode(int obj)
            {
                return obj.GetHashCode();
            }
        }

    }
}
