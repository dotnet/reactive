// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Append<T>(AppendSeq<T> seq) =>
        _bridge.Run<Seq<T>>(AppendImpl<T>, seq.Source, seq.Value);

    Realized<Seq<T>> ISeqVisitor.AppendScheduler<T>(AppendSchedulerSeq<T> seq) =>
        _bridge.Run<Seq<T>>(AppendSchedulerImpl<T>, seq.Source, seq.Value, Unwrap(seq.Scheduler));

    private static IObservable<T> AppendImpl<T>(IObservable<T> source, T value) =>
        source.Append(value);

    private static IObservable<T> AppendSchedulerImpl<T>(
        IObservable<T> source,
        T value,
        IScheduler scheduler) =>
        source.Append(value, scheduler);
}
