// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// A description of an observable sequence of observable sequences of <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the inner sequences.</typeparam>
/// <remarks>
/// What <c>Window</c> and <c>GroupJoin</c> produce. The higher-kinded gap is confined to this one
/// type: it is not a <c>Seq&lt;Seq&lt;T&gt;&gt;</c>, because the neutral world never needs to hold
/// an inner sequence as an element value — only to describe what is done with one.
/// </remarks>
public abstract class Nested<T> : ISeq
{
    /// <inheritdoc/>
    public abstract object Accept(ISeqVisitor visitor);

    /// <summary>
    /// The description as the query was written in the scenario, for diagnostics.
    /// </summary>
    public abstract override string ToString();
}
