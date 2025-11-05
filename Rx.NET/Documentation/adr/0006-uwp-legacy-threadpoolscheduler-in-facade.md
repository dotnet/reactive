# UWP-specific `ThreadPoolScheduler` features remain visible in `uap10.0.18362` target

[`System.Reactive` NuGet package](https://www.nuget.org/packages/System.Reactive/), which is the main Rx.NET package, now hides UI-framework-specific types in its public-facing API. It retains them in the runtime binaries for backwards compatibility, but these types are not present in the `ref` assemblies, meaning that the compiler doesn't see them. However, the `uap10.0.18362` target (the target for UWP applications that are _not_ using the .NET runtime support for UWP that was added in .NET 9) provides a `ThreadPoolScheduler` class that has some UWP-specific features, and these are visible even in the `ref` assembly. This document explains why.

## Status

Proposed


## Authors

@idg10 ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/)).


## Context

As described in [ADR 0005](0005-package-split.md), the main [`System.Reactive` NuGet package](https://www.nuget.org/packages/System.Reactive/) now hides UI-framework-specific types at build time. This means applications only get support for a UI framework if they asked for it. This fixes a long-standing problem in which self-contained applications using Rx would get a complete copy of the WPF and Windows Forms frameworks even if they used neither.

At runtime, `System.Reactive` needs to retain the same API surface area as in previous versions, so that when an application using components built for, say, Rx 6.0, ends up using a later version of Rx, those older components will still run. If a library has a reference to `System.Reactive` and uses the `System.Reactive.Linq.ControlObservable` class from that component, there will be no problem because the runtime `System.Reactive` assembly (from the NuGet package's `lib` folder) still provides this type. But the assemblies in the `ref` folder omit these types, meaning they are unavailable at compile time.

If a developer wants to use UI-framework-specific code, the project will need a suitable package reference.

There's one wrinkle in this: UWP's specialized `ThreadPoolScheduler`.

`ThreadPoolScheduler` should be a UI-framework-independent type. It is available in all Rx.NET targets, including `netstandard2.0` and the no-UI-framework-available `netX.0` targets. (E.g., the `net6.0` target in Rx 6.0.) So it belongs in publicly visible API, i.e. it needs to be present in the `System.Reactive` package's `ref` assemblies. The problem is that the `System.Reactive` UWP target (the `uap10.0.18362` TFM) contains a slightly different version of this type than all the other targets. It has:

* Three public constructors
  * a default constructor
  * a constructor accepting a `WorkItemPriority` argument
  * a constructor accepting `WorkItemPriority` and `WorkItemOptions` arguments
* Read-only `Priority` and `Options` properties that report the `WorkItemPriority` and `WorkItemOptions` supplied at construction

It makes these available because it is implemented on top of the Windows Runtime [`Windows.System.Threading.ThreadPool`](https://learn.microsoft.com/en-us/uwp/api/windows.system.threading.threadpool?view=winrt-18362). All the other target use the .NET runtime library's [`System.Threading.ThreadPool`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadpool). This was unavailable in early versions of UWP, necessitating a different implementation of `ThreadPoolScheduler` on that platform. UWP has supported `netstandard2.0` since Windows 10.0.16299 (aka 1709, aka the 'Windows 10 Fall Creators Update'), released in 2017, so there's no longer an absolute requirement for a UWP-specific `ThreadPoolScheduler`: the `netstandard2.0` Rx.NET implementation now works just fine.

However, by the time UWP did get support for .NET `ThreadPool`, it was not possible to modify the UWP implementation to use it. This is because those additional public members described above can only be offered when using the `Windows.System.Threading.ThreadPool`: the `WorkItemPriority` and `WorkItemOptions` and types are specific to that particular thread pool.

Legacy code written for UWP using Rx 6.0 or older may expect `ThreadPoolScheduler` to offer these members. Therefore it is absolutely necessary for the `ThreadPoolScheduler` provided by the `System.Reactive` package's runtime UAP assembly (the one in its `lib/)uap10.0.18362` folder) to provide the UWP-specific members.

We could hide these members in the `ref` assembly. But this would mean that unlike all the other UI frameworks we support, developers can't resolve the build errors caused by upgrading to Rx 7 simply by adding a new package reference. With all the other UI-frameworks-specific features, we've migrated whole _types_ from `System.Reactive` to new packages. But we can't do that in this instance because `ThreadPoolScheduler` can't move out of `System.Reactive`—it's a core type available on all platforms—it's just the handful of 

There's no mechanism in .NET that would enable us to define the core `ThreadPoolScheduler` in `System.Reactive` and then extend it with the additional UWP-specific members in a separate package. (Even the new `extensions` feature in C# 14 doesn't help because it supports only properties, operators, and methods. You can't define extension constructors today.) So when a developer is building a UAP-style UWP app, then even if they have added a reference to the `System.Reactive.WindowsRuntime` package, the `ThreadPoolScheduler` type will continue to be the one from `System.Reactive`.

There are two ways we could handle this:

1. Remove the UWP-specific members from `ThreadPoolScheduler` in the `System.Reactive` public API, forcing developers not just to add a package reference to `System.Reactive.WindowsRuntime` but also to rewrite their code to use a replacement type
2. Continue to make this particular UWP-specific functionality visible in the `System.Reactive` public API just for old-style UWP apps (those with a `uap10` target).


## Decision

We have chosen option 2. However, we are deprecating these members, and we have introduced a new `WindowsRuntimeThreadPoolScheduler` type as the new way to use this functionality, available in `System.Reactive.WindowsRuntime`. (Note that new UWP applications are encouraged to target .NET (e.g. `net10.0`) and not the old `uap10` TFMs, in which case this new type is the _only_ way to get this UWP-specific scheduler behaviour.)

Our view is that it was a mistake to add UWP-specific members to the `ThreadPoolScheduler`. We do not want that to be a feature of Rx.NET in normal use. However, removing them completely from the public API without warning would be unacceptable. (We're OK with removing other UI-framework-specific types from the public API because they can easily be re-instated with a suitable package reference, and no other code changes.) We have therefore rejected option 1.

Since we consider the incorporation of UWP-specific members into `ThreadPoolScheduler` to be a mistake, we want to deprecate their use, hence the definition of the new `WindowsRuntimeThreadPoolScheduler` type in the `System.Reactive.WindowsRuntime` package. And we have deprecated the UWP-specific members of `ThreadPoolScheduler` to encourage developers who need this functionality to move onto this new type.


## Consequences

* Legacy code built against `System.Reactive` continues to run with no change in behaviour (because the runtime assemblies all continue to offer the same API as before, including UI-framework-specific features)
* The public API of the main Rx.NET component, `System.Reactive` is not completely free from UI-framework-specific code: in this one specific case of the `uap10.0.18362` target, it still defines some UWP-specific members
* Any developer using these UWP-specific members in `ThreadPoolScheduler` who upgrades to a new version of Rx.NET will see warnings (because the members are deprecated with the `[Obsolete]` attribute) advising them to use the new `WindowsRuntimeThreadPoolScheduler`
