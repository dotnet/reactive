// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public static partial class Seq
{
    /// <summary>Describes <c>Seq.Interval(period)</c>.</summary>
    /// <param name="period">
    /// Period for producing the values in the resulting sequence. If this value is equal to
    /// TimeSpan.Zero, the timer will recur as fast as possible.
    /// </param>
    /// <remarks>
    /// Builds an <see cref="IntervalSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Interval(IntervalSeq)"/>.
    /// </remarks>
    public static Seq<long> Interval(TimeSpan period) => new IntervalSeq(period, null);

    /// <summary>Describes <c>Seq.Interval(period, scheduler)</c>.</summary>
    /// <param name="period">
    /// Period for producing the values in the resulting sequence. If this value is equal to
    /// TimeSpan.Zero, the timer will recur as fast as possible.
    /// </param>
    /// <param name="scheduler">Scheduler to run the timer on.</param>
    /// <remarks>
    /// Builds an <see cref="IntervalSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Interval(IntervalSeq)"/>.
    /// </remarks>
    public static Seq<long> Interval(TimeSpan period, SchedulerRef scheduler) =>
        new IntervalSeq(period, scheduler);
}
