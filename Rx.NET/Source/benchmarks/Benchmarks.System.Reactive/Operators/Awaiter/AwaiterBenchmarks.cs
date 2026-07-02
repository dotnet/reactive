// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Awaiter
{
    /// <summary>
    /// Await support: <c>RunAsync</c> and <c>GetAwaiter</c> both project the sequence to its final element via an
    /// <c>AsyncSubject</c>; blocking on the result drains the whole stream.
    /// </summary>
    [BenchmarkCategory("Awaiter")]
    public class AwaiterBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public int RunAsync() => Observable.Range(1, N).RunAsync(CancellationToken.None).GetAwaiter().GetResult();

        [Benchmark]
        public int GetAwaiter() => Observable.Range(1, N).GetAwaiter().GetResult();
    }
}
