// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Two properties of the query description itself, checked on the Rx.NET target: the tree prints as the query was written, and an assertion failure names it.</summary>
[TestClass]
public sealed class QueryDescriptionTests : SharedReactiveTest
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void TheQueryPrintsAsWritten()
    {
        var xs = Scheduler.CreateHotObservable(OnNext(210, 1), OnCompleted<int>(300));

        var res = Scheduler.Start(() =>
            xs.Window(2, 1).Select((w, i) => w.Select(x => i + " " + x)).Merge().Take(3)
        );

        Assert.AreEqual("Hot(2 messages).Window(2, 1).Select((w, i) => w.Select(x => i + \" \" + x)).Merge().Take(3)", res.Query);
    }

    [TestMethod]
    public void AFailureNamesTheQuery()
    {
        var xs = Scheduler.CreateHotObservable(OnNext(210, 1), OnCompleted<int>(300));

        var res = Scheduler.Start(() => xs.Take(1));

        var failure = Assert.ThrowsExactly<AssertFailedException>(() => res.Messages.AssertEqual(OnNext(210, 2)));

        Assert.StartsWith("Messages of Hot(2 messages).Take(1): ", failure.Message);
    }
}
