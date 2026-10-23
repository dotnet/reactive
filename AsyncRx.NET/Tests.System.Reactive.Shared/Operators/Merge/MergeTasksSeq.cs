// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>sources.Merge()</c> over a sequence of tasks.</summary>
/// <remarks>
/// Built by <see cref="Seq.Merge{T}(Seq{Task{T}})"/>; materialized by each target through
/// <see cref="ISeqVisitor.MergeTasks{T}(MergeTasksSeq{T})"/>.
/// </remarks>
public sealed class MergeTasksSeq<T>(Seq<Task<T>> sources) : Seq<T>
{
    /// <summary>Observable sequence of tasks.</summary>
    public Seq<Task<T>> Sources => sources;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.MergeTasks(this);

    /// <inheritdoc/>
    public override string ToString() => $"{sources}.Merge()";
}
