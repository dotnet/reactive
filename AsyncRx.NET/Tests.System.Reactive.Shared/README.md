# Shared Rx scenarios

The behavioural tests for AsyncRx.NET's operators are Rx.NET's own operator tests, run against
both implementations. This project, the shared library, holds everything that is common to both
targets, in three parts: the **query model** (`Query/` and `Operators/`), a target-neutral
description of a query that each target turns into its own real query; the **shared harness**
(`Harness/`), the per-test scheduler, testable sources, assertion handles and test base class
that forward to whichever target is running; and the **shared scenarios** (`Scenarios/`), the
tests themselves, one abstract class per operator. The two sibling projects run the scenarios
against a target each.

| Project | Role |
|---|---|
| `Tests.System.Reactive.Shared` (this project) | The shared library: query model (`Query/`, with one folder per operator under `Operators/`), shared harness (`Harness/`, including the `SharedReactiveTest` base class) and shared scenarios (`Scenarios/`). References only the two testing vocabularies (`Microsoft.Reactive.Testing` and `Microsoft.Reactive.Testing.Async`) and MSTest; it calls no operator on either target. Not itself a test project. |
| `Tests.System.Reactive.Async` | The AsyncRx.NET test suite: `AsyncRxTarget` over `TestAsyncScheduler`, one `[TestClass]` per shared class and execution shape, plus scenarios that only make sense on the async target (a consumer that prolongs completion, for example). |
| `Tests.System.Reactive.Shared.Rx` | Runs the same shared scenarios against the released Rx.NET package. Its purpose is to keep the shared scenarios honest: a scenario that passes here is a faithful migration of the Rx.NET test it came from. Also holds the two tests of the query model itself (`QueryDescriptionTests`). |

## How a shared scenario works

A scenario is written in the shape of the Rx.NET test it was migrated from. The differences are
that `var scheduler = new TestScheduler();` is gone (the base class supplies a fresh `Scheduler`
per test), `IObservable<T>` is `Seq<T>`, and the creation operators are `Seq.Timer(...)`,
`Seq.Return(...)` and so on rather than `Observable.Timer(...)`, named for what they return:

```csharp
var xs = Scheduler.CreateHotObservable(OnNext(210, 1), OnNext(220, 2), OnCompleted<int>(230));

var res = Scheduler.Start(() => xs.Take(1));

res.Messages.AssertEqual(OnNext(210, 1), OnCompleted<int>(210));
xs.Subscriptions.AssertEqual(Subscribe(200, 210));
```

`xs.Take(1)` does not run anything. A `Seq<T>` is a *description* of an observable sequence: a
tree whose leaves are the target's own objects (a testable source the scheduler created, or an
inner window or group handed to a callback) and whose interior nodes are operator applications,
one node type per overload. Inside `Start`, the target walks that tree as a visitor and builds
its own pipeline, so the operators under test are the target's real operators with nothing in
between.

A nested sequence is a `Seq<Seq<T>>`, and a sequence of groups a `Seq<Group<TKey, T>>`. Nothing
in the query model knows about nesting: `xs.Window(2, 2).Skip(1).Merge()` applies the ordinary
generic `Skip` to a sequence of windows, and `Window(2, 2).Window(1, 1).Merge().Merge()` nests two
deep, with no code written for either. Where a scenario's callback receives a window or group
(`xs.Window(...).Select((w, i) => w.Select(...)).Merge()`), it arrives as a leaf and the
description the callback returns is materialized in place.

One operator's scenarios drive an observer themselves: `Create`. Its callback receives an
`ObserverRef<T>` and awaits `OnNextAsync` where the Rx.NET test calls `OnNext`, in an `async`
lambda that returns its action or disposable through a `ValueTask`:

```csharp
Seq.Create<int>(async o =>
{
    await o.OnNextAsync(1);
    await o.OnNextAsync(2);
    return () => { };
})
```

The bridge hands the callback the target's real observer wrapped as an `ObserverRef<T>`. On
Rx.NET every await completes synchronously and the target runs the callback as a synchronous
subscribe function; on AsyncRx.NET the callback is the subscribe function.

## How a target materializes a description

Each target (`RxTarget`, `AsyncRxTarget`) writes every operator once, as an ordinary generic
method over the library's own types with the library's own genericity, in a partial file named
for the operator's folder (`RxTarget.Take.cs` for `Operators/Take/`):

```csharp
private static IObservable<T> TakeImpl<T>(IObservable<T> source, int count) => source.Take(count);
```

