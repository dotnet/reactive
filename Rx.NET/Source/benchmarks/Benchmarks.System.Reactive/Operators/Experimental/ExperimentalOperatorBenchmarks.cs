// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Experimental
{
    /// <summary>
    /// S1: the <c>ObservableEx</c> operators other than <c>Expand</c> (see <see cref="ExpandBenchmarks"/>), all
    /// implemented with lambdas and closures in <c>QueryLanguageEx</c> rather than dedicated sinks.
    /// <list type="bullet">
    /// <item><c>ForkJoin_Binary</c>/<c>ForkJoin_Params</c> — wait for every source to complete and combine their last
    /// values (binary with a selector; three sources via the array form, which emits an <c>int[]</c>). Each source is a
    /// cold <c>Range(1, N)</c>.</item>
    /// <item><c>ManySelect</c> — for every element, builds a chain node exposing the rest of the sequence and passes it
    /// to the selector (here one that ignores it), via <c>ObserveOn</c>. The trampolining
    /// <see cref="CurrentThreadScheduler"/> is passed explicitly in place of the default <c>DefaultScheduler</c> so it
    /// stays on the calling thread and deterministic.</item>
    /// <item><c>ToListObservable</c> — subscribes eagerly and collects into a list; the result (an
    /// <c>AsyncSubject</c>-backed <c>IObservable&lt;object&gt;</c>) is then subscribed, observing only completion.</item>
    /// <item><c>Create_Iterator</c> — the iterator-style <c>ObservableEx.Create&lt;TResult&gt;(observer =&gt; IEnumerable&lt;IObservable&lt;object&gt;&gt;)</c>:
    /// the iterator pushes one value per step and yields a pre-built empty observable, which <c>Concat</c> subscribes
    /// before resuming it.</item>
    /// </list>
    /// <c>ObservableEx.Let</c> is not measured: it is just <c>selector(source)</c> and adds no operator.
    /// </summary>
    [BenchmarkCategory("Experimental")]
    public class ExperimentalOperatorBenchmarks : OperatorVariantBenchmarkBase
    {
        private static readonly IObservable<object> Step = Observable.Empty<object>();

        private IObservable<int> _s;

        [GlobalSetup]
        public void Setup() => _s = Observable.Range(1, N);

        [Benchmark]
        public void ForkJoin_Binary() => _s.ForkJoin(_s, static (a, b) => a + b).SubscribeConsume(Consumer);

        [Benchmark]
        public void ForkJoin_Params() => ObservableEx.ForkJoin(_s, _s, _s).SubscribeConsume(Consumer);

        [Benchmark]
        public void ManySelect() =>
            _s.ManySelect(static _ => 1, CurrentThreadScheduler.Instance).SubscribeConsume(Consumer);

        [Benchmark]
        public void ToListObservable() => _s.ToListObservable().SubscribeConsume(Consumer);

        [Benchmark]
        public void Create_Iterator()
        {
            var n = N;
            ObservableEx.Create<int>(observer => Iterate(observer, n)).SubscribeConsume(Consumer);
        }

        private static IEnumerable<IObservable<object>> Iterate(IObserver<int> observer, int n)
        {
            for (var i = 0; i < n; i++)
            {
                observer.OnNext(i);
                yield return Step;
            }
        }
    }
}
