// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public static partial class Seq
{
    /// <summary>Describes a sequence whose subscribe call throws.</summary>
    /// <typeparam name="T">The type of the elements the sequence would carry.</typeparam>
    /// <param name="error">The exception thrown by every subscribe call.</param>
    /// <remarks>
    /// Where the Rx.NET test writes its <c>RogueObservable</c>. Builds a
    /// <see cref="ThrowOnSubscribeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ThrowOnSubscribe{T}(ThrowOnSubscribeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> ThrowOnSubscribe<T>(Exception error) => new ThrowOnSubscribeSeq<T>(error);
}
