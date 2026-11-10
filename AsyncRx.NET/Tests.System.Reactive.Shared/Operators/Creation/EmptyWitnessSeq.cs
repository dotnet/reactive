// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Empty(witness)</c> or <c>Seq.Empty(scheduler, witness)</c>, the witness
/// forms.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.Empty{T}(T)"/> or <see cref="Seq.Empty{T}(SchedulerRef, T)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.EmptyWitness{T}(EmptyWitnessSeq{T})"/>, which calls the target's
/// witness overload, so that the overload itself is what a scenario over this node exercises.
/// </remarks>
public sealed class EmptyWitnessSeq<T>(SchedulerRef? scheduler, T witness) : Seq<T>
{
    /// <summary>Scheduler to send the termination call on.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <summary>Object solely used to infer the element type.</summary>
    public T Witness => witness;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.EmptyWitness(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null ? $"Seq.Empty({witness})" : $"Seq.Empty({scheduler}, {witness})";
}
