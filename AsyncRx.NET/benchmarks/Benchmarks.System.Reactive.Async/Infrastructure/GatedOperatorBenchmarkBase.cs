// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive.Async.Infrastructure
{
    /// <summary>
    /// Base class for throughput (S1) benchmarks of operators whose <b>per-element path takes an
    /// <c>AsyncGate</c></b> (Merge, Zip, CombineLatest, SelectMany, Switch, GroupBy, Window, Synchronize,
    /// ObserveOn, the Behavior/Replay subjects, ...). It uses a capped sweep (N = 1, 100, 10,000).
    /// This type is <c>abstract</c> so auto-discovery skips it.
    /// </summary>
    /// <remarks>
    /// <c>AsyncGate.LockAsync</c> acquires its semaphore through
    /// <c>SemaphoreSlim.WaitAsync().ContinueWith(...)</c> without <c>ExecuteSynchronously</c>, so every
    /// acquisition, even an uncontended one, queues a work item to the thread pool. Each element therefore costs
    /// at least one thread-pool round trip, completion arrives on a pool thread, and the timings include
    /// thread-pool scheduling variance. A 1,000,000-element sweep would take seconds per invocation, hence the
    /// cap. The Allocated column stays close to deterministic and is the more reliable before/after signal for
    /// these classes.
    /// </remarks>
    public abstract class GatedOperatorBenchmarkBase : BenchmarkBase
    {
        [Params(1, 100, 10_000)]
        public int N;
    }
}
