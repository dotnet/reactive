# Shared Rx scenarios

The behavioural tests for AsyncRx.NET's operators are Rx.NET's own operator tests, run against
both implementations. This project holds the parts that are common to both platforms; the two
sibling projects run them.

| Project | Role |
|---|---|
| `Tests.System.Reactive.Shared` (this project) | The platform-neutral kit: the query description (`Query/`, with one folder per operator under `Operators/`), the environment a test runs in (`Harness/`, including the `SharedReactiveTest` base class), and the shared scenarios (`Scenarios/`, one abstract class per operator). References only the two testing vocabularies (`Microsoft.Reactive.Testing` and `Microsoft.Reactive.Testing.Async`) and MSTest; it calls no operator on either platform. Not itself a test project. |
| `Tests.System.Reactive.Async` | The AsyncRx.NET test suite: `AsyncRxPlatform` over `TestAsyncScheduler`, one `[TestClass]` per shared class and execution shape, plus scenarios that only make sense on the async platform (a consumer that prolongs completion, for example). |
| `Tests.System.Reactive.Shared.Rx` | Runs the same shared scenarios against the released Rx.NET package. Its purpose is to keep the shared scenarios honest: a scenario that passes here is a faithful migration of the Rx.NET test it came from. |

## How a shared scenario works

A scenario is written in the shape of the Rx.NET test it was migrated from. The differences are
that `var scheduler = new TestScheduler();` is gone (the base class supplies a fresh `Scheduler`
per test) and `IObservable<T>` is `Seq<T>`:

```csharp
var xs = Scheduler.CreateHotObservable(OnNext(210, 1), OnNext(220, 2), OnCompleted<int>(230));

var res = Scheduler.Start(() => xs.Take(1));

res.Messages.AssertEqual(OnNext(210, 1), OnCompleted<int>(210));
xs.Subscriptions.AssertEqual(Subscribe(200, 210));
```

`xs.Take(1)` does not run anything. A `Seq<T>` is a *description* of an observable sequence: a
tree whose leaves are the platform's own objects (a testable source the scheduler created, or an
inner window or group handed to a callback) and whose interior nodes are operator applications,
one node type per overload. Inside `Start`, the platform walks that tree as a visitor and builds
its own pipeline, so the operators under test are the platform's real operators with nothing in
between. Where a scenario nests sequences (`xs.Window(...).Select((w, i) => w.Select(...)).Merge()`),
the inner window reaches the projection callback as a value and the description the callback
returns is materialized in place; no wrapping is needed at either level.

Everything a scenario touches other than the query (the scheduler, testable sources, `Start`,
the assertions) forwards to the platform instance the running test class supplies, so the same
scenario text runs against Rx.NET's `TestScheduler` and AsyncRx.NET's `TestAsyncScheduler`. The
raw surface (`ScheduleAbsolute`, `CreateObserver`, `SubscribeAsync`, `Start()`) is async-shaped so
that it, too, can be shared; on Rx.NET it completes synchronously.

Assertion failures name the query as written, then give the platform's own diff.

## Adding things

* **A scenario:** one `[TestMethod]` in the operator's shared class. Nothing else changes.
* **An operator overload:** in the operator's folder under `Operators/`, a node class (one file), a fluent method in the operator's extensions class, and a member on that folder's `ISeqVisitor` part; then one line in each platform.
* **A test that only one platform can express:** put it in that platform's test project, next
  to the shared ones, using the platform's native scheduler directly. The extended expectation
  forms (`OnNext((210, 260), 1)`, four-timestamp `Subscribe`) come from `SharedReactiveTest` by
  inheritance, as the compact ones do.
