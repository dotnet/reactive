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
    /// S1: <c>Materialize</c> (one <c>Notification</c> allocation per element).
    /// </summary>
    /// <remarks>
    /// KNOWN GAP: the <c>Materialize().Dematerialize()</c> round trip throws
    /// <c>InvalidOperationException("The observer has already terminated.")</c> for every N: Dematerialize
    /// completes on the materialized OnCompleted notification, then Materialize forwards its own OnCompleted to the
    /// terminated observer. Restore the commented-out row when the library is fixed.
    /// </remarks>
    [BenchmarkCategory("Single")]
    public class MaterializeBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Materialize() =>
            BenchmarkSources.Range(N).Materialize().SubscribeConsumeAsync(Consumer);

        // KNOWN GAP (see remarks):
        // [Benchmark]
        // public ValueTask Materialize_Dematerialize() =>
        //     BenchmarkSources.Range(N).Materialize().Dematerialize().SubscribeConsumeAsync(Consumer);
    }
}
