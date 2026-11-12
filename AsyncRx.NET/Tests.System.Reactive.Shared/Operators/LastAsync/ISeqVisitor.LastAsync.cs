// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="LastAsyncSeq{T}"/> as the target's own <c>LastAsync()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="LastAsyncExtensions.LastAsync{T}(Seq{T})"/>.</remarks>
    Realized<Seq<T>> LastAsync<T>(LastAsyncSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="LastAsyncPredicateSeq{T}"/> as the target's own
    /// <c>LastAsync(predicate)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="LastAsyncExtensions.LastAsync{T}(Seq{T}, Func{T, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> LastAsyncPredicate<T>(LastAsyncPredicateSeq<T> seq);
}
