﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class OfTypeTest : ReactiveTest
    {

        [TestMethod]
        public void OfType_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OfType<bool>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OfType<bool>(DummyObservable<object>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void OfType_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext<object>(210, new B(0)),
                OnNext<object>(220, new A(1)),
                OnNext<object>(230, new E(2)),
                OnNext<object>(240, new D(3)),
                OnNext<object>(250, new C(4)),
                OnNext<object>(260, new B(5)),
                OnNext<object>(270, new B(6)),
                OnNext<object>(280, new D(7)),
                OnNext<object>(290, new A(8)),
                OnNext<object>(300, new E(9)),
                OnNext<object>(310, 3),
                OnNext<object>(320, "foo"),
                OnNext<object>(330, true),
                OnNext<object>(340, new B(10)),
                OnCompleted<object>(350)
            );

            var res = scheduler.Start(() =>
                xs.OfType<B>()
            );

            res.Messages.AssertEqual(
                OnNext(210, new B(0)),
                OnNext<B>(240, new D(3)),
                OnNext(260, new B(5)),
                OnNext(270, new B(6)),
                OnNext<B>(280, new D(7)),
                OnNext(340, new B(10)),
                OnCompleted<B>(350)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

#nullable enable
        [TestMethod]
        public void OfType_NullableSourceOfTypeNonNull()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext<A?>(210, new B(0)),
                OnNext<A?>(220, new A(1)),
                OnNext<A?>(230, default(A?)),
                OnNext<A?>(240, new D(3)),
                OnNext<A?>(250, new C(4)),
                OnNext<A?>(260, new B(5)),
                OnNext<A?>(270, new B(6)),
                OnNext<A?>(280, new D(7)),
                OnNext<A?>(290, new A(8)),
                OnNext<A?>(340, new B(10)),
                OnCompleted<A?>(350)
            );

            var res = scheduler.Start(() =>
                xs.OfType<A>()
            );

            res.Messages.AssertEqual(
                OnNext<A>(210, new B(0)),
                OnNext<A>(220, new A(1)),
                OnNext<A>(240, new D(3)),
                OnNext<A>(250, new C(4)),
                OnNext<A>(260, new B(5)),
                OnNext<A>(270, new B(6)),
                OnNext<A>(280, new D(7)),
                OnNext<A>(290, new A(8)),
                OnNext<A>(340, new B(10)),
                OnCompleted<A>(350)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }
#nullable restore

        [TestMethod]
        public void OfType_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext<object>(210, new B(0)),
                OnNext<object>(220, new A(1)),
                OnNext<object>(230, new E(2)),
                OnNext<object>(240, new D(3)),
                OnNext<object>(250, new C(4)),
                OnNext<object>(260, new B(5)),
                OnNext<object>(270, new B(6)),
                OnNext<object>(280, new D(7)),
                OnNext<object>(290, new A(8)),
                OnNext<object>(300, new E(9)),
                OnNext<object>(310, 3),
                OnNext<object>(320, "foo"),
                OnNext<object>(330, true),
                OnNext<object>(340, new B(10)),
                OnError<object>(350, ex)
            );

            var res = scheduler.Start(() =>
                xs.OfType<B>()
            );

            res.Messages.AssertEqual(
                OnNext(210, new B(0)),
                OnNext<B>(240, new D(3)),
                OnNext(260, new B(5)),
                OnNext(270, new B(6)),
                OnNext<B>(280, new D(7)),
                OnNext(340, new B(10)),
                OnError<B>(350, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [TestMethod]
        public void OfType_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext<object>(210, new B(0)),
                OnNext<object>(220, new A(1)),
                OnNext<object>(230, new E(2)),
                OnNext<object>(240, new D(3)),
                OnNext<object>(250, new C(4)),
                OnNext<object>(260, new B(5)),
                OnNext<object>(270, new B(6)),
                OnNext<object>(280, new D(7)),
                OnNext<object>(290, new A(8)),
                OnNext<object>(300, new E(9)),
                OnNext<object>(310, 3),
                OnNext<object>(320, "foo"),
                OnNext<object>(330, true),
                OnNext<object>(340, new B(10)),
                OnError<object>(350, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.OfType<B>(),
                275
            );

            res.Messages.AssertEqual(
                OnNext(210, new B(0)),
                OnNext<B>(240, new D(3)),
                OnNext(260, new B(5)),
                OnNext(270, new B(6))
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 275)
            );
        }

    }
}
