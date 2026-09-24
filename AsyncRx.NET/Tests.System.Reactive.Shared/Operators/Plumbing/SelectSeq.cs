// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public sealed class SelectSeq<TIn, TOut>(Seq<TIn> source, Func<TIn, TOut> selector, string text) : Seq<TOut>
{
    public Seq<TIn> Source => source;

    public Func<TIn, TOut> Selector => selector;

    public override object Accept(ISeqVisitor visitor) => visitor.Select(this);

    public override string ToString() => $"{source}.Select({text})";
}
