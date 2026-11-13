// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.StandardSequence
{
    /// <summary>
    /// S1 (variant sweep): the <c>SelectMany</c> overloads that <see cref="SelectManyBenchmarks"/> and
    /// <see cref="SelectManyOverloadBenchmarks"/> do not cover. The shape is linear, not pinned total work: the
    /// outer source has <c>N</c> elements and each maps to a small, constant inner.
    /// <list type="bullet">
    /// <item><c>_Task</c>: a <c>ValueTask&lt;TResult&gt;</c> per element (plain, with a token, indexed, and with a
    /// result selector). The tasks complete synchronously.</item>
    /// <item><c>_Enumerable</c>: one cached 4-element array per element (plain, indexed, and with a result
    /// selector), so the output has <c>4N</c> elements.</item>
    /// <item><c>SelectMany_Indexed_ResultSelector_Async</c> and <c>SelectMany_Notifications</c>: one inner
    /// observable subscription per element, using a cached single-value source.</item>
    /// </list>
    /// Every selector lives in a field typed as the exact delegate its overload takes, built once in
    /// <c>[GlobalSetup]</c>, so overload resolution cannot pick a sibling (for example the indexed form instead
    /// of the token form) and no delegate is allocated per invocation.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class SelectManyVariantBenchmarks : OperatorVariantBenchmarkBase
    {
        private Func<int, ValueTask<int>> _task = default!;
        private Func<int, CancellationToken, ValueTask<int>> _taskToken = default!;
        private Func<int, int, ValueTask<int>> _taskIndexed = default!;
        private Func<int, int, int> _taskResult = default!;
        private Func<int, IEnumerable<int>> _enumerable = default!;
        private Func<int, int, IEnumerable<int>> _enumerableIndexed = default!;
        private Func<int, int, int> _collectionResult = default!;
        private Func<int, int, ValueTask<IAsyncObservable<int>>> _innerIndexedAsync = default!;
        private Func<int, int, int, int, ValueTask<int>> _innerIndexedResultAsync = default!;
        private Func<int, IAsyncObservable<int>> _onNext = default!;
        private Func<Exception, IAsyncObservable<int>> _onError = default!;
        private Func<IAsyncObservable<int>> _onCompleted = default!;

        [GlobalSetup]
        public void Setup()
        {
            IEnumerable<int> items = new[] { 1, 2, 3, 4 };
            var inner = BenchmarkSources.Return(0);

            _task = static v => new ValueTask<int>(v);
            _taskToken = static (int v, CancellationToken _) => new ValueTask<int>(v);
            _taskIndexed = static (int v, int i) => new ValueTask<int>(v + i);
            _taskResult = static (v, r) => v + r;
            _enumerable = _ => items;
            _enumerableIndexed = (_, _) => items;
            _collectionResult = static (v, c) => v + c;
            _innerIndexedAsync = (_, _) => new ValueTask<IAsyncObservable<int>>(inner);
            _innerIndexedResultAsync = static (v, vi, c, ci) => new ValueTask<int>(v + c);
            _onNext = _ => inner;
            _onError = _ => inner;
            _onCompleted = () => inner;
        }

        [Benchmark]
        public ValueTask SelectMany_Task() =>
            BenchmarkSources.Range(N).SelectMany(_task).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SelectMany_Task_CancellationToken() =>
            BenchmarkSources.Range(N).SelectMany(_taskToken).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SelectMany_Task_Indexed() =>
            BenchmarkSources.Range(N).SelectMany(_taskIndexed).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SelectMany_Task_ResultSelector() =>
            BenchmarkSources.Range(N).SelectMany(_task, _taskResult).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SelectMany_Enumerable() =>
            BenchmarkSources.Range(N).SelectMany(_enumerable).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SelectMany_Enumerable_Indexed() =>
            BenchmarkSources.Range(N).SelectMany(_enumerableIndexed).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SelectMany_Enumerable_ResultSelector() =>
            BenchmarkSources.Range(N).SelectMany(_enumerable, _collectionResult).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SelectMany_Indexed_ResultSelector_Async() =>
            BenchmarkSources.Range(N).SelectMany(_innerIndexedAsync, _innerIndexedResultAsync).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SelectMany_Notifications() =>
            BenchmarkSources.Range(N).SelectMany(_onNext, _onError, _onCompleted).SubscribeConsumeAsync(Consumer);
    }
}
