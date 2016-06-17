// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Reactive.Testing;
using Xunit;
using ReactiveTests.Dummies;

namespace ReactiveTests.Tests
{
    
    public class ObservableConversionTests : ReactiveTest
    {
        #region + Subscribe +

        [Fact]
        public void SubscribeToEnumerable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Subscribe<int>((IEnumerable<int>)null, DummyObserver<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Subscribe<int>(DummyEnumerable<int>.Instance, (IObserver<int>)null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Subscribe<int>((IEnumerable<int>)null, DummyObserver<int>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Subscribe<int>(DummyEnumerable<int>.Instance, DummyObserver<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Subscribe<int>(DummyEnumerable<int>.Instance, (IObserver<int>)null, DummyScheduler.Instance));
            ReactiveAssert.Throws<NullReferenceException>(() => NullEnumeratorEnumerable<int>.Instance.Subscribe(Observer.Create<int>(x => { }), Scheduler.CurrentThread));
        }

        [Fact]
        public void SubscribeToEnumerable_Finite()
        {
            var scheduler = new TestScheduler();

            var results = scheduler.CreateObserver<int>();
            var d = default(IDisposable);
            var xs = default(MockEnumerable<int>);

            scheduler.ScheduleAbsolute(Created, () => xs = new MockEnumerable<int>(scheduler, Enumerable_Finite()));
            scheduler.ScheduleAbsolute(Subscribed, () => d = xs.Subscribe(results, scheduler));
            scheduler.ScheduleAbsolute(Disposed, () => d.Dispose());

            scheduler.Start();

            results.Messages.AssertEqual(
                OnNext(201, 1),
                OnNext(202, 2),
                OnNext(203, 3),
                OnNext(204, 4),
                OnNext(205, 5),
                OnCompleted<int>(206)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 206)
            );
        }

        [Fact]
        public void SubscribeToEnumerable_Infinite()
        {
            var scheduler = new TestScheduler();

            var results = scheduler.CreateObserver<int>();
            var d = default(IDisposable);
            var xs = default(MockEnumerable<int>);

            scheduler.ScheduleAbsolute(Created, () => xs = new MockEnumerable<int>(scheduler, Enumerable_Infinite()));
            scheduler.ScheduleAbsolute(Subscribed, () => d = xs.Subscribe(results, scheduler));
            scheduler.ScheduleAbsolute(210, () => d.Dispose());

            scheduler.Start();

            results.Messages.AssertEqual(
                OnNext(201, 1),
                OnNext(202, 1),
                OnNext(203, 1),
                OnNext(204, 1),
                OnNext(205, 1),
                OnNext(206, 1),
                OnNext(207, 1),
                OnNext(208, 1),
                OnNext(209, 1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void SubscribeToEnumerable_Error()
        {
            var scheduler = new TestScheduler();

            var results = scheduler.CreateObserver<int>();
            var d = default(IDisposable);
            var xs = default(MockEnumerable<int>);
            var ex = new Exception();

            scheduler.ScheduleAbsolute(Created, () => xs = new MockEnumerable<int>(scheduler, Enumerable_Error(ex)));
            scheduler.ScheduleAbsolute(Subscribed, () => d = xs.Subscribe(results, scheduler));
            scheduler.ScheduleAbsolute(Disposed, () => d.Dispose());

            scheduler.Start();

            results.Messages.AssertEqual(
                OnNext(201, 1),
                OnNext(202, 2),
                OnNext(203, 3),
                OnError<int>(204, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 204)
            );
        }

#if !SILVERLIGHTM7
        [Fact]
        public void SubscribeToEnumerable_DefaultScheduler()
        {
            for (int i = 0; i < 100; i++)
            {
                var scheduler = new TestScheduler();

                var results1 = new List<int>();
                var results2 = new List<int>();

                var s1 = new Semaphore(0, 1);
                var s2 = new Semaphore(0, 1);

                Observable.Subscribe(Enumerable_Finite(),
                    Observer.Create<int>(x => results1.Add(x), ex => { throw ex; }, () => s1.Release()));
                Observable.Subscribe(Enumerable_Finite(),
                    Observer.Create<int>(x => results2.Add(x), ex => { throw ex; }, () => s2.Release()),
                    DefaultScheduler.Instance);

                s1.WaitOne();
                s2.WaitOne();

                results1.AssertEqual(results2);
            }
        }
#endif

        #endregion

        #region ToEnumerable

        [Fact]
        public void ToEnumerable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToEnumerable(default(IObservable<int>)));
        }

        [Fact]
        public void ToEnumerable_Generic()
        {
            Assert.True(Observable.Range(0, 10).ToEnumerable().SequenceEqual(Enumerable.Range(0, 10)));
        }

        [Fact]
        public void ToEnumerable_NonGeneric()
        {
            Assert.True(((IEnumerable)Observable.Range(0, 10).ToEnumerable()).Cast<int>().SequenceEqual(Enumerable.Range(0, 10)));
        }

        [Fact]
        public void ToEnumerable_ManualGeneric()
        {
            var res = Observable.Range(0, 10).ToEnumerable();
            var ieg = res.GetEnumerator();
            for (int i = 0; i < 10; i++)
            {
                Assert.True(ieg.MoveNext());
                Assert.Equal(i, ieg.Current);
            }
            Assert.False(ieg.MoveNext());
        }

        [Fact]
        public void ToEnumerable_ManualNonGeneric()
        {
            var res = (IEnumerable)Observable.Range(0, 10).ToEnumerable();
            var ien = res.GetEnumerator();
            for (int i = 0; i < 10; i++)
            {
                Assert.True(ien.MoveNext());
                Assert.Equal(i, ien.Current);
            }
            Assert.False(ien.MoveNext());
        }

        [Fact]
        public void ToEnumerable_ResetNotSupported()
        {
            ReactiveAssert.Throws<NotSupportedException>(() => Observable.Range(0, 10).ToEnumerable().GetEnumerator().Reset());
        }

        #endregion

        #region ToEvent

        [Fact]
        public void ToEvent_ArgumentChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToEvent(default(IObservable<Unit>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToEvent(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToEvent(default(IObservable<EventPattern<EventArgs>>)));
        }

        [Fact]
        public void ToEvent_Unit()
        {
            var src = new Subject<Unit>();
            var evt = src.ToEvent();

            var num = 0;
            var hnd = new Action<Unit>(_ =>
            {
                num++;
            });

            evt.OnNext += hnd;

            Assert.Equal(0, num);

            src.OnNext(new Unit());
            Assert.Equal(1, num);

            src.OnNext(new Unit());
            Assert.Equal(2, num);

            evt.OnNext -= hnd;

            src.OnNext(new Unit());
            Assert.Equal(2, num);
        }

        [Fact]
        public void ToEvent_NonUnit()
        {
            var src = new Subject<int>();
            var evt = src.ToEvent();

            var lst = new List<int>();
            var hnd = new Action<int>(e =>
            {
                lst.Add(e);
            });

            evt.OnNext += hnd;

            src.OnNext(1);
            src.OnNext(2);

            evt.OnNext -= hnd;

            src.OnNext(3);

            Assert.True(lst.SequenceEqual(new[] { 1, 2 }));
        }

        [Fact]
        public void ToEvent_FromEvent()
        {
            var src = new Subject<int>();
            var evt = src.ToEvent();

            var res = Observable.FromEvent<int>(h => evt.OnNext += h, h => evt.OnNext -= h);

            var lst = new List<int>();
            using (res.Subscribe(e => lst.Add(e), () => Assert.True(false)))
            {
                src.OnNext(1);
                src.OnNext(2);
            }

            src.OnNext(3);

            Assert.True(lst.SequenceEqual(new[] { 1, 2 }));
        }

        #endregion

        #region ToEventPattern

        [Fact]
        public void ToEventPattern_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToEventPattern<EventArgs>(null));
        }

        [Fact]
        public void ToEventPattern_IEvent()
        {
            var src = new Subject<EventPattern<EventArgs<int>>>();
            var evt = src.ToEventPattern();

            var snd = new object();

            var lst = new List<int>();
            var hnd = new EventHandler<EventArgs<int>>((s, e) =>
            {
                Assert.Same(snd, s);
                lst.Add(e.Value);
            });

            evt.OnNext += hnd;

            src.OnNext(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(42)));
            src.OnNext(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(43)));

