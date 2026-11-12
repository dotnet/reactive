// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes an <see cref="IntervalSeq"/> as the target's own
    /// <c>Interval(period[, scheduler])</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Interval(TimeSpan)"/> or
    /// <see cref="Seq.Interval(TimeSpan, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<long>> Interval(IntervalSeq seq);
}
