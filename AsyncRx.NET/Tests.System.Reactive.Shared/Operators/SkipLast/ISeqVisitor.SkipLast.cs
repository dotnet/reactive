// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="SkipLastCountSeq{T}"/> as the target's own <c>SkipLast(count)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SkipLastExtensions.SkipLast{T}(Seq{T}, int)"/>.</remarks>
    Realized<Seq<T>> SkipLastCount<T>(SkipLastCountSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="SkipLastTimeSeq{T}"/> as the target's own
    /// <c>SkipLast(duration[, scheduler])</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SkipLastExtensions.SkipLast{T}(Seq{T}, TimeSpan, SchedulerRef)"/> or
    /// <see cref="SkipLastExtensions.SkipLast{T}(Seq{T}, TimeSpan)"/>.
    /// </remarks>
    Realized<Seq<T>> SkipLastTime<T>(SkipLastTimeSeq<T> seq);
}
