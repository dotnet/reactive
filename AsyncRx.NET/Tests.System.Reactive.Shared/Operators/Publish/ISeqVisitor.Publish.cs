// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="PublishSeq{T}"/> as the target's own <c>Publish()</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Publish{T}(Seq{T})"/>. The realization is the target's
    /// connectable observable.
    /// </remarks>
    Realized<Seq<T>> Publish<T>(PublishSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="PublishInitialSeq{T}"/> as the target's own
    /// <c>Publish(initialValue)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Publish{T}(Seq{T}, T)"/>. The realization is the target's
    /// connectable observable.
    /// </remarks>
    Realized<Seq<T>> PublishInitial<T>(PublishInitialSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="PublishSelectorSeq{T, TResult}"/> as the target's own
    /// <c>Publish(selector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Publish{T, TResult}(Seq{T}, Func{Seq{T}, Seq{TResult}}, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> PublishSelector<T, TResult>(PublishSelectorSeq<T, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="PublishSelectorInitialSeq{T, TResult}"/> as the target's own
    /// <c>Publish(selector, initialValue)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.Publish{T, TResult}(Seq{T}, Func{Seq{T}, Seq{TResult}}, T, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> PublishSelectorInitial<T, TResult>(PublishSelectorInitialSeq<T, TResult> seq);
}
