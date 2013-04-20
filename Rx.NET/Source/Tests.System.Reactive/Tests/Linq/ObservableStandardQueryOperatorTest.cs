// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveTests.Dummies;

#if !NO_TPL
using System.Threading;
using System.Threading.Tasks;
#endif

namespace ReactiveTests.Tests
{
    [TestClass]
    public class ObservableStandardQueryOperatorTest : ReactiveTest
    {
        #region + Cast +

        [TestMethod]
        public void Cast_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Cast<bool>(default(IObservable<object>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Cast<bool>(DummyObservable<object>.Instance).Subscribe(null));
        }

        class A : IEquatable<A>
        {
            int id;

            public A(int id)
            {
                this.id = id;
            }

            public bool Equals(A other)
            {
                if (other == null)
                    return false;
                return id == other.id && GetType().Equals(other.GetType());
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as A);
            }

            public override int GetHashCode()
            {
                return id;
            }
        }

        class B : A
        {
            public B(int id)
                : base(id)
            {
            }
        }

        class C : A
        {
            public C(int id)
                : base(id)
            {
            }
        }

        class D : B
        {
            public D(int id)
                : base(id)
            {
            }
        }

        class E : IEquatable<E>
        {
            int id;

            public E(int id)
            {
                this.id = id;
            }

            public bool Equals(E other)
            {
                if (other == null)
                    return false;
                return id == other.id && GetType().Equals(other.GetType());
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as E);
            }

