// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Binding
{
    /// <summary>
    /// S1 (gated sweep): the <c>Multicast</c> overloads with a <see cref="SequentialSimpleAsyncSubject{T}"/>: an
    /// explicit subject plus connect, a subject factory (a fresh subject per subscription), and the factory plus
    /// selector form, synchronous and <c>ValueTask</c>.
    /// </summary>
    [BenchmarkCategory("Binding")]
    public class MulticastBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Multicast_Subject_Connect() =>
            BenchmarkSources.Range(N).Multicast(new SequentialSimpleAsyncSubject<int>()).SubscribeConnectConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Multicast_Factory() =>
            BenchmarkSources.Range(N).Multicast(static () => new SequentialSimpleAsyncSubject<int>()).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Multicast_Factory_Async() =>
            BenchmarkSources.Range(N).Multicast(static () => new ValueTask<IAsyncSubject<int>>(new SequentialSimpleAsyncSubject<int>())).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Multicast_Factory_Selector() =>
            BenchmarkSources.Range(N)
                .Multicast<int, int, int>(static () => new SequentialSimpleAsyncSubject<int>(), static s => s.Select(static v => v + 1))
                .SubscribeConsumeAsync(Consumer);
    }
}
