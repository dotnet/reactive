// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="SubscribeOnSeq{T}"/> as the target's own
    /// <c>SubscribeOn(scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SubscribeOnExtensions.SubscribeOn{T}(Seq{T}, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<T>> SubscribeOn<T>(SubscribeOnSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="SubscribeOnContextSeq{T}"/> as the target's own
    /// <c>SubscribeOn(context)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SubscribeOnExtensions.SubscribeOn{T}(Seq{T}, SynchronizationContext)"/>.
    /// </remarks>
    Realized<Seq<T>> SubscribeOnContext<T>(SubscribeOnContextSeq<T> seq);
}
