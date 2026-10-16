// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>source.SelectMany(selector)</c> for a task-returning selector.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="SelectManyExtensions.SelectMany{TIn, TOut}(Seq{TIn}, Func{TIn, Task{TOut}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.SelectManyTask{TIn, TOut}(SelectManyTaskSeq{TIn, TOut})"/>.
/// </remarks>
public sealed class SelectManyTaskSeq<TIn, TOut>(
    Seq<TIn> source,
    Func<TIn, Task<TOut>> selector,
    string text) : Seq<TOut>
{
    /// <summary>An observable sequence of elements to project.</summary>
    public Seq<TIn> Source => source;

    /// <summary>
    /// A transform function to apply to each element.
    /// </summary>
    public Func<TIn, Task<TOut>> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<Seq<TOut>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SelectManyTask(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SelectMany({text})";
}
