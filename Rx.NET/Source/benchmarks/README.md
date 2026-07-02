# Rx.NET benchmark suite

Infrastructure for measuring Rx.NET performance, built to support before/after comparisons during
performance work (e.g. the .NET 10 modernization spike).

| Folder | Purpose |
|---|---|
| `Benchmarks.System.Reactive/` | The BenchmarkDotNet microbenchmark suite (~100 operator classes under `Operators/<Category>/`, shared infrastructure under `Infrastructure/`). |
| `Rx.WorkloadHarness/` | A long-running console host for system-level telemetry (`dotnet-counters` / `dotnet-trace` / `dotnet-monitor`) — the steady-state GC/thread-pool view microbenchmarks can't give. |
| `tools/` | `capture-baseline.ps1` and `compare-results.ps1` — the A/B workflow (see below). |
| `baselines/`, `traces/` | Local run outputs. **Gitignored** — keep them locally or publish as CI artifacts. |

## Scenario archetypes

Every benchmark class follows one of four shapes:

- **S1 throughput** — cold synchronous pipelines (`Observable.Range`), swept over `N` = 1…1,000,000 elements (`OperatorBenchmarkBase`).
- **S2 hot push** — a pumped `Subject<T>` exercising the per-element `OnNext` dispatch path.
- **S3 temporal** — time-based operators driven in *virtual time* (`HistoricalScheduler` /
  `PeriodicVirtualScheduler`), so even dense timelines complete in microseconds of wall clock with
  zero timing noise. Operators with a periodic path (`Interval`, `Timer`, `Sample`, `Buffer(time)`,
  `Window(time)`) use `PeriodicVirtualScheduler`, which implements `ISchedulerPeriodic` so Rx's real
  periodic fast path is measured rather than the stopwatch-emulated fallback.
- **S4 subscription/lifecycle** — construction, subscription, and disposal costs in isolation.

Results are consumed through a shared `Consumer` sink (defeating dead-code elimination); async
pipelines block on real completion (`SubscribeBlocking`). `MemoryDiagnoser` is applied globally, so
allocation is a first-class column everywhere, alongside per-element `ns/N` (mean time / N) and
`B/N` (allocated bytes / N) columns for classes with an `N` parameter.

Reading the per-element columns:

- Small-N rows are dominated by fixed subscription/setup cost, so `ns/N` and `B/N` only converge
  to the true steady-state per-element cost as N grows (e.g. `Select`: 544 `B/N` at N=1 flattening
  to a constant 64 `B/N` from N≈1,000 up). Read the large-N rows for per-element cost; read the
  small-N rows for per-subscription overhead.
- A `B/N` that stays flat as N grows means the operator allocates *per element* — the primary
  zero-allocation target the columns exist to surface.
- For fan-out classes (SelectMany/Merge/Switch cross-map variants), `N` is the fan-out width with
  total work pinned, so the figures read as per-subscription rather than per-element.
- The columns are display-only; the exported JSON keeps raw statistics, and `compare-results.ps1`
  recomputes per-element values itself.

## Running

```powershell
cd Benchmarks.System.Reactive

# Everything (long!) on .NET 10
dotnet run -c Release -f net10.0 -- --filter *

# One class / one category
dotnet run -c Release -f net10.0 -- --filter *SelectBenchmarks*
dotnet run -c Release -f net10.0 -- --anyCategories Time

# Cross-runtime comparison in one table
dotnet run -c Release -f net10.0 -- --filter *SelectBenchmarks* --runtimes net472 net8.0 net10.0

# Fast sanity pass (no statistics — just checks benchmarks execute)
dotnet run -c Release -f net10.0 -- --filter * --job Dry
```

Opt-in profiling (layered on the shared config per run):

```powershell
dotnet run -c Release -f net10.0 -- --filter *Merge* --profiler ETW    # Windows, elevated: ETW traces
dotnet run -c Release -f net10.0 -- --filter *Merge* --profiler EP     # cross-platform EventPipe
dotnet run -c Release -f net10.0 -- --filter *Select* --disasm         # JIT disassembly
```

Note: `System.Reactive` currently tops out at `net8.0`, so net9/net10 runs execute the net8-compiled
assembly on the newer runtime — they show JIT/runtime gains only. Product code-path changes need a
same-TFM before/after comparison, which is what the tools below provide.

## Before/after workflow (A/B)

```powershell
# 1. On the baseline commit:
tools\capture-baseline.ps1 -Filter * -Framework net10.0

# 2. Check out the candidate commit (or apply your change), then:
tools\capture-baseline.ps1 -Filter * -Framework net10.0

# 3. Compare the two stamped folders:
tools\compare-results.ps1 -Baseline baselines\2026-07-01-abc1234 -Candidate baselines\2026-07-02-def5678
```

`capture-baseline.ps1` runs the suite, stamps results into `baselines/<date>-<shortsha>[-label]/results/`
(full JSON + GitHub markdown + CSV per class), and records the environment (commit, branch,
`dotnet --info`) alongside. It refuses to stamp a dirty working tree unless `-AllowDirty` is passed.

`compare-results.ps1` matches benchmarks by full name (including parameter values) across the two
result sets, classifies each as regression / improvement / unchanged using a ratio threshold
(default 2%) plus the reported confidence intervals, and writes a markdown report sorted
worst-regression-first.

## Workload harness (steady-state telemetry)

```powershell
cd Rx.WorkloadHarness
dotnet run -c Release -f net10.0 -- buffer --rate-ms 1          # runs until Ctrl+C, prints its PID
dotnet-counters monitor -p <pid> System.Runtime                 # from a second terminal
```

Workloads: `buffer` (default; allocates a `List` per batch as a visible GC signal), `select`,
`merge`, `groupby`. Use `--seconds N` for a bounded run.
