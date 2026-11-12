// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.TakeLast(duration, timerScheduler, loopScheduler)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="TakeLastExtensions.TakeLast{T}(Seq{T}, TimeSpan, SchedulerRef, SchedulerRef)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.TakeLastTimeLoop{T}(TakeLastTimeLoopSeq{T})"/>.
/// </remarks>
public sealed class TakeLastTimeLoopSeq<T>(
    Seq<T> source,
    TimeSpan duration,
    SchedulerRef timerScheduler,
    SchedulerRef loopScheduler)
    : Seq<T>
{
    /// <summary>Source sequence to take elements from.</summary>
    public Seq<T> Source => source;

    /// <summary>Duration for taking elements from the end of the sequence.</summary>
    public TimeSpan Duration => duration;

    /// <summary>Scheduler to run the timer on.</summary>
    public SchedulerRef TimerScheduler => timerScheduler;

    /// <summary>Scheduler to drain the collected elements.</summary>
    public SchedulerRef LoopScheduler => loopScheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.TakeLastTimeLoop(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"{source}.TakeLast({duration.Ticks} ticks, {timerScheduler}, {loopScheduler})";
}
