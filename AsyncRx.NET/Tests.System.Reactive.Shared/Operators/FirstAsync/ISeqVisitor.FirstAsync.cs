// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="FirstAsyncSeq{T}"/> as the target's own <c>FirstAsync()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="FirstAsyncExtensions.FirstAsync{T}(Seq{T})"/>.</remarks>
    Realized<Seq<T>> FirstAsync<T>(FirstAsyncSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="FirstAsyncPredicateSeq{T}"/> as the target's own
    /// <c>FirstAsync(predicate)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="FirstAsyncExtensions.FirstAsync{T}(Seq{T}, Func{T, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> FirstAsyncPredicate<T>(FirstAsyncPredicateSeq<T> seq);
}
