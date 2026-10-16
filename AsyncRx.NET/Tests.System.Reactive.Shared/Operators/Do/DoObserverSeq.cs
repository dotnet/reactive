// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Do(observer)</c>.</summary>
/// <remarks>
/// Built by <see cref="DoExtensions.Do{T}(Seq{T}, ObserverRef{T}, string)"/>;
/// materialized by each target through <see cref="ISeqVisitor.DoObserver{T}(DoObserverSeq{T})"/>.
/// </remarks>
public sealed class DoObserverSeq<T>(
    Seq<T> source,
    ObserverRef<T> observer,
    string text)
    : Seq<T>
{
    /// <summary>Source sequence.</summary>
    public Seq<T> Source => source;

    /// <summary>
    /// Observer whose methods to invoke as part of the source sequence's observation.
    /// </summary>
    public ObserverRef<T> Observer => observer;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.DoObserver(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Do({text})";
}
