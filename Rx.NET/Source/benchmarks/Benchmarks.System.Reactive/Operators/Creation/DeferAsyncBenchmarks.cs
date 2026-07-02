// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Creation
{
    /// <summary>S1: <c>DeferAsync</c> builds the sequence lazily via an async factory, once per subscription.</summary>
    [BenchmarkCategory("Creation")]
    public class DeferAsyncBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void DeferAsync()
        {
            var n = N;
            Observable.DeferAsync(_ => Task.FromResult(Observable.Range(1, n))).SubscribeConsume(Consumer);
        }
    }
}
