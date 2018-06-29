// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
#pragma warning disable IDE0039 // Use local function
    public class ManySelectTest : ReactiveTest
    {

        [Fact]
        public void ManySelect_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.ManySelect(null, DummyFunc<IObservable<int>, int>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.ManySelect<int, int>(DummyObservable<int>.Instance, null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.ManySelect(DummyObservable<int>.Instance, DummyFunc<IObservable<int>, int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.ManySelect(null, DummyFunc<IObservable<int>, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.ManySelect<int, int>(DummyObservable<int>.Instance, null));
        }

        [Fact]
        public void ManySelect_Law_1()
        {
            var xs = Observable.Range(1, 0);

            var left = xs.ManySelect(Observable.First);
            var right = xs;

            Assert.True(left.SequenceEqual(right).First());
        }

        [Fact]
        public void ManySelect_Law_2()
        {
            var xs = Observable.Range(1, 10);
            Func<IObservable<int>, int> f = ys => ys.Count().First();

            var left = xs.ManySelect(f).First();
            var right = f(xs);

            Assert.Equal(left, right);
        }

        [Fact]
        public void ManySelect_Law_3()
        {
            var xs = Observable.Range(1, 10);
            Func<IObservable<int>, int> f = ys => ys.Count().First();
            Func<IObservable<int>, int> g = ys => ys.Last();

            var left = xs.ManySelect(f).ManySelect(g);
            var right = xs.ManySelect(ys => g(ys.ManySelect(f)));

            Assert.True(left.SequenceEqual(right).First());
        }

        [Fact]
        public void ManySelect_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(220, 2),
                OnNext(270, 3),
                OnNext(410, 4),
                OnCompleted<int>(500)
            );

            var res = scheduler.Start(() =>
                xs.ManySelect(ys => ys.First(), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(221, 2),
                OnNext(271, 3),
                OnNext(411, 4),
                OnCompleted<int>(501)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [Fact]
        public void ManySelect_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(220, 2),
                OnNext(270, 3),
                OnNext(410, 4),
                OnError<int>(500, ex)
            );

            var res = scheduler.Start(() =>
                xs.ManySelect(ys => ys.First(), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(221, 2),
                OnNext(271, 3),
                OnNext(411, 4),
                OnError<int>(501, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

    }
#pragma warning restore IDE0039 // Use local function
}
