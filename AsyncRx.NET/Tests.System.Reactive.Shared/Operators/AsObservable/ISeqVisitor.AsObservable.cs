// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes an <see cref="AsObservableSeq{T}"/> as the target's own
    /// <c>AsObservable()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="AsObservableExtensions.AsObservable{T}(Seq{T})"/>.</remarks>
    Realized<Seq<T>> AsObservable<T>(AsObservableSeq<T> seq);
}
