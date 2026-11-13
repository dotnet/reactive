// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>
    /// The suite is otherwise all <c>int</c> pipelines; this runs the GC-sensitive operators over a
    /// <b>reference-type</b> (<c>string</c>) element, so behaviour with GC-tracked payloads — the internal
    /// <c>HashSet</c>/<c>List</c>/<c>Queue</c> holding object references, equality via <c>EqualityComparer</c> — is
    /// measured rather than the value-type fast paths.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class ReferenceTypeElementBenchmarks : OperatorBenchmarkBase
    {
        private string[] _strings = default!;

        [GlobalSetup]
        public void Setup() => _strings = Enumerable.Range(1, N).Select(static i => (i % 1000).ToString()).ToArray();

        [Benchmark]
        public void Distinct() => _strings.ToObservable().Distinct().SubscribeConsume(Consumer);

        [Benchmark]
        public void DistinctUntilChanged() => _strings.ToObservable().DistinctUntilChanged().SubscribeConsume(Consumer);

        [Benchmark]
        public void GroupBy() => _strings.ToObservable().GroupBy(static s => s.Length).SelectMany(static g => g).SubscribeConsume(Consumer);

        [Benchmark]
        public void Buffer() => _strings.ToObservable().Buffer(16).SubscribeConsume(Consumer);
    }
}
