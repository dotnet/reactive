// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="DelayTimeSeq{T}"/> as the target's own
    /// <c>Delay(dueTime, scheduler)</c> (relative).
    /// </summary>
    /// <remarks>
    /// Built by <see cref="DelayExtensions.Delay{T}(Seq{T}, TimeSpan, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<T>> DelayTime<T>(DelayTimeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="DelayAbsoluteSeq{T}"/> as the target's own
    /// <c>Delay(dueTime, scheduler)</c> (absolute).
    /// </summary>
    /// <remarks>
    /// Built by <see cref="DelayExtensions.Delay{T}(Seq{T}, DateTimeOffset, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<T>> DelayAbsolute<T>(DelayAbsoluteSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="DelaySelectorSeq{T, TDelay}"/> as the target's own
    /// <c>Delay(delayDurationSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="DelayExtensions.Delay{T, TDelay}(Seq{T}, Func{T, Seq{TDelay}}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> DelaySelector<T, TDelay>(DelaySelectorSeq<T, TDelay> seq);

    /// <summary>
    /// Materializes a <see cref="DelaySubscriptionSeq{T, TDelay}"/> as the target's own
    /// <c>Delay(subscriptionDelay, delayDurationSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="DelayExtensions.Delay{T, TDelay}(Seq{T}, Seq{TDelay}, Func{T, Seq{TDelay}}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> DelaySubscription<T, TDelay>(DelaySubscriptionSeq<T, TDelay> seq);
}