            public override int GetHashCode()
            {
                return id;
            }
        }

        [TestMethod]
        public void Cast_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<object>(
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
                OnNext<B>(210, new B(0)),
                OnNext<B>(220, new D(1)),
                OnNext<B>(240, new B(2)),
                OnNext<B>(270, new D(3)),
                OnCompleted<B>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void Cast_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<object>(
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
                OnNext<B>(210, new B(0)),
                OnNext<B>(220, new D(1)),
                OnNext<B>(240, new B(2)),
                OnNext<B>(270, new D(3)),
                OnError<B>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void Cast_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<object>(
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
                OnNext<B>(210, new B(0)),
                OnNext<B>(220, new D(1)),
                OnNext<B>(240, new B(2))
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Cast_NotValid()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<object>(
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
                OnNext<B>(210, new B(0)),
                OnNext<B>(220, new D(1)),
                OnNext<B>(240, new B(2)),
                OnError<B>(250, e => e is InvalidCastException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        #endregion

        #region + DefaultIfEmpty +

        [TestMethod]
        public void DefaultIfEmpty_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DefaultIfEmpty(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DefaultIfEmpty(default(IObservable<int>), 42));
        }

        [TestMethod]
        public void DefaultIfEmpty_NonEmpty1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.DefaultIfEmpty()
            );

            res.Messages.AssertEqual(
                OnNext(280, 42),
                OnNext(360, 43),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void DefaultIfEmpty_NonEmpty2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.DefaultIfEmpty(-1)
            );

            res.Messages.AssertEqual(
                OnNext(280, 42),
                OnNext(360, 43),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void DefaultIfEmpty_Empty1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.DefaultIfEmpty()
            );

            res.Messages.AssertEqual(
                OnNext(420, 0),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void DefaultIfEmpty_Empty2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.DefaultIfEmpty(-1)
            );

            res.Messages.AssertEqual(
                OnNext(420, -1),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void DefaultIfEmpty_Throw1()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(420, ex)
            );

            var res = scheduler.Start(() =>
                xs.DefaultIfEmpty()
            );

            res.Messages.AssertEqual(
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void DefaultIfEmpty_Throw2()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(420, ex)
            );

            var res = scheduler.Start(() =>
                xs.DefaultIfEmpty(42)
            );

            res.Messages.AssertEqual(
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        #endregion

        #region + Distinct +

        [TestMethod]
        public void Distinct_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(default(IObservable<int>), EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(DummyObservable<int>.Instance, default(EqualityComparer<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(default(IObservable<int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(DummyObservable<int>.Instance, default(Func<int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(default(IObservable<int>), x => x, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(DummyObservable<int>.Instance, default(Func<int, int>), EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Distinct(DummyObservable<int>.Instance, x => x, default(EqualityComparer<int>)));
        }

        [TestMethod]
        public void Distinct_DefaultComparer_AllDistinct()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(350, 1),
                OnNext(380, 3),
                OnNext(400, 5),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct()
            );

            res.Messages.AssertEqual(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(350, 1),
                OnNext(380, 3),
                OnNext(400, 5),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void Distinct_DefaultComparer_SomeDuplicates()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(350, 2),
                OnNext(380, 3),
                OnNext(400, 4),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct()
            );

            res.Messages.AssertEqual(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(380, 3),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void Distinct_CustomComparer_AllDistinct()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(350, 1),
                OnNext(380, 3),
                OnNext(400, 5),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct(new ModComparer(10))
            );

            res.Messages.AssertEqual(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(350, 1),
                OnNext(380, 3),
                OnNext(400, 5),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void Distinct_CustomComparer_SomeDuplicates()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(350, 12),
                OnNext(380, 3),
                OnNext(400, 24),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct(new ModComparer(10))
            );

            res.Messages.AssertEqual(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(380, 3),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        class ModComparer : IEqualityComparer<int>
        {
            private readonly int _mod;

            public ModComparer(int mod)
            {
                _mod = mod;
            }

            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(x % _mod, y % _mod);
            }

            public int GetHashCode(int obj)
            {
                return EqualityComparer<int>.Default.GetHashCode(obj % _mod);
            }
        }

        [TestMethod]
        public void Distinct_KeySelector_DefaultComparer_AllDistinct()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 8),
                OnNext(300, 4),
                OnNext(350, 2),
                OnNext(380, 6),
                OnNext(400, 10),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct(x => x / 2)
            );

            res.Messages.AssertEqual(
                OnNext(280, 8),
                OnNext(300, 4),
                OnNext(350, 2),
                OnNext(380, 6),
                OnNext(400, 10),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void Distinct_KeySelector_DefaultComparer_SomeDuplicates()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(380, 7),
                OnNext(400, 5),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct(x => x / 2)
            );

            res.Messages.AssertEqual(
                OnNext(280, 4),
                OnNext(300, 2),
                OnNext(380, 7),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void Distinct_KeySelector_CustomComparer_AllDistinct()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 8),
                OnNext(300, 4),
                OnNext(350, 2),
                OnNext(380, 6),
                OnNext(400, 10),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct(x => x / 2, new ModComparer(10))
            );

            res.Messages.AssertEqual(
                OnNext(280, 8),
                OnNext(300, 4),
                OnNext(350, 2),
                OnNext(380, 6),
                OnNext(400, 10),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void Distinct_KeySelector_CustomComparer_SomeDuplicates()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 8),
                OnNext(300, 4),
                OnNext(350, 2),
                OnNext(380, 6),
                OnNext(400, 10),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.Distinct(x => x / 2, new ModComparer(3))
            );

            res.Messages.AssertEqual(
                OnNext(280, 8),
                OnNext(300, 4),
                OnNext(380, 6),
                OnCompleted<int>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void Distinct_KeySelector_Throws()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 3),
                OnNext(300, 2),
                OnNext(350, 1),
                OnNext(380, 0),
                OnNext(400, 4),
                OnCompleted<int>(420)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Distinct(x => { if (x == 0) throw ex; return x / 2; })
            );

            res.Messages.AssertEqual(
                OnNext(280, 3),
                OnNext(350, 1),
                OnError<int>(380, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 380)
            );
        }

        [TestMethod]
        public void Distinct_CustomComparer_Throws()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(380, 4),
                OnNext(400, 5),
                OnCompleted<int>(420)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Distinct(new ThrowComparer(4, ex))
            );

            res.Messages.AssertEqual(
                OnNext(280, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnError<int>(380, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 380)
            );
        }

        class ThrowComparer : IEqualityComparer<int>
        {
            private readonly int _err;
            private readonly Exception _ex;

            public ThrowComparer(int err, Exception ex)
            {
                _err = err;
                _ex = ex;
            }

            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(x, y);
            }

            public int GetHashCode(int obj)
            {
                if (obj == _err)
                    throw _ex;

                return EqualityComparer<int>.Default.GetHashCode(obj);
            }
        }

        [TestMethod]
        public void Distinct_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnError<int>(380, ex)
            );

            var res = scheduler.Start(() =>
                xs.Distinct()
            );

            res.Messages.AssertEqual(
                OnNext(280, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnError<int>(380, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 380)
            );
        }

        [TestMethod]
        public void Distinct_Null()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "bar"),
                OnNext(240, default(string)),
                OnNext(260, "bar"),
                OnNext(280, "foo"),
                OnNext(300, default(string)),
                OnCompleted<string>(400)
            );

            var res = scheduler.Start(() =>
                xs.Distinct()
            );

            res.Messages.AssertEqual(
                OnNext(220, "bar"),
                OnNext(240, default(string)),
                OnNext(280, "foo"),
                OnCompleted<string>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        #endregion

        #region + GroupBy +

        [TestMethod]
        public void GroupBy_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy((Func<int, int>)null, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, (Func<int, int>)null, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default).Subscribe(null));
        }

        [TestMethod]
        public void GroupBy_KeyEle_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy((Func<int, int>)null, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, (Func<int, int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void GroupBy_KeyComparer_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy((Func<int, int>)null, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, (IEqualityComparer<int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, EqualityComparer<int>.Default).Subscribe(null));
        }

        [TestMethod]
        public void GroupBy_Key_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy((Func<int, int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void GroupBy_WithKeyComparer()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupBy(x =>
                {
                    keyInvoked++;
                    return x.Trim();
                }, comparer).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            Assert.AreEqual(12, keyInvoked);
        }

        [TestMethod]
        public void GroupBy_Outer_Complete()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupBy(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            Assert.AreEqual(12, keyInvoked);
            Assert.AreEqual(12, eleInvoked);
        }

        [TestMethod]
        public void GroupBy_Outer_Error()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, ex),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupBy(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnError<string>(570, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            Assert.AreEqual(12, keyInvoked);
            Assert.AreEqual(12, eleInvoked);
        }

        [TestMethod]
        public void GroupBy_Outer_Dispose()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupBy(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    }, x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    }, comparer
                ).Select(g => g.Key),
                355
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 355)
            );

            Assert.AreEqual(5, keyInvoked);
            Assert.AreEqual(5, eleInvoked);
        }

        [TestMethod]
        public void GroupBy_Outer_KeyThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupBy(
                    x =>
                    {
                        keyInvoked++;
                        if (keyInvoked == 10)
                            throw ex;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnError<string>(480, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 480)
            );

            Assert.AreEqual(10, keyInvoked);
            Assert.AreEqual(9, eleInvoked);
        }

        [TestMethod]
        public void GroupBy_Outer_EleThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupBy(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        if (eleInvoked == 10)
                            throw ex;
                        return Reverse(x);
                    },
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnError<string>(480, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 480)
            );

            Assert.AreEqual(10, keyInvoked);
            Assert.AreEqual(10, eleInvoked);
        }

        [TestMethod]
        public void GroupBy_Outer_ComparerEqualsThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, 250, ushort.MaxValue);

            var res = scheduler.Start(() =>
                xs.GroupBy(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnError<string>(310, comparer.EqualsException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            Assert.AreEqual(4, keyInvoked);
            Assert.AreEqual(3, eleInvoked);
        }

        [TestMethod]
        public void GroupBy_Outer_ComparerGetHashCodeThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, ushort.MaxValue, 410);

            var res = scheduler.Start(() =>
                xs.GroupBy(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnError<string>(420, comparer.HashCodeException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            Assert.AreEqual(8, keyInvoked);
            Assert.AreEqual(7, eleInvoked);
        }

        [TestMethod]
        public void GroupBy_Inner_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                scheduler.ScheduleRelative(100, () => innerSubscriptions[group.Key] = group.Subscribe(result));
            }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(470, " OOF"),
                OnNext(530, "    oOf    "),
                OnCompleted<string>(570)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnCompleted<string>(570)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "),
                OnCompleted<string>(570)
            );

            res["qux"].Messages.AssertEqual(
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupBy_Inner_Complete_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnNext(470, " OOF"),
                OnNext(530, "    oOf    "),
                OnCompleted<string>(570)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnCompleted<string>(570)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "),
                OnCompleted<string>(570)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupBy_Inner_Error()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, ex1),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                scheduler.ScheduleRelative(100, () => innerSubscriptions[group.Key] = group.Subscribe(result));
            }, ex => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(470, " OOF"),
                OnNext(530, "    oOf    "),
                OnError<string>(570, ex1)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnError<string>(570, ex1)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "),
                OnError<string>(570, ex1)
            );

            res["qux"].Messages.AssertEqual(
                OnError<string>(570, ex1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupBy_Inner_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }));

            scheduler.ScheduleAbsolute(400, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof")
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   ")
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB ")
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  ")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void GroupBy_Inner_KeyThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            var keyInvoked = 0;

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x =>
            {
                keyInvoked++;
                if (keyInvoked == 6)
                    throw ex;
                return x.Trim();
            }, x => Reverse(x), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(3, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnError<string>(360, ex)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnError<string>(360, ex)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(360, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 360)
            );
        }

        [TestMethod]
        public void GroupBy_Inner_EleThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            var eleInvoked = 0;

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x =>
            {
                eleInvoked++;
                if (eleInvoked == 6)
                    throw ex;
                return Reverse(x);
            }, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnError<string>(360, ex)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnError<string>(360, ex)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(360, ex)
            );

            res["qux"].Messages.AssertEqual(
                OnError<string>(360, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 360)
            );
        }

        [TestMethod]
        public void GroupBy_Inner_Comparer_EqualsThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, 400, ushort.MaxValue);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnError<string>(420, comparer.EqualsException)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnError<string>(420, comparer.EqualsException)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(420, comparer.EqualsException)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnError<string>(420, comparer.EqualsException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void GroupBy_Inner_Comparer_GetHashCodeThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, ushort.MaxValue, 400);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnError<string>(420, comparer.HashCodeException)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnError<string>(420, comparer.HashCodeException)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(420, comparer.HashCodeException)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnError<string>(420, comparer.HashCodeException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void GroupBy_Outer_Independence()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();
            var outerResults = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                outerResults.OnNext(group.Key);
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, outerResults.OnError, outerResults.OnCompleted));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.ScheduleAbsolute(320, () => outerSubscription.Dispose());

            scheduler.Start();

            Assert.AreEqual(2, inners.Count);

            outerResults.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR")
            );

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnNext(470, " OOF"),
                OnNext(530, "    oOf    "),
                OnCompleted<string>(570)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupBy_Inner_Independence()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();
            var outerResults = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                outerResults.OnNext(group.Key);
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, outerResults.OnError, outerResults.OnCompleted));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof")
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnCompleted<string>(570)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "),
                OnCompleted<string>(570)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupBy_Inner_Multiple_Independence()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();
            var outerResults = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                outerResults.OnNext(group.Key);
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, outerResults.OnError, outerResults.OnCompleted));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());
            scheduler.ScheduleAbsolute(280, () => innerSubscriptions["baR"].Dispose());
            scheduler.ScheduleAbsolute(355, () => innerSubscriptions["Baz"].Dispose());
            scheduler.ScheduleAbsolute(400, () => innerSubscriptions["qux"].Dispose());

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof")
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab")
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB ")
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  ")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupBy_Inner_Escape_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(310, "foO "),
                OnNext(470, "FOO "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570)
            );

            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inner = default(IObservable<string>);
            var innerSubscription = default(IDisposable);
            var res = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim()));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                inner = group;
            }));

            scheduler.ScheduleAbsolute(600, () => innerSubscription = inner.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                innerSubscription.Dispose();
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            res.Messages.AssertEqual(
                OnCompleted<string>(600)
            );
        }

        [TestMethod]
        public void GroupBy_Inner_Escape_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(310, "foO "),
                OnNext(470, "FOO "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, ex)
            );

            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inner = default(IObservable<string>);
            var innerSubscription = default(IDisposable);
            var res = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim()));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                inner = group;
            }, _ => { }));

            scheduler.ScheduleAbsolute(600, () => innerSubscription = inner.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                innerSubscription.Dispose();
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            res.Messages.AssertEqual(
                OnError<string>(600, ex)
            );
        }

        [TestMethod]
        public void GroupBy_Inner_Escape_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(310, "foO "),
                OnNext(470, "FOO "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, new Exception())
            );

            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inner = default(IObservable<string>);
            var innerSubscription = default(IDisposable);
            var res = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim()));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                inner = group;
            }));

            scheduler.ScheduleAbsolute(400, () => outerSubscription.Dispose());

            scheduler.ScheduleAbsolute(600, () => innerSubscription = inner.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                innerSubscription.Dispose();
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void GroupBy_NullKeys_Simple()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "bar"),
                OnNext(240, "foo"),
                OnNext(310, "qux"),
                OnNext(470, "baz"),
                OnCompleted<string>(500)
            );

            var res = scheduler.Start(() => xs.GroupBy(x => x[0] == 'b' ? null : x.ToUpper()).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

            res.Messages.AssertEqual(
                OnNext(220, "(null)bar"),
                OnNext(240, "FOOfoo"),
                OnNext(310, "QUXqux"),
                OnNext(470, "(null)baz"),
                OnCompleted<string>(500)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [TestMethod]
        public void GroupBy_NullKeys_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "bar"),
                OnNext(240, "foo"),
                OnNext(310, "qux"),
                OnNext(470, "baz"),
                OnError<string>(500, ex)
            );

            var nullGroup = scheduler.CreateObserver<string>();
            var err = default(Exception);

            scheduler.ScheduleAbsolute(200, () => xs.GroupBy(x => x[0] == 'b' ? null : x.ToUpper()).Where(g => g.Key == null).Subscribe(g => g.Subscribe(nullGroup), ex_ => err = ex_));
            scheduler.Start();

            Assert.AreSame(ex, err);

            nullGroup.Messages.AssertEqual(
                OnNext(220, "bar"),
                OnNext(470, "baz"),
                OnError<string>(500, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        class GroupByComparer : IEqualityComparer<string>
        {
            TestScheduler scheduler;
            int equalsThrowsAfter;
            ushort getHashCodeThrowsAfter;

            public Exception HashCodeException = new Exception();
            public Exception EqualsException = new Exception();

            public GroupByComparer(TestScheduler scheduler, ushort equalsThrowsAfter, ushort getHashCodeThrowsAfter)
            {
                this.scheduler = scheduler;
                this.equalsThrowsAfter = equalsThrowsAfter;
                this.getHashCodeThrowsAfter = getHashCodeThrowsAfter;
            }

            public GroupByComparer(TestScheduler scheduler)
                : this(scheduler, ushort.MaxValue, ushort.MaxValue)
            {
            }

            public bool Equals(string x, string y)
            {
                if (scheduler.Clock > equalsThrowsAfter)
                    throw EqualsException;

                return x.Equals(y, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(string obj)
            {
                if (scheduler.Clock > getHashCodeThrowsAfter)
                    throw HashCodeException;

                return StringComparer.OrdinalIgnoreCase.GetHashCode(obj);
            }
        }

        static string Reverse(string s)
        {
            var sb = new StringBuilder();

            for (var i = s.Length - 1; i >= 0; i--)
                sb.Append(s[i]);

            return sb.ToString();
        }

        #endregion

        #region + GroupBy w/capacity +

        private const int _groupByCapacity = 1024;

        [TestMethod]
        public void GroupBy_Capacity_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, _groupByCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy((Func<int, int>)null, DummyFunc<int, int>.Instance, _groupByCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, (Func<int, int>)null, _groupByCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, _groupByCapacity, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, _groupByCapacity, EqualityComparer<int>.Default).Subscribe(null));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, -1, EqualityComparer<int>.Default));
        }

        [TestMethod]
        public void GroupBy_Capacity_KeyEle_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, _groupByCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy((Func<int, int>)null, DummyFunc<int, int>.Instance, _groupByCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, (Func<int, int>)null, _groupByCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, _groupByCapacity).Subscribe(null));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, -1));
        }

        [TestMethod]
        public void GroupBy_Capacity_KeyComparer_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance, _groupByCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy((Func<int, int>)null, _groupByCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, _groupByCapacity, (IEqualityComparer<int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, _groupByCapacity, EqualityComparer<int>.Default).Subscribe(null));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, -1, EqualityComparer<int>.Default));
        }

        [TestMethod]
        public void GroupBy_Capacity_Key_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance, _groupByCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy((Func<int, int>)null, _groupByCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, _groupByCapacity).Subscribe(null));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, -1));
        }

        [TestMethod]
        public void GroupBy_Capacity_WithKeyComparer()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupBy(x =>
                {
                    keyInvoked++;
                    return x.Trim();
                }, _groupByCapacity, comparer).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            Assert.AreEqual(12, keyInvoked);
        }

        [TestMethod]
        public void GroupBy_Capacity_Outer_Complete()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupBy(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    _groupByCapacity,
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            Assert.AreEqual(12, keyInvoked);
            Assert.AreEqual(12, eleInvoked);
        }

        [TestMethod]
        public void GroupBy_Capacity_Outer_Error()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, ex),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupBy(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    _groupByCapacity,
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnError<string>(570, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            Assert.AreEqual(12, keyInvoked);
            Assert.AreEqual(12, eleInvoked);
        }

        [TestMethod]
        public void GroupBy_Capacity_Outer_Dispose()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupBy(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    }, x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    }, _groupByCapacity, comparer
                ).Select(g => g.Key),
                355
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 355)
            );

            Assert.AreEqual(5, keyInvoked);
            Assert.AreEqual(5, eleInvoked);
        }

        [TestMethod]
        public void GroupBy_Capacity_Outer_KeyThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupBy(
                    x =>
                    {
                        keyInvoked++;
                        if (keyInvoked == 10)
                            throw ex;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    _groupByCapacity,
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnError<string>(480, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 480)
            );

            Assert.AreEqual(10, keyInvoked);
            Assert.AreEqual(9, eleInvoked);
        }

        [TestMethod]
        public void GroupBy_Capacity_Outer_EleThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupBy(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        if (eleInvoked == 10)
                            throw ex;
                        return Reverse(x);
                    },
                    _groupByCapacity,
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnError<string>(480, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 480)
            );

            Assert.AreEqual(10, keyInvoked);
            Assert.AreEqual(10, eleInvoked);
        }

        [TestMethod]
        public void GroupBy_Capacity_Outer_ComparerEqualsThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, 250, ushort.MaxValue);

            var res = scheduler.Start(() =>
                xs.GroupBy(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    _groupByCapacity,
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnError<string>(310, comparer.EqualsException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            Assert.AreEqual(4, keyInvoked);
            Assert.AreEqual(3, eleInvoked);
        }

        [TestMethod]
        public void GroupBy_Capacity_Outer_ComparerGetHashCodeThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, ushort.MaxValue, 410);

            var res = scheduler.Start(() =>
                xs.GroupBy(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    _groupByCapacity,
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnError<string>(420, comparer.HashCodeException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            Assert.AreEqual(8, keyInvoked);
            Assert.AreEqual(7, eleInvoked);
        }

        [TestMethod]
        public void GroupBy_Capacity_Inner_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), _groupByCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                scheduler.ScheduleRelative(100, () => innerSubscriptions[group.Key] = group.Subscribe(result));
            }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(470, " OOF"),
                OnNext(530, "    oOf    "),
                OnCompleted<string>(570)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnCompleted<string>(570)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "),
                OnCompleted<string>(570)
            );

            res["qux"].Messages.AssertEqual(
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupBy_Capacity_Inner_Complete_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), _groupByCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnNext(470, " OOF"),
                OnNext(530, "    oOf    "),
                OnCompleted<string>(570)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnCompleted<string>(570)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "),
                OnCompleted<string>(570)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupBy_Capacity_Inner_Error()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, ex1),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), _groupByCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                scheduler.ScheduleRelative(100, () => innerSubscriptions[group.Key] = group.Subscribe(result));
            }, ex => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(470, " OOF"),
                OnNext(530, "    oOf    "),
                OnError<string>(570, ex1)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnError<string>(570, ex1)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "),
                OnError<string>(570, ex1)
            );

            res["qux"].Messages.AssertEqual(
                OnError<string>(570, ex1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupBy_Capacity_Inner_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), _groupByCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }));

            scheduler.ScheduleAbsolute(400, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof")
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   ")
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB ")
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  ")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void GroupBy_Capacity_Inner_KeyThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            var keyInvoked = 0;

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x =>
            {
                keyInvoked++;
                if (keyInvoked == 6)
                    throw ex;
                return x.Trim();
            }, x => Reverse(x), _groupByCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(3, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnError<string>(360, ex)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnError<string>(360, ex)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(360, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 360)
            );
        }

        [TestMethod]
        public void GroupBy_Capacity_Inner_EleThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            var eleInvoked = 0;

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x =>
            {
                eleInvoked++;
                if (eleInvoked == 6)
                    throw ex;
                return Reverse(x);
            }, _groupByCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnError<string>(360, ex)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnError<string>(360, ex)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(360, ex)
            );

            res["qux"].Messages.AssertEqual(
                OnError<string>(360, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 360)
            );
        }

        [TestMethod]
        public void GroupBy_Capacity_Inner_Comparer_EqualsThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, 400, ushort.MaxValue);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), _groupByCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnError<string>(420, comparer.EqualsException)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnError<string>(420, comparer.EqualsException)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(420, comparer.EqualsException)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnError<string>(420, comparer.EqualsException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void GroupBy_Capacity_Inner_Comparer_GetHashCodeThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, ushort.MaxValue, 400);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), _groupByCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnError<string>(420, comparer.HashCodeException)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnError<string>(420, comparer.HashCodeException)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(420, comparer.HashCodeException)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnError<string>(420, comparer.HashCodeException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void GroupBy_Capacity_Outer_Independence()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();
            var outerResults = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), _groupByCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                outerResults.OnNext(group.Key);
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, outerResults.OnError, outerResults.OnCompleted));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.ScheduleAbsolute(320, () => outerSubscription.Dispose());

            scheduler.Start();

            Assert.AreEqual(2, inners.Count);

            outerResults.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR")
            );

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnNext(470, " OOF"),
                OnNext(530, "    oOf    "),
                OnCompleted<string>(570)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupBy_Capacity_Inner_Independence()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();
            var outerResults = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), _groupByCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                outerResults.OnNext(group.Key);
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, outerResults.OnError, outerResults.OnCompleted));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof")
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnCompleted<string>(570)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "),
                OnCompleted<string>(570)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupBy_Capacity_Inner_Multiple_Independence()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();
            var outerResults = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), _groupByCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                outerResults.OnNext(group.Key);
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, outerResults.OnError, outerResults.OnCompleted));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());
            scheduler.ScheduleAbsolute(280, () => innerSubscriptions["baR"].Dispose());
            scheduler.ScheduleAbsolute(355, () => innerSubscriptions["Baz"].Dispose());
            scheduler.ScheduleAbsolute(400, () => innerSubscriptions["qux"].Dispose());

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof")
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab")
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB ")
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  ")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupBy_Capacity_Inner_Escape_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(310, "foO "),
                OnNext(470, "FOO "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570)
            );

            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inner = default(IObservable<string>);
            var innerSubscription = default(IDisposable);
            var res = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), _groupByCapacity));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                inner = group;
            }));

            scheduler.ScheduleAbsolute(600, () => innerSubscription = inner.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                innerSubscription.Dispose();
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            res.Messages.AssertEqual(
                OnCompleted<string>(600)
            );
        }

        [TestMethod]
        public void GroupBy_Capacity_Inner_Escape_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(310, "foO "),
                OnNext(470, "FOO "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, ex)
            );

            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inner = default(IObservable<string>);
            var innerSubscription = default(IDisposable);
            var res = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), _groupByCapacity));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                inner = group;
            }, _ => { }));

            scheduler.ScheduleAbsolute(600, () => innerSubscription = inner.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                innerSubscription.Dispose();
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            res.Messages.AssertEqual(
                OnError<string>(600, ex)
            );
        }

        [TestMethod]
        public void GroupBy_Capacity_Inner_Escape_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(310, "foO "),
                OnNext(470, "FOO "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, new Exception())
            );

            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inner = default(IObservable<string>);
            var innerSubscription = default(IDisposable);
            var res = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), _groupByCapacity));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                inner = group;
            }));

            scheduler.ScheduleAbsolute(400, () => outerSubscription.Dispose());

            scheduler.ScheduleAbsolute(600, () => innerSubscription = inner.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                innerSubscription.Dispose();
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void GroupBy_Capacity_NullKeys_Simple()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "bar"),
                OnNext(240, "foo"),
                OnNext(310, "qux"),
                OnNext(470, "baz"),
                OnCompleted<string>(500)
            );

            var res = scheduler.Start(() => xs.GroupBy(x => x[0] == 'b' ? null : x.ToUpper(), _groupByCapacity).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

            res.Messages.AssertEqual(
                OnNext(220, "(null)bar"),
                OnNext(240, "FOOfoo"),
                OnNext(310, "QUXqux"),
                OnNext(470, "(null)baz"),
                OnCompleted<string>(500)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [TestMethod]
        public void GroupBy_Capacity_NullKeys_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "bar"),
                OnNext(240, "foo"),
                OnNext(310, "qux"),
                OnNext(470, "baz"),
                OnError<string>(500, ex)
            );

            var nullGroup = scheduler.CreateObserver<string>();
            var err = default(Exception);

            scheduler.ScheduleAbsolute(200, () => xs.GroupBy(x => x[0] == 'b' ? null : x.ToUpper(), _groupByCapacity).Where(g => g.Key == null).Subscribe(g => g.Subscribe(nullGroup), ex_ => err = ex_));
            scheduler.Start();

            Assert.AreSame(ex, err);

            nullGroup.Messages.AssertEqual(
                OnNext(220, "bar"),
                OnNext(470, "baz"),
                OnError<string>(500, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        #endregion

        #region + GroupByUntil +

        [TestMethod]
        public void GroupByUntil_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default(IObservable<int>), DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default(Func<int, int>), DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default(Func<int, int>), DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>), EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, default(IEqualityComparer<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default(IObservable<int>), DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default(Func<int, int>), DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default(Func<int, int>), DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default(IObservable<int>), DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default(Func<int, int>), DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>), EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, default(IEqualityComparer<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default(IObservable<int>), DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default(Func<int, int>), DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>)));
        }

        [TestMethod]
        public void GroupByUntil_WithKeyComparer()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    g => g.Skip(2),
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnNext(470, "FOO"),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            Assert.AreEqual(12, keyInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Outer_Complete()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    g => g.Skip(2),
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnNext(470, "FOO"),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            Assert.AreEqual(12, keyInvoked);
            Assert.AreEqual(12, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Outer_Error()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, ex),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    g => g.Skip(2),
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnNext(470, "FOO"),
                OnError<string>(570, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            Assert.AreEqual(12, keyInvoked);
            Assert.AreEqual(12, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Outer_Dispose()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    g => g.Skip(2),
                    comparer
                ).Select(g => g.Key),
                355
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 355)
            );

            Assert.AreEqual(5, keyInvoked);
            Assert.AreEqual(5, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Outer_KeyThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        if (keyInvoked == 10)
                            throw ex;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    g => g.Skip(2),
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnNext(470, "FOO"),
                OnError<string>(480, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 480)
            );

            Assert.AreEqual(10, keyInvoked);
            Assert.AreEqual(9, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Outer_EleThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        if (eleInvoked == 10)
                            throw ex;
                        return Reverse(x);
                    },
                    g => g.Skip(2),
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnNext(470, "FOO"),
                OnError<string>(480, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 480)
            );

            Assert.AreEqual(10, keyInvoked);
            Assert.AreEqual(10, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Outer_ComparerEqualsThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, 250, ushort.MaxValue);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    g => g.Skip(2),
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnError<string>(310, comparer.EqualsException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            Assert.AreEqual(4, keyInvoked);
            Assert.AreEqual(3, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Outer_ComparerGetHashCodeThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, ushort.MaxValue, 410);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    g => g.Skip(2),
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnError<string>(420, comparer.HashCodeException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            Assert.AreEqual(8, keyInvoked);
            Assert.AreEqual(7, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Inner_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                scheduler.ScheduleRelative(100, () => innerSubscriptions[group.Key] = group.Subscribe(result));
            }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(5, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnCompleted<string>(320)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(390, "rab   "),
                OnCompleted<string>(420)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(480, "  zab"),
                OnCompleted<string>(510)
            );

            res["qux"].Messages.AssertEqual(
                OnCompleted<string>(570)
            );

            res["FOO"].Messages.AssertEqual(
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupByUntil_Inner_Complete_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(5, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnCompleted<string>(420)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "),
                OnCompleted<string>(510)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnCompleted<string>(570)
            );

            res["FOO"].Messages.AssertEqual(
                OnNext(470, " OOF"),
                OnNext(530, "    oOf    "),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupByUntil_Inner_Error()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, ex1),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                scheduler.ScheduleRelative(100, () => innerSubscriptions[group.Key] = group.Subscribe(result));
            }, ex => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(5, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnCompleted<string>(320)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(390, "rab   "),
                OnCompleted<string>(420)
               );

            res["Baz"].Messages.AssertEqual(
                OnNext(480, "  zab"),
                OnCompleted<string>(510)
            );

            res["qux"].Messages.AssertEqual(
                OnError<string>(570, ex1)
            );

            res["FOO"].Messages.AssertEqual(
                OnError<string>(570, ex1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupByUntil_Inner_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }));

            scheduler.ScheduleAbsolute(400, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   ")
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB ")
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  ")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void GroupByUntil_Inner_KeyThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            var keyInvoked = 0;
            var ex = new Exception();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x =>
            {
                keyInvoked++;
                if (keyInvoked == 6)
                    throw ex;
                return x.Trim();
            }, x => Reverse(x), g => g.Skip(2), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(3, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnError<string>(360, ex)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(360, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 360)
            );
        }

        [TestMethod]
        public void GroupByUntil_Inner_EleThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            var eleInvoked = 0;
            var ex = new Exception();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x =>
            {
                eleInvoked++;
                if (eleInvoked == 6)
                    throw ex;
                return Reverse(x);
            }, g => g.Skip(2), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnError<string>(360, ex)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(360, ex)
            );

            res["qux"].Messages.AssertEqual(
                OnError<string>(360, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 360)
            );
        }

        [TestMethod]
        public void GroupByUntil_Inner_Comparer_EqualsThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, 400, ushort.MaxValue);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnError<string>(420, comparer.EqualsException)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(420, comparer.EqualsException)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnError<string>(420, comparer.EqualsException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void GroupByUntil_Inner_Comparer_GetHashCodeThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, ushort.MaxValue, 400);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnError<string>(420, comparer.HashCodeException)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(420, comparer.HashCodeException)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnError<string>(420, comparer.HashCodeException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void GroupByUntil_Outer_Independence()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();
            var outerResults = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                outerResults.OnNext(group.Key);
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, outerResults.OnError, outerResults.OnCompleted));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.ScheduleAbsolute(320, () => outerSubscription.Dispose());

            scheduler.Start();

            Assert.AreEqual(2, inners.Count);

            outerResults.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR")
            );

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnCompleted<string>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void GroupByUntil_Inner_Independence()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();
            var outerResults = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                outerResults.OnNext(group.Key);
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, outerResults.OnError, outerResults.OnCompleted));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());

            scheduler.Start();

            Assert.AreEqual(5, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnCompleted<string>(420)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "),
                OnCompleted<string>(510)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnCompleted<string>(570)
            );

            res["FOO"].Messages.AssertEqual(
                OnNext(470, " OOF"),
                OnNext(530, "    oOf    "),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupByUntil_Inner_Multiple_Independence()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();
            var outerResults = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                outerResults.OnNext(group.Key);
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, outerResults.OnError, outerResults.OnCompleted));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());
            scheduler.ScheduleAbsolute(280, () => innerSubscriptions["baR"].Dispose());
            scheduler.ScheduleAbsolute(355, () => innerSubscriptions["Baz"].Dispose());
            scheduler.ScheduleAbsolute(400, () => innerSubscriptions["qux"].Dispose());

            scheduler.Start();

            Assert.AreEqual(5, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab")
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB ")
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  ")
            );

            res["FOO"].Messages.AssertEqual(
                OnNext(470, " OOF"),
                OnNext(530, "    oOf    "),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupByUntil_Inner_Escape_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(310, "foO "),
                OnNext(470, "FOO "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570)
            );

            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inner = default(IObservable<string>);
            var innerSubscription = default(IDisposable);
            var res = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), g => g.Skip(2)));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                inner = group;
            }));

            scheduler.ScheduleAbsolute(600, () => innerSubscription = inner.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                innerSubscription.Dispose();
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            res.Messages.AssertEqual(
                OnCompleted<string>(600)
            );
        }

        [TestMethod]
        public void GroupByUntil_Inner_Escape_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(310, "foO "),
                OnNext(470, "FOO "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, ex)
            );

            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inner = default(IObservable<string>);
            var innerSubscription = default(IDisposable);
            var res = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), g => g.Skip(2)));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                inner = group;
            }, _ => { }));

            scheduler.ScheduleAbsolute(600, () => innerSubscription = inner.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                innerSubscription.Dispose();
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            res.Messages.AssertEqual(
                OnError<string>(600, ex)
            );
        }

        [TestMethod]
        public void GroupByUntil_Inner_Escape_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(310, "foO "),
                OnNext(470, "FOO "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, new Exception())
            );

            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inner = default(IObservable<string>);
            var innerSubscription = default(IDisposable);
            var res = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), g => g.Skip(2)));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                inner = group;
            }));

            scheduler.ScheduleAbsolute(290, () => outerSubscription.Dispose());

            scheduler.ScheduleAbsolute(600, () => innerSubscription = inner.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                innerSubscription.Dispose();
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void GroupByUntil_Default()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim().ToLower();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    g => g.Skip(2)
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "bar"),
                OnNext(350, "baz"),
                OnNext(360, "qux"),
                OnNext(470, "foo"),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            Assert.AreEqual(12, keyInvoked);
            Assert.AreEqual(12, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_DurationSelector_Throws()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "foo")
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.GroupByUntil<string, string, string>(x => x, g => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<IGroupedObservable<string, string>>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void GroupByUntil_NullKeys_Simple_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "bar"),
                OnNext(240, "foo"),
                OnNext(310, "qux"),
                OnNext(470, "baz"),
                OnCompleted<string>(500)
            );

            var res = scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => Observable.Never<Unit>()).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

            res.Messages.AssertEqual(
                OnNext(220, "(null)bar"),
                OnNext(240, "FOOfoo"),
                OnNext(310, "QUXqux"),
                OnNext(470, "(null)baz"),
                OnCompleted<string>(500)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [TestMethod]
        public void GroupByUntil_NullKeys_Simple_Expire1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "bar"),
                OnNext(240, "foo"),
                OnNext(310, "qux"),
                OnNext(470, "baz"),
                OnCompleted<string>(500)
            );

            var n = 0;
            var res = scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => { if (g.Key == null) n++; return Observable.Timer(TimeSpan.FromTicks(50), scheduler); }).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

            Assert.AreEqual(2, n);

            res.Messages.AssertEqual(
                OnNext(220, "(null)bar"),
                OnNext(240, "FOOfoo"),
                OnNext(310, "QUXqux"),
                OnNext(470, "(null)baz"),
                OnCompleted<string>(500)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [TestMethod]
        public void GroupByUntil_NullKeys_Simple_Expire2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "bar"),
                OnNext(240, "foo"),
                OnNext(310, "qux"),
                OnNext(470, "baz"),
                OnCompleted<string>(500)
            );

            var n = 0;
            var res = scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => { if (g.Key == null) n++; return Observable.Timer(TimeSpan.FromTicks(50), scheduler).IgnoreElements(); }).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

            Assert.AreEqual(2, n);

            res.Messages.AssertEqual(
                OnNext(220, "(null)bar"),
                OnNext(240, "FOOfoo"),
                OnNext(310, "QUXqux"),
                OnNext(470, "(null)baz"),
                OnCompleted<string>(500)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [TestMethod]
        public void GroupByUntil_NullKeys_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "bar"),
                OnNext(240, "foo"),
                OnNext(310, "qux"),
                OnNext(470, "baz"),
                OnError<string>(500, ex)
            );

            var nullGroup = scheduler.CreateObserver<string>();
            var err = default(Exception);

            scheduler.ScheduleAbsolute(200, () => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => Observable.Never<Unit>()).Where(g => g.Key == null).Subscribe(g => g.Subscribe(nullGroup), ex_ => err = ex_));
            scheduler.Start();

            Assert.AreSame(ex, err);

            nullGroup.Messages.AssertEqual(
                OnNext(220, "bar"),
                OnNext(470, "baz"),
                OnError<string>(500, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        #endregion

        #region + GroupByUntil w/capacity +

        private const int _groupByUntilCapacity = 1024;

        [TestMethod]
        public void GroupByUntil_Capacity_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default(IObservable<int>), DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default(Func<int, int>), DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default(Func<int, int>), DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>), _groupByUntilCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity, default(IEqualityComparer<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default(IObservable<int>), DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default(Func<int, int>), DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default(Func<int, int>), DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>), _groupByUntilCapacity));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default(IObservable<int>), DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default(Func<int, int>), DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>), _groupByUntilCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity, default(IEqualityComparer<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default(IObservable<int>), DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default(Func<int, int>), DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>), _groupByUntilCapacity));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, -1, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, -1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, -1, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, -1));
        }

        [TestMethod]
        public void GroupByUntil_Capacity_WithKeyComparer()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    g => g.Skip(2),
                    _groupByUntilCapacity,
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnNext(470, "FOO"),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            Assert.AreEqual(12, keyInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Outer_Complete()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    g => g.Skip(2),
                    _groupByUntilCapacity,
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnNext(470, "FOO"),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            Assert.AreEqual(12, keyInvoked);
            Assert.AreEqual(12, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Outer_Error()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, ex),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    g => g.Skip(2),
                    _groupByUntilCapacity,
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnNext(470, "FOO"),
                OnError<string>(570, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            Assert.AreEqual(12, keyInvoked);
            Assert.AreEqual(12, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Outer_Dispose()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    g => g.Skip(2),
                    _groupByUntilCapacity,
                    comparer
                ).Select(g => g.Key),
                355
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 355)
            );

            Assert.AreEqual(5, keyInvoked);
            Assert.AreEqual(5, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Outer_KeyThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        if (keyInvoked == 10)
                            throw ex;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    g => g.Skip(2),
                    _groupByUntilCapacity,
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnNext(470, "FOO"),
                OnError<string>(480, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 480)
            );

            Assert.AreEqual(10, keyInvoked);
            Assert.AreEqual(9, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Outer_EleThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        if (eleInvoked == 10)
                            throw ex;
                        return Reverse(x);
                    },
                    g => g.Skip(2),
                    _groupByUntilCapacity,
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnNext(470, "FOO"),
                OnError<string>(480, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 480)
            );

            Assert.AreEqual(10, keyInvoked);
            Assert.AreEqual(10, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Outer_ComparerEqualsThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, 250, ushort.MaxValue);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    g => g.Skip(2),
                    _groupByUntilCapacity,
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnError<string>(310, comparer.EqualsException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            Assert.AreEqual(4, keyInvoked);
            Assert.AreEqual(3, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Outer_ComparerGetHashCodeThrow()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, ushort.MaxValue, 410);

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    g => g.Skip(2),
                    _groupByUntilCapacity,
                    comparer
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR"),
                OnNext(350, "Baz"),
                OnNext(360, "qux"),
                OnError<string>(420, comparer.HashCodeException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            Assert.AreEqual(8, keyInvoked);
            Assert.AreEqual(7, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Inner_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), _groupByUntilCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                scheduler.ScheduleRelative(100, () => innerSubscriptions[group.Key] = group.Subscribe(result));
            }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(5, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnCompleted<string>(320)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(390, "rab   "),
                OnCompleted<string>(420)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(480, "  zab"),
                OnCompleted<string>(510)
            );

            res["qux"].Messages.AssertEqual(
                OnCompleted<string>(570)
            );

            res["FOO"].Messages.AssertEqual(
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Inner_Complete_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), _groupByUntilCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(5, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnCompleted<string>(420)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "),
                OnCompleted<string>(510)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnCompleted<string>(570)
            );

            res["FOO"].Messages.AssertEqual(
                OnNext(470, " OOF"),
                OnNext(530, "    oOf    "),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Inner_Error()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, ex1),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), _groupByUntilCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                scheduler.ScheduleRelative(100, () => innerSubscriptions[group.Key] = group.Subscribe(result));
            }, ex => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(5, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnCompleted<string>(320)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(390, "rab   "),
                OnCompleted<string>(420)
               );

            res["Baz"].Messages.AssertEqual(
                OnNext(480, "  zab"),
                OnCompleted<string>(510)
            );

            res["qux"].Messages.AssertEqual(
                OnError<string>(570, ex1)
            );

            res["FOO"].Messages.AssertEqual(
                OnError<string>(570, ex1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Inner_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), _groupByUntilCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }));

            scheduler.ScheduleAbsolute(400, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   ")
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB ")
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  ")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Inner_KeyThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            var keyInvoked = 0;
            var ex = new Exception();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x =>
            {
                keyInvoked++;
                if (keyInvoked == 6)
                    throw ex;
                return x.Trim();
            }, x => Reverse(x), g => g.Skip(2), _groupByUntilCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(3, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnError<string>(360, ex)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(360, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 360)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Inner_EleThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            var eleInvoked = 0;
            var ex = new Exception();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x =>
            {
                eleInvoked++;
                if (eleInvoked == 6)
                    throw ex;
                return Reverse(x);
            }, g => g.Skip(2), _groupByUntilCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnError<string>(360, ex)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(360, ex)
            );

            res["qux"].Messages.AssertEqual(
                OnError<string>(360, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 360)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Inner_Comparer_EqualsThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, 400, ushort.MaxValue);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), _groupByUntilCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnError<string>(420, comparer.EqualsException)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(420, comparer.EqualsException)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnError<string>(420, comparer.EqualsException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Inner_Comparer_GetHashCodeThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler, ushort.MaxValue, 400);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), _groupByUntilCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, _ => { }));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.Start();

            Assert.AreEqual(4, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnError<string>(420, comparer.HashCodeException)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnError<string>(420, comparer.HashCodeException)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnError<string>(420, comparer.HashCodeException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Outer_Independence()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();
            var outerResults = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), _groupByUntilCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                outerResults.OnNext(group.Key);
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, outerResults.OnError, outerResults.OnCompleted));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.ScheduleAbsolute(320, () => outerSubscription.Dispose());

            scheduler.Start();

            Assert.AreEqual(2, inners.Count);

            outerResults.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "baR")
            );

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnCompleted<string>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Inner_Independence()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();
            var outerResults = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), _groupByUntilCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                outerResults.OnNext(group.Key);
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, outerResults.OnError, outerResults.OnCompleted));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());

            scheduler.Start();

            Assert.AreEqual(5, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab"),
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "),
                OnCompleted<string>(420)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB "),
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "),
                OnCompleted<string>(510)
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  "),
                OnCompleted<string>(570)
            );

            res["FOO"].Messages.AssertEqual(
                OnNext(470, " OOF"),
                OnNext(530, "    oOf    "),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Inner_Multiple_Independence()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var comparer = new GroupByComparer(scheduler);
            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inners = new Dictionary<string, IObservable<string>>();
            var innerSubscriptions = new Dictionary<string, IDisposable>();
            var res = new Dictionary<string, ITestableObserver<string>>();
            var outerResults = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), _groupByUntilCapacity, comparer));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                outerResults.OnNext(group.Key);
                var result = scheduler.CreateObserver<string>();
                inners[group.Key] = group;
                res[group.Key] = result;
                innerSubscriptions[group.Key] = group.Subscribe(result);
            }, outerResults.OnError, outerResults.OnCompleted));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions.Values)
                    d.Dispose();
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());
            scheduler.ScheduleAbsolute(280, () => innerSubscriptions["baR"].Dispose());
            scheduler.ScheduleAbsolute(355, () => innerSubscriptions["Baz"].Dispose());
            scheduler.ScheduleAbsolute(400, () => innerSubscriptions["qux"].Dispose());

            scheduler.Start();

            Assert.AreEqual(5, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnNext(220, "oof  "),
                OnNext(240, " OoF "),
                OnNext(310, " Oof"),
                OnCompleted<string>(310)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(270, "  Rab")
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(350, "   zaB ")
            );

            res["qux"].Messages.AssertEqual(
                OnNext(360, " xuq  ")
            );

            res["FOO"].Messages.AssertEqual(
                OnNext(470, " OOF"),
                OnNext(530, "    oOf    "),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Inner_Escape_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(310, "foO "),
                OnNext(470, "FOO "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570)
            );

            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inner = default(IObservable<string>);
            var innerSubscription = default(IDisposable);
            var res = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), g => g.Skip(2), _groupByUntilCapacity));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                inner = group;
            }));

            scheduler.ScheduleAbsolute(600, () => innerSubscription = inner.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                innerSubscription.Dispose();
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            res.Messages.AssertEqual(
                OnCompleted<string>(600)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Inner_Escape_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(310, "foO "),
                OnNext(470, "FOO "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, ex)
            );

            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inner = default(IObservable<string>);
            var innerSubscription = default(IDisposable);
            var res = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), g => g.Skip(2), _groupByUntilCapacity));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                inner = group;
            }, _ => { }));

            scheduler.ScheduleAbsolute(600, () => innerSubscription = inner.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                outerSubscription.Dispose();
                innerSubscription.Dispose();
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            res.Messages.AssertEqual(
                OnError<string>(600, ex)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Inner_Escape_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(310, "foO "),
                OnNext(470, "FOO "),
                OnNext(530, "    fOo    "),
                OnError<string>(570, new Exception())
            );

            var outer = default(IObservable<IGroupedObservable<string, string>>);
            var outerSubscription = default(IDisposable);
            var inner = default(IObservable<string>);
            var innerSubscription = default(IDisposable);
            var res = scheduler.CreateObserver<string>();

            scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), g => g.Skip(2), _groupByUntilCapacity));

            scheduler.ScheduleAbsolute(Subscribed, () => outerSubscription = outer.Subscribe(group =>
            {
                inner = group;
            }));

            scheduler.ScheduleAbsolute(290, () => outerSubscription.Dispose());

            scheduler.ScheduleAbsolute(600, () => innerSubscription = inner.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () =>
            {
                innerSubscription.Dispose();
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_Default()
        {
            var scheduler = new TestScheduler();

            var keyInvoked = 0;
            var eleInvoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(90, "error"),
                OnNext(110, "error"),
                OnNext(130, "error"),
                OnNext(220, "  foo"),
                OnNext(240, " FoO "),
                OnNext(270, "baR  "),
                OnNext(310, "foO "),
                OnNext(350, " Baz   "),
                OnNext(360, "  qux "),
                OnNext(390, "   bar"),
                OnNext(420, " BAR  "),
                OnNext(470, "FOO "),
                OnNext(480, "baz  "),
                OnNext(510, " bAZ "),
                OnNext(530, "    fOo    "),
                OnCompleted<string>(570),
                OnNext(580, "error"),
                OnCompleted<string>(600),
                OnError<string>(650, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.GroupByUntil(
                    x =>
                    {
                        keyInvoked++;
                        return x.Trim().ToLower();
                    },
                    x =>
                    {
                        eleInvoked++;
                        return Reverse(x);
                    },
                    g => g.Skip(2),
                    _groupByUntilCapacity
                ).Select(g => g.Key)
            );

            res.Messages.AssertEqual(
                OnNext(220, "foo"),
                OnNext(270, "bar"),
                OnNext(350, "baz"),
                OnNext(360, "qux"),
                OnNext(470, "foo"),
                OnCompleted<string>(570)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 570)
            );

            Assert.AreEqual(12, keyInvoked);
            Assert.AreEqual(12, eleInvoked);
        }

        [TestMethod]
        public void GroupByUntil_Capacity_DurationSelector_Throws()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "foo")
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.GroupByUntil<string, string, string>(x => x, g => { throw ex; }, _groupByUntilCapacity)
            );

            res.Messages.AssertEqual(
                OnError<IGroupedObservable<string, string>>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_NullKeys_Simple_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "bar"),
                OnNext(240, "foo"),
                OnNext(310, "qux"),
                OnNext(470, "baz"),
                OnCompleted<string>(500)
            );

            var res = scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => Observable.Never<Unit>(), _groupByUntilCapacity).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

            res.Messages.AssertEqual(
                OnNext(220, "(null)bar"),
                OnNext(240, "FOOfoo"),
                OnNext(310, "QUXqux"),
                OnNext(470, "(null)baz"),
                OnCompleted<string>(500)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_NullKeys_Simple_Expire1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "bar"),
                OnNext(240, "foo"),
                OnNext(310, "qux"),
                OnNext(470, "baz"),
                OnCompleted<string>(500)
            );

            var n = 0;
            var res = scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => { if (g.Key == null) n++; return Observable.Timer(TimeSpan.FromTicks(50), scheduler); }, _groupByUntilCapacity).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

            Assert.AreEqual(2, n);

            res.Messages.AssertEqual(
                OnNext(220, "(null)bar"),
                OnNext(240, "FOOfoo"),
                OnNext(310, "QUXqux"),
                OnNext(470, "(null)baz"),
                OnCompleted<string>(500)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_NullKeys_Simple_Expire2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "bar"),
                OnNext(240, "foo"),
                OnNext(310, "qux"),
                OnNext(470, "baz"),
                OnCompleted<string>(500)
            );

            var n = 0;
            var res = scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => { if (g.Key == null) n++; return Observable.Timer(TimeSpan.FromTicks(50), scheduler).IgnoreElements(); }, _groupByUntilCapacity).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

            Assert.AreEqual(2, n);

            res.Messages.AssertEqual(
                OnNext(220, "(null)bar"),
                OnNext(240, "FOOfoo"),
                OnNext(310, "QUXqux"),
                OnNext(470, "(null)baz"),
                OnCompleted<string>(500)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [TestMethod]
        public void GroupByUntil_Capacity_NullKeys_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, "bar"),
                OnNext(240, "foo"),
                OnNext(310, "qux"),
                OnNext(470, "baz"),
                OnError<string>(500, ex)
            );

            var nullGroup = scheduler.CreateObserver<string>();
            var err = default(Exception);

            scheduler.ScheduleAbsolute(200, () => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => Observable.Never<Unit>(), _groupByUntilCapacity).Where(g => g.Key == null).Subscribe(g => g.Subscribe(nullGroup), ex_ => err = ex_));
            scheduler.Start();

            Assert.AreSame(ex, err);

            nullGroup.Messages.AssertEqual(
                OnNext(220, "bar"),
                OnNext(470, "baz"),
                OnError<string>(500, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        #endregion

        #region + GroupJoin +

        [TestMethod]
        public void GroupJoinOp_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupJoin(null, DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupJoin(DummyObservable<int>.Instance, null, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupJoin(DummyObservable<int>.Instance, DummyObservable<int>.Instance, default(Func<int, IObservable<int>>), DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupJoin(DummyObservable<int>.Instance, DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, default(Func<int, IObservable<int>>), DummyFunc<int, IObservable<int>, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupJoin(DummyObservable<int>.Instance, DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>>.Instance, default(Func<int, IObservable<int>, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupJoin(DummyObservable<int>.Instance, DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>, int>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void GroupJoinOp_Normal_I()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(280))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnNext(722, "7rat"),
                OnNext(722, "6rat"),
                OnNext(722, "8rat"),
                OnNext(732, "7wig"),
                OnNext(732, "8wig"),
                OnNext(830, "9rat"),
                OnCompleted<string>(990)
            );

            AssertDurations(xs, xsd, 990);
            AssertDurations(ys, ysd, 990);

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 990)
            );
#endif

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 800)
            );
#else
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 990)
            );
#endif
        }

        [TestMethod]
        public void GroupJoinOp_Normal_II()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(200))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnCompleted<TimeInterval<int>>(721)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(990)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnNext(722, "7rat"),
                OnNext(722, "6rat"),
                OnNext(722, "8rat"),
                OnNext(732, "7wig"),
                OnNext(732, "8wig"),
                OnCompleted<string>(910)
            );

            AssertDurations(xs, xsd, 910);
            AssertDurations(ys, ysd, 910);

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 721)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 910)
            );
