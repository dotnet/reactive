# AsyncRx.NET benchmark suite

Infrastructure for measuring AsyncRx.NET (`System.Reactive.Async`) performance, built to support
before/after comparisons during performance work. It mirrors the Rx.NET suite in
`Rx.NET/Source/benchmarks` (same archetypes, conventions, columns and A/B tooling), adapted to an
`async`/`ValueTask` observer model.

| Folder | Purpose |
|---|---|
| `Benchmarks.System.Reactive.Async/` | The BenchmarkDotNet microbenchmark suite (153 benchmark classes and 550 benchmarks under `Operators/<Category>/`, shared infrastructure under `Infrastructure/`). Each folder is also the `[BenchmarkCategory]`, mostly mirroring the Rx suite's categories, plus `Threading` (the AsyncRx locking primitives). |
| `AsyncRx.WorkloadHarness/` | A long-running console host for system-level telemetry (`dotnet-counters` / `dotnet-trace` / `dotnet-monitor`): steady-state GC and, above all, thread-pool load, which microbenchmarks under-report. |
| `tools/` | `capture-baseline.ps1` and `compare-results.ps1`: thin wrappers over the Rx suite's scripts (see below). |
| `baselines/`, `traces/` | Local run outputs. **Gitignored**: keep them locally or publish them as CI artifacts. |

## What is different from the Rx suite

Read this before reading any numbers.

- **Many operators take an `AsyncGate` per element.** Merge, Zip, CombineLatest, SelectMany, Switch, Amb,
  WithLatestFrom, Synchronize, SequenceEqual, GroupBy, Window, the join operators, the Behavior/Replay
  subjects and connectable observables lock a gate on their per-element path. The gate does not leave
  the calling thread. An uncontended `LockAsync` returns a completed `ValueTask` under a monitor, and
  `Release` completes a contended waiter inline. These classes derive from `GatedOperatorBenchmarkBase`,
  which marks them as gated and uses the full sweep (N = 1 to 1,000,000). The gate is not re-entrant: a
  holder that locks it again waits forever. `Threading/AsyncGateBenchmarks` measures the gate on its own.
- **Some gated classes use a capped sweep (N = 1, 100, 10,000).** `SwitchBenchmarks`,
  `SelectManyBenchmarks` and `SelectManyOverloadBenchmarks` hold the total work at 100,000 elements
  (`TotalWork / N` per inner), so N = 1,000,000 would give empty inners. `ObserveOnBenchmarks` and
  `SynchronizationContextBenchmarks` stop at 10,000 because the `ObserveOn` queue is unbounded: the
  producer does not wait for the drain. These classes derive from `BenchmarkBase` and declare their own
  `[Params]`.
- **Explicit schedulers everywhere.** See "Scheduler policy" below.
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
  (N = 1, 1,000, 1,000,000). Gated operators use `GatedOperatorBenchmarkBase`, with the same 7-point
  sweep as `OperatorBenchmarkBase`. The capped classes listed above sweep N = 1, 100, 10,000.
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
completion signal (`CompletionSignal`) otherwise. The signal reports `IsSet` only after its result is
set, and `Reset` waits for a `Set` in progress, so a pipeline that completes on another thread cannot
race the next invocation. The sinks consume only expected errors: the shared
`FaultingSource.BenchmarkError`, or any error in a row that passes `allowErrors: true` (the `Single` rows
that fault on purpose). Any other error makes the benchmark throw `UnexpectedBenchmarkErrorException`, so a
library change that made a pipeline fail early shows as a failed run, not as a speed-up. `MemoryDiagnoser` is applied globally,
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

## Scheduler policy

Many AsyncRx operators default to `TaskPoolAsyncScheduler.Default` (`Range`, `Repeat`, `Generate`,
`ToAsyncObservable`, `ToAsync`/`Start`, every time operator). `TakeLast(count)` is an exception: its
scheduler-less overload drains on `ImmediateAsyncScheduler`. The suite does not rely on a default scheduler:

- Every row passes a scheduler explicitly: `ImmediateAsyncScheduler.Instance`, or the virtual-time
  `TestAsyncScheduler` in temporal classes. The shared cold source `BenchmarkSources.Range(n)` therefore
  pushes every element inline inside `SubscribeAsync`.
