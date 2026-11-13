// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Single
{
    /// <summary>S1: <c>Append</c> / <c>Prepend</c> a single element, and <c>StartWith</c> a leading value.</summary>
    [BenchmarkCategory("Single")]
    public class AppendPrependStartWithBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Append() => Observable.Range(1, N).Append(0).SubscribeConsume(Consumer);

        [Benchmark]
        public void Prepend() => Observable.Range(1, N).Prepend(0).SubscribeConsume(Consumer);

        [Benchmark]
        public void StartWith() => Observable.Range(1, N).StartWith(0).SubscribeConsume(Consumer);
    }
}
