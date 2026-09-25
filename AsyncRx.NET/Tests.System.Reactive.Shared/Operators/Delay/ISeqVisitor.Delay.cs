// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>Materializes a <see cref="DelayTimeSeq{T}"/> (built by <see cref="DelayExtensions.Delay{T}(Seq{T}, TimeSpan, SchedulerRef)"/>) as the target's own <c>Delay(dueTime, scheduler)</c> (relative).</summary>
    object DelayTime<T>(DelayTimeSeq<T> seq);

    /// <summary>Materializes a <see cref="DelayAbsoluteSeq{T}"/> (built by <see cref="DelayExtensions.Delay{T}(Seq{T}, DateTimeOffset, SchedulerRef)"/>) as the target's own <c>Delay(dueTime, scheduler)</c> (absolute).</summary>
    object DelayAbsolute<T>(DelayAbsoluteSeq<T> seq);

    /// <summary>Materializes a <see cref="DelaySelectorSeq{T, TDelay}"/> (built by <see cref="DelayExtensions.Delay{T, TDelay}(Seq{T}, Func{T, Seq{TDelay}}, string)"/>) as the target's own <c>Delay(delayDurationSelector)</c>.</summary>
    object DelaySelector<T, TDelay>(DelaySelectorSeq<T, TDelay> seq);

    /// <summary>Materializes a <see cref="DelaySubscriptionSeq{T, TDelay}"/> (built by <see cref="DelayExtensions.Delay{T, TDelay}(Seq{T}, Seq{TDelay}, Func{T, Seq{TDelay}}, string)"/>) as the target's own <c>Delay(subscriptionDelay, delayDurationSelector)</c>.</summary>
    object DelaySubscription<T, TDelay>(DelaySubscriptionSeq<T, TDelay> seq);
}
