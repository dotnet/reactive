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
    public class ElementAtOrDefaultTest : ReactiveTest
    {

        [Fact]
        public void ElementAtOrDefault_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ElementAtOrDefault(default(IObservable<int>), 2));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.ElementAtOrDefault(DummyObservable<int>.Instance, -1));
        }

        [Fact]
        public void ElementAtOrDefault_First()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnNext(470, 44),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ElementAtOrDefault(0)
            );

            res.Messages.AssertEqual(
                OnNext(280, 42),
                OnCompleted<int>(280)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 280)
            );
        }

        [Fact]
        public void ElementAtOrDefault_Other()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnNext(470, 44),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ElementAtOrDefault(2)
            );

            res.Messages.AssertEqual(
                OnNext(470, 44),
                OnCompleted<int>(470)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 470)
            );
        }

        [Fact]
        public void ElementAtOrDefault_OutOfRange()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnNext(470, 44),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ElementAtOrDefault(3)
            );

            res.Messages.AssertEqual(
                OnNext(600, 0),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void ElementAtOrDefault_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnError<int>(420, ex)
            );

            var res = scheduler.Start(() =>
                xs.ElementAtOrDefault(3)
            );

            res.Messages.AssertEqual(
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

    }
}
