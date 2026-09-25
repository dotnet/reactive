// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Disposables;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Creation
{
    /// <summary>S1: <c>Observable.Create</c> — the primitive factory, pushing N elements from a user callback.</summary>
    [BenchmarkCategory("Creation")]
    public class CreateBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Create()
        {
            var n = N;
            Observable.Create<int>(observer =>
            {
                for (var i = 0; i < n; i++)
                {
                    observer.OnNext(i);
                }

                observer.OnCompleted();
                return Disposable.Empty;
            }).SubscribeConsume(Consumer);
        }
    }
}
