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
    Realized<Seq<T>> Native<T>(NativeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="TimerSeq"/> as the target's own <c>Timer(dueTime, scheduler)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Timer(TimeSpan, SchedulerRef)"/>.</remarks>
    Realized<Seq<long>> Timer(TimerSeq seq);

    /// <summary>
    /// Materializes an <see cref="IntervalSeq"/> as the target's own
    /// <c>Interval(period, scheduler)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Interval(TimeSpan, SchedulerRef)"/>.</remarks>
    Realized<Seq<long>> Interval(IntervalSeq seq);

    /// <summary>
    /// Materializes a <see cref="ReturnSeq{T}"/> as the target's own
    /// <c>Return(value[, scheduler])</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Return{T}(T)"/>.</remarks>
    Realized<Seq<T>> Return<T>(ReturnSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="RangeSeq"/> as the target's own <c>Range(start, count)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Range(int, int)"/>.</remarks>
    Realized<Seq<int>> Range(RangeSeq seq);

    /// <summary>
    /// Materializes a <see cref="RangeScheduledSeq"/> as the target's own
    /// <c>Range(start, count, scheduler)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Range(int, int, SchedulerRef)"/>.</remarks>
    Realized<Seq<int>> RangeScheduled(RangeScheduledSeq seq);

    /// <summary>
    /// Materializes an <see cref="EnumerableSeq{T}"/> as the target's own conversion of an
    /// enumerable to a sequence.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.ToSeq{T}(IEnumerable{T})"/>.</remarks>
    Realized<Seq<T>> Enumerable<T>(EnumerableSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="EmptySeq{T}"/> as the target's own
    /// <c>Empty&lt;T&gt;([scheduler])</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Empty{T}()"/>.</remarks>
    Realized<Seq<T>> Empty<T>(EmptySeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ThrowSeq{T}"/> as the target's own
    /// <c>Throw&lt;T&gt;(error[, scheduler])</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Throw{T}(Exception)"/>.</remarks>
    Realized<Seq<T>> Throw<T>(ThrowSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="SelectSeq{TIn, TOut}"/> as the target's own
    /// <c>Select(selector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SeqExtensions.Select{TIn, TOut}(Seq{TIn}, Func{TIn, TOut}, string)"/>.
    /// </remarks>
    Realized<Seq<TOut>> Select<TIn, TOut>(SelectSeq<TIn, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="SelectIndexedSeq{TIn, TOut}"/> as the target's own
    /// <c>Select((x, i) =&gt; ...)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SeqExtensions.Select{TIn, TOut}(Seq{TIn}, Func{TIn, int, TOut}, string)"/>.
    /// </remarks>
    Realized<Seq<TOut>> SelectIndexed<TIn, TOut>(SelectIndexedSeq<TIn, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="WhereSeq{T}"/> as the target's own <c>Where(predicate)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SeqExtensions.Where{T}(Seq{T}, Func{T, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> Where<T>(WhereSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ConcatSeq{T}"/> as the target's own <c>Concat(second)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqExtensions.Concat{T}(Seq{T}, Seq{T})"/>.</remarks>
    Realized<Seq<T>> Concat<T>(ConcatSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="DeferSeq{T}"/> as the target's own <c>Defer(factory)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Defer{T}(Func{Seq{T}}, string)"/>.</remarks>
    Realized<Seq<T>> Defer<T>(DeferSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="NeverSeq{T}"/> as the target's own <c>Never()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Never{T}"/>.</remarks>
    Realized<Seq<T>> Never<T>(NeverSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ConcatNestedSeq{T}"/> as the target's own <c>Concat()</c> over a
    /// nested sequence.
    /// </summary>
    /// <remarks>Built by <see cref="SeqExtensions.Concat{T}(Seq{Seq{T}})"/>.</remarks>
    Realized<Seq<T>> ConcatNested<T>(ConcatNestedSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="RepeatSeq{T}"/> as the target's own <c>Repeat()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqExtensions.Repeat{T}(Seq{T})"/>.</remarks>
    Realized<Seq<T>> Repeat<T>(RepeatSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="RepeatCountSeq{T}"/> as the target's own
    /// <c>Repeat(repeatCount)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqExtensions.Repeat{T}(Seq{T}, int)"/>.</remarks>
    Realized<Seq<T>> RepeatCount<T>(RepeatCountSeq<T> seq);
}
