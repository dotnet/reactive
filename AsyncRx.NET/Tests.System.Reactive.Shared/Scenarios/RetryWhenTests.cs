// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>RetryWhen</c> scenarios, from Rx.NET's <c>RetryWhenTest.cs</c>.</summary>
/// <remarks>
/// 6 of the file's 7 tests (<c>RetryWhen_Observable_ArgumentChecking</c> is the code-generated
/// stratum), all transcribed as they are: the handler receives the error signals as a
/// <c>Seq&lt;Exception&gt;</c> and returns a description, which the bridge adapts to the
/// target's own delegate.
/// </remarks>
public abstract class RetryWhenTests : SharedReactiveTest
{
    [TestMethod]
    public void RetryWhen_Observable_Basic()
    {
        var xs = Scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.RetryWhen(v => v)
        );

        res.Messages.AssertEqual(
            OnNext(300, 1),
            OnNext(350, 2),
            OnNext(400, 3),
            OnCompleted<int>(450)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );
    }

    [TestMethod]
    public void RetryWhen_Observable_Handler_Completes()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3),
            OnError<int>(250, ex)
        );

        var res = Scheduler.Start(() =>
            xs.RetryWhen(v => v.Take(1).Skip(1))
        );

        res.Messages.AssertEqual(
            OnNext(300, 1),
            OnNext(350, 2),
            OnNext(400, 3),
            OnCompleted<int>(450)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );
    }

    [TestMethod]
    public void RetryWhen_Observable_Handler_Throws()
    {
        var ex = new Exception();

        var res = Scheduler.Start(() =>
            Seq.Return(1).RetryWhen<int, int>(v => { throw ex; })
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );
    }

    [TestMethod]
    public void RetryWhen_Observable_Handler_Errors()
    {
        var ex = new Exception();
        var ex2 = new Exception();

        var xs = Scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3),
            OnError<int>(250, ex)
        );

        var res = Scheduler.Start(() =>
            xs.RetryWhen(v => v.SelectMany(w => Seq.Throw<int>(ex2)))
        );

        res.Messages.AssertEqual(
            OnNext(300, 1),
            OnNext(350, 2),
            OnNext(400, 3),
            OnError<int>(450, ex2)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );
    }

    [TestMethod]
    public void RetryWhen_Observable_RetryCount_Basic()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateColdObservable(
            OnNext(5, 1),
            OnNext(10, 2),
            OnNext(15, 3),
            OnError<int>(20, ex)
        );

        var res = Scheduler.Start(() =>
            xs.RetryWhen(v =>
            {
                int[] count = [0];
                return v.SelectMany(w =>
                {
                    var c = ++count[0];
                    if (c == 3)
                    {
                        return Seq.Throw<int>(w);
                    }
                    return Seq.Return(1);
                });
            })
        );

        res.Messages.AssertEqual(
            OnNext(205, 1),
            OnNext(210, 2),
            OnNext(215, 3),
            OnNext(225, 1),
            OnNext(230, 2),
            OnNext(235, 3),
            OnNext(245, 1),
            OnNext(250, 2),
            OnNext(255, 3),
            OnError<int>(260, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220),
            Subscribe(220, 240),
            Subscribe(240, 260)
        );
    }

    [TestMethod]
    public void RetryWhen_Observable_RetryCount_Delayed()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateColdObservable(
            OnNext(5, 1),
            OnNext(10, 2),
            OnNext(15, 3),
            OnError<int>(20, ex)
        );

        var res = Scheduler.Start(() =>
            xs.RetryWhen(v =>
            {
                int[] count = [0];
                return v.SelectMany(w =>
                {
                    var c = ++count[0];
                    if (c == 3)
                    {
                        return Seq.Throw<int>(w);
                    }
                    return Seq.Return(1).Delay(TimeSpan.FromTicks(c * 100), Scheduler);
                });
            })
        );

        res.Messages.AssertEqual(
            OnNext(205, 1),
            OnNext(210, 2),
            OnNext(215, 3),
            OnNext(325, 1),
            OnNext(330, 2),
            OnNext(335, 3),
            OnNext(545, 1),
            OnNext(550, 2),
            OnNext(555, 3),
            OnError<int>(560, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220),
            Subscribe(320, 340),
            Subscribe(540, 560)
        );
    }
}
