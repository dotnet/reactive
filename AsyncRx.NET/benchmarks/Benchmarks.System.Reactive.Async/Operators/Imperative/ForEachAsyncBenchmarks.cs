// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Imperative
{
    /// <summary>
    /// S1 (variant sweep): <c>ForEachAsync</c>, the awaitable consumer, in its four forms: <c>Action</c>, indexed
    /// <c>Action</c>, <c>Func&lt;Task&gt;</c> and indexed <c>Func&lt;Task&gt;</c>. The callbacks are built in
    /// <c>[GlobalSetup]</c>.
    /// </summary>
    [BenchmarkCategory("Imperative")]
    public class ForEachAsyncBenchmarks : OperatorVariantBenchmarkBase
    {
        private Action<int> _action = default!;
        private Action<int, int> _actionIndexed = default!;
        private Func<int, Task> _func = default!;
        private Func<int, int, Task> _funcIndexed = default!;

        [GlobalSetup]
        public void Setup()
        {
            _action = v => Consumer.Consume(v);
            _actionIndexed = (v, i) => Consumer.Consume(v + i);
            _func = v =>
            {
                Consumer.Consume(v);
                return Task.CompletedTask;
            };
            _funcIndexed = (v, i) =>
            {
                Consumer.Consume(v + i);
                return Task.CompletedTask;
            };
        }

        [Benchmark]
        public Task ForEachAsync_Action() => BenchmarkSources.Range(N).ForEachAsync(_action);

        [Benchmark]
        public Task ForEachAsync_Action_Indexed() => BenchmarkSources.Range(N).ForEachAsync(_actionIndexed);

        [Benchmark]
        public Task ForEachAsync_Func() => BenchmarkSources.Range(N).ForEachAsync(_func);

        [Benchmark]
        public Task ForEachAsync_Func_Indexed() => BenchmarkSources.Range(N).ForEachAsync(_funcIndexed);
    }
}
