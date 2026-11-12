// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Single
{
    /// <summary>
    /// S1 (variant sweep): the handler-driven resubscription operators. <c>RepeatWhen</c> over a completing
    /// source and <c>RetryWhen</c> over a faulting source, each with a handler that answers two signals
    /// (<c>signals.Take(2)</c>) and then completes. The handler's completion completes the result, so each row
    /// runs a fixed, small number of attempts of <c>N</c> elements (at most three), as
    /// <see cref="RetryRepeatBenchmarks"/> does. <c>RetryWhen</c> therefore completes rather than faults.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class RepeatWhenRetryWhenBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask RepeatWhen() =>
            BenchmarkSources.Range(N).RepeatWhen(static signals => signals.Take(2)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask RetryWhen() =>
            FaultingSource.Faulting(N).RetryWhen(static errors => errors.Take(2)).SubscribeConsumeAsync(Consumer);
    }
}
