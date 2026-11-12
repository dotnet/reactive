// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>Materializes a <see cref="CountSeq{T}"/> as the target's own <c>Count()</c>.</summary>
    /// <remarks>Built by <see cref="CountExtensions.Count{T}(Seq{T})"/>.</remarks>
    Realized<Seq<int>> Count<T>(CountSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="CountPredicateSeq{T}"/> as the target's own
    /// <c>Count(predicate)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="CountExtensions.Count{T}(Seq{T}, Func{T, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<int>> CountPredicate<T>(CountPredicateSeq<T> seq);
}
