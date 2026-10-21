// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>sources.OnErrorResumeNext()</c> over an enumerable of sources.</summary>
/// <remarks>
/// Built by <see cref="Seq.OnErrorResumeNext{T}(IEnumerable{Seq{T}})"/>; materialized by each target
/// through <see cref="ISeqVisitor.OnErrorResumeNextEnumerable{T}(OnErrorResumeNextEnumerableSeq{T})"/>. The
/// enumerable is handed to the target lazily, so its own enumeration timing shows through.
/// </remarks>
public sealed class OnErrorResumeNextEnumerableSeq<T>(IEnumerable<Seq<T>> sources) : Seq<T>
{
    /// <summary>The sources, enumerated by the target.</summary>
    public IEnumerable<Seq<T>> Sources => sources;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.OnErrorResumeNextEnumerable(this);

    /// <inheritdoc/>
    public override string ToString() =>
        sources is ICollection<Seq<T>>
            ? $"Seq.OnErrorResumeNext([{string.Join(", ", sources)}])"
            : $"Seq.OnErrorResumeNext({sources.GetType().Name})";
}
