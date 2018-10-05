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
    public class MaterializeTest : ReactiveTest
    {

        [Fact]
        public void Materialize_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Materialize<int>(null));
        }

        [Fact]
        public void Materialize_Never()
        {
            var scheduler = new TestScheduler();
            var res = scheduler.Start(() =>
                Observable.Never<int>().Materialize()
            );

            res.Messages.AssertEqual(
            );
        }

        [Fact]
        public void Materialize_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Materialize()
            );

            res.Messages.AssertEqual(
                OnNext(250, Notification.CreateOnCompleted<int>()),
                OnCompleted<Notification<int>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Materialize_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Materialize()
            );

            res.Messages.AssertEqual(
                OnNext(210, Notification.CreateOnNext(2)),
                OnNext(250, Notification.CreateOnCompleted<int>()),
                OnCompleted<Notification<int>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Materialize_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.Materialize()
            );

            res.Messages.AssertEqual(
                OnNext(250, Notification.CreateOnError<int>(ex)),
                OnCompleted<Notification<int>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

    }
}
