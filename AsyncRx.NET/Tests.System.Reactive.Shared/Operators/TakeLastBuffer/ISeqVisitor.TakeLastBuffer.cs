// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="TakeLastBufferCountSeq{T}"/> as the target's own
    /// <c>TakeLastBuffer(count)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="TakeLastBufferExtensions.TakeLastBuffer{T}(Seq{T}, int)"/>.
    /// </remarks>
    Realized<Seq<IList<T>>> TakeLastBufferCount<T>(TakeLastBufferCountSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="TakeLastBufferTimeSeq{T}"/> as the target's own
    /// <c>TakeLastBuffer(duration[, scheduler])</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="TakeLastBufferExtensions.TakeLastBuffer{T}(Seq{T}, TimeSpan, SchedulerRef)"/> or
    /// <see cref="TakeLastBufferExtensions.TakeLastBuffer{T}(Seq{T}, TimeSpan)"/>.
    /// </remarks>
    Realized<Seq<IList<T>>> TakeLastBufferTime<T>(TakeLastBufferTimeSeq<T> seq);
}
