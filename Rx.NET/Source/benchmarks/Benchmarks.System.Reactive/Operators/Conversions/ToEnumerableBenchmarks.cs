// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Conversions
{
    /// <summary>
    /// Conversion-category exemplar: <c>ToEnumerable</c> converts a push sequence to a blocking pull sequence;
    /// the benchmark iterates it to completion.
    /// </summary>
    [BenchmarkCategory("Conversions")]
    public class ToEnumerableBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void ToEnumerable()
        {
            foreach (var value in Observable.Range(1, N).ToEnumerable())
            {
                Consumer.Consume(value);
            }
        }
    }
}
