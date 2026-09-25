// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The creation operators, under the sync suite's own name so that <c>Observable.Timer(t, Scheduler)</c>
/// is the sync text with <c>scheduler</c> capitalised. Targets that also import
/// <c>System.Reactive.Linq</c> alias one of the two.
/// </summary>
/// <remarks>
/// Nothing here runs an operator. Each method builds a leaf node of the query description,
/// which each target materializes as its own creation operator through the matching member
/// of <see cref="ISeqVisitor"/>; each method's own comment names its node and visitor member.
/// </remarks>
public static class Observable
{
    /// <summary>Describes <c>Observable.Timer(dueTime, scheduler)</c>.</summary>
    /// <param name="dueTime">Relative time at which to produce the value. If this value is less than or equal to TimeSpan.Zero, the timer will fire as soon as possible.</param>
    /// <param name="scheduler">Scheduler to run the timer on.</param>
    /// <remarks>Builds a <see cref="TimerSeq"/>, which each target materializes through <see cref="ISeqVisitor.Timer(TimerSeq)"/>.</remarks>
    public static Seq<long> Timer(TimeSpan dueTime, SchedulerRef scheduler) => new TimerSeq(dueTime, scheduler);

    /// <summary>Describes <c>Observable.Return(value)</c>.</summary>
    /// <typeparam name="T">The type of the element that will be returned in the produced sequence.</typeparam>
    /// <param name="value">Single element in the resulting observable sequence.</param>
    /// <remarks>Builds a <see cref="ReturnSeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.Return{T}(ReturnSeq{T})"/>.</remarks>
    public static Seq<T> Return<T>(T value) => new ReturnSeq<T>(value);

    /// <summary>Describes <c>Observable.Range(start, count)</c>.</summary>
    /// <param name="start">The value of the first integer in the sequence.</param>
    /// <param name="count">The number of sequential integers to generate.</param>
    /// <remarks>Builds a <see cref="RangeSeq"/>, which each target materializes through <see cref="ISeqVisitor.Range(RangeSeq)"/>.</remarks>
    public static Seq<int> Range(int start, int count) => new RangeSeq(start, count);

    /// <summary>Describes <c>Observable.Empty&lt;T&gt;()</c>.</summary>
    /// <typeparam name="T">The type used for the <see cref="IObservable{T}"/> type parameter of the resulting sequence.</typeparam>
    /// <remarks>Builds a <see cref="EmptySeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.Empty{T}(EmptySeq{T})"/>.</remarks>
    public static Seq<T> Empty<T>() => new EmptySeq<T>();

    /// <summary>Describes <c>Observable.Throw&lt;T&gt;(error)</c>.</summary>
    /// <typeparam name="T">The type used for the <see cref="IObservable{T}"/> type parameter of the resulting sequence.</typeparam>
    /// <param name="error">Exception object used for the sequence's termination.</param>
    /// <remarks>Builds a <see cref="ThrowSeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.Throw{T}(ThrowSeq{T})"/>.</remarks>
    public static Seq<T> Throw<T>(Exception error) => new ThrowSeq<T>(error, null);

    /// <summary>Describes <c>Observable.Throw&lt;T&gt;(error, scheduler)</c>.</summary>
    /// <typeparam name="T">The type used for the <see cref="IObservable{T}"/> type parameter of the resulting sequence.</typeparam>
    /// <param name="error">Exception object used for the sequence's termination.</param>
    /// <param name="scheduler">Scheduler to send the exceptional termination call on.</param>
    /// <remarks>Builds a <see cref="ThrowSeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.Throw{T}(ThrowSeq{T})"/>.</remarks>
    public static Seq<T> Throw<T>(Exception error, SchedulerRef scheduler) => new ThrowSeq<T>(error, scheduler);
}
