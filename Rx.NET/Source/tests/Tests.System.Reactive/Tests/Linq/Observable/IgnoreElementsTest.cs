// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class IgnoreElementsTest : ReactiveTest
    {

        [Fact]
        public void IgnoreElements_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.IgnoreElements<int>(null));
        }

        [Fact]
        public void IgnoreElements_IgnoreElements()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.IgnoreElements().IgnoreElements()
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void IgnoreElements_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            var res = scheduler.Start(() =>
                xs.IgnoreElements()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void IgnoreElements_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnCompleted<int>(610)
            );

            var res = scheduler.Start(() =>
                xs.IgnoreElements()
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(610)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 610)
            );
        }

        [Fact]
        public void IgnoreElements_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnError<int>(610, ex)
            );

            var res = scheduler.Start(() =>
                xs.IgnoreElements()
            );

            res.Messages.AssertEqual(
                OnError<int>(610, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 610)
            );
        }

    }
}
