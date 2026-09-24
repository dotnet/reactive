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
/// </summary>
public sealed class TestScheduler : SchedulerRef
{
    public TestScheduler(IPlatform platform)
        : base(platform.CreateTestScheduler(), "Scheduler")
    {
        Platform = platform;
    }

    public IPlatform Platform { get; }

    public TestableSeq<T> CreateHotObservable<T>(params Recorded<Notification<T>>[] messages) => Platform.CreateHotObservable(this, messages);

    public TestableSeq<T> CreateColdObservable<T>(params Recorded<Notification<T>>[] messages) => Platform.CreateColdObservable(this, messages);

    public TestableObserver<T> Start<T>(Func<Seq<T>> create) => Platform.Start(this, create, ReactiveTest.Created, ReactiveTest.Subscribed, ReactiveTest.Disposed);

    public TestableObserver<T> Start<T>(Func<Seq<T>> create, long disposed) => Platform.Start(this, create, ReactiveTest.Created, ReactiveTest.Subscribed, disposed);

    public TestableObserver<T> Start<T>(Func<Seq<T>> create, long created, long subscribed, long disposed) => Platform.Start(this, create, created, subscribed, disposed);

    /// <summary>Runs virtual time to exhaustion (the parameterless <c>TestScheduler.Start()</c>).</summary>
    public void Start() => Platform.Run(this);

    public SchedulerRef DisableOptimizations() => Platform.DisableOptimizations(this);

    public long Clock => Platform.Clock(this);

    public void ScheduleAbsolute(long tick, Func<ValueTask> action) => Platform.ScheduleAbsolute(this, tick, action);

    public void ScheduleAbsolute(long tick, Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        Platform.ScheduleAbsolute(this, tick, () =>
        {
            action();
            return default;
        });
    }

    public TestableObserver<T> CreateObserver<T>() => Platform.CreateObserver<T>(this);
}
