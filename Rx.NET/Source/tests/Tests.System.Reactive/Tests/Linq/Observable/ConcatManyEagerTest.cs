// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class ConcatManyEagerTest : ReactiveTest
    {
        [Fact]
        public void ConcatEager_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ConcatEager(default(IObservable<IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.ConcatEager(Observable.Return(Observable.Return(1)), maxConcurrency: 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.ConcatEager(Observable.Return(Observable.Return(1)), maxConcurrency: -1));
        }

        [Fact]
        public void ConcatEager_All_Basic()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(10, 4),
                OnNext(20, 5),
                OnCompleted<int>(30)
            );

            var xs3 = scheduler.CreateColdObservable<int>(
                OnNext(10, 6),
                OnNext(20, 7),
                OnNext(30, 8),
                OnNext(40, 9),
                OnCompleted<int>(50)
            );

            var res = scheduler.Start(() =>
                Observable.ConcatEager(new[] { xs1, xs2, xs3 }.ToObservable())
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(240, 5),
                OnNext(240, 6),
                OnNext(240, 7),
                OnNext(240, 8),
                OnNext(240, 9),
                OnCompleted<int>(250)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            xs3.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void ConcatEager_All_Error()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var ex = new InvalidOperationException();

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(5, 4),
                OnError<int>(15, ex)
            );

            var res = scheduler.Start(() =>
                Observable.ConcatEager(new[] { xs1, xs2 }.ToObservable())
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnError<int>(215, ex)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [Fact]
        public void ConcatEager_All_Delay_Error()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var ex = new InvalidOperationException();

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(10, 4),
                OnError<int>(15, ex)
            );

            var res = scheduler.Start(() =>
                Observable.ConcatEager(new[] { xs1, xs2 }.ToObservable(), delayErrors: true)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnError<int>(240, ex)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [Fact]
        public void ConcatEager_All_Delay_Error_First()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var ex = new InvalidOperationException();

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(10, 4),
                OnError<int>(15, ex)
            );

            var res = scheduler.Start(() =>
                Observable.ConcatEager(new[] { xs2, xs1 }.ToObservable(), delayErrors: true)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(215, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnError<int>(240, ex)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [Fact]
        public void ConcatEager_All_Empty()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnCompleted<int>(10)
            );

            var xs2 = scheduler.CreateColdObservable<int>(
                OnCompleted<int>(10)
            );

            var res = scheduler.Start(() =>
                Observable.ConcatEager(new[] { xs1, xs2 }.ToObservable())
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(210)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void ConcatEager_All_Dispose()
        {
            var subj1 = new Subject<int>();
            var subj2 = new Subject<int>();

            using (new[] { subj1, subj2 }.ToObservable().ConcatEager().Subscribe())
            {
                Assert.True(subj1.HasObservers);
                Assert.True(subj2.HasObservers);
            }

            Assert.False(subj1.HasObservers);
            Assert.False(subj2.HasObservers);
        }

        [Fact]
        public void ConcatEager_Some_Basic_1()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(10, 4),
                OnNext(20, 5),
                OnCompleted<int>(30)
            );

            var xs3 = scheduler.CreateColdObservable<int>(
                OnNext(10, 6),
                OnNext(20, 7),
                OnNext(30, 8),
                OnNext(40, 9),
                OnCompleted<int>(50)
            );

            var res = scheduler.Start(() =>
                Observable.ConcatEager(new[] { xs1, xs2, xs3 }.ToObservable(), maxConcurrency: 1)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(250, 4),
                OnNext(260, 5),
                OnNext(280, 6),
                OnNext(290, 7),
                OnNext(300, 8),
                OnNext(310, 9),
                OnCompleted<int>(320)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(240, 270)
            );

            xs3.Subscriptions.AssertEqual(
                Subscribe(270, 320)
            );
        }

        [Fact]
        public void ConcatEager_Some_Basic_3()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(10, 4),
                OnNext(20, 5),
                OnCompleted<int>(30)
            );

            var xs3 = scheduler.CreateColdObservable<int>(
                OnNext(10, 6),
                OnNext(20, 7),
                OnNext(30, 8),
                OnNext(40, 9),
                OnCompleted<int>(50)
            );

            var res = scheduler.Start(() =>
                Observable.ConcatEager(new[] { xs1, xs2, xs3 }.ToObservable(), maxConcurrency: 3)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(240, 5),
                OnNext(240, 6),
                OnNext(240, 7),
                OnNext(240, 8),
                OnNext(240, 9),
                OnCompleted<int>(250)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            xs3.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void ConcatEager_Some_Error_1()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var ex = new InvalidOperationException();

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(5, 4),
                OnError<int>(15, ex)
            );

            var res = scheduler.Start(() =>
                Observable.ConcatEager(new[] { xs1, xs2 }.ToObservable(), maxConcurrency: 1)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(245, 4),
                OnError<int>(255, ex)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(240, 255)
            );
        }

        [Fact]
        public void ConcatEager_Some_Error_2()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var ex = new InvalidOperationException();

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(5, 4),
                OnError<int>(15, ex)
            );

            var res = scheduler.Start(() =>
                Observable.ConcatEager(new[] { xs1, xs2 }.ToObservable(), maxConcurrency: 2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnError<int>(215, ex)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [Fact]
        public void ConcatEager_Some_Delay_Error()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var ex = new InvalidOperationException();

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(10, 4),
                OnError<int>(15, ex)
            );

            var res = scheduler.Start(() =>
                Observable.ConcatEager(new[] { xs1, xs2 }.ToObservable(), delayErrors: true, maxConcurrency: 2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnError<int>(240, ex)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [Fact]
        public void ConcatEager_Some_Delay_Error_First()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var ex = new InvalidOperationException();

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(10, 4),
                OnError<int>(15, ex)
            );

            var res = scheduler.Start(() =>
                Observable.ConcatEager(new[] { xs2, xs1 }.ToObservable(), delayErrors: true, maxConcurrency: 2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(215, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnError<int>(240, ex)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [Fact]
        public void ConcatEager_Some_Empty_1()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnCompleted<int>(10)
            );

            var xs2 = scheduler.CreateColdObservable<int>(
                OnCompleted<int>(10)
            );

            var res = scheduler.Start(() =>
                Observable.ConcatEager(new[] { xs1, xs2 }.ToObservable(), maxConcurrency: 1)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(220)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(210, 220)
            );
        }

        [Fact]
        public void ConcatEager_Some_Empty_2()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnCompleted<int>(10)
            );

            var xs2 = scheduler.CreateColdObservable<int>(
                OnCompleted<int>(10)
            );

            var res = scheduler.Start(() =>
                Observable.ConcatEager(new[] { xs1, xs2 }.ToObservable(), maxConcurrency: 2)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(210)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void ConcatEager_Some_Dispose_1()
        {
            var subj1 = new Subject<int>();
            var subj2 = new Subject<int>();

            using (new[] { subj1, subj2 }.ToObservable().ConcatEager(maxConcurrency: 1).Subscribe())
            {
                Assert.True(subj1.HasObservers);
                Assert.False(subj2.HasObservers);
            }

            Assert.False(subj1.HasObservers);
            Assert.False(subj2.HasObservers);
        }

        [Fact]
        public void ConcatEager_Some_Dispose_2()
        {
            var subj1 = new Subject<int>();
            var subj2 = new Subject<int>();

            using (new[] { subj1, subj2 }.ToObservable().ConcatEager(maxConcurrency: 2).Subscribe())
            {
                Assert.True(subj1.HasObservers);
                Assert.True(subj2.HasObservers);
            }

            Assert.False(subj1.HasObservers);
            Assert.False(subj2.HasObservers);
        }

        [Fact]
        public void ConcatEager_Some_Switch_Over_1()
        {
            var subj1 = new Subject<int>();
            var subj2 = new Subject<int>();

            using (new[] { subj1, subj2 }.ToObservable().ConcatEager(maxConcurrency: 1).Subscribe())
            {
                Assert.True(subj1.HasObservers);
                Assert.False(subj2.HasObservers);

                subj1.OnCompleted();

                Assert.True(subj2.HasObservers);
            }

            Assert.False(subj1.HasObservers);
            Assert.False(subj2.HasObservers);
        }

        [Fact]
        public void ConcatManyEager_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ConcatManyEager(default(IObservable<IObservable<int>>), v => v));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ConcatManyEager<int, int>(Observable.Return(1), null));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.ConcatManyEager(Observable.Return(Observable.Return(1)), v => v, maxConcurrency: 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.ConcatManyEager(Observable.Return(Observable.Return(1)), v => v, maxConcurrency: -1));
        }

        [Fact]
        public void ConcatManyEager_All_Basic()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(10, 4),
                OnNext(20, 5),
                OnCompleted<int>(30)
            );

            var xs3 = scheduler.CreateColdObservable<int>(
                OnNext(10, 6),
                OnNext(20, 7),
                OnNext(30, 8),
                OnNext(40, 9),
                OnCompleted<int>(50)
            );

            var sources = new[] { xs1, xs2, xs3 };

            var res = scheduler.Start(() =>
                Observable.Range(0, 3).ConcatManyEager(v => sources[v])
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(240, 5),
                OnNext(240, 6),
                OnNext(240, 7),
                OnNext(240, 8),
                OnNext(240, 9),
                OnCompleted<int>(250)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            xs3.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void ConcatManyEager_All_Mapper_Crash()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var ex = new InvalidOperationException();

            var res = scheduler.Start(() =>
                Observable.Range(0, 3).ConcatManyEager(v => 
                {
                    if (v == 1)
                    {
                        throw ex;
                    }
                    return xs1;
                })
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 200)
            );

        }

        [Fact]
        public void ConcatManyEager_All_Mapper_Crash_DelayError()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var ex = new InvalidOperationException();

            var res = scheduler.Start(() =>
                Observable.Range(0, 3).ConcatManyEager(v => 
                {
                    if (v == 1)
                    {
                        throw ex;
                    }
                    return xs1;
                }, delayErrors: true)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnError<int>(240, ex)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void ConcatManyEager_All_Async()
        {
            var list = new List<int>(20000);
            var error = default(Exception);
            var mre = new CountdownEvent(1);

            Observable.Range(1, 100)
                .ConcatManyEager(v => Observable.Range(1, 200).SubscribeOn(TaskPoolScheduler.Default))
                .Subscribe(v => list.Add(v), e =>
                {
                    error = e;
                    mre.Signal();
                }, () => mre.Signal());

            Assert.True(mre.Wait(5000), "Timeout!");

            Assert.Equal(20000, list.Count);
            Assert.Null(error);

            var idx = 0;
            for (var j = 1; j <= 100; j++)
            {
                for (var k = 1; k <= 200; k++)
                {
                    Assert.Equal(k, list[idx++]);
                }
            }
        }

        [Fact]
        public void ConcatManyEager_Some_Basic_1()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(10, 4),
                OnNext(20, 5),
                OnCompleted<int>(30)
            );

            var xs3 = scheduler.CreateColdObservable<int>(
                OnNext(10, 6),
                OnNext(20, 7),
                OnNext(30, 8),
                OnNext(40, 9),
                OnCompleted<int>(50)
            );

            var sources = new[] { xs1, xs2, xs3 };

            var res = scheduler.Start(() =>
                Observable.Range(0, 3).ConcatManyEager(v => sources[v], maxConcurrency: 1)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(250, 4),
                OnNext(260, 5),
                OnNext(280, 6),
                OnNext(290, 7),
                OnNext(300, 8),
                OnNext(310, 9),
                OnCompleted<int>(320)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(240, 270)
            );

            xs3.Subscriptions.AssertEqual(
                Subscribe(270, 320)
            );
        }

        [Fact]
        public void ConcatManyEager_Some_Basic_3()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(10, 4),
                OnNext(20, 5),
                OnCompleted<int>(30)
            );

            var xs3 = scheduler.CreateColdObservable<int>(
                OnNext(10, 6),
                OnNext(20, 7),
                OnNext(30, 8),
                OnNext(40, 9),
                OnCompleted<int>(50)
            );

            var sources = new[] { xs1, xs2, xs3 };

            var res = scheduler.Start(() =>
                Observable.Range(0, 3).ConcatManyEager(v => sources[v], maxConcurrency: 3)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(240, 5),
                OnNext(240, 6),
                OnNext(240, 7),
                OnNext(240, 8),
                OnNext(240, 9),
                OnCompleted<int>(250)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            xs3.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void ConcatManyEager_Some_Mapper_Crash_1()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var ex = new InvalidOperationException();

            var res = scheduler.Start(() =>
                Observable.Range(0, 3).ConcatManyEager(v =>
                {
                    if (v == 1)
                    {
                        throw ex;
                    }
                    return xs1;
                }, maxConcurrency: 1)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnError<int>(240, ex)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

        }

        [Fact]
        public void ConcatManyEager_Some_Mapper_Crash_2()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var ex = new InvalidOperationException();

            var res = scheduler.Start(() =>
                Observable.Range(0, 3).ConcatManyEager(v =>
                {
                    if (v == 1)
                    {
                        throw ex;
                    }
                    return xs1;
                }, maxConcurrency: 2)
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 200)
            );

        }

        [Fact]
        public void ConcatManyEager_Some_Mapper_Crash_2_DelayError()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var ex = new InvalidOperationException();

            var res = scheduler.Start(() =>
                Observable.Range(0, 3).ConcatManyEager(v =>
                {
                    if (v == 1)
                    {
                        throw ex;
                    }
                    return xs1;
                }, delayErrors: true, maxConcurrency: 2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnError<int>(240, ex)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void ConcatManyEager_All_Crash_Disposes_Main()
        {
            var subj1 = new Subject<int>();

            subj1.ConcatManyEager<int, int>(v => throw new InvalidOperationException())
                .Subscribe(v => { }, e => { });

            Assert.True(subj1.HasObservers);

            subj1.OnNext(1);

            Assert.False(subj1.HasObservers);
        }

        [Fact]
        public void ConcatManyEager_Some_Crash_Disposes_Main()
        {
            var subj1 = new Subject<int>();

            subj1.ConcatManyEager<int, int>(v => throw new InvalidOperationException(), maxConcurrency: 1)
                .Subscribe(v => { }, e => { });

            Assert.True(subj1.HasObservers);

            subj1.OnNext(1);

            Assert.False(subj1.HasObservers);
        }

        [Fact]
        public void ConcatManyEager_Some_Async()
        {
            for (var i = 1; i < 17; i++)
            {
                var list = new List<int>(20000);
                var error = default(Exception);
                var mre = new CountdownEvent(1);

                Observable.Range(1, 100)
                    .ConcatManyEager(v => Observable.Range(1, 200).SubscribeOn(TaskPoolScheduler.Default), maxConcurrency: i)
                    .Subscribe(v => list.Add(v), e =>
                    {
                        error = e;
                        mre.Signal();
                    }, () => mre.Signal());

                Assert.True(mre.Wait(5000), "Timeout!");

                Assert.Equal(20000, list.Count);
                Assert.Null(error);

                var idx = 0;
                for (var j = 1; j <= 100; j++)
                {
                    for (var k = 1; k <= 200; k++)
                    {
                        Assert.Equal(k, list[idx++]);
                    }
                }
            }
        }
    }
}