#endif

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 910)
            );
        }

        [TestMethod]
        public void GroupJoinOp_Normal_III()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(280))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler).Where(_ => false), y => NewTimer(ysd, y.Interval, scheduler).Where(_ => false), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnNext(722, "7rat"),
                OnNext(722, "6rat"),
                OnNext(722, "8rat"),
                OnNext(732, "7wig"),
                OnNext(732, "8wig"),
                OnNext(830, "9rat"),
                OnCompleted<string>(990)
            );

            AssertDurations(xs, xsd, 990);
            AssertDurations(ys, ysd, 990);

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 990)
            );
#endif

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 800)
            );
#else
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 990)
            );
#endif
        }

        [TestMethod]
        public void GroupJoinOp_Normal_IV()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(200))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnCompleted<TimeInterval<int>>(990)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(980)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnNext(722, "7rat"),
                OnNext(722, "6rat"),
                OnNext(722, "8rat"),
                OnNext(732, "7wig"),
                OnNext(732, "8wig"),
                OnCompleted<string>(990)
            );

            AssertDurations(xs, xsd, 990);
            AssertDurations(ys, ysd, 990);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 990)
            );

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 980)
            );
#else
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 990)
            );
#endif
        }

        [TestMethod]
        public void GroupJoinOp_Normal_V()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(200))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnCompleted<TimeInterval<int>>(990)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(900)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnNext(722, "7rat"),
                OnNext(722, "6rat"),
                OnNext(722, "8rat"),
                OnNext(732, "7wig"),
                OnNext(732, "8wig"),
                OnCompleted<string>(990)
            );

            AssertDurations(xs, xsd, 990);
            AssertDurations(ys, ysd, 990);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 990)
            );

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );
#else
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 990)
            );
