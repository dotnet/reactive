// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.StandardSequence
{
    /// <summary>
    /// S1: reference-type (<c>string</c>) elements through Select, Where and Distinct, so generic code paths
    /// shared across reference types (and their write barriers and hashing) are measured, not only the
    /// <c>int</c> specialisations. The strings are built in <c>[GlobalSetup]</c>.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class ReferenceTypeElementBenchmarks : OperatorBenchmarkBase
    {
        private ArrayAsyncSource<string> _strings = default!;

        [GlobalSetup]
        public void Setup()
        {
            var items = new string[N];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = i.ToString(CultureInfo.InvariantCulture);
            }

            _strings = new ArrayAsyncSource<string>(items);
        }

        [Benchmark]
        public ValueTask Select_String() =>
            _strings.Select(static s => s.Length).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Where_String() =>
            _strings.Where(static s => s.Length > 1).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Distinct_String() =>
            AsyncObservable.Distinct(_strings).SubscribeConsumeAsync(Consumer);
    }
}
