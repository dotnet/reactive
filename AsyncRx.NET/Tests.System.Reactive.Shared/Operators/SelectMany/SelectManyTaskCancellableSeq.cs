// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>source.SelectMany(selector)</c> for a task-returning selector with a cancellation
/// token.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="SelectManyExtensions.SelectMany{TIn, TOut}(Seq{TIn}, Func{TIn, CancellationToken, Task{TOut}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.SelectManyTaskCancellable{TIn, TOut}(SelectManyTaskCancellableSeq{TIn, TOut})"/>.
/// </remarks>
public sealed class SelectManyTaskCancellableSeq<TIn, TOut>(
    Seq<TIn> source,
    Func<TIn, CancellationToken, Task<TOut>> selector,
    string text) : Seq<TOut>
{
    /// <summary>An observable sequence of elements to project.</summary>
    public Seq<TIn> Source => source;

    /// <summary>
    /// A transform function to apply to each element; the token it receives is cancelled when
    /// the subscription is disposed.
    /// </summary>
    public Func<TIn, CancellationToken, Task<TOut>> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<Seq<TOut>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SelectManyTaskCancellable(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SelectMany({text})";
}
