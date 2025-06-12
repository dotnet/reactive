﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class ToLookupTest : ReactiveTest
    {

        [TestMethod]
        public void ToLookup_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup(null, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup(DummyObservable<int>.Instance, null, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup(null, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int>(DummyObservable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup(null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup(DummyObservable<int>.Instance, null, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int, int>(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, null, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup(null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int, int>(DummyObservable<int>.Instance, null, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int, int>(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, null));
        }


        [TestMethod]
        public void ToLookup_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(660)
            );

            var res = scheduler.Start(() =>
                xs.ToLookup(x => x % 2, x => x * 2, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnNext<ILookup<int, int>>(660, d =>
                {
                    return d.Count == 2
                        && d[0].SequenceEqual([4, 8])
                        && d[1].SequenceEqual([6, 10]);
                }),
                OnCompleted<ILookup<int, int>>(660)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [TestMethod]
        public void ToLookup_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnError<int>(660, ex)
            );

            var res = scheduler.Start(() =>
                xs.ToLookup(x => x % 2, x => x * 2, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnError<ILookup<int, int>>(660, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [TestMethod]
        public void ToLookup_KeySelectorThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ToLookup(x => { if (x < 4) { return x * 2; } throw ex; }, x => x * 4, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnError<ILookup<int, int>>(440, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 440)
            );
        }

        [TestMethod]
        public void ToLookup_ElementSelectorThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ToLookup(x => x * 2, x => { if (x < 4) { return x * 4; } throw ex; }, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnError<ILookup<int, int>>(440, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 440)
            );
        }

        [TestMethod]
        public void ToLookup_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5)
            );

            var res = scheduler.Start(() =>
                xs.ToLookup(x => x % 2, x => x * 2, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void ToLookup_Default()
        {
            var d1 = Observable.Range(1, 10).ToLookup(x => (x % 2).ToString()).First();
            d1["0"].AssertEqual(2, 4, 6, 8, 10);

            var d2 = Observable.Range(1, 10).ToLookup(x => (x % 2).ToString(), x => x * 2).First();
            d2["1"].AssertEqual(2, 6, 10, 14, 18);

            var d3 = Observable.Range(1, 10).ToLookup(x => (x % 2).ToString(), EqualityComparer<string>.Default).First();
            d3["0"].AssertEqual(2, 4, 6, 8, 10);
        }

        [TestMethod]
        public void ToLookup_Contains()
        {
            var d1 = Observable.Range(1, 10).ToLookup(x => (x % 2).ToString()).First();
            Assert.True(d1.Contains("1"));
            Assert.False(d1.Contains("2"));
        }

        [TestMethod]
        public void ToLookup_Hides_Internal_List()
        {
            var d1 = Observable.Range(1, 10).ToLookup(x => (x % 2).ToString()).First();

            // Up to .NET 7.0, the wrapper returned by LINQ to Objects' Skip (which is
            // what Rx uses today to hide the list) used not to implement IList or
            // ICollection. As of .NET 8.0 it does, but we can check we don't have
            // access to the underlying list by checking that we are unable to modify
            // it. Before .NET 8.0, these tests succeed because the wrapped list
            // doesn't implement the interfaces. On .NET 8.0 they succeed because it
            // provides a read-only implementation of them.
            Assert.False(d1["0"] is ICollection<int> coll && !coll.IsReadOnly);
            Assert.False(d1["0"] is IList<int> list && !list.IsReadOnly);
        }

        [TestMethod]
        public void ToLookup_Groups()
        {
            var d1 = Observable.Range(1, 10).ToLookup(x => (x % 2).ToString()).First();

            foreach (var g in d1)
            {
                if (g.Key == "0")
                {
                    g.AssertEqual(2, 4, 6, 8, 10);
                }
                else if (g.Key == "1")
                {
                    g.AssertEqual(1, 3, 5, 7, 9);
                }
                else
                {
                    Assert.True(false, "Unknown group.");
                }
            }
        }

        [TestMethod]
        public void ToLookup_Groups_2()
        {
            var d1 = Observable.Range(1, 10).ToLookup(x => (x % 2).ToString()).First();

            foreach (IGrouping<string, int> g in ((System.Collections.IEnumerable)d1))
            {
                if (g.Key == "0")
                {
                    var l = new List<int>();
                    foreach (int v in ((System.Collections.IEnumerable)g))
                    {
                        l.Add(v);
                    }

                    l.AssertEqual(2, 4, 6, 8, 10);
                }
                else if (g.Key == "1")
                {
                    var l = new List<int>();
                    foreach (int v in ((System.Collections.IEnumerable)g))
                    {
                        l.Add(v);
                    }

                    l.AssertEqual(1, 3, 5, 7, 9);
                }
                else
                {
                    Assert.True(false, "Unknown group.");
                }
            }
        }

        [TestMethod]
        public void ToLookup_IndexerForInvalidKey()
        {
            var d1 = Observable.Range(1, 10).ToLookup(x => (x % 2).ToString()).First();

            var values = d1["2"];
            values.AssertEqual(Enumerable.Empty<int>());
        }

    }
}
