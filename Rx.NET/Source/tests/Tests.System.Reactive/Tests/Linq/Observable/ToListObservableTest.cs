// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class ToListObservableTest : ReactiveTest
    {

        [Fact]
        public void ToListObservable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).ToListObservable());
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Never<int>().ToListObservable().Subscribe(null));
        }

        [Fact]
        public void ToListObservable_OnNext()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 1),
                OnNext(400, 2),
                OnNext(500, 3),
                OnNext(600, 4)
            );

            var res = scheduler.Start(() =>
                xs.ToListObservable()
            );

            res.Messages.AssertEqual(
            );
        }

        [Fact]
        public void ToListObservable_OnError()
        {
            var scheduler = new TestScheduler();

            var ex = new InvalidOperationException();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 1),
                OnNext(400, 2),
                OnNext(500, 3),
                OnError<int>(600, ex)
            );

            var s = default(ListObservable<int>);
            var subscription = default(IDisposable);
            var res = scheduler.CreateObserver<object>();

            scheduler.ScheduleAbsolute(Created, () => s = xs.ToListObservable());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = s.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.Start();

            ReactiveAssert.Throws<InvalidOperationException>(() => { var t = s.Value; });

            res.Messages.AssertEqual(
                OnError<object>(600, ex)
            );
        }

        [Fact]
        public void ToListObservable_OnCompleted()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 1),
                OnNext(400, 2),
                OnNext(500, 3),
                OnCompleted<int>(600)
            );

            var s = default(ListObservable<int>);

            var res = scheduler.Start(() =>
                s = xs.ToListObservable()
            );

            s.AssertEqual(1, 2, 3);

            res.Messages.AssertEqual(
                OnCompleted<object>(600)
            );

            Assert.Equal(3, s.Value);
        }

        [Fact]
        public void ToListObservable_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 1),
                OnNext(400, 2),
                OnNext(500, 3),
                OnNext(1050, 4),
                OnCompleted<int>(1100)
            );

            var s = default(ListObservable<int>);

            var res = scheduler.Start(() =>
                s = xs.ToListObservable()
            );

            res.Messages.AssertEqual(
            );
        }

        [Fact]
        public void ToListObservable_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.ToListObservable()
            );

            res.Messages.AssertEqual(
            );
        }

    }
}
