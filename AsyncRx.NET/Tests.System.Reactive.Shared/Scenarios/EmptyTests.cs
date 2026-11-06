// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Empty</c> scenarios, from Rx.NET's <c>EmptyTest.cs</c>.</summary>
/// <remarks>
/// 6 of the file's 7 tests (the <c>Empty_ArgumentChecking</c> test is the code-generated
/// stratum). <c>Empty_Basic</c> and the scheduled witness form, which the model now
/// materializes as the target's witness overload, transcribe with <c>ScheduledAt(200)</c> for
/// the original's 201; <c>Empty_Disposed</c> disposes inside the scheduled subscribe, as
/// <c>Return_Disposed</c> does and for the same reason. <c>Empty_ObserverThrows</c> takes the
/// <c>Repeat</c> scenarios' shape (Step 31). <c>Empty_DefaultScheduler</c> and
/// <c>Empty_Basic_Witness2</c> await the completion through
/// <see cref="SharedReactiveTest.ToListAsync{T}"/>, where the originals compare with a
/// blocking <c>AssertEqual</c> or wait on an event.
/// </remarks>
public abstract class EmptyTests : SharedReactiveTest
{
    [TestMethod]
    public void Empty_Basic()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Empty<int>(scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(200))
        );
    }

    [TestMethod]
    public void Empty_Disposed()
    {
        var scheduler = Scheduler;

        var xs = Seq.Empty<int>(scheduler);

        var res = scheduler.CreateObserver<int>();

        scheduler.ScheduleAbsolute(200, async () =>
        {
            var d = await xs.SubscribeAsync(res);
            await d.DisposeAsync();
        });

        scheduler.Start();

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void Empty_ObserverThrows()
    {
        var scheduler1 = new TestSchedulerRef(Target);

        var xs = Seq.Empty<int>(scheduler1);

        scheduler1.ScheduleAbsolute(100, async () =>
            await xs.SubscribeAsync(
                scheduler1,
                x => { },
                exception => { },
                () => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler1.Start());
    }

    [TestMethod]
    public async Task Empty_DefaultScheduler()
    {
        Assert.IsTrue(
            (await ToListAsync(Seq.Empty<int>()))
                .SequenceEqual(await ToListAsync(Seq.Empty<int>(DefaultScheduler))));
    }

    [TestMethod]
    public void Empty_Basic_Witness1()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Empty(scheduler, 42)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(200))
        );
    }

    [TestMethod]
    public async Task Empty_Basic_Witness2()
    {
        Assert.IsEmpty(await ToListAsync(Seq.Empty(42)));
    }
}
