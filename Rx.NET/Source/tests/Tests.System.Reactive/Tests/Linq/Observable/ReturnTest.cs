// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class ReturnTest : ReactiveTest
    {

        [TestMethod]
        public void Return_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Return(0, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Return(0, DummyScheduler.Instance).Subscribe(null));
        }

        [TestMethod]
        public void Return_Basic()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Return(42, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 42),
                OnCompleted<int>(201)
            );
        }

        [TestMethod]
        public void Return_Disposed()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Return(42, scheduler),
                200
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void Return_DisposedAfterNext()
        {
            var scheduler = new TestScheduler();

            var d = new SerialDisposable();

            var xs = Observable.Return(42, scheduler);

            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(100, () =>
                d.Disposable = xs.Subscribe(
                    x =>
                    {
                        d.Dispose();
                        res.OnNext(x);
                    },
                    res.OnError,
                    res.OnCompleted
                )
            );

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(101, 42)
            );
        }

        [TestMethod]
        public void Return_ObserverThrows()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Return(1, scheduler1);

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());

            var scheduler2 = new TestScheduler();

            var ys = Observable.Return(1, scheduler2);

            ys.Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler2.Start());
        }

        [TestMethod]
        public void Return_DefaultScheduler()
        {
            Observable.Return(42).AssertEqual(Observable.Return(42, DefaultScheduler.Instance));
        }

    }
}
