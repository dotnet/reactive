# AsyncRx.NET benchmark suite

Infrastructure for measuring AsyncRx.NET (`System.Reactive.Async`) performance, built to support
before/after comparisons during performance work. It mirrors the Rx.NET suite in
`Rx.NET/Source/benchmarks` (same archetypes, conventions, columns and A/B tooling), adapted to an
`async`/`ValueTask` observer model.

| Folder | Purpose |
|---|---|
| `Benchmarks.System.Reactive.Async/` | The BenchmarkDotNet microbenchmark suite (118 benchmark classes under `Operators/<Category>/`, shared infrastructure under `Infrastructure/`). Each folder is also the `[BenchmarkCategory]`, mostly mirroring the Rx suite's categories, plus `Threading` (the AsyncRx locking primitives). |
| `AsyncRx.WorkloadHarness/` | A long-running console host for system-level telemetry (`dotnet-counters` / `dotnet-trace` / `dotnet-monitor`): steady-state GC and, above all, thread-pool load, which microbenchmarks under-report. |
| `tools/` | `capture-baseline.ps1` and `compare-results.ps1`: thin wrappers over the Rx suite's scripts (see below). |
| `baselines/`, `traces/` | Local run outputs. **Gitignored**: keep them locally or publish them as CI artifacts. |

## What is different from the Rx suite

Read this before reading any numbers.

- **`AsyncGate` hops to the thread pool on every acquisition.** `AsyncGate.LockAsync` acquires its
  semaphore through `SemaphoreSlim.WaitAsync().ContinueWith(...)` without `ExecuteSynchronously`, so even
  an uncontended lock queues a work item. Every operator whose per-element path takes a gate (Merge, Zip,
  CombineLatest, SelectMany, Switch, Amb, WithLatestFrom, Synchronize, ObserveOn, SequenceEqual, the
  join operators, Behavior/Replay subjects, connectable observables, and the timed operators) therefore
  pays a thread-pool round trip per element, and completes on a pool thread. These classes use
  `GatedOperatorBenchmarkBase`, a capped sweep of N = 1, 100, 10,000. Their timings include thread-pool
  scheduling variance; the Allocated column is close to deterministic and is the more reliable A/B
  signal for them. `Threading/AsyncGateBenchmarks` measures the gate on its own.
- **Explicit schedulers everywhere.** Many AsyncRx operators default to `TaskPoolAsyncScheduler.Default`
  (`Range`, `Repeat`, `Generate`, `TakeLast(count)`, `ToAsyncObservable`, `ToAsync`/`Start`, every time
  operator). The suite passes the immediate scheduler (or the virtual-time scheduler) explicitly, so the
  shared cold source `BenchmarkSources.Range(n)` pushes every element inline inside `SubscribeAsync`.
  Rows named `_DefaultScheduler` measure the default path on purpose.
- **Virtual time comes from `TestAsyncScheduler`.** The temporal benchmarks use the virtual-time
  scheduler from `Microsoft.Reactive.Testing.Async` (see the S3 archetype and "Virtual time" below).
  The benchmark project references that library; it brings the `Microsoft.Reactive.Testing` package
  and no test framework.
- **Hot sources are benchmark-owned.** `PushSource<T>` is a single-observer source the benchmark pushes
  into directly. The library subjects copy their observer list per call (simple subjects) or take a gate
  (Behavior/Replay), so they are measured on their own in `Hot/SubjectBenchmarks` and kept out of the
  operator benchmarks.
- **No Rx-vs-AsyncRx comparison.** The semantics differ (awaited backpressure), and the Rx assembly
  available to this project is the System.Reactive 6.0.0 package rather than the in-repo sources, so a
  cross-library ratio is not an A/B signal. `Overhead/OverheadBenchmarks` anchors AsyncRx against plain
  loops instead.

## Scenario archetypes

