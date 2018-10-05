// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class AmbTest : ReactiveTest
    {

        [Fact]
        public void Amb_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Amb((IObservable<int>[])null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Amb((IEnumerable<IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Amb(null, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Amb(DummyObservable<int>.Instance, null));
        }

        [Fact]
        public void Amb_Never2()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                l.Amb(r)
            );

            res.Messages.AssertEqual(
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Amb_Never3()
        {
            var scheduler = new TestScheduler();

            var n1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var n2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var n3 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                new[] { n1, n2, n3 }.Amb()
            );

            res.Messages.AssertEqual(
            );

            n1.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            n2.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            n3.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Amb_Never3_Params()
        {
            var scheduler = new TestScheduler();

            var n1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var n2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var n3 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                Observable.Amb(n1, n2, n3)
            );

            res.Messages.AssertEqual(
            );

            n1.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            n2.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            n3.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Amb_NeverEmpty()
        {
            var scheduler = new TestScheduler();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(225)
            );

            var res = scheduler.Start(() =>
                n.Amb(e)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(225)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void Amb_EmptyNever()
        {
            var scheduler = new TestScheduler();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(225)
            );

            var res = scheduler.Start(() =>
                e.Amb(n)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(225)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void Amb_RegularShouldDisposeLoser()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(240)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Amb(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(240)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Amb_WinnerThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(220, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Amb(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Amb_LoserThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Amb(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Amb_ThrowsBeforeElectionLeft()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Amb(o2)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Amb_ThrowsBeforeElectionRight()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                o1.Amb(o2)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Amb_Many_Array_OnNext()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateColdObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var o2 = scheduler.CreateColdObservable(
                OnNext(150, 2),
                OnError<int>(210, ex)
            );

            var o3 = scheduler.CreateColdObservable(
                OnCompleted<int>(150)
            );

            var res = scheduler.Start(() =>
                Observable.Amb(o1, o2, o3)
            );

            res.Messages.AssertEqual(
                OnNext(350, 1),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [Fact]
        public void Amb_Many_Array_OnError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateColdObservable(
                OnError<int>(150, ex)
            );

            var o2 = scheduler.CreateColdObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var o3 = scheduler.CreateColdObservable(
                OnCompleted<int>(150)
            );

            var res = scheduler.Start(() =>
                Observable.Amb(o1, o2, o3)
            );

            res.Messages.AssertEqual(
                OnError<int>(350, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [Fact]
        public void Amb_Many_Array_OnCompleted()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateColdObservable(
                OnCompleted<int>(150)
            );

            var o2 = scheduler.CreateColdObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var o3 = scheduler.CreateColdObservable(
                OnNext(150, 2),
                OnError<int>(210, ex)
            );


            var res = scheduler.Start(() =>
                Observable.Amb(o1, o2, o3)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(350)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }


        [Fact]
        public void Amb_Many_Enumerable_OnNext()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateColdObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var o2 = scheduler.CreateColdObservable(
                OnNext(150, 2),
                OnError<int>(210, ex)
            );

            var o3 = scheduler.CreateColdObservable(
                OnCompleted<int>(150)
            );

            var res = scheduler.Start(() =>
                new[] { o1, o2, o3 }.Amb()
            );

            res.Messages.AssertEqual(
                OnNext(350, 1),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [Fact]
        public void Amb_Many_Enumerable_OnError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateColdObservable(
                OnError<int>(150, ex)
            );

            var o2 = scheduler.CreateColdObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var o3 = scheduler.CreateColdObservable(
                OnCompleted<int>(150)
            );

            var res = scheduler.Start(() =>
                new[] { o1, o2, o3 }.Amb()
            );

            res.Messages.AssertEqual(
                OnError<int>(350, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [Fact]
        public void Amb_Many_Enumerable_OnCompleted()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateColdObservable(
                OnCompleted<int>(150)
            );

            var o2 = scheduler.CreateColdObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var o3 = scheduler.CreateColdObservable(
                OnNext(150, 2),
                OnError<int>(210, ex)
            );


            var res = scheduler.Start(() =>
                new[] { o1, o2, o3 }.Amb()
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(350)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }


        [Fact]
        public void Amb_Many_Enumerable_Many_Sources()
        {
            for (var i = 0; i < 32; i++)
            {
                var sources = new List<IObservable<int>>();
                for (var j = 0; j < i; j++)
                {
                    sources.Add(Observable.Return(j));
                }

                var result = sources.Amb().ToList().First();

                if (i == 0)
                {
                    Assert.Equal(0, result.Count);
                }
                else
                {
                    Assert.Equal(1, result.Count);
                    Assert.Equal(0, result[0]);
                }
            }
        }

        [Fact]
        public void Amb_Many_Enumerable_Many_Sources_NoStackOverflow()
        {
            for (var i = 0; i < 100; i++)
            {
                var sources = new List<IObservable<int>>();
                for (var j = 0; j < i; j++)
                {
                    if (j == i - 1)
                    {
                        sources.Add(Observable.Return(j));
                    }
                    else
                    {
                        sources.Add(Observable.Never<int>());
                    }
                }

                var result = sources.Amb().ToList().First();

                if (i == 0)
                {
                    Assert.Equal(0, result.Count);
                }
                else
                {
                    Assert.Equal(1, result.Count);
                    Assert.Equal(i - 1, result[0]);
                }
            }
        }
    }
}
