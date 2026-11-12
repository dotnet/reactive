# `IAsyncScheduler` State Argument

`IAsyncScheduler.ScheduleAsync` takes a caller-supplied state argument and passes it to the scheduled action, as `IScheduler.Schedule` does in Rx.NET.

## Status

Proposed.

## Context

Rx.NET's `IScheduler` has always (since Rx 2.0) taken a `TState` argument on every `Schedule` method:

```csharp
IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action);
```

Operators pass their sink as the state with a static (non-capturing) lambda, so scheduling a unit of work allocates neither a closure nor a delegate. AsyncRx.NET's `IAsyncScheduler`, written in September 2017 as a placeholder that was filled in the next day, had no such argument:

```csharp
ValueTask<IAsyncDisposable> ScheduleAsync(Func<CancellationToken, ValueTask> action);
```

No commit, comment or discussion records why. The original author had already designed an async-callback scheduling API with a state argument for Rx.NET (`Scheduler.ScheduleAsync<TState>` in `Scheduler.Async.cs`, 2012), so it was not a belief that async callbacks and state do not mix. The most likely explanation is structural: `IAsyncScheduler` also dropped the scheduler argument from the callback, because recursive scheduling (the other thing Rx.NET threads `TState` through) is replaced in AsyncRx.NET by a loop inside one long-running async action with a `CancellationToken`. Once both were gone, the signature was simply the `Task.Run` shape, and the allocation angle was never weighed. The question was raised in dotnet/reactive#896 (2020) and went unanswered.

Two arguments were considered against adding the argument:

1. An async action that actually suspends allocates its boxed state machine (or, under .NET 11 runtime async, a continuation object) regardless, and that object already holds the hoisted locals. So state saves two allocations out of three rather than two out of two. This is diminishing returns, not futility: many scheduled actions complete synchronously (an `OnNextAsync` that completes synchronously, a cancellation check), and for those the saving is total. Runtime async does not change the analysis, because closures and delegates are ordinary objects it never touches; it only shrinks the unavoidable part, which makes the avoidable part a larger share.
2. The current `AsyncSchedulerBase`/`TaskPoolAsyncScheduler` allocate several objects per work item themselves, so a caller-side saving would be invisible. This is not an argument about the interface. Implementations can be rewritten at any time without breaking anyone; the interface is public API and sets the ceiling on what any implementation can achieve, so it is precisely the place to get right first.

## Decision

Every method on `IAsyncScheduler` takes a `TState` argument, in the same parameter order as Rx.NET (state, then due time, then action, so the lambda is trailing), and the action receives the state before the cancellation token, matching `CancellationToken.Register(Action<object?, CancellationToken>, object?)`:

```csharp
ValueTask<IAsyncDisposable> ScheduleAsync<TState>(TState state, Func<TState, CancellationToken, ValueTask> action);
ValueTask<IAsyncDisposable> ScheduleAsync<TState>(TState state, TimeSpan dueTime, Func<TState, CancellationToken, ValueTask> action);
ValueTask<IAsyncDisposable> ScheduleAsync<TState>(TState state, DateTimeOffset dueTime, Func<TState, CancellationToken, ValueTask> action);
```

The previous stateless signatures become extension methods on `AsyncScheduler`, implemented by passing the caller's delegate as the state with a static lambda, so they add no allocation of their own and existing calling code compiles unchanged. `AsyncScheduler.ExecuteAsync` gains matching `TState` overloads. `AsyncSchedulerBase`'s `ScheduleAsyncCore` becomes generic, so schedulers derived from it (all of the ones in this repository, including `TestAsyncScheduler`) override one generic method.

Call sites in the library use the state argument where everything the action needs is immutable: the sink (`this`) or a value tuple of the observer, scheduler and parameters, deconstructed on the first line of a `static async` lambda. Sites that share mutable locals between the scheduled action and an observer (`Throttle`, `Timeout`, `Buffer`, `Window`, `Skip`, `SkipUntil`, the `Delay` and `ObserveOn` drains) are left as capturing lambdas for now; they need restructuring into sink classes to benefit, which is a separate change. The T4-generated `ToAsync` overloads are also left alone.

## Consequences

* Breaking for implementers of `IAsyncScheduler` and subclasses of `AsyncSchedulerBase`. AsyncRx.NET has not shipped, so there are none outside this repository.
* Source-compatible for callers: `scheduler.ScheduleAsync(async ct => ...)` and `scheduler.ScheduleAsync(async ct => ..., dueTime)` still compile and behave identically.
* `TaskPoolAsyncScheduler` and `SynchronizationContextAsyncScheduler` now pass a single boxed tuple to `StartNew`/`Post` with a static callback instead of a closure and delegate; `AsyncSchedulerBase`'s timed overloads no longer capture. The rendez-vous awaiters (`TaskAwaitable`, `ValueTaskAwaitable`) replace their per-await closures with one `ScheduledContinuation` object that is its own callback state.
* The scheduler implementations still allocate more per work item than they need to (cancellation source, `Unwrap`, `ContinueWith`). That is now an implementation matter, which the interface no longer caps.
