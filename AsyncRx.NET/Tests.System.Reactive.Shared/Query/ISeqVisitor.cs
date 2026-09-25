// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// A target is a visitor: one member per node type, each returning the target's own
/// observable as <see cref="object"/>. The target casts; nothing outside it does. The
/// interface is partial and each operator's folder adds its members.
/// </summary>
public partial interface ISeqVisitor
{
    /// <summary>Materializes a <see cref="NativeSeq{T}"/>: the identity, returning its <see cref="NativeSeq{T}.Native"/>.</summary>
    object Native<T>(NativeSeq<T> seq);

    /// <summary>Materializes a <see cref="TimerSeq"/> (built by <see cref="Observable.Timer(TimeSpan, SchedulerRef)"/>) as the target's own <c>Timer(dueTime, scheduler)</c>.</summary>
    object Timer(TimerSeq seq);

    /// <summary>Materializes a <see cref="ReturnSeq{T}"/> (built by <see cref="Observable.Return{T}(T)"/>) as the target's own <c>Return(value)</c>.</summary>
    object Return<T>(ReturnSeq<T> seq);

    /// <summary>Materializes a <see cref="RangeSeq"/> (built by <see cref="Observable.Range(int, int)"/>) as the target's own <c>Range(start, count)</c>.</summary>
    object Range(RangeSeq seq);

    /// <summary>Materializes a <see cref="EmptySeq{T}"/> (built by <see cref="Observable.Empty{T}"/>) as the target's own <c>Empty&lt;T&gt;()</c>.</summary>
    object Empty<T>(EmptySeq<T> seq);

    /// <summary>Materializes a <see cref="ThrowSeq{T}"/> (built by <see cref="Observable.Throw{T}(Exception)"/>) as the target's own <c>Throw&lt;T&gt;(error[, scheduler])</c>.</summary>
    object Throw<T>(ThrowSeq<T> seq);

    /// <summary>Materializes a <see cref="SelectSeq{TIn, TOut}"/> (built by <see cref="SeqExtensions.Select{TIn, TOut}(Seq{TIn}, Func{TIn, TOut}, string)"/>) as the target's own <c>Select(selector)</c>.</summary>
    object Select<TIn, TOut>(SelectSeq<TIn, TOut> seq);

    /// <summary>Materializes a <see cref="SelectIndexedSeq{TIn, TOut}"/> (built by <see cref="SeqExtensions.Select{TIn, TOut}(Seq{TIn}, Func{TIn, int, TOut}, string)"/>) as the target's own <c>Select((x, i) =&gt; ...)</c>.</summary>
    object SelectIndexed<TIn, TOut>(SelectIndexedSeq<TIn, TOut> seq);

    /// <summary>Materializes a <see cref="WhereSeq{T}"/> (built by <see cref="SeqExtensions.Where{T}(Seq{T}, Func{T, bool}, string)"/>) as the target's own <c>Where(predicate)</c>.</summary>
    object Where<T>(WhereSeq<T> seq);

    /// <summary>Materializes a <see cref="SelectManySeq{TIn, TOut}"/> (built by <see cref="SeqExtensions.SelectMany{TIn, TOut}(Seq{TIn}, Seq{TOut})"/>) as the target's own <c>SelectMany(other)</c>.</summary>
    object SelectMany<TIn, TOut>(SelectManySeq<TIn, TOut> seq);

    /// <summary>Materializes a <see cref="ConcatSeq{T}"/> (built by <see cref="SeqExtensions.Concat{T}(Seq{T}, Seq{T})"/>) as the target's own <c>Concat(second)</c>.</summary>
    object Concat<T>(ConcatSeq<T> seq);

    /// <summary>Materializes a <see cref="MergeSeq{T}"/> (built by <see cref="SeqExtensions.Merge{T}(Nested{T})"/>) as the target's own <c>Merge()</c>.</summary>
    object Merge<T>(MergeSeq<T> seq);

    /// <summary>Materializes a <see cref="SelectNestedSeq{TIn, TOut}"/> (built by <see cref="SeqExtensions.Select{TIn, TOut}(Nested{TIn}, Func{Seq{TIn}, int, Seq{TOut}}, string)"/>) as the target's own <c>Select((window, i) =&gt; ...)</c> over its nested sequence, materializing the description each callback returns in place over the real window.</summary>
    object SelectNested<TIn, TOut>(SelectNestedSeq<TIn, TOut> seq);
}
