// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Conversions
{
    /// <summary>
    /// S1: <c>ToTask</c> bridges a sequence to a <c>Task</c> of its final element. The synchronous source
    /// completes immediately, so the task is already resolved when awaited.
    /// </summary>
    [BenchmarkCategory("Conversions")]
    public class ToTaskBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public int ToTask() => Observable.Range(1, N).ToTask().GetAwaiter().GetResult();
    }
}