- **S1 throughput**: cold pipelines over `BenchmarkSources.Range(n)` (immediate scheduler), swept over
  `N` = 1…1,000,000 (`OperatorBenchmarkBase`). Overload-variant classes use `OperatorVariantBenchmarkBase`
  (N = 1, 1,000, 1,000,000); gated operators use `GatedOperatorBenchmarkBase` (N = 1, 100, 10,000).
  `ArrayAsyncSource<T>` pushes a pre-built array where an overload needs a pre-typed source (typed
  aggregates, boxed objects, strings, inner observables). Benchmark methods return `ValueTask`; rows
  named `_Async` use the `ValueTask`-returning selector/predicate overload with a synchronously
  completed result.
- **S2 hot push**: the benchmark awaits `OnNextAsync` per element into a `PushSource<T>` (operators) or a
  library subject (`SubjectBenchmarks`), the AsyncRx backpressure contract.
- **S3 temporal**: time-based operators driven by `TestAsyncScheduler`. Each temporal class inherits
  `Source_Baseline` (N pushes one period apart, then completion; no operator) as its baseline, so the
  Ratio column reads as operator cost over the cost of driving the timeline, scheduler included. `Density`
  (Sparse / Boundary / Dense) varies the window against the arrival period for the rate/window operators.
- **S4 subscription/lifecycle**: construction vs subscription (`ConstructionBenchmarks`), disposal of a
  still-live subscription (`DisposeBenchmarks`), and the async disposable building blocks
  (`AsyncDisposableBenchmarks`).

Results go to a shared BenchmarkDotNet `Consumer`. `SubscribeConsumeAsync` subscribes a cached
`ConsumingAsyncObserver<T>` and completes when the sequence terminates; it is allocation-free when the
pipeline runs to completion inside `SubscribeAsync`, and waits on a reusable `IValueTaskSource`
completion signal otherwise. Errors are consumed, not rethrown. `MemoryDiagnoser` is applied globally,
with per-element `ns/N` and `B/N` columns as in the Rx suite (read large-N rows for per-element cost,
small-N rows for per-subscription cost; for the pinned-work fan-out classes N is the inner-subscription
count).

### Virtual time

`TestAsyncScheduler` is a synchronous, single-threaded pump. `Start` runs the timeline to the end on the
calling thread: it drains the work ready at the current tick (continuations first, then newly scheduled
work), then moves the clock to the next due timer, and repeats until nothing is left. Every await
continuation resumes inline on the pump thread, so a temporal benchmark is deterministic and completes
synchronously, with no thread-pool work.

`VirtualTimeDriver` (in `TemporalBenchmarkBase.cs`) drives each run:

1. Subscribe the pipeline from the benchmark body, before the pump has run anything.
2. Schedule the pushes as a chain of timed work items. Each one pushes a value and schedules the next
   one period later. The last one completes the source.
3. Call `Start`, then require the sink to have terminated.

The pushes are scheduled work rather than calls made from the benchmark body between `AdvanceBy` calls.
An operator that cancels or completes an in-flight timer in response to a push would otherwise do it
off the pump, and the scheduler reports that as an escape.

Failures fail the benchmark instead of hanging it:
- `Start` throws if scheduled work faults, escapes to a real thread (`Task.Run`, `Task.Yield`, real
  timers), or is still outstanding when the timeline runs out.
- `Start` also throws if 100,000 items run at one tick without time advancing.
- The driver throws if the sink never received `OnCompleted` or `OnError`.

At the same tick, items run in the order they were scheduled. A push and an operator timer due at the
same tick therefore run in scheduling order, not "timers first".

Per scheduled item, `TestAsyncScheduler` allocates a cancellation source, its work record, an execution
context capture, and (for timed work) a `TaskCompletionSource` and a timer entry. That cost is in every
temporal row, `Source_Baseline` included, so compare operator rows against the baseline rather than
reading their absolute allocations.

## Running

```powershell
cd Benchmarks.System.Reactive.Async

# Check the benchmark infrastructure first: sources, sinks and the virtual-time pump
dotnet run -c Release -f net10.0 -- --verify

# Everything (long!) on .NET 10
dotnet run -c Release -f net10.0 -- --filter *

# One class / one category
dotnet run -c Release -f net10.0 -- --filter *SelectBenchmarks*
dotnet run -c Release -f net10.0 -- --anyCategories Time

# Cross-runtime comparison in one table
dotnet run -c Release -f net10.0 -- --filter *SelectBenchmarks* --runtimes net8.0 net9.0 net10.0

# Fast sanity pass (no statistics: just checks benchmarks execute)
dotnet run -c Release -f net10.0 -- --filter * --job Dry

# Confirm the thread-pool hops of a gated operator
dotnet run -c Release -f net10.0 -- --filter *MergeBenchmarks* --threading
```

