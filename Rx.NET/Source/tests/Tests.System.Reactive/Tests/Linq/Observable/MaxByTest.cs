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
    public class MaxByTest : ReactiveTest
    {
        #region + MaxBy +

        [Fact]
        public void MaxBy_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MaxBy(default(IObservable<int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MaxBy(DummyObservable<int>.Instance, default(Func<int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MaxBy(default(IObservable<int>), x => x, Comparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MaxBy(DummyObservable<int>.Instance, default, Comparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MaxBy(DummyObservable<int>.Instance, x => x, null));
        }

        [Fact]
        public void MaxBy_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key)
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.Count == 0),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MaxBy_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(2, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key)
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual(new[] {
                    new KeyValuePair<int, string>(2, "a"),
                })),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MaxBy_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(3, "b")),
                OnNext(220, new KeyValuePair<int, string>(4, "c")),
                OnNext(230, new KeyValuePair<int, string>(2, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key)
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual(new[] {
                    new KeyValuePair<int, string>(4, "c"),
                })),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MaxBy_Multiple()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(3, "b")),
                OnNext(215, new KeyValuePair<int, string>(2, "d")),
                OnNext(220, new KeyValuePair<int, string>(3, "c")),
                OnNext(225, new KeyValuePair<int, string>(2, "y")),
                OnNext(230, new KeyValuePair<int, string>(4, "a")),
                OnNext(235, new KeyValuePair<int, string>(4, "r")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key)
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual(new[] {
                    new KeyValuePair<int, string>(4, "a"),
                    new KeyValuePair<int, string>(4, "r"),
                })),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }
        [Fact]
        public void MaxBy_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnError<KeyValuePair<int, string>>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key)
            );

            res.Messages.AssertEqual(
                OnError<IList<KeyValuePair<int, string>>>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void MaxBy_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z"))
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void MaxBy_Comparer_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.Count == 0),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MaxBy_Comparer_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(2, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual(new[] {
                    new KeyValuePair<int, string>(2, "a"),
                })),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MaxBy_Comparer_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(3, "b")),
                OnNext(220, new KeyValuePair<int, string>(4, "c")),
                OnNext(230, new KeyValuePair<int, string>(2, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual(new[] {
                    new KeyValuePair<int, string>(2, "a"),
                })),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MaxBy_Comparer_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnError<KeyValuePair<int, string>>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
            );

            res.Messages.AssertEqual(
                OnError<IList<KeyValuePair<int, string>>>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void MaxBy_Comparer_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z"))
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void MaxBy_SelectorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(3, "b")),
                OnNext(220, new KeyValuePair<int, string>(2, "c")),
                OnNext(230, new KeyValuePair<int, string>(4, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy<KeyValuePair<int, string>, int>(x => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<IList<KeyValuePair<int, string>>>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void MaxBy_ComparerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(3, "b")),
                OnNext(220, new KeyValuePair<int, string>(2, "c")),
                OnNext(230, new KeyValuePair<int, string>(4, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key, new ThrowingComparer<int>(ex))
            );

            res.Messages.AssertEqual(
                OnError<IList<KeyValuePair<int, string>>>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

    }
}
