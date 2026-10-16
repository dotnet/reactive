// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="PublishLastSeq{T}"/> as the target's own <c>PublishLast()</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="PublishLastExtensions.PublishLast{T}(Seq{T})"/>. The realization is
    /// the target's connectable observable.
    /// </remarks>
    Realized<Seq<T>> PublishLast<T>(PublishLastSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="PublishLastSelectorSeq{T, TResult}"/> as the target's own
    /// <c>PublishLast(selector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="PublishLastExtensions.PublishLast{T, TResult}(Seq{T}, Func{Seq{T}, Seq{TResult}}, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> PublishLastSelector<T, TResult>(PublishLastSelectorSeq<T, TResult> seq);
}
