// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="GroupBySeq{T, TKey, TElement}"/> as the target's <c>GroupBy</c>.
    /// </summary>
    Realized<Seq<Group<TKey, TElement>>> GroupBy<T, TKey, TElement>(GroupBySeq<T, TKey, TElement> seq);
}
