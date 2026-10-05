// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="RefCountSeq{T}"/> as the target's own <c>RefCount()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="RefCountExtensions.RefCount{T}(ConnectableSeq{T})"/>.</remarks>
    Realized<Seq<T>> RefCount<T>(RefCountSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="RefCountMinObserversSeq{T}"/> as the target's own
    /// <c>RefCount(minObservers)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="RefCountExtensions.RefCount{T}(ConnectableSeq{T}, int)"/>.
    /// </remarks>
    Realized<Seq<T>> RefCountMinObservers<T>(RefCountMinObserversSeq<T> seq);
}
