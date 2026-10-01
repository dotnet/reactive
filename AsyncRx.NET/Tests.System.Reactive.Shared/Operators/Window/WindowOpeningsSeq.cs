// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Window(windowOpenings, windowClosingSelector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="WindowExtensions.Window{T, TWindowOpening, TWindowClosing}(Seq{T}, Seq{TWindowOpening}, Func{TWindowOpening, Seq{TWindowClosing}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.WindowOpenings{T, TWindowOpening, TWindowClosing}(WindowOpeningsSeq{T, TWindowOpening, TWindowClosing})"/>.
/// </remarks>
public sealed class WindowOpeningsSeq<T, TWindowOpening, TWindowClosing>(Seq<T> source, Seq<TWindowOpening> windowOpenings, Func<TWindowOpening, Seq<TWindowClosing>> windowClosingSelector, string text) : Seq<Seq<T>>
{
    /// <summary>Source sequence to produce windows over.</summary>
    public Seq<T> Source => source;

    /// <summary>Observable sequence whose elements denote the creation of new windows.</summary>
    public Seq<TWindowOpening> WindowOpenings => windowOpenings;

    /// <summary>A function invoked to define the closing of each produced window.</summary>
    public Func<TWindowOpening, Seq<TWindowClosing>> WindowClosingSelector => windowClosingSelector;

    /// <inheritdoc/>
    public override Realized<Seq<Seq<T>>> Accept(ISeqVisitor visitor) =>
        visitor.WindowOpenings(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Window({windowOpenings}, {text})";
}
