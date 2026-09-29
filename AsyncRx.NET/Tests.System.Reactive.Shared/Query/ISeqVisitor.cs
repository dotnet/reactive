// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A target is a visitor: one member per node type.</summary>
/// <remarks>
/// Each member returns the target's own observable as <see cref="object"/>. The target casts;
/// nothing outside it does. The interface is partial and each operator's folder adds its members.
/// </remarks>
public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="NativeSeq{T}"/>: the identity, returning its
    /// <see cref="NativeSeq{T}.Native"/>.
    /// </summary>
    RSeq<T> Native<T>(NativeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="TimerSeq"/> as the target's own <c>Timer(dueTime, scheduler)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Timer(TimeSpan, SchedulerRef)"/>.</remarks>
    RSeq<long> Timer(TimerSeq seq);

    /// <summary>
    /// Materializes a <see cref="ReturnSeq{T}"/> as the target's own <c>Return(value)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Return{T}(T)"/>.</remarks>
    RSeq<T> Return<T>(ReturnSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="RangeSeq"/> as the target's own <c>Range(start, count)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Range(int, int)"/>.</remarks>
    RSeq<int> Range(RangeSeq seq);

    /// <summary>
    /// Materializes a <see cref="EmptySeq{T}"/> as the target's own <c>Empty&lt;T&gt;()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Empty{T}"/>.</remarks>
    RSeq<T> Empty<T>(EmptySeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ThrowSeq{T}"/> as the target's own
    /// <c>Throw&lt;T&gt;(error[, scheduler])</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Throw{T}(Exception)"/>.</remarks>
    RSeq<T> Throw<T>(ThrowSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="SelectSeq{TIn, TOut}"/> as the target's own
    /// <c>Select(selector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SeqExtensions.Select{TIn, TOut}(Seq{TIn}, Func{TIn, TOut}, string)"/>.
    /// </remarks>
    RSeq<TOut> Select<TIn, TOut>(SelectSeq<TIn, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="SelectIndexedSeq{TIn, TOut}"/> as the target's own
    /// <c>Select((x, i) =&gt; ...)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SeqExtensions.Select{TIn, TOut}(Seq{TIn}, Func{TIn, int, TOut}, string)"/>.
    /// </remarks>
    RSeq<TOut> SelectIndexed<TIn, TOut>(SelectIndexedSeq<TIn, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="WhereSeq{T}"/> as the target's own <c>Where(predicate)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SeqExtensions.Where{T}(Seq{T}, Func{T, bool}, string)"/>.
    /// </remarks>
    RSeq<T> Where<T>(WhereSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="SelectManySeq{TIn, TOut}"/> as the target's own
    /// <c>SelectMany(other)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SeqExtensions.SelectMany{TIn, TOut}(Seq{TIn}, Seq{TOut})"/>.
    /// </remarks>
    RSeq<TOut> SelectMany<TIn, TOut>(SelectManySeq<TIn, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="ConcatSeq{T}"/> as the target's own <c>Concat(second)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqExtensions.Concat{T}(Seq{T}, Seq{T})"/>.</remarks>
    RSeq<T> Concat<T>(ConcatSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MergeSeq{T}"/> as the target's own <c>Merge()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqExtensions.Merge{T}(Nested{T})"/>.</remarks>
    RSeq<T> Merge<T>(MergeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="SelectNestedSeq{TIn, TOut}"/> as the target's own indexed
    /// <c>Select</c> over its nested sequence.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SeqExtensions.Select{TIn, TOut}(Nested{TIn}, Func{Seq{TIn}, int, Seq{TOut}}, string)"/>.
    /// The description each callback returns is materialized in place over the real window.
    /// </remarks>
    RSeq<RSeq<TOut>> SelectNested<TIn, TOut>(SelectNestedSeq<TIn, TOut> seq);
}
