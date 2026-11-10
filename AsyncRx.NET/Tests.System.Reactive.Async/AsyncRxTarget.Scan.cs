// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Scan<T>(ScanSeq<T> seq) =>
        RunByForm<Seq<T>>(ScanImpl<T>, ScanAsyncImpl<T>, seq.Source, seq.Accumulator);

    Realized<Seq<TAccumulate>> ISeqVisitor.ScanSeed<T, TAccumulate>(
        ScanSeedSeq<T, TAccumulate> seq) =>
        RunByForm<Seq<TAccumulate>>(
            ScanSeedImpl<T, TAccumulate>, ScanSeedAsyncImpl<T, TAccumulate>, seq.Source, seq.Seed,
            seq.Accumulator);

    private static IAsyncObservable<T> ScanImpl<T>(
        IAsyncObservable<T> source,
        Func<T, T, T> accumulator) =>
        source.Scan(accumulator);

    private static IAsyncObservable<T> ScanAsyncImpl<T>(
        IAsyncObservable<T> source,
        Func<T, T, ValueTask<T>> accumulator) =>
        source.Scan(accumulator);

    private static IAsyncObservable<TAccumulate> ScanSeedImpl<T, TAccumulate>(
        IAsyncObservable<T> source,
        TAccumulate seed,
        Func<TAccumulate, T, TAccumulate> accumulator) =>
        source.Scan(seed, accumulator);

    private static IAsyncObservable<TAccumulate> ScanSeedAsyncImpl<T, TAccumulate>(
        IAsyncObservable<T> source,
        TAccumulate seed,
        Func<TAccumulate, T, ValueTask<TAccumulate>> accumulator) =>
        source.Scan(seed, accumulator);
}
