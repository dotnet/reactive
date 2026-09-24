// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public sealed class DelaySelectorSeq<T, TDelay>(Seq<T> source, Func<T, Seq<TDelay>> delayDurationSelector, string text) : Seq<T>
{
    public Seq<T> Source => source;

    public Func<T, Seq<TDelay>> DelayDurationSelector => delayDurationSelector;

    public override object Accept(ISeqVisitor visitor) => visitor.DelaySelector(this);

    public override string ToString() => $"{source}.Delay({text})";
}
