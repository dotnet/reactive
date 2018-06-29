// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class SwitchTest : ReactiveTest
    {

        [Fact]
        public void Switch_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Switch((IObservable<IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Switch((IObservable<Task<int>>)null));
        }

        [Fact]
        public void Switch_Data()
        {
            var scheduler = new TestScheduler();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(10, 101),
                OnNext(20, 102),
                OnNext(110, 103),
                OnNext(120, 104),
                OnNext(210, 105),
                OnNext(220, 106),
                OnCompleted<int>(230)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(10, 201),
                OnNext(20, 202),
                OnNext(30, 203),
                OnNext(40, 204),
                OnCompleted<int>(50)
            );

            var ys3 = scheduler.CreateColdObservable(
                OnNext(10, 301),
                OnNext(20, 302),
                OnNext(30, 303),
                OnNext(40, 304),
                OnCompleted<int>(150)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(300, ys1),
                OnNext<IObservable<int>>(400, ys2),
                OnNext<IObservable<int>>(500, ys3),
                OnCompleted<IObservable<int>>(600)
            );

            var res = scheduler.Start(() =>
                xs.Switch()
            );

            res.Messages.AssertEqual(
                OnNext(310, 101),
                OnNext(320, 102),
                OnNext(410, 201),
                OnNext(420, 202),
                OnNext(430, 203),
                OnNext(440, 204),
                OnNext(510, 301),
                OnNext(520, 302),
                OnNext(530, 303),
                OnNext(540, 304),
                OnCompleted<int>(650)
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
#endif

            ys1.Subscriptions.AssertEqual(
                Subscribe(300, 400)
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys2.Subscriptions.AssertEqual(
                Subscribe(400, 450)
            );
#else
            ys2.Subscriptions.AssertEqual(
                Subscribe(400, 500)
            );
#endif

            ys3.Subscriptions.AssertEqual(
                Subscribe(500, 650)
            );
        }

        [Fact]
        public void Switch_InnerThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(10, 101),
                OnNext(20, 102),
                OnNext(110, 103),
                OnNext(120, 104),
                OnNext(210, 105),
                OnNext(220, 106),
                OnCompleted<int>(230)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(10, 201),
                OnNext(20, 202),
                OnNext(30, 203),
                OnNext(40, 204),
                OnError<int>(50, ex)
            );

            var ys3 = scheduler.CreateColdObservable(
                OnNext(10, 301),
                OnNext(20, 302),
                OnNext(30, 303),
                OnNext(40, 304),
                OnCompleted<int>(150)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(300, ys1),
                OnNext<IObservable<int>>(400, ys2),
                OnNext<IObservable<int>>(500, ys3),
                OnCompleted<IObservable<int>>(600)
            );

            var res = scheduler.Start(() =>
                xs.Switch()
            );

            res.Messages.AssertEqual(
                OnNext(310, 101),
                OnNext(320, 102),
                OnNext(410, 201),
                OnNext(420, 202),
                OnNext(430, 203),
                OnNext(440, 204),
                OnError<int>(450, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );

            ys1.Subscriptions.AssertEqual(
                Subscribe(300, 400)
            );

            ys2.Subscriptions.AssertEqual(
                Subscribe(400, 450)
            );

            ys3.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void Switch_OuterThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(10, 101),
                OnNext(20, 102),
                OnNext(110, 103),
                OnNext(120, 104),
                OnNext(210, 105),
                OnNext(220, 106),
                OnCompleted<int>(230)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(10, 201),
                OnNext(20, 202),
                OnNext(30, 203),
                OnNext(40, 204),
                OnCompleted<int>(50)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(300, ys1),
                OnNext<IObservable<int>>(400, ys2),
                OnError<IObservable<int>>(500, ex)
            );

            var res = scheduler.Start(() =>
                xs.Switch()
            );

            res.Messages.AssertEqual(
                OnNext(310, 101),
                OnNext(320, 102),
                OnNext(410, 201),
                OnNext(420, 202),
                OnNext(430, 203),
                OnNext(440, 204),
                OnError<int>(500, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );

            ys1.Subscriptions.AssertEqual(
                Subscribe(300, 400)
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys2.Subscriptions.AssertEqual(
                Subscribe(400, 450)
            );
#else
            ys2.Subscriptions.AssertEqual(
                Subscribe(400, 500)
            );
#endif
        }

        [Fact]
        public void Switch_NoInner()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<IObservable<int>>(500)
            );

            var res = scheduler.Start(() =>
                xs.Switch()
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [Fact]
        public void Switch_InnerCompletes()
        {
            var scheduler = new TestScheduler();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(10, 101),
                OnNext(20, 102),
                OnNext(110, 103),
                OnNext(120, 104),
                OnNext(210, 105),
                OnNext(220, 106),
                OnCompleted<int>(230)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(300, ys1),
                OnCompleted<IObservable<int>>(540)
            );

            var res = scheduler.Start(() =>
                xs.Switch()
            );

            res.Messages.AssertEqual(
                OnNext(310, 101),
                OnNext(320, 102),
                OnNext(410, 103),
                OnNext(420, 104),
                OnNext(510, 105),
                OnNext(520, 106),
                OnCompleted<int>(540)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 540)
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys1.Subscriptions.AssertEqual(
                Subscribe(300, 530)
            );
#else
            ys1.Subscriptions.AssertEqual(
                Subscribe(300, 540)
            );
#endif
        }

        [Fact]
        public void Switch_Task()
        {
            var tss = Observable.Switch(new[] { Task.Factory.StartNew(() => 1), Task.Factory.StartNew(() => 2), Task.Factory.StartNew(() => 3) }.ToObservable());

            var res = tss.ToArray().Single();

            Assert.True(res.Zip(res.Skip(1), (l, r) => r > l).All(b => b));
        }

    }
}