Opt-in profiling works as in the Rx suite (`--profiler ETW`, `--profiler EP`, `--disasm`).

Notes:

- The project targets `net8.0;net9.0;net10.0`. Building it needs the .NET 10 SDK, as the AsyncRx CI
  pipeline already uses.
- `System.Reactive.Async` targets `netstandard2.0` and `netstandard2.1`; every runtime here executes
  the `netstandard2.1` build, so cross-runtime runs show JIT/runtime gains only.

## Before/after workflow (A/B)

```powershell
# 1. On the baseline commit:
tools\capture-baseline.ps1 -Filter * -Framework net10.0

# 2. Check out the candidate commit (or apply your change), then:
tools\capture-baseline.ps1 -Filter * -Framework net10.0

# 3. Compare the two stamped folders:
tools\compare-results.ps1 -Baseline baselines\2026-07-01-abc1234 -Candidate baselines\2026-07-02-def5678 -ThresholdPercent 5
```

Both scripts forward to the Rx suite's `tools` scripts; `capture-baseline.ps1` passes `-ProjectDir` so
captures land in `AsyncRx.NET/benchmarks/baselines/<date>-<shortsha>[-label]/` with the same
`environment.json` and `dotnet-info.txt`. See the Rx suite README for what they record and how
comparisons are classified. Because gated operators pay a pool hop per element, a 5% threshold is a
better default here than the Rx suite's 2%, and allocation changes deserve more weight than timing
changes on those rows.

## Workload harness (steady-state telemetry)

```powershell
cd AsyncRx.WorkloadHarness
dotnet run -c Release -f net10.0 -- merge                       # runs until Ctrl+C, prints its PID
dotnet-counters monitor -p <pid> System.Runtime                 # from a second terminal
```

Workloads: `buffer` (default; allocates a list per batch as a visible GC signal), `select` (no gates),
`merge` and `groupby` (a gate per element; watch `ThreadPool Completed Work Item Count`). By default a
free-running producer awaits each `OnNextAsync` into a subject (`--rate-ms 0`); `--rate-ms N` uses
`AsyncObservable.Interval` instead (timer resolution makes small intervals inaccurate on Windows). Use
`--seconds N` for a bounded run.

## Known gaps

AsyncRx is alpha. Where a library bug stops an operator from being measured the
way the Rx suite measures it, the benchmark works around it or comments the row out, marked
`// KNOWN GAP:` in the source. `System.Reactive.Async` itself is not changed. Restore the original shape
when the library is fixed.

