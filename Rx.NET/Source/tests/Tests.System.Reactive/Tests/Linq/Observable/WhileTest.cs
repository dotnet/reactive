// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class WhileTest : ReactiveTest
    {

        [Fact]
        public void While_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.While(default, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.While(DummyFunc<bool>.Instance, default(IObservable<int>)));
        }

        [Fact]
        public void While_AlwaysFalse()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            var results = scheduler.Start(() => Observable.While(() => false, xs));

            results.Messages.AssertEqual(
                OnCompleted<int>(200)
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void While_AlwaysTrue()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            var results = scheduler.Start(() => Observable.While(() => true, xs));

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
        public void While_AlwaysTrue_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnError<int>(50, ex)
            );

            var results = scheduler.Start(() => Observable.While(() => true, xs));

            results.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void While_AlwaysTrue_Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1)
            );

            var results = scheduler.Start(() => Observable.While(() => true, xs));

            results.Messages.AssertEqual(
                OnNext(250, 1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void While_SometimesTrue()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            var n = 0;

            var results = scheduler.Start(() => Observable.While(() => ++n < 3, xs));

            results.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnNext(500, 1),
                OnNext(550, 2),
                OnNext(600, 3),
                OnNext(650, 4),
                OnCompleted<int>(700)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450),
                Subscribe(450, 700)
            );
        }

        private static T Throw<T>(Exception ex)
        {
            throw ex;
        }

        [Fact]
        public void While_SometimesThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            var n = 0;

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.While(() => ++n < 3 ? true : Throw<bool>(ex), xs));

            results.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnNext(500, 1),
                OnNext(550, 2),
                OnNext(600, 3),
                OnNext(650, 4),
                OnError<int>(700, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450),
                Subscribe(450, 700)
            );
        }

        #region General tests for loops
#if HAS_STACKTRACE
        [Fact]
        public void LoopTest1()
        {
            var loop = Observable.Defer(() =>
            {
                var n = 0;
                return Observable.While(
                    () => n++ < 5,
                    Observable.Defer(() =>
                    {
                        return Observable.For(
                            Enumerable.Range(0, n),
                            x => Observable.Return(x)
                        );
                    })
                );
            });

            var res = new List<int>();
            var std = new List<int>();
            loop.ForEach(x =>
            {
                res.Add(x);
                std.Add(new System.Diagnostics.StackTrace().FrameCount);
            });

            Assert.True(res.SequenceEqual(new[] { 0, 0, 1, 0, 1, 2, 0, 1, 2, 3, 0, 1, 2, 3, 4 }));
            Assert.True(std.Distinct().Count() == 1);
        }

        [Fact]
        public void LoopTest2()
        {
            var n = 0;

            var loop = default(IObservable<int>);
            loop = Observable.While(
                () => n++ < 10,
                Observable.Concat(
                    Observable.Return(42),
                    Observable.Defer(() => loop)
                )
            );

            var res = new List<int>();
            var std = new List<int>();
            loop.ForEach(x =>
            {
                res.Add(x);
                std.Add(new System.Diagnostics.StackTrace().FrameCount);
            });

            Assert.True(res.SequenceEqual(Enumerable.Repeat(42, 10)));
            Assert.True(std.Distinct().Count() == 1);
        }

#endif
        #endregion
    }
}
