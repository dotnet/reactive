// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="MaterializeSeq{T}"/> as the target's own <c>Materialize()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="MaterializeExtensions.Materialize{T}(Seq{T})"/>.</remarks>
    Realized<Seq<Notification<T>>> Materialize<T>(MaterializeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="DematerializeSeq{T}"/> as the target's own
    /// <c>Dematerialize()</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="MaterializeExtensions.Dematerialize{T}(Seq{Notification{T}})"/>.
    /// </remarks>
    Realized<Seq<T>> Dematerialize<T>(DematerializeSeq<T> seq);
}
