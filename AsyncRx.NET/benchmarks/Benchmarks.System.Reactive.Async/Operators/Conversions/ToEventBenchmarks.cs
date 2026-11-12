// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Conversions
{
    /// <summary>
    /// S1 (variant sweep): conversions out of AsyncRx. <c>ToEvent</c> and <c>ToEventPattern</c> expose a hot
    /// <see cref="PushSource{T}"/> as a .NET event (adding the handler subscribes; N pushes raise N events; removing
    /// it disposes). The notification-to-observer bridge (<c>NotificationAsyncExtensions</c>) is internal and is not
    /// measured.
    /// </summary>
    [BenchmarkCategory("Conversions")]
    public class ToEventBenchmarks : OperatorVariantBenchmarkBase
    {
        private EventPattern<EventArgs>[] _patterns = default!;
        private Action<int> _onNext = default!;
        private EventHandler<EventArgs> _onPattern = default!;

        [GlobalSetup]
        public void Setup()
        {
            _patterns = new EventPattern<EventArgs>[N];
            for (var i = 0; i < N; i++)
            {
                _patterns[i] = new EventPattern<EventArgs>(this, EventArgs.Empty);
            }

            _onNext = v => Consumer.Consume(v);
            _onPattern = (_, e) => Consumer.Consume(e);
        }

        [Benchmark]
        public async ValueTask ToEvent()
        {
            var source = new PushSource<int>();
            var events = source.ToEvent();
            events.OnNext += _onNext;
            await source.PushAsync(N).ConfigureAwait(false);
            events.OnNext -= _onNext;
        }

        [Benchmark]
        public async ValueTask ToEventPattern()
        {
            var source = new PushSource<EventPattern<EventArgs>>();
            var events = source.ToEventPattern();
            events.OnNext += _onPattern;
            var patterns = _patterns;
            for (var i = 0; i < patterns.Length; i++)
            {
                await source.OnNextAsync(patterns[i]).ConfigureAwait(false);
            }

            events.OnNext -= _onPattern;
        }
    }
}
