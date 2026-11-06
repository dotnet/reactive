// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>FromEventPattern</c> scenarios on Rx.NET, plus two that wait on a library
/// question.
/// </summary>
/// <remarks>
/// <c>FromEventPattern_ConversionThrows</c> and <c>FromEventPattern_Reflection_Instance_Throws</c>
/// are the originals (but for MSTest's assertions): an error raised while subscribing, by the
/// conversion or by the event's add accessor, reaches the observer as <c>OnError</c>, which Rx.NET
/// has done since v2. AsyncRx.NET lets it propagate from <c>SubscribeAsync</c> instead, the open
/// question on <c>AsyncObservableBase.SubscribeAsync</c> recorded at Step 28; the two tests sit
/// here until it is settled, and move to the shared class if the answer is Rx.NET's.
/// </remarks>
[TestClass]
public sealed class RxFromEventPatternTests : FromEventPatternTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void FromEventPattern_ConversionThrows()
    {
        var ex = new Exception();

        var fe = new FromEventPattern();

        var res =
            Observable.FromEventPattern<Action<int>, FromEventPattern.TestEventArgs>(
                h => { throw ex; },
                h => fe.E4 += h,
                h => fe.E4 -= h
            );

        var err = default(Exception);
        res.Subscribe(_ => { Assert.Fail(); }, ex_ => err = ex_, () => { Assert.Fail(); });

        Assert.AreSame(ex, err);
    }

    [TestMethod]
    public void FromEventPattern_Reflection_Instance_Throws()
    {
        //
        // BREAKING CHANGE v2.0 > v1.x - We no longer throw the exception synchronously as part of
        //                               the Subscribe, so it comes out through OnError now. Also,
        //                               we now unwrap TargetInvocationException objects.
        //
        var exAdd = default(Exception);
        var xs = Observable.FromEventPattern<FromEventPattern.TestEventArgs>(
            new FromEventPattern(),
            "AddThrows");
        xs.Subscribe(_ => { Assert.Fail(); }, ex => exAdd = ex, () => { Assert.Fail(); });
        Assert.IsTrue(exAdd is InvalidOperationException);

        //
        // Notice the exception propgation behavior is asymmetric by design. Below, the Dispose
        // call will throw synchronously, merely because we happen to use FromEventPattern from a
        // thread with no SynchronizationContext, causing the Immediate scheduler to be used.
        //
        // See AddHandler in FromEvent.cs for more information on the design rationale of the
        // asymmetry you see here.
        //
        var exRem = default(Exception);
        var ys = Observable.FromEventPattern<FromEventPattern.TestEventArgs>(
            new FromEventPattern(),
            "RemoveThrows");
        var d = ys.Subscribe(_ => { Assert.Fail(); }, ex => exRem = ex, () => { Assert.Fail(); });
        Assert.ThrowsExactly<InvalidOperationException>(d.Dispose);
    }
}
