// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public static partial class Seq
{
    /// <summary>Describes <c>Seq.Timer(dueTime)</c>.</summary>
    /// <param name="dueTime">
    /// Relative time at which to produce the value. If this value is less than or equal to
    /// TimeSpan.Zero, the timer will fire as soon as possible.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TimerSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Timer(TimerSeq)"/>.
    /// </remarks>
    public static Seq<long> Timer(TimeSpan dueTime) => new TimerSeq(dueTime, null);

    /// <summary>Describes <c>Seq.Timer(dueTime, scheduler)</c>.</summary>
    /// <param name="dueTime">
    /// Relative time at which to produce the value. If this value is less than or equal to
    /// TimeSpan.Zero, the timer will fire as soon as possible.
    /// </param>
    /// <param name="scheduler">Scheduler to run the timer on.</param>
    /// <remarks>
    /// Builds a <see cref="TimerSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Timer(TimerSeq)"/>.
    /// </remarks>
    public static Seq<long> Timer(TimeSpan dueTime, SchedulerRef scheduler) =>
        new TimerSeq(dueTime, scheduler);

    /// <summary>Describes <c>Seq.Timer(dueTime)</c> with an absolute due time.</summary>
    /// <param name="dueTime">
    /// Absolute time at which to produce the value. If this value is less than or equal to the
    /// current time, the timer will fire as soon as possible.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TimerAbsoluteSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TimerAbsolute(TimerAbsoluteSeq)"/>.
    /// </remarks>
    public static Seq<long> Timer(DateTimeOffset dueTime) => new TimerAbsoluteSeq(dueTime, null);

    /// <summary>Describes <c>Seq.Timer(dueTime, scheduler)</c> with an absolute due time.</summary>
    /// <param name="dueTime">
    /// Absolute time at which to produce the value. If this value is less than or equal to the
    /// current time, the timer will fire as soon as possible.
    /// </param>
    /// <param name="scheduler">Scheduler to run the timer on.</param>
    /// <remarks>
    /// Builds a <see cref="TimerAbsoluteSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TimerAbsolute(TimerAbsoluteSeq)"/>.
    /// </remarks>
    public static Seq<long> Timer(DateTimeOffset dueTime, SchedulerRef scheduler) =>
        new TimerAbsoluteSeq(dueTime, scheduler);

    /// <summary>Describes <c>Seq.Timer(dueTime, period)</c>.</summary>
    /// <param name="dueTime">
    /// Relative time at which to produce the first value. If this value is less than or equal to
    /// TimeSpan.Zero, the timer will fire as soon as possible.
    /// </param>
    /// <param name="period">
    /// Period to produce subsequent values. If this value is equal to TimeSpan.Zero, the timer
    /// will recur as fast as possible.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TimerPeriodicSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TimerPeriodic(TimerPeriodicSeq)"/>.
    /// </remarks>
    public static Seq<long> Timer(TimeSpan dueTime, TimeSpan period) =>
        new TimerPeriodicSeq(dueTime, period, null);

    /// <summary>Describes <c>Seq.Timer(dueTime, period, scheduler)</c>.</summary>
    /// <param name="dueTime">
    /// Relative time at which to produce the first value. If this value is less than or equal to
    /// TimeSpan.Zero, the timer will fire as soon as possible.
    /// </param>
    /// <param name="period">
    /// Period to produce subsequent values. If this value is equal to TimeSpan.Zero, the timer
    /// will recur as fast as possible.
    /// </param>
    /// <param name="scheduler">Scheduler to run timers on.</param>
    /// <remarks>
    /// Builds a <see cref="TimerPeriodicSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TimerPeriodic(TimerPeriodicSeq)"/>.
    /// </remarks>
    public static Seq<long> Timer(TimeSpan dueTime, TimeSpan period, SchedulerRef scheduler) =>
        new TimerPeriodicSeq(dueTime, period, scheduler);

    /// <summary>Describes <c>Seq.Timer(dueTime, period)</c> with an absolute due time.</summary>
    /// <param name="dueTime">
    /// Absolute time at which to produce the first value. If this value is less than or equal to
    /// the current time, the timer will fire as soon as possible.
    /// </param>
    /// <param name="period">
    /// Period to produce subsequent values. If this value is equal to TimeSpan.Zero, the timer
    /// will recur as fast as possible.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TimerAbsolutePeriodicSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TimerAbsolutePeriodic(TimerAbsolutePeriodicSeq)"/>.
    /// </remarks>
    public static Seq<long> Timer(DateTimeOffset dueTime, TimeSpan period) =>
        new TimerAbsolutePeriodicSeq(dueTime, period, null);

    /// <summary>
    /// Describes <c>Seq.Timer(dueTime, period, scheduler)</c> with an absolute due time.
    /// </summary>
    /// <param name="dueTime">
    /// Absolute time at which to produce the first value. If this value is less than or equal to
    /// the current time, the timer will fire as soon as possible.
    /// </param>
    /// <param name="period">
    /// Period to produce subsequent values. If this value is equal to TimeSpan.Zero, the timer
    /// will recur as fast as possible.
    /// </param>
    /// <param name="scheduler">Scheduler to run timers on.</param>
    /// <remarks>
    /// Builds a <see cref="TimerAbsolutePeriodicSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TimerAbsolutePeriodic(TimerAbsolutePeriodicSeq)"/>.
    /// </remarks>
    public static Seq<long> Timer(
        DateTimeOffset dueTime,
        TimeSpan period,
        SchedulerRef scheduler) =>
        new TimerAbsolutePeriodicSeq(dueTime, period, scheduler);
}
