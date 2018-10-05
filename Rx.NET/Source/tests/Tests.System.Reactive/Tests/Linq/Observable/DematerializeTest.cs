// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class DematerializeTest : ReactiveTest
    {

        [Fact]
        public void Dematerialize_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Dematerialize<int>(null));
        }

        [Fact]
        public void Dematerialize_Range1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, Notification.CreateOnNext(41)),
                OnNext(210, Notification.CreateOnNext(42)),
                OnNext(220, Notification.CreateOnNext(43)),
                OnCompleted<Notification<int>>(250)
            );
            var res = scheduler.Start(() =>
                xs.Dematerialize()
            );

            res.Messages.AssertEqual(
                OnNext(210, 42),
                OnNext(220, 43),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Dematerialize_Range2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, Notification.CreateOnNext(41)),
                OnNext(210, Notification.CreateOnNext(42)),
                OnNext(220, Notification.CreateOnNext(43)),
                OnNext(230, Notification.CreateOnCompleted<int>())
            );
            var res = scheduler.Start(() =>
                xs.Dematerialize()
            );

            res.Messages.AssertEqual(
                OnNext(210, 42),
                OnNext(220, 43),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void Dematerialize_Error1()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, Notification.CreateOnNext(41)),
                OnNext(210, Notification.CreateOnNext(42)),
                OnNext(220, Notification.CreateOnNext(43)),
                OnError<Notification<int>>(230, ex)
            );
            var res = scheduler.Start(() =>
                xs.Dematerialize()
            );

            res.Messages.AssertEqual(
                OnNext(210, 42),
                OnNext(220, 43),
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void Dematerialize_Error2()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, Notification.CreateOnNext(41)),
                OnNext(210, Notification.CreateOnNext(42)),
                OnNext(220, Notification.CreateOnNext(43)),
                OnNext(230, Notification.CreateOnError<int>(ex))
            );
            var res = scheduler.Start(() =>
                xs.Dematerialize()
            );

            res.Messages.AssertEqual(
                OnNext(210, 42),
                OnNext(220, 43),
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void Materialize_Dematerialize_Never()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Never<int>();

            var res = scheduler.Start(() =>
                xs.Materialize().Dematerialize()
            );

            res.Messages.AssertEqual(
            );
        }

        [Fact]
        public void Materialize_Dematerialize_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Materialize().Dematerialize()
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Materialize_Dematerialize_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Materialize().Dematerialize()
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
        public void Materialize_Dematerialize_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.Materialize().Dematerialize()
            );

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

    }
}