#endif
        }

        [TestMethod]
        public void GroupJoinOp_Normal_VI()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(30))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(200))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(850)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(20))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(900)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnNext(722, "7rat"),
                OnNext(722, "6rat"),
                OnNext(722, "8rat"),
                OnNext(732, "7wig"),
                OnNext(732, "8wig"),
                OnCompleted<string>(920)
            );

            AssertDurations(xs, xsd, 920);
            AssertDurations(ys, ysd, 920);

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 850)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 920)
            );
#endif

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );
#else
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 920)
            );
#endif
        }

        [TestMethod]
        public void GroupJoinOp_Normal_VII()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<TimeInterval<int>>(210)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(20))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(900)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
            );

            res.Messages.AssertEqual(
                OnCompleted<string>(210)
            );

            AssertDurations(xs, xsd, 210);
            AssertDurations(ys, ysd, 210);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void GroupJoinOp_Normal_VIII()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(200)))
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, new TimeInterval<string>("hat", TimeSpan.FromTicks(100))),
                OnCompleted<TimeInterval<string>>(230)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
            );

            res.Messages.AssertEqual(
                OnNext(220, "0hat")
            );

            AssertDurations(xs, xsd, 1000);
            AssertDurations(ys, ysd, 1000);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
#else
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
#endif
        }

        [TestMethod]
        public void GroupJoinOp_Normal_IX()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge(),
                713
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man")
            );

            AssertDurations(xs, xsd, 713);
            AssertDurations(ys, ysd, 713);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 713)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 713)
            );
        }

        [TestMethod]
        public void GroupJoinOp_Error_I()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnError<TimeInterval<int>>(310, ex)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnError<string>(310, ex)
            );

            AssertDurations(xs, xsd, 310);
            AssertDurations(ys, ysd, 310);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [TestMethod]
        public void GroupJoinOp_Error_II()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnError<TimeInterval<string>>(722, ex)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnError<string>(722, ex)
            );

            AssertDurations(xs, xsd, 722);
            AssertDurations(ys, ysd, 722);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 722)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 722)
            );
        }

        [TestMethod]
        public void GroupJoinOp_Error_III()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var ex = new Exception();

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler).SelectMany(x.Value == 6 ? Observable.Throw<long>(ex) : Observable.Empty<long>()), y => NewTimer(ysd, y.Interval, scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnNext(722, "7rat"),
                OnNext(722, "6rat"),
                OnNext(722, "8rat"),
                OnError<string>(725, ex)
            );

            AssertDurations(xs, xsd, 725);
            AssertDurations(ys, ysd, 725);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 725)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 725)
            );
        }

        [TestMethod]
        public void GroupJoinOp_Error_IV()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(19))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var ex = new Exception();

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler).SelectMany(y.Value == "tin" ? Observable.Throw<long>(ex) : Observable.Empty<long>()), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnError<string>(721, ex)
            );

            AssertDurations(xs, xsd, 721);
            AssertDurations(ys, ysd, 721);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 721)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 721)
            );
        }

        [TestMethod]
        public void GroupJoinOp_Error_V()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var ex = new Exception();

            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => { if (x.Value >= 0) throw ex; return Observable.Empty<long>(); }, y => NewTimer(ysd, y.Interval, scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
            );

            res.Messages.AssertEqual(
                OnError<string>(210, ex)
            );

            AssertDurations(ys, ysd, 210);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void GroupJoinOp_Error_VI()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var ex = new Exception();

            var xsd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler), y => { if (y.Value.Length >= 0) throw ex; return Observable.Empty<long>(); }, (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
            );

            res.Messages.AssertEqual(
                OnError<string>(215, ex)
            );

            AssertDurations(xs, xsd, 215);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [TestMethod]
        public void GroupJoinOp_Error_VII()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var ex = new Exception();

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, yy) => { if (x.Value >= 0) throw ex; return yy.Select(y => x.Value + y.Value); }).Merge()
            );

            res.Messages.AssertEqual(
                OnError<string>(215, ex)
            );

#if !NO_PERF // BREAKING CHANGE v2 > v1.x - Duration selector is now invoked before the result selector
            AssertDurations(xs, xsd, 215);
#endif
            AssertDurations(ys, ysd, 215);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [TestMethod]
        public void GroupJoinOp_Error_VIII()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var ex = new Exception();

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, yy) => { if (x.Value >= 0) throw ex; return yy.Select(y => x.Value + y.Value); }).Merge()
            );

            res.Messages.AssertEqual(
                OnError<string>(210, ex)
            );

#if !NO_PERF // BREAKING CHANGE v2 > v1.x - Duration selector is now invoked before the result selector
            AssertDurations(xs, xsd, 210);
#endif
            AssertDurations(ys, ysd, 210);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        #endregion

        #region + Join +

        [TestMethod]
        public void JoinOp_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Join(null, DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Join(DummyObservable<int>.Instance, null, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Join(DummyObservable<int>.Instance, DummyObservable<int>.Instance, default(Func<int, IObservable<int>>), DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Join(DummyObservable<int>.Instance, DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, default(Func<int, IObservable<int>>), DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Join(DummyObservable<int>.Instance, DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>>.Instance, default(Func<int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Join(DummyObservable<int>.Instance, DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, int, int>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void JoinOp_Normal_I()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnNext(722, "7rat"),
                OnNext(722, "6rat"),
                OnNext(722, "8rat"),
                OnNext(732, "7wig"),
                OnNext(732, "8wig"),
                OnNext(830, "9rat"),
                OnCompleted<string>(900)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 800)
            );
#else
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );
#endif

            AssertDurations(xs, xsd, 900);
            AssertDurations(ys, ysd, 900);
        }

        [TestMethod]
        public void JoinOp_Normal_II()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(200))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnCompleted<TimeInterval<int>>(721)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(990)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnNext(722, "7rat"),
                OnNext(722, "6rat"),
                OnNext(722, "8rat"),
                OnNext(732, "7wig"),
                OnNext(732, "8wig"),
                OnCompleted<string>(910)
            );

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 721)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 910)
            );
#endif

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 910)
            );

            AssertDurations(xs, xsd, 910);
            AssertDurations(ys, ysd, 910);
        }

        [TestMethod]
        public void JoinOp_Normal_III()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler).Where(_ => false), y => NewTimer(ysd, y.Interval, scheduler).Where(_ => false), (x, y) => x.Value + y.Value)
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnNext(722, "7rat"),
                OnNext(722, "6rat"),
                OnNext(722, "8rat"),
                OnNext(732, "7wig"),
                OnNext(732, "8wig"),
                OnNext(830, "9rat"),
                OnCompleted<string>(900)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 800)
            );
#else
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );
#endif

            AssertDurations(xs, xsd, 900);
            AssertDurations(ys, ysd, 900);
        }

        [TestMethod]
        public void JoinOp_Normal_IV()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(200))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnCompleted<TimeInterval<int>>(990)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(980)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnNext(722, "7rat"),
                OnNext(722, "6rat"),
                OnNext(722, "8rat"),
                OnNext(732, "7wig"),
                OnNext(732, "8wig"),
                OnCompleted<string>(980)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 980)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 980)
            );

            AssertDurations(xs, xsd, 980);
            AssertDurations(ys, ysd, 980);
        }

        [TestMethod]
        public void JoinOp_Normal_V()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(200))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnCompleted<TimeInterval<int>>(990)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(900)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnNext(722, "7rat"),
                OnNext(722, "6rat"),
                OnNext(722, "8rat"),
                OnNext(732, "7wig"),
                OnNext(732, "8wig"),
                OnCompleted<string>(922)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 922)
            );

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );
#else
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 922)
            );
