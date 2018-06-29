// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class StartWithTest : ReactiveTest
    {

        [Fact]
        public void StartWith_ArgumentChecking()
        {
            var values = (IEnumerable<int>)new[] { 1, 2, 3 };

            var scheduler = new TestScheduler();
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartWith(default, 1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartWith(someObservable, default(int[])));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartWith(default(IObservable<int>), values));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartWith(someObservable, default(IEnumerable<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartWith(default, scheduler, 1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartWith(someObservable, default(IScheduler), 1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartWith(someObservable, scheduler, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartWith(default(IObservable<int>), scheduler, values));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartWith(someObservable, default, values));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartWith(someObservable, scheduler, default(IEnumerable<int>)));
        }

        [Fact]
        public void StartWith()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.StartWith(1)
            );

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );
        }

        [Fact]
        public void StartWith_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.StartWith(scheduler, 1, 2, 3)
            );

            res.Messages.AssertEqual(
                OnNext(201, 1),
                OnNext(202, 2),
                OnNext(203, 3),
                OnNext(220, 4),
                OnCompleted<int>(250)
            );
        }

        [Fact]
        public void StartWith_Enumerable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 4),
                OnCompleted<int>(250)
            );

            var data = new List<int>(new[] { 1, 2, 3 });
            var res = scheduler.Start(() =>
                xs.StartWith(data)
            );

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(200, 2),
                OnNext(200, 3),
                OnNext(220, 4),
                OnCompleted<int>(250)
            );
        }

        [Fact]
        public void StartWith_Enumerable_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 4),
                OnCompleted<int>(250)
            );

            var data = new List<int>(new[] { 1, 2, 3 });
            var res = scheduler.Start(() =>
                xs.StartWith(scheduler, data)
            );

            res.Messages.AssertEqual(
                OnNext(201, 1),
                OnNext(202, 2),
                OnNext(203, 3),
                OnNext(220, 4),
                OnCompleted<int>(250)
            );
        }

    }
}