- A row that runs on the task pool on purpose is named `_TaskPool`.
- Rows named `_DefaultScheduler` measure the default path on purpose. There are four:
  `Range_DefaultScheduler`, `TakeLast_DefaultScheduler`, `ToAsyncObservable_DefaultScheduler` and
  `Start_Func_DefaultScheduler`. Do not add more, and do not add real-time rows.
  `TakeLast_DefaultScheduler` runs inline, because the default for `TakeLast(count)` is the immediate
  scheduler; it measures the scheduler-less overload, not a task-pool hand-off.
- The default-scheduler and default-clock siblings of measured overloads are not measured. They run the
  same code as the measured overload on a different scheduler.
- The `FromEvent` / `FromEventPattern` forms that capture the current synchronization context are not
  measured. The rows use the explicit-scheduler forms.

## Coverage

The suite measures every public overload that has its own code: its own sink, its own closure-building
body, or a shape that nothing else measures. Generated families are covered by representatives:

- `CombineLatest` / `Zip` with a tuple result at arities 2, 3, 4, 8 and 15, and with a selector at
  arities 2 and 8 (the `ValueTask` selector at arity 2).
- `FromAsyncPattern` at arities 0 and 2 for the `TResult` end and the `Action<IAsyncResult>` end, and at
  arity 0 for the `Unit`-returning end.
- `ToAsync` with a `Func` and with an `Action`, at arities 0 and 2, with an explicit scheduler.
- `FromEvent` with `Action` add/remove handlers at arities 0, 1, 2 and 3.
- The typed `Sum`, `Average`, `Min` and `Max` over `int`, `int?`, `long`, `float`, `double`, `double?`
  and `decimal`. The typed selector forms are measured over one element type each.
- `GroupBy` and `GroupByUntil` once per parameter dimension (key, element selector, comparer, capacity,
  `ValueTask` selectors, duration selector), not in every combination.

These overloads are skipped on purpose:

- Pure forwarders and aliases: the `*Async` aliases, `Empty` / `Never` / `Throw` with a witness,
  `Cast<TResult>` / `OfType<TResult>`, `StartWith(scheduler, IEnumerable)`, `Catch` /
  `OnErrorResumeNext(IEnumerable)`, `Amb(IEnumerable)`, `SelectMany(other)`, `ObserveOn` /
  `SubscribeOn(SynchronizationContext)` and `Take(count, scheduler)`. The binary, `IEnumerable` and
  scheduler forms of `Merge` all forward to `sources.ToAsyncObservable(scheduler).Merge()`, so one
  `Merge_Params` row stands for them.
- Default-scheduler and default-clock siblings (see "Scheduler policy").
- Generated family members beyond the representatives above (arity 16, other arities, other element
  types).
- Combinations of dimensions that are each measured once, such as the remaining `SelectMany`-over-
  `ValueTask` index/token/result-selector forms, or an async overload with a comparer.
- Error-only `Do` forms (the same adapter as the completion-only form), `ToEvent(IAsyncObservable<Unit>)`
  and the `HasObservers` getters.

`Binding/RefCountDelayBenchmarks.Publish_RefCount_DisconnectDelay` measures the `RefCount` path that
takes a disconnect delay. It covers the whole lifecycle: when the sequence completes, the subscription is
disposed, and the disconnect runs one delay later. `--verify` checks that the disconnect runs.

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

# Count thread-pool work items and lock contentions (useful for the task-pool rows)
dotnet run -c Release -f net10.0 -- --filter *ObserveOnBenchmarks* --threading
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
comparisons are classified. Most rows run inline on the calling thread, and the Rx suite's 2% threshold
suits them. Some rows run on the task pool: the `_TaskPool` and `_DefaultScheduler` rows, `ObserveOn`
and `SubscribeOn` on the task pool, the synchronization-context rows and the task-pool scheduler rows.
Their timings include thread-pool scheduling variance, so the example above uses 5%. On those rows,
give allocation changes more weight than timing changes.

## Workload harness (steady-state telemetry)

```powershell
cd AsyncRx.WorkloadHarness
dotnet run -c Release -f net10.0 -- merge                       # runs until Ctrl+C, prints its PID
dotnet-counters monitor -p <pid> System.Runtime                 # from a second terminal
```

Workloads: `buffer` (default; allocates a list per batch as a visible GC signal), `select` (no gates),
`merge` and `groupby` (a gate per element; the gate completes inline, so `ThreadPool Completed Work Item
Count` should not grow with the element rate). By default a
free-running producer awaits each `OnNextAsync` into a subject (`--rate-ms 0`); `--rate-ms N` uses
`AsyncObservable.Interval` instead (timer resolution makes small intervals inaccurate on Windows). Use
`--seconds N` for a bounded run.

