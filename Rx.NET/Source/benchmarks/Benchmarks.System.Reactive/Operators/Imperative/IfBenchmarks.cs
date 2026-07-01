// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Imperative
{
    /// <summary>
    /// Imperative-combinator-category exemplar (S1): <c>Observable.If</c> chooses a source from a predicate at
    /// subscription time, then relays it.
    /// </summary>
    [BenchmarkCategory("Imperative")]
    public class IfBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void If() => Observable.If(static () => true, Observable.Range(1, N)).SubscribeConsume(Consumer);
    }
}