            evt.OnNext -= hnd;

            src.OnNext(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(44)));

            Assert.True(lst.SequenceEqual(new[] { 42, 43 }));
        }

        [Fact]
        public void ToEventPattern_IEvent_Fails()
        {
            var src = new Subject<EventPattern<EventArgs<int>>>();
            var evt = src.ToEventPattern();

            var snd = new object();

            var lst = new List<int>();
            var hnd = new EventHandler<EventArgs<int>>((s, e) =>
            {
                Assert.Same(snd, s);
                lst.Add(e.Value);
            });

            evt.OnNext += hnd;

            src.OnNext(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(42)));
            src.OnNext(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(43)));

            var ex = new Exception();

            ReactiveAssert.Throws(ex, () => src.OnError(ex));

            Assert.True(lst.SequenceEqual(new[] { 42, 43 }));
        }

        [Fact]
        public void ToEventPattern_IEvent_Completes()
        {
            var src = new Subject<EventPattern<EventArgs<int>>>();
            var evt = src.ToEventPattern();

            var snd = new object();

            var lst = new List<int>();
            var hnd = new EventHandler<EventArgs<int>>((s, e) =>
            {
                Assert.Same(snd, s);
                lst.Add(e.Value);
            });

            evt.OnNext += hnd;

            src.OnNext(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(42)));
            src.OnNext(new EventPattern<EventArgs<int>>(snd, new EventArgs<int>(43)));

            src.OnCompleted();

            Assert.True(lst.SequenceEqual(new[] { 42, 43 }));
        }

        class EventSrc
        {
            public event EventHandler<EventArgs<string>> E;

            public void On(string s)
            {
                var e = E;
                if (e != null)
                    e(this, new EventArgs<string>(s));
            }
        }

        class EventArgs<T> : EventArgs
        {
            public T Value { get; private set;  }

            public EventArgs(T value)
            {
                Value = value;
            }
        }

        [Fact]
        public void FromEventPattern_ToEventPattern()
        {
            var src = new EventSrc();
            var evt = Observable.FromEventPattern<EventHandler<EventArgs<string>>, EventArgs<string>>(h => new EventHandler<EventArgs<string>>(h), h => src.E += h, h => src.E -= h);

            var res = evt.ToEventPattern();

            var lst = new List<string>();
            var hnd = new EventHandler<EventArgs<string>>((s, e) =>
            {
                Assert.Same(src, s);
                lst.Add(e.Value);
            });

            src.On("bar");

            res.OnNext += hnd;

            src.On("foo");
            src.On("baz");

            res.OnNext -= hnd;

            src.On("qux");

            Assert.True(lst.SequenceEqual(new[] { "foo", "baz" }));
        }

        [Fact]
        public void ToEvent_DuplicateHandlers()
        {
            var src = new Subject<Unit>();
            var evt = src.ToEvent();

            var num = 0;
            var hnd = new Action<Unit>(e => num++);

            evt.OnNext += hnd;

            Assert.Equal(0, num);

            src.OnNext(new Unit());
            Assert.Equal(1, num);

            evt.OnNext += hnd;

            src.OnNext(new Unit());
            Assert.Equal(3, num);

            evt.OnNext -= hnd;

            src.OnNext(new Unit());
            Assert.Equal(4, num);

            evt.OnNext -= hnd;

            src.OnNext(new Unit());
            Assert.Equal(4, num);
        }

        [Fact]
        public void ToEvent_SourceCompletes()
        {
            var src = new Subject<Unit>();
            var evt = src.ToEvent();

            var num = 0;
            var hnd = new Action<Unit>(e => num++);

            evt.OnNext += hnd;

            Assert.Equal(0, num);

            src.OnNext(new Unit());
            Assert.Equal(1, num);

            src.OnNext(new Unit());
            Assert.Equal(2, num);

            src.OnCompleted();
            Assert.Equal(2, num);

#if !SILVERLIGHT // FieldAccessException
            var tbl = GetSubscriptionTable(evt);
            Assert.True(tbl.Count == 0);
#endif
        }

        [Fact]
        public void ToEvent_SourceFails()
        {
            var src = new Subject<Unit>();
            var evt = src.ToEvent();

            var num = 0;
            var hnd = new Action<Unit>(e => num++);

            evt.OnNext += hnd;

            Assert.Equal(0, num);

            src.OnNext(new Unit());
            Assert.Equal(1, num);

            src.OnNext(new Unit());
            Assert.Equal(2, num);

            var ex = new Exception();

            ReactiveAssert.Throws(ex, () => src.OnError(ex));

#if !SILVERLIGHT // FieldAccessException
            var tbl = GetSubscriptionTable(evt);
            Assert.True(tbl.Count == 0);
#endif
        }

        [Fact]
        public void ToEvent_DoneImmediately()
        {
            var src = Observable.Empty<Unit>();
            var evt = src.ToEvent();

            var num = 0;
            var hnd = new Action<Unit>(e => num++);

            for (int i = 0; i < 2; i++)
            {
                evt.OnNext += hnd;

                Assert.Equal(0, num);

#if !SILVERLIGHT // FieldAccessException
                var tbl = GetSubscriptionTable(evt);
                Assert.True(tbl.Count == 0);
#endif
            }
        }

        [Fact]
        public void ToEvent_UnbalancedHandlers()
        {
            var src = new Subject<Unit>();
            var evt = src.ToEvent();

            var num = 0;
            var hnd = new Action<Unit>(e => num++);

            evt.OnNext += hnd;
            Assert.Equal(0, num);

            evt.OnNext -= hnd;
            Assert.Equal(0, num);

            evt.OnNext -= hnd;
            Assert.Equal(0, num);

            evt.OnNext += hnd;
            Assert.Equal(0, num);

            src.OnNext(new Unit());
            Assert.Equal(1, num);

            src.OnNext(new Unit());
            Assert.Equal(2, num);

            evt.OnNext -= hnd;
            Assert.Equal(2, num);

            src.OnNext(new Unit());
            Assert.Equal(2, num);
        }

        private static Dictionary<Delegate, Stack<IDisposable>> GetSubscriptionTable(object evt)
        {
            return (Dictionary<Delegate, Stack<IDisposable>>)evt.GetType().GetField("_subscriptions", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(evt);
        }

        [Fact]
        public void EventPattern_Equality()
        {
            var e1 = new EventPattern<string, EventArgs>("Bart", EventArgs.Empty);
            var e2 = new EventPattern<string, EventArgs>("Bart", EventArgs.Empty);

            Assert.True(e1.Equals(e1));
            Assert.True(e1.Equals(e2));
            Assert.True(e2.Equals(e1));
            Assert.True(e1 == e2);
            Assert.True(!(e1 != e2));
            Assert.True(e1.GetHashCode() == e2.GetHashCode());

            Assert.False(e1.Equals(null));
            Assert.False(e1.Equals("xy"));
            Assert.False(e1 == null);
        }

        [Fact]
        public void EventPattern_Inequality()
        {
            var a1 = new MyEventArgs();
            var a2 = new MyEventArgs();

            var e1 = new EventPattern<string, MyEventArgs>("Bart", a1);
            var e2 = new EventPattern<string, MyEventArgs>("John", a1);
            var e3 = new EventPattern<string, MyEventArgs>("Bart", a2);

            Assert.True(!e1.Equals(e2));
            Assert.True(!e2.Equals(e1));
            Assert.True(!(e1 == e2));
            Assert.True(e1 != e2);
            Assert.True(e1.GetHashCode() != e2.GetHashCode());

            Assert.True(!e1.Equals(e3));
            Assert.True(!e3.Equals(e1));
            Assert.True(!(e1 == e3));
            Assert.True(e1 != e3);
            Assert.True(e1.GetHashCode() != e3.GetHashCode());
        }

        class MyEventArgs : EventArgs
        {
        }

        #endregion

        #region + ToObservable +

        [Fact]
        public void EnumerableToObservable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToObservable((IEnumerable<int>)null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToObservable(DummyEnumerable<int>.Instance, (IScheduler)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToObservable(DummyEnumerable<int>.Instance, DummyScheduler.Instance).Subscribe(null));
            ReactiveAssert.Throws<NullReferenceException>(() => Observable.ToObservable(NullEnumeratorEnumerable<int>.Instance, Scheduler.CurrentThread).Subscribe());
        }

        [Fact]
        public void EnumerableToObservable_Complete()
        {
            var scheduler = new TestScheduler();

            var e = new MockEnumerable<int>(scheduler,
                new[] { 3, 1, 2, 4 }
            );

            var results = scheduler.Start(() =>
                e.ToObservable(scheduler)
            );

            results.Messages.AssertEqual(
                OnNext(201, 3),
                OnNext(202, 1),
                OnNext(203, 2),
                OnNext(204, 4),
                OnCompleted<int>(205)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 205)
            );
        }

        [Fact]
        public void EnumerableToObservable_Dispose()
        {
            var scheduler = new TestScheduler();

            var e = new MockEnumerable<int>(scheduler,
                new[] { 3, 1, 2, 4 }
            );

            var results = scheduler.Start(() =>
                e.ToObservable(scheduler),
                203
            );

            results.Messages.AssertEqual(
                OnNext(201, 3),
                OnNext(202, 1)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 203)
            );
        }

        [Fact]
        public void EnumerableToObservable_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var e = new MockEnumerable<int>(scheduler,
                EnumerableToObservable_Error_Core(ex)
            );

            var results = scheduler.Start(() =>
                e.ToObservable(scheduler)
            );

            results.Messages.AssertEqual(
                OnNext(201, 1),
                OnNext(202, 2),
                OnError<int>(203, ex)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 203)
            );
        }

        [Fact]
        public void EnumerableToObservable_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToObservable((IEnumerable<int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToObservable(DummyEnumerable<int>.Instance).Subscribe(null));
        }

        [Fact]
        public void EnumerableToObservable_Default()
        {
            var xs = new[] { 4, 3, 1, 5, 9, 2 };

            xs.ToObservable().AssertEqual(xs.ToObservable(DefaultScheduler.Instance));
        }

