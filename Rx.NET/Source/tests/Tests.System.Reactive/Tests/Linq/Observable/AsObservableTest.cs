// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class AsObservableTest : ReactiveTest
    {

        [Fact]
        public void AsObservable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.AsObservable<int>(null));
        }

        [Fact]
        public void AsObservable_AsObservable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var ys = xs.AsObservable().AsObservable();

            Assert.NotSame(xs, ys);

            var res = scheduler.Start(() =>
                ys
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
        public void AsObservable_Hides()
        {
            var xs = Observable.Empty<int>();

            var res = xs.AsObservable();

            Assert.NotSame(xs, res);
        }

        [Fact]
        public void AsObservable_Never()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Never<int>();

            var res = scheduler.Start(() =>
                xs.AsObservable()
            );

            res.Messages.AssertEqual(
            );
        }

        [Fact]
        public void AsObservable_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.AsObservable()
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void AsObservable_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();
            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.AsObservable()
            );

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void AsObservable_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.AsObservable()
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
        public void AsObservable_IsNotEager()
        {
            var scheduler = new TestScheduler();

            var subscribed = false;
            var xs = Observable.Create<int>(obs =>
            {
                subscribed = true;

                var disp = scheduler.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(250)
                ).Subscribe(obs);

                return disp.Dispose;
            });

            xs.AsObservable();
            Assert.False(subscribed);

            var res = scheduler.Start(() =>
                xs.AsObservable()
            );
            Assert.True(subscribed);
        }

    }
}
