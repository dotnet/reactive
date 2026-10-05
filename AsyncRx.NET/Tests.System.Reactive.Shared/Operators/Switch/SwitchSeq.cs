// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>sources.Switch()</c> over a nested sequence.</summary>
public sealed class SwitchSeq<T>(Seq<Seq<T>> sources) : Seq<T>
{
    /// <summary>Observable sequence of inner observable sequences.</summary>
    public Seq<Seq<T>> Sources => sources;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Switch(this);

    /// <inheritdoc/>
    public override string ToString() => $"{sources}.Switch()";
}