These `*Impl` methods are the real calls into the library: a breakpoint there is a breakpoint on
the operator under test, and a wrong call is a compiler error. The visitor member for a node is
one line that hands the method and the node's parts to the target's `DescriptionBridge`:

```csharp
public Realized<Seq<T>> Take<T>(TakeSeq<T> seq) => bridge.Run<Seq<T>>(TakeImpl<T>, seq.Source, seq.Count);
```

The bridge is the one place that crosses from descriptions to real objects, and the only place
that uses reflection. It rewrites each description type to the target's real type at any depth
(`Seq<Seq<int>>` to `IObservable<IObservable<int>>`, `Group<string, int>` to
`IGroupedObservable<string, int>`), re-instantiates the `*Impl` method at those types, converts
the arguments (descriptions are materialized, delegates over descriptions are adapted so that a
callback sees leaves and its returned description is materialized, recorded messages are mapped
element-wise), invokes the method, and returns the result as a `Realized<Seq<...>>`. A
mismatch between a node and its `*Impl` surfaces at run time, in the first test that uses the
member, with a message that names the method, the argument and both types. The bridge's frames
are hidden from stack traces.

One rule follows from this: an `*Impl` must keep the library's genericity. `TakeImpl<T>` over
`IObservable<T>` serves flat and nested sequences alike; `TakeImpl` over `IObservable<int>` would
compile and pass every flat scenario, then fail the first nested one. Each target's test project
has an `ImplSignatureTests` class that checks every `*Impl` for this and fails immediately.

Everything a scenario touches other than the query (the scheduler, testable sources, `Start`,
the assertions) forwards to the target instance the running test class supplies, so the same
scenario text runs against Rx.NET's `TestScheduler` and AsyncRx.NET's `TestAsyncScheduler`. The
raw surface (`ScheduleAbsolute`, `ScheduleRelative`, `CreateObserver`, `SubscribeAsync` with one,
two or three handlers, a testable observer's `OnNextAsync`/`OnErrorAsync`/`OnCompletedAsync`,
`Start()`) is async-shaped so that it, too, can be shared; on Rx.NET it completes synchronously.

Assertion failures name the query as written, then give the target's own diff.

## Real-time scenarios

A few Rx.NET tests run on the real default scheduler rather than a `TestScheduler`: they build a
query over `Observable.Range(0, 10, Scheduler.Default)`, subscribe with `lst.Add`, block on a
`ManualResetEvent` until completion, and assert on the list. The shared form of such a test is an
`async Task` test method that names `DefaultScheduler` where the original has
`Scheduler.Default` and awaits `ToListAsync(res)` where the original blocks:

```csharp
[TestMethod]
public async Task Skip_Default()
{
    var xs = Seq.Range(0, 10, DefaultScheduler);

    var res = xs.Skip(TimeSpan.FromSeconds(60));

    var lst = await ToListAsync(res);

    Assert.IsEmpty(lst);
}
```

`DefaultScheduler` is `Scheduler.Default` on Rx.NET and the task pool scheduler on AsyncRx.NET,
which is what its scheduler-less overloads use. `ToListAsync` materializes the query, subscribes,
and completes with everything the sequence produced; an error faults the task, so a test that
expects one awaits it inside `Assert.ThrowsExactlyAsync`. Where the original drives a
`Subject<T>` from the test body, the shared scenario drives a `CreateSubject<T>()` leaf through
its async-shaped `OnNextAsync`, `OnErrorAsync` and `OnCompletedAsync`, which complete
synchronously on Rx.NET. The `Scheduler` property still exists in such a test but is unused.

## The argument-validation stratum

