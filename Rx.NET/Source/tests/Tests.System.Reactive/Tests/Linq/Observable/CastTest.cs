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
    public class CastTest : ReactiveTest
    {

        [Fact]
        public void Cast_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Cast<bool>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Cast<bool>(DummyObservable<object>.Instance).Subscribe(null));
        }

        [Fact]
        public void Cast_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext<object>(210, new B(0)),
                OnNext<object>(220, new D(1)),
                OnNext<object>(240, new B(2)),
                OnNext<object>(270, new D(3)),
                OnCompleted<object>(300)
            );

            var res = scheduler.Start(() =>
                xs.Cast<B>()
            );

            res.Messages.AssertEqual(
                OnNext(210, new B(0)),
                OnNext<B>(220, new D(1)),
                OnNext(240, new B(2)),
                OnNext<B>(270, new D(3)),
                OnCompleted<B>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void Cast_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext<object>(210, new B(0)),
                OnNext<object>(220, new D(1)),
                OnNext<object>(240, new B(2)),
                OnNext<object>(270, new D(3)),
                OnError<object>(300, ex)
            );

            var res = scheduler.Start(() =>
                xs.Cast<B>()
            );

            res.Messages.AssertEqual(
                OnNext(210, new B(0)),
                OnNext<B>(220, new D(1)),
                OnNext(240, new B(2)),
                OnNext<B>(270, new D(3)),
                OnError<B>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void Cast_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext<object>(210, new B(0)),
                OnNext<object>(220, new D(1)),
                OnNext<object>(240, new B(2)),
                OnNext<object>(270, new D(3)),
                OnCompleted<object>(300)
            );

            var res = scheduler.Start(() =>
                xs.Cast<B>(),
                250
            );

            res.Messages.AssertEqual(
                OnNext(210, new B(0)),
                OnNext<B>(220, new D(1)),
                OnNext(240, new B(2))
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Cast_NotValid()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext<object>(210, new B(0)),
                OnNext<object>(220, new D(1)),
                OnNext<object>(240, new B(2)),
                OnNext<object>(250, new A(-1)),
                OnNext<object>(270, new D(3)),
                OnCompleted<object>(300)
            );

            var res = scheduler.Start(() =>
                xs.Cast<B>()
            );

            res.Messages.AssertEqual(
                OnNext(210, new B(0)),
                OnNext<B>(220, new D(1)),
                OnNext(240, new B(2)),
                OnError<B>(250, e => e is InvalidCastException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }
    }

    internal class A : IEquatable<A>
    {
        private int _id;

        public A(int id)
        {
            _id = id;
        }

        public bool Equals(A other)
        {
            if (other == null)
            {
                return false;
            }

            return _id == other._id && GetType().Equals(other.GetType());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as A);
        }

        public override int GetHashCode()
        {
            return _id;
        }
    }

    internal class B : A
    {
        public B(int id)
            : base(id)
        {
        }
    }

    internal class C : A
    {
        public C(int id)
            : base(id)
        {
        }
    }

    internal class D : B
    {
        public D(int id)
            : base(id)
        {
        }
    }

    internal class E : IEquatable<E>
    {
        private int _id;

        public E(int id)
        {
            _id = id;
        }

        public bool Equals(E other)
        {
            if (other == null)
            {
                return false;
            }

            return _id == other._id && GetType().Equals(other.GetType());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as E);
        }

        public override int GetHashCode()
        {
            return _id;
        }
    }
}
