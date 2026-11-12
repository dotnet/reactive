// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Conversions
{
    /// <summary>
    /// Conversions bridging to .NET events and pull sources: <c>ToEvent</c> / <c>ToEventPattern</c> expose an
    /// observable as an event (attaching the handler drives the synchronous source), and the
    /// <c>IEnumerable.Subscribe</c> overload pushes a pull sequence to an observer. The <c>ToEventPattern</c> input
    /// (N <see cref="EventPattern{TEventArgs}"/> instances) is pre-built in <c>[GlobalSetup]</c>, so the measured
    /// region excludes constructing them.
    /// </summary>
    [BenchmarkCategory("Conversions")]
    public class ToEventBenchmarks : OperatorBenchmarkBase
    {
        private EventPattern<EventArgs>[] _patterns = default!;

        [GlobalSetup]
        public void Setup()
        {
            _patterns = new EventPattern<EventArgs>[N];
            for (var i = 0; i < _patterns.Length; i++)
            {
                _patterns[i] = new EventPattern<EventArgs>(this, EventArgs.Empty);
            }
        }

        [Benchmark]
        public void ToEvent()
        {
            var source = Observable.Range(1, N).ToEvent();
            void Handler(int value) => Consumer.Consume(value);
            source.OnNext += Handler;   // attaching subscribes; the synchronous source runs to completion here
            source.OnNext -= Handler;
        }

        [Benchmark]
        public void ToEventPattern()
        {
            var source = _patterns.ToObservable().ToEventPattern();
            void Handler(object sender, EventArgs e) => Consumer.Consume(e);
            source.OnNext += Handler;
            source.OnNext -= Handler;
        }

        [Benchmark]
        public void EnumerableSubscribe() =>
            Enumerable.Range(1, N).Subscribe(new ConsumingObserver<int>(Consumer));
    }
}
