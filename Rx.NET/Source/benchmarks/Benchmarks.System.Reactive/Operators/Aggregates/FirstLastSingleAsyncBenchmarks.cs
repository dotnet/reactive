// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Aggregates
{
    /// <summary>
    /// S1: the non-blocking <c>*Async</c> element selectors (each returns a single-value observable).
    /// <c>First*</c> short-circuits on the first element; <c>Last*</c> drains the stream; <c>Single*</c> uses a
    /// predicate matching exactly one element (so it must scan the whole stream to confirm uniqueness).
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class FirstLastSingleAsyncBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void FirstAsync() => Observable.Range(1, N).FirstAsync().SubscribeConsume(Consumer);

        [Benchmark]
        public void FirstOrDefaultAsync() => Observable.Range(1, N).FirstOrDefaultAsync().SubscribeConsume(Consumer);

        [Benchmark]
        public void LastAsync() => Observable.Range(1, N).LastAsync().SubscribeConsume(Consumer);

        [Benchmark]
        public void LastOrDefaultAsync() => Observable.Range(1, N).LastOrDefaultAsync().SubscribeConsume(Consumer);

        [Benchmark]
        public void SingleAsync() => Observable.Range(1, N).SingleAsync(static v => v == 1).SubscribeConsume(Consumer);

        [Benchmark]
        public void SingleOrDefaultAsync() => Observable.Range(1, N).SingleOrDefaultAsync(static v => v == 1).SubscribeConsume(Consumer);
    }
}
