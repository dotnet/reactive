# The `System.Reactive` legacy facade contains the UWP-specific `ThreadPoolScheduler`

[`System.Reactive` NuGet package](https://www.nuget.org/packages/System.Reactive/), which used to be the main Rx.NET package, now exists for backwards compatibility. It is mostly a 'facade' containing type forwarders. However, the `uap10.0.18362` target (the target for UWP applications that are _not_ using the .NET runtime support for UWP that was added in .NET 9) includes a `ThreadPoolScheduler` class, and does not use a type forwarder for that type. This document explains why.

## Status

Proposed


## Authors

@idg10 ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/)).


## Context

As described in [ADR 0004](0004-package-split.md), the [`System.Reactive` NuGet package](https://www.nuget.org/packages/System.Reactive/) is no longer the main Rx.NET package. `System.Reactive.Net` is now the main package, with all UI-framework-specific functionality moved into separate packages. This means applications only get support for a UI framework if they asked for it. This fixes a long-standing problem in which self-contained applications using Rx would get a complete copy of the WPF and Windows Forms frameworks even if they used neither.

The [`System.Reactive`](https://www.nuget.org/packages/System.Reactive/) package still exists of course, but its purpose is now backwards compatibility. It is marked as obsolete, to encourage people to move on to the new `System.Reactive.Net` component (and, if required, to add reference to whichever UI-framework-specific Rx.NET integration components they require).

`System.Reactive` needs to retain the same API surface area as in previous versions, so that when an application using components build for, say, Rx 6.0, ends up using a later version of Rx, those older components will still run. If a library has a reference to `System.Reactive` and uses the `System.Reactive.Linq.Observable` class from that component, the CLR will discover that `System.Reactive` does not define this type, and instead contains a type forwarder entry referring to the type of that name in `System.Reactive.Net`.

The situation is similar for UI-framework-specific types. If a library has a reference to `System.Reactive` and uses the `System.Reactive.Concurrency.ControlScheduler` type, again the CLR will discover the type forwarder in `System.Reactive`. But this time, the forwarder will refer to `System.Reactive.For.WindowsForms`, because that is the new home of this UI-framework-specific type. If an application still references `System.Reactive` (or uses libraries that reference this), it will end up with implicit transitive dependencies on all of the UI-framework-specific packages. This is the direct equivalent to how things were back in Rx 6.0, because `System.Reactive` contained all the UI-framework-specific code. It's just that this fact is now visible in the NuGet package dependency structure. The important change here is that once an application move off `System.Reactive` and onto `System.Reactive.Net` (and once all of its Rx.NET-using dependencies have also done so) it will no longer get UI-frameworkspecific code unless it explicitly asks for it with a suitable package reference.

There's one wrinkle in this: UWP's specialized `ThreadPoolScheduler`.

`ThreadPoolScheduler` should be a UI-framework-independent type. It is available in all Rx.NET targets, including `netstandard2.0` and the no-UI-framework-available `netX.0` targets. (E.g., the `net6.0` target in Rx 6.0,.) So it belongs in `System.Reactive.Net`. The problem is that the `System.Reactive` UWP target (the `uap10.0.18362` TFM) contains a slightly different version of this type than all the other targets. It has:

* Three public constructors
  * a default constructor
  * a constructor accepting a `WorkItemPriority` argument
  * a constructor accepting `WorkItemPriority` and `WorkItemOptions` arguments
* Read-only `Priority` and `Options` properties that report the `WorkItemPriority` and `WorkItemOptions` supplied at construction

It makes these available because it is implemented on top of the Windows Runtime [`Windows.System.Threading.ThreadPool`](https://learn.microsoft.com/en-us/uwp/api/windows.system.threading.threadpool?view=winrt-18362). All the other target use the .NET runtime library's [`System.Threading.ThreadPool`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadpool). This was unavailable in early versions of UWP, necessitating a different implementation of `ThreadPoolScheduler` on that platform. UWP has supported `netstandard2.0` since Windows 10.0.16299 (aka 1709, aka the 'Windows 10 Fall Creators Update'), released in 2017, so there's no longer an absolute requirement for a UWP-specific `ThreadPoolScheduler`: the `netstandard2.0` Rx.NET implementation now works just fine.

However, by the time UWP did get support for .NET `ThreadPool`, it was not possible to modify the UWP implementation to use it. This is because those additional public members described above can only be offered when using the `Windows.System.Threading.ThreadPool`: the `WorkItemPriority` and `WorkItemOptions` and types are specific to that particular thread pool.

Legacy code written for UWP using Rx 6.0 or older may expect `ThreadPoolScheduler` to offer these members. Therefore it is absolutely necessary for the `ThreadPoolScheduler` obtained through a reference to `System.Reactive` when targetting UWP to provide the UWP-specific implementation.

There are a few ways we could achieve this:

1. Continue to have `System.Reactive.Net` offer a UWP-specific target, and have `System.Reactive` forward to the `ThreadPoolScheduler` in `System.Reactive.Net`
2. Define a `System.Reactive.Concurrency.ThreadPoolScheduler` in `System.Reactive.For.Uwp`, and have `System.Reactive` forward to the `ThreadPoolScheduler` in `System.Reactive.Net`
3.

Note that in options 2 and 3, the `System.Reactive.Net` assembly would define its own `ThreadPoolScheduler`. UWP applications would use the `netstandard2.0` target, so if they reference `System.Reactive.Net` directly, they'll get that `ThreadPoolScheduler`, which will not have 


## Decision

We have chosen option 3.

Our view is that it was a mistake to add UWP-specific members to the `ThreadPoolScheduler`. We do not want that to be a feature of Rx.NET in normal use. Furthermore, the whole point of the repackaging, of which this change forms a part, was to remove all UI-framework-specific code from the main Rx.NET package. For these reasons, we reject option 1 above.

Since we consider the incorporation of UWP-specific members into `ThreadPoolScheduler` to be a mistake, we want to deprecate their use. To support the UWP-specific functionality, we define a new `UwpThreadPoolScheduler` in the `System.Reactive.For.Uwp` library, so anyone requiring either the UWP-specific constructors or properties, or who rely on some difference in behaviour between the .NET thread pool used by the `netstandard2.0` `ThreadPoolScheduler` and the `Windows.System.Threading.ThreadPool` can have this, they just need to ask for it explicitly. In order to discourage 

But it is necessary to support legacy code. So anything built against `System.Reactive` v6 or earlier that targets UWP must continue to get the



## Consequences

Positive
* Legacy code built against `System.Reactive` continues to run with no change in behaviour
* The main Rx.NET component, `System.Reactive.Net` is completely free from any UI-framework-specific code
* Any developer using the UWP-specific members in the legacy `System.Reactive` component will be told to use the new `UwpThreadPoolScheduler` when they upgrade to a new version of Rx.NET (but can continue to use the old one if they really want to)

Negative:
* Two identically-named types

The only way to avoid that would have been to continue to offer a `uap10.0.18362` target in `System.Reactive.Net`. Since new application development on UWP is strongly discouraged, this would seem like a misstep. Moreover, the continued presence of UWP in our build has caused increasing levels of pain over the year, so we really don't want to offer `uap10.0.18362` targets except in cases where they are absolutely required (e.g. in the UWP-specific component and test suite); we hope to move those out into a completely separate project at some point so that we can finally avoid all the problems the UWP causes for the build process.


What about .NET 9 on UWP?