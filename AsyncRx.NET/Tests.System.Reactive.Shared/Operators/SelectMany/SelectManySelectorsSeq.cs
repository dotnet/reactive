// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>source.SelectMany(onNext, onError, onCompleted)</c>.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="SelectManyExtensions.SelectMany{TIn, TOut}(Seq{TIn}, Func{TIn, Seq{TOut}}, Func{Exception, Seq{TOut}}, Func{Seq{TOut}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.SelectManySelectors{TIn, TOut}(SelectManySelectorsSeq{TIn, TOut})"/>.
/// </remarks>
public sealed class SelectManySelectorsSeq<TIn, TOut>(
    Seq<TIn> source,
    Func<TIn, Seq<TOut>> onNext,
    Func<Exception, Seq<TOut>> onError,
    Func<Seq<TOut>> onCompleted,
    string text) : Seq<TOut>
{
    /// <summary>An observable sequence of notifications to project.</summary>
    public Seq<TIn> Source => source;

    /// <summary>
    /// A transform function to apply to each element.
    /// </summary>
    public Func<TIn, Seq<TOut>> OnNext => onNext;

    /// <summary>
    /// A transform function to apply when an error occurs in the source sequence.
    /// </summary>
    public Func<Exception, Seq<TOut>> OnError => onError;

    /// <summary>
    /// A transform function to apply when the end of the source sequence is reached.
    /// </summary>
    public Func<Seq<TOut>> OnCompleted => onCompleted;

    /// <inheritdoc/>
    protected override Realized<Seq<TOut>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SelectManySelectors(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SelectMany({text}, ...)";
}
