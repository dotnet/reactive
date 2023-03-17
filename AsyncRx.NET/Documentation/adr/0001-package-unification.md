# AsyncRx.NET NuGet Package Unification

When the AsyncRx.NET was added to the dotnet/reactive repository back in 2017, it reflected the Rx v3 packaging approach in which all the various elements were split out. This ADR describes the change to a smaller number of packages.

## Status

Proposed.

## Context

The original AsyncRx.NET code was split into many packages. Note that none of these was published to NuGet. We had the following:

* `System.Reactive.Async.Concurrency`
* `System.Reactive.Async.Core`
* `System.Reactive.Async.Disposables`
* `System.Reactive.Async.Interfaces`
* `System.Reactive.Async.Linq`
* `System.Reactive.Async.Subjects`
* `System.Reactive.Async`

There were also two more projects that didn't contain anything specific to asynchronous Rx:

* `System.Reactive.Bcl`
* `System.Reactive.Shared`

The last of these, `System.Reactive.Shared`, contains the following types:

* `EventPattern`
* `IEventPattern`
* `IEventPatternSource`
* `IEventSource`
* `Notification`
* `TimeInterval`
* `TimeStamped`
* `Unit`

Definitions for all of these types already exist in the `System.Reactive` component published to NuGet.

The `System.Reactive.Bcl` project contains two types providing asynchronous locking features. They appear to have been labelled `Bcl` because they are entirely non-Rx-specific, and are the sort of thing that might reasonably ultimately migrate into the BCL (and historically, a few things from Rx have done that). They are defined in the `System.Threading` namespace.

It's useful to understand the history of `System.Reactive.Async.Interfaces` in particular. This is the asynchronous counterpart of `System.Reactive.Interfaces`. Back in the days of Rx v2.0, that interfaces library was conceived of as the home for types that were expected not to evolve. The plan was that this library would remain unchanged as the rest of Rx expanded and evolved. This way, APIs could expose Rx interface types in a way that did not cause a dependency on some specific version of the Rx implementation types. But it didn't go to plan. New versions of the `System.Reactive.Async.Interfaces` got published with new Rx releases. And then in Rx 4.0, the 'great unification' occurred, and the relevant types moved into `System.Reactive` with `System.Reactive.Interfaces` becoming a backwards-compatibility façade containing nothing but type forwarders.

There's one important difference between Rx.NET and AsyncRx.NET when it comes to core interfaces. With Rx.NET, the two most critical interfaces, `IObservable<T>` and `IObserver<T>`, were added to the .NET runtime libraries. This means that any component can exposed Rx-based surface area without taking a dependency on any Rx library at all. The same is **not** true for AsyncRx.NET.

The two most critical interfaces in AsyncRx.NET, `IAsyncObservable<T>` and `IAsyncObserver<T>` have not been defined in the .NET runtime libraries. There is arguably a need—for example, Project Orleans has defined (Orleans.Streams.IAsyncObservable<T>)[https://learn.microsoft.com/en-us/dotnet/api/orleans.streams.iasyncobservable-1?view=orleans-7.0]. However, its definitions include Orleans-specific dependencies, so while the existence of that interface is evidence of a general-purpose requirement for this kind of thing, AsyncRx.NET cannot use that actual definition.

Moreover, there are some unresolved questions over how cancellation fits into the picture discussed at https://github.com/dotnet/reactive/issues/1296 which suggests that AsyncRx.NET's existing definitions of these interfaces might not yet be fully baked.

## Decision

There will be a single `System.Reactive.Async` component. In most cases, we will simply move code out of the other projects and into this one without modification. However, some of the projects require special consideration.

The `System.Reactive.Async.Interfaces` will be removed because it's not clear that even the two most critical interfaces, `IAsyncObservable<T>` and `IAsyncObserver<T>`, are fully baked, so it would be misleading to imply that these interfaces represent some stable type that can be relied on over a longer time frame than any particular AsyncRx.NET implementation release. (And in any case, that idea didn't quite pan out as planned for Rx.NET.) Moreover, merging these interface types into `System.Reactive.Async` is entirely consistent with the unification that was done with Rx.NET in v4.0. (And this does not introduce any of the problems we now see from that unification have gone too far, because there are no UI-framework-specific concerns in AsyncRx.NET.)

The `System.Reactive.Shared` component will be removed, and AsyncRx.NET will instead take a dependency on `System.Reactive`, so that it can use the definitions of these types from that library. The long-term intention is that AsyncRx.NET will depend on the (to-be-created) version of Rx.NET that fixes the problems around unwanted accidental dependencies on UI framework, currently planned to be Rx 7.0. To signal this 'future-oriented' nature of AsyncRx.NET, initial previews will not depend on the currently published Rx 5.0, but will depend on the latest available preview until such time as a non-preview Rx 7.0 ships.

The `System.Reactive.Bcl` component will be removed. The types it contains will move into `System.Reactive.Async`. We will move them out of the `System.Threading` namespace because at this point we have no plan for getting them into the .NET runtime libraries. We will check to see whether the runtime libraries do now in fact have similar functionality, in which case we will use that instead, and delete these types. But if there are no direct equivalents, we will move these into a suitable namespace and make them `internal` since they are implementation details, and it is not our intention for AsyncRx.NET to be providing general-purpose asynchronous programming utilities.

## Consequences

Consumers of AsyncRx.NET will see just a single NuGet package, `System.Reactive.Async`, removing any uncertainty over which is the right package to use.

By removing `System.Reactive.Shared`, we avoid duplicate definitions of all the types it contains. The downside is that a dependency on AsyncRx.NET now necessarily means a dependency on Rx.NET, which wasn't previously the case. (Our view is that most projects wanting AsyncRx.NET will probably want Rx.NET too, so we don't regard this as a serious downside.)

