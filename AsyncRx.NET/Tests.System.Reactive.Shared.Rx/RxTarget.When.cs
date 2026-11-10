// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Joins;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<TResult>> ISeqVisitor.When<TResult>(WhenSeq<TResult> seq) =>
        _bridge.Run<Seq<TResult>>(WhenImpl<TResult>, [seq.Plans]);

    Realized<Seq<TResult>> ISeqVisitor.WhenEnumerable<TResult>(WhenEnumerableSeq<TResult> seq) =>
        _bridge.Run<Seq<TResult>>(WhenEnumerableImpl<TResult>, seq.Plans);

    private static IObservable<TResult> WhenImpl<TResult>(Plan<TResult>[] plans) =>
        Observable.When(plans);

    private static IObservable<TResult> WhenEnumerableImpl<TResult>(
        IEnumerable<Plan<TResult>> plans) =>
        plans.When();
}
