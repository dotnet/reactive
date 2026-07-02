// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Creation
{
    /// <summary>S1: <c>Defer</c> builds the underlying sequence lazily, once per subscription.</summary>
    [BenchmarkCategory("Creation")]
    public class DeferBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Defer()
        {
            var n = N;
            Observable.Defer(() => Observable.Range(1, n)).SubscribeConsume(Consumer);
        }
    }
}
