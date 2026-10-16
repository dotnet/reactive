// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes an <see cref="OfTypeSeq{TResult}"/> as the target's own
    /// <c>OfType&lt;TResult&gt;()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="OfTypeExtensions.OfType{TResult}(Seq{object})"/>.</remarks>
    Realized<Seq<TResult>> OfType<TResult>(OfTypeSeq<TResult> seq);
}
