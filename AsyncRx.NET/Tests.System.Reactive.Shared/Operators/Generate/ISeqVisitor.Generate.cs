// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="GenerateSeq{TState, TResult}"/> as the target's own
    /// <c>Generate(initialState, condition, iterate, resultSelector[, scheduler])</c>.
    /// </summary>
    /// <remarks>Built by the <c>Seq.Generate</c> overloads in <c>Seq.Generate.cs</c>.</remarks>
    Realized<Seq<TResult>> Generate<TState, TResult>(GenerateSeq<TState, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="GenerateTimeSeq{TState, TResult}"/> as the target's own
    /// <c>Generate(..., timeSelector[, scheduler])</c> with a relative time selector.
    /// </summary>
    /// <remarks>Built by the <c>Seq.Generate</c> overloads in <c>Seq.Generate.cs</c>.</remarks>
    Realized<Seq<TResult>> GenerateTime<TState, TResult>(GenerateTimeSeq<TState, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="GenerateAbsoluteSeq{TState, TResult}"/> as the target's own
    /// <c>Generate(..., timeSelector[, scheduler])</c> with an absolute time selector.
    /// </summary>
    /// <remarks>Built by the <c>Seq.Generate</c> overloads in <c>Seq.Generate.cs</c>.</remarks>
    Realized<Seq<TResult>> GenerateAbsolute<TState, TResult>(
        GenerateAbsoluteSeq<TState, TResult> seq);
}
