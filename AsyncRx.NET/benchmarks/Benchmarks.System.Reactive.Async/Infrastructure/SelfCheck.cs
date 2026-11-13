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
                    // On a pool thread, so that a pipeline that hangs synchronously (in the TestAsyncScheduler pump or on
                    // ImmediateAsyncScheduler) cannot block the guard below.
                    var run = Task.Run(check);
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

            await Check("Pump.PushPairsAsync into a Merge of two sources delivers 2 x count", async () =>
            {
                var a = new PushSource<int>();
                var b = new PushSource<int>();
                var o = new CountingAsyncObserver<int>();
                var merged = new ArrayAsyncSource<IAsyncObservable<int>>(new IAsyncObservable<int>[] { a, b }).Merge();
                await merged.SubscribeAsync(o);
                await Pump.PushPairsAsync(a, b, N / 2);
                await a.OnCompletedAsync();
                await b.OnCompletedAsync();
                return Expect(o, N, completed: true);
            });

            // Observers that complete asynchronously take Pump's slow paths; the interleaved order must survive them.
            await Check("Pump.PushPairsAsync keeps the interleaved order when the observers yield", async () =>
            {
                var order = new List<string>();
                await Pump.PushPairsAsync(new YieldingRecorder("a", order), new YieldingRecorder("b", order), 3);
                var actual = string.Join(" ", order);
                return actual == "a0 b0 a1 b1 a2 b2" ? null : $"got '{actual}'";
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

            // Without this, a pipeline that started to fail early would be measured as a fast row.
            await Check("Cached sink fails on an unexpected error and consumes expected ones", async () =>
            {
                var consumer = new BenchmarkDotNet.Engines.Consumer();
                try
                {
                    await AsyncObservable.Throw<int>(new TimeoutException()).SubscribeConsumeAsync(consumer);
                    return "an unexpected error did not throw UnexpectedBenchmarkErrorException";
                }
                catch (UnexpectedBenchmarkErrorException)
                {
                }

                await AsyncObservable.Throw<int>(FaultingSource.BenchmarkError).SubscribeConsumeAsync(consumer);
                await AsyncObservable.Throw<int>(new TimeoutException()).SubscribeConsumeAsync(consumer, allowErrors: true);
                await BenchmarkSources.Range(N).SubscribeConsumeAsync(consumer); // the failed signal re-arms cleanly
                return null;
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

            await Check("ObserveOn(TaskPoolAsyncScheduler) delivers N", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await BenchmarkSources.Range(N).ObserveOn(TaskPoolAsyncScheduler.Default).SubscribeAwaitAsync(o, o.Completion);
                return Expect(o, N, completed: true);
            });

            await Check("ObserveOn(SynchronizationContextAsyncScheduler) delivers N", async () =>
            {
                var scheduler = new SynchronizationContextAsyncScheduler(new Operators.Concurrency.SynchronizationContextBenchmarks.InlineSynchronizationContext());
                var o = new CountingAsyncObserver<int>();
                await BenchmarkSources.Range(N).ObserveOn(scheduler).SubscribeAwaitAsync(o, o.Completion);
                return Expect(o, N, completed: true);
            });

            // Windows of 4 opened every 8 elements: half of the elements fall in a gap between windows.
            await Check("Gapped Window(4, 8) flattened delivers N/2", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await BenchmarkSources.Range(N).Window(4, 8).Merge().SubscribeAwaitAsync(o, o.Completion);
                return Expect(o, N / 2, completed: true);
            });

            await Check("Cold generated Zip of 3 sources delivers N", async () =>
            {
                var s = BenchmarkSources.Range(N);
                var o = new CountingAsyncObserver<(int, int, int)>();
                await s.Zip(s, s).SubscribeAwaitAsync(o, o.Completion);
                return Expect(o, N, completed: true);
            });

            await Check("Cold Zip(IEnumerable of 4 sources) delivers N", async () =>
            {
                var s = BenchmarkSources.Range(N);
                var o = new CountingAsyncObserver<int>();
                await AsyncObservable.Zip(new List<IAsyncObservable<int>> { s, s, s, s }, static (IList<int> l) => l[0] + l[3])
                    .SubscribeAwaitAsync(o, o.Completion);
                return Expect(o, N, completed: true);
            });

            await Check("Early termination over a cold source: Range(N).Take(10) delivers 10", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await BenchmarkSources.Range(N).Take(10).SubscribeAwaitAsync(o, o.Completion);
                return Expect(o, 10, completed: true);
            });

            await Check("FromEvent delivers N and disposal removes the handler", async () =>
            {
                var host = new EventHost();
                var o = new CountingAsyncObserver<int>();
                var subscription = await AsyncObservable.FromEvent<int>(h => host.Value += h, h => host.Value -= h, ImmediateAsyncScheduler.Instance)
                    .SubscribeAsync(o);
                for (var i = 0; i < N; i++)
                {
                    host.Raise(i);
                }

                await subscription.DisposeAsync();
                return o.Count == N && !host.HasHandlers ? null : $"{Describe(o)}, handler still attached {host.HasHandlers}";
            });

            await Check("FromEventPattern delivers N and disposal removes the handler", async () =>
            {
                var host = new EventHost();
                var o = new CountingAsyncObserver<global::System.Reactive.EventPattern<object>>();
                var subscription = await AsyncObservable.FromEventPattern(h => host.Plain += h, h => host.Plain -= h, ImmediateAsyncScheduler.Instance)
                    .SubscribeAsync(o);
                for (var i = 0; i < N; i++)
                {
                    host.RaisePlain();
                }

                await subscription.DisposeAsync();
                return o.Count == N && !host.HasHandlers ? null : $"{Describe(o)}, handler still attached {host.HasHandlers}";
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
                Timed(static (s, sch) => s.Skip(TimeSpan.FromTicks(VirtualTimeDriver.MidpointTicks(N, PeriodTicks)), sch), N - N / 2));

            await Check("Virtual time: Take(duration) at mid-timeline delivers N/2", () =>
                Timed(static (s, sch) => s.Take(TimeSpan.FromTicks(VirtualTimeDriver.MidpointTicks(N, PeriodTicks)), sch), N / 2));

            await Check("Virtual time: SkipUntil(startTime) at mid-timeline delivers N/2", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await VirtualTimeDriver.RunTimedAsync(N, PeriodTicks, o, o.Completion, 0,
                    static (s, sch, _) => s.SkipUntil(sch.Now.AddTicks(VirtualTimeDriver.MidpointTicks(N, PeriodTicks)), sch));
                return Expect(o, N - N / 2, completed: true);
            });

            await Check("Virtual time: TakeUntil(endTime) stops at the end time", async () =>
            {
                var o = new CountingAsyncObserver<int>();
                await VirtualTimeDriver.RunTimedAsync(N, PeriodTicks, o, o.Completion, 0,
                    static (s, sch, _) => s.TakeUntil(sch.Now.AddTicks(VirtualTimeDriver.MidpointTicks(N, PeriodTicks)), sch));
                return o.Count == N / 2 && o.Completed ? null : $"expected {N / 2}, {Describe(o)}";
            });

            // An odd element count: the shared midpoint must still fall between two elements.
            await Check("Virtual time: Take(duration) at the midpoint of an odd count delivers count/2", async () =>
            {
                const int odd = N + 1;
                var o = new CountingAsyncObserver<int>();
                await VirtualTimeDriver.RunTimedAsync(odd, PeriodTicks, o, o.Completion, 0,
                    static (s, sch, _) => s.Take(TimeSpan.FromTicks(VirtualTimeDriver.MidpointTicks(odd, PeriodTicks)), sch));
                return Expect(o, odd / 2, completed: true);
            });

            // RefCountDelayBenchmarks relies on this. Completion disposes the RefCount subscription (AutoDetach), which
            // schedules the delayed disconnect. The pump runs until no work remains, so its final clock is one delay
            // past the last push only if the disconnect timer ran.
            await Check("Virtual time: Publish().RefCount(delay) runs the delayed disconnect after completion", async () =>
            {
                TestAsyncScheduler? scheduler = null;
                var o = new CountingAsyncObserver<int>();
                await VirtualTimeDriver.RunTimedAsync(N, PeriodTicks, o, o.Completion, 0,
                    (s, sch, _) =>
                    {
                        scheduler = sch;
                        return s.Publish().RefCount(TimeSpan.FromTicks(PeriodTicks), sch);
                    });
                var expectedEnd = PeriodTicks * N + PeriodTicks; // the last push (and completion) is at PeriodTicks * N
                return o.Count == N && o.Completed && scheduler!.Clock == expectedEnd
                    ? null
                    : $"{Describe(o)}, timeline ended at tick {scheduler?.Clock} (expected {expectedEnd})";
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

            // DisposeBenchmarks.Throttle_Dispose relies on this: every push cancels the previous timer, and disposal
            // cancels the last one, so nothing is emitted and the timeline ends at the disposal, one period after
            // the last push. A timer that disposal did not cancel would run later and move the clock past that tick.
            await Check("Virtual time: Throttle disposed mid-stream cancels its timer and emits nothing", () =>
            {
                var o = new CountingAsyncObserver<int>();
                var period = Operators.Lifecycle.DisposeBenchmarks.ThrottlePeriodTicks;
                var scheduler = VirtualTimeDriver.RunTimedThenDispose(N, period, o,
                    Operators.Lifecycle.DisposeBenchmarks.ThrottleWindow, Operators.Lifecycle.DisposeBenchmarks.ThrottlePipeline);
                var expectedEnd = period * (N + 1);
                return Task.FromResult(o.Count == 0 && !o.Completion.IsSet && scheduler.Clock == expectedEnd
                    ? null
                    : $"{Describe(o)}, timeline ended at tick {scheduler.Clock} (expected {expectedEnd})");
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

        private sealed class EventHost
        {
            public event Action<int>? Value;

            public event EventHandler? Plain;

            public bool HasHandlers => Value != null || Plain != null;

            public void Raise(int value) => Value?.Invoke(value);

            public void RaisePlain() => Plain?.Invoke(this, EventArgs.Empty);
        }

        // Records each value after yielding, so every OnNextAsync completes asynchronously.
        private sealed class YieldingRecorder : IAsyncObserver<int>
        {
            private readonly string _name;
            private readonly List<string> _order;

            public YieldingRecorder(string name, List<string> order)
            {
                _name = name;
                _order = order;
            }

            public async ValueTask OnNextAsync(int value)
            {
                await Task.Yield();
                lock (_order)
                {
                    _order.Add(_name + value);
                }
            }

            public ValueTask OnErrorAsync(Exception error) => default;

            public ValueTask OnCompletedAsync() => default;
        }
    }
}
