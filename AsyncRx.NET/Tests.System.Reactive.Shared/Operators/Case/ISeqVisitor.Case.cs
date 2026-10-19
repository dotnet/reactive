// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="CaseSeq{TValue, TResult}"/> as the target's own
    /// <c>Case(selector, sources[, defaultSource | scheduler])</c>.
    /// </summary>
    /// <remarks>Built by the <c>Seq.Case</c> overloads in <c>Seq.Case.cs</c>.</remarks>
    Realized<Seq<TResult>> Case<TValue, TResult>(CaseSeq<TValue, TResult> seq)
        where TValue : notnull;
}
