// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// A platform is a visitor: one member per node type, each returning the platform's own
/// observable as <see cref="object"/>. The platform casts; nothing outside it does. The
/// interface is partial and each operator's folder adds its members.
/// </summary>
public partial interface ISeqVisitor
{
    object Native<T>(NativeSeq<T> seq);

    object Timer(TimerSeq seq);

    object Return<T>(ReturnSeq<T> seq);

    object Range(RangeSeq seq);

    object Empty<T>(EmptySeq<T> seq);

    object Throw<T>(ThrowSeq<T> seq);

    object Select<TIn, TOut>(SelectSeq<TIn, TOut> seq);

    object SelectIndexed<TIn, TOut>(SelectIndexedSeq<TIn, TOut> seq);

    object Where<T>(WhereSeq<T> seq);

    object SelectMany<TIn, TOut>(SelectManySeq<TIn, TOut> seq);

    object Concat<T>(ConcatSeq<T> seq);

    object Merge<T>(MergeSeq<T> seq);

    object SelectNested<TIn, TOut>(SelectNestedSeq<TIn, TOut> seq);
}
