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
    /// Event bridging: <c>FromEvent</c> / <c>FromEventPattern</c> expose a .NET event as an observable. The
    /// immediate scheduler is used so the handler is attached synchronously, then the event is raised N times.
    /// </summary>
    [BenchmarkCategory("Events")]
    public class FromEventBenchmarks : OperatorBenchmarkBase
    {
        private event Action<int> Plain;

        private event EventHandler<int> Pattern;

        [Benchmark]
        public void FromEvent()
        {
            using (Observable.FromEvent<int>(h => Plain += h, h => Plain -= h, ImmediateScheduler.Instance).SubscribeConsume(Consumer))
            {
                var n = N;
                for (var i = 0; i < n; i++)
                {
                    Plain?.Invoke(i);
                }
            }
        }

        [Benchmark]
        public void FromEventPattern()
        {
            using (Observable.FromEventPattern<int>(h => Pattern += h, h => Pattern -= h, ImmediateScheduler.Instance).SubscribeConsume(Consumer))
            {
                var n = N;
                for (var i = 0; i < n; i++)
                {
                    Pattern?.Invoke(this, i);
                }
            }
        }
    }
}
