// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;

using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class StartTest : ReactiveTest
    {

        [TestMethod]
        public void Start_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Start(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Start<int>(null));

            var someScheduler = new TestScheduler();
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Start(null, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Start<int>(null, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Start(() => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Start(() => 1, null));
        }


        [TestMethod]
        public void Start_Action()
        {
            var done = false;
            Assert.True(Observable.Start(() => { done = true; }).ToEnumerable().SequenceEqual([new Unit()]));
            Assert.True(done, "done");
        }

        [TestMethod]
        public void Start_Action2()
        {
            var scheduler = new TestScheduler();

            var done = false;

            var res = scheduler.Start(() =>
                Observable.Start(() => { done = true; }, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(200, new Unit()),
                OnCompleted<Unit>(200)
            );

            Assert.True(done, "done");
        }

        [TestMethod]
        public void Start_ActionError()
        {
            var ex = new Exception();

            var res = Observable.Start(() => { throw ex; }).Materialize().ToEnumerable();

            Assert.True(res.SequenceEqual([
                Notification.CreateOnError<Unit>(ex)
            ]));
        }

        [TestMethod]
        public void Start_ActionErrorAfterUnsubscribeDoesNotThrow()
        {
            var ex = new Exception();
            using Barrier gate = new(2);

            var sub = Observable.Start(
                () =>
                {
                    // 1: action running
                    gate.SignalAndWait();
                    // 2: unsubscribe Dispose returned
                    gate.SignalAndWait();
                    throw ex;
                })
                .Subscribe();

            // 1: action running
            gate.SignalAndWait();

            sub.Dispose();

            // 2: unsubscribe Dispose returned
            gate.SignalAndWait();
        }

        [TestMethod]
        public void Start_Func()
        {
            var res = Observable.Start(() => 1).ToEnumerable();

            Assert.True(res.SequenceEqual([
                1
            ]));
        }

        [TestMethod]
        public void Start_Func2()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Start(() => 1, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnCompleted<int>(200)
            );
        }

        [TestMethod]
        public void Start_FuncError()
        {
            var ex = new Exception();

            var res = Observable.Start<int>(() => { throw ex; }).Materialize().ToEnumerable();

            Assert.True(res.SequenceEqual([
                Notification.CreateOnError<int>(ex)
            ]));
        }

    }
}
