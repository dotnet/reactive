// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>Materializes a <see cref="TakeSeq{T}"/> (built by <see cref="TakeExtensions.Take{T}(Seq{T}, int)"/>) as the target's own <c>Take(count)</c>.</summary>
    object Take<T>(TakeSeq<T> seq);

    /// <summary>Materializes a <see cref="TakeScheduledSeq{T}"/> (built by <see cref="TakeExtensions.Take{T}(Seq{T}, int, SchedulerRef)"/>) as the target's own <c>Take(count, scheduler)</c>.</summary>
    object TakeScheduled<T>(TakeScheduledSeq<T> seq);

    /// <summary>Materializes a <see cref="TakeTimeSeq{T}"/> (built by <see cref="TakeExtensions.Take{T}(Seq{T}, TimeSpan, SchedulerRef)"/>) as the target's own <c>Take(duration, scheduler)</c>.</summary>
    object TakeTime<T>(TakeTimeSeq<T> seq);
}
