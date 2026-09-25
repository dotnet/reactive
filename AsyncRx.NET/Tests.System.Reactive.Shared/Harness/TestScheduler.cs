// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The per-test virtual-time scheduler: the kit's counterpart of Rx.NET's <c>TestScheduler</c>,
/// usable both as the harness and as the scheduler argument to operators (it is a
/// <see cref="SchedulerRef"/>), the two roles the sync suite's <c>scheduler</c> local plays.
/// Every member forwards to the <see cref="Target"/>. Its <see cref="SchedulerRef.Native"/> is the
/// target's own test scheduler — <c>Microsoft.Reactive.Testing.TestScheduler</c> on Rx.NET,
/// <c>TestAsyncScheduler</c> on AsyncRx.NET — which target-specific tests may cast to reach
/// features the shared surface does not expose.
/// </summary>
public sealed class TestScheduler : SchedulerRef
{
    public TestScheduler(IRxTarget target)
        : base(target.CreateTestScheduler(), "Scheduler")
    {
        Target = target;
    }

    public IRxTarget Target { get; }

    public TestableSeq<T> CreateHotObservable<T>(params Recorded<Notification<T>>[] messages) => Target.CreateHotObservable(this, messages);

    public TestableSeq<T> CreateColdObservable<T>(params Recorded<Notification<T>>[] messages) => Target.CreateColdObservable(this, messages);

    public TestableObserver<T> Start<T>(Func<Seq<T>> create) => Target.Start(this, create, ReactiveTest.Created, ReactiveTest.Subscribed, ReactiveTest.Disposed);

    public TestableObserver<T> Start<T>(Func<Seq<T>> create, long disposed) => Target.Start(this, create, ReactiveTest.Created, ReactiveTest.Subscribed, disposed);

    public TestableObserver<T> Start<T>(Func<Seq<T>> create, long created, long subscribed, long disposed) => Target.Start(this, create, created, subscribed, disposed);

    /// <summary>Runs virtual time to exhaustion (the parameterless <c>TestScheduler.Start()</c>).</summary>
    public void Start() => Target.Run(this);

    public SchedulerRef DisableOptimizations() => Target.DisableOptimizations(this);

    public long Clock => Target.Clock(this);

    public void ScheduleAbsolute(long tick, Func<ValueTask> action) => Target.ScheduleAbsolute(this, tick, action);

    public void ScheduleAbsolute(long tick, Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        Target.ScheduleAbsolute(this, tick, () =>
        {
            action();
            return default;
        });
    }

    public TestableObserver<T> CreateObserver<T>() => Target.CreateObserver<T>(this);
}
