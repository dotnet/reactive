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
    public class PrependTest : ReactiveTest
    {

        [Fact]
        public void Prepend_ArgumentChecking()
        {
            var scheduler = new TestScheduler();
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Prepend(default, 1));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Prepend(default, 1, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Prepend(someObservable, 1, default));
        }

        [Fact]
        public void Prepend()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Prepend(3)
            );

            res.Messages.AssertEqual(
                OnNext(200, 3),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );
        }

        [Fact]
        public void Prepend_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Prepend(3, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 3),
                OnNext(220, 4),
                OnCompleted<int>(250)
            );
        }

        [Fact]
        public void Prepend_Many()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Prepend(3).Prepend(4).Prepend(5)
            );

            res.Messages.AssertEqual(
                OnNext(200, 5),
                OnNext(200, 4),
                OnNext(200, 3),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );
        }

        [Fact]
        public void Prepend_Many_Take()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Prepend(3).Prepend(4).Prepend(5).Take(2)
            );

            res.Messages.AssertEqual(
                OnNext(200, 5),
                OnNext(200, 4),
                OnCompleted<int>(200)
            );
        }


        [Fact]
        public void Prepend_Many_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Prepend(3, scheduler).Prepend(4, scheduler).Prepend(5, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 5),
                OnNext(202, 4),
                OnNext(203, 3),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );
        }

        [Fact]
        public void Prepend_Many_Take_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Prepend(3, scheduler).Prepend(4, scheduler).Prepend(5, scheduler).Take(2)
            );

            res.Messages.AssertEqual(
                OnNext(201, 5),
                OnNext(202, 4),
                OnCompleted<int>(202)
            );
        }
    }
}
