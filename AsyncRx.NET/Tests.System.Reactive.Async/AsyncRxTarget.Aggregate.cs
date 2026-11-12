// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Aggregate<T>(AggregateSeq<T> seq) =>
        RunByForm<Seq<T>>(AggregateImpl<T>, AggregateAsyncImpl<T>, seq.Source, seq.Accumulator);

    Realized<Seq<TAccumulate>> ISeqVisitor.AggregateSeed<T, TAccumulate>(
        AggregateSeedSeq<T, TAccumulate> seq) =>
        RunByForm<Seq<TAccumulate>>(
            AggregateSeedImpl<T, TAccumulate>, AggregateSeedAsyncImpl<T, TAccumulate>, seq.Source,
            seq.Seed, seq.Accumulator);

    Realized<Seq<TResult>> ISeqVisitor.AggregateSeedResult<T, TAccumulate, TResult>(
        AggregateSeedResultSeq<T, TAccumulate, TResult> seq) =>
        RunByForm<Seq<TResult>>(
            AggregateSeedResultImpl<T, TAccumulate, TResult>,
            AggregateSeedResultAsyncImpl<T, TAccumulate, TResult>, seq.Source, seq.Seed,
            seq.Accumulator, seq.ResultSelector);

    private static IAsyncObservable<T> AggregateImpl<T>(
        IAsyncObservable<T> source,
        Func<T, T, T> accumulator) =>
        source.Aggregate(accumulator);

    private static IAsyncObservable<T> AggregateAsyncImpl<T>(
        IAsyncObservable<T> source,
        Func<T, T, ValueTask<T>> accumulator) =>
        source.Aggregate(accumulator);

    private static IAsyncObservable<TAccumulate> AggregateSeedImpl<T, TAccumulate>(
        IAsyncObservable<T> source,
        TAccumulate seed,
        Func<TAccumulate, T, TAccumulate> accumulator) =>
        source.Aggregate(seed, accumulator);

    private static IAsyncObservable<TAccumulate> AggregateSeedAsyncImpl<T, TAccumulate>(
        IAsyncObservable<T> source,
        TAccumulate seed,
        Func<TAccumulate, T, ValueTask<TAccumulate>> accumulator) =>
        source.Aggregate(seed, accumulator);

    private static IAsyncObservable<TResult> AggregateSeedResultImpl<T, TAccumulate, TResult>(
        IAsyncObservable<T> source,
        TAccumulate seed,
        Func<TAccumulate, T, TAccumulate> accumulator,
        Func<TAccumulate, TResult> resultSelector) =>
        source.Aggregate(seed, accumulator, resultSelector);

    private static IAsyncObservable<TResult> AggregateSeedResultAsyncImpl<T, TAccumulate, TResult>(
        IAsyncObservable<T> source,
        TAccumulate seed,
        Func<TAccumulate, T, ValueTask<TAccumulate>> accumulator,
        Func<TAccumulate, ValueTask<TResult>> resultSelector) =>
        source.Aggregate(seed, accumulator, resultSelector);
}
