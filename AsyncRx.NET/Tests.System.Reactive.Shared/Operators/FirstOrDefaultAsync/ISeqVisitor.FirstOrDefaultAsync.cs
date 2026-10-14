// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="FirstOrDefaultAsyncSeq{T}"/> as the target's own <c>FirstOrDefaultAsync()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="FirstOrDefaultAsyncExtensions.FirstOrDefaultAsync{T}(Seq{T})"/>.</remarks>
    Realized<Seq<T>> FirstOrDefaultAsync<T>(FirstOrDefaultAsyncSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="FirstOrDefaultAsyncPredicateSeq{T}"/> as the target's own
    /// <c>FirstOrDefaultAsync(predicate)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="FirstOrDefaultAsyncExtensions.FirstOrDefaultAsync{T}(Seq{T}, Func{T, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> FirstOrDefaultAsyncPredicate<T>(FirstOrDefaultAsyncPredicateSeq<T> seq);
}
