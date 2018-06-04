// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;
using ReactiveTests.Dummies;
using System.Reflection;
using System.Threading;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace ReactiveTests.Tests
{
    public class DoWhileTest : ReactiveTest
    {

        [Fact]
        public void DoWhile_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DoWhile(DummyObservable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DoWhile(default(IObservable<int>), DummyFunc<bool>.Instance));
        }

        [Fact]
        public void DoWhile_AlwaysFalse()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            var results = scheduler.Start(() => Observable.DoWhile(xs, () => false));

            results.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnCompleted<int>(450)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [Fact]
        public void DoWhile_AlwaysTrue()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            var results = scheduler.Start(() => Observable.DoWhile(xs, () => true));

            results.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnNext(500, 1),
                OnNext(550, 2),
                OnNext(600, 3),
                OnNext(650, 4),
                OnNext(750, 1),
                OnNext(800, 2),
                OnNext(850, 3),
                OnNext(900, 4)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450),
                Subscribe(450, 700),
                Subscribe(700, 950),
                Subscribe(950, 1000)
            );
        }

        [Fact]
        public void DoWhile_AlwaysTrue_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnError<int>(50, ex)
            );

            var results = scheduler.Start(() => Observable.DoWhile(xs, () => true));

            results.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void DoWhile_AlwaysTrue_Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1)
            );

            var results = scheduler.Start(() => Observable.DoWhile(xs, () => true));

            results.Messages.AssertEqual(
                OnNext(250, 1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void DoWhile_SometimesTrue()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            int n = 0;

            var results = scheduler.Start(() => Observable.DoWhile(xs, () => ++n < 3));

            results.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnNext(500, 1),
                OnNext(550, 2),
                OnNext(600, 3),
                OnNext(650, 4),
                OnNext(750, 1),
                OnNext(800, 2),
                OnNext(850, 3),
                OnNext(900, 4),
                OnCompleted<int>(950)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450),
                Subscribe(450, 700),
                Subscribe(700, 950)
            );
        }

        [Fact]
        public void DoWhile_SometimesThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            int n = 0;

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.DoWhile(xs, () => ++n < 3 ? true : Throw<bool>(ex)));

            results.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnNext(500, 1),
                OnNext(550, 2),
                OnNext(600, 3),
                OnNext(650, 4),
                OnNext(750, 1),
                OnNext(800, 2),
                OnNext(850, 3),
                OnNext(900, 4),
                OnError<int>(950, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450),
                Subscribe(450, 700),
                Subscribe(700, 950)
            );
        }

        static T Throw<T>(Exception ex)
        {
            throw ex;
        }
    }
}
