// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="TakeUntilSeq{T, TOther}"/> as the target's own
    /// <c>TakeUntil(other)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="TakeUntilExtensions.TakeUntil{T, TOther}(Seq{T}, Seq{TOther})"/>.
    /// </remarks>
    Realized<Seq<T>> TakeUntil<T, TOther>(TakeUntilSeq<T, TOther> seq);

    /// <summary>
    /// Materializes a <see cref="TakeUntilTimeSeq{T}"/> as the target's own
    /// <c>TakeUntil(endTime[, scheduler])</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="TakeUntilExtensions.TakeUntil{T}(Seq{T}, DateTimeOffset, SchedulerRef)"/>
    /// or <see cref="TakeUntilExtensions.TakeUntil{T}(Seq{T}, DateTimeOffset)"/>.
    /// </remarks>
    Realized<Seq<T>> TakeUntilTime<T>(TakeUntilTimeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="TakeUntilPredicateSeq{T}"/> as the target's own
    /// <c>TakeUntil(stopPredicate)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="TakeUntilExtensions.TakeUntil{T}(Seq{T}, Func{T, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> TakeUntilPredicate<T>(TakeUntilPredicateSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="TakeUntilTokenSeq{T}"/> as the target's own
    /// <c>TakeUntil(cancellationToken)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="TakeUntilExtensions.TakeUntil{T}(Seq{T}, CancellationToken)"/>.
    /// </remarks>
    Realized<Seq<T>> TakeUntilToken<T>(TakeUntilTokenSeq<T> seq);
}