#endif

            AssertDurations(xs, xsd, 922);
            AssertDurations(ys, ysd, 922);
        }

        [TestMethod]
        public void JoinOp_Normal_VI()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(30))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(200))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(850)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(20))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(900)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnNext(722, "7rat"),
                OnNext(722, "6rat"),
                OnNext(722, "8rat"),
                OnNext(732, "7wig"),
                OnNext(732, "8wig"),
                OnCompleted<string>(900)
            );

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 850)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );
#endif

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );

            AssertDurations(xs, xsd, 900);
            AssertDurations(ys, ysd, 900);
        }

        [TestMethod]
        public void JoinOp_Normal_VII()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value),
                713
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 713)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 713)
            );

            AssertDurations(xs, xsd, 713);
            AssertDurations(ys, ysd, 713);
        }

        [TestMethod]
        public void JoinOp_Error_I()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnError<TimeInterval<int>>(310, ex)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnError<string>(310, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            AssertDurations(xs, xsd, 310);
            AssertDurations(ys, ysd, 310);
        }

        [TestMethod]
        public void JoinOp_Error_II()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnError<TimeInterval<string>>(722, ex)
            );

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnError<string>(722, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 722)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 722)
            );

            AssertDurations(xs, xsd, 722);
            AssertDurations(ys, ysd, 722);
        }

        [TestMethod]
        public void JoinOp_Error_III()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var ex = new Exception();

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler).SelectMany(x.Value == 6 ? Observable.Throw<long>(ex) : Observable.Empty<long>()), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnNext(722, "7rat"),
                OnNext(722, "6rat"),
                OnNext(722, "8rat"),
                OnError<string>(725, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 725)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 725)
            );

            AssertDurations(xs, xsd, 725);
            AssertDurations(ys, ysd, 725);
        }

        [TestMethod]
        public void JoinOp_Error_IV()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(19))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var ex = new Exception();

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler).SelectMany(y.Value == "tin" ? Observable.Throw<long>(ex) : Observable.Empty<long>()), (x, y) => x.Value + y.Value)
            );

            res.Messages.AssertEqual(
                OnNext(215, "0hat"),
                OnNext(217, "0bat"),
                OnNext(219, "1hat"),
                OnNext(300, "3wag"),
                OnNext(300, "3pig"),
                OnNext(305, "3cup"),
                OnNext(310, "4wag"),
                OnNext(310, "4pig"),
                OnNext(310, "4cup"),
                OnNext(702, "6tin"),
                OnNext(710, "7tin"),
                OnNext(712, "7man"),
                OnNext(712, "6man"),
                OnNext(720, "8tin"),
                OnNext(720, "8man"),
                OnError<string>(721, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 721)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 721)
            );

            AssertDurations(xs, xsd, 721);
            AssertDurations(ys, ysd, 721);
        }

        [TestMethod]
        public void JoinOp_Error_V()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var ex = new Exception();

            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => { if (x.Value >= 0) throw ex; return Observable.Empty<long>(); }, y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
            );

            res.Messages.AssertEqual(
                OnError<string>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            AssertDurations(ys, ysd, 210);
        }

        [TestMethod]
        public void JoinOp_Error_VI()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var ex = new Exception();

            var xsd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => { if (y.Value.Length >= 0) throw ex; return Observable.Empty<long>(); }, (x, y) => x.Value + y.Value)
            );

            res.Messages.AssertEqual(
                OnError<string>(215, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            AssertDurations(xs, xsd, 215);
        }

        [TestMethod]
        public void JoinOp_Error_VII()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var ex = new Exception();

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => { if (x.Value >= 0) throw ex; return x.Value + y.Value; })
            );

            res.Messages.AssertEqual(
                OnError<string>(215, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            AssertDurations(xs, xsd, 215);
            AssertDurations(ys, ysd, 215);
        }

        [TestMethod]
        public void JoinOp_Error_VIII()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
                OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
                OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
                OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
                OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
                OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
                OnCompleted<TimeInterval<int>>(900)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
                OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
                OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
                OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
                OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
                OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
                OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
                OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
                OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
                OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
                OnCompleted<TimeInterval<string>>(800)
            );

            var ex = new Exception();

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => { if (x.Value >= 0) throw ex; return x.Value + y.Value; })
            );

            res.Messages.AssertEqual(
                OnError<string>(215, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            AssertDurations(xs, xsd, 215);
            AssertDurations(ys, ysd, 215);
        }

        private ITestableObservable<long> NewTimer(List<ITestableObservable<long>> l, TimeSpan t, TestScheduler scheduler)
        {
            var timer = scheduler.CreateColdObservable(OnNext(t.Ticks, 0L), OnCompleted<long>(t.Ticks));
            l.Add(timer);
            return timer;
        }

        private void AssertDurations<T, U>(ITestableObservable<TimeInterval<T>> xs, List<ITestableObservable<U>> xsd, long lastEnd)
        {
            Assert.AreEqual(xs.Messages.Where(x => x.Value.Kind == NotificationKind.OnNext && x.Time <= lastEnd).Count(), xsd.Count);

            foreach (var pair in xs.Messages.Zip(xsd, (x, y) => new { Item1 = x, Item2 = y }))
            {
                var start = pair.Item1.Time;
                var end = Math.Min(start + pair.Item1.Value.Value.Interval.Ticks, lastEnd);
                pair.Item2.Subscriptions.AssertEqual(
                    Subscribe(start, end)
                );
            }
        }

        #endregion

        #region + OfType +

        [TestMethod]
        public void OfType_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OfType<bool>(default(IObservable<object>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OfType<bool>(DummyObservable<object>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void OfType_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<object>(
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
                OnNext<B>(210, new B(0)),
                OnNext<B>(240, new D(3)),
                OnNext<B>(260, new B(5)),
                OnNext<B>(270, new B(6)),
                OnNext<B>(280, new D(7)),
                OnNext<B>(340, new B(10)),
                OnCompleted<B>(350)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [TestMethod]
        public void OfType_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<object>(
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
                OnNext<B>(210, new B(0)),
                OnNext<B>(240, new D(3)),
                OnNext<B>(260, new B(5)),
                OnNext<B>(270, new B(6)),
                OnNext<B>(280, new D(7)),
                OnNext<B>(340, new B(10)),
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

            var xs = scheduler.CreateHotObservable<object>(
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
                OnNext<B>(210, new B(0)),
                OnNext<B>(240, new D(3)),
                OnNext<B>(260, new B(5)),
                OnNext<B>(270, new B(6))
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 275)
            );
        }

        #endregion

        #region + Select +

        [TestMethod]
        public void Select_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).Select<int, int>(DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Select<int, int>((Func<int, int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Select<int, int>(DummyFunc<int, int>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void Select_Throws()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Return(1).Select<int, int>(x => x).Subscribe(
                 x =>
                 {
                     throw new InvalidOperationException();
                 }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Throw<int>(new Exception()).Select<int, int>(x => x).Subscribe(
                 x => { },
                 exception =>
                 {
                     throw new InvalidOperationException();
                 }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                 Observable.Empty<int>().Select<int, int>(x => x).Subscribe(
                 x => { },
                 exception => { },
                 () =>
                 {
                     throw new InvalidOperationException();
                 }));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Create<int>(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).Select(x => x).Subscribe());
        }

        [TestMethod]
        public void Select_DisposeInsideSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(500, 3),
                OnNext(600, 4)
            );

            var invoked = 0;

            var res = scheduler.CreateObserver<int>();

            var d = new SerialDisposable();
            d.Disposable = xs.Select(x =>
            {
                invoked++;
                if (scheduler.Clock > 400)
                    d.Dispose();
                return x;
            }).Subscribe(res);

            scheduler.ScheduleAbsolute(Disposed, d.Dispose);

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(100, 1),
                OnNext(200, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(0, 500)
            );

            Assert.AreEqual(3, invoked);
        }

        [TestMethod]
        public void Select_Completed()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(350, 5),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Select(x =>
                {
                    invoked++;
                    return x + 1;
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(290, 5),
                OnNext(350, 6),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            Assert.AreEqual(4, invoked);
        }

        [TestMethod]
        public void Select_NotCompleted()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(350, 5)
            );

            var res = scheduler.Start(() =>
                xs.Select(x =>
                {
                    invoked++;
                    return x + 1;
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(290, 5),
                OnNext(350, 6)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            Assert.AreEqual(4, invoked);
        }

        [TestMethod]
        public void Select_Error()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(350, 5),
                OnError<int>(400, ex),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Select(x =>
                {
                    invoked++;
                    return x + 1;
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(290, 5),
                OnNext(350, 6),
                OnError<int>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            Assert.AreEqual(4, invoked);
        }

        [TestMethod]
        public void Select_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(350, 5),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Select(x =>
                {
                    invoked++;
                    if (invoked == 3)
                        throw ex;
                    return x + 1;
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnError<int>(290, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );

            Assert.AreEqual(3, invoked);
        }

        [TestMethod]
        public void SelectWithIndex_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).Select<int, int>(DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Select<int, int>((Func<int, int, int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Select<int, int>(DummyFunc<int, int, int>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void SelectWithIndex_Throws()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Return(1).Select<int, int>((x, index) => x).Subscribe(
                 x =>
                 {
                     throw new InvalidOperationException();
                 }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Throw<int>(new Exception()).Select<int, int>((x, index) => x).Subscribe(
                 x => { },
                 exception =>
                 {
                     throw new InvalidOperationException();
                 }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                 Observable.Empty<int>().Select<int, int>((x, index) => x).Subscribe(
                 x => { },
                 exception => { },
                 () =>
                 {
                     throw new InvalidOperationException();
                 }));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Create<int>(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).Select((x, index) => x).Subscribe());
        }

        [TestMethod]
        public void SelectWithIndex_DisposeInsideSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 4),
                OnNext(200, 3),
                OnNext(500, 2),
                OnNext(600, 1)
            );

            var invoked = 0;

            var res = scheduler.CreateObserver<int>();

            var d = new SerialDisposable();
            d.Disposable = xs.Select((x, index) =>
            {
                invoked++;
                if (scheduler.Clock > 400)
                    d.Dispose();
                return x + index * 10;
            }).Subscribe(res);

            scheduler.ScheduleAbsolute(Disposed, d.Dispose);

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(100, 4),
                OnNext(200, 13)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(0, 500)
            );

            Assert.AreEqual(3, invoked);
        }

        [TestMethod]
        public void SelectWithIndex_Completed()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Select((x, index) =>
                {
                    invoked++;
                    return (x + 1) + (index * 10);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 5),
                OnNext(240, 14),
                OnNext(290, 23),
                OnNext(350, 32),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            Assert.AreEqual(4, invoked);
        }

        [TestMethod]
        public void SelectWithIndex_NotCompleted()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1)
            );

            var res = scheduler.Start(() =>
                xs.Select((x, index) =>
                {
                    invoked++;
                    return (x + 1) + (index * 10);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 5),
                OnNext(240, 14),
                OnNext(290, 23),
                OnNext(350, 32)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            Assert.AreEqual(4, invoked);
        }

        [TestMethod]
        public void SelectWithIndex_Error()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnError<int>(400, ex),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Select((x, index) =>
                {
                    invoked++;
                    return (x + 1) + (index * 10);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 5),
                OnNext(240, 14),
                OnNext(290, 23),
                OnNext(350, 32),
                OnError<int>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            Assert.AreEqual(4, invoked);
        }

        [TestMethod]
        public void SelectWithIndex_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Select((x, index) =>
                {
                    invoked++;
                    if (invoked == 3)
                        throw ex;
                    return (x + 1) + (index * 10);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 5),
                OnNext(240, 14),
                OnError<int>(290, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );

            Assert.AreEqual(3, invoked);
        }

        [TestMethod]
        public void Select_Select1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Select(x => x + 1).Select(x => x - 2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4 + 1 - 2),
                OnNext(240, 3 + 1 - 2),
                OnNext(290, 2 + 1 - 2),
                OnNext(350, 1 + 1 - 2),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Select_Select2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Select((x, i) => x + i).Select(x => x - 2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4 + 0 - 2),
                OnNext(240, 3 + 1 - 2),
                OnNext(290, 2 + 2 - 2),
                OnNext(350, 1 + 3 - 2),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Select_Select3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Select(x => x + 1).Select((x, i) => x - i)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4 + 1 - 0),
                OnNext(240, 3 + 1 - 1),
                OnNext(290, 2 + 1 - 2),
                OnNext(350, 1 + 1 - 3),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Select_Select4()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Select((x, i) => x + i).Select((x, i) => x - i)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        #endregion

        #region + SelectMany +

        [TestMethod]
        public void SelectMany_Then_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyObservable<string>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(((IObservable<string>)null)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyObservable<string>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void SelectMany_Then_Complete_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 4),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 1),
                OnCompleted<int>(500)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, "foo"),
                OnNext(100, "bar"),
                OnNext(150, "baz"),
                OnNext(200, "qux"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(ys)
            );

            res.Messages.AssertEqual(
                OnNext(350, "foo"),
                OnNext(400, "bar"),
                OnNext(450, "baz"),
                OnNext(450, "foo"),
                OnNext(500, "qux"),
                OnNext(500, "bar"),
                OnNext(550, "baz"),
                OnNext(550, "foo"),
                OnNext(600, "qux"),
                OnNext(600, "bar"),
                OnNext(650, "baz"),
                OnNext(650, "foo"),
                OnNext(700, "qux"),
                OnNext(700, "bar"),
                OnNext(750, "baz"),
                OnNext(800, "qux"),
                OnCompleted<string>(850)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 700)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 550),
                Subscribe(400, 650),
                Subscribe(500, 750),
                Subscribe(600, 850)
            );
        }

        [TestMethod]
        public void SelectMany_Then_Complete_Complete_2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 4),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 1),
                OnCompleted<int>(700)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, "foo"),
                OnNext(100, "bar"),
                OnNext(150, "baz"),
                OnNext(200, "qux"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(ys)
            );

            res.Messages.AssertEqual(
                OnNext(350, "foo"),
                OnNext(400, "bar"),
                OnNext(450, "baz"),
                OnNext(450, "foo"),
                OnNext(500, "qux"),
                OnNext(500, "bar"),
                OnNext(550, "baz"),
                OnNext(550, "foo"),
                OnNext(600, "qux"),
                OnNext(600, "bar"),
                OnNext(650, "baz"),
                OnNext(650, "foo"),
                OnNext(700, "qux"),
                OnNext(700, "bar"),
                OnNext(750, "baz"),
                OnNext(800, "qux"),
                OnCompleted<string>(900)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 550),
                Subscribe(400, 650),
                Subscribe(500, 750),
                Subscribe(600, 850)
            );
        }

        [TestMethod]
        public void SelectMany_Then_Never_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 4),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 1),
                OnNext(500, 5),
                OnNext(700, 0)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, "foo"),
                OnNext(100, "bar"),
                OnNext(150, "baz"),
                OnNext(200, "qux"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(ys)
            );

            res.Messages.AssertEqual(
                OnNext(350, "foo"),
                OnNext(400, "bar"),
                OnNext(450, "baz"),
                OnNext(450, "foo"),
                OnNext(500, "qux"),
                OnNext(500, "bar"),
                OnNext(550, "baz"),
                OnNext(550, "foo"),
                OnNext(600, "qux"),
                OnNext(600, "bar"),
                OnNext(650, "baz"),
                OnNext(650, "foo"),
                OnNext(700, "qux"),
                OnNext(700, "bar"),
                OnNext(750, "baz"),
                OnNext(750, "foo"),
                OnNext(800, "qux"),
                OnNext(800, "bar"),
                OnNext(850, "baz"),
                OnNext(900, "qux"),
                OnNext(950, "foo")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 550),
                Subscribe(400, 650),
                Subscribe(500, 750),
                Subscribe(600, 850),
                Subscribe(700, 950),
                Subscribe(900, 1000)
            );
        }

        [TestMethod]
        public void SelectMany_Then_Complete_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 4),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 1),
                OnCompleted<int>(500)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, "foo"),
                OnNext(100, "bar"),
                OnNext(150, "baz"),
                OnNext(200, "qux")
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(ys)
            );

            res.Messages.AssertEqual(
                OnNext(350, "foo"),
                OnNext(400, "bar"),
                OnNext(450, "baz"),
                OnNext(450, "foo"),
                OnNext(500, "qux"),
                OnNext(500, "bar"),
                OnNext(550, "baz"),
                OnNext(550, "foo"),
                OnNext(600, "qux"),
                OnNext(600, "bar"),
                OnNext(650, "baz"),
                OnNext(650, "foo"),
                OnNext(700, "qux"),
                OnNext(700, "bar"),
                OnNext(750, "baz"),
                OnNext(800, "qux")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 700)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 1000),
                Subscribe(400, 1000),
                Subscribe(500, 1000),
                Subscribe(600, 1000)
            );
        }

        [TestMethod]
        public void SelectMany_Then_Complete_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 4),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 1),
                OnCompleted<int>(500)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, "foo"),
                OnNext(100, "bar"),
                OnNext(150, "baz"),
                OnNext(200, "qux"),
                OnError<string>(300, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(ys)
            );

            res.Messages.AssertEqual(
                OnNext(350, "foo"),
                OnNext(400, "bar"),
                OnNext(450, "baz"),
                OnNext(450, "foo"),
                OnNext(500, "qux"),
                OnNext(500, "bar"),
                OnNext(550, "baz"),
                OnNext(550, "foo"),
                OnError<string>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 600),
                Subscribe(400, 600),
                Subscribe(500, 600),
                Subscribe(600, 600)
            );
        }

        [TestMethod]
        public void SelectMany_Then_Error_Complete()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 4),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 1),
                OnError<int>(500, ex)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, "foo"),
                OnNext(100, "bar"),
                OnNext(150, "baz"),
                OnNext(200, "qux"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(ys)
            );

            res.Messages.AssertEqual(
                OnNext(350, "foo"),
                OnNext(400, "bar"),
                OnNext(450, "baz"),
                OnNext(450, "foo"),
                OnNext(500, "qux"),
                OnNext(500, "bar"),
                OnNext(550, "baz"),
                OnNext(550, "foo"),
                OnNext(600, "qux"),
                OnNext(600, "bar"),
                OnNext(650, "baz"),
                OnNext(650, "foo"),
                OnError<string>(700, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 700)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 550),
                Subscribe(400, 650),
                Subscribe(500, 700),
                Subscribe(600, 700)
            );
        }

        [TestMethod]
        public void SelectMany_Then_Error_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 4),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 1),
                OnError<int>(500, new Exception())
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, "foo"),
                OnNext(100, "bar"),
                OnNext(150, "baz"),
                OnNext(200, "qux"),
                OnError<string>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(ys)
            );

            res.Messages.AssertEqual(
                OnNext(350, "foo"),
                OnNext(400, "bar"),
                OnNext(450, "baz"),
                OnNext(450, "foo"),
                OnNext(500, "qux"),
                OnNext(500, "bar"),
                OnError<string>(550, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 550),
                Subscribe(400, 550),
                Subscribe(500, 550)
            );
        }

        [TestMethod]
        public void SelectMany_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany<int, int>(DummyFunc<int, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, IObservable<int>>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void SelectMany_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnCompleted<ITestableObservable<int>>(900)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => x)
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303),
                OnNext(740, 106),
                OnNext(810, 304),
                OnNext(860, 305),
                OnNext(930, 401),
                OnNext(940, 402),
                OnCompleted<int>(960)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 760));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 605));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 960));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
                Subscribe(750, 790));

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
                Subscribe(850, 950));
        }

        [TestMethod]
        public void SelectMany_Complete_InnerNotComplete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnCompleted<ITestableObservable<int>>(900)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => x)
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303),
                OnNext(740, 106),
                OnNext(810, 304),
                OnNext(860, 305),
                OnNext(930, 401),
                OnNext(940, 402)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 760));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 1000));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 960));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
                Subscribe(750, 790));

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
                Subscribe(850, 950));
        }

        [TestMethod]
        public void SelectMany_Complete_OuterNotComplete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100)))
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => x)
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303),
                OnNext(740, 106),
                OnNext(810, 304),
                OnNext(860, 305),
                OnNext(930, 401),
                OnNext(940, 402)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 760));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 605));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 960));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
                Subscribe(750, 790));

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
                Subscribe(850, 950));
        }

        [TestMethod]
        public void SelectMany_Error_Outer()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnError<ITestableObservable<int>>(900, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => x)
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303),
                OnNext(740, 106),
                OnNext(810, 304),
                OnNext(860, 305),
                OnError<int>(900, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 760));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 605));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 900));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
                Subscribe(750, 790));

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
                Subscribe(850, 900));
        }

        [TestMethod]
        public void SelectMany_Error_Inner()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnError<int>(460, ex))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnCompleted<ITestableObservable<int>>(900)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => x)
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303),
                OnNext(740, 106),
                OnError<int>(760, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 760));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 760));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 605));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 760));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
                Subscribe(750, 760));

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void SelectMany_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnCompleted<ITestableObservable<int>>(900)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => x),
                700
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 700));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 700));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 605));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 700));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
            );

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void SelectMany_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnCompleted<ITestableObservable<int>>(900)
            );

            var invoked = 0;

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x =>
                {
                    invoked++;
                    if (invoked == 3)
                        throw ex;
                    return x;
                })
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnError<int>(550, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 550));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 550));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
            );

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
            );

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
            );

            Assert.AreEqual(3, invoked);
        }

        [TestMethod]
        public void SelectMany_UseFunction()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 4),
                OnNext(220, 3),
                OnNext(250, 5),
                OnNext(270, 1),
                OnCompleted<int>(290)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => Observable.Interval(TimeSpan.FromTicks(10), scheduler).Select(_ => x).Take(x))
            );

            res.Messages.AssertEqual(
                OnNext(220, 4),
                OnNext(230, 3),
                OnNext(230, 4),
                OnNext(240, 3),
                OnNext(240, 4),
                OnNext(250, 3),
                OnNext(250, 4),
                OnNext(260, 5),
                OnNext(270, 5),
                OnNext(280, 1),
                OnNext(280, 5),
                OnNext(290, 5),
                OnNext(300, 5),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );
        }

        [TestMethod]
        // Tests this overload:
        // IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector);
        public void SelectMany_WithIndex_Complete()
        {
            var scheduler = new TestScheduler();

            ITestableObservable<char> cs = scheduler.CreateHotObservable(
                 OnNext(190, 'h'),   // Test scheduler starts pushing events at time 200, so this is ignored.
                 OnNext(250, 'a'),
                 OnNext(270, 'l'),
                 OnNext(310, 'o'),
                 OnCompleted<char>(410)
                 );

            var res = scheduler.Start(() =>
                cs.SelectMany(
                    (x, i) => Observable.Return(new { x, i }, scheduler)
                ));

            res.Messages.AssertEqual(
                OnNext(251, new { x = 'a', i = 0 }),
                OnNext(271, new { x = 'l', i = 1 }),
                OnNext(311, new { x = 'o', i = 2 }),
                OnCompleted(new { x = default(char), i = default(int) }, 410)
            );

            cs.Subscriptions.AssertEqual(
                Subscribe(200, 410));
        }

        [TestMethod]
        // Tests this overload:
        // IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector);
        public void SelectMany_WithIndex_IEnumerable_Complete()
        {
            var scheduler = new TestScheduler();

            ITestableObservable<char> cs = scheduler.CreateHotObservable(
                 OnNext(190, 'h'),   // Test scheduler starts pushing events at time 200, so this is ignored.
                 OnNext(250, 'a'),
                 OnNext(270, 'l'),
                 OnNext(310, 'o'),
                 OnCompleted<char>(410)
                 );

            var res = scheduler.Start(() =>
                cs.SelectMany(
                    (c, i) => new[] { new { c = c, i = i } }
                ));


            res.Messages.AssertEqual(
                OnNext(250, new { c = 'a', i = 0 }),
                OnNext(270, new { c = 'l', i = 1 }),
                OnNext(310, new { c = 'o', i = 2 }),
                OnCompleted(new { c = default(char), i = default(int) }, 410)
            );

            cs.Subscriptions.AssertEqual(
                Subscribe(200, 410));
        }


        [TestMethod]
        // Tests this overload:
        // IObservable<TResult> SelectMany<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TCollection>> collectionSelector, Func<TSource, TCollection, int, TResult> resultSelector);
        public void SelectMany_WithIndex_IObservable_ResultSelector_Complete()
        {
            var scheduler = new TestScheduler();

            ITestableObservable<ITestableObservable<char>> css = scheduler.CreateHotObservable(
                 OnNext(190, scheduler.CreateColdObservable(
                        OnNext(1, 'h'),
                        OnCompleted<char>(2))),
                 OnNext(250, scheduler.CreateColdObservable(
                        OnNext(1, 'a'),
                        OnCompleted<char>(2))),
                 OnNext(270, scheduler.CreateColdObservable(
                        OnNext(1, 'l'),
                        OnCompleted<char>(2))),
                 OnNext(310, scheduler.CreateColdObservable(
                        OnNext(1, 'o'),
                        OnNext(2, ' '),
                        OnNext(3, 'r'),
                        OnNext(4, 'u'),
                        OnNext(5, 'l'),
                        OnNext(6, 'e'),
                        OnNext(7, 'z'),
                        OnCompleted<char>(8))),
                 OnCompleted<ITestableObservable<char>>(410)
                 );

            var res = scheduler.Start(() =>
                css.SelectMany(
                    (foo, i) =>
                    {
                        return foo.Select(c => new { c = c, i = i });
                    },
                    (source, i, cs, j) => new { c = cs.c, i = cs.i, i2 = i, j = j }
                ));

            res.Messages.AssertEqual(
                OnNext(251, new { c = 'a', i = 0, i2 = 0, j = 0 }),
                OnNext(271, new { c = 'l', i = 1, i2 = 1, j = 0 }),
                OnNext(311, new { c = 'o', i = 2, i2 = 2, j = 0 }),
                OnNext(312, new { c = ' ', i = 2, i2 = 2, j = 1 }),
                OnNext(313, new { c = 'r', i = 2, i2 = 2, j = 2 }),
                OnNext(314, new { c = 'u', i = 2, i2 = 2, j = 3 }),
                OnNext(315, new { c = 'l', i = 2, i2 = 2, j = 4 }),
                OnNext(316, new { c = 'e', i = 2, i2 = 2, j = 5 }),
                OnNext(317, new { c = 'z', i = 2, i2 = 2, j = 6 }),
                OnCompleted(new { c = 'a', i = 0, i2 = 0, j = 0 }, 410)
            );

            css.Subscriptions.AssertEqual(
                Subscribe(200, 410));
        }


        [TestMethod]
        // Tests this overload:
        // IObservable<TResult> SelectMany<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector);
        public void SelectMany_WithIndex_IEnumerable_ResultSelector_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 5),
                OnNext(340, 4),
                OnNext(420, 3),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                        xs.SelectMany(
                          (x, i) => new[] { new { x = x + 1, i = i }, new { x = -x, i = i }, new { x = x * x, i = i } },
                          (x, i, y, j) => new { x = x, i = i, y = y.x, y_i = y.i, j = j })
            );

            res.Messages.AssertEqual(
                OnNext(210, new { x = 5, i = 0, y = 6, y_i = 0, j = 0 }),
                OnNext(210, new { x = 5, i = 0, y = -5, y_i = 0, j = 1 }),
                OnNext(210, new { x = 5, i = 0, y = 25, y_i = 0, j = 2 }),
                OnNext(340, new { x = 4, i = 1, y = 5, y_i = 1, j = 0 }),
                OnNext(340, new { x = 4, i = 1, y = -4, y_i = 1, j = 1 }),
                OnNext(340, new { x = 4, i = 1, y = 16, y_i = 1, j = 2 }),
                OnNext(420, new { x = 3, i = 2, y = 4, y_i = 2, j = 0 }),
                OnNext(420, new { x = 3, i = 2, y = -3, y_i = 2, j = 1 }),
                OnNext(420, new { x = 3, i = 2, y = 9, y_i = 2, j = 2 }),
                OnCompleted(new { x = default(int), i = default(int), y = default(int), y_i = default(int), j = default(int) }, 600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        // Tests this overload:
        // IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> onNext, Func<Exception, int, IObservable<TResult>> onError, Func<int, IObservable<TResult>> onCompleted);
        public void SelectMany_WithIndex_Triple_Complete()
        {
            var scheduler = new TestScheduler();

            ITestableObservable<char> cs = scheduler.CreateHotObservable(
                 OnNext(190, 'h'),   // Test scheduler starts pushing events at time 200, so this is ignored.
                 OnNext(250, 'a'),
                 OnNext(270, 'l'),
                 OnNext(310, 'o'),
                 OnCompleted<char>(410)
                 );

            var res = scheduler.Start(() =>
                cs.SelectMany(
                    (c, i) => Observable.Return(new { c = c, i = i }, scheduler),
                    (ex, i) => { throw ex; },
                    (i) => Observable.Repeat(new { c = 'x', i = -1 }, i, scheduler)
                ));

            res.Messages.AssertEqual(
                OnNext(251, new { c = 'a', i = 0 }),
                OnNext(271, new { c = 'l', i = 1 }),
                OnNext(311, new { c = 'o', i = 2 }),
                OnCompleted(new { c = default(char), i = default(int) }, 410)
            );

            cs.Subscriptions.AssertEqual(
                Subscribe(200, 410));
        }


        [TestMethod]
        public void SelectMany_Enumerable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany<int, int>(DummyFunc<int, IEnumerable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, IEnumerable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, IEnumerable<int>>.Instance).Subscribe(null));

            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany<int, int, int>(DummyFunc<int, IEnumerable<int>>.Instance, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, IEnumerable<int>>)null, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, IEnumerable<int>>.Instance, (Func<int, int, int>)null));
        }

        [TestMethod]
        public void SelectMany_Enumerable_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var inners = new List<MockEnumerable<int>>();

            var res = scheduler.Start(() =>
                xs.SelectMany(x =>
                {
                    var ys = new MockEnumerable<int>(scheduler, Enumerable.Repeat(x, x));
                    inners.Add(ys);
                    return ys;
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(420, 3),
                OnNext(420, 3),
                OnNext(510, 2),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.AreEqual(4, inners.Count);

            inners[0].Subscriptions.AssertEqual(
                Subscribe(210, 210)
            );

            inners[1].Subscriptions.AssertEqual(
                Subscribe(340, 340)
            );

            inners[2].Subscriptions.AssertEqual(
                Subscribe(420, 420)
            );

            inners[3].Subscriptions.AssertEqual(
                Subscribe(510, 510)
            );
        }

        [TestMethod]
        public void SelectMany_Enumerable_Complete_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => Enumerable.Repeat(x, x), (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(210, 4),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(420, 6),
                OnNext(420, 6),
                OnNext(420, 6),
                OnNext(510, 4),
                OnNext(510, 4),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void SelectMany_Enumerable_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnError<int>(600, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => Enumerable.Repeat(x, x))
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(420, 3),
                OnNext(420, 3),
                OnNext(510, 2),
                OnNext(510, 2),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void SelectMany_Enumerable_Error_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnError<int>(600, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => Enumerable.Repeat(x, x), (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(210, 4),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(420, 6),
                OnNext(420, 6),
                OnNext(420, 6),
                OnNext(510, 4),
                OnNext(510, 4),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void SelectMany_Enumerable_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => Enumerable.Repeat(x, x)),
                350
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [TestMethod]
        public void SelectMany_Enumerable_Dispose_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => Enumerable.Repeat(x, x), (x, y) => x + y),
                350
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(210, 4),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [TestMethod]
        public void SelectMany_Enumerable_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var invoked = 0;
            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x =>
                {
                    invoked++;
                    if (invoked == 3)
                        throw ex;

                    return Enumerable.Repeat(x, x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            Assert.AreEqual(3, invoked);
        }

        [TestMethod]
        public void SelectMany_Enumerable_ResultSelectorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var inners = new List<MockEnumerable<int>>();

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x =>
                    {
                        var ys = new MockEnumerable<int>(scheduler, Enumerable.Repeat(x, x));
                        inners.Add(ys);
                        return ys;
                    },
                    (x, y) =>
                    {
                        if (x == 3)
                            throw ex;

                        return x + y;
                    }
                )
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(210, 4),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            Assert.AreEqual(3, inners.Count);

            inners[0].Subscriptions.AssertEqual(
                Subscribe(210, 210)
            );

            inners[1].Subscriptions.AssertEqual(
                Subscribe(340, 340)
            );

            inners[2].Subscriptions.AssertEqual(
                Subscribe(420, 420)
            );
        }

        [TestMethod]
        public void SelectMany_Enumerable_ResultSelector_GetEnumeratorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x => new RogueEnumerable<int>(ex), (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void SelectMany_Enumerable_SelectorThrows_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var invoked = 0;
            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x =>
                    {
                        invoked++;
                        if (invoked == 3)
                            throw ex;

                        return Enumerable.Repeat(x, x);
                    },
                    (x, y) => x + y
                )
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(210, 4),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            Assert.AreEqual(3, invoked);
        }

        class CurrentThrowsEnumerable<T> : IEnumerable<T>
        {
            IEnumerable<T> e;
            Exception ex;

            public CurrentThrowsEnumerable(IEnumerable<T> e, Exception ex)
            {
                this.e = e;
                this.ex = ex;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new Enumerator(e.GetEnumerator(), ex);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            class Enumerator : IEnumerator<T>
            {
                IEnumerator<T> e;
                Exception ex;

                public Enumerator(IEnumerator<T> e, Exception ex)
                {
                    this.e = e;
                    this.ex = ex;
                }

                public T Current
                {
                    get { throw ex; }
                }

                public void Dispose()
                {
                    e.Dispose();
                }

                object System.Collections.IEnumerator.Current
                {
                    get { return Current; }
                }

                public bool MoveNext()
                {
                    return e.MoveNext();
                }

                public void Reset()
                {
                    e.Reset();
                }
            }
        }

        [TestMethod]
        public void SelectMany_Enumerable_CurrentThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x => new CurrentThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void SelectMany_Enumerable_CurrentThrows_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x => new CurrentThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex), (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        class MoveNextThrowsEnumerable<T> : IEnumerable<T>
        {
            IEnumerable<T> e;
            Exception ex;

            public MoveNextThrowsEnumerable(IEnumerable<T> e, Exception ex)
            {
                this.e = e;
                this.ex = ex;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new Enumerator(e.GetEnumerator(), ex);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            class Enumerator : IEnumerator<T>
            {
                IEnumerator<T> e;
                Exception ex;

                public Enumerator(IEnumerator<T> e, Exception ex)
                {
                    this.e = e;
                    this.ex = ex;
                }

                public T Current
                {
                    get { return e.Current; }
                }

                public void Dispose()
                {
                    e.Dispose();
                }

                object System.Collections.IEnumerator.Current
                {
                    get { return Current; }
                }

                public bool MoveNext()
                {
                    throw ex;
                }

                public void Reset()
                {
                    e.Reset();
                }
            }
        }

        [TestMethod]
        public void SelectMany_Enumerable_GetEnumeratorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x => new RogueEnumerable<int>(ex))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void SelectMany_Enumerable_MoveNextThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x => new MoveNextThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void SelectMany_Enumerable_MoveNextThrows_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x => new MoveNextThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex), (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void SelectMany_QueryOperator_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany<int, int, int>(DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, IObservable<int>>)null, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, IObservable<int>>.Instance, ((Func<int, int, int>)null)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, int, int>.Instance).Subscribe(null));

