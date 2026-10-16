// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>source.SelectMany(taskSelector, resultSelector)</c> for a task-returning selector
/// with a cancellation token.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="SelectManyExtensions.SelectMany{TIn, TTask, TOut}(Seq{TIn}, Func{TIn, CancellationToken, Task{TTask}}, Func{TIn, TTask, TOut}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.SelectManyTaskResultCancellable{TIn, TTask, TOut}(SelectManyTaskResultCancellableSeq{TIn, TTask, TOut})"/>.
/// </remarks>
public sealed class SelectManyTaskResultCancellableSeq<TIn, TTask, TOut>(
    Seq<TIn> source,
    Func<TIn, CancellationToken, Task<TTask>> taskSelector,
    Func<TIn, TTask, TOut> resultSelector,
    string text) : Seq<TOut>
{
    /// <summary>An observable sequence of elements to project.</summary>
    public Seq<TIn> Source => source;

    /// <summary>
    /// A transform function to apply to each element; the token it receives is cancelled when
    /// the subscription is disposed.
    /// </summary>
    public Func<TIn, CancellationToken, Task<TTask>> TaskSelector => taskSelector;

    /// <summary>
    /// A transform function to apply to each task's result.
    /// </summary>
    public Func<TIn, TTask, TOut> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TOut>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SelectManyTaskResultCancellable(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SelectMany({text})";
}
