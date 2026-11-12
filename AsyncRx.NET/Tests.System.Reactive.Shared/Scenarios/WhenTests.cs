// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>When</c> scenarios, from Rx.NET's <c>WhenTest.cs</c>.</summary>
/// <remarks>
/// 8 of the file's 9 tests (<c>When_ArgumentChecking</c> is the code-generated stratum). Six
/// transcribe as they are, <c>WhenNeverNever</c> included: its <c>AsObservable()</c> on the
/// second <c>Never</c> keeps the two sources distinct objects, which matters because the join
/// keys its subscriptions by source, and the model gains that operator here.
/// <c>When_PlansIteratorThrows</c> subscribes through the kit's handler form with a completion
/// handler the original leaves out; <c>SameSource</c> awaits through
/// <see cref="SharedReactiveTest.ToListAsync{T}"/> where the original blocks on
/// <c>ToList().First()</c>.
/// </remarks>
public abstract class WhenTests : SharedReactiveTest
{
    [TestMethod]
    public void WhenMultipleDataSymmetric()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnCompleted<int>(270)
        );

        var res = scheduler.Start(() =>
            Seq.When(
                xs.And(ys).Then((x, y) => x + y)
            )
        );

        res.Messages.AssertEqual(
            OnNext(240, 1 + 4),
            OnNext(250, 2 + 5),
            OnNext(260, 3 + 6),
            OnCompleted<int>(270)
        );
    }

    [TestMethod]
    public void WhenMultipleDataAsymmetric()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(240, 4),
            OnNext(250, 5),
            OnCompleted<int>(270)
        );

        var res = scheduler.Start(() =>
            Seq.When(
                xs.And(ys).Then((x, y) => x + y)
            )
        );

        res.Messages.AssertEqual(
            OnNext(240, 1 + 4),
            OnNext(250, 2 + 5),
            OnCompleted<int>(270)
        );
    }

    [TestMethod]
    public void WhenEmptyEmpty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnCompleted<int>(240)
        );

        var ys = scheduler.CreateHotObservable(
            OnCompleted<int>(270)
        );

        var res = scheduler.Start(() =>
            Seq.When(
                xs.And(ys).Then((x, y) => x + y)
            )
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(270)
        );
    }

    [TestMethod]
    public void WhenNeverNever()
    {
        var scheduler = Scheduler;

        var xs = Seq.Never<int>();
        var ys = Seq.Never<int>().AsObservable();

        var res = scheduler.Start(() =>
            Seq.When(
                xs.And(ys).Then((x, y) => x + y)
            )
        );

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void WhenThrowNonEmpty()
    {
        var ex = new Exception();
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnError<int>(240, ex)
        );

        var ys = scheduler.CreateHotObservable(
            OnCompleted<int>(270)
        );

        var res = scheduler.Start(() =>
            Seq.When(
                xs.And(ys).Then((x, y) => x + y)
            )
        );

        res.Messages.AssertEqual(
            OnError<int>(240, ex)
        );
    }

    [TestMethod]
    public void ComplicatedWhen()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnCompleted<int>(270)
        );

        var zs = scheduler.CreateHotObservable(
            OnNext(220, 7),
            OnNext(230, 8),
            OnNext(240, 9),
            OnCompleted<int>(300)
        );

        var res = scheduler.Start(() =>
            Seq.When(
                xs.And(ys).Then((x, y) => x + y),
                xs.And(zs).Then((x, z) => x * z),
                ys.And(zs).Then((y, z) => y - z)
            )
        );

        res.Messages.AssertEqual(
            OnNext(220, 1 * 7),
            OnNext(230, 2 * 8),
            OnNext(240, 3 + 4),
            OnNext(250, 5 - 9),
            OnCompleted<int>(300)
        );
    }

    [TestMethod]
    public async Task When_PlansIteratorThrows()
    {
        var ex = new Exception();
        var _e = default(Exception);

        await GetPlans(ex).When().SubscribeAsync(Scheduler, _ => { }, e => { _e = e; }, () => { });
        Assert.AreSame(_e, ex);
    }

    private IEnumerable<SeqPlan<int>> GetPlans(Exception ex)
    {
        if (ex != null)
        {
            throw ex;
        }

        yield break;
    }

    [TestMethod]
    public async Task SameSource()
    {
        var source = Seq.Range(1, 5);

        var list = await ToListAsync(Seq.When(source.And(source).Then((a, b) => a + b)));

        Assert.IsTrue(new List<int>() { 2, 4, 6, 8, 10 }.SequenceEqual(list));
    }
}
