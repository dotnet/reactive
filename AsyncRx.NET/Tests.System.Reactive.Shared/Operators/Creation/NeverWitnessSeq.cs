// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Never(witness)</c>, the witness form.</summary>
/// <remarks>
/// Built by <see cref="Seq.Never{T}(T)"/>; materialized by each target through
/// <see cref="ISeqVisitor.NeverWitness{T}(NeverWitnessSeq{T})"/>, which calls the target's
/// witness overload, so that the overload itself is what a scenario over this node exercises.
/// </remarks>
public sealed class NeverWitnessSeq<T>(T witness) : Seq<T>
{
    /// <summary>Object solely used to infer the element type.</summary>
    public T Witness => witness;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.NeverWitness(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Never({witness})";
}
