// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>
    /// S1: <c>SelectMany</c> with a <c>Task</c>-returning selector, which runs the <c>TaskSelector</c> sink (a
    /// completed-task fast path that forwards the result inline, otherwise a continuation). Unlike the cross-mapped
    /// classes, each of the N elements maps to one task, so N is the element count. The selector returns one
    /// pre-completed <c>Task&lt;int&gt;</c> built in <c>[GlobalSetup]</c>, so the measured cost is the operator's
    /// per-task bookkeeping, not task allocation.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class SelectManyTaskBenchmarks : OperatorVariantBenchmarkBase
    {
        private Task<int> _completed;

        [GlobalSetup]
        public void Setup() => _completed = Task.FromResult(42);

        [Benchmark]
        public void SelectMany_Task() =>
            Observable.Range(1, N).SelectMany(_ => _completed).SubscribeConsume(Consumer);
    }
}
