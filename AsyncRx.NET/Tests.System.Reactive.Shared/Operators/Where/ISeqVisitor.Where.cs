// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="WhereSeq{T}"/> as the target's own <c>Where(predicate)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="WhereExtensions.Where{T}(Seq{T}, Func{T, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> Where<T>(WhereSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="WhereIndexedSeq{T}"/> as the target's own
    /// <c>Where((x, i) =&gt; ...)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="WhereExtensions.Where{T}(Seq{T}, Func{T, int, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> WhereIndexed<T>(WhereIndexedSeq<T> seq);
}
