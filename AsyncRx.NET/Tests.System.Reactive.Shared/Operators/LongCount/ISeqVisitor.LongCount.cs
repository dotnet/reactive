// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>Materializes a <see cref="LongCountSeq{T}"/> as the target's own <c>LongCount()</c>.</summary>
    /// <remarks>Built by <see cref="LongCountExtensions.LongCount{T}(Seq{T})"/>.</remarks>
    Realized<Seq<long>> LongCount<T>(LongCountSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="LongCountPredicateSeq{T}"/> as the target's own
    /// <c>LongCount(predicate)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="LongCountExtensions.LongCount{T}(Seq{T}, Func{T, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<long>> LongCountPredicate<T>(LongCountPredicateSeq<T> seq);
}
