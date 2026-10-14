// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>Materializes a <see cref="RetrySeq{T}"/> as the target's own <c>Retry()</c>.</summary>
    /// <remarks>Built by <see cref="RetryExtensions.Retry{T}(Seq{T})"/>.</remarks>
    Realized<Seq<T>> Retry<T>(RetrySeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="RetryCountSeq{T}"/> as the target's own <c>Retry(retryCount)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="RetryExtensions.Retry{T}(Seq{T}, int)"/>.</remarks>
    Realized<Seq<T>> RetryCount<T>(RetryCountSeq<T> seq);
}
