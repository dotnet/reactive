// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="ScanSeq{T}"/> as the target's own <c>Scan(accumulator)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="ScanExtensions.Scan{T}(Seq{T}, Func{T, T, T}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> Scan<T>(ScanSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ScanSeedSeq{T, TAccumulate}"/> as the target's own
    /// <c>Scan(seed, accumulator)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ScanExtensions.Scan{T, TAccumulate}(Seq{T}, TAccumulate, Func{TAccumulate, T, TAccumulate}, string)"/>.
    /// </remarks>
    Realized<Seq<TAccumulate>> ScanSeed<T, TAccumulate>(ScanSeedSeq<T, TAccumulate> seq);
}
