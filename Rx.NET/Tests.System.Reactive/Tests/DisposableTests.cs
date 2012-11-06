// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class DisposableTests
    {
        [TestMethod]
        public void AnonymousDisposable_Create()
        {
            var d = Disposable.Create(() => { });
            Assert.IsNotNull(d);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void AnonymousDisposable_CreateNull()
        {
            Disposable.Create(null);
        }

        [TestMethod]
        public void AnonymousDisposable_Dispose()
        {
            var disposed = false;
            var d = Disposable.Create(() => { disposed = true; });
            Assert.IsFalse(disposed);
            d.Dispose();
            Assert.IsTrue(disposed);

            var c = d as ICancelable;
            Assert.IsNotNull(c);
            Assert.IsTrue(c.IsDisposed);
        }

        [TestMethod]
        public void EmptyDisposable()
        {
            var d = Disposable.Empty;
            Assert.IsNotNull(d);
            d.Dispose();
        }

        [TestMethod]
        public void BooleanDisposable()
        {
            var d = new BooleanDisposable();
            Assert.IsFalse(d.IsDisposed);
            d.Dispose();
            Assert.IsTrue(d.IsDisposed);
            d.Dispose();
            Assert.IsTrue(d.IsDisposed);
        }

        [TestMethod]
        public void SingleAssignmentDisposable_SetNull()
        {
            var d = new SingleAssignmentDisposable();
            d.Disposable = null;
        }

        [TestMethod]
        public void SingleAssignmentDisposable_DisposeAfterSet()
        {
            var disposed = false;

            var d = new SingleAssignmentDisposable();
            var dd = Disposable.Create(() => { disposed = true; });
            d.Disposable = dd;

            Assert.AreSame(dd, d.Disposable);

            Assert.IsFalse(disposed);
            d.Dispose();
            Assert.IsTrue(disposed);
            d.Dispose();
            Assert.IsTrue(disposed);

            Assert.IsTrue(d.IsDisposed);
        }

        [TestMethod]
        public void SingleAssignmentDisposable_DisposeBeforeSet()
        {
            var disposed = false;

            var d = new SingleAssignmentDisposable();
            var dd = Disposable.Create(() => { disposed = true; });

            Assert.IsFalse(disposed);
            d.Dispose();
            Assert.IsFalse(disposed);
            Assert.IsTrue(d.IsDisposed);

            d.Disposable = dd;
            Assert.IsTrue(disposed);
            //Assert.IsNull(d.Disposable); // BREAKING CHANGE v2 > v1.x - Undefined behavior after disposal.
            d.Disposable.Dispose();        // This should be a nop.

            d.Dispose();
            Assert.IsTrue(disposed);
        }

        [TestMethod]
        public void SingleAssignmentDisposable_SetMultipleTimes()
        {
            var d = new SingleAssignmentDisposable();
            d.Disposable = Disposable.Empty;

            ReactiveAssert.Throws<InvalidOperationException>(() => { d.Disposable = Disposable.Empty; });
        }

        [TestMethod]
        public void CompositeDisposable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new CompositeDisposable(default(IDisposable[])));
            ReactiveAssert.Throws<ArgumentNullException>(() => new CompositeDisposable(default(IEnumerable<IDisposable>)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => new CompositeDisposable(-1));
        }

        [TestMethod]
        public void CompositeDisposable_Contains()
        {
            var d1 = Disposable.Create(() => {} );
            var d2 = Disposable.Create(() => { });

            var g = new CompositeDisposable(d1, d2);
            Assert.AreEqual(2, g.Count);
            Assert.IsTrue(g.Contains(d1));
            Assert.IsTrue(g.Contains(d2));

            ReactiveAssert.Throws<ArgumentNullException>(() => g.Contains(null));
        }

        [TestMethod]
        public void CompositeDisposable_IsReadOnly()
        {
            Assert.IsFalse(new CompositeDisposable().IsReadOnly);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void CompositeDisposable_CopyTo_Null()
        {
            new CompositeDisposable().CopyTo(null, 0);
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CompositeDisposable_CopyTo_Negative()
        {
            new CompositeDisposable().CopyTo(new IDisposable[2], -1);
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CompositeDisposable_CopyTo_BeyondEnd()
        {
            new CompositeDisposable().CopyTo(new IDisposable[2], 2);
        }

        [TestMethod]
        public void CompositeDisposable_CopyTo()
        {
            var d1 = Disposable.Create(() => { });
            var d2 = Disposable.Create(() => { });
            var g = new CompositeDisposable(new List<IDisposable> { d1, d2 });

            var d = new IDisposable[3];
            g.CopyTo(d, 1);
            Assert.AreSame(d1, d[1]);
            Assert.AreSame(d2, d[2]);
        }

        [TestMethod]
        public void CompositeDisposable_ToArray()
        {
            var d1 = Disposable.Create(() => { });
            var d2 = Disposable.Create(() => { });
            var g = new CompositeDisposable(d1, d2);
            Assert.AreEqual(2, g.Count);
            var x = Enumerable.ToArray(g);
            Assert.IsTrue(g.ToArray().SequenceEqual(new[] { d1, d2 }));
        }

        [TestMethod]
        public void CompositeDisposable_GetEnumerator()
        {
            var d1 = Disposable.Create(() => { });
            var d2 = Disposable.Create(() => { });
            var g = new CompositeDisposable(d1, d2);
            var lst = new List<IDisposable>();
            foreach (var x in g)
                lst.Add(x);
            Assert.IsTrue(lst.SequenceEqual(new[] { d1, d2 }));
        }

        [TestMethod]
        public void CompositeDisposable_GetEnumeratorNonGeneric()
        {
            var d1 = Disposable.Create(() => { });
            var d2 = Disposable.Create(() => { });
            var g = new CompositeDisposable(d1, d2);
            var lst = new List<IDisposable>();
            foreach (IDisposable x in (IEnumerable)g)
                lst.Add(x);
            Assert.IsTrue(lst.SequenceEqual(new[] { d1, d2 }));
        }

        [TestMethod]
        public void CompositeDisposable_CollectionInitializer()
        {
            var d1 = Disposable.Create(() => { });
            var d2 = Disposable.Create(() => { });
            var g = new CompositeDisposable { d1, d2 };
            Assert.AreEqual(2, g.Count);
            Assert.IsTrue(g.Contains(d1));
            Assert.IsTrue(g.Contains(d2));
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void CompositeDisposable_AddNull()
        {
            new CompositeDisposable().Add(null);
        }

        [TestMethod]
        public void CompositeDisposable_Add()
        {
            var d1 = Disposable.Create(() => { });
            var d2 = Disposable.Create(() => { });
            var g = new CompositeDisposable(d1);
            Assert.AreEqual(1, g.Count);
            Assert.IsTrue(g.Contains(d1));
            g.Add(d2);
            Assert.AreEqual(2, g.Count);
            Assert.IsTrue(g.Contains(d2));
        }

        [TestMethod]
        public void CompositeDisposable_AddAfterDispose()
        {
            var disp1 = false;
            var disp2 = false;

            var d1 = Disposable.Create(() => { disp1 = true; });
            var d2 = Disposable.Create(() => { disp2 = true; });
            var g = new CompositeDisposable(d1);
            Assert.AreEqual(1, g.Count);

            g.Dispose();
            Assert.IsTrue(disp1);
            Assert.AreEqual(0, g.Count); // CHECK

            g.Add(d2);
            Assert.IsTrue(disp2);
            Assert.AreEqual(0, g.Count); // CHECK

            Assert.IsTrue(g.IsDisposed);
        }

        [TestMethod]
        public void CompositeDisposable_Remove()
        {
            var disp1 = false;
            var disp2 = false;

            var d1 = Disposable.Create(() => { disp1 = true; });
            var d2 = Disposable.Create(() => { disp2 = true; });
            var g = new CompositeDisposable(d1, d2);

            Assert.AreEqual(2, g.Count);
            Assert.IsTrue(g.Contains(d1));
            Assert.IsTrue(g.Contains(d2));

            Assert.IsTrue(g.Remove(d1));
            Assert.AreEqual(1, g.Count);
            Assert.IsFalse(g.Contains(d1));
            Assert.IsTrue(g.Contains(d2));
            Assert.IsTrue(disp1);

            Assert.IsTrue(g.Remove(d2));
            Assert.IsFalse(g.Contains(d1));
            Assert.IsFalse(g.Contains(d2));
            Assert.IsTrue(disp2);

            var disp3 = false;
            var d3 = Disposable.Create(() => { disp3 = true; });
            Assert.IsFalse(g.Remove(d3));
            Assert.IsFalse(disp3);
        }

        [TestMethod]
        public void CompositeDisposable_Clear()
        {
            var disp1 = false;
            var disp2 = false;

            var d1 = Disposable.Create(() => { disp1 = true; });
            var d2 = Disposable.Create(() => { disp2 = true; });
            var g = new CompositeDisposable(d1, d2);
            Assert.AreEqual(2, g.Count);

            g.Clear();
            Assert.IsTrue(disp1);
            Assert.IsTrue(disp2);
            Assert.AreEqual(0, g.Count);

            var disp3 = false;
            var d3 = Disposable.Create(() => { disp3 = true; });
            g.Add(d3);
            Assert.IsFalse(disp3);
            Assert.AreEqual(1, g.Count);
        }

        [TestMethod]
        public void CompositeDisposable_RemoveOptimizationBehavior()
        {
            var g = new CompositeDisposable();
            var m = new Dictionary<int, IDisposable>();
            var r = new List<int>();

            var N = 100;

            for (int i = 0; i < N; i++)
            {
                var j = i;

                var d = Disposable.Create(() => r.Add(j));
                m[j] = d;
                g.Add(d);
            }

            var d1 = Enumerable.Range(0, N).Where(i => i % 2 == 0).ToArray();
            foreach (var i in d1)
                g.Remove(m[i]);
            Assert.IsTrue(r.SequenceEqual(d1));

            var d2 = Enumerable.Range(0, N).Where(i => i % 3 == 0).ToArray();
            foreach (var i in d2)
                g.Remove(m[i]);
            Assert.IsTrue(r.SequenceEqual(d1.Concat(d2.Where(x => !d1.Any(y => x == y)))));

            var d3 = Enumerable.Range(0, N).Where(i => i % 5 == 0).ToArray();
            foreach (var i in d3)
                g.Remove(m[i]);
            Assert.IsTrue(r.SequenceEqual(d1.Concat(d2.Where(x => !d1.Any(y => x == y))).Concat(d3.Where(x => !d1.Any(y => x == y) && !d2.Any(y => x == y)))));

            g.Dispose();

            var z = r.Except(d1.Union(d2).Union(d3)).ToArray();
            Assert.IsTrue(z.SequenceEqual(Enumerable.Range(0, N).Where(i => !(i % 2 == 0 || i % 3 == 0 || i % 5 == 0))));
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void CompositeDisposable_RemoveNull()
        {
            new CompositeDisposable().Remove(null);
        }

#if DESKTOPCLR40 || DESKTOPCLR45
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void CancellationDisposable_Ctor_Null()
        {
            new CancellationDisposable(null);
        }

        [TestMethod]
        public void CancellationDisposable_DefaultCtor()
        {
            var c = new CancellationDisposable();
            Assert.IsNotNull(c.Token);
            Assert.IsFalse(c.Token.IsCancellationRequested);
            Assert.IsTrue(c.Token.CanBeCanceled);
            c.Dispose();
            Assert.IsTrue(c.IsDisposed);
            Assert.IsTrue(c.Token.IsCancellationRequested);
        }

        [TestMethod]
        public void CancellationDisposable_TokenCtor()
        {
            var t = new CancellationTokenSource();
            var c = new CancellationDisposable(t);
            Assert.IsTrue(t.Token == c.Token);
            Assert.IsFalse(c.Token.IsCancellationRequested);
            Assert.IsTrue(c.Token.CanBeCanceled);
            c.Dispose();
            Assert.IsTrue(c.IsDisposed);
            Assert.IsTrue(c.Token.IsCancellationRequested);
        }
#endif
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void ContextDisposable_CreateNullContext()
        {
            new ContextDisposable(null, Disposable.Empty);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void ContextDisposable_CreateNullDisposable()
        {
            new ContextDisposable(new SynchronizationContext(), null);
        }

        [TestMethod]
        public void ContextDisposable()
        {
            var disp = false;
            var m = new MySync();
            var c = new ContextDisposable(m, Disposable.Create(() => { disp = true; }));
            Assert.AreSame(m, c.Context);
            Assert.IsFalse(m._disposed);
            Assert.IsFalse(disp);
            c.Dispose();
            Assert.IsTrue(c.IsDisposed);
            Assert.IsTrue(m._disposed);
            Assert.IsTrue(disp);
        }

        class MySync : SynchronizationContext
        {
            internal bool _disposed = false;

            public override void Post(SendOrPostCallback d, object state)
            {
                d(state);
                _disposed = true;
            }
        }

        [TestMethod]
        public void SerialDisposable_Ctor_Prop()
        {
            var m = new SerialDisposable();
            Assert.IsNull(m.Disposable);
        }

        [TestMethod]
        public void SerialDisposable_ReplaceBeforeDispose()
        {
            var disp1 = false;
            var disp2 = false;

            var m = new SerialDisposable();
            var d1 = Disposable.Create(() => { disp1 = true; });
            m.Disposable = d1;
            Assert.AreSame(d1, m.Disposable);
            Assert.IsFalse(disp1);

            var d2 = Disposable.Create(() => { disp2 = true; });
            m.Disposable = d2;
            Assert.AreSame(d2, m.Disposable);
            Assert.IsTrue(disp1);
            Assert.IsFalse(disp2);
        }

        [TestMethod]
        public void SerialDisposable_ReplaceAfterDispose()
        {
            var disp1 = false;
            var disp2 = false;

            var m = new SerialDisposable();
            m.Dispose();
            Assert.IsTrue(m.IsDisposed);

            var d1 = Disposable.Create(() => { disp1 = true; });
            m.Disposable = d1;
            Assert.IsNull(m.Disposable); // CHECK
            Assert.IsTrue(disp1);

            var d2 = Disposable.Create(() => { disp2 = true; });
            m.Disposable = d2;
            Assert.IsNull(m.Disposable); // CHECK
            Assert.IsTrue(disp2);
        }

        [TestMethod]
        public void SerialDisposable_Dispose()
        {
            var disp = false;

            var m = new SerialDisposable();
            var d = Disposable.Create(() => { disp = true; });
            m.Disposable = d;
            Assert.AreSame(d, m.Disposable);
            Assert.IsFalse(disp);

            m.Dispose();
            Assert.IsTrue(m.IsDisposed);
            Assert.IsTrue(disp);
            //Assert.IsNull(m.Disposable); // BREAKING CHANGE v2 > v1.x - Undefined behavior after disposal.
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void RefCountDisposable_Ctor_Null()
        {
            new RefCountDisposable(null);
        }

        [TestMethod]
        public void RefCountDisposable_SingleReference()
        {
            var d = new BooleanDisposable();
            var r = new RefCountDisposable(d);
            Assert.IsFalse(d.IsDisposed);
            r.Dispose();
            Assert.IsTrue(d.IsDisposed);
            r.Dispose();
            Assert.IsTrue(d.IsDisposed);
        }

        [TestMethod]
        public void RefCountDisposable_RefCounting()
        {
            var d = new BooleanDisposable();
            var r = new RefCountDisposable(d);
            Assert.IsFalse(d.IsDisposed);

            var d1 = r.GetDisposable();
            var d2 = r.GetDisposable();
            Assert.IsFalse(d.IsDisposed);

            d1.Dispose();
            Assert.IsFalse(d.IsDisposed);

            d2.Dispose();
            Assert.IsFalse(d.IsDisposed); // CHECK

            r.Dispose();
            Assert.IsTrue(d.IsDisposed);
            Assert.IsTrue(r.IsDisposed);

            var d3 = r.GetDisposable(); // CHECK
            d3.Dispose();
        }

        [TestMethod]
        public void RefCountDisposable_PrimaryDisposesFirst()
        {
            var d = new BooleanDisposable();
            var r = new RefCountDisposable(d);
            Assert.IsFalse(d.IsDisposed);

            var d1 = r.GetDisposable();
            var d2 = r.GetDisposable();
            Assert.IsFalse(d.IsDisposed);

            d1.Dispose();
            Assert.IsFalse(d.IsDisposed);

            r.Dispose();
            Assert.IsFalse(d.IsDisposed);

            d2.Dispose();
            Assert.IsTrue(d.IsDisposed);
        }

        [TestMethod]
        public void ScheduledDisposable_Null()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new ScheduledDisposable(null, Disposable.Empty));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ScheduledDisposable(Scheduler.Immediate, null));
        }

        [TestMethod]
        public void ScheduledDisposable()
        {
            var d = new BooleanDisposable();
            var s = new ScheduledDisposable(Scheduler.Immediate, d);

            Assert.IsFalse(d.IsDisposed);

            Assert.AreSame(Scheduler.Immediate, s.Scheduler);
            Assert.AreSame(d, s.Disposable);

            s.Dispose();

            Assert.IsTrue(d.IsDisposed);
            Assert.IsTrue(s.IsDisposed);

            Assert.AreSame(Scheduler.Immediate, s.Scheduler);
            //Assert.AreSame(d, s.Disposable); // BREAKING CHANGE v2 > v1.x - Undefined behavior after disposal.
            s.Disposable.Dispose();            // This should be a nop.
        }

        [TestMethod]
        public void MultipleAssignmentDisposable()
        {
            var m = new MultipleAssignmentDisposable();

            var disp1 = false;
            var d1 = Disposable.Create(() => { disp1 = true; });
            m.Disposable = d1;
            Assert.AreSame(d1, m.Disposable);
            Assert.IsFalse(m.IsDisposed);

            var disp2 = false;
            var d2 = Disposable.Create(() => { disp2 = true; });
            m.Disposable = d2;
            Assert.AreSame(d2, m.Disposable);
            Assert.IsFalse(m.IsDisposed);
            Assert.IsFalse(disp1);

            m.Dispose();
            Assert.IsTrue(disp2);
            Assert.IsTrue(m.IsDisposed);
            //Assert.IsNull(m.Disposable); // BREAKING CHANGE v2 > v1.x - Undefined behavior after disposal.
            m.Disposable.Dispose();        // This should be a nop.

            var disp3 = false;
            var d3 = Disposable.Create(() => { disp3 = true; });
            m.Disposable = d3;
            Assert.IsTrue(disp3);
            //Assert.IsNull(m.Disposable); // BREAKING CHANGE v2 > v1.x - Undefined behavior after disposal.
            m.Disposable.Dispose();        // This should be a nop.
            Assert.IsTrue(m.IsDisposed);
        }
    }
}
