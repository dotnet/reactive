// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Imperative
{
    /// <summary>
    /// Imperative: <c>Case</c> selects a source from a key→source map at subscription time; <c>ForEachAsync</c> is
    /// the Task-returning iteration, waited on to completion inside each invocation (so the default job applies).
    /// </summary>
    [BenchmarkCategory("Imperative")]
    public class CaseForEachAsyncBenchmarks : OperatorBenchmarkBase
    {
        private Dictionary<int, IObservable<int>> _cases = default!;

        [GlobalSetup]
        public void Setup() => _cases = new Dictionary<int, IObservable<int>> { [0] = Observable.Range(1, N) };

        [Benchmark]
        public void Case() => Observable.Case(static () => 0, _cases).SubscribeConsume(Consumer);

        [Benchmark]
        public void ForEachAsync() => Observable.Range(1, N).ForEachAsync(v => Consumer.Consume(v)).GetAwaiter().GetResult();
    }
}
