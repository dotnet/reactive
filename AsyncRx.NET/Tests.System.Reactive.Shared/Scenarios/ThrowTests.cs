// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Throw</c> scenarios, from Rx.NET's <c>ThrowTest.cs</c>.</summary>
/// <remarks>
/// 6 of the file's 7 tests (the <c>Throw_ArgumentChecking</c> test is the code-generated
/// stratum). <c>Throw_Basic</c> and the scheduled witness form, which the model now
/// materializes as the target's witness overload, transcribe with <c>ScheduledAt(200)</c> for
/// the original's 201; <c>Throw_Disposed</c> disposes inside the scheduled subscribe, as
/// <c>Return_Disposed</c> does and for the same reason. <c>Throw_ObserverThrows</c> takes the
/// <c>Repeat</c> scenarios' shape (Step 31). <c>Throw_DefaultScheduler</c> and
/// <c>Throw_Witness_Basic2</c> await the error through
/// <see cref="SharedReactiveTest.ToListAsync{T}"/> and check it is the one thrown, where the
/// originals compare with a blocking <c>AssertEqual</c> or wait on an event.
/// </remarks>
public abstract class ThrowTests : SharedReactiveTest
{
    [TestMethod]
    public void Throw_Basic()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Throw<int>(ex, scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(ScheduledAt(200), ex)
        );
    }

    [TestMethod]
    public void Throw_Disposed()
    {
        var scheduler = Scheduler;

        var xs = Seq.Throw<int>(new Exception(), scheduler);

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
    public void Throw_ObserverThrows()
    {
        var scheduler1 = new TestSchedulerRef(Target);

        var xs = Seq.Throw<int>(new Exception(), scheduler1);

        scheduler1.ScheduleAbsolute(100, async () =>
            await xs.SubscribeAsync(
                scheduler1,
                x => { },
                ex => { throw new InvalidOperationException(); },
                () => { }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler1.Start());
    }

    [TestMethod]
    public async Task Throw_DefaultScheduler()
    {
        var ex = new Exception();

        var e1 = await Assert.ThrowsExactlyAsync<Exception>(
            async () => await ToListAsync(Seq.Throw<int>(ex)));
        var e2 = await Assert.ThrowsExactlyAsync<Exception>(
            async () => await ToListAsync(Seq.Throw<int>(ex, DefaultScheduler)));

        Assert.AreSame(ex, e1);
        Assert.AreSame(ex, e2);
    }

    [TestMethod]
    public void Throw_Witness_Basic1()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Throw(ex, scheduler, 42)
        );

        res.Messages.AssertEqual(
            OnError<int>(ScheduledAt(200), ex)
        );
    }

    [TestMethod]
    public async Task Throw_Witness_Basic2()
    {
        var ex = new Exception();

        var res = await Assert.ThrowsExactlyAsync<Exception>(
            async () => await ToListAsync(Seq.Throw(ex, 42)));

        Assert.AreSame(ex, res);
    }
}
