// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class DefaultIfEmptyTest : ReactiveTest
    {

        [Fact]
        public void DefaultIfEmpty_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DefaultIfEmpty(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DefaultIfEmpty(default, 42));
        }

        [Fact]
        public void DefaultIfEmpty_NonEmpty1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.DefaultIfEmpty()
            );

            res.Messages.AssertEqual(
                OnNext(280, 42),
                OnNext(360, 43),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [Fact]
        public void DefaultIfEmpty_NonEmpty2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.DefaultIfEmpty(-1)
            );

            res.Messages.AssertEqual(
                OnNext(280, 42),
                OnNext(360, 43),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [Fact]
        public void DefaultIfEmpty_Empty1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.DefaultIfEmpty()
            );

            res.Messages.AssertEqual(
                OnNext(420, 0),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [Fact]
        public void DefaultIfEmpty_Empty2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.DefaultIfEmpty(-1)
            );

            res.Messages.AssertEqual(
                OnNext(420, -1),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [Fact]
        public void DefaultIfEmpty_Throw1()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(420, ex)
            );

            var res = scheduler.Start(() =>
                xs.DefaultIfEmpty()
            );

            res.Messages.AssertEqual(
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [Fact]
        public void DefaultIfEmpty_Throw2()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(420, ex)
            );

            var res = scheduler.Start(() =>
                xs.DefaultIfEmpty(42)
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
