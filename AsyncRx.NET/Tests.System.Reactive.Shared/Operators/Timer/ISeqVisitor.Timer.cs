// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="TimerSeq"/> as the target's own <c>Timer(dueTime[, scheduler])</c>
    /// with a relative due time.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Timer(TimeSpan)"/> or <see cref="Seq.Timer(TimeSpan, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<long>> Timer(TimerSeq seq);

    /// <summary>
    /// Materializes a <see cref="TimerAbsoluteSeq"/> as the target's own
    /// <c>Timer(dueTime[, scheduler])</c> with an absolute due time.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Timer(DateTimeOffset)"/> or
    /// <see cref="Seq.Timer(DateTimeOffset, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<long>> TimerAbsolute(TimerAbsoluteSeq seq);

    /// <summary>
    /// Materializes a <see cref="TimerPeriodicSeq"/> as the target's own
    /// <c>Timer(dueTime, period[, scheduler])</c> with a relative due time.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Timer(TimeSpan, TimeSpan)"/> or
    /// <see cref="Seq.Timer(TimeSpan, TimeSpan, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<long>> TimerPeriodic(TimerPeriodicSeq seq);

    /// <summary>
    /// Materializes a <see cref="TimerAbsolutePeriodicSeq"/> as the target's own
    /// <c>Timer(dueTime, period[, scheduler])</c> with an absolute due time.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Timer(DateTimeOffset, TimeSpan)"/> or
    /// <see cref="Seq.Timer(DateTimeOffset, TimeSpan, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<long>> TimerAbsolutePeriodic(TimerAbsolutePeriodicSeq seq);
}
