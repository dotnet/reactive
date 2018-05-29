// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;
using ReactiveTests.Dummies;
using System.Reflection;
using System.Threading;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace ReactiveTests.Tests
{
    public class FinallyTest : ReactiveTest
    {

        [Fact]
        public void Finally_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Finally<int>(null, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Finally<int>(someObservable, null));
        }

        [Fact]
        public void Finally_Never()
        {
            var scheduler = new TestScheduler();

            bool invoked = false;
            var res = scheduler.Start(() =>
                Observable.Never<int>().Finally(() => { invoked = true; })
            );

            res.Messages.AssertEqual(
            );

            Assert.True(invoked); // due to unsubscribe; see 1356
        }

        [Fact]
        public void Finally_OnlyCalledOnce_Never()
        {
            int invokeCount = 0;
            var someObservable = Observable.Never<int>().Finally(() => { invokeCount++; });
            var d = someObservable.Subscribe();
            d.Dispose();
            d.Dispose();

            Assert.Equal(1, invokeCount);
        }

        [Fact]
        public void Finally_OnlyCalledOnce_Empty()
        {
            var invokeCount = 0;
            var someObservable = Observable.Empty<int>().Finally(() => { invokeCount++; });
            var d = someObservable.Subscribe();
            d.Dispose();
            d.Dispose();

            Assert.Equal(1, invokeCount);
        }

        [Fact]
        public void Finally_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var invoked = false;
            var res = scheduler.Start(() =>
                xs.Finally(() => { invoked = true; })
            );

            Assert.True(invoked);

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Finally_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var invoked = false;
            var res = scheduler.Start(() =>
                xs.Finally(() => { invoked = true; })
            );

            Assert.True(invoked);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Finally_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();
            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var invoked = false;
            var res = scheduler.Start(() =>
                xs.Finally(() => { invoked = true; })
            );

            Assert.True(invoked);

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

    }
}
