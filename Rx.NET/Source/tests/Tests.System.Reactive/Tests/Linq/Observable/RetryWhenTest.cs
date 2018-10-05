// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class RetryWhenTest : ReactiveTest
    {
        [Fact]
        public void RetryWhen_Observable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RetryWhen<int, Exception>(null, v => v));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RetryWhen<int, Exception>(Observable.Return(1), null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.RetryWhen(v => v).Subscribe(null));
        }

        [Fact]
        public void RetryWhen_Observable_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.RetryWhen(v => v)
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3),
                OnCompleted<int>(450)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [Fact]
        public void RetryWhen_Observable_Handler_Completes()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.RetryWhen(v => v.Take(1).Skip(1))
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3),
                OnCompleted<int>(450)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }


        [Fact]
        public void RetryWhen_Observable_Handler_Throws()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Return(1).RetryWhen<int, int>(v => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [Fact]
        public void RetryWhen_Observable_Handler_Errors()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();
            var ex2 = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.RetryWhen(v => v.SelectMany(w => Observable.Throw<int>(ex2)))
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3),
                OnError<int>(450, ex2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [Fact]
        public void RetryWhen_Observable_RetryCount_Basic()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(5, 1),
                OnNext(10, 2),
                OnNext(15, 3),
                OnError<int>(20, ex)
            );

            var res = scheduler.Start(() =>
                xs.RetryWhen(v =>
                {
                    int[] count = { 0 };
                    return v.SelectMany(w =>
                    {
                        var c = ++count[0];
                        if (c == 3)
                        {
                            return Observable.Throw<int>(w);
                        }
                        return Observable.Return(1);
                    });
                })
            );

            res.Messages.AssertEqual(
                OnNext(205, 1),
                OnNext(210, 2),
                OnNext(215, 3),
                OnNext(225, 1),
                OnNext(230, 2),
                OnNext(235, 3),
                OnNext(245, 1),
                OnNext(250, 2),
                OnNext(255, 3),
                OnError<int>(260, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220),
                Subscribe(220, 240),
                Subscribe(240, 260)
            );
        }

        [Fact]
        public void RetryWhen_Observable_RetryCount_Delayed()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(5, 1),
                OnNext(10, 2),
                OnNext(15, 3),
                OnError<int>(20, ex)
            );

            var res = scheduler.Start(() =>
                xs.RetryWhen(v =>
                {
                    int[] count = { 0 };
                    return v.SelectMany(w =>
                    {
                        var c = ++count[0];
                        if (c == 3)
                        {
                            return Observable.Throw<int>(w);
                        }
                        return Observable.Return(1).Delay(TimeSpan.FromTicks(c * 100), scheduler);
                    });
                })
            );

            res.Messages.AssertEqual(
                OnNext(205, 1),
                OnNext(210, 2),
                OnNext(215, 3),
                OnNext(325, 1),
                OnNext(330, 2),
                OnNext(335, 3),
                OnNext(545, 1),
                OnNext(550, 2),
                OnNext(555, 3),
                OnError<int>(560, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220),
                Subscribe(320, 340),
                Subscribe(540, 560)
            );
        }
    }
}
