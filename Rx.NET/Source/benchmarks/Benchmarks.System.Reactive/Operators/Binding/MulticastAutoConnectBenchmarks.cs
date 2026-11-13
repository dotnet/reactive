// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Binding
{
    /// <summary>
    /// S2: <c>Multicast</c> shares a source through an explicit subject; <c>AutoConnect</c> connects a published
    /// source automatically once the required number of subscribers attach.
    /// </summary>
    [BenchmarkCategory("Binding")]
    public class MulticastAutoConnectBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Multicast()
        {
            var connectable = Observable.Range(1, N).Multicast(new Subject<int>());
            using (connectable.SubscribeConsume(Consumer))
            using (connectable.Connect())
            {
            }
        }

        [Benchmark]
        public void AutoConnect() => Observable.Range(1, N).Publish().AutoConnect(1).SubscribeConsume(Consumer);
    }
}
