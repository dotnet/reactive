// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Single
{
    /// <summary>S1: <c>DistinctUntilChanged</c> compares adjacent elements; the ascending source keeps every element.</summary>
    [BenchmarkCategory("Single")]
    public class DistinctUntilChangedBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void DistinctUntilChanged() => Observable.Range(1, N).DistinctUntilChanged().SubscribeConsume(Consumer);
    }
}
