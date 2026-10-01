// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<long>> ISeqVisitor.Timer(TimerSeq seq) =>
        Realized.Of<Seq<long>>(Observable.Timer(seq.DueTime, Unwrap(seq.Scheduler)));

    Realized<Seq<T>> ISeqVisitor.Return<T>(ReturnSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ReturnImpl<T>, seq.Value);

    Realized<Seq<int>> ISeqVisitor.Range(RangeSeq seq) =>
        Realized.Of<Seq<int>>(Observable.Range(seq.Start, seq.Count));

    Realized<Seq<T>> ISeqVisitor.Empty<T>(EmptySeq<T> seq) => _bridge.Run<Seq<T>>(EmptyImpl<T>);

    Realized<Seq<T>> ISeqVisitor.Throw<T>(ThrowSeq<T> seq) =>
        seq.Scheduler is null ? _bridge.Run<Seq<T>>(
            ThrowImpl<T>,
            seq.Error) : _bridge.Run<Seq<T>>(ThrowOnImpl<T>,
            seq.Error,
            Unwrap(seq.Scheduler));

    private static IObservable<T> ReturnImpl<T>(T value) => Observable.Return(value);

    private static IObservable<T> EmptyImpl<T>() => Observable.Empty<T>();

    private static IObservable<T> ThrowImpl<T>(Exception error) => Observable.Throw<T>(error);

    private static IObservable<T> ThrowOnImpl<T>(Exception error, IScheduler scheduler) =>
        Observable.Throw<T>(error, scheduler);
}
