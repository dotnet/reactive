// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes an <see cref="IfSeq{TResult}"/> as the target's own
    /// <c>If(condition, thenSource[, elseSource | scheduler])</c>.
    /// </summary>
    /// <remarks>Built by the <c>Seq.If</c> overloads in <c>Seq.If.cs</c>.</remarks>
    Realized<Seq<TResult>> If<TResult>(IfSeq<TResult> seq);
}
