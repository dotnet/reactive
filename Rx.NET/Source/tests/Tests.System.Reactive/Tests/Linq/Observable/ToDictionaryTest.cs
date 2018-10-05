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
    public class ToDictionaryTest : ReactiveTest
    {

        [Fact]
        public void ToDictionary_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary(null, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary(DummyObservable<int>.Instance, null, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary(null, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int>(DummyObservable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary(null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary(DummyObservable<int>.Instance, null, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int, int>(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, null, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary(null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int, int>(DummyObservable<int>.Instance, null, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int, int>(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, null));
        }

        [Fact]
        public void ToDictionary_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(660)
            );

            var res = scheduler.Start(() =>
                xs.ToDictionary(x => x * 2, x => x * 4, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnNext<IDictionary<int, int>>(660, d =>
                {
                    return d.Count == 4
                        && d[4] == 8
                        && d[6] == 12
                        && d[8] == 16
                        && d[10] == 20;
                }),
                OnCompleted<IDictionary<int, int>>(660)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [Fact]
        public void ToDictionary_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnError<int>(660, ex)
            );

            var res = scheduler.Start(() =>
                xs.ToDictionary(x => x * 2, x => x * 4, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnError<IDictionary<int, int>>(660, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [Fact]
        public void ToDictionary_KeySelectorThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ToDictionary(x => { if (x < 4) { return x * 2; } throw ex; }, x => x * 4, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnError<IDictionary<int, int>>(440, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 440)
            );
        }

        [Fact]
        public void ToDictionary_ElementSelectorThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ToDictionary(x => x * 2, x => { if (x < 4) { return x * 4; } throw ex; }, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnError<IDictionary<int, int>>(440, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 440)
            );
        }

        [Fact]
        public void ToDictionary_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5)
            );

            var res = scheduler.Start(() =>
                xs.ToDictionary(x => x * 2, x => x * 4, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void ToDictionary_MultipleAdd()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(660)
            );

            var res = scheduler.Start(() =>
                xs.ToDictionary(x => x % 2, x => x * 2, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnError<IDictionary<int, int>>(440, e => true)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 440)
            );
        }

        [Fact]
        public void ToDictionary_Default()
        {
            var d1 = Observable.Range(1, 10).ToDictionary(x => x.ToString()).First();
            Assert.Equal(7, d1["7"]);

            var d2 = Observable.Range(1, 10).ToDictionary(x => x.ToString(), x => x * 2).First();
            Assert.Equal(18, d2["9"]);

            var d3 = Observable.Range(1, 10).ToDictionary(x => x.ToString(), EqualityComparer<string>.Default).First();
            Assert.Equal(7, d3["7"]);
        }

    }
}
