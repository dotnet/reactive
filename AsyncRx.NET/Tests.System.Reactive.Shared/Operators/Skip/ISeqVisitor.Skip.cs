// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="SkipSeq{T}"/> as the target's own <c>Skip(count)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SkipExtensions.Skip{T}(Seq{T}, int)"/>.</remarks>
    Realized<Seq<T>> Skip<T>(SkipSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="SkipTimeSeq{T}"/> as the target's own
    /// <c>Skip(duration, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SkipExtensions.Skip{T}(Seq{T}, TimeSpan, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<T>> SkipTime<T>(SkipTimeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="SkipTimeDefaultSeq{T}"/> as the target's own
    /// <c>Skip(duration)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SkipExtensions.Skip{T}(Seq{T}, TimeSpan)"/>.</remarks>
    Realized<Seq<T>> SkipTimeDefault<T>(SkipTimeDefaultSeq<T> seq);
}
