// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="RepeatWhenSeq{T, TSignal}"/> as the target's own
    /// <c>source.RepeatWhen(handler)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="RepeatWhenExtensions.RepeatWhen{T, TSignal}"/>.</remarks>
    Realized<Seq<T>> RepeatWhen<T, TSignal>(RepeatWhenSeq<T, TSignal> seq);
}
