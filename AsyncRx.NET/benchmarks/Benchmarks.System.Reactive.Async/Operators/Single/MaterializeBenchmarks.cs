// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Single
{
    /// <summary>
    /// S1: <c>Materialize</c> (one <c>Notification</c> allocation per element) and the
    /// <c>Materialize().Dematerialize()</c> round trip.
    /// </summary>
    /// <remarks>
    /// In the round trip, Dematerialize completes on the materialized OnCompleted notification. Materialize then
    /// forwards its own OnCompleted to the terminated observer, which drops it.
    /// </remarks>
    [BenchmarkCategory("Single")]
    public class MaterializeBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Materialize() =>
            BenchmarkSources.Range(N).Materialize().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Materialize_Dematerialize() =>
            BenchmarkSources.Range(N).Materialize().Dematerialize().SubscribeConsumeAsync(Consumer);
    }
}
