// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class GroupByUntilTest : ReactiveTest
    {
        #region + GroupByUntil +

        [Fact]
        public void GroupByUntil_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>), EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, default(IEqualityComparer<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>), EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, default(IEqualityComparer<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>)));
        }

        [Fact]
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

            Assert.Equal(12, keyInvoked);
        }

        [Fact]
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

            Assert.Equal(12, keyInvoked);
            Assert.Equal(12, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(12, keyInvoked);
            Assert.Equal(12, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(5, keyInvoked);
            Assert.Equal(5, eleInvoked);
        }

        [Fact]
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
                        {
                            throw ex;
                        }

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

            Assert.Equal(10, keyInvoked);
            Assert.Equal(9, eleInvoked);
        }

        [Fact]
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
                        {
                            throw ex;
                        }

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

            Assert.Equal(10, keyInvoked);
            Assert.Equal(10, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(4, keyInvoked);
            Assert.Equal(3, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(8, keyInvoked);
            Assert.Equal(7, eleInvoked);
        }

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(5, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnCompleted<string>(320)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
                OnCompleted<string>(420)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(5, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(5, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnCompleted<string>(320)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
                OnCompleted<string>(420)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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
                {
                    throw ex;
                }

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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(3, inners.Count);

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

        [Fact]
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
                {
                    throw ex;
                }

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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.ScheduleAbsolute(320, () => outerSubscription.Dispose());

            scheduler.Start();

            Assert.Equal(2, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());

            scheduler.Start();

            Assert.Equal(5, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());
            scheduler.ScheduleAbsolute(280, () => innerSubscriptions["baR"].Dispose());
            scheduler.ScheduleAbsolute(355, () => innerSubscriptions["Baz"].Dispose());
            scheduler.ScheduleAbsolute(400, () => innerSubscriptions["qux"].Dispose());

            scheduler.Start();

            Assert.Equal(5, inners.Count);

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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

            Assert.Equal(12, keyInvoked);
            Assert.Equal(12, eleInvoked);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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
            var res = scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => { if (g.Key == null) { n++; } return Observable.Timer(TimeSpan.FromTicks(50), scheduler); }).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

            Assert.Equal(2, n);

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

        [Fact]
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
            var res = scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => { if (g.Key == null) { n++; } return Observable.Timer(TimeSpan.FromTicks(50), scheduler).IgnoreElements(); }).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

            Assert.Equal(2, n);

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

        [Fact]
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

            Assert.Same(ex, err);

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

        [Fact]
        public void GroupByUntil_Capacity_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>), _groupByUntilCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>), _groupByUntilCapacity));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>), _groupByUntilCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(default, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, default, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, _groupByUntilCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, default(Func<IGroupedObservable<int, int>, IObservable<int>>), _groupByUntilCapacity));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, -1, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, -1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, -1, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.GroupByUntil(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<IGroupedObservable<int, int>, IObservable<int>>.Instance, -1));
        }

        [Fact]
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

            Assert.Equal(12, keyInvoked);
        }

        [Fact]
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

            Assert.Equal(12, keyInvoked);
            Assert.Equal(12, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(12, keyInvoked);
            Assert.Equal(12, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(5, keyInvoked);
            Assert.Equal(5, eleInvoked);
        }

        [Fact]
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
                        {
                            throw ex;
                        }

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

            Assert.Equal(10, keyInvoked);
            Assert.Equal(9, eleInvoked);
        }

        [Fact]
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
                        {
                            throw ex;
                        }

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

            Assert.Equal(10, keyInvoked);
            Assert.Equal(10, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(4, keyInvoked);
            Assert.Equal(3, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(8, keyInvoked);
            Assert.Equal(7, eleInvoked);
        }

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(5, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnCompleted<string>(320)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
                OnCompleted<string>(420)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(5, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(5, inners.Count);

            res["foo"].Messages.AssertEqual(
                OnCompleted<string>(320)
            );

            res["baR"].Messages.AssertEqual(
                OnNext(390, "rab   "),
                OnNext(420, "  RAB "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
                OnCompleted<string>(420)
            );

            res["Baz"].Messages.AssertEqual(
                OnNext(480, "  zab"),
                OnNext(510, " ZAb "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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
                {
                    throw ex;
                }

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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(3, inners.Count);

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

        [Fact]
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
                {
                    throw ex;
                }

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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.ScheduleAbsolute(320, () => outerSubscription.Dispose());

            scheduler.Start();

            Assert.Equal(2, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());

            scheduler.Start();

            Assert.Equal(5, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());
            scheduler.ScheduleAbsolute(280, () => innerSubscriptions["baR"].Dispose());
            scheduler.ScheduleAbsolute(355, () => innerSubscriptions["Baz"].Dispose());
            scheduler.ScheduleAbsolute(400, () => innerSubscriptions["qux"].Dispose());

            scheduler.Start();

            Assert.Equal(5, inners.Count);

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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

            Assert.Equal(12, keyInvoked);
            Assert.Equal(12, eleInvoked);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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
            var res = scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => { if (g.Key == null) { n++; } return Observable.Timer(TimeSpan.FromTicks(50), scheduler); }, _groupByUntilCapacity).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

            Assert.Equal(2, n);

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

        [Fact]
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
            var res = scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => { if (g.Key == null) { n++; } return Observable.Timer(TimeSpan.FromTicks(50), scheduler).IgnoreElements(); }, _groupByUntilCapacity).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

            Assert.Equal(2, n);

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

        [Fact]
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

            Assert.Same(ex, err);

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

        private static string Reverse(string s)
        {
            var sb = new StringBuilder();

            for (var i = s.Length - 1; i >= 0; i--)
            {
                sb.Append(s[i]);
            }

            return sb.ToString();
        }
    }
}
