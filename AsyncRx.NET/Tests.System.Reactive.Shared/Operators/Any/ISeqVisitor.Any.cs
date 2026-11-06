// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>Materializes an <see cref="AnySeq{T}"/> as the target's own <c>Any()</c>.</summary>
    /// <remarks>Built by <see cref="AnyExtensions.Any{T}(Seq{T})"/>.</remarks>
    Realized<Seq<bool>> Any<T>(AnySeq<T> seq);

    /// <summary>
    /// Materializes an <see cref="AnyPredicateSeq{T}"/> as the target's own
    /// <c>Any(predicate)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="AnyExtensions.Any{T}(Seq{T}, Func{T, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<bool>> AnyPredicate<T>(AnyPredicateSeq<T> seq);
}
