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
    /// Materializes a <see cref="DeferSeq{T}"/> as the target's own <c>Defer(factory)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Defer{T}(Func{Seq{T}}, string)"/>.</remarks>
    Realized<Seq<T>> Defer<T>(DeferSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="NeverSeq{T}"/> as the target's own <c>Never()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Never{T}()"/>.</remarks>
    Realized<Seq<T>> Never<T>(NeverSeq<T> seq);

    /// <summary>
    /// Materializes an <see cref="EmptyWitnessSeq{T}"/> as the target's own
    /// <c>Empty([scheduler, ]witness)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Empty{T}(T)"/> or <see cref="Seq.Empty{T}(SchedulerRef, T)"/>.
    /// </remarks>
    Realized<Seq<T>> EmptyWitness<T>(EmptyWitnessSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ThrowWitnessSeq{T}"/> as the target's own
    /// <c>Throw(error, [scheduler, ]witness)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Throw{T}(Exception, T)"/> or
    /// <see cref="Seq.Throw{T}(Exception, SchedulerRef, T)"/>.
    /// </remarks>
    Realized<Seq<T>> ThrowWitness<T>(ThrowWitnessSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="NeverWitnessSeq{T}"/> as the target's own <c>Never(witness)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Never{T}(T)"/>.</remarks>
    Realized<Seq<T>> NeverWitness<T>(NeverWitnessSeq<T> seq);
}
