// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="RetryWhenSeq{T, TSignal}"/> as the target's own
    /// <c>source.RetryWhen(handler)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="RetryWhenExtensions.RetryWhen{T, TSignal}"/>.</remarks>
    Realized<Seq<T>> RetryWhen<T, TSignal>(RetryWhenSeq<T, TSignal> seq);
}
