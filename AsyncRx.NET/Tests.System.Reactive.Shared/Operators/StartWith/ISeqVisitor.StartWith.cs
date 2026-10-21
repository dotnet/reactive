// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="StartWithSeq{T}"/> as the target's own <c>StartWith(values)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="StartWithExtensions.StartWith{T}(Seq{T}, T[])"/>.</remarks>
    Realized<Seq<T>> StartWith<T>(StartWithSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="StartWithEnumerableSeq{T}"/> as the target's own
    /// <c>StartWith(values)</c> over an enumerable of values.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="StartWithExtensions.StartWith{T}(Seq{T}, IEnumerable{T})"/>.
    /// </remarks>
    Realized<Seq<T>> StartWithEnumerable<T>(StartWithEnumerableSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="StartWithSchedulerSeq{T}"/> as the target's own
    /// <c>StartWith(scheduler, values)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="StartWithExtensions.StartWith{T}(Seq{T}, SchedulerRef, T[])"/>.
    /// </remarks>
    Realized<Seq<T>> StartWithScheduler<T>(StartWithSchedulerSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="StartWithSchedulerEnumerableSeq{T}"/> as the target's own
    /// <c>StartWith(scheduler, values)</c> over an enumerable of values.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="StartWithExtensions.StartWith{T}(Seq{T}, SchedulerRef, IEnumerable{T})"/>.
    /// </remarks>
    Realized<Seq<T>> StartWithSchedulerEnumerable<T>(StartWithSchedulerEnumerableSeq<T> seq);
}
