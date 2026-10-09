// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes an <see cref="ObserveOnSeq{T}"/> as the target's own
    /// <c>ObserveOn(scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="ObserveOnExtensions.ObserveOn{T}(Seq{T}, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<T>> ObserveOn<T>(ObserveOnSeq<T> seq);

    /// <summary>
    /// Materializes an <see cref="ObserveOnContextSeq{T}"/> as the target's own
    /// <c>ObserveOn(context)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ObserveOnExtensions.ObserveOn{T}(Seq{T}, SynchronizationContext)"/>.
    /// </remarks>
    Realized<Seq<T>> ObserveOnContext<T>(ObserveOnContextSeq<T> seq);
}
