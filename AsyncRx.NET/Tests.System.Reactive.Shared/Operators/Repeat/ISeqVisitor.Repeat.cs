// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="RepeatSeq{T}"/> as the target's own <c>Repeat()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Repeat{T}(Seq{T})"/>.</remarks>
    Realized<Seq<T>> Repeat<T>(RepeatSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="RepeatCountSeq{T}"/> as the target's own
    /// <c>Repeat(repeatCount)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Repeat{T}(Seq{T}, int)"/>.</remarks>
    Realized<Seq<T>> RepeatCount<T>(RepeatCountSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="RepeatValueSeq{T}"/> as the target's own
    /// <c>Repeat(value[, scheduler])</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Repeat{T}(T)"/> and <see cref="Seq.Repeat{T}(T, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<T>> RepeatValue<T>(RepeatValueSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="RepeatValueCountSeq{T}"/> as the target's own
    /// <c>Repeat(value, repeatCount[, scheduler])</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Repeat{T}(T, int)"/> and
    /// <see cref="Seq.Repeat{T}(T, int, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<T>> RepeatValueCount<T>(RepeatValueCountSeq<T> seq);
}
