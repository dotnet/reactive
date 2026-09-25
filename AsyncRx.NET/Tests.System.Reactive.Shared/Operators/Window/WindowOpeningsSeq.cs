// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Window(windowOpenings, windowClosingSelector)</c>.</summary>
/// <remarks>Built by <see cref="WindowExtensions.Window{T, TWindowOpening, TWindowClosing}(Seq{T}, Seq{TWindowOpening}, Func{TWindowOpening, Seq{TWindowClosing}}, string)"/>; materialized by each target through <see cref="ISeqVisitor.WindowOpenings{T, TWindowOpening, TWindowClosing}(WindowOpeningsSeq{T, TWindowOpening, TWindowClosing})"/>.</remarks>
public sealed class WindowOpeningsSeq<T, TWindowOpening, TWindowClosing>(Seq<T> source, Seq<TWindowOpening> windowOpenings, Func<TWindowOpening, Seq<TWindowClosing>> windowClosingSelector, string text) : Nested<T>
{
    public Seq<T> Source => source;

    public Seq<TWindowOpening> WindowOpenings => windowOpenings;

    public Func<TWindowOpening, Seq<TWindowClosing>> WindowClosingSelector => windowClosingSelector;

    public override object Accept(ISeqVisitor visitor) => visitor.WindowOpenings(this);

    public override string ToString() => $"{source}.Window({windowOpenings}, {text})";
}
