// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes an <see cref="PrependSeq{T}"/> as the target's own <c>Prepend(value)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Prepend{T}(Seq{T}, T)"/>.</remarks>
    Realized<Seq<T>> Prepend<T>(PrependSeq<T> seq);

    /// <summary>
    /// Materializes an <see cref="PrependSchedulerSeq{T}"/> as the target's own
    /// <c>Prepend(value, scheduler)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Prepend{T}(Seq{T}, T, SchedulerRef)"/>.</remarks>
    Realized<Seq<T>> PrependScheduler<T>(PrependSchedulerSeq<T> seq);
}
