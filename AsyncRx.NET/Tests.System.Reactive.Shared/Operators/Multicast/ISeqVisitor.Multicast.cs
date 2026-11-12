// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="MulticastSeq{T}"/> as the target's own <c>Multicast(subject)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="MulticastExtensions.Multicast{T}(Seq{T}, SubjectSeq{T})"/>. The
    /// realization is the target's connectable observable.
    /// </remarks>
    Realized<Seq<T>> Multicast<T>(MulticastSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MulticastSelectorSeq{T, TResult}"/> as the target's own
    /// <c>Multicast(subjectSelector, selector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="MulticastExtensions.Multicast{T, TResult}(Seq{T}, Func{SubjectSeq{T}}, Func{Seq{T}, Seq{TResult}}, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> MulticastSelector<T, TResult>(MulticastSelectorSeq<T, TResult> seq);
}
