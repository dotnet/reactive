// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="SingleAsyncSeq{T}"/> as the target's own <c>SingleAsync()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SingleAsyncExtensions.SingleAsync{T}(Seq{T})"/>.</remarks>
    Realized<Seq<T>> SingleAsync<T>(SingleAsyncSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="SingleAsyncPredicateSeq{T}"/> as the target's own
    /// <c>SingleAsync(predicate)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SingleAsyncExtensions.SingleAsync{T}(Seq{T}, Func{T, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> SingleAsyncPredicate<T>(SingleAsyncPredicateSeq<T> seq);
}
