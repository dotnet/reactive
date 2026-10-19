// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="ForSeq{TSource, TResult}"/> as the target's own
    /// <c>For(source, resultSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.For{TSource, TResult}(IEnumerable{TSource}, Func{TSource, Seq{TResult}}, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> For<TSource, TResult>(ForSeq<TSource, TResult> seq);
}
