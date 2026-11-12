// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="SkipUntilSeq{T, TOther}"/> as the target's own
    /// <c>SkipUntil(other)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SkipUntilExtensions.SkipUntil{T, TOther}(Seq{T}, Seq{TOther})"/>.
    /// </remarks>
    Realized<Seq<T>> SkipUntil<T, TOther>(SkipUntilSeq<T, TOther> seq);

    /// <summary>
    /// Materializes a <see cref="SkipUntilTimeSeq{T}"/> as the target's own
    /// <c>SkipUntil(startTime[, scheduler])</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SkipUntilExtensions.SkipUntil{T}(Seq{T}, DateTimeOffset, SchedulerRef)"/>
    /// or <see cref="SkipUntilExtensions.SkipUntil{T}(Seq{T}, DateTimeOffset)"/>.
    /// </remarks>
    Realized<Seq<T>> SkipUntilTime<T>(SkipUntilTimeSeq<T> seq);
}
