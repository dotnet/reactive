// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Catch</c> overloads, as static and extension methods on <see cref="Seq"/>.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>Observable.Catch(xs, ys, zs)</c>, <c>sources.Catch()</c>,
/// <c>o1.Catch(o2)</c> or <c>o1.Catch((ArgumentException ex) =&gt; o2)</c>, a shared scenario
/// writes the same with <see cref="Seq"/> in place of <c>Observable</c>. One node type per
/// overload, in this folder: two sources, any number of sources, an enumerable of sources, and
/// a handler for one exception type.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>first.Catch(second)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="first">First observable sequence whose exception (if any) is caught.</param>
    /// <param name="second">
    /// Second observable sequence used to produce results when an error occurred in the first
    /// sequence.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="CatchSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Catch{T}(CatchSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Catch<T>(this Seq<T> first, Seq<T> second) =>
        new CatchSeq<T>(first, second);

    /// <summary>
    /// Describes <c>Seq.Catch(source1, source2, ...)</c> over any number of sources.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">Observable sequences to catch exceptions for.</param>
    /// <remarks>
    /// Builds a <see cref="CatchParamsSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.CatchParams{T}(CatchParamsSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Catch<T>(params Seq<T>[] sources) => new CatchParamsSeq<T>(sources);

    /// <summary>Describes <c>sources.Catch()</c> over an enumerable of sources.</summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">Observable sequences to catch exceptions for.</param>
    /// <remarks>
    /// Builds a <see cref="CatchEnumerableSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.CatchEnumerable{T}(CatchEnumerableSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Catch<T>(this IEnumerable<Seq<T>> sources) =>
        new CatchEnumerableSeq<T>(sources);

    /// <summary>Describes <c>source.Catch(handler)</c> for exceptions of one type.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TException">The type of the exception to catch.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="handler">
    /// Exception handler function, producing another observable sequence.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the handler), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="CatchHandlerSeq{T, TException}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.CatchHandler{T, TException}(CatchHandlerSeq{T, TException})"/>.
    /// </remarks>
    public static Seq<T> Catch<T, TException>(
        this Seq<T> source,
        Func<TException, Seq<T>> handler,
        [CallerArgumentExpression(nameof(handler))] string text = "")
        where TException : Exception =>
        new CatchHandlerSeq<T, TException>(source, handler, text);
}
