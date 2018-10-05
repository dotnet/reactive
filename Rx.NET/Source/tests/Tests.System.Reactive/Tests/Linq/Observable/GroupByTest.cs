// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class GroupByTest : ReactiveTest
    {
        #region + GroupBy +

        [Fact]
        public void GroupBy_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(null, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, (Func<int, int>)null, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default).Subscribe(null));
        }

        [Fact]
        public void GroupBy_KeyEle_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy((Func<int, int>)null, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, (Func<int, int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance).Subscribe(null));
        }

        [Fact]
        public void GroupBy_KeyComparer_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(null, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, EqualityComparer<int>.Default).Subscribe(null));
        }

        [Fact]
        public void GroupBy_Key_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy((Func<int, int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance).Subscribe(null));
        }

        [Fact]
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

            Assert.Equal(12, keyInvoked);
        }

        [Fact]
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

            Assert.Equal(12, keyInvoked);
            Assert.Equal(12, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(12, keyInvoked);
            Assert.Equal(12, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(5, keyInvoked);
            Assert.Equal(5, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(10, keyInvoked);
            Assert.Equal(9, eleInvoked);
        }

        [Fact]
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
                        {
                            throw ex;
                        }

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

            Assert.Equal(10, keyInvoked);
            Assert.Equal(10, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(4, keyInvoked);
            Assert.Equal(3, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(8, keyInvoked);
            Assert.Equal(7, eleInvoked);
        }

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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
                {
                    throw ex;
                }

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

        [Fact]
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
                {
                    throw ex;
                }

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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());
            scheduler.ScheduleAbsolute(280, () => innerSubscriptions["baR"].Dispose());
            scheduler.ScheduleAbsolute(355, () => innerSubscriptions["Baz"].Dispose());
            scheduler.ScheduleAbsolute(400, () => innerSubscriptions["qux"].Dispose());

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        private static string Reverse(string s)
        {
            var sb = new StringBuilder();

            for (var i = s.Length - 1; i >= 0; i--)
            {
                sb.Append(s[i]);
            }

            return sb.ToString();
        }

        #endregion

        #region + GroupBy w/capacity +

        private const int _groupByCapacity = 1024;

        [Fact]
        public void GroupBy_Capacity_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, _groupByCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(null, DummyFunc<int, int>.Instance, _groupByCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, (Func<int, int>)null, _groupByCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, _groupByCapacity, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, _groupByCapacity, EqualityComparer<int>.Default).Subscribe(null));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, -1, EqualityComparer<int>.Default));
        }

        [Fact]
        public void GroupBy_Capacity_KeyEle_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, _groupByCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy((Func<int, int>)null, DummyFunc<int, int>.Instance, _groupByCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, (Func<int, int>)null, _groupByCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, _groupByCapacity).Subscribe(null));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, -1));
        }

        [Fact]
        public void GroupBy_Capacity_KeyComparer_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance, _groupByCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(null, _groupByCapacity, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, _groupByCapacity, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, _groupByCapacity, EqualityComparer<int>.Default).Subscribe(null));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, -1, EqualityComparer<int>.Default));
        }

        [Fact]
        public void GroupBy_Capacity_Key_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).GroupBy(DummyFunc<int, int>.Instance, _groupByCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy((Func<int, int>)null, _groupByCapacity));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, _groupByCapacity).Subscribe(null));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.GroupBy(DummyFunc<int, int>.Instance, -1));
        }

        [Fact]
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

            Assert.Equal(12, keyInvoked);
        }

        [Fact]
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

            Assert.Equal(12, keyInvoked);
            Assert.Equal(12, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(12, keyInvoked);
            Assert.Equal(12, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(5, keyInvoked);
            Assert.Equal(5, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(10, keyInvoked);
            Assert.Equal(9, eleInvoked);
        }

        [Fact]
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
                        {
                            throw ex;
                        }

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

            Assert.Equal(10, keyInvoked);
            Assert.Equal(10, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(4, keyInvoked);
            Assert.Equal(3, eleInvoked);
        }

        [Fact]
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

            Assert.Equal(8, keyInvoked);
            Assert.Equal(7, eleInvoked);
        }

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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
                {
                    throw ex;
                }

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

        [Fact]
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
                {
                    throw ex;
                }

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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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
                {
                    d.Dispose();
                }
            });

            scheduler.ScheduleAbsolute(320, () => innerSubscriptions["foo"].Dispose());
            scheduler.ScheduleAbsolute(280, () => innerSubscriptions["baR"].Dispose());
            scheduler.ScheduleAbsolute(355, () => innerSubscriptions["Baz"].Dispose());
            scheduler.ScheduleAbsolute(400, () => innerSubscriptions["qux"].Dispose());

            scheduler.Start();

            Assert.Equal(4, inners.Count);

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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

    }

    internal class GroupByComparer : IEqualityComparer<string>
    {
        private TestScheduler _scheduler;
        private readonly int _equalsThrowsAfter;
        private readonly ushort _getHashCodeThrowsAfter;

        public Exception HashCodeException = new Exception();
        public Exception EqualsException = new Exception();

        public GroupByComparer(TestScheduler scheduler, ushort equalsThrowsAfter, ushort getHashCodeThrowsAfter)
        {
            this._scheduler = scheduler;
            this._equalsThrowsAfter = equalsThrowsAfter;
            this._getHashCodeThrowsAfter = getHashCodeThrowsAfter;
        }

        public GroupByComparer(TestScheduler scheduler)
            : this(scheduler, ushort.MaxValue, ushort.MaxValue)
        {
        }

        public bool Equals(string x, string y)
        {
            if (_scheduler.Clock > _equalsThrowsAfter)
            {
                throw EqualsException;
            }

            return x.Equals(y, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            if (_scheduler.Clock > _getHashCodeThrowsAfter)
            {
                throw HashCodeException;
            }

            return StringComparer.OrdinalIgnoreCase.GetHashCode(obj);
        }
    }
}