Rx.NET's `*_ArgumentChecking` tests are not migrated; they are derived from the API surface. Each
runner project has an `ArgumentCheckingTests` class with one `[TestMethod]` per operator name,
`Take_ArgumentChecking` and so on, and each is one line: `ArgumentChecks` walks every overload of
that name on the target's surface (`IApiSurface`: the static operator classes, which are
`Observable` and `ObservableExtensions` on Rx.NET and `AsyncObservable` and
`AsyncObservableExtensions` on AsyncRx.NET, the observable interface, and its subscribe method)
and applies Rx.NET's rules. Every non-nullable reference
parameter passed as null must raise `ArgumentNullException` naming it; every parameter whose name
says it is a count or duration passed as minus one must raise `ArgumentOutOfRangeException`
naming it (with a short, documented list of exceptions where Rx.NET accepts the value); and where
the overload returns an observable, subscribing a null observer must raise
`ArgumentNullException`. The other arguments come from `Dummies`, which builds a plausible
argument of any type. The Rx.NET runner is the oracle for the rules: they pass there, so an
AsyncRx.NET failure is a library defect. The shared class declares the operator names both
surfaces have, and the ones only one library has, which are inconclusive on the target that lacks
them: each of those is an unresolved difference between the libraries, and the skip is the
reminder until it is resolved one way or the other. The only one-sided names declared in a
target's own derived class are the resolved pairs that differ by a word in the name:
`AsObservable`/`AsAsyncObservable`, `ToObservable`/`ToAsyncObservable`,
`SubscribeSafe`/`SubscribeSafeAsync`, the handler-based `Subscribe`/`SubscribeAsync` family, and
Rx.NET's asynchronous-factory `Using` overload against `UsingAsync`. A meta-test fails if a surface
method has no test, so a new operator is covered the moment it is added.

## `Native`, and what the shared library does not hold

The shared library never holds a target's observable, observer, scheduler or recorded data under
its own type. Each shared object that stands for a target object carries it in a property named
`Native`, as `object` or as a `Realized<...>` (an `object` that records which description it
realizes), and only the target, or a target-specific test, gets at the real type:

| Shared object | Its `Native` on Rx.NET | On AsyncRx.NET |
|---|---|---|
| `TestSchedulerRef` (a `SchedulerRef`) | `Microsoft.Reactive.Testing.TestScheduler` | `TestAsyncScheduler` |
| `NativeSeq<T>` (a leaf): a `Realized<Seq<T>>` | `IObservable<T>` | `IAsyncObservable<T>` |
| `Group<TKey, T>` (a leaf with a `Key`): a `Realized<Seq<T>>` | `IGroupedObservable<TKey, T>` | `IGroupedAsyncObservable<TKey, T>` |
| `TestableSeq<T>` (a source the scheduler created) | `ITestableObservable<T>` | `ITestableAsyncObservable<T>` |
| `TestableObserver<T>` (what `Start` returns) | `ITestableObserver<T>` | `ITestableAsyncObserver<T>` |
| `SubjectSeq<T>` (a leaf a real-time scenario drives by hand) | `Subject<T>` | `SequentialSimpleAsyncSubject<T>` |

`res.Messages` and `xs.Subscriptions` are not collections. They are handles (`MessageLog<T>`,
`SubscriptionLog<T>`) that hold the shared observer or source and whose `AssertEqual` asks the
target to compare that object's own records against the shared expectations. The records stay in
the target's own type because that is where the information is: on AsyncRx.NET a recorded
message has a delivery start and end tick and a subscription has four timestamps, and the
target's comparison is what can check a compact `OnNext(210, 1)` against that and name the
timestamp that mismatched. Target-specific tests that need the richer forms cast `Native`
(for example `(TestAsyncScheduler)Scheduler.Native` or `xs.Native.Get<IAsyncObservable<int>>()`)
and use the target's own API directly.

## Adding things

* **A scenario:** one `[TestMethod]` in the operator's shared class. Nothing else changes.
* **An operator overload:** in the operator's folder under `Operators/`, a node class (one file), a fluent method in the operator's extensions class (or, as for `Merge`, in a partial of `Seq` when the Rx.NET tests also call the operator statically, as `Observable.Merge(scheduler, xs, ys)`), and a member on that folder's `ISeqVisitor` part; then, in each target's partial file for that operator (`RxTarget.Take.cs`, `AsyncRxTarget.Take.cs`), an `*Impl` method that makes the real call and a one-line visitor member that hands it to the bridge. A new operator gets a new folder and a new partial file in each target, named the same way. A method parameter that is a sequence is a `Seq<T>` in the node and the library's observable type in the `*Impl`; a callback that returns a sequence is a `Func<..., Seq<T>>` in the node and `Func<..., IObservable<T>>` in the `*Impl`. The bridge does the rest.
* **A scenario that passes on Rx.NET and fails on AsyncRx.NET:** that is the suite doing its job. Leave it failing and fix AsyncRx.NET in a commit of its own.
* **A test that only one target can express:** put it in that target's test project, next
  to the shared ones, using the target's native scheduler directly. The extended expectation
  forms (`OnNext((210, 260), 1)`, four-timestamp `Subscribe`) come from `SharedReactiveTest` by
  inheritance, as the compact ones do.
