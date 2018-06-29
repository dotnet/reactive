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
    public class DistinctTest : ReactiveTest
    {

        [Fact]
        public void Distinct_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(default, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(DummyObservable<int>.Instance, default(EqualityComparer<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(default(IObservable<int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(DummyObservable<int>.Instance, default(Func<int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(default(IObservable<int>), x => x, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(DummyObservable<int>.Instance, default, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(DummyObservable<int>.Instance, x => x, default(EqualityComparer<int>)));
        }

        [Fact]
        public void Distinct_DefaultComparer_AllDistinct()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(350, 1),
                OnNext(380, 3),
                OnNext(400, 5),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct()
            );

            res.Messages.AssertEqual(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(350, 1),
                OnNext(380, 3),
                OnNext(400, 5),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [Fact]
        public void Distinct_DefaultComparer_SomeDuplicates()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(350, 2),
                OnNext(380, 3),
                OnNext(400, 4),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct()
            );

            res.Messages.AssertEqual(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(380, 3),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [Fact]
        public void Distinct_CustomComparer_AllDistinct()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(350, 1),
                OnNext(380, 3),
                OnNext(400, 5),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct(new ModComparer(10))
            );

            res.Messages.AssertEqual(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(350, 1),
                OnNext(380, 3),
                OnNext(400, 5),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [Fact]
        public void Distinct_CustomComparer_SomeDuplicates()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(350, 12),
                OnNext(380, 3),
                OnNext(400, 24),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct(new ModComparer(10))
            );

            res.Messages.AssertEqual(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(380, 3),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        private class ModComparer : IEqualityComparer<int>
        {
            private readonly int _mod;

            public ModComparer(int mod)
            {
                _mod = mod;
            }

            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(x % _mod, y % _mod);
            }

            public int GetHashCode(int obj)
            {
                return EqualityComparer<int>.Default.GetHashCode(obj % _mod);
            }
        }

        [Fact]
        public void Distinct_KeySelector_DefaultComparer_AllDistinct()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 8),
                OnNext(300, 4),
                OnNext(350, 2),
                OnNext(380, 6),
                OnNext(400, 10),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct(x => x / 2)
            );

            res.Messages.AssertEqual(
                OnNext(280, 8),
                OnNext(300, 4),
                OnNext(350, 2),
                OnNext(380, 6),
                OnNext(400, 10),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [Fact]
        public void Distinct_KeySelector_DefaultComparer_SomeDuplicates()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(380, 7),
                OnNext(400, 5),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct(x => x / 2)
            );

            res.Messages.AssertEqual(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(380, 7),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [Fact]
        public void Distinct_KeySelector_CustomComparer_AllDistinct()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 8),
                OnNext(300, 4),
                OnNext(350, 2),
                OnNext(380, 6),
                OnNext(400, 10),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct(x => x / 2, new ModComparer(10))
            );

            res.Messages.AssertEqual(
                OnNext(280, 8),
                OnNext(300, 4),
                OnNext(350, 2),
                OnNext(380, 6),
                OnNext(400, 10),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [Fact]
        public void Distinct_KeySelector_CustomComparer_SomeDuplicates()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 8),
                OnNext(300, 4),
                OnNext(350, 2),
                OnNext(380, 6),
                OnNext(400, 10),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct(x => x / 2, new ModComparer(3))
            );

            res.Messages.AssertEqual(
                OnNext(280, 8),
                OnNext(300, 4),
                OnNext(380, 6),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [Fact]
        public void Distinct_KeySelector_Throws()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 3),
                OnNext(300, 2),
                OnNext(350, 1),
                OnNext(380, 0),
                OnNext(400, 4),
                OnCompleted<int>(420)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Distinct(x => { if (x == 0) { throw ex; } return x / 2; })
            );

            res.Messages.AssertEqual(
                OnNext(280, 3),
                OnNext(350, 1),
                OnError<int>(380, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 380)
            );
        }

        [Fact]
        public void Distinct_CustomComparer_Throws()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(380, 4),
                OnNext(400, 5),
                OnCompleted<int>(420)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Distinct(new ThrowComparer(4, ex))
            );

            res.Messages.AssertEqual(
                OnNext(280, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnError<int>(380, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 380)
            );
        }

        private class ThrowComparer : IEqualityComparer<int>
        {
            private readonly int _err;
            private readonly Exception _ex;

            public ThrowComparer(int err, Exception ex)
            {
                _err = err;
                _ex = ex;
            }

            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(x, y);
            }

            public int GetHashCode(int obj)
            {
                if (obj == _err)
                {
                    throw _ex;
                }

                return EqualityComparer<int>.Default.GetHashCode(obj);
            }
        }

        [Fact]
        public void Distinct_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnError<int>(380, ex)
            );

            var res = scheduler.Start(() =>
                xs.Distinct()
            );

            res.Messages.AssertEqual(
                OnNext(280, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnError<int>(380, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 380)
            );
        }

        [Fact]
        public void Distinct_Null()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "bar"),
                OnNext(240, default(string)),
                OnNext(260, "bar"),
                OnNext(280, "foo"),
                OnNext(300, default(string)),
                OnCompleted<string>(400)
            );

            var res = scheduler.Start(() =>
                xs.Distinct()
            );

            res.Messages.AssertEqual(
                OnNext(220, "bar"),
                OnNext(240, default(string)),
                OnNext(280, "foo"),
                OnCompleted<string>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

    }
}
