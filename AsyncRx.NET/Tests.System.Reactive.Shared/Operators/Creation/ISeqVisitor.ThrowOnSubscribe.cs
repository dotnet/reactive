// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="ThrowOnSubscribeSeq{T}"/> as the target's own raw observable
    /// whose subscribe call throws.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.ThrowOnSubscribe{T}(Exception)"/>.</remarks>
    Realized<Seq<T>> ThrowOnSubscribe<T>(ThrowOnSubscribeSeq<T> seq);
}
