// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class CaseTest : ReactiveTest
    {

        [Fact]
        public void Case_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case(null, new Dictionary<int, IObservable<int>>(), DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case(DummyFunc<int>.Instance, null, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case(DummyFunc<int>.Instance, new Dictionary<int, IObservable<int>>(), default(IObservable<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case(null, new Dictionary<int, IObservable<int>>(), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case<int, int>(DummyFunc<int>.Instance, null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case(DummyFunc<int>.Instance, new Dictionary<int, IObservable<int>>(), default(IScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case(null, new Dictionary<int, IObservable<int>>()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case<int, int>(DummyFunc<int>.Instance, null));
        }

        [Fact]
        public void Case_One()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var zs = scheduler.CreateHotObservable(
                OnNext(230, 21),
                OnNext(240, 22),
                OnNext(290, 23),
                OnCompleted<int>(320)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var results = scheduler.Start(() => Observable.Case(() => 1, map, zs));

            results.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
            );

            zs.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void Case_Two()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var zs = scheduler.CreateHotObservable(
                OnNext(230, 21),
                OnNext(240, 22),
                OnNext(290, 23),
                OnCompleted<int>(320)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var results = scheduler.Start(() => Observable.Case(() => 2, map, zs));

            results.Messages.AssertEqual(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            zs.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void Case_Three()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var zs = scheduler.CreateHotObservable(
                OnNext(230, 21),
                OnNext(240, 22),
                OnNext(290, 23),
                OnCompleted<int>(320)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var results = scheduler.Start(() => Observable.Case(() => 3, map, zs));

            results.Messages.AssertEqual(
                OnNext(230, 21),
                OnNext(240, 22),
                OnNext(290, 23),
                OnCompleted<int>(320)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(200, 320)
            );
        }

        [Fact]
        public void Case_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var zs = scheduler.CreateHotObservable(
                OnNext(230, 21),
                OnNext(240, 22),
                OnNext(290, 23),
                OnCompleted<int>(320)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.Case(() => Throw<int>(ex), map, zs));

            results.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
            );

            zs.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void CaseWithDefault_One()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var results = scheduler.Start(() => Observable.Case(() => 1, map, scheduler));

            results.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void CaseWithDefault_Two()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var results = scheduler.Start(() => Observable.Case(() => 2, map, scheduler));

            results.Messages.AssertEqual(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [Fact]
        public void CaseWithDefault_Three()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var results = scheduler.Start(() => Observable.Case(() => 3, map, scheduler));

            results.Messages.AssertEqual(
                OnCompleted<int>(201)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void CaseWithDefault_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.Case(() => Throw<int>(ex), map, scheduler));

            results.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void CaseWithDefault_CheckDefault()
        {
            Observable.Case(() => 1, new Dictionary<int, IObservable<int>>(), DefaultScheduler.Instance)
                .AssertEqual(Observable.Case(() => 1, new Dictionary<int, IObservable<int>>()));
        }

        [Fact]
        public void Case_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnError<int>(300, ex)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var results = scheduler.Start(() => Observable.Case(() => 1, map, scheduler));

            results.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        private static T Throw<T>(Exception ex)
        {
            throw ex;
        }
    }
}
