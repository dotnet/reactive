// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive.Async.Operators.Lifecycle
{
    /// <summary>
    /// The async disposable building blocks every operator uses for subscription bookkeeping, one lifecycle per
    /// row: composite add/remove/dispose, serial reassignment, single assignment, ref-counting, the stable
    /// composites (binary and n-ary) and the cancellation disposable. <c>CompositeAsyncDisposable</c>,
    /// <c>SerialAsyncDisposable</c> and <c>RefCountAsyncDisposable</c> take an <c>AsyncGate</c> per operation.
    /// </summary>
    [BenchmarkCategory("Lifecycle")]
    public class AsyncDisposableBenchmarks
    {
        private static readonly IAsyncDisposable Nop = AsyncDisposable.Nop;

        [Benchmark(Baseline = true)]
        public ValueTask StableComposite_Binary() =>
            StableCompositeAsyncDisposable.Create(Nop, Nop).DisposeAsync();

        [Benchmark]
        public ValueTask StableComposite_NAry() =>
            StableCompositeAsyncDisposable.Create(Nop, Nop, Nop, Nop).DisposeAsync();

        [Benchmark]
        public async ValueTask Composite_AddRemoveDispose()
        {
            var composite = new CompositeAsyncDisposable();
            var item = AsyncDisposable.Create(static () => default);
            await composite.AddAsync(Nop).ConfigureAwait(false);
            await composite.AddAsync(item).ConfigureAwait(false);
            await composite.RemoveAsync(item).ConfigureAwait(false);
            await composite.DisposeAsync().ConfigureAwait(false);
        }

        [Benchmark]
        public async ValueTask Serial_AssignTwiceDispose()
        {
            var serial = new SerialAsyncDisposable();
            await serial.AssignAsync(Nop).ConfigureAwait(false);
            await serial.AssignAsync(Nop).ConfigureAwait(false);
            await serial.DisposeAsync().ConfigureAwait(false);
        }

        [Benchmark]
        public async ValueTask SingleAssignment_AssignDispose()
        {
            var single = new SingleAssignmentAsyncDisposable();
            await single.AssignAsync(Nop).ConfigureAwait(false);
            await single.DisposeAsync().ConfigureAwait(false);
        }

        [Benchmark]
        public async ValueTask RefCount_TwoReferences()
        {
            var refCount = new RefCountAsyncDisposable(Nop);
            var first = await refCount.GetDisposableAsync().ConfigureAwait(false);
            var second = await refCount.GetDisposableAsync().ConfigureAwait(false);
            await first.DisposeAsync().ConfigureAwait(false);
            await second.DisposeAsync().ConfigureAwait(false);
            await refCount.DisposeAsync().ConfigureAwait(false);
        }

        [Benchmark]
        public ValueTask Cancellation_Dispose() => new CancellationAsyncDisposable().DisposeAsync();
    }
}
