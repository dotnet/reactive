// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="CastSeq{TResult}"/> as the target's own
    /// <c>Cast&lt;TResult&gt;()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="CastExtensions.Cast{TResult}(Seq{object})"/>.</remarks>
    Realized<Seq<TResult>> Cast<TResult>(CastSeq<TResult> seq);
}