#if !NO_PERF
        [Fact]
        public void EnumerableToObservable_LongRunning_Complete()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var scheduler = new TestLongRunningScheduler(x => start = x, x => end = x);

            var e = new[] { 3, 1, 2, 4 };

            var results = e.ToObservable(scheduler);

            var lst = new List<int>();
            results.Subscribe(lst.Add);

            start.WaitOne();
            end.WaitOne();

            Assert.True(e.SequenceEqual(lst));
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public void EnumerableToObservable_LongRunning_Dispose()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var scheduler = new TestLongRunningScheduler(x => start = x, x => end = x);

            var e = Enumerable.Range(0, int.MaxValue);

            var results = e.ToObservable(scheduler);

            var lst = new List<int>();
            var d = results.Subscribe(lst.Add);

            start.WaitOne();

            while (lst.Count < 100)
                ;

            d.Dispose();
            end.WaitOne();

            Assert.True(e.Take(100).SequenceEqual(lst.Take(100)));
        }

        [Fact]
        public void EnumerableToObservable_LongRunning_Error()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var scheduler = new TestLongRunningScheduler(x => start = x, x => end = x);

            var ex = new Exception();
            var e = EnumerableToObservable_Error_Core(ex);

            var results = e.ToObservable(scheduler);

            var lst = new List<int>();
            var err = default(Exception);
            results.Subscribe(lst.Add, ex_ => err = ex_);

            start.WaitOne();
            end.WaitOne();

            Assert.True(new[] { 1, 2 }.SequenceEqual(lst));
            Assert.Same(ex, err);
        }
#endif

        static IEnumerable<int> EnumerableToObservable_Error_Core(Exception ex)
        {
            yield return 1;
            yield return 2;
            throw ex;
        }

        [Fact]
        public void EnumerableToObservable_GetEnumeratorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = new RogueEnumerable<int>(ex);
            
            var res = scheduler.Start(() =>
                xs.ToObservable(scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        #endregion

        #region |> Helpers <|

        IEnumerable<int> Enumerable_Finite()
        {
            yield return 1;
            yield return 2;
            yield return 3;
            yield return 4;
            yield return 5;
            yield break;
        }

        IEnumerable<int> Enumerable_Infinite()
        {
            while (true)
                yield return 1;
        }

        IEnumerable<int> Enumerable_Error(Exception exception)
        {
            yield return 1;
            yield return 2;
            yield return 3;
            throw exception;
        }

        #endregion
    }
}
