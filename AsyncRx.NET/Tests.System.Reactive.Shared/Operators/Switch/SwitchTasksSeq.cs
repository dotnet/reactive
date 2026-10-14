// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>sources.Switch()</c> over a sequence of tasks.</summary>
/// <remarks>
/// Built by <see cref="SwitchExtensions.Switch{T}(Seq{Task{T}})"/>; materialized by each target
/// through <see cref="ISeqVisitor.SwitchTasks{T}(SwitchTasksSeq{T})"/>. A task-valued element is
/// the natural form on both targets, as it is for <c>Concat</c> and <c>Merge</c>.
/// </remarks>
public sealed class SwitchTasksSeq<T>(Seq<Task<T>> sources) : Seq<T>
{
    /// <summary>Observable sequence of tasks.</summary>
    public Seq<Task<T>> Sources => sources;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SwitchTasks(this);

    /// <inheritdoc/>
    public override string ToString() => $"{sources}.Switch()";
}
