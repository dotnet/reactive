// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Aggregate<T>(AggregateSeq<T> seq) =>
        _bridge.Run<Seq<T>>(AggregateImpl<T>, seq.Source, seq.Accumulator);

    Realized<Seq<TAccumulate>> ISeqVisitor.AggregateSeed<T, TAccumulate>(
        AggregateSeedSeq<T, TAccumulate> seq) =>
        _bridge.Run<Seq<TAccumulate>>(
            AggregateSeedImpl<T, TAccumulate>, seq.Source, seq.Seed, seq.Accumulator);

    Realized<Seq<TResult>> ISeqVisitor.AggregateSeedResult<T, TAccumulate, TResult>(
        AggregateSeedResultSeq<T, TAccumulate, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            AggregateSeedResultImpl<T, TAccumulate, TResult>,
            seq.Source,
            seq.Seed,
            seq.Accumulator,
            seq.ResultSelector);

    private static IAsyncObservable<T> AggregateImpl<T>(
        IAsyncObservable<T> source,
        Func<T, T, T> accumulator) =>
        source.Aggregate(accumulator);

    private static IAsyncObservable<TAccumulate> AggregateSeedImpl<T, TAccumulate>(
        IAsyncObservable<T> source,
        TAccumulate seed,
        Func<TAccumulate, T, TAccumulate> accumulator) =>
        source.Aggregate(seed, accumulator);

    private static IAsyncObservable<TResult> AggregateSeedResultImpl<T, TAccumulate, TResult>(
        IAsyncObservable<T> source,
        TAccumulate seed,
        Func<TAccumulate, T, TAccumulate> accumulator,
        Func<TAccumulate, TResult> resultSelector) =>
        source.Aggregate(seed, accumulator, resultSelector);
}
