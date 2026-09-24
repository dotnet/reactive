// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Reactive.Shared.Rx;

/// <summary>Two properties of the representation itself: the tree prints as the query, and a failure names it.</summary>
[TestClass]
public sealed class RxKitTests : DescribedTest
{
    protected override IPlatform Platform => RxPlatform.Instance;

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

        StringAssert.StartsWith(failure.Message, "Messages of Hot(2 messages).Take(1): ");
    }
}
