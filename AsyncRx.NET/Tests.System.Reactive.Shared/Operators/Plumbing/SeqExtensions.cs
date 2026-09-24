// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

public static class SeqExtensions
{
    public static Seq<TOut> Select<TIn, TOut>(this Seq<TIn> source, Func<TIn, TOut> selector, [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SelectSeq<TIn, TOut>(source, selector, text);

    public static Seq<TOut> Select<TIn, TOut>(this Seq<TIn> source, Func<TIn, int, TOut> selector, [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SelectIndexedSeq<TIn, TOut>(source, selector, text);

    public static Seq<T> Where<T>(this Seq<T> source, Func<T, bool> predicate, [CallerArgumentExpression(nameof(predicate))] string text = "") =>
        new WhereSeq<T>(source, predicate, text);

    public static Seq<TOut> SelectMany<TIn, TOut>(this Seq<TIn> source, Seq<TOut> other) => new SelectManySeq<TIn, TOut>(source, other);

    public static Seq<T> Concat<T>(this Seq<T> first, Seq<T> second) => new ConcatSeq<T>(first, second);

    public static Seq<T> Merge<T>(this Nested<T> sources) => new MergeSeq<T>(sources);

    public static Nested<TOut> Select<TIn, TOut>(this Nested<TIn> source, Func<Seq<TIn>, int, Seq<TOut>> selector, [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SelectNestedSeq<TIn, TOut>(source, selector, text);
}
