// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>ToLookup</c> scenarios, from Rx.NET's <c>ToLookupTest.cs</c>.</summary>
/// <remarks>
/// 11 of the file's 12 tests (<c>ToLookup_ArgumentChecking</c> is the code-generated
/// stratum), all transcribed as they are: completion, failure, a key selector and an element
/// selector that throw, disposal before completion, and the lookup itself (the key-only forms
/// over <c>Range</c>, <c>Contains</c>, the hidden group list, enumeration through both
/// interfaces, the indexer on an absent key); where the original blocks on <c>First()</c> the
/// shared text reads the one element of what <c>ToListAsync</c> collects, and its
/// <c>Assert.True(false, …)</c> is <c>Assert.Fail</c>. The group assertions are the
/// harness's element-wise <c>AssertEqual</c>, added for them as Rx.NET's
/// <c>ReactiveAssert</c> has it.
/// </remarks>
public abstract class ToLookupTests : SharedReactiveTest
{
    [TestMethod]
    public void ToLookup_Completed()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnNext(440, 4),
            OnNext(550, 5),
            OnCompleted<int>(660)
        );

        var res = Scheduler.Start(() =>
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
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnNext(440, 4),
            OnNext(550, 5),
            OnError<int>(660, ex)
        );

        var res = Scheduler.Start(() =>
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
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnNext(440, 4),
            OnNext(550, 5),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
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
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnNext(440, 4),
            OnNext(550, 5),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
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
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnNext(440, 4),
            OnNext(550, 5)
        );

        var res = Scheduler.Start(() =>
            xs.ToLookup(x => x % 2, x => x * 2, EqualityComparer<int>.Default)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public async Task ToLookup_Default()
    {
        var d1 = (await ToListAsync(Seq.Range(1, 10).ToLookup(x => (x % 2).ToString()))).Single();
        d1["0"].AssertEqual(2, 4, 6, 8, 10);

        var d2 = (await ToListAsync(
            Seq.Range(1, 10).ToLookup(x => (x % 2).ToString(), x => x * 2)))
            .Single();
        d2["1"].AssertEqual(2, 6, 10, 14, 18);

        var d3 = (await ToListAsync(
            Seq.Range(1, 10).ToLookup(x => (x % 2).ToString(), EqualityComparer<string>.Default)))
            .Single();
        d3["0"].AssertEqual(2, 4, 6, 8, 10);
    }

    [TestMethod]
    public async Task ToLookup_Contains()
    {
        var d1 = (await ToListAsync(Seq.Range(1, 10).ToLookup(x => (x % 2).ToString()))).Single();
        Assert.IsTrue(d1.Contains("1"));
        Assert.IsFalse(d1.Contains("2"));
    }

    [TestMethod]
    public async Task ToLookup_Hides_Internal_List()
    {
        var d1 = (await ToListAsync(Seq.Range(1, 10).ToLookup(x => (x % 2).ToString()))).Single();

        // Up to .NET 7.0, the wrapper returned by LINQ to Objects' Skip (which is
        // what Rx uses today to hide the list) used not to implement IList or
        // ICollection. As of .NET 8.0 it does, but we can check we don't have
        // access to the underlying list by checking that we are unable to modify
        // it. Before .NET 8.0, these tests succeed because the wrapped list
        // doesn't implement the interfaces. On .NET 8.0 they succeed because it
        // provides a read-only implementation of them.
        Assert.IsFalse(d1["0"] is ICollection<int> coll && !coll.IsReadOnly);
        Assert.IsFalse(d1["0"] is IList<int> list && !list.IsReadOnly);
    }

    [TestMethod]
    public async Task ToLookup_Groups()
    {
        var d1 = (await ToListAsync(Seq.Range(1, 10).ToLookup(x => (x % 2).ToString()))).Single();

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
                Assert.Fail("Unknown group.");
            }
        }
    }

    [TestMethod]
    public async Task ToLookup_Groups_2()
    {
        var d1 = (await ToListAsync(Seq.Range(1, 10).ToLookup(x => (x % 2).ToString()))).Single();

        foreach (IGrouping<string, int> g in ((global::System.Collections.IEnumerable)d1))
        {
            if (g.Key == "0")
            {
                var l = new List<int>();
                foreach (int v in ((global::System.Collections.IEnumerable)g))
                {
                    l.Add(v);
                }

                l.AssertEqual(2, 4, 6, 8, 10);
            }
            else if (g.Key == "1")
            {
                var l = new List<int>();
                foreach (int v in ((global::System.Collections.IEnumerable)g))
                {
                    l.Add(v);
                }

                l.AssertEqual(1, 3, 5, 7, 9);
            }
            else
            {
                Assert.Fail("Unknown group.");
            }
        }
    }

    [TestMethod]
    public async Task ToLookup_IndexerForInvalidKey()
    {
        var d1 = (await ToListAsync(Seq.Range(1, 10).ToLookup(x => (x % 2).ToString()))).Single();

        var values = d1["2"];
        values.AssertEqual(Enumerable.Empty<int>());
    }
}
