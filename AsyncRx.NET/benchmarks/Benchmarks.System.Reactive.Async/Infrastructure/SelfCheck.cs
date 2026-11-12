// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Microsoft.Reactive.Testing.Async;

namespace Benchmarks.System.Reactive.Async.Infrastructure
{
    /// <summary>
    /// Sanity checks for the benchmark infrastructure, run with <c>--verify</c>. It checks that the sources, sinks and <see cref="Microsoft.Reactive.Testing.Async.TestAsyncScheduler"/> deliver what the
    /// benchmarks assume before any time is spent measuring them.
    /// </summary>
    public static class SelfCheck
    {
        private const int N = 1_000;
        private const long PeriodTicks = 10;
        private static readonly TimeSpan Guard = TimeSpan.FromSeconds(30);

        public static async Task<int> RunAsync()
        {
            var failures = 0;

            async Task Check(string name, Func<Task<string?>> check)
            {
                var stopwatch = Stopwatch.StartNew();
                string? problem;
                try
                {
                    var run = check();
                    var finished = await Task.WhenAny(run, Task.Delay(Guard)).ConfigureAwait(false);
                    problem = finished == run ? await run.ConfigureAwait(false) : $"did not finish within {Guard.TotalSeconds} s";
                }
                catch (Exception ex)
                {
                    problem = ex.GetType().Name + ": " + ex.Message;
                    if (Environment.GetEnvironmentVariable("ASYNCRX_BENCH_VERBOSE") == "1")
                    {
                        problem += Environment.NewLine + ex;
                    }
                }

                if (problem == null)
                {
                    Console.WriteLine($"  PASS  {name} ({stopwatch.ElapsedMilliseconds} ms)");
                }
                else
                {
                    failures++;
                    Console.WriteLine($"  FAIL  {name}: {problem}");
                }
            }

            Console.WriteLine("AsyncRx benchmark self-check (N = " + N + "):");

            await Check("Range(N).Select delivers N", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await BenchmarkSources.Range(N).Select(static v => v + 1).SubscribeAwaitAsync(o, o.Completion);
                return Expect(o, N, completed: true);
            });

            await Check("ArrayAsyncSource delivers N", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await new ArrayAsyncSource<int>(new int[N]).SubscribeAwaitAsync(o, o.Completion);
                return Expect(o, N, completed: true);
            });

