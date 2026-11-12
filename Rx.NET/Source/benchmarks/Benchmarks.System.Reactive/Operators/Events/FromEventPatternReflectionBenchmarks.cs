// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Events
{
    /// <summary>
    /// Event bridging by name: the reflection-based <c>FromEventPattern(target, eventName, scheduler)</c>. Each
    /// subscription looks the event up by reflection and attaches a handler built for its delegate type, then every
    /// raise is wrapped in an <c>EventPattern&lt;object&gt;</c> — unlike the delegate-based overload in
    /// <see cref="FromEventBenchmarks"/>, which needs no reflection. The immediate scheduler attaches the handler
    /// synchronously; the event is raised N times with the shared <see cref="EventArgs.Empty"/> instance.
    /// </summary>
    [BenchmarkCategory("Events")]
    public class FromEventPatternReflectionBenchmarks : OperatorVariantBenchmarkBase
    {
        private readonly EventTarget _target = new();

        [Benchmark]
        public void FromEventPattern_Reflection()
        {
            using (Observable.FromEventPattern(_target, nameof(EventTarget.Fired), ImmediateScheduler.Instance).SubscribeConsume(Consumer))
            {
                var target = _target;
                var n = N;
                for (var i = 0; i < n; i++)
                {
                    target.Raise();
                }
            }
        }

        /// <summary>The event source; public so the reflection lookup (public instance events) finds <see cref="Fired"/>.</summary>
        public sealed class EventTarget
        {
            public event EventHandler Fired;

            public void Raise() => Fired?.Invoke(this, EventArgs.Empty);
        }
    }
}
