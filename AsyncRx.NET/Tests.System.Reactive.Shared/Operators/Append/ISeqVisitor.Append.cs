// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes an <see cref="AppendSeq{T}"/> as the target's own <c>Append(value)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Append{T}(Seq{T}, T)"/>.</remarks>
    Realized<Seq<T>> Append<T>(AppendSeq<T> seq);

    /// <summary>
    /// Materializes an <see cref="AppendSchedulerSeq{T}"/> as the target's own
    /// <c>Append(value, scheduler)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Append{T}(Seq{T}, T, SchedulerRef)"/>.</remarks>
    Realized<Seq<T>> AppendScheduler<T>(AppendSchedulerSeq<T> seq);
}
