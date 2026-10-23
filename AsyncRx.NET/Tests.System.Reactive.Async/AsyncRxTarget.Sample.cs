// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.SampleTime<T>(SampleTimeSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(SampleTimeDefaultImpl<T>, seq.Source, seq.Interval)
            : _bridge.Run<Seq<T>>(
                SampleTimeImpl<T>, seq.Source, seq.Interval, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.SampleSampler<T, TSample>(SampleSamplerSeq<T, TSample> seq) =>
        _bridge.Run<Seq<T>>(SampleSamplerImpl<T, TSample>, seq.Source, seq.Sampler);

    private static IAsyncObservable<T> SampleTimeImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan interval,
        IAsyncScheduler scheduler) =>
        source.Sample(interval, scheduler);

    private static IAsyncObservable<T> SampleTimeDefaultImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan interval) =>
        source.Sample(interval);

    private static IAsyncObservable<T> SampleSamplerImpl<T, TSample>(
        IAsyncObservable<T> source,
        IAsyncObservable<TSample> sampler) =>
        source.Sample(sampler);
}
