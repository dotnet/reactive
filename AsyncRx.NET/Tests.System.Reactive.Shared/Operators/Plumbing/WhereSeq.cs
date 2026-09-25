// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Where(predicate)</c>.</summary>
/// <remarks>Built by <see cref="SeqExtensions.Where{T}(Seq{T}, Func{T, bool}, string)"/>; materialized by each target through <see cref="ISeqVisitor.Where{T}(WhereSeq{T})"/>.</remarks>
public sealed class WhereSeq<T>(Seq<T> source, Func<T, bool> predicate, string text) : Seq<T>
{
    public Seq<T> Source => source;

    public Func<T, bool> Predicate => predicate;

    public override object Accept(ISeqVisitor visitor) => visitor.Where(this);

    public override string ToString() => $"{source}.Where({text})";
}
