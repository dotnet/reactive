// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Never</c> scenarios, from Rx.NET's <c>NeverTest.cs</c>.</summary>
/// <remarks>
/// 2 of the file's 3 tests (the <c>Never_ArgumentChecking</c> test is the code-generated
/// stratum), both transcribed as they are but for the awaited subscribe: the plain form and the
/// witness form, which the model now materializes as the target's witness overload.
/// </remarks>
public abstract class NeverTests : SharedReactiveTest
{
    [TestMethod]
    public async Task Never_Basic()
    {
        var scheduler = Scheduler;

        var xs = Seq.Never<int>();

        var res = scheduler.CreateObserver<int>();

        await xs.SubscribeAsync(res);

        scheduler.Start();

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public async Task Never_Basic_Witness()
    {
        var scheduler = Scheduler;

        var xs = Seq.Never(42);

        var res = scheduler.CreateObserver<int>();

        await xs.SubscribeAsync(res);

        scheduler.Start();

        res.Messages.AssertEqual(
        );
    }
}
