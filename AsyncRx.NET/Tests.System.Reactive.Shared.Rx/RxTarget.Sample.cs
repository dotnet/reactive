// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.SampleTime<T>(SampleTimeSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(SampleTimeDefaultImpl<T>, seq.Source, seq.Interval)
            : _bridge.Run<Seq<T>>(
                SampleTimeImpl<T>, seq.Source, seq.Interval, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.SampleSampler<T, TSample>(SampleSamplerSeq<T, TSample> seq) =>
        _bridge.Run<Seq<T>>(SampleSamplerImpl<T, TSample>, seq.Source, seq.Sampler);

    private static IObservable<T> SampleTimeImpl<T>(
        IObservable<T> source,
        TimeSpan interval,
        IScheduler scheduler) =>
        source.Sample(interval, scheduler);

    private static IObservable<T> SampleTimeDefaultImpl<T>(
        IObservable<T> source,
        TimeSpan interval) =>
        source.Sample(interval);

    private static IObservable<T> SampleSamplerImpl<T, TSample>(
        IObservable<T> source,
        IObservable<TSample> sampler) =>
        source.Sample(sampler);
}
