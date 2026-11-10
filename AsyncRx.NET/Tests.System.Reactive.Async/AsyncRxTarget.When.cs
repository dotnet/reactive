// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Joins;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<TResult>> ISeqVisitor.When<TResult>(WhenSeq<TResult> seq) =>
        _bridge.Run<Seq<TResult>>(WhenImpl<TResult>, [seq.Plans]);

    Realized<Seq<TResult>> ISeqVisitor.WhenEnumerable<TResult>(WhenEnumerableSeq<TResult> seq) =>
        _bridge.Run<Seq<TResult>>(WhenEnumerableImpl<TResult>, seq.Plans);

    private static IAsyncObservable<TResult> WhenImpl<TResult>(AsyncPlan<TResult>[] plans) =>
        AsyncObservable.When(plans);

    private static IAsyncObservable<TResult> WhenEnumerableImpl<TResult>(
        IEnumerable<AsyncPlan<TResult>> plans) =>
        AsyncObservable.When(plans);
}
