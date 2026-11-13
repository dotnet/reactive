// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Concurrency
{
    /// <summary>
    /// Concurrency (async S4): the scalar factories (<c>Return</c>, <c>Throw</c>) and <c>Prepend</c> with an explicit
    /// scheduler that runs the work on another thread. Each invocation schedules one work item, so the rows measure the
    /// cost of one <c>Schedule</c> call on each scheduler plus the hand-off back to the waiting thread.
    /// </summary>
    /// <remarks>
    /// <para>
    /// It replaces the earlier <c>ScalarScheduleBenchmark</c>. <see cref="Creation.ScalarCreationBenchmarks"/> covers the
    /// same factories on their default (immediate) scheduler.
    /// </para>
    /// <para>
    /// Each invocation blocks until the sequence terminates (<c>SubscribeBlocking</c>), which adds one
    /// <c>ManualResetEventSlim</c> per call. That cost is the same on every scheduler, so compare the rows of this class
    /// with each other. <c>NewThread</c> starts a thread for each work item, so expect it to be much slower.
    /// </para>
    /// </remarks>
    [BenchmarkCategory("Concurrency")]
    public class ScalarSchedulerBenchmarks
    {
        private readonly Consumer _consumer = new();
        private readonly Exception _error = new InvalidOperationException("benchmark");
        private IScheduler _scheduler = default!;
        private EventLoopScheduler _eventLoop;

        [Params("EventLoop", "TaskPool", "ThreadPool", "NewThread")]
        public string Kind = "EventLoop";

        [GlobalSetup]
        public void Setup()
        {
            _scheduler = Kind switch
            {
                "EventLoop" => _eventLoop = new EventLoopScheduler(),
                "TaskPool" => TaskPoolScheduler.Default,
                "ThreadPool" => ThreadPoolScheduler.Instance,
                "NewThread" => NewThreadScheduler.Default,
                _ => throw new InvalidOperationException("Unknown scheduler " + Kind),
            };
        }

        [GlobalCleanup]
        public void Cleanup() => _eventLoop?.Dispose();

        [Benchmark]
        public void Return() => Observable.Return(1, _scheduler).SubscribeBlocking(_consumer);

        [Benchmark]
        public void Throw() => Observable.Throw<int>(_error, _scheduler).SubscribeBlocking(_consumer);

        [Benchmark]
        public void Prepend() => Observable.Return(1, _scheduler).Prepend(0, _scheduler).SubscribeBlocking(_consumer);
    }
}
