// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Aggregates
{
    /// <summary>
    /// S1 materialization into keyed collections: <c>ToDictionary</c> (one entry per unique key) and
    /// <c>ToLookup</c> (grouped by key). Both buffer the whole stream — allocation candidates.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class ToDictionaryToLookupBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void ToDictionary() => Observable.Range(1, N).ToDictionary(static v => v).SubscribeConsume(Consumer);

        [Benchmark]
        public void ToLookup() => Observable.Range(1, N).ToLookup(static v => v % 8).SubscribeConsume(Consumer);
    }
}
