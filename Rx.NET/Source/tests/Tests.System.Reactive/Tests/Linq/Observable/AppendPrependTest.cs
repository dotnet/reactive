// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class AppendPrependTest : ReactiveTest
    {
        [Fact]
        public void AppendPrepend()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Append(3).Prepend(4)
            );

            res.Messages.AssertEqual(
                OnNext(200, 4),
                OnNext(220, 2),
                OnNext(250, 3),
                OnCompleted<int>(250)
            );
        }

        [Fact]
        public void PrependAppend()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Prepend(4).Append(3)
            );

            res.Messages.AssertEqual(
                OnNext(200, 4),
                OnNext(220, 2),
                OnNext(250, 3),
                OnCompleted<int>(250)
            );
        }

        [Fact]
        public void AppendPrepend_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Append(3, scheduler).Prepend(4, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 4),
                OnNext(220, 2),
                OnNext(251, 3),
                OnCompleted<int>(251)
            );
        }

        [Fact]
        public void PrependAppend_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Prepend(4, scheduler).Append(3, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 4),
                OnNext(220, 2),
                OnNext(251, 3),
                OnCompleted<int>(251)
            );
        }

        [Fact]
        public void AppendPrepend_Many()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Append(3).Append(4).Append(5).Prepend(6).Prepend(7).Prepend(8)
            );

            res.Messages.AssertEqual(
                OnNext(200, 8),
                OnNext(200, 7),
                OnNext(200, 6),
                OnNext(220, 2),
                OnNext(250, 3),
                OnNext(250, 4),
                OnNext(250, 5),
                OnCompleted<int>(250)
            );
        }

        [Fact]
        public void PrependAppend_Many()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Prepend(6).Prepend(7).Prepend(8).Append(3).Append(4).Append(5)
            );

            res.Messages.AssertEqual(
                OnNext(200, 8),
                OnNext(200, 7),
                OnNext(200, 6),
                OnNext(220, 2),
                OnNext(250, 3),
                OnNext(250, 4),
                OnNext(250, 5),
                OnCompleted<int>(250)
            );
        }

        [Fact]
        public void AppendPrepend_Many_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Append(3, scheduler)
                  .Append(4, scheduler)
                  .Append(5, scheduler)
                  .Prepend(6, scheduler)
                  .Prepend(7, scheduler)
                  .Prepend(8, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 8),
                OnNext(202, 7),
                OnNext(203, 6),
                OnNext(220, 2),
                OnNext(251, 3),
                OnNext(252, 4),
                OnNext(253, 5),
                OnCompleted<int>(253)
            );
        }

        [Fact]
        public void PrependAppend_Many_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Prepend(6, scheduler)
                  .Prepend(7, scheduler)
                  .Prepend(8, scheduler)
                  .Append(3, scheduler)
                  .Append(4, scheduler)
                  .Append(5, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 8),
                OnNext(202, 7),
                OnNext(203, 6),
                OnNext(220, 2),
                OnNext(251, 3),
                OnNext(252, 4),
                OnNext(253, 5),
                OnCompleted<int>(253)
            );
        }

        [Fact]
        public void AppendPrepend_Mixed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Append(3).Prepend(6).Append(4).Prepend(7).Prepend(8).Append(5)
            );

            res.Messages.AssertEqual(
                OnNext(200, 8),
                OnNext(200, 7),
                OnNext(200, 6),
                OnNext(220, 2),
                OnNext(250, 3),
                OnNext(250, 4),
                OnNext(250, 5),
                OnCompleted<int>(250)
            );
        }

        [Fact]
        public void AppendPrepend_Mixed_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Append(3, scheduler)
                  .Prepend(6, scheduler)
                  .Append(4, scheduler)
                  .Prepend(7, scheduler)
                  .Prepend(8, scheduler)
                  .Append(5, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 8),
                OnNext(202, 7),
                OnNext(203, 6),
                OnNext(220, 2),
                OnNext(251, 3),
                OnNext(252, 4),
                OnNext(253, 5),
                OnCompleted<int>(253)
            );
        }

        [Fact]
        public void AppendPrepend_MixedSchedulers()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Append(3, scheduler)
                  .Prepend(6)
                  .Append(4, scheduler)
                  .Prepend(7)
                  .Prepend(8, scheduler)
                  .Append(5)
            );

            res.Messages.AssertEqual(
                OnNext(201, 8),
                OnNext(201, 7),
                OnNext(201, 6),
                OnNext(220, 2),
                OnNext(251, 3),
                OnNext(252, 4),
                OnNext(252, 5),
                OnCompleted<int>(252)
            );
        }

        [Fact]
        public void AppendPrepend_SchedulerRecursive()
        {
            var scheduler = new EventLoopScheduler();

            var source = Observable.Range(1, 1000).DelaySubscription(TimeSpan.FromMilliseconds(100), ThreadPoolScheduler.Instance);

            var result = source.Prepend(0, scheduler).Prepend(-1, scheduler)
                .Timeout(TimeSpan.FromSeconds(5))
                .ToList().First();

            Assert.Equal(1002, result.Count);

            var j = 0;
            for (var i = -1; i <= 1000; i++)
            {
                Assert.Equal(i, result[j++]);
            }
        }

        [Fact]
        public void AppendPrepend_SchedulerLongRunning()
        {
            var scheduler = NewThreadScheduler.Default;
            Assert.True(scheduler is ISchedulerLongRunning, "Not a long-running scheduler!");

            var source = Observable.Range(1, 1000).DelaySubscription(TimeSpan.FromMilliseconds(100), ThreadPoolScheduler.Instance);

            var result = source.Prepend(0, scheduler).Prepend(-1, scheduler)
                .Timeout(TimeSpan.FromSeconds(5))
                .ToList().First();

            Assert.Equal(1002, result.Count);

            var j = 0;
            for (var i = -1; i <= 1000; i++)
            {
                Assert.Equal(i, result[j++]);
            }
        }
    }
}
