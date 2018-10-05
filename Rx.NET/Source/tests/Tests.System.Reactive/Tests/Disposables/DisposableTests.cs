// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{

    public class DisposableTests
    {
        [Fact]
        public void AnonymousDisposable_Create()
        {
            var d = Disposable.Create(() => { });
            Assert.NotNull(d);
        }

        [Fact]
        public void AnonymousDisposable_CreateNull()
        {
            Assert.Throws(typeof(ArgumentNullException), () => Disposable.Create(null));
        }

        [Fact]
        public void AnonymousDisposable_Dispose()
        {
            var disposed = false;
            var d = Disposable.Create(() => { disposed = true; });
            Assert.False(disposed);
            d.Dispose();
            Assert.True(disposed);

            var c = d as ICancelable;
            Assert.NotNull(c);
            Assert.True(c.IsDisposed);
        }

        [Fact]
        public void EmptyDisposable()
        {
            var d = Disposable.Empty;
            Assert.NotNull(d);
            d.Dispose();
        }

        [Fact]
        public void BooleanDisposable()
        {
            var d = new BooleanDisposable();
            Assert.False(d.IsDisposed);
            d.Dispose();
            Assert.True(d.IsDisposed);
            d.Dispose();
            Assert.True(d.IsDisposed);
        }

        [Fact]
        public void SingleAssignmentDisposable_SetNull()
        {
            var d = new SingleAssignmentDisposable
            {
                Disposable = null
            };
        }

        [Fact]
        public void SingleAssignmentDisposable_DisposeAfterSet()
        {
            var disposed = false;

            var d = new SingleAssignmentDisposable();
            var dd = Disposable.Create(() => { disposed = true; });
            d.Disposable = dd;

            Assert.Same(dd, d.Disposable);

            Assert.False(disposed);
            d.Dispose();
            Assert.True(disposed);
            d.Dispose();
            Assert.True(disposed);

            Assert.True(d.IsDisposed);
        }

        [Fact]
        public void SingleAssignmentDisposable_DisposeBeforeSet()
        {
            var disposed = false;

            var d = new SingleAssignmentDisposable();
            var dd = Disposable.Create(() => { disposed = true; });

            Assert.False(disposed);
            d.Dispose();
            Assert.False(disposed);
            Assert.True(d.IsDisposed);

            d.Disposable = dd;
            Assert.True(disposed);
            //Assert.Null(d.Disposable); // BREAKING CHANGE v2 > v1.x - Undefined behavior after disposal.
            d.Disposable.Dispose();        // This should be a nop.

            d.Dispose();
            Assert.True(disposed);
        }

        [Fact]
        public void SingleAssignmentDisposable_SetMultipleTimes()
        {
            var d = new SingleAssignmentDisposable
            {
                Disposable = Disposable.Empty
            };

            ReactiveAssert.Throws<InvalidOperationException>(() => { d.Disposable = Disposable.Empty; });
        }

        [Fact]
        public void CompositeDisposable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new CompositeDisposable(default(IDisposable[])));
            ReactiveAssert.Throws<ArgumentNullException>(() => new CompositeDisposable(default(IEnumerable<IDisposable>)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => new CompositeDisposable(-1));
        }

        [Fact]
        public void CompositeDisposable_Contains()
        {
            var d1 = Disposable.Create(() => { });
            var d2 = Disposable.Create(() => { });

            var g = new CompositeDisposable(d1, d2);
            Assert.Equal(2, g.Count);
            Assert.True(g.Contains(d1));
            Assert.True(g.Contains(d2));

            ReactiveAssert.Throws<ArgumentNullException>(() => g.Contains(null));
        }

        [Fact]
        public void CompositeDisposable_IsReadOnly()
        {
            Assert.False(new CompositeDisposable().IsReadOnly);
        }

        [Fact]
        public void CompositeDisposable_CopyTo_Null()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new CompositeDisposable().CopyTo(null, 0));
        }

        [Fact]
        public void CompositeDisposable_CopyTo_Negative()
        {
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => new CompositeDisposable().CopyTo(new IDisposable[2], -1));
        }

        [Fact]
        public void CompositeDisposable_CopyTo_BeyondEnd()
        {
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => new CompositeDisposable().CopyTo(new IDisposable[2], 2));
        }

        [Fact]
        public void CompositeDisposable_CopyTo()
        {
            var d1 = Disposable.Create(() => { });
            var d2 = Disposable.Create(() => { });
            var g = new CompositeDisposable(new List<IDisposable> { d1, d2 });

            var d = new IDisposable[3];
            g.CopyTo(d, 1);
            Assert.Same(d1, d[1]);
            Assert.Same(d2, d[2]);
        }

        [Fact]
        public void CompositeDisposable_ToArray()
        {
            var d1 = Disposable.Create(() => { });
            var d2 = Disposable.Create(() => { });
            var g = new CompositeDisposable(d1, d2);
            Assert.Equal(2, g.Count);
            var x = Enumerable.ToArray(g);
            Assert.True(g.ToArray().SequenceEqual(new[] { d1, d2 }));
        }

        [Fact]
        public void CompositeDisposable_GetEnumerator()
        {
            var d1 = Disposable.Create(() => { });
            var d2 = Disposable.Create(() => { });
            var g = new CompositeDisposable(d1, d2);
            var lst = new List<IDisposable>();
            foreach (var x in g)
            {
                lst.Add(x);
            }

            Assert.True(lst.SequenceEqual(new[] { d1, d2 }));
        }

        [Fact]
        public void CompositeDisposable_GetEnumeratorNonGeneric()
        {
            var d1 = Disposable.Create(() => { });
            var d2 = Disposable.Create(() => { });
            var g = new CompositeDisposable(d1, d2);
            var lst = new List<IDisposable>();
            foreach (IDisposable x in (IEnumerable)g)
            {
                lst.Add(x);
            }

            Assert.True(lst.SequenceEqual(new[] { d1, d2 }));
        }

        [Fact]
        public void CompositeDisposable_CollectionInitializer()
        {
            var d1 = Disposable.Create(() => { });
            var d2 = Disposable.Create(() => { });
            var g = new CompositeDisposable { d1, d2 };
            Assert.Equal(2, g.Count);
            Assert.True(g.Contains(d1));
            Assert.True(g.Contains(d2));
        }

        [Fact]
        public void CompositeDisposable_AddNull_via_params_ctor()
        {
            IDisposable d1 = null;
            ReactiveAssert.Throws<ArgumentException>(() => new CompositeDisposable(d1));
        }

        [Fact]
        public void CompositeDisposable_AddNull_via_IEnum_ctor()
        {
            IEnumerable<IDisposable> values = new IDisposable[] { null };
            ReactiveAssert.Throws<ArgumentException>(() => new CompositeDisposable(values));
        }

        [Fact]
        public void CompositeDisposable_AddNull()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new CompositeDisposable().Add(null));
        }

        [Fact]
        public void CompositeDisposable_Add()
        {
            var d1 = Disposable.Create(() => { });
            var d2 = Disposable.Create(() => { });
            var g = new CompositeDisposable(d1);
            Assert.Equal(1, g.Count);
            Assert.True(g.Contains(d1));
            g.Add(d2);
            Assert.Equal(2, g.Count);
            Assert.True(g.Contains(d2));
        }

        [Fact]
        public void CompositeDisposable_AddAfterDispose()
        {
            var disp1 = false;
            var disp2 = false;

            var d1 = Disposable.Create(() => { disp1 = true; });
            var d2 = Disposable.Create(() => { disp2 = true; });
            var g = new CompositeDisposable(d1);
            Assert.Equal(1, g.Count);

            g.Dispose();
            Assert.True(disp1);
            Assert.Equal(0, g.Count); // CHECK

            g.Add(d2);
            Assert.True(disp2);
            Assert.Equal(0, g.Count); // CHECK

            Assert.True(g.IsDisposed);
        }

        [Fact]
        public void CompositeDisposable_Remove()
        {
            var disp1 = false;
            var disp2 = false;

            var d1 = Disposable.Create(() => { disp1 = true; });
            var d2 = Disposable.Create(() => { disp2 = true; });
            var g = new CompositeDisposable(d1, d2);

            Assert.Equal(2, g.Count);
            Assert.True(g.Contains(d1));
            Assert.True(g.Contains(d2));

            Assert.True(g.Remove(d1));
            Assert.Equal(1, g.Count);
            Assert.False(g.Contains(d1));
            Assert.True(g.Contains(d2));
            Assert.True(disp1);

            Assert.True(g.Remove(d2));
            Assert.False(g.Contains(d1));
            Assert.False(g.Contains(d2));
            Assert.True(disp2);

            var disp3 = false;
            var d3 = Disposable.Create(() => { disp3 = true; });
            Assert.False(g.Remove(d3));
            Assert.False(disp3);
        }

        [Fact]
        public void CompositeDisposable_Clear()
        {
            var disp1 = false;
            var disp2 = false;

            var d1 = Disposable.Create(() => { disp1 = true; });
            var d2 = Disposable.Create(() => { disp2 = true; });
            var g = new CompositeDisposable(d1, d2);
            Assert.Equal(2, g.Count);

            g.Clear();
            Assert.True(disp1);
            Assert.True(disp2);
            Assert.Equal(0, g.Count);

            var disp3 = false;
            var d3 = Disposable.Create(() => { disp3 = true; });
            g.Add(d3);
            Assert.False(disp3);
            Assert.Equal(1, g.Count);
        }

        [Fact]
        public void CompositeDisposable_RemoveOptimizationBehavior()
        {
            var g = new CompositeDisposable();
            var m = new Dictionary<int, IDisposable>();
            var r = new List<int>();

            var N = 100;

            for (var i = 0; i < N; i++)
            {
                var j = i;

                var d = Disposable.Create(() => r.Add(j));
                m[j] = d;
                g.Add(d);
            }

            var d1 = Enumerable.Range(0, N).Where(i => i % 2 == 0).ToArray();
            foreach (var i in d1)
            {
                g.Remove(m[i]);
            }

            Assert.True(r.SequenceEqual(d1));

            var d2 = Enumerable.Range(0, N).Where(i => i % 3 == 0).ToArray();
            foreach (var i in d2)
            {
                g.Remove(m[i]);
            }

            Assert.True(r.SequenceEqual(d1.Concat(d2.Where(x => !d1.Any(y => x == y)))));

            var d3 = Enumerable.Range(0, N).Where(i => i % 5 == 0).ToArray();
            foreach (var i in d3)
            {
                g.Remove(m[i]);
            }

            Assert.True(r.SequenceEqual(d1.Concat(d2.Where(x => !d1.Any(y => x == y))).Concat(d3.Where(x => !d1.Any(y => x == y) && !d2.Any(y => x == y)))));

            g.Dispose();

            var z = r.Except(d1.Union(d2).Union(d3)).ToArray();
            Assert.True(z.SequenceEqual(Enumerable.Range(0, N).Where(i => !(i % 2 == 0 || i % 3 == 0 || i % 5 == 0))));
        }

        [Fact]
        public void CompositeDisposable_RemoveNull()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new CompositeDisposable().Remove(null));
        }

        [Fact]
        public void CompositeDisposable_Empty_GetEnumerator()
        {
            var composite = new CompositeDisposable();

            Assert.False(composite.GetEnumerator().MoveNext());
        }

        [Fact]
        public void CompositeDisposable_NonCollection_Enumerable_Init()
        {
            var d = new BooleanDisposable();

            var composite = new CompositeDisposable(Just(d));

            composite.Dispose();

            Assert.True(d.IsDisposed);
        }

        private static IEnumerable<IDisposable> Just(IDisposable d)
        {
            yield return d;
        }

        [Fact]
        public void CompositeDisposable_Disposed_Is_NoOp()
        {
            var d = new BooleanDisposable();

            var composite = new CompositeDisposable(d);

            composite.Dispose();

            composite.Clear();

            Assert.False(composite.Contains(d));

            var array = new IDisposable[1];

            composite.CopyTo(array, 0);

            Assert.Null(array[0]);
        }

        [Fact]
        public void CompositeDisposable_CopyTo_Index_Out_Of_Range()
        {
            var d1 = new BooleanDisposable();
            var d2 = new BooleanDisposable();

            var composite = new CompositeDisposable(d1, d2);

            var array = new IDisposable[2];

            try
            {
                composite.CopyTo(array, 1);
                Assert.False(true, "Should have thrown!");
            }
            catch (ArgumentOutOfRangeException)
            {
                // expected
            }
        }

        [Fact]
        public void CompositeDisposable_GetEnumerator_Reset()
        {
            var d = new BooleanDisposable();

            var composite = new CompositeDisposable(d);

            var enumerator = composite.GetEnumerator();

            Assert.True(enumerator.MoveNext());
            Assert.Equal(d, enumerator.Current);
            Assert.False(enumerator.MoveNext());

            enumerator.Reset();

            Assert.True(enumerator.MoveNext());
            Assert.Equal(d, enumerator.Current);
        }

        [Fact]
        public void CompositeDisposable_GetEnumerator_Disposed_Entries()
        {
            var d1 = new BooleanDisposable();
            var d2 = new BooleanDisposable();
            var d3 = new BooleanDisposable();

            var composite = new CompositeDisposable(d1, d2, d3);

            composite.Remove(d2);

            var enumerator = composite.GetEnumerator();

            Assert.True(enumerator.MoveNext());
            Assert.Equal(d1, enumerator.Current);

            Assert.True(enumerator.MoveNext());
            Assert.Equal(d3, enumerator.Current);

            Assert.False(enumerator.MoveNext());
        }

