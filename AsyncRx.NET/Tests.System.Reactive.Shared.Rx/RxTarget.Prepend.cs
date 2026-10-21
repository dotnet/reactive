// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Prepend<T>(PrependSeq<T> seq) =>
        _bridge.Run<Seq<T>>(PrependImpl<T>, seq.Source, seq.Value);

    Realized<Seq<T>> ISeqVisitor.PrependScheduler<T>(PrependSchedulerSeq<T> seq) =>
        _bridge.Run<Seq<T>>(PrependSchedulerImpl<T>, seq.Source, seq.Value, Unwrap(seq.Scheduler));

    private static IObservable<T> PrependImpl<T>(IObservable<T> source, T value) =>
        source.Prepend(value);

    private static IObservable<T> PrependSchedulerImpl<T>(
        IObservable<T> source,
        T value,
        IScheduler scheduler) =>
        source.Prepend(value, scheduler);
}
