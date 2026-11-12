// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="ThrottleTimeSeq{T}"/> as the target's own
    /// <c>Throttle(dueTime[, scheduler])</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="ThrottleExtensions.Throttle{T}(Seq{T}, TimeSpan, SchedulerRef)"/> or
    /// <see cref="ThrottleExtensions.Throttle{T}(Seq{T}, TimeSpan)"/>.
    /// </remarks>
    Realized<Seq<T>> ThrottleTime<T>(ThrottleTimeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ThrottleSelectorSeq{T, TThrottle}"/> as the target's own
    /// <c>Throttle(throttleDurationSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ThrottleExtensions.Throttle{T, TThrottle}(Seq{T}, Func{T, Seq{TThrottle}}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> ThrottleSelector<T, TThrottle>(ThrottleSelectorSeq<T, TThrottle> seq);
}