#if NET45 || NET46 
        [Fact]
        public void CancellationDisposable_Ctor_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new CancellationDisposable(null));
        }

        [Fact]
        public void CancellationDisposable_DefaultCtor()
        {
            var c = new CancellationDisposable();
            Assert.NotNull(c.Token);
            Assert.False(c.Token.IsCancellationRequested);
            Assert.True(c.Token.CanBeCanceled);
            c.Dispose();
            Assert.True(c.IsDisposed);
            Assert.True(c.Token.IsCancellationRequested);
        }

        [Fact]
        public void CancellationDisposable_TokenCtor()
        {
            var t = new CancellationTokenSource();
            var c = new CancellationDisposable(t);
            Assert.True(t.Token == c.Token);
            Assert.False(c.Token.IsCancellationRequested);
            Assert.True(c.Token.CanBeCanceled);
            c.Dispose();
            Assert.True(c.IsDisposed);
            Assert.True(c.Token.IsCancellationRequested);
        }
#endif
        [Fact]
        public void ContextDisposable_CreateNullContext()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new ContextDisposable(null, Disposable.Empty));
        }

        [Fact]
        public void ContextDisposable_CreateNullDisposable()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new ContextDisposable(new SynchronizationContext(), null));
        }

        [Fact]
        public void ContextDisposable()
        {
            var disp = false;
            var m = new MySync();
            var c = new ContextDisposable(m, Disposable.Create(() => { disp = true; }));
            Assert.Same(m, c.Context);
            Assert.False(m._disposed);
            Assert.False(disp);
            c.Dispose();
            Assert.True(c.IsDisposed);
            Assert.True(m._disposed);
            Assert.True(disp);
        }

        private class MySync : SynchronizationContext
        {
            internal bool _disposed = false;

            public override void Post(SendOrPostCallback d, object state)
            {
                d(state);
                _disposed = true;
            }
        }

        [Fact]
        public void SerialDisposable_Ctor_Prop()
        {
            var m = new SerialDisposable();
            Assert.Null(m.Disposable);
        }

        [Fact]
        public void SerialDisposable_ReplaceBeforeDispose()
        {
            var disp1 = false;
            var disp2 = false;

            var m = new SerialDisposable();
            var d1 = Disposable.Create(() => { disp1 = true; });
            m.Disposable = d1;
            Assert.Same(d1, m.Disposable);
            Assert.False(disp1);

            var d2 = Disposable.Create(() => { disp2 = true; });
            m.Disposable = d2;
            Assert.Same(d2, m.Disposable);
            Assert.True(disp1);
            Assert.False(disp2);
        }

        [Fact]
        public void SerialDisposable_ReplaceAfterDispose()
        {
            var disp1 = false;
            var disp2 = false;

            var m = new SerialDisposable();
            m.Dispose();
            Assert.True(m.IsDisposed);

            var d1 = Disposable.Create(() => { disp1 = true; });
            m.Disposable = d1;
            Assert.Null(m.Disposable); // CHECK
            Assert.True(disp1);

            var d2 = Disposable.Create(() => { disp2 = true; });
            m.Disposable = d2;
            Assert.Null(m.Disposable); // CHECK
            Assert.True(disp2);
        }

        [Fact]
        public void SerialDisposable_Dispose()
        {
            var disp = false;

            var m = new SerialDisposable();
            var d = Disposable.Create(() => { disp = true; });
            m.Disposable = d;
            Assert.Same(d, m.Disposable);
            Assert.False(disp);

            m.Dispose();
            Assert.True(m.IsDisposed);
            Assert.True(disp);
            //Assert.Null(m.Disposable); // BREAKING CHANGE v2 > v1.x - Undefined behavior after disposal.
        }

        [Fact]
        public void RefCountDisposable_Ctor_Null()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new RefCountDisposable(null));
        }

        [Fact]
        public void RefCountDisposable_SingleReference()
        {
            var d = new BooleanDisposable();
            var r = new RefCountDisposable(d);
            Assert.False(d.IsDisposed);
            r.Dispose();
            Assert.True(d.IsDisposed);
            r.Dispose();
            Assert.True(d.IsDisposed);
        }

        [Fact]
        public void RefCountDisposable_RefCounting()
        {
            var d = new BooleanDisposable();
            var r = new RefCountDisposable(d);
            Assert.False(d.IsDisposed);

            var d1 = r.GetDisposable();
            var d2 = r.GetDisposable();
            Assert.False(d.IsDisposed);

            d1.Dispose();
            Assert.False(d.IsDisposed);

            d2.Dispose();
            Assert.False(d.IsDisposed); // CHECK

            r.Dispose();
            Assert.True(d.IsDisposed);
            Assert.True(r.IsDisposed);

            var d3 = r.GetDisposable(); // CHECK
            d3.Dispose();
        }

        [Fact]
        public void RefCountDisposable_PrimaryDisposesFirst()
        {
            var d = new BooleanDisposable();
            var r = new RefCountDisposable(d);
            Assert.False(d.IsDisposed);

            var d1 = r.GetDisposable();
            var d2 = r.GetDisposable();
            Assert.False(d.IsDisposed);

            d1.Dispose();
            Assert.False(d.IsDisposed);

            r.Dispose();
            Assert.False(d.IsDisposed);

            d2.Dispose();
            Assert.True(d.IsDisposed);
        }

        [Fact]
        public void RefCountDisposable_Throw_If_Disposed()
        {
            var d = new BooleanDisposable();
            var r = new RefCountDisposable(d, true);
            r.Dispose();

            Assert.True(d.IsDisposed);

            ReactiveAssert.Throws<ObjectDisposedException>(() => { r.GetDisposable(); });
        }

        [Fact]
        public void ScheduledDisposable_Null()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new ScheduledDisposable(null, Disposable.Empty));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ScheduledDisposable(Scheduler.Immediate, null));
        }

        [Fact]
        public void ScheduledDisposable()
        {
            var d = new BooleanDisposable();
            var s = new ScheduledDisposable(Scheduler.Immediate, d);

            Assert.False(d.IsDisposed);

            Assert.Same(Scheduler.Immediate, s.Scheduler);
            Assert.Same(d, s.Disposable);

            s.Dispose();

            Assert.True(d.IsDisposed);
            Assert.True(s.IsDisposed);

            Assert.Same(Scheduler.Immediate, s.Scheduler);
            //Assert.Same(d, s.Disposable); // BREAKING CHANGE v2 > v1.x - Undefined behavior after disposal.
            s.Disposable.Dispose();            // This should be a nop.
        }

        [Fact]
        public void MultipleAssignmentDisposable()
        {
            var m = new MultipleAssignmentDisposable();

            var disp1 = false;
            var d1 = Disposable.Create(() => { disp1 = true; });
            m.Disposable = d1;
            Assert.Same(d1, m.Disposable);
            Assert.False(m.IsDisposed);

            var disp2 = false;
            var d2 = Disposable.Create(() => { disp2 = true; });
            m.Disposable = d2;
            Assert.Same(d2, m.Disposable);
            Assert.False(m.IsDisposed);
            Assert.False(disp1);

            m.Dispose();
            Assert.True(disp2);
            Assert.True(m.IsDisposed);
            //Assert.Null(m.Disposable); // BREAKING CHANGE v2 > v1.x - Undefined behavior after disposal.
            m.Disposable.Dispose();        // This should be a nop.

            var disp3 = false;
            var d3 = Disposable.Create(() => { disp3 = true; });
            m.Disposable = d3;
            Assert.True(disp3);
            //Assert.Null(m.Disposable); // BREAKING CHANGE v2 > v1.x - Undefined behavior after disposal.
            m.Disposable.Dispose();        // This should be a nop.
            Assert.True(m.IsDisposed);
        }

        [Fact]
        public void StableCompositeDisposable_ArgumentChecking()
        {
            var d = Disposable.Empty;

            ReactiveAssert.Throws<ArgumentNullException>(() => StableCompositeDisposable.Create(null, d));
            ReactiveAssert.Throws<ArgumentNullException>(() => StableCompositeDisposable.Create(d, null));

            ReactiveAssert.Throws<ArgumentNullException>(() => StableCompositeDisposable.Create(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => StableCompositeDisposable.Create(default(IEnumerable<IDisposable>)));

            ReactiveAssert.Throws<ArgumentException>(() => StableCompositeDisposable.Create(null, d, d));
            ReactiveAssert.Throws<ArgumentException>(() => StableCompositeDisposable.Create(d, null, d));
            ReactiveAssert.Throws<ArgumentException>(() => StableCompositeDisposable.Create(d, d, null));
        }

        [Fact]
        public void StableCompositeDisposable_Binary()
        {
            var disp1 = false;
            var d1 = Disposable.Create(() => { Assert.False(disp1); disp1 = true; });

            var disp2 = false;
            var d2 = Disposable.Create(() => { Assert.False(disp2); disp2 = true; });

            var d = StableCompositeDisposable.Create(d1, d2);

            Assert.False(disp1);
            Assert.False(disp2);
            Assert.False(d.IsDisposed);

            d.Dispose();

            Assert.True(disp1);
            Assert.True(disp2);
            Assert.True(d.IsDisposed);

            d.Dispose();

            Assert.True(disp1);
            Assert.True(disp2);
            Assert.True(d.IsDisposed);
        }

        [Fact]
        public void StableCompositeDisposable_Nary1()
        {
            var disp1 = false;
            var d1 = Disposable.Create(() => { Assert.False(disp1); disp1 = true; });

            var disp2 = false;
            var d2 = Disposable.Create(() => { Assert.False(disp2); disp2 = true; });

            var disp3 = false;
            var d3 = Disposable.Create(() => { Assert.False(disp3); disp3 = true; });

            var d = StableCompositeDisposable.Create(d1, d2, d3);

            Assert.False(disp1);
            Assert.False(disp2);
            Assert.False(disp3);
            Assert.False(d.IsDisposed);

            d.Dispose();

            Assert.True(disp1);
            Assert.True(disp2);
            Assert.True(disp3);
            Assert.True(d.IsDisposed);

            d.Dispose();

            Assert.True(disp1);
            Assert.True(disp2);
            Assert.True(disp3);
            Assert.True(d.IsDisposed);
        }

        [Fact]
        public void StableCompositeDisposable_Nary2()
        {
            var disp1 = false;
            var d1 = Disposable.Create(() => { Assert.False(disp1); disp1 = true; });

            var disp2 = false;
            var d2 = Disposable.Create(() => { Assert.False(disp2); disp2 = true; });

            var disp3 = false;
            var d3 = Disposable.Create(() => { Assert.False(disp3); disp3 = true; });

            var d = StableCompositeDisposable.Create(new List<IDisposable>(new[] { d1, d2, d3 }));

            Assert.False(disp1);
            Assert.False(disp2);
            Assert.False(disp3);
            Assert.False(d.IsDisposed);

            d.Dispose();

            Assert.True(disp1);
            Assert.True(disp2);
            Assert.True(disp3);
            Assert.True(d.IsDisposed);

            d.Dispose();

            Assert.True(disp1);
            Assert.True(disp2);
            Assert.True(disp3);
            Assert.True(d.IsDisposed);
        }

        [Fact]
        public void Disposable_TryRelease_Already_Disposed()
        {
            var field = default(IDisposable);

            Disposable.TryDispose(ref field);

            var count = 0;

            Assert.False(Disposable.TryRelease(ref field, 1, (d, i) => count = i));

            Assert.Equal(0, count);
        }
    }
}
