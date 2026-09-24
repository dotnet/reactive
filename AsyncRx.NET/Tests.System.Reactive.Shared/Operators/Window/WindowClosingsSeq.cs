// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public sealed class WindowClosingsSeq<T, TWindowClosing>(Seq<T> source, Func<Seq<TWindowClosing>> windowClosingSelector, string text) : Nested<T>
{
    public Seq<T> Source => source;

    public Func<Seq<TWindowClosing>> WindowClosingSelector => windowClosingSelector;

    public override object Accept(ISeqVisitor visitor) => visitor.WindowClosings(this);

    public override string ToString() => $"{source}.Window({text})";
}
