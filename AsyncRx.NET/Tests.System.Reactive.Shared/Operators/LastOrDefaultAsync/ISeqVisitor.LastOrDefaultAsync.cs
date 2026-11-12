// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="LastOrDefaultAsyncSeq{T}"/> as the target's own <c>LastOrDefaultAsync()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="LastOrDefaultAsyncExtensions.LastOrDefaultAsync{T}(Seq{T})"/>.</remarks>
    Realized<Seq<T>> LastOrDefaultAsync<T>(LastOrDefaultAsyncSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="LastOrDefaultAsyncPredicateSeq{T}"/> as the target's own
    /// <c>LastOrDefaultAsync(predicate)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="LastOrDefaultAsyncExtensions.LastOrDefaultAsync{T}(Seq{T}, Func{T, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> LastOrDefaultAsyncPredicate<T>(LastOrDefaultAsyncPredicateSeq<T> seq);
}
