// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using BenchmarkDotNet.Attributes;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Benchmarks.System.Reactive
{
    [MemoryDiagnoser]
    public class SwitchBenchmark
    {
        [Benchmark]
        public async Task Switch_10000_Sources()
        {
            await Observable
                .Range(1, 10000)
                .Select(x => Observable.Return(x))
                .Switch()
                .ToTask();
        }
    }
}
