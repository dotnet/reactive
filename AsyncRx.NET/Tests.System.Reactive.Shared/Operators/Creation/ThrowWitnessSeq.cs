// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Throw(error, witness)</c> or <c>Seq.Throw(error, scheduler, witness)</c>,
/// the witness forms.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.Throw{T}(Exception, T)"/> or
/// <see cref="Seq.Throw{T}(Exception, SchedulerRef, T)"/>; materialized by each target
/// through <see cref="ISeqVisitor.ThrowWitness{T}(ThrowWitnessSeq{T})"/>, which calls the
/// target's witness overload, so that the overload itself is what a scenario over this node
/// exercises.
/// </remarks>
public sealed class ThrowWitnessSeq<T>(Exception error, SchedulerRef? scheduler, T witness)
    : Seq<T>
{
    /// <summary>Exception object used for the sequence's termination.</summary>
    public Exception Error => error;

    /// <summary>Scheduler to send the exceptional termination call on.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <summary>Object solely used to infer the element type.</summary>
    public T Witness => witness;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ThrowWitness(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"Seq.Throw({error.GetType().Name}, {witness})"
            : $"Seq.Throw({error.GetType().Name}, {scheduler}, {witness})";
}
