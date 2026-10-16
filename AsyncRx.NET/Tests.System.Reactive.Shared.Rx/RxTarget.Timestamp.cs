// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<Timestamped<T>>> ISeqVisitor.Timestamp<T>(TimestampSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<Timestamped<T>>>(TimestampImpl<T>, seq.Source)
            : _bridge.Run<Seq<Timestamped<T>>>(
                TimestampOnImpl<T>, seq.Source, Unwrap(seq.Scheduler));

    private static IObservable<Timestamped<T>> TimestampImpl<T>(
        IObservable<T> source) =>
        source.Timestamp();

    private static IObservable<Timestamped<T>> TimestampOnImpl<T>(
        IObservable<T> source,
        IScheduler scheduler) =>
        source.Timestamp(scheduler);
}
