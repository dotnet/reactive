// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Reactive;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Zip</c> scenarios, from Rx.NET's <c>ZipTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 196 of the file's 199 tests, transcribed mechanically as <c>CombineLatest</c>'s were: five
/// families (<c>SymmetricReturn</c>, <c>AllCompleted</c>, <c>Never</c>, <c>Empty</c>,
/// <c>SelectorThrows</c>) at every arity from 2 to 16, the first four with and without a result
/// selector; the <c>NAry_*</c> tests over the <c>params</c> and list forms; the two-source tests
/// over <c>o1.Zip(o2, ...)</c>; and the <c>ZipWithEnumerable_*</c> tests over an observable
/// zipped with an enumerable, where the enumerable is the shared <see cref="MockEnumerable{T}"/>
/// and the original's <c>Subscriptions</c> assertions on it become <c>Enumerations</c>. Where the
/// original writes <c>Observable.Zip</c> the scenario writes <see cref="Seq.Zip{T}(Seq{T}[])"/>
/// and its siblings, and where it writes <c>ObservableEx.Zip</c> for the tuple-producing forms,
/// <see cref="SeqEx"/>.
/// </para>
/// <para>
/// Hand-adjusted: the three <c>SomeData*</c> tests, which read the recorded messages back one by
/// one, assert over the whole log instead, with the same expectation; the two
/// <c>NoAsyncDispose*</c> tests, which dispose the subscription from inside the enumerator, use
/// the shared subject and await the dispose; <c>NAry_Enumerable_Throws</c> and the three
/// <c>*WithImmediateReturn</c> tests, which subscribe in real time, await the raw surface. Not
/// here: the three <c>*ArgumentChecking</c> tests, the code-generated stratum.
/// </para>
/// </remarks>
public abstract class ZipTests : SharedReactiveTest
{
    [TestMethod]
    public void Zip_Never2()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, (_0, _1) => 42)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never2Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never3()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, (_0, _1, _2) => 42)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never3Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never4()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never4Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never5()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => 42)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never5Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never6()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => 42)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never6Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never7()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => 42)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never7Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never8()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => 42)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never8Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never9()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => 42)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never9Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never10()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => 42)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never10Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never11()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => 42)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never11Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never12()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => 42)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never12Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never13()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => 42)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never13Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never14()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e13 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(
            () =>
                Seq.Zip(
                    e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13,
                    (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => 42)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never14Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e13 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never15()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e13 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e14 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => 42)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never15Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e13 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e14 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never16()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e13 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e14 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e15 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => 42)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[]
            { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_Never16Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e13 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e14 = scheduler.CreateHotObservable([OnNext(150, 1)]);
        var e15 = scheduler.CreateHotObservable([OnNext(150, 1)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15)
        );

        res.Messages.AssertEqual(
        );

        foreach (var e in new[]
            { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
    }

    [TestMethod]
    public void Zip_NeverEmpty()
    {
        var scheduler = Scheduler;

        var n = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(210)
        );

        var res = scheduler.Start(() =>
            n.Zip(e, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
        );

        n.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Zip_EmptyNever()
    {
        var scheduler = Scheduler;

        var n = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(210)
        );

        var res = scheduler.Start(() =>
            e.Zip(n, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
        );

        n.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Zip_EmptyEmpty()
    {
        var scheduler = Scheduler;

        var e1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(210)
        );

        var e2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(210)
        );

        var res = scheduler.Start(() =>
            e1.Zip(e2, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(210)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );

        e2.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Zip_Empty2()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, (_0, _1) => 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(220)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty2Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1)
        );

        res.Messages.AssertEqual(
            OnCompleted<(int, int)>(220)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty3()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, (_0, _1, _2) => 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(230)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty3Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2)
        );

        res.Messages.AssertEqual(
            OnCompleted<(int, int, int)>(230)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty4()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(240)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty4Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3)
        );

        res.Messages.AssertEqual(
            OnCompleted<(int, int, int, int)>(240)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty5()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty5Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4)
        );

        res.Messages.AssertEqual(
            OnCompleted<(int, int, int, int, int)>(250)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty6()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(260)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty6Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5)
        );

        res.Messages.AssertEqual(
            OnCompleted<(int, int, int, int, int, int)>(260)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty7()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(270)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty7Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6)
        );

        res.Messages.AssertEqual(
            OnCompleted<(int, int, int, int, int, int, int)>(270)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty8()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(280)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty8Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7)
        );

        res.Messages.AssertEqual(
            OnCompleted<(int, int, int, int, int, int, int, int)>(280)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty9()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(290)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty9Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8)
        );

        res.Messages.AssertEqual(
            OnCompleted<(int, int, int, int, int, int, int, int, int)>(290)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty10()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(300)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty10Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9)
        );

        res.Messages.AssertEqual(
            OnCompleted<(int, int, int, int, int, int, int, int, int, int)>(300)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty11()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(310)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty11Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10)
        );

        res.Messages.AssertEqual(
            OnCompleted<(int, int, int, int, int, int, int, int, int, int, int)>(310)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty12()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(320)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty12Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11)
        );

        res.Messages.AssertEqual(
            OnCompleted<(int, int, int, int, int, int, int, int, int, int, int, int)>(320)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty13()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(330)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty13Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12)
        );

        res.Messages.AssertEqual(
            OnCompleted<(int, int, int, int, int, int, int, int, int, int, int, int, int)>(330)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty14()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
        var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(340)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty14Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
        var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13)
        );

        res.Messages.AssertEqual(
            OnCompleted<(int, int, int, int, int, int, int, int, int, int, int, int, int, int)>(340)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty15()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
        var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
        var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(350)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty15Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
        var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
        var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14)
        );

        res.Messages.AssertEqual(
            OnCompleted<(
                int, int, int, int, int, int, int, int, int, int, int, int, int, int, int
                )>(350)
        );

        var i = 0;
        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty16()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
        var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
        var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);
        var e15 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(360)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(360)
        );

        var i = 0;
        foreach (var e in new[]
            { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_Empty16Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
        var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
        var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
        var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
        var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
        var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
        var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
        var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
        var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
        var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
        var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
        var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
        var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
        var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);
        var e15 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(360)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15)
        );

        res.Messages.AssertEqual(
            OnCompleted<(
                int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int
                )>(360)
        );

        var i = 0;
        foreach (var e in new[]
            { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
    }

    [TestMethod]
    public void Zip_EmptyNonEmpty()
    {
        var scheduler = Scheduler;

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(210)
        );

        var o = scheduler.CreateHotObservable(
            OnNext(150, 1),
            // Intended behavior - will only know here there was no error and we can complete
            // gracefully
            OnNext(215, 2),
            OnCompleted<int>(220)
        );

        var res = scheduler.Start(() =>
            e.Zip(o, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(215)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );

        o.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );
    }

    [TestMethod]
    public void Zip_NonEmptyEmpty()
    {
        var scheduler = Scheduler;

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(210)
        );

        var o = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 2),
            OnCompleted<int>(220)
        );

        var res = scheduler.Start(() =>
            o.Zip(e, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(215)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );

        o.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );
    }

    [TestMethod]
    public void Zip_NeverNonEmpty()
    {
        var scheduler = Scheduler;

        var o = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 2),
            OnCompleted<int>(220)
        );

        var n = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            n.Zip(o, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
        );

        o.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        n.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Zip_NonEmptyNever()
    {
        var scheduler = Scheduler;

        var o = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 2),
            OnCompleted<int>(220)
        );

        var n = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            o.Zip(n, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
        );

        o.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        n.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Zip_NonEmptyNonEmpty()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 2),
            OnCompleted<int>(230)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 3),
            // Intended behavior - will only know here there was no error and we can complete
            // gracefully
            OnCompleted<int>(240)
        );

        var res = scheduler.Start(() =>
            o1.Zip(o2, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnNext(220, 2 + 3),
            OnCompleted<int>(240)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void Zip_EmptyError()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(230)
        );

        var f = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(220, ex)
        );

        var res = scheduler.Start(() =>
            e.Zip(f, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        f.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Zip_ErrorEmpty()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(230)
        );

        var f = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(220, ex)
        );

        var res = scheduler.Start(() =>
            f.Zip(e, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        f.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Zip_NeverError()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var n = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var f = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(220, ex)
        );

        var res = scheduler.Start(() =>
            n.Zip(f, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        n.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        f.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Zip_ErrorNever()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var n = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var f = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(220, ex)
        );

        var res = scheduler.Start(() =>
            f.Zip(n, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        n.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        f.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Zip_ErrorError()
    {
        var scheduler = Scheduler;

        var ex1 = new Exception();
        var ex2 = new Exception();

        var f1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(230, ex1)
        );

        var f2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(220, ex2)
        );

        var res = scheduler.Start(() =>
            f1.Zip(f2, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex2)
        );

        f1.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        f2.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Zip_SomeError()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 2),
            OnCompleted<int>(230)
        );

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(220, ex)
        );

        var res = scheduler.Start(() =>
            o.Zip(e, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        o.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Zip_ErrorSome()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 2),
            OnCompleted<int>(230)
        );

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(220, ex)
        );

        var res = scheduler.Start(() =>
            e.Zip(o, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        o.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Zip_LeftCompletesFirst()
    {
        var scheduler = Scheduler;

        var o = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(220)
        );

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 4),
            OnCompleted<int>(225)
        );

        var res = scheduler.Start(() =>
            o.Zip(e, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnNext(215, 6),
            OnCompleted<int>(225)
        );

        o.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void Zip_RightCompletesFirst()
    {
        var scheduler = Scheduler;

        var o = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 4),
            OnCompleted<int>(225)
        );

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(220)
        );

        var res = scheduler.Start(() =>
            o.Zip(e, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnNext(215, 6),
            OnCompleted<int>(225)
        );

        o.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Zip_LeftTriggersSelectorError()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2)
        );

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 4)
        );

        var res = scheduler.Start(() =>
            o.Zip(e, (x, y) => { if (x == y) { return 42; } throw ex; })
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        o.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Zip_RightTriggersSelectorError()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2)
        );

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 4)
        );

        var res = scheduler.Start(() =>
            o.Zip(e, (x, y) => { if (x == y) { return 42; } throw ex; })
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        o.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Zip_SymmetricReturn2()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, (_0, _1) => _0 + _1)
        );

        res.Messages.AssertEqual(
            OnNext(220, 3),
            OnCompleted<int>(400)
        );

        foreach (var e in new[] { e0, e1 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn2Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1)
        );

        res.Messages.AssertEqual(
            OnNext(220, (1, 2)),
            OnCompleted<(int First, int Second)>(400)
        );

        foreach (var e in new[] { e0, e1 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn3()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, (_0, _1, _2) => _0 + _1 + _2)
        );

        res.Messages.AssertEqual(
            OnNext(230, 6),
            OnCompleted<int>(400)
        );

        foreach (var e in new[] { e0, e1, e2 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn3Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2)
        );

        res.Messages.AssertEqual(
            OnNext(230, (1, 2, 3)),
            OnCompleted<(int First, int Second, int Third)>(400)
        );

        foreach (var e in new[] { e0, e1, e2 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn4()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => _0 + _1 + _2 + _3)
        );

        res.Messages.AssertEqual(
            OnNext(240, 10),
            OnCompleted<int>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn4Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3)
        );

        res.Messages.AssertEqual(
            OnNext(240, (1, 2, 3, 4)),
            OnCompleted<(int First, int Second, int Third, int Fourth)>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn5()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4)
        );

        res.Messages.AssertEqual(
            OnNext(250, 15),
            OnCompleted<int>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn5Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4)
        );

        res.Messages.AssertEqual(
            OnNext(250, (1, 2, 3, 4, 5)),
            OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth)>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn6()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5)
        );

        res.Messages.AssertEqual(
            OnNext(260, 21),
            OnCompleted<int>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn6Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5)
        );

        res.Messages.AssertEqual(
            OnNext(260, (1, 2, 3, 4, 5, 6)),
            OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth, int Sixth)>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn7()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6,
                (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6)
        );

        res.Messages.AssertEqual(
            OnNext(270, 28),
            OnCompleted<int>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn7Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6)
        );

        res.Messages.AssertEqual(
            OnNext(270, (1, 2, 3, 4, 5, 6, 7)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh
                )>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn8()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7,
                (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7)
        );

        res.Messages.AssertEqual(
            OnNext(280, 36),
            OnCompleted<int>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn8Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7)
        );

        res.Messages.AssertEqual(
            OnNext(280, (1, 2, 3, 4, 5, 6, 7, 8)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth
                )>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn9()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8)
        );

        res.Messages.AssertEqual(
            OnNext(290, 45),
            OnCompleted<int>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn9Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8)
        );

        res.Messages.AssertEqual(
            OnNext(290, (1, 2, 3, 4, 5, 6, 7, 8, 9)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth
                )>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn10()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) =>
                    _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9)
        );

        res.Messages.AssertEqual(
            OnNext(300, 55),
            OnCompleted<int>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn10Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9)
        );

        res.Messages.AssertEqual(
            OnNext(300, (1, 2, 3, 4, 5, 6, 7, 8, 9, 10)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth, int Tenth
                )>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn11()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) =>
                    _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10)
        );

        res.Messages.AssertEqual(
            OnNext(310, 66),
            OnCompleted<int>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn11Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10)
        );

        res.Messages.AssertEqual(
            OnNext(310, (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth, int Tenth, int Eleventh
                )>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn12()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
        var e11 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) =>
                    _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11)
        );

        res.Messages.AssertEqual(
            OnNext(320, 78),
            OnCompleted<int>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn12Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
        var e11 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11)
        );

        res.Messages.AssertEqual(
            OnNext(320, (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth, int Tenth, int Eleventh, int Twelfth
                )>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn13()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
        var e11 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
        var e12 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) =>
                    _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12)
        );

        res.Messages.AssertEqual(
            OnNext(330, 91),
            OnCompleted<int>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn13Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
        var e11 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
        var e12 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12)
        );

        res.Messages.AssertEqual(
            OnNext(330, (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth, int Tenth, int Eleventh, int Twelfth, int Thirteenth
                )>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn14()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
        var e11 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
        var e12 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);
        var e13 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) =>
                    _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13)
        );

        res.Messages.AssertEqual(
            OnNext(340, 105),
            OnCompleted<int>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn14Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
        var e11 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
        var e12 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);
        var e13 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13)
        );

        res.Messages.AssertEqual(
            OnNext(340, (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth, int Tenth, int Eleventh, int Twelfth, int Thirteenth,
                int Fourteenth
                )>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn15()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
        var e11 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
        var e12 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);
        var e13 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400)]);
        var e14 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) =>
                    _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14)
        );

        res.Messages.AssertEqual(
            OnNext(350, 120),
            OnCompleted<int>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn15Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
        var e11 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
        var e12 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);
        var e13 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400)]);
        var e14 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14)
        );

        res.Messages.AssertEqual(
            OnNext(350, (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth, int Tenth, int Eleventh, int Twelfth, int Thirteenth,
                int Fourteenth, int Fifteenth
                )>(400)
        );

        foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn16()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
        var e11 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
        var e12 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);
        var e13 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400)]);
        var e14 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400)]);
        var e15 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(360, 16), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) =>
                    _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14
                    + _15)
        );

        res.Messages.AssertEqual(
            OnNext(360, 136),
            OnCompleted<int>(400)
        );

        foreach (var e in new[]
            { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SymmetricReturn16Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
        var e11 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
        var e12 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);
        var e13 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400)]);
        var e14 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400)]);
        var e15 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(360, 16), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15)
        );

        res.Messages.AssertEqual(
            OnNext(360, (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth, int Tenth, int Eleventh, int Twelfth, int Thirteenth,
                int Fourteenth, int Fifteenth, int Sixteenth
                )>(400)
        );

        foreach (var e in new[]
            { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
    }

    [TestMethod]
    public void Zip_SomeDataAsymmetric1()
    {
        var scheduler = Scheduler;

        var msgs1 = Enumerable.Range(0, 5)
            .Select((x, i) => OnNext((ushort)(205 + i * 5), x))
            .ToArray();
        var msgs2 = Enumerable.Range(0, 10)
            .Select((x, i) => OnNext((ushort)(202 + i * 8), x))
            .ToArray();

        var len = Math.Min(msgs1.Length, msgs2.Length);

        var o1 = scheduler.CreateHotObservable(msgs1);
        var o2 = scheduler.CreateHotObservable(msgs2);

        var res = scheduler.Start(() =>
            o1.Zip(o2, (x, y) => x + y)
        );

        // The original reads the recorded messages back one by one; the shared log is asserted
        // over as a whole, with the same expectation: one pair per index, at the later of the
        // two times, carrying the sum, and nothing after.
        res.Messages.AssertEqual(
            Enumerable.Range(0, len).Select(i =>
                OnNext(
                    Math.Max(msgs1[i].Time, msgs2[i].Time),
                    msgs1[i].Value.Value + msgs2[i].Value.Value)));
    }

    [TestMethod]
    public void Zip_SomeDataAsymmetric2()
    {
        var scheduler = Scheduler;

        var msgs1 = Enumerable.Range(0, 10)
            .Select((x, i) => OnNext((ushort)(205 + i * 5), x))
            .ToArray();
        var msgs2 = Enumerable.Range(0, 5)
            .Select((x, i) => OnNext((ushort)(202 + i * 8), x))
            .ToArray();

        var len = Math.Min(msgs1.Length, msgs2.Length);

        var o1 = scheduler.CreateHotObservable(msgs1);
        var o2 = scheduler.CreateHotObservable(msgs2);

        var res = scheduler.Start(() =>
            o1.Zip(o2, (x, y) => x + y)
        );

        // The original reads the recorded messages back one by one; the shared log is asserted
        // over as a whole, with the same expectation: one pair per index, at the later of the
        // two times, carrying the sum, and nothing after.
        res.Messages.AssertEqual(
            Enumerable.Range(0, len).Select(i =>
                OnNext(
                    Math.Max(msgs1[i].Time, msgs2[i].Time),
                    msgs1[i].Value.Value + msgs2[i].Value.Value)));
    }

    [TestMethod]
    public void Zip_SomeDataSymmetric()
    {
        var scheduler = Scheduler;

        var msgs1 = Enumerable.Range(0, 10)
            .Select((x, i) => OnNext((ushort)(205 + i * 5), x))
            .ToArray();
        var msgs2 = Enumerable.Range(0, 10)
            .Select((x, i) => OnNext((ushort)(202 + i * 8), x))
            .ToArray();

        var len = Math.Min(msgs1.Length, msgs2.Length);

        var o1 = scheduler.CreateHotObservable(msgs1);
        var o2 = scheduler.CreateHotObservable(msgs2);

        var res = scheduler.Start(() =>
            o1.Zip(o2, (x, y) => x + y)
        );

        // The original reads the recorded messages back one by one; the shared log is asserted
        // over as a whole, with the same expectation: one pair per index, at the later of the
        // two times, carrying the sum, and nothing after.
        res.Messages.AssertEqual(
            Enumerable.Range(0, len).Select(i =>
                OnNext(
                    Math.Max(msgs1[i].Time, msgs2[i].Time),
                    msgs1[i].Value.Value + msgs2[i].Value.Value)));
    }

    [TestMethod]
    public void Zip_SelectorThrows()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 2),
            OnNext(225, 4),
            OnCompleted<int>(240)
        );

        var o2 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 3),
            OnNext(230, 5), //!
            OnCompleted<int>(250)
        );

        var ex = new Exception();

        var res = scheduler.Start(() =>
            o1.Zip(o2, (x, y) =>
            {
                if (y == 5)
                {
                    throw ex;
                }

                return x + y;
            })
        );

        res.Messages.AssertEqual(
            OnNext(220, 2 + 3),
            OnError<int>(230, ex)
        );
    }

    [TestMethod]
    public void Zip_SelectorThrows2()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);

        var ex = new Exception();
        Func<int> f = () => { throw ex; };

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, (_0, _1) => f())
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        var es = new[] { e0, e1 };
        foreach (var e in es)
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
    }

    [TestMethod]
    public void Zip_SelectorThrows3()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);

        var ex = new Exception();
        Func<int> f = () => { throw ex; };

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, (_0, _1, _2) => f())
        );

        res.Messages.AssertEqual(
            OnError<int>(230, ex)
        );

        var es = new[] { e0, e1, e2 };
        foreach (var e in es)
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
    }

    [TestMethod]
    public void Zip_SelectorThrows4()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);

        var ex = new Exception();
        Func<int> f = () => { throw ex; };

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => f())
        );

        res.Messages.AssertEqual(
            OnError<int>(240, ex)
        );

        var es = new[] { e0, e1, e2, e3 };
        foreach (var e in es)
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
    }

    [TestMethod]
    public void Zip_SelectorThrows5()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);

        var ex = new Exception();
        Func<int> f = () => { throw ex; };

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => f())
        );

        res.Messages.AssertEqual(
            OnError<int>(250, ex)
        );

        var es = new[] { e0, e1, e2, e3, e4 };
        foreach (var e in es)
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
    }

    [TestMethod]
    public void Zip_SelectorThrows6()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);

        var ex = new Exception();
        Func<int> f = () => { throw ex; };

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => f())
        );

        res.Messages.AssertEqual(
            OnError<int>(260, ex)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5 };
        foreach (var e in es)
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
    }

    [TestMethod]
    public void Zip_SelectorThrows7()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);

        var ex = new Exception();
        Func<int> f = () => { throw ex; };

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => f())
        );

        res.Messages.AssertEqual(
            OnError<int>(270, ex)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6 };
        foreach (var e in es)
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
    }

    [TestMethod]
    public void Zip_SelectorThrows8()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);

        var ex = new Exception();
        Func<int> f = () => { throw ex; };

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => f())
        );

        res.Messages.AssertEqual(
            OnError<int>(280, ex)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7 };
        foreach (var e in es)
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
    }

    [TestMethod]
    public void Zip_SelectorThrows9()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);

        var ex = new Exception();
        Func<int> f = () => { throw ex; };

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => f())
        );

        res.Messages.AssertEqual(
            OnError<int>(290, ex)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 };
        foreach (var e in es)
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
    }

    [TestMethod]
    public void Zip_SelectorThrows10()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);

        var ex = new Exception();
        Func<int> f = () => { throw ex; };

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => f())
        );

        res.Messages.AssertEqual(
            OnError<int>(300, ex)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 };
        foreach (var e in es)
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
    }

    [TestMethod]
    public void Zip_SelectorThrows11()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);

        var ex = new Exception();
        Func<int> f = () => { throw ex; };

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => f())
        );

        res.Messages.AssertEqual(
            OnError<int>(310, ex)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };
        foreach (var e in es)
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
    }

    [TestMethod]
    public void Zip_SelectorThrows12()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
        var e11 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);

        var ex = new Exception();
        Func<int> f = () => { throw ex; };

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => f())
        );

        res.Messages.AssertEqual(
            OnError<int>(320, ex)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 };
        foreach (var e in es)
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
    }

    [TestMethod]
    public void Zip_SelectorThrows13()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
        var e11 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
        var e12 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);

        var ex = new Exception();
        Func<int> f = () => { throw ex; };

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => f())
        );

        res.Messages.AssertEqual(
            OnError<int>(330, ex)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 };
        foreach (var e in es)
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
    }

    [TestMethod]
    public void Zip_SelectorThrows14()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
        var e11 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
        var e12 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);
        var e13 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400)]);

        var ex = new Exception();
        Func<int> f = () => { throw ex; };

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => f())
        );

        res.Messages.AssertEqual(
            OnError<int>(340, ex)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 };
        foreach (var e in es)
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
    }

    [TestMethod]
    public void Zip_SelectorThrows15()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
        var e11 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
        var e12 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);
        var e13 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400)]);
        var e14 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400)]);

        var ex = new Exception();
        Func<int> f = () => { throw ex; };

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => f())
        );

        res.Messages.AssertEqual(
            OnError<int>(350, ex)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 };
        foreach (var e in es)
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
    }

    [TestMethod]
    public void Zip_SelectorThrows16()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
        var e4 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
        var e5 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
        var e6 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
        var e7 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
        var e8 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
        var e9 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
        var e10 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
        var e11 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
        var e12 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);
        var e13 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400)]);
        var e14 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400)]);
        var e15 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(360, 16), OnCompleted<int>(400)]);

        var ex = new Exception();
        Func<int> f = () => { throw ex; };

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => f())
        );

        res.Messages.AssertEqual(
            OnError<int>(360, ex)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 };
        foreach (var e in es)
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
    }

    [TestMethod]
    public void Zip_GetEnumeratorThrows()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 42),
            OnNext(220, 43),
            OnCompleted<int>(230)
        );

        var ys = new RogueEnumerable<int>(ex);

        var res = scheduler.Start(() =>
            xs.Zip(ys, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );

        xs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Zip_AllCompleted2()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, (_0, _1) => _0 + _1)
        );

        res.Messages.AssertEqual(
            OnNext(210, 10),
            OnCompleted<int>(220)
        );

        var es = new[] { e0, e1 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted2Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1)
        );

        res.Messages.AssertEqual(
            OnNext(210, (5, 5)),
            OnCompleted<(int First, int Second)>(220)
        );

        var es = new[] { e0, e1 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted3()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, (_0, _1, _2) => _0 + _1 + _2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 15),
            OnCompleted<int>(230)
        );

        var es = new[] { e0, e1, e2 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted3Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2)
        );

        res.Messages.AssertEqual(
            OnNext(210, (5, 5, 5)),
            OnCompleted<(int First, int Second, int Third)>(230)
        );

        var es = new[] { e0, e1, e2 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted4()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => _0 + _1 + _2 + _3)
        );

        res.Messages.AssertEqual(
            OnNext(210, 20),
            OnCompleted<int>(240)
        );

        var es = new[] { e0, e1, e2, e3 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted4Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3)
        );

        res.Messages.AssertEqual(
            OnNext(210, (5, 5, 5, 5)),
            OnCompleted<(int First, int Second, int Third, int Fourth)>(240)
        );

        var es = new[] { e0, e1, e2, e3 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted5()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4)
        );

        res.Messages.AssertEqual(
            OnNext(210, 25),
            OnCompleted<int>(250)
        );

        var es = new[] { e0, e1, e2, e3, e4 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted5Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4)
        );

        res.Messages.AssertEqual(
            OnNext(210, (5, 5, 5, 5, 5)),
            OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth)>(250)
        );

        var es = new[] { e0, e1, e2, e3, e4 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted6()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5)
        );

        res.Messages.AssertEqual(
            OnNext(210, 30),
            OnCompleted<int>(260)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted6Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5)
        );

        res.Messages.AssertEqual(
            OnNext(210, (5, 5, 5, 5, 5, 5)),
            OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth, int Sixth)>(260)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted7()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6,
                (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6)
        );

        res.Messages.AssertEqual(
            OnNext(210, 35),
            OnCompleted<int>(270)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted7Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6)
        );

        res.Messages.AssertEqual(
            OnNext(210, (5, 5, 5, 5, 5, 5, 5)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh
                )>(270)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted8()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7,
                (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7)
        );

        res.Messages.AssertEqual(
            OnNext(210, 40),
            OnCompleted<int>(280)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted8Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7)
        );

        res.Messages.AssertEqual(
            OnNext(210, (5, 5, 5, 5, 5, 5, 5, 5)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth
                )>(280)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted9()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8)
        );

        res.Messages.AssertEqual(
            OnNext(210, 45),
            OnCompleted<int>(290)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted9Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8)
        );

        res.Messages.AssertEqual(
            OnNext(210, (5, 5, 5, 5, 5, 5, 5, 5, 5)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth
                )>(290)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted10()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);
        var e9 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnCompleted<int>(310)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) =>
                    _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9)
        );

        res.Messages.AssertEqual(
            OnNext(210, 50),
            OnCompleted<int>(300)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted10Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);
        var e9 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnCompleted<int>(310)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9)
        );

        res.Messages.AssertEqual(
            OnNext(210, (5, 5, 5, 5, 5, 5, 5, 5, 5, 5)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth, int Tenth
                )>(300)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted11()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);
        var e9 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnCompleted<int>(310)]);
        var e10 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnCompleted<int>(320)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) =>
                    _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10)
        );

        res.Messages.AssertEqual(
            OnNext(210, 55),
            OnCompleted<int>(310)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted11Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);
        var e9 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnCompleted<int>(310)]);
        var e10 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnCompleted<int>(320)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10)
        );

        res.Messages.AssertEqual(
            OnNext(210, (5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth, int Tenth, int Eleventh
                )>(310)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted12()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);
        var e9 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnCompleted<int>(310)]);
        var e10 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnCompleted<int>(320)]);
        var e11 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnCompleted<int>(330)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) =>
                    _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11)
        );

        res.Messages.AssertEqual(
            OnNext(210, 60),
            OnCompleted<int>(320)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted12Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);
        var e9 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnCompleted<int>(310)]);
        var e10 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnCompleted<int>(320)]);
        var e11 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnCompleted<int>(330)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11)
        );

        res.Messages.AssertEqual(
            OnNext(210, (5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth, int Tenth, int Eleventh, int Twelfth
                )>(320)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted13()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);
        var e9 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnCompleted<int>(310)]);
        var e10 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnCompleted<int>(320)]);
        var e11 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnCompleted<int>(330)]);
        var e12 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnCompleted<int>(340)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) =>
                    _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12)
        );

        res.Messages.AssertEqual(
            OnNext(210, 65),
            OnCompleted<int>(330)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted13Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);
        var e9 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnCompleted<int>(310)]);
        var e10 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnCompleted<int>(320)]);
        var e11 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnCompleted<int>(330)]);
        var e12 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnCompleted<int>(340)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12)
        );

        res.Messages.AssertEqual(
            OnNext(210, (5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth, int Tenth, int Eleventh, int Twelfth, int Thirteenth
                )>(330)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted14()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);
        var e9 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnCompleted<int>(310)]);
        var e10 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnCompleted<int>(320)]);
        var e11 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnCompleted<int>(330)]);
        var e12 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnCompleted<int>(340)]);
        var e13 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnNext(340, 18),
                OnCompleted<int>(350)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) =>
                    _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13)
        );

        res.Messages.AssertEqual(
            OnNext(210, 70),
            OnCompleted<int>(340)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted14Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);
        var e9 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnCompleted<int>(310)]);
        var e10 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnCompleted<int>(320)]);
        var e11 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnCompleted<int>(330)]);
        var e12 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnCompleted<int>(340)]);
        var e13 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnNext(340, 18),
                OnCompleted<int>(350)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13)
        );

        res.Messages.AssertEqual(
            OnNext(210, (5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth, int Tenth, int Eleventh, int Twelfth, int Thirteenth,
                int Fourteenth
                )>(340)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted15()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);
        var e9 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnCompleted<int>(310)]);
        var e10 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnCompleted<int>(320)]);
        var e11 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnCompleted<int>(330)]);
        var e12 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnCompleted<int>(340)]);
        var e13 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnNext(340, 18),
                OnCompleted<int>(350)]);
        var e14 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnNext(340, 18),
                OnNext(350, 19),
                OnCompleted<int>(360)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) =>
                    _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14)
        );

        res.Messages.AssertEqual(
            OnNext(210, 75),
            OnCompleted<int>(350)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted15Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);
        var e9 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnCompleted<int>(310)]);
        var e10 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnCompleted<int>(320)]);
        var e11 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnCompleted<int>(330)]);
        var e12 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnCompleted<int>(340)]);
        var e13 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnNext(340, 18),
                OnCompleted<int>(350)]);
        var e14 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnNext(340, 18),
                OnNext(350, 19),
                OnCompleted<int>(360)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14)
        );

        res.Messages.AssertEqual(
            OnNext(210, (5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth, int Tenth, int Eleventh, int Twelfth, int Thirteenth,
                int Fourteenth, int Fifteenth
                )>(350)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted16()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);
        var e9 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnCompleted<int>(310)]);
        var e10 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnCompleted<int>(320)]);
        var e11 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnCompleted<int>(330)]);
        var e12 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnCompleted<int>(340)]);
        var e13 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnNext(340, 18),
                OnCompleted<int>(350)]);
        var e14 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnNext(340, 18),
                OnNext(350, 19),
                OnCompleted<int>(360)]);
        var e15 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnNext(340, 18),
                OnNext(350, 19),
                OnNext(360, 20),
                OnCompleted<int>(370)]);

        var res = scheduler.Start(() =>
            Seq.Zip(
                e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15,
                (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) =>
                    _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14
                    + _15)
        );

        res.Messages.AssertEqual(
            OnNext(210, 80),
            OnCompleted<int>(360)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void Zip_AllCompleted16Tuple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnCompleted<int>(240)]);
        var e3 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnCompleted<int>(250)]);
        var e4 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnCompleted<int>(260)]);
        var e5 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnCompleted<int>(270)]);
        var e6 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnCompleted<int>(280)]);
        var e7 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnCompleted<int>(290)]);
        var e8 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnCompleted<int>(300)]);
        var e9 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnCompleted<int>(310)]);
        var e10 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnCompleted<int>(320)]);
        var e11 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnCompleted<int>(330)]);
        var e12 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnCompleted<int>(340)]);
        var e13 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnNext(340, 18),
                OnCompleted<int>(350)]);
        var e14 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnNext(340, 18),
                OnNext(350, 19),
                OnCompleted<int>(360)]);
        var e15 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(210, 5),
                OnNext(220, 6),
                OnNext(230, 7),
                OnNext(240, 8),
                OnNext(250, 9),
                OnNext(260, 10),
                OnNext(270, 11),
                OnNext(280, 12),
                OnNext(290, 13),
                OnNext(300, 14),
                OnNext(310, 15),
                OnNext(320, 16),
                OnNext(330, 17),
                OnNext(340, 18),
                OnNext(350, 19),
                OnNext(360, 20),
                OnCompleted<int>(370)]);

        var res = scheduler.Start(() =>
            SeqEx.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15)
        );

        res.Messages.AssertEqual(
            OnNext(210, (5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5)),
            OnCompleted<(
                int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh,
                int Eighth, int Ninth, int Tenth, int Eleventh, int Twelfth, int Thirteenth,
                int Fourteenth, int Fifteenth, int Sixteenth
                )>(360)
        );

        var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 };

        var i = 0;
        foreach (var e in es.Take(es.Length - 1))
        {
            e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
        }

        es.Last().Subscriptions.AssertEqual(
            Subscribe(200, 220 + (i - 1) * 10)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_NeverNever()
    {
        var evt = new ManualResetEvent(false);
        var scheduler = Scheduler;

        var n1 = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var n2 = new MockEnumerable<int>(scheduler,
            EnumerableNever(evt)
        );

        var res = scheduler.Start(() =>
            n1.Zip(n2, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
        );

        n1.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        n2.Enumerations.AssertEqual(
            new Enumeration(200, 1000)
        );

        evt.Set();
    }

    [TestMethod]
    public void ZipWithEnumerable_NeverEmpty()
    {
        var scheduler = Scheduler;

        var n = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var e = new MockEnumerable<int>(scheduler,
            []
        );

        var res = scheduler.Start(() =>
            n.Zip(e, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
        );

        n.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        e.Enumerations.AssertEqual(
            new Enumeration(200, 1000)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_EmptyNever()
    {
        var evt = new ManualResetEvent(false);

        var scheduler = Scheduler;

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(210)
        );

        var n = new MockEnumerable<int>(scheduler,
            EnumerableNever(evt)
        );

        var res = scheduler.Start(() =>
            e.Zip(n, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(210)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );

        n.Enumerations.AssertEqual(
            new Enumeration(200, 210)
        );

        evt.Set();
    }

    [TestMethod]
    public void ZipWithEnumerable_EmptyEmpty()
    {
        var scheduler = Scheduler;

        var e1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(210)
        );

        var e2 = new MockEnumerable<int>(scheduler,
            []
        );

        var res = scheduler.Start(() =>
            e1.Zip(e2, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(210)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );

        e2.Enumerations.AssertEqual(
            new Enumeration(200, 210)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_EmptyNonEmpty()
    {
        var scheduler = Scheduler;

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(210)
        );

        var o = new MockEnumerable<int>(scheduler,
            [2]
        );

        var res = scheduler.Start(() =>
            e.Zip(o, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(210)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );

        o.Enumerations.AssertEqual(
            new Enumeration(200, 210)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_NonEmptyEmpty()
    {
        var scheduler = Scheduler;

        var e = new MockEnumerable<int>(scheduler,
            []
        );

        var o = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 2),
            OnCompleted<int>(220)
        );

        var res = scheduler.Start(() =>
            o.Zip(e, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(215)
        );

        e.Enumerations.AssertEqual(
            new Enumeration(200, 215)
        );

        o.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_NeverNonEmpty()
    {
        var scheduler = Scheduler;

        var o = new MockEnumerable<int>(scheduler,
            [2]
        );

        var n = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            n.Zip(o, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
        );

        o.Enumerations.AssertEqual(
            new Enumeration(200, 1000)
        );

        n.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_NonEmptyNonEmpty()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 2),
            OnCompleted<int>(230)
        );

        var o2 = new MockEnumerable<int>(scheduler,
            [3]
        );

        var res = scheduler.Start(() =>
            o1.Zip(o2, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnNext(215, 2 + 3),
            OnCompleted<int>(230)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Enumerations.AssertEqual(
            new Enumeration(200, 230)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_EmptyError()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(230)
        );

        var f = new MockEnumerable<int>(scheduler,
            ThrowEnumerable(false, ex)
        );

        var res = scheduler.Start(() =>
            e.Zip(f, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(230)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        f.Enumerations.AssertEqual(
            new Enumeration(200, 230)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_ErrorEmpty()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var e = new MockEnumerable<int>(scheduler,
            []
        );

        var f = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(220, ex)
        );

        var res = scheduler.Start(() =>
            f.Zip(e, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        e.Enumerations.AssertEqual(
            new Enumeration(200, 220)
        );

        f.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_NeverError()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var n = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var f = new MockEnumerable<int>(scheduler,
            ThrowEnumerable(false, ex)
        );

        var res = scheduler.Start(() =>
            n.Zip(f, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
        );

        n.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        f.Enumerations.AssertEqual(
            new Enumeration(200, 1000)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_ErrorNever()
    {
        var evt = new ManualResetEvent(false);

        var scheduler = Scheduler;

        var ex = new Exception();

        var n = new MockEnumerable<int>(scheduler,
            EnumerableNever(evt)
        );

        var f = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(220, ex)
        );

        var res = scheduler.Start(() =>
            f.Zip(n, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        n.Enumerations.AssertEqual(
            new Enumeration(200, 220)
        );

        f.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        evt.Set();
    }

    [TestMethod]
    public void ZipWithEnumerable_ErrorError()
    {
        var scheduler = Scheduler;

        var ex1 = new Exception();
        var ex2 = new Exception();

        var f1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(230, ex1)
        );

        var f2 = new MockEnumerable<int>(scheduler,
            ThrowEnumerable(false, ex2)
        );

        var res = scheduler.Start(() =>
            f1.Zip(f2, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(230, ex1)
        );

        f1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        f2.Enumerations.AssertEqual(
            new Enumeration(200, 230)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_SomeError()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 2),
            OnCompleted<int>(230)
        );

        var e = new MockEnumerable<int>(scheduler,
            ThrowEnumerable(false, ex)
        );

        var res = scheduler.Start(() =>
            o.Zip(e, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(215, ex)
        );

        o.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );

        e.Enumerations.AssertEqual(
            new Enumeration(200, 215)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_ErrorSome()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o = new MockEnumerable<int>(scheduler,
            [2]
        );

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(220, ex)
        );

        var res = scheduler.Start(() =>
            e.Zip(o, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        o.Enumerations.AssertEqual(
            new Enumeration(200, 220)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_SomeDataBothSides()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var o = new MockEnumerable<int>(scheduler,
            [5, 4, 3, 2]
        );

        var e = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5)
        );

        var res = scheduler.Start(() =>
            e.Zip(o, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnNext(210, 7),
            OnNext(220, 7),
            OnNext(230, 7),
            OnNext(240, 7)
        );

        o.Enumerations.AssertEqual(
            new Enumeration(200, 1000)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_EnumeratorThrowsMoveNext()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 2),
            OnNext(225, 4),
            OnCompleted<int>(240)
        );

        var o2 = new MockEnumerable<int>(scheduler,
            new MyEnumerable(false, ex)
        );

        var res = scheduler.Start(() =>
            o1.Zip(o2, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(215, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );

        o2.Enumerations.AssertEqual(
            new Enumeration(200, 215)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_EnumeratorThrowsCurrent()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 2),
            OnNext(225, 4),
            OnCompleted<int>(240)
        );

        var o2 = new MockEnumerable<int>(scheduler,
            new MyEnumerable(true, ex)
        );

        var res = scheduler.Start(() =>
            o1.Zip(o2, (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(215, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );

        o2.Enumerations.AssertEqual(
            new Enumeration(200, 215)
        );
    }

    [TestMethod]
    public void ZipWithEnumerable_SelectorThrows()
    {
        var scheduler = Scheduler;

        var o1 = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 2),
            OnNext(225, 4),
            OnCompleted<int>(240)
        );

        var o2 = new MockEnumerable<int>(scheduler,
            [3, 5]
        );

        var ex = new Exception();

        var res = scheduler.Start(() =>
            o1.Zip(o2, (x, y) =>
            {
                if (y == 5)
                {
                    throw ex;
                }

                return x + y;
            })
        );

        res.Messages.AssertEqual(
            OnNext(215, 2 + 3),
            OnError<int>(225, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        o2.Enumerations.AssertEqual(
            new Enumeration(200, 225)
        );
    }

    [TestMethod]
    public async Task ZipWithEnumerable_NoAsyncDisposeOnMoveNext()
    {
        var source = CreateSubject<int>();
        IAsyncDisposable? subscription = null;
        var other = new MoveNextDisposeDetectEnumerable(() => subscription!, true);
        subscription = await source.Zip(other, (a, b) => a + b).SubscribeAsync(Scheduler, _ => { });
        await source.OnNextAsync(1);
        Assert.IsTrue(other.IsDisposed);
        Assert.IsFalse(other.DisposedWhileMoveNext);
        Assert.IsFalse(other.DisposedWhileCurrent);
    }

    [TestMethod]
    public async Task ZipWithEnumerable_NoAsyncDisposeOnCurrent()
    {
        var source = CreateSubject<int>();
        IAsyncDisposable? subscription = null;
        var other = new MoveNextDisposeDetectEnumerable(() => subscription!, false);
        subscription = await source.Zip(other, (a, b) => a + b).SubscribeAsync(Scheduler, _ => { });
        await source.OnNextAsync(1);
        Assert.IsTrue(other.IsDisposed);
        Assert.IsFalse(other.DisposedWhileMoveNext);
        Assert.IsFalse(other.DisposedWhileCurrent);
    }

    [TestMethod]
    public void Zip_NAry_Symmetric()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(420)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(230, l => l.SequenceEqual([1, 2, 3])),
            OnNext<IList<int>>(260, l => l.SequenceEqual([4, 5, 6])),
            OnCompleted<IList<int>>(420)
        );

        e0.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(200, 410)
        );

        e2.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Zip_NAry_Symmetric_Selector()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(420)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip([e0, e1, e2], xs => xs.Sum())
        );

        res.Messages.AssertEqual(
            OnNext(230, new[] { 1, 2, 3 }.Sum()),
            OnNext(260, new[] { 4, 5, 6 }.Sum()),
            OnCompleted<int>(420)
        );

        e0.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(200, 410)
        );

        e2.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Zip_NAry_Asymmetric()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(270)]);
        var e1 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(220, 2),
                OnNext(240, 5),
                OnNext(290, 7),
                OnNext(310, 9),
                OnCompleted<int>(410)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(230, 3),
                OnNext(260, 6),
                OnNext(280, 8),
                OnCompleted<int>(300)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(230, l => l.SequenceEqual([1, 2, 3])),
            OnNext<IList<int>>(260, l => l.SequenceEqual([4, 5, 6])),
            OnCompleted<IList<int>>(310)
        );

        e0.Subscriptions.AssertEqual(
            Subscribe(200, 270)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        e2.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void Zip_NAry_Asymmetric_Selector()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(270)]);
        var e1 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(220, 2),
                OnNext(240, 5),
                OnNext(290, 7),
                OnNext(310, 9),
                OnCompleted<int>(410)]);
        var e2 = scheduler.CreateHotObservable(
            [
                OnNext(150, 1),
                OnNext(230, 3),
                OnNext(260, 6),
                OnNext(280, 8),
                OnCompleted<int>(300)]);

        var res = scheduler.Start(() =>
            Seq.Zip([e0, e1, e2], xs => xs.Sum())
        );

        res.Messages.AssertEqual(
            OnNext(230, new[] { 1, 2, 3 }.Sum()),
            OnNext(260, new[] { 4, 5, 6 }.Sum()),
            OnCompleted<int>(310)
        );

        e0.Subscriptions.AssertEqual(
            Subscribe(200, 270)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        e2.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void Zip_NAry_Error()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnError<int>(250, ex)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(230, l => l.SequenceEqual([1, 2, 3])),
            OnError<IList<int>>(250, ex)
        );

        e0.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        e2.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Zip_NAry_Error_Selector()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnError<int>(250, ex)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip([e0, e1, e2], xs => xs.Sum())
        );

        res.Messages.AssertEqual(
            OnNext(230, new[] { 1, 2, 3 }.Sum()),
            OnError<int>(250, ex)
        );

        e0.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        e2.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Zip_NAry_Enumerable_Simple()
    {
        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(420)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410)]);
        var e2 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400)]);

        var started = default(long);
        var xss = GetSources(() => started = scheduler.Clock, e0, e1, e2)
            .Select(xs => (Seq<int>)xs);

        var res = scheduler.Start(() =>
            Seq.Zip(xss)
        );

        Assert.AreEqual(200, started);

        res.Messages.AssertEqual(
            OnNext<IList<int>>(230, l => l.SequenceEqual([1, 2, 3])),
            OnNext<IList<int>>(260, l => l.SequenceEqual([4, 5, 6])),
            OnCompleted<IList<int>>(420)
        );

        e0.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(200, 410)
        );

        e2.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public async Task Zip_NAry_Enumerable_Throws()
    {
        var ex = new Exception();
        var xss = GetSources(ex, Seq.Return(42));
        var res = Seq.Zip(xss);

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(
                async () => await res.SubscribeAsync(Scheduler, _ => { })));
    }

    [TestMethod]
    public void Zip_AtLeastOneThrows4()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var e0 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
        var e1 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
        var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnError<int>(230, ex)]);
        var e3 = scheduler.CreateHotObservable(
            [OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);

        var res = scheduler.Start(() =>
            Seq.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
        );

        res.Messages.AssertEqual(
            OnError<int>(230, ex)
        );

        e0.Subscriptions.AssertEqual(Subscribe(200, 230));
        e1.Subscriptions.AssertEqual(Subscribe(200, 230));
        e2.Subscriptions.AssertEqual(Subscribe(200, 230));
        e3.Subscriptions.AssertEqual(Subscribe(200, 230));
    }

    [TestMethod]
    public async Task Zip2WithImmediateReturn()
    {
        await Seq.Zip<Unit, Unit, Unit>(
            Seq.Return(Unit.Default), 
            Seq.Return(Unit.Default), 
            (_, __) => Unit.Default
        )
        .SubscribeAsync(Scheduler, _ => { });
    }

    [TestMethod]
    public async Task Zip3WithImmediateReturn()
    {
        var result = 0;

        await Seq.Zip<int, int, int, int>(
            Seq.Return(1),
            Seq.Return(2),
            Seq.Return(4),
            (a, b, c) => a + b + c
        )
        .SubscribeAsync(Scheduler, v => result = v);

        Assert.AreEqual(7, result);
    }

    [TestMethod]
    public async Task ZipEnumerableWithImmediateReturn()
    {
        await Enumerable.Range(0, 100)
            .Select(_ => Seq.Return(Unit.Default))
            .Zip()
            .SubscribeAsync(Scheduler, _ => { });
    }

    /// <summary>
    /// An enumerable that disposes the subscription it is given from inside <c>MoveNext</c> or
    /// <c>Current</c>, and records whether it was itself disposed while either was running.
    /// </summary>
    /// <param name="subscription">The subscription to dispose, assigned after subscribing.</param>
    /// <param name="disposeOnMoveNext">
    /// Whether to dispose from <c>MoveNext</c> (true) or from <c>Current</c> (false).
    /// </param>
    /// <remarks>
    /// Rx.NET's <c>MoveNextDisposeDetectEnumerable</c>, private to its <c>ZipTest</c>. The
    /// original holds a <c>SingleAssignmentDisposable</c>; here the subscription is reached
    /// through a function, since it exists only after the subscribe call that this enumerable
    /// takes part in. Disposal is awaited to completion, which on both targets is immediate.
    /// </remarks>
    private sealed class MoveNextDisposeDetectEnumerable(
        Func<IAsyncDisposable> subscription,
        bool disposeOnMoveNext) : IEnumerable<int>, IEnumerator<int>
    {
        private bool _moveNextRunning;
        private bool _currentRunning;

        public bool DisposedWhileMoveNext { get; private set; }

        public bool DisposedWhileCurrent { get; private set; }

        public bool IsDisposed { get; private set; }

        public int Current
        {
            get
            {
                _currentRunning = true;

                if (!disposeOnMoveNext)
                {
                    DisposeSubscription();
                }

                _currentRunning = false;
                return 0;
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            DisposedWhileMoveNext = _moveNextRunning;
            DisposedWhileCurrent = _currentRunning;
            IsDisposed = true;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            _moveNextRunning = true;

            if (disposeOnMoveNext)
            {
                DisposeSubscription();
            }

            _moveNextRunning = false;
            return true;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        private void DisposeSubscription()
        {
            var disposal = subscription().DisposeAsync();

            if (!disposal.IsCompleted)
            {
                disposal.AsTask().GetAwaiter().GetResult();
            }
        }
    }

    private IEnumerable<int> EnumerableNever(ManualResetEvent evt)
    {
        evt.WaitOne();
        yield break;
    }

    private IEnumerable<int> ThrowEnumerable(bool b, Exception ex)
    {
        if (!b)
        {
            throw ex;
        }

        yield break;
    }

    private class MyEnumerable : IEnumerable<int>
    {
        private readonly bool _throwInCurrent;
        private readonly Exception _ex;

        public MyEnumerable(bool throwInCurrent, Exception ex)
        {
            _throwInCurrent = throwInCurrent;
            _ex = ex;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new MyEnumerator(_throwInCurrent, _ex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class MyEnumerator : IEnumerator<int>
        {
            private readonly bool _throwInCurrent;
            private readonly Exception _ex;

            public MyEnumerator(bool throwInCurrent, Exception ex)
            {
                _throwInCurrent = throwInCurrent;
                _ex = ex;
            }

            public int Current
            {
                get
                {
                    if (_throwInCurrent)
                    {
                        throw _ex;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (!_throwInCurrent)
                {
                    throw _ex;
                }

                return true;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }

    private IEnumerable<TestableSeq<int>> GetSources(
        Action start,
        params TestableSeq<int>[] sources)
    {
        start();

        foreach (var xs in sources)
        {
            yield return xs;
        }
    }

    private IEnumerable<Seq<T>> GetSources<T>(Exception ex, params Seq<T>[] sources)
    {
        foreach (var xs in sources)
        {
            yield return xs;
        }

        throw ex;
    }
}
