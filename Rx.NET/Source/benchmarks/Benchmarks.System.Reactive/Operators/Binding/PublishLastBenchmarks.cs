// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Binding
{
    /// <summary>S2: <c>PublishLast</c> (AsyncSubject-backed) multicasts only the final element on completion.</summary>
    [BenchmarkCategory("Binding")]
    public class PublishLastBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void PublishLast()
        {
            var connectable = Observable.Range(1, N).PublishLast();
            using (connectable.SubscribeConsume(Consumer))
            using (connectable.Connect())
            {
            }
        }
    }
}
