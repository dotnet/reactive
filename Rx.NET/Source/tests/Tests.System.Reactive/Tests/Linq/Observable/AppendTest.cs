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
    public class AppendTest : ReactiveTest
    {
        [Fact]
        public void Append_ArgumentChecking()
        {
            var scheduler = new TestScheduler();
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Append(default, 1));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Append(default, 1, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Append(someObservable, 1, default));
        }

        [Fact]
        public void Append()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Append(3)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(250, 3),
                OnCompleted<int>(250)
            );
        }

        [Fact]
        public void Append_Null()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "1"),
                OnNext(220, "2"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Append(null)
            );

            res.Messages.AssertEqual(
                OnNext(220, "2"),
                OnNext(250, (string)null),
                OnCompleted<string>(250)
            );
        }

        [Fact]
        public void Append_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Append(3, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(251, 3),
                OnCompleted<int>(251)
            );
        }

        [Fact]
        public void Append_Many()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Append(3).Append(4).Append(5)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(250, 3),
                OnNext(250, 4),
                OnNext(250, 5),
                OnCompleted<int>(250)
            );
        }

        [Fact]
        public void Append_Many_Take()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Append(3).Append(4).Append(5).Take(2)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(250, 3),
                OnCompleted<int>(250)
            );
        }

        [Fact]
        public void Append_Many_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Append(3, scheduler).Append(4, scheduler).Append(5, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(251, 3),
                OnNext(252, 4),
                OnNext(253, 5),
                OnCompleted<int>(253)
            );
        }

        [Fact]
        public void Append_Many_Take_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Append(3, scheduler).Append(4, scheduler).Append(5, scheduler).Take(2)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(251, 3),
                OnCompleted<int>(251)
            );
        }
    }
}