#if !NO_TPL
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany<int, int, int>(DummyFunc<int, Task<int>>.Instance, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, Task<int>>)null, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, Task<int>>.Instance, ((Func<int, int, int>)null)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany<int, int, int>(DummyFunc<int, CancellationToken, Task<int>>.Instance, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, CancellationToken, Task<int>>)null, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, CancellationToken, Task<int>>.Instance, ((Func<int, int, int>)null)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany<int, int>(DummyFunc<int, Task<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, Task<int>>)null));

            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany<int, int>(DummyFunc<int, CancellationToken, Task<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, CancellationToken, Task<int>>)null));
#endif
        }

        [TestMethod]
        public void SelectMany_QueryOperator_CompleteOuterFirst()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(224)
            );

            var res = scheduler.Start(() =>
                from x in xs
                from y in Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x)
                select x * 10 + (int)y
            );

            res.Messages.AssertEqual(
                OnNext(221, 40),
                OnNext(222, 30),
                OnNext(222, 41),
                OnNext(223, 20),
                OnNext(223, 31),
                OnNext(223, 42),
                OnNext(224, 50),
                OnNext(224, 21),
                OnNext(224, 32),
                OnNext(224, 43),
                OnNext(225, 51),
                OnNext(226, 52),
                OnNext(227, 53),
                OnNext(228, 54),
                OnCompleted<int>(228)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 224)
            );
        }

        [TestMethod]
        public void SelectMany_QueryOperator_CompleteInnerFirst()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                from x in xs
                from y in Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x)
                select x * 10 + (int)y
            );

            res.Messages.AssertEqual(
                OnNext(221, 40),
                OnNext(222, 30),
                OnNext(222, 41),
                OnNext(223, 20),
                OnNext(223, 31),
                OnNext(223, 42),
                OnNext(224, 50),
                OnNext(224, 21),
                OnNext(224, 32),
                OnNext(224, 43),
                OnNext(225, 51),
                OnNext(226, 52),
                OnNext(227, 53),
                OnNext(228, 54),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void SelectMany_QueryOperator_ErrorOuter()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnError<int>(224, ex)
            );

            var res = scheduler.Start(() =>
                from x in xs
                from y in Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x)
                select x * 10 + (int)y
            );

            res.Messages.AssertEqual(
                OnNext(221, 40),
                OnNext(222, 30),
                OnNext(222, 41),
                OnNext(223, 20),
                OnNext(223, 31),
                OnNext(223, 42),
                OnError<int>(224, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 224)
            );
        }

        [TestMethod]
        public void SelectMany_QueryOperator_ErrorInner()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(224)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                from x in xs
                from y in x == 2 ? Observable.Throw<long>(ex, scheduler)
                                 : Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x)
                select x * 10 + (int)y
            );

            res.Messages.AssertEqual(
                OnNext(221, 40),
                OnNext(222, 30),
                OnNext(222, 41),
                OnError<int>(223, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 223)
            );
        }

        [TestMethod]
        public void SelectMany_QueryOperator_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(224)
            );

            var res = scheduler.Start(() =>
                from x in xs
                from y in Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x)
                select x * 10 + (int)y,
                223
            );

            res.Messages.AssertEqual(
                OnNext(221, 40),
                OnNext(222, 30),
                OnNext(222, 41)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 223)
            );
        }

        static T Throw<T>(Exception ex)
        {
            throw ex;
        }


        [TestMethod]
        public void SelectMany_QueryOperator_ThrowSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(224)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                from x in xs
                from y in Throw<IObservable<long>>(ex)
                select x * 10 + (int)y
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void SelectMany_QueryOperator_ThrowResult()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(224)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                from x in xs
                from y in Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x)
                select Throw<int>(ex)
            );

            res.Messages.AssertEqual(
                OnError<int>(221, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 221)
            );
        }

        [TestMethod]
        public void SelectMany_Triple_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(null, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<Exception, IObservable<int>>.Instance, DummyFunc<IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, null, DummyFunc<Exception, IObservable<int>>.Instance, DummyFunc<IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, null, DummyFunc<IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<Exception, IObservable<int>>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<Exception, IObservable<int>>.Instance, DummyFunc<IObservable<int>>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void SelectMany_Triple_Identity()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Return(x, scheduler),
                    ex => Observable.Throw<int>(ex, scheduler),
                    () => Observable.Empty<int>(scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, 0),
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(305, 4),
                OnCompleted<int>(306)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [TestMethod]
        public void SelectMany_Triple_InnersWithTiming1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var ysn = scheduler.CreateColdObservable(
                OnNext(10, 10),
                OnNext(20, 11),
                OnNext(30, 12),
                OnCompleted<int>(40)
            );

            var yse = scheduler.CreateColdObservable(
                OnNext(0, 99),
                OnCompleted<int>(10)
            );

            var ysc = scheduler.CreateColdObservable(
                OnNext(10, 42),
                OnCompleted<int>(20)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => ysn,
                    ex => yse,
                    () => ysc
                )
            );

            res.Messages.AssertEqual(
                OnNext(310, 10),
                OnNext(311, 10),
                OnNext(312, 10),
                OnNext(313, 10),
                OnNext(314, 10),
                OnNext(315, 42),
                OnNext(320, 11),
                OnNext(321, 11),
                OnNext(322, 11),
                OnNext(323, 11),
                OnNext(324, 11),
                OnNext(330, 12),
                OnNext(331, 12),
                OnNext(332, 12),
                OnNext(333, 12),
                OnNext(334, 12),
                OnCompleted<int>(344)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );

            ysn.Subscriptions.AssertEqual(
                Subscribe(300, 340),
                Subscribe(301, 341),
                Subscribe(302, 342),
                Subscribe(303, 343),
                Subscribe(304, 344)
            );

            yse.Subscriptions.AssertEqual(
            );

            ysc.Subscriptions.AssertEqual(
                Subscribe(305, 325)
            );
        }

        [TestMethod]
        public void SelectMany_Triple_InnersWithTiming2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var ysn = scheduler.CreateColdObservable(
                OnNext(10, 10),
                OnNext(20, 11),
                OnNext(30, 12),
                OnCompleted<int>(40)
            );

            var yse = scheduler.CreateColdObservable(
                OnNext(0, 99),
                OnCompleted<int>(10)
            );

            var ysc = scheduler.CreateColdObservable(
                OnNext(10, 42),
                OnCompleted<int>(50)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => ysn,
                    ex => yse,
                    () => ysc
                )
            );

            res.Messages.AssertEqual(
                OnNext(310, 10),
                OnNext(311, 10),
                OnNext(312, 10),
                OnNext(313, 10),
                OnNext(314, 10),
                OnNext(315, 42),
                OnNext(320, 11),
                OnNext(321, 11),
                OnNext(322, 11),
                OnNext(323, 11),
                OnNext(324, 11),
                OnNext(330, 12),
                OnNext(331, 12),
                OnNext(332, 12),
                OnNext(333, 12),
                OnNext(334, 12),
                OnCompleted<int>(355)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );

            ysn.Subscriptions.AssertEqual(
                Subscribe(300, 340),
                Subscribe(301, 341),
                Subscribe(302, 342),
                Subscribe(303, 343),
                Subscribe(304, 344)
            );

            yse.Subscriptions.AssertEqual(
            );

            ysc.Subscriptions.AssertEqual(
                Subscribe(305, 355)
            );
        }

        [TestMethod]
        public void SelectMany_Triple_InnersWithTiming3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(400, 1),
                OnNext(500, 2),
                OnNext(600, 3),
                OnNext(700, 4),
                OnCompleted<int>(800)
            );

            var ysn = scheduler.CreateColdObservable(
                OnNext(10, 10),
                OnNext(20, 11),
                OnNext(30, 12),
                OnCompleted<int>(40)
            );

            var yse = scheduler.CreateColdObservable(
                OnNext(0, 99),
                OnCompleted<int>(10)
            );

            var ysc = scheduler.CreateColdObservable(
                OnNext(10, 42),
                OnCompleted<int>(100)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => ysn,
                    ex => yse,
                    () => ysc
                )
            );

            res.Messages.AssertEqual(
                OnNext(310, 10),
                OnNext(320, 11),
                OnNext(330, 12),
                OnNext(410, 10),
                OnNext(420, 11),
                OnNext(430, 12),
                OnNext(510, 10),
                OnNext(520, 11),
                OnNext(530, 12),
                OnNext(610, 10),
                OnNext(620, 11),
                OnNext(630, 12),
                OnNext(710, 10),
                OnNext(720, 11),
                OnNext(730, 12),
                OnNext(810, 42),
                OnCompleted<int>(900)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 800)
            );

            ysn.Subscriptions.AssertEqual(
                Subscribe(300, 340),
                Subscribe(400, 440),
                Subscribe(500, 540),
                Subscribe(600, 640),
                Subscribe(700, 740)
            );

            yse.Subscriptions.AssertEqual(
            );

            ysc.Subscriptions.AssertEqual(
                Subscribe(800, 900)
            );
        }

        [TestMethod]
        public void SelectMany_Triple_Error_Identity()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnError<int>(305, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Return(x, scheduler),
                    ex1 => Observable.Throw<int>(ex1, scheduler),
                    () => Observable.Empty<int>(scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, 0),
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(305, 4),
                OnError<int>(306, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [TestMethod]
        public void SelectMany_Triple_SelectMany()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Repeat(x, x, scheduler),
                    ex => Observable.Throw<int>(ex, scheduler),
                    () => Observable.Empty<int>(scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnNext(305, 4),
                OnNext(305, 3),
                OnNext(306, 4),
                OnNext(306, 3),
                OnNext(307, 4),
                OnNext(308, 4),
                OnCompleted<int>(308)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }


        [TestMethod]
        public void SelectMany_Triple_Concat()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Return(x, scheduler),
                    ex => Observable.Throw<int>(ex, scheduler),
                    () => Observable.Range(1, 3, scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, 0),
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(305, 4),
                OnNext(306, 1),
                OnNext(307, 2),
                OnNext(308, 3),
                OnCompleted<int>(309)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [TestMethod]
        public void SelectMany_Triple_Catch()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Return(x, scheduler),
                    ex => Observable.Range(1, 3, scheduler),
                    () => Observable.Empty<int>(scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, 0),
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(305, 4),
                OnCompleted<int>(306)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [TestMethod]
        public void SelectMany_Triple_Error_Catch()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnError<int>(305, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Return(x, scheduler),
                    ex => Observable.Range(1, 3, scheduler),
                    () => Observable.Empty<int>(scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, 0),
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(305, 4),
                OnNext(306, 1),
                OnNext(307, 2),
                OnNext(308, 3),
                OnCompleted<int>(309)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [TestMethod]
        public void SelectMany_Triple_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Repeat(x, x, scheduler),
                    ex => Observable.Repeat(0, 2, scheduler),
                    () => Observable.Repeat(-1, 2, scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnNext(305, 4),
                OnNext(305, 3),
                OnNext(306, -1),
                OnNext(306, 4),
                OnNext(306, 3),
                OnNext(307, -1),
                OnNext(307, 4),
                OnNext(308, 4),
                OnCompleted<int>(308)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [TestMethod]
        public void SelectMany_Triple_Error_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnError<int>(305, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Repeat(x, x, scheduler),
                    ex => Observable.Repeat(0, 2, scheduler),
                    () => Observable.Repeat(-1, 2, scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnNext(305, 4),
                OnNext(305, 3),
                OnNext(306, 0),
                OnNext(306, 4),
                OnNext(306, 3),
                OnNext(307, 0),
                OnNext(307, 4),
                OnNext(308, 4),
                OnCompleted<int>(308)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [TestMethod]
        public void SelectMany_Triple_All_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Repeat(x, x, scheduler),
                    ex => Observable.Repeat(0, 2, scheduler),
                    () => Observable.Repeat(-1, 2, scheduler)
                ),
                307
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnNext(305, 4),
                OnNext(305, 3),
                OnNext(306, -1),
                OnNext(306, 4),
                OnNext(306, 3)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [TestMethod]
        public void SelectMany_Triple_All_Dispose_Before_First()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Repeat(x, x, scheduler),
                    ex => Observable.Repeat(0, 2, scheduler),
                    () => Observable.Repeat(-1, 2, scheduler)
                ),
                304
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 304)
            );
        }

        [TestMethod]
        public void SelectMany_Triple_OnNextThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Throw<IObservable<int>>(ex),
                    ex1 => Observable.Repeat(0, 2, scheduler),
                    () => Observable.Repeat(-1, 2, scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void SelectMany_Triple_OnErrorThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnError<int>(305, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Repeat(x, x, scheduler),
                    ex1 => Throw<IObservable<int>>(ex),
                    () => Observable.Repeat(-1, 2, scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnError<int>(305, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [TestMethod]
        public void SelectMany_Triple_OnCompletedThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Repeat(x, x, scheduler),
                    ex1 => Observable.Repeat(0, 2, scheduler),
                    () => Throw<IObservable<int>>(ex)
                )
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnError<int>(305, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

#if !NO_TPL

        [TestMethod]
        public void SelectMany_Task_ArgumentChecking()
        {
            var t = new Task<int>(() => 42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(default(IObservable<int>), x => t));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, default(Func<int, Task<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(default(IObservable<int>), (x, ct) => t));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, default(Func<int, CancellationToken, Task<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(default(IObservable<int>), x => t, (x, y) => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, default(Func<int, Task<int>>), (x, y) => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, x => t, default(Func<int, int, int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(default(IObservable<int>), (x, ct) => t, (x, y) => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, default(Func<int, CancellationToken, Task<int>>), (x, y) => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, (x, ct) => t, default(Func<int, int, int>)));
        }

        [TestMethod]
        public void SelectMany_Task1()
        {
            var res = Observable.Range(0, 10).SelectMany(x => Task.Factory.StartNew(() => x + 1)).ToEnumerable();
            Assert.IsTrue(Enumerable.Range(0, 10).SelectMany(x => new[] { x + 1 }).SequenceEqual(res.OrderBy(x => x)));
        }

        [TestMethod]
        public void SelectMany_Task2()
        {
            var res = Observable.Range(0, 10).SelectMany((x, ct) => Task.Factory.StartNew(() => x + 1, ct)).ToEnumerable();
            Assert.IsTrue(Enumerable.Range(0, 10).SelectMany(x => new[] { x + 1 }).SequenceEqual(res.OrderBy(x => x)));
        }

        [TestMethod]
        public void SelectMany_Task_TaskThrows()
        {
            var ex = new Exception();

            var res = Observable.Range(0, 10).SelectMany(x => Task.Factory.StartNew(() =>
            {
                if (x > 5)
                    throw ex;
                return x + 1;
            })).ToEnumerable();

            ReactiveAssert.Throws(ex, () =>
            {
                foreach (var x in res)
                    ;
            });
        }

        [TestMethod]
        public void SelectMany_Task_SelectorThrows()
        {
            var ex = new Exception();

            var res = Observable.Range(0, 10).SelectMany(x =>
            {
                if (x > 5)
                    throw ex;
                return Task.Factory.StartNew(() => x + 1);
            }).ToEnumerable();

            ReactiveAssert.Throws(ex, () =>
            {
                foreach (var x in res)
                    ;
            });
        }

        [TestMethod]
        public void SelectMany_Task_ResultSelector1()
        {
            var res = Observable.Range(0, 10).SelectMany(x => Task.Factory.StartNew(() => x + 1), (x, y) => x + y).ToEnumerable();
            Assert.IsTrue(Enumerable.Range(0, 10).SelectMany(x => new[] { 2 * x + 1 }).SequenceEqual(res.OrderBy(x => x)));
        }

        [TestMethod]
        public void SelectMany_Task_ResultSelector2()
        {
            var res = Observable.Range(0, 10).SelectMany((x, ct) => Task.Factory.StartNew(() => x + 1, ct), (x, y) => x + y).ToEnumerable();
            Assert.IsTrue(Enumerable.Range(0, 10).SelectMany(x => new[] { 2 * x + 1 }).SequenceEqual(res.OrderBy(x => x)));
        }

        [TestMethod]
        public void SelectMany_Task_ResultSelectorThrows()
        {
            var ex = new Exception();

            var res = Observable.Range(0, 10).SelectMany(x => Task.Factory.StartNew(() => x + 1), (x, y) =>
            {
                if (x > 5)
                    throw ex;
                return x + y;
            }).ToEnumerable();

            ReactiveAssert.Throws(ex, () =>
            {
                foreach (var x in res)
                    ;
            });
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_Simple_RanToCompletion_Async()
        {
            var tcss = new TaskCompletionSource<int>[2];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 2), x => tcss[x].Task);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[0].SetResult(42);
            tcss[1].SetResult(43);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43 });
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_Simple_RanToCompletion_Sync()
        {
            var tcss = new TaskCompletionSource<int>[2];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();

            tcss[0].SetResult(42);
            tcss[1].SetResult(43);

            var res = Observable.SelectMany(Observable.Range(0, 2), x => tcss[x].Task);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43 });
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_Simple_Faulted_Async()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            var ex = new Exception();
            tcss[1].SetException(ex);

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_Simple_Faulted_Sync()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var ex = new Exception();
            tcss[1].SetException(ex);

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_Simple_Canceled_Async()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            tcss[1].SetCanceled();

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.IsTrue(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_Simple_Canceled_Sync()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            tcss[1].SetCanceled();

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.IsTrue(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_Simple_InnerCompleteBeforeOuter()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, x => tcss[x].Task);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            xs.OnCompleted();

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43, 44 });
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_Simple_OuterCompleteBeforeInner()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, x => tcss[x].Task);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);
            xs.OnCompleted();

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43, 44 });
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_Simple_Cancellation_NeverInvoked()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => tcs.SetCanceled());

                return tcs.Task;
            });

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            var d = res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);
            xs.OnCompleted();

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43, 44 });
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_Simple_Cancellation_Invoked()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            });

            var lst = new List<int>();

            var done = false;
            var d = res.Subscribe(lst.Add, () => done = true);

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);

            d.Dispose();

            xs.OnNext(2);
            xs.OnCompleted();

            Assert.IsFalse(tcss[0].TrySetResult(43));
            tcss[2].SetResult(44); // never observed because xs.OnNext(2) happened after dispose

            lst.AssertEqual(new[] { 42 });
            Assert.IsFalse(done);
            Assert.AreEqual(2, n);
            Assert.AreEqual(1, m); // tcss[1] was already finished
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_Simple_Cancellation_AfterOuterError()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            });

            var lst = new List<int>();

            var done = false;
            var err = default(Exception);
            res.Subscribe(lst.Add, ex_ => err = ex_, () => done = true);

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);

            var ex = new Exception();
            xs.OnError(ex);

            Assert.IsFalse(tcss[0].TrySetResult(43));
            tcss[2].SetResult(44); // no-op

            lst.AssertEqual(new[] { 42 });
            Assert.AreSame(ex, err);
            Assert.IsFalse(done);
            Assert.AreEqual(2, n);
            Assert.AreEqual(1, m); // tcss[1] was already finished
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_Simple_Cancellation_AfterSelectorThrows()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[4];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();
            tcss[3] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var ex = new Exception();

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                if (x == 2)
                    throw ex;

                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            });

            var lst = new List<int>();

            var done = false;
            var evt = new ManualResetEvent(false);
            var err = default(Exception);
            res.Subscribe(lst.Add, ex_ => { err = ex_; evt.Set(); }, () => { done = true; evt.Set(); });

            tcss[1].SetResult(43);

            xs.OnNext(0);
            xs.OnNext(1);

            tcss[0].SetResult(42);

            xs.OnNext(2); // causes error
            xs.OnCompleted();

            evt.WaitOne();

            Assert.IsFalse(done);
            Assert.AreSame(ex, err);
            Assert.AreEqual(2, n);
            Assert.AreEqual(0, m);
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_RanToCompletion_Async()
        {
            var tcss = new TaskCompletionSource<int>[2];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 2), x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[0].SetResult(42);
            tcss[1].SetResult(43);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42 + 0, 43 + 1 });
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_RanToCompletion_Sync()
        {
            var tcss = new TaskCompletionSource<int>[2];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();

            tcss[0].SetResult(42);
            tcss[1].SetResult(43);

            var res = Observable.SelectMany(Observable.Range(0, 2), x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42 + 0, 43 + 1 });
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Faulted_Async()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            var ex = new Exception();
            tcss[1].SetException(ex);

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Faulted_Sync()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var ex = new Exception();
            tcss[1].SetException(ex);

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Canceled_Async()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            tcss[1].SetCanceled();

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.IsTrue(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Canceled_Sync()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            tcss[1].SetCanceled();

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.IsTrue(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_InnerCompleteBeforeOuter()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            xs.OnCompleted();

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42 + 1, 43 + 0, 44 + 2 });
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_OuterCompleteBeforeInner()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);
            xs.OnCompleted();

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42 + 1, 43 + 0, 44 + 2 });
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Cancellation_NeverInvoked()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => tcs.SetCanceled());

                return tcs.Task;
            }, (x, y) => x + y);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            var d = res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);
            xs.OnCompleted();

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42 + 1, 43 + 0, 44 + 2 });
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Cancellation_Invoked()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            }, (x, y) => x + y);

            var lst = new List<int>();

            var done = false;
            var d = res.Subscribe(lst.Add, () => done = true);

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);

            d.Dispose();

            xs.OnNext(2);
            xs.OnCompleted();

            Assert.IsFalse(tcss[0].TrySetResult(43));
            tcss[2].SetResult(44); // never observed because xs.OnNext(2) happened after dispose

            lst.AssertEqual(new[] { 42 + 1 });
            Assert.IsFalse(done);
            Assert.AreEqual(2, n);
            Assert.AreEqual(1, m); // tcss[1] was already finished
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Cancellation_AfterOuterError()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            }, (x, y) => x + y);

            var lst = new List<int>();

            var done = false;
            var err = default(Exception);
            res.Subscribe(lst.Add, ex_ => err = ex_, () => done = true);

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);

            var ex = new Exception();
            xs.OnError(ex);

            Assert.IsFalse(tcss[0].TrySetResult(43));
            tcss[2].SetResult(44); // no-op

            lst.AssertEqual(new[] { 42 + 1 });
            Assert.AreSame(ex, err);
            Assert.IsFalse(done);
            Assert.AreEqual(2, n);
            Assert.AreEqual(1, m); // tcss[1] was already finished
        }

        [TestMethod]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Cancellation_AfterSelectorThrows()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[4];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();
            tcss[3] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var ex = new Exception();

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                if (x == 2)
                    throw ex;

                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            }, (x, y) => x + y);

            var lst = new List<int>();

            var done = false;
            var evt = new ManualResetEvent(false);
            var err = default(Exception);
            res.Subscribe(lst.Add, ex_ => { err = ex_; evt.Set(); }, () => { done = true; evt.Set(); });

            tcss[1].SetResult(43);

            xs.OnNext(0);
            xs.OnNext(1);

            tcss[0].SetResult(42);

            xs.OnNext(2); // causes error
            xs.OnCompleted();

            evt.WaitOne();

            Assert.IsFalse(done);
            Assert.AreSame(ex, err);
            Assert.AreEqual(2, n);
            Assert.AreEqual(0, m);
        }

