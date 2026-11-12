// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Awaiter
{
    /// <summary>
    /// S1 (variant sweep): <c>RunAsync</c> on a connectable observable, which subscribes and then connects it,
    /// awaited for the last value of a <c>Publish()</c> over a cold source of N. Complements
    /// <see cref="AwaiterBenchmarks"/>, whose connectable row goes through <c>GetAwaiter</c> without a token.
    /// </summary>
    [BenchmarkCategory("Awaiter")]
    public class AwaiterConnectableBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public async ValueTask RunAsync_Connectable()
        {
            Consumer.Consume(await BenchmarkSources.Range(N).Publish().RunAsync(CancellationToken.None));
        }
    }
}
