// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Catch(source1, source2, ...)</c> over any number of sources.</summary>
/// <remarks>
/// Built by <see cref="Seq.Catch{T}(Seq{T}[])"/>, the <c>params</c> form; materialized by each
/// target through <see cref="ISeqVisitor.CatchParams{T}(CatchParamsSeq{T})"/>.
/// </remarks>
public sealed class CatchParamsSeq<T>(Seq<T>[] sources) : Seq<T>
{
    /// <summary>Observable sequences to catch exceptions for.</summary>
    public Seq<T>[] Sources => sources;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.CatchParams(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"Seq.Catch({string.Join(", ", sources.Select(s => s.ToString()))})";
}
