// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<Timestamped<T>>> ISeqVisitor.Timestamp<T>(TimestampSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<Timestamped<T>>>(TimestampImpl<T>, seq.Source)
            : _bridge.Run<Seq<Timestamped<T>>>(
                TimestampOnImpl<T>, seq.Source, Unwrap(seq.Scheduler));

    private static IAsyncObservable<Timestamped<T>> TimestampImpl<T>(
        IAsyncObservable<T> source) =>
        source.Timestamp();

    private static IAsyncObservable<Timestamped<T>> TimestampOnImpl<T>(
        IAsyncObservable<T> source,
        IClock clock) =>
        source.Timestamp(clock);
}