| Operator | Bug | What the suite does instead |
|---|---|---|
| `Take(count)`, `First`, `FirstOrDefault`, `Any()`, `IsEmpty`, `ElementAt`, `ElementAtOrDefault`, `Single`/`SingleOrDefault` (faulting), `First(predicate)` | An operator that terminates before its source does, over a cold source that pushes inside `SubscribeAsync` (such as `Range` on the immediate scheduler), throws `InvalidOperationException("The observer has already terminated.")`. The source has not yet returned its subscription, so it cannot be cancelled and keeps calling the terminated observer. | Driven by `Pump.RunUntilDoneAsync`: a hot `PushSource` that stops pushing once the sink has terminated. |
| `Skip(duration, scheduler)`, `Take(duration, scheduler)` | The scheduler argument is ignored; the timer runs on `TaskPoolAsyncScheduler.Default` in real time. | Measured with a one-day real duration that never fires (`_NeverFires` rows): per-element cost only, not the transition. |
| `SkipUntil(endTime, scheduler)` | The scheduler argument is ignored (real clock, task pool). | Measured with an end time one real day ahead (`_NeverFires`). |
| `Window(count)`, `Window(count, skip)` with `skip > count` | Throws `InvalidOperationException("Queue empty")` once `count` elements have arrived: the first window opens only after `skip` elements. | Rows commented out; only the overlapping `Window(16, 8)` is measured (also in `DisposeBenchmarks`). |
| `Materialize().Dematerialize()` | Throws "The observer has already terminated." for every N: Dematerialize completes on the materialized OnCompleted, then Materialize forwards its own OnCompleted. | Row commented out; `Materialize` alone is measured. |
| Generated `Zip` overloads (2 to 15 sources) | Observers use 1-based indices against a 0-based `isDone` array, so completing the last source throws `IndexOutOfRangeException`. | Hot sources, never completed: pushes, then dispose (`Pump.PushAllAndDisposeAsync`). Measures the pairing path, not completion. |
| `Zip(params)` | Over cold sources it completes when one source drains, and the others then hit the terminated observer. | Same hot, never-completed approach. |
| `Amb(params)`, `Amb(IEnumerable)` | Throws `IndexOutOfRangeException`: the `ContinueWith` assigning each subscription captures the loop variable. | Row commented out; binary `Amb` is measured. |
| `async IAsyncObservable<T>` methods (`AsyncObservableMethodBuilder<T>`) | When the method completes synchronously, the builder's `TaskObservable` emits the value but never calls `OnCompletedAsync`, so a subscriber waiting for termination hangs. | Synchronous row commented out; the method awaits `Task.Yield()` so it completes through the builder's async subject. |
| `FromEvent`, `FromEventPattern` (all forms) | Disposing the subscription throws `OperationCanceledException`: the `SubscribeOn` dispose action awaits a `RendezVous` on the token it has just cancelled. | `Pump.DisposeIgnoringCancellationAsync` swallows it; a fresh event host per invocation. |
| `ObserveOn(ImmediateAsyncScheduler.Instance)` | Never completes (hung at N = 1). Not isolated further; the immediate scheduler appears to run the drain loop inline inside `SubscribeAsync`. | Row commented out. |
| `ObserveOn(TaskPoolAsyncScheduler)`, `ObserveOn(SynchronizationContextAsyncScheduler)` | Pass at N = 1 and 100 but hang at larger N (task pool: hung at 1,000 and 10,000; synchronization context: at 10,000), on two different schedulers, which points to a per-element race in the library's scheduled observer. Not isolated further. | `ObserveOnBenchmarks` and `SynchronizationContextBenchmarks` sweep only N = 1 and 100. |

API quirks that shape the benchmarks (not bugs):

- `Distinct`, `DistinctUntilChanged`, the generic `Min`/`Max` and `MinBy`/`MaxBy` are declared without
  `this`, so they are called as static methods (`AsyncObservable.Distinct(source)`).
- `Cast` and `OfType` take both type arguments (`Cast<object, int>()`).
- `Sum()` over `int`/`int?` is checked, so those rows use small pre-built values.
- `Sample(sampler)` completes at the first sample after the source completes (Rx completes with the
  source), so the benchmark fires one final sample.
- `IAsyncObservable<T>` is not declared task-like, so `async IAsyncObservable<T>` methods name
  `AsyncObservableMethodBuilder<>` with a method-level `[AsyncMethodBuilder]`.

Not measured because AsyncRx has no equivalent: the Rx `Blocking`, `Experimental` (`ObservableEx`) and
`Queryable` (`Qbservable`) categories; `Merge` with `maxConcurrency`/params/`IEnumerable`/`Task`;
`TakeUntil(predicate)`; `CombineLatest`/`Zip` over `IEnumerable` of sources; `RepeatWhen`/`RetryWhen`;
`SelectMany` over `Task`/`IEnumerable`; `Delay`/`Timeout` selector and absolute-time overloads;
`ToTask` (covered by the awaiter rows); join patterns over three or more sources (no public `And` on
`AsyncPattern<T1, T2>`); the notification-to-observer bridge (internal). The Rx suite's
`Interval_EmulatedPeriodic` has no counterpart because AsyncRx has no periodic scheduler interface.