#endif

        #endregion

        #region + Skip +

        [TestMethod]
        public void Skip_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).Skip(0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.Skip(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Skip(0).Subscribe(null));
        }

        [TestMethod]
        public void Skip_Complete_After()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(690)
            );

            var res = scheduler.Start(() =>
                xs.Skip(20)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(690)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 690)
            );
        }

        [TestMethod]
        public void Skip_Complete_Same()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(690)
            );

            var res = scheduler.Start(() =>
                xs.Skip(17)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(690)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 690)
            );
        }

        [TestMethod]
        public void Skip_Complete_Before()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(690)
            );

            var res = scheduler.Start(() =>
                xs.Skip(10)
            );

            res.Messages.AssertEqual(
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(690)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 690)
            );
        }

        [TestMethod]
        public void Skip_Complete_Zero()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(690)
            );

            var res = scheduler.Start(() =>
                xs.Skip(0)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(690)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 690)
            );
        }

        [TestMethod]
        public void Skip_Error_After()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnError<int>(690, ex)
            );

            var res = scheduler.Start(() =>
                xs.Skip(20)
            );

            res.Messages.AssertEqual(
                OnError<int>(690, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 690)
            );
        }

        [TestMethod]
        public void Skip_Error_Same()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnError<int>(690, ex)
            );

            var res = scheduler.Start(() =>
                xs.Skip(17)
            );

            res.Messages.AssertEqual(
                OnError<int>(690, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 690)
            );
        }

        [TestMethod]
        public void Skip_Error_Before()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnError<int>(690, ex)
            );

            var res = scheduler.Start(() =>
                xs.Skip(3)
            );

            res.Messages.AssertEqual(
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnError<int>(690, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 690)
            );
        }

        [TestMethod]
        public void Skip_Dispose_Before()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10)
            );

            var res = scheduler.Start(() =>
                xs.Skip(3),
                250
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Skip_Dispose_After()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10)
            );

            var res = scheduler.Start(() =>
                xs.Skip(3),
                400
            );

            res.Messages.AssertEqual(
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Skip_Skip1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Skip(3).Skip(2)
            );

            res.Messages.AssertEqual(
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        #endregion

        #region + SkipWhile +

        [TestMethod]
        public void SkipWhile_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SkipWhile(DummyFunc<int, bool>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SkipWhile(default(Func<int, bool>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SkipWhile(DummyFunc<int, bool>.Instance).Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SkipWhile((x, i) => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SkipWhile(default(Func<int, int, bool>)));
        }

        [TestMethod]
        public void SkipWhile_Complete_Before()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnCompleted<int>(330),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            var invoked = 0;

            var res = scheduler.Start(() =>
                xs.SkipWhile(x =>
                {
                    invoked++;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(330)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 330)
            );

            Assert.AreEqual(4, invoked);
        }

        [TestMethod]
        public void SkipWhile_Complete_After()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            var invoked = 0;

            var res = scheduler.Start(() =>
                xs.SkipWhile(x =>
                {
                    invoked++;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.AreEqual(6, invoked);
        }

        [TestMethod]
        public void SkipWhile_Error_Before()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(210, 2),
                OnNext(260, 5),
                OnError<int>(270, ex),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23)
            );

            var invoked = 0;

            var res = scheduler.Start(() =>
                xs.SkipWhile(x =>
                {
                    invoked++;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnError<int>(270, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );

            Assert.AreEqual(2, invoked);
        }

        [TestMethod]
        public void SkipWhile_Error_After()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnError<int>(600, ex)
            );

            var invoked = 0;

            var res = scheduler.Start(() =>
                xs.SkipWhile(x =>
                {
                    invoked++;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.AreEqual(6, invoked);
        }

        [TestMethod]
        public void SkipWhile_Dispose_Before()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            var invoked = 0;

            var res = scheduler.Start(() =>
                xs.SkipWhile(x =>
                {
                    invoked++;
                    return IsPrime(x);
                }),
                300
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            Assert.AreEqual(3, invoked);
        }

        [TestMethod]
        public void SkipWhile_Dispose_After()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            var invoked = 0;

            var res = scheduler.Start(() =>
                xs.SkipWhile(x =>
                {
                    invoked++;
                    return IsPrime(x);
                }),
                470
            );

            res.Messages.AssertEqual(
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 470)
            );

            Assert.AreEqual(6, invoked);
        }

        [TestMethod]
        public void SkipWhile_Zero()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(205, 100),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            var invoked = 0;

            var res = scheduler.Start(() =>
                xs.SkipWhile(x =>
                {
                    invoked++;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(205, 100),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.AreEqual(1, invoked);
        }

        [TestMethod]
        public void SkipWhile_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            var invoked = 0;
            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SkipWhile(x =>
                {
                    invoked++;
                    if (invoked == 3)
                        throw ex;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnError<int>(290, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );

            Assert.AreEqual(3, invoked);
        }

        [TestMethod]
        public void SkipWhile_Index()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(205, 100),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.SkipWhile((x, i) => i < 5)
            );

            res.Messages.AssertEqual(
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void SkipWhile_Index_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(205, 100),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnError<int>(400, ex)
            );

            var res = scheduler.Start(() =>
                xs.SkipWhile((x, i) => i < 5)
            );

            res.Messages.AssertEqual(
                OnNext(350, 7),
                OnNext(390, 4),
                OnError<int>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void SkipWhile_Index_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(205, 100),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.SkipWhile((x, i) => { if (i < 5) return true; throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(350, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        #endregion

        #region + Take +

        [TestMethod]
        public void Take_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).Take(0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.Take(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Take(1).Subscribe(null));

            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).Take(0, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Take(0, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.Take(-1, Scheduler.Immediate));
        }

        [TestMethod]
        public void Take_Complete_After()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(690)
            );

            var res = scheduler.Start(() =>
                xs.Take(20)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(690)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 690)
            );
        }

        [TestMethod]
        public void Take_Complete_Same()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(690)
            );

            var res = scheduler.Start(() =>
                xs.Take(17)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(630)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 630)
            );
        }

        [TestMethod]
        public void Take_Complete_Before()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(690)
            );

            var res = scheduler.Start(() =>
                xs.Take(10)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnCompleted<int>(415)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 415)
            );
        }

        [TestMethod]
        public void Take_Error_After()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnError<int>(690, ex)
            );

            var res = scheduler.Start(() =>
                xs.Take(20)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnError<int>(690, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 690)
            );
        }

        [TestMethod]
        public void Take_Error_Same()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnError<int>(690, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Take(17)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(630)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 630)
            );
        }

        [TestMethod]
        public void Take_Error_Before()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnError<int>(690, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Take(3)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnCompleted<int>(270)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [TestMethod]
        public void Take_Dispose_Before()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10)
            );

            var res = scheduler.Start(() =>
                xs.Take(3),
                250
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Take_Dispose_After()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10)
            );

            var res = scheduler.Start(() =>
                xs.Take(3),
                400
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnCompleted<int>(270)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [TestMethod]
        public void Take_0_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13)
            );

            var res = scheduler.Start(() =>
                xs.Take(0, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(200 + 1) // Extra scheduling call by Empty
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Take_0_DefaultScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13)
            );

            var res = scheduler.Start(() =>
                xs.Take(0)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(200) // Immediate
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Take_Non0_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13)
            );

            var res = scheduler.Start(() =>
                xs.Take(1, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnCompleted<int>(210)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Take_Take1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Take(3).Take(4)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnCompleted<int>(270)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [TestMethod]
        public void Take_Take2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Take(4).Take(3)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnCompleted<int>(270)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [TestMethod]
        public void Take_DecrementsCountFirst()
        {
            var k = new BehaviorSubject<bool>(true);
            k.Take(1).Subscribe(b => k.OnNext(!b));

            //
            // No assert needed; test will stack overflow for failure.
            //
        }

        #endregion

        #region + TakeWhile +

        [TestMethod]
        public void TakeWhile_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).TakeWhile(DummyFunc<int, bool>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.TakeWhile(default(Func<int, bool>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.TakeWhile(DummyFunc<int, bool>.Instance).Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).TakeWhile((x, i) => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.TakeWhile(default(Func<int, int, bool>)));
        }

        [TestMethod]
        public void TakeWhile_Complete_Before()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnCompleted<int>(330),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            var invoked = 0;

            var res = scheduler.Start(() =>
                xs.TakeWhile(x =>
                {
                    invoked++;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnCompleted<int>(330)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 330)
            );

            Assert.AreEqual(4, invoked);
        }

        [TestMethod]
        public void TakeWhile_Complete_After()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            var invoked = 0;

            var res = scheduler.Start(() =>
                xs.TakeWhile(x =>
                {
                    invoked++;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnCompleted<int>(390)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );

            Assert.AreEqual(6, invoked);
        }

        [TestMethod]
        public void TakeWhile_Error_Before()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(210, 2),
                OnNext(260, 5),
                OnError<int>(270, ex),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23)
            );

            var invoked = 0;

            var res = scheduler.Start(() =>
                xs.TakeWhile(x =>
                {
                    invoked++;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(260, 5),
                OnError<int>(270, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );

            Assert.AreEqual(2, invoked);
        }

        [TestMethod]
        public void TakeWhile_Error_After()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnError<int>(600, new Exception())
            );

            var invoked = 0;

            var res = scheduler.Start(() =>
                xs.TakeWhile(x =>
                {
                    invoked++;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnCompleted<int>(390)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );

            Assert.AreEqual(6, invoked);
        }

        [TestMethod]
        public void TakeWhile_Dispose_Before()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            var invoked = 0;

            var res = scheduler.Start(() =>
                xs.TakeWhile(x =>
                {
                    invoked++;
                    return IsPrime(x);
                }),
                300
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            Assert.AreEqual(3, invoked);
        }

        [TestMethod]
        public void TakeWhile_Dispose_After()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            var invoked = 0;

            var res = scheduler.Start(() =>
                xs.TakeWhile(x =>
                {
                    invoked++;
                    return IsPrime(x);
                }),
                400
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnCompleted<int>(390)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );

            Assert.AreEqual(6, invoked);
        }

        [TestMethod]
        public void TakeWhile_Zero()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(205, 100),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            var invoked = 0;

            var res = scheduler.Start(() =>
                xs.TakeWhile(x =>
                {
                    invoked++;
                    return IsPrime(x);
                }),
                300
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(205)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 205)
            );

            Assert.AreEqual(1, invoked);
        }

        [TestMethod]
        public void TakeWhile_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            var invoked = 0;
            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.TakeWhile(x =>
                {
                    invoked++;
                    if (invoked == 3)
                        throw ex;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(260, 5),
                OnError<int>(290, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );

            Assert.AreEqual(3, invoked);
        }

        [TestMethod]
        public void TakeWhile_Index1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(205, 100),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnNext(410, 17),
                OnNext(450, 8),
                OnNext(500, 23),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.TakeWhile((x, i) => i < 5)
            );

            res.Messages.AssertEqual(
                OnNext(205, 100),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnCompleted<int>(350)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [TestMethod]
        public void TakeWhile_Index2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(205, 100),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.TakeWhile((x, i) => i >= 0)
            );

            res.Messages.AssertEqual(
                OnNext(205, 100),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void TakeWhile_Index_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(205, 100),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnError<int>(400, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeWhile((x, i) => i >= 0)
            );

            res.Messages.AssertEqual(
                OnNext(205, 100),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnError<int>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void TakeWhile_Index_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, -1),
                OnNext(110, -1),
                OnNext(205, 100),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnNext(350, 7),
                OnNext(390, 4),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.TakeWhile((x, i) => { if (i < 5) return true; throw ex; })
            );

            res.Messages.AssertEqual(
                OnNext(205, 100),
                OnNext(210, 2),
                OnNext(260, 5),
                OnNext(290, 13),
                OnNext(320, 3),
                OnError<int>(350, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        #endregion

        #region + Where +

        [TestMethod]
        public void Where_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).Where<int>(DummyFunc<int, bool>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Where<int>((Func<int, bool>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Where<int>(DummyFunc<int, bool>.Instance).Subscribe(null));
        }

        static bool IsPrime(int i)
        {
            if (i <= 1)
                return false;

            var max = (int)Math.Sqrt(i);
            for (var j = 2; j <= max; ++j)
                if (i % j == 0)
                    return false;

            return true;
        }

        [TestMethod]
        public void Where_Complete()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.Start(() =>
                xs.Where(x =>
                {
                    invoked++;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(340, 5),
                OnNext(390, 7),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.AreEqual(9, invoked);
        }

        [TestMethod]
        public void Where_True()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where(x =>
                {
                    invoked++;
                    return true;
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.AreEqual(9, invoked);
        }

        [TestMethod]
        public void Where_False()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where(x =>
                {
                    invoked++;
                    return false;
                })
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.AreEqual(9, invoked);
        }

        [TestMethod]
        public void Where_Dispose()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where(x =>
                {
                    invoked++;
                    return IsPrime(x);
                }),
                400
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(340, 5),
                OnNext(390, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            Assert.AreEqual(5, invoked);
        }

        [TestMethod]
        public void Where_Error()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnError<int>(600, ex),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.Start(() =>
                xs.Where(x =>
                {
                    invoked++;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(340, 5),
                OnNext(390, 7),
                OnNext(580, 11),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.AreEqual(9, invoked);
        }

        [TestMethod]
        public void Where_Throw()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.Start(() =>
                xs.Where(x =>
                {
                    invoked++;
                    if (x > 5)
                        throw ex;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(340, 5),
                OnError<int>(380, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 380)
            );

            Assert.AreEqual(4, invoked);
        }

        [TestMethod]
        public void Where_DisposeInPredicate()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.CreateObserver<int>();

            var d = new SerialDisposable();
            var ys = default(IObservable<int>);

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Where(x =>
            {
                invoked++;
                if (x == 8)
                    d.Dispose();
                return IsPrime(x);
            }));

            scheduler.ScheduleAbsolute(Subscribed, () => d.Disposable = ys.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () => d.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(340, 5),
                OnNext(390, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );

            Assert.AreEqual(6, invoked);
        }

        [TestMethod]
        public void WhereWhereOptimization_Regular()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where(x => x > 3).Where(x => x % 2 == 0)
            );

            res.Messages.AssertEqual(
                OnNext(270, 4),
                OnNext(380, 6),
                OnNext(450, 8),
                OnNext(560, 10),
                OnCompleted<int>(600)
            );
        }

        [TestMethod]
        public void WhereWhereOptimization_SecondPredicateThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where(x => x > 3).Where(x =>
                {
                    if (x <= 3)
                        throw new Exception();

                    return x % 2 == 0;
                })
            );

            res.Messages.AssertEqual(
                OnNext(270, 4),
                OnNext(380, 6),
                OnNext(450, 8),
                OnNext(560, 10),
                OnCompleted<int>(600)
            );
        }

        [TestMethod]
        public void WhereIndex_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).Where<int>(DummyFunc<int, int, bool>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Where<int>((Func<int, int, bool>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Where<int>(DummyFunc<int, int, bool>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void WhereIndex_Complete()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) =>
                {
                    invoked++;
                    return IsPrime(x + i * 10);
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(390, 7),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.AreEqual(9, invoked);
        }

        [TestMethod]
        public void WhereIndex_True()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) =>
                {
                    invoked++;
                    return true;
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.AreEqual(9, invoked);
        }

        [TestMethod]
        public void WhereIndex_False()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) =>
                {
                    invoked++;
                    return false;
                })
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.AreEqual(9, invoked);
        }

        [TestMethod]
        public void WhereIndex_Dispose()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) =>
                {
                    invoked++;
                    return IsPrime(x + i * 10);
                }),
                400
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(390, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            Assert.AreEqual(5, invoked);
        }

        [TestMethod]
        public void WhereIndex_Error()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnError<int>(600, ex),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) =>
                {
                    invoked++;
                    return IsPrime(x + i * 10);
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(390, 7),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.AreEqual(9, invoked);
        }

        [TestMethod]
        public void WhereIndex_Throw()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) =>
                {
                    invoked++;
                    if (x > 5)
                        throw ex;
                    return IsPrime(x + i * 10);
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnError<int>(380, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 380)
            );

            Assert.AreEqual(4, invoked);
        }

        [TestMethod]
        public void WhereIndex_DisposeInPredicate()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.CreateObserver<int>();

            var d = new SerialDisposable();
            var ys = default(IObservable<int>);


            scheduler.ScheduleAbsolute(Created, () => ys = xs.Where((x, i) =>
            {
                invoked++;
                if (x == 8)
                    d.Dispose();
                return IsPrime(x + i * 10);
            }));

            scheduler.ScheduleAbsolute(Subscribed, () => d.Disposable = ys.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () => d.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(390, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );

            Assert.AreEqual(6, invoked);
        }

        [TestMethod]
        public void Where_Where1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Where(x => x > 3).Where(x => x < 6)
            );

            res.Messages.AssertEqual(
                OnNext(270, 4),
                OnNext(340, 5),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Where_Where2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) => i >= 1).Where(x => x < 6)
            );

            res.Messages.AssertEqual(
                OnNext(270, 4),
                OnNext(340, 5),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Where_Where3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Where(x => x > 3).Where((x, i) => i < 2)
            );

            res.Messages.AssertEqual(
                OnNext(270, 4),
                OnNext(340, 5),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Where_Where4()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) => i >= 1).Where((x, i) => i < 2)
            );

            res.Messages.AssertEqual(
                OnNext(270, 4),
                OnNext(340, 5),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        #endregion
    }
}
