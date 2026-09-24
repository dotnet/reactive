// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public sealed class SelectIndexedSeq<TIn, TOut>(Seq<TIn> source, Func<TIn, int, TOut> selector, string text) : Seq<TOut>
{
    public Seq<TIn> Source => source;

    public Func<TIn, int, TOut> Selector => selector;

    public override object Accept(ISeqVisitor visitor) => visitor.SelectIndexed(this);

    public override string ToString() => $"{source}.Select({text})";
}
