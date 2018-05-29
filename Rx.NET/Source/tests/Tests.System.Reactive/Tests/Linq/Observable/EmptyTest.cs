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
    public class EmptyTest : ReactiveTest
    {

        [Fact]
        public void Empty_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Empty<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Empty<int>(null, 42));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Empty<int>(DummyScheduler.Instance).Subscribe(null));
        }

        [Fact]
        public void Empty_Basic()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Empty<int>(scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(201)
            );
        }

        [Fact]
        public void Empty_Disposed()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Empty<int>(scheduler),
                200
            );

            res.Messages.AssertEqual(
            );
        }

        [Fact]
        public void Empty_ObserverThrows()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Empty<int>(scheduler1);

            xs.Subscribe(x => { }, exception => { }, () => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());
        }

        [Fact]
        public void Empty_DefaultScheduler()
        {
            Observable.Empty<int>().AssertEqual(Observable.Empty<int>(DefaultScheduler.Instance));
        }

        [Fact]
        public void Empty_Basic_Witness1()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Empty<int>(scheduler, 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(201)
            );
        }

        [Fact]
        public void Empty_Basic_Witness2()
        {
            var e = new ManualResetEvent(false);

            Observable.Empty<int>(42).Subscribe(
                _ => { Assert.True(false); },
                _ => { Assert.True(false); },
                () => e.Set()
            );

            e.WaitOne();
        }

    }
}
