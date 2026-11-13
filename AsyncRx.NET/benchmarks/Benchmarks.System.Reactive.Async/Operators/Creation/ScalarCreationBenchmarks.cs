// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Creation
{
    /// <summary>
    /// Per-subscription cost of the scalar sources: Return, Empty and Throw (default and explicit immediate
    /// scheduler), and Never (subscribe, then dispose, because it never terminates). No N sweep: each row is one
    /// subscription.
    /// </summary>
    [BenchmarkCategory("Creation")]
    public class ScalarCreationBenchmarks
    {
        private readonly Consumer _consumer = new();

        [Benchmark(Baseline = true)]
        public ValueTask Return() => AsyncObservable.Return(42).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask Return_Scheduler() => AsyncObservable.Return(42, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask Empty() => AsyncObservable.Empty<int>().SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask Empty_Scheduler() => AsyncObservable.Empty<int>(ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask Throw() => AsyncObservable.Throw<int>(FaultingSource.BenchmarkError).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask Throw_Scheduler() => AsyncObservable.Throw<int>(FaultingSource.BenchmarkError, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public async ValueTask Never_SubscribeDispose()
        {
            var subscription = await AsyncObservable.Never<int>().SubscribeNoWaitAsync(_consumer).ConfigureAwait(false);
            await subscription.DisposeAsync().ConfigureAwait(false);
        }
    }
}
