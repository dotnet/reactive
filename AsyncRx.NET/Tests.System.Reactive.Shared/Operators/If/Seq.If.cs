// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

public static partial class Seq
{
    /// <summary>Describes <c>Seq.If(condition, thenSource, elseSource)</c>.</summary>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="condition">
    /// Condition evaluated to decide which of the different sources to return.
    /// </param>
    /// <param name="thenSource">Sequence returned in case the condition evaluates true.</param>
    /// <param name="elseSource">Sequence returned in case the condition evaluates false.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="condition"/>), for printing
    /// the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds an <see cref="IfSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.If{TResult}(IfSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> If<TResult>(
        Func<bool> condition,
        Seq<TResult> thenSource,
        Seq<TResult> elseSource,
        [CallerArgumentExpression(nameof(condition))] string text = "") =>
        new IfSeq<TResult>(condition, thenSource, elseSource, null, text);

    /// <summary>Describes <c>Seq.If(condition, thenSource)</c>.</summary>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="condition">
    /// Condition evaluated to decide which of the different sources to return.
    /// </param>
    /// <param name="thenSource">Sequence returned in case the condition evaluates true.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="condition"/>), for printing
    /// the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds an <see cref="IfSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.If{TResult}(IfSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> If<TResult>(
        Func<bool> condition,
        Seq<TResult> thenSource,
        [CallerArgumentExpression(nameof(condition))] string text = "") =>
        new IfSeq<TResult>(condition, thenSource, null, null, text);

    /// <summary>Describes <c>Seq.If(condition, thenSource, scheduler)</c>.</summary>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="condition">
    /// Condition evaluated to decide which of the different sources to return.
    /// </param>
    /// <param name="thenSource">Sequence returned in case the condition evaluates true.</param>
    /// <param name="scheduler">
    /// Scheduler to generate an empty sequence on in case the condition evaluates false.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="condition"/>), for printing
    /// the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds an <see cref="IfSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.If{TResult}(IfSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> If<TResult>(
        Func<bool> condition,
        Seq<TResult> thenSource,
        SchedulerRef scheduler,
        [CallerArgumentExpression(nameof(condition))] string text = "") =>
        new IfSeq<TResult>(condition, thenSource, null, scheduler, text);
}
