// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class ThrowTest : ReactiveTest
    {

        [Fact]
        public void Throw_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throw<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throw(null, 42));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throw<int>(new Exception(), null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throw(new Exception(), null, 42));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throw<int>(null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throw(null, DummyScheduler.Instance, 42));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throw<int>(new Exception(), DummyScheduler.Instance).Subscribe(null));
        }

        [Fact]
        public void Throw_Basic()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Throw<int>(ex, scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [Fact]
        public void Throw_Disposed()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Throw<int>(new Exception(), scheduler),
                200
            );

            res.Messages.AssertEqual(
            );
        }

        [Fact]
        public void Throw_ObserverThrows()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Throw<int>(new Exception(), scheduler1);

            xs.Subscribe(x => { }, ex => { throw new InvalidOperationException(); }, () => { });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());
        }

        [Fact]
        public void Throw_DefaultScheduler()
        {
            var ex = new Exception();
            Observable.Throw<int>(ex).AssertEqual(Observable.Throw<int>(ex, DefaultScheduler.Instance));
        }

        [Fact]
        public void Throw_Witness_Basic1()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Throw(ex, scheduler, 42)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [Fact]
        public void Throw_Witness_Basic2()
        {
            var e = new ManualResetEvent(false);

            var ex = new Exception();

            var res = default(Exception);

            Observable.Throw(ex, 42).Subscribe(
                _ => { Assert.True(false); },
                err => { res = err; e.Set(); },
                () => { Assert.True(false); }
            );

            e.WaitOne();

            Assert.Same(ex, res);
        }

    }
}
