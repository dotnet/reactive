// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using BenchmarkDotNet.Engines;

namespace Benchmarks.System.Reactive.Async.Infrastructure
{
    public static class ConnectableExtensions
    {
        /// <summary>
        /// Subscribes the cached consuming sink to a connectable observable, connects it (a cold source then runs
        /// through the multicast subject), waits for the sink to terminate, and disposes the connection.
        /// </summary>
        public static async ValueTask SubscribeConnectConsumeAsync<T>(this IConnectableAsyncObservable<T> source, Consumer consumer)
        {
            var sink = Sinks.For<T>(consumer);
            await source.SubscribeAsync(sink).ConfigureAwait(false);
            var connection = await source.ConnectAsync().ConfigureAwait(false);
            await sink.Completion.WaitAsync().ConfigureAwait(false);
            await connection.DisposeAsync().ConfigureAwait(false);
        }
    }
}