## Known gaps

AsyncRx is alpha. Where a library bug stops an operator from being measured the way the Rx suite
measures it, the benchmark comments the row out, marked `// KNOWN GAP:` in the source.
`System.Reactive.Async` itself is not changed. Restore the original shape when the library is fixed.

| Operator | Bug | What the suite does instead |
|---|---|---|
| `async IAsyncObservable<T>` methods (`AsyncObservableMethodBuilder<T>`) | When the method completes synchronously, the builder's `TaskObservable` emits the value but never calls `OnCompletedAsync`, so a subscriber waiting for termination hangs. | Synchronous row commented out; the method awaits `Task.Yield()` so it completes through the builder's async subject. |

Earlier gaps are fixed in the library and their rows are measured again: early termination over a cold
source, `Skip` / `Take(duration, scheduler)` and `SkipUntil(startTime, scheduler)` (now in virtual time),
`Window(count)` and gapped `Window(count, skip)`, `Materialize().Dematerialize()`, the generated and
`params` `Zip` over cold sources, `Amb(params)`, `FromEvent` / `FromEventPattern` disposal, and
`ObserveOn` on the immediate scheduler, the task pool and a synchronization context. `--verify` checks
each restored shape.

Design notes (not bugs):

- **Operators that terminate before their source** (`Take(count)`, `First`, `FirstOrDefault`, `Any()`,
  `IsEmpty`, `Single` / `SingleOrDefault` faulting) run through `Pump.RunUntilDoneAsync`: a hot
  `PushSource` that stops pushing once the sink has terminated. A cold source that pushes inside
  `SubscribeAsync` (such as `Range` on the immediate scheduler) has not yet returned its subscription
  when the operator terminates, so it cannot be cancelled. It pushes its remaining elements into the
  terminated observer, which drops them, so the work would grow with N instead of with the operator's
  needs. Operators that end on the last element (`ElementAt`, `ElementAtOrDefault`, `First(predicate)`,
  `FirstOrDefault(predicate)`) use the cold `Range(N)`.
- **Infinite `Repeat` cannot be cancelled on the immediate scheduler.** `Repeat(value, scheduler)` runs
  its push loop inside `SubscribeAsync`, so `Take(N)` never receives a subscription to dispose, and
  `source.Repeat()` over a cold source that completes synchronously loops for the same reason.
  `RepeatInfiniteBenchmarks` runs `Repeat(value)` on the task pool and drives `source.Repeat()` from a
  `PushSource`.

API quirks that shape the benchmarks (not bugs):

- Some overloads are declared without `this` and are called as static methods
  (`AsyncObservable.MinBy(source, ...)`). `Distinct`, `DistinctUntilChanged`, the generic `Min` / `Max`
  and `MinBy` / `MaxBy` are extension methods now; the older rows still use the static call form, which
  binds the same overloads.
- `Cast` and `OfType` have a two-type-argument form (`Cast<object, int>()`), which the rows measure, and
  a `Cast<TResult>()` / `OfType<TResult>()` form over `IAsyncObservable<object>` that forwards to it.
- `Sum()` over `int`/`int?` is checked, so those rows use small pre-built values.
- `Sample(sampler)` completes at the first sample after the source completes (Rx completes with the
  source), so the benchmark fires one final sample.
- `IAsyncObservable<T>` is not declared task-like, so `async IAsyncObservable<T>` methods name
  `AsyncObservableMethodBuilder<>` with a method-level `[AsyncMethodBuilder]`.
- `GroupJoin`-based `Buffer` / `Window` with openings complete only when the openings complete, so
  `BufferWindowOpeningsBenchmarks` pushes a final closing after the loop.

Not measured because AsyncRx has no equivalent: the Rx `Blocking`, `Experimental` (`ObservableEx`) and
`Queryable` (`Qbservable`) categories; `ToTask` (covered by the awaiter rows); the notification-to-observer
bridge (internal). The Rx suite's `Interval_EmulatedPeriodic` has no counterpart because AsyncRx has no
periodic scheduler interface.

Upstream has since added operators that this list used to name. They are now measured: `Merge` with
`maxConcurrent`, `params` and `Task` inners; `TakeUntil(predicate)`; `CombineLatest` / `Zip` over an
`IEnumerable` of sources; `RepeatWhen` / `RetryWhen`; `SelectMany` over `ValueTask` and `IEnumerable`;
the `Delay` / `Timeout` selector and absolute-time forms; and join patterns (`And`) over more than two
sources.
