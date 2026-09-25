// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Window(windowClosingSelector)</c>.</summary>
/// <remarks>Built by <see cref="WindowExtensions.Window{T, TWindowClosing}(Seq{T}, Func{Seq{TWindowClosing}}, string)"/>; materialized by each target through <see cref="ISeqVisitor.WindowClosings{T, TWindowClosing}(WindowClosingsSeq{T, TWindowClosing})"/>.</remarks>
public sealed class WindowClosingsSeq<T, TWindowClosing>(Seq<T> source, Func<Seq<TWindowClosing>> windowClosingSelector, string text) : Nested<T>
{
    public Seq<T> Source => source;

    public Func<Seq<TWindowClosing>> WindowClosingSelector => windowClosingSelector;

    public override object Accept(ISeqVisitor visitor) => visitor.WindowClosings(this);

    public override string ToString() => $"{source}.Window({text})";
}
