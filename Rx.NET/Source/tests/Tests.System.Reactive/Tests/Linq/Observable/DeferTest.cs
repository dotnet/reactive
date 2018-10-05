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
    public class DeferTest : ReactiveTest
    {

        [Fact]
        public void Defer_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Defer(default(Func<IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Defer(() => DummyObservable<int>.Instance).Subscribe(null));
            ReactiveAssert.Throws</*some*/Exception>(() => Observable.Defer(() => default(IObservable<int>)).Subscribe());
        }

        [Fact]
        public void Defer_Complete()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var xs = default(ITestableObservable<long>);

            var res = scheduler.Start(() =>
                Observable.Defer(() =>
                {
                    invoked++;
                    xs = scheduler.CreateColdObservable(
                        OnNext(100, scheduler.Clock),
                        OnCompleted<long>(200));
                    return xs;
                })
            );

            res.Messages.AssertEqual(
                OnNext(300, 200L),
                OnCompleted<long>(400)
            );

            Assert.Equal(1, invoked);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Defer_Error()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var xs = default(ITestableObservable<long>);
            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Defer(() =>
                {
                    invoked++;
                    xs = scheduler.CreateColdObservable(
                        OnNext(100, scheduler.Clock),
                        OnError<long>(200, ex));
                    return xs;
                })
            );

            res.Messages.AssertEqual(
                OnNext(300, 200L),
                OnError<long>(400, ex)
            );

            Assert.Equal(1, invoked);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Defer_Dispose()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var xs = default(ITestableObservable<long>);

            var res = scheduler.Start(() =>
                Observable.Defer(() =>
                {
                    invoked++;
                    xs = scheduler.CreateColdObservable(
                        OnNext(100, scheduler.Clock),
                        OnNext<long>(200, invoked),
                        OnNext<long>(1100, 1000));
                    return xs;
                })
            );

            res.Messages.AssertEqual(
                OnNext(300, 200L),
                OnNext(400, 1L)
            );

            Assert.Equal(1, invoked);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Defer_Throw()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Defer(new Func<IObservable<int>>(() =>
                {
                    invoked++;
                    throw ex;
                }))
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            Assert.Equal(1, invoked);
        }

    }
}
