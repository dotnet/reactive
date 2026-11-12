// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Using(resourceFactory, observableFactory)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="Seq.Using{T, TResource}(Func{TResource}, Func{TResource, Seq{T}}, string, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.Using{T, TResource}(UsingSeq{T, TResource})"/>. The observable
/// factory returns a description, which the target materializes at the moment it invokes the
/// factory, so a factory that throws, throws inside the target's own call.
/// </remarks>
public sealed class UsingSeq<T, TResource>(
    Func<TResource> resourceFactory,
    Func<TResource, Seq<T>> observableFactory,
    string resourceText,
    string observableText)
    : Seq<T>
    where TResource : IDisposable
{
    /// <summary>Factory function to obtain a resource object.</summary>
    public Func<TResource> ResourceFactory => resourceFactory;

    /// <summary>
    /// Factory function to obtain an observable sequence that depends on the obtained resource.
    /// </summary>
    public Func<TResource, Seq<T>> ObservableFactory => observableFactory;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Using(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Using({resourceText}, {observableText})";
}