            await Check("Faulting(N) delivers N then an error", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await FaultingSource.Faulting(N).SubscribeAwaitAsync(o, o.Completion);
                return o.Count == N && ReferenceEquals(o.Error, FaultingSource.BenchmarkError) ? null : Describe(o);
            });

            await Check("Gated Merge of 10 sources delivers N", async () =>
            {
                var sources = new IAsyncObservable<int>[10];
                for (var i = 0; i < sources.Length; i++)
                {
                    sources[i] = BenchmarkSources.Range(N / 10);
                }

                var o = new CountingAsyncObserver<int>();
                await new ArrayAsyncSource<IAsyncObservable<int>>(sources).Merge().SubscribeAwaitAsync(o, o.Completion);
                return Expect(o, N, completed: true);
            });

            await Check("Cached sink completes for a synchronous pipeline", async () =>
            {
                var consumer = new BenchmarkDotNet.Engines.Consumer();
                var pending = BenchmarkSources.Range(N).SubscribeConsumeAsync(consumer);
                var synchronous = pending.IsCompletedSuccessfully;
                await pending;
                return synchronous ? null : "SubscribeConsumeAsync did not complete synchronously";
            });

            await Check("Cold Range(N).Zip(Range(N)) delivers N and completes", async () =>
            {
                var o = new CountingAsyncObserver<(int, int)>();
                await BenchmarkSources.Range(N).Zip(BenchmarkSources.Range(N)).SubscribeAwaitAsync(o, o.Completion);
                return Expect(o, N, completed: true);
            });

            await Check("Window(count) flattened delivers N", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await BenchmarkSources.Range(N).Window(16).Merge().SubscribeAwaitAsync(o, o.Completion);
                return Expect(o, N, completed: true);
            });

            await Check("Materialize().Dematerialize() delivers N", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await BenchmarkSources.Range(N).Materialize().Dematerialize().SubscribeAwaitAsync(o, o.Completion);
                return Expect(o, N, completed: true);
            });

            await Check("ObserveOn(ImmediateAsyncScheduler) delivers N", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await BenchmarkSources.Range(N).ObserveOn(ImmediateAsyncScheduler.Instance).SubscribeAwaitAsync(o, o.Completion);
                return Expect(o, N, completed: true);
            });

            await Check("Amb(params) delivers N", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await AsyncObservable.Amb(BenchmarkSources.Range(N), AsyncObservable.Never<int>(), AsyncObservable.Never<int>())
                    .SubscribeAwaitAsync(o, o.Completion);
                return Expect(o, N, completed: true);
            });

            // The same shape as RepeatInfiniteBenchmarks.Repeat_Source_Infinite: one element and one completion per resubscription.
            await Check("PushSource Repeat().Take(N) delivers N", async () =>
            {
                var source = new PushSource<int>();
                var o = new CountingAsyncObserver<int>();
                await source.Repeat().Take(N).SubscribeAsync(o);
                for (var i = 0; i < N && !o.Completion.IsSet; i++)
                {
                    await source.OnNextAsync(i);
                    await source.OnCompletedAsync();
                }

                await o.Completion.WaitAsync();
                return Expect(o, N, completed: true);
            });

            await Check("Virtual time: source baseline delivers N",() => Timed(static (s, _) => s, N));

            await Check("Virtual time: Throttle (Sparse) delivers N", () =>
                Timed(static (s, sch) => s.Throttle(TimeSpan.FromTicks(PeriodTicks / 2), sch), N));

            await Check("Virtual time: Throttle (Dense) delivers 1", () =>
                Timed(static (s, sch) => s.Throttle(TimeSpan.FromTicks(PeriodTicks * N), sch), 1));

            await Check("Virtual time: Buffer(time) buffer sizes sum to N", async () =>
            {
                long total = 0;
                var o = new CountingAsyncObserver<IList<int>> { OnValue = b => total += b.Count };
                await VirtualTimeDriver.RunTimedAsync(N, PeriodTicks, o, o.Completion, 0,
                    static (s, sch, _) => s.Buffer(TimeSpan.FromTicks(PeriodTicks * 4), sch));
                return total == N && o.Completed ? null : $"sum {total}, buffers {o.Count}, completed {o.Completed}";
            });

            await Check("Virtual time: Window(time) flattened delivers N", () =>
                Timed(static (s, sch) => s.Window(TimeSpan.FromTicks(PeriodTicks * 4), sch).Merge(), N));

            // Windows 4 periods long opened every 2 periods: each element lands in about two windows.
            await Check("Virtual time: Window(timeSpan, timeShift) flattened delivers between N and 2N", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await VirtualTimeDriver.RunTimedAsync(N, PeriodTicks, o, o.Completion, 0,
                    static (s, sch, _) => s.Window(TimeSpan.FromTicks(PeriodTicks * 4), TimeSpan.FromTicks(PeriodTicks * 2), sch).Merge());
                return o.Count >= N && o.Count <= 2 * N && o.Completed ? null : Describe(o);
            });

            await Check("Virtual time: Sample (Sparse) delivers between 1 and N and terminates", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await VirtualTimeDriver.RunTimedAsync(N, PeriodTicks, o, o.Completion, 0,
                    static (s, sch, _) => s.Sample(TimeSpan.FromTicks(PeriodTicks / 2), sch));
                return o.Count >= 1 && o.Count <= N && o.Completed ? null : Describe(o);
            });

            await Check("Virtual time: Delay delivers N", () =>
                Timed(static (s, sch) => s.Delay(TimeSpan.FromTicks(PeriodTicks * 3), sch), N));

            await Check("Virtual time: Timeout (never fires) delivers N", () =>
                Timed(static (s, sch) => s.Timeout(TimeSpan.FromTicks(PeriodTicks * 100), sch), N));

            // The deadline falls half a period after element N/2 - 1: Skip drops elements 0..N/2-1, and Take keeps them.
            await Check("Virtual time: Skip(duration) at mid-timeline delivers N/2", () =>
                Timed(static (s, sch) => s.Skip(TimeSpan.FromTicks(PeriodTicks * (N / 2) + PeriodTicks / 2), sch), N - N / 2));

            await Check("Virtual time: Take(duration) at mid-timeline delivers N/2", () =>
                Timed(static (s, sch) => s.Take(TimeSpan.FromTicks(PeriodTicks * (N / 2) + PeriodTicks / 2), sch), N / 2));

            await Check("Virtual time: SkipUntil(startTime) at mid-timeline delivers N/2", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await VirtualTimeDriver.RunTimedAsync(N, PeriodTicks, o, o.Completion, 0,
                    static (s, sch, _) => s.SkipUntil(sch.Now.AddTicks(PeriodTicks * (N / 2) + PeriodTicks / 2), sch));
                return Expect(o, N - N / 2, completed: true);
            });

            await Check("Virtual time: TakeUntil(endTime) stops at the end time", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await VirtualTimeDriver.RunTimedAsync(N, PeriodTicks, o, o.Completion, 0,
                    static (s, sch, _) => s.TakeUntil(sch.Now.AddTicks(PeriodTicks * (N / 2) + PeriodTicks / 2), sch));
                return o.Count == N / 2 && o.Completed ? null : $"expected {N / 2}, {Describe(o)}";
            });

            await Check("Virtual time: Interval(period).Take(N) delivers N", async () =>
            {
                var o = new CountingAsyncObserver<long>();
                await VirtualTimeDriver.RunSourceAsync(o, o.Completion, 0,
                    static (sch, _) => AsyncObservable.Interval(TimeSpan.FromTicks(PeriodTicks), sch).Take(N));
                return Expect(o, N, completed: true);
            });

            await Check("Virtual time: Timer(due) delivers 1", async () =>
            {
                var o = new CountingAsyncObserver<long>();
                await VirtualTimeDriver.RunSourceAsync(o, o.Completion, 0,
                    static (sch, _) => AsyncObservable.Timer(TimeSpan.FromTicks(PeriodTicks), sch));
                return Expect(o, 1, completed: true);
            });

            Console.WriteLine(failures == 0 ? "All checks passed." : $"{failures} check(s) failed.");
            return failures == 0 ? 0 : 1;
        }

        private static async Task<string?> Timed<TResult>(
            Func<IAsyncObservable<int>, TestAsyncScheduler, IAsyncObservable<TResult>> build,
            long expected)
        {
            var o = new CountingAsyncObserver<TResult>();
            await VirtualTimeDriver.RunTimedAsync(N, PeriodTicks, o, o.Completion, build,
                static (s, sch, b) => b(s, sch));
            return Expect(o, expected, completed: true);
        }

        private static string? Expect<T>(CountingAsyncObserver<T> o, long count, bool completed) =>
            o.Count == count && o.Completed == completed && o.Error == null ? null : $"expected {count} values, {Describe(o)}";

        private static string Describe<T>(CountingAsyncObserver<T> o) =>
            $"got {o.Count} values, completed {o.Completed}, error {o.Error?.GetType().Name ?? "none"}";
    }
}
