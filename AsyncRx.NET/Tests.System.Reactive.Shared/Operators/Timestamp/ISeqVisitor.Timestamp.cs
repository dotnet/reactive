// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="TimestampSeq{T}"/> as the target's own
    /// <c>Timestamp([scheduler])</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="TimestampExtensions.Timestamp{T}(Seq{T})"/> or
    /// <see cref="TimestampExtensions.Timestamp{T}(Seq{T}, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<Timestamped<T>>> Timestamp<T>(TimestampSeq<T> seq);
}
