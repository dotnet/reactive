// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Append<T>(AppendSeq<T> seq) =>
        _bridge.Run<Seq<T>>(AppendImpl<T>, seq.Source, seq.Value);

    Realized<Seq<T>> ISeqVisitor.AppendScheduler<T>(AppendSchedulerSeq<T> seq) =>
        _bridge.Run<Seq<T>>(AppendSchedulerImpl<T>, seq.Source, seq.Value, Unwrap(seq.Scheduler));

    private static IAsyncObservable<T> AppendImpl<T>(IAsyncObservable<T> source, T value) =>
        source.Append(value);

    private static IAsyncObservable<T> AppendSchedulerImpl<T>(
        IAsyncObservable<T> source,
        T value,
        IAsyncScheduler scheduler) =>
        source.Append(value, scheduler);
}
