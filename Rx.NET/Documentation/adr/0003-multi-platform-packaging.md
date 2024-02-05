# NuGet packages and multi-platform support

Rx has tried a few strategies for cross-platform support over its history. The approach used currently in Rx (from v4.0 to v6.0) has turned out to cause problems for some UI frameworks on the latest versions of .NET: self-contained deployments would end up including copies of various WPF and Windows Forms components whether they used them or not, causing binaries to be tens of megabytes larger than they otherwise would have been. (https://github.com/AvaloniaUI/Avalonia/issues/9549 describes one example of this problem.)

This ADR describes a change in approach designed to address this.

## Status

Proposed

## Context

Rx has always run on multiple .NET platforms. This caused two distinct but related challenges. First, some parts of Rx (notably the way schedulers use threads) depend on platform functionality (such as thread pools) that is slightly different across different flavours of .NET—for these parts, the public API surface area was the same, but the underlying implementation was platform-specific. Second, there were some platform-specific API features, such as schedulers that integrate with WPF—the public API offered by Rx varied slightly according to your target platform. Several different strategies for dealing with these two issues have been tried over the years:

* In v1, Microsoft built multiple different versions of the Rx libraries for different platforms. (This was long before .NET Standard or even before Portable Class Libraries.)
* v2 was designed to take advantage of the (then new, now defunct) Portable Class Library (PCL) concept. Assemblies were in one of these categories:
  * A portable core
    * `System.Reactive.Interfaces` (in the [`Rx-Interfaces`](https://www.nuget.org/packages/Rx-Interfaces/2.2.5) NuGet package)—originally conceived as an assembly that would stay on v2.0.0.0 indefinitely, containing the canonical definitions of core interfaces
    * `System.Reactive.Core` (in the [`Rx-Core`](https://www.nuget.org/packages/Rx-Core/2.2.5) NuGet package)—platform-independent schedulers, and utility types for implementing Rx operators
    * `System.Reactive.Linq` (in the [`Rx-Linq`](https://www.nuget.org/packages/Rx-Linq/2.2.5) NuGet package)—implementations of LINQ operators for Rx
    * The [v2.0 beta announcement](http://web.archive.org/web/20130522225545/http://blogs.msdn.com/b/rxteam/archive/2012/03/12/reactive-extensions-v2-0-beta-available-now.aspx) mentions `System.Reactive.Providers`—expression tree support for Rx LINQ—but I don't see this on NuGet, so I'm not sure if it got rolled into something else
  * Platform-specific services, just one component, `System.Reactive.PlatformServices`, but the `Rx-PlatformServices` package contained builds for multiple target platforms including:
    * .NET Framework 4.0
    * .NET Framework 4.5
    * Silverlight 5
    * Windows Phone (7, 7.1, and 8)
    * PCL (various profiles, covering .NET Framework, Silverlight, and several versions of Windows Phone)
  * UI-framework-specific functionality
    * `System.Reactive.Windows.Threading` (in the [`Rx-Xaml`](https://www.nuget.org/packages/Rx-Xaml/2.2.5) NuGet package)—(WPF, WinRT, Silverlight, and Windows Phone schedulers and extension methods)
    * `System.Reactive.Windows.Forms` (in the [`Rx-WinForms`](https://www.nuget.org/packages/Rx-WinForms/2.2.5) NuGet package)—(Windows Forms schedulers and extension methods)
    * `System.Reactive.WindowsRuntime` (in the [`Rx-Metro`](https://www.nuget.org/packages/Rx-Metro/2.2.5) NuGet package)—(additional support for some WinRT-specific types for working with async operations)
* v3 changed the NuGet package names, but essentially kept the same structure
  * The old `Rx-Main` metapackage is replaced by `System.Reactive`
  * `Rx-Interfaces` becomes `System.Reactive.Interfaces` (version number changed to v3.0, not clear if there were actually interface changes)
  * `Rx-Core` becomes `System.Reactive.Core`
  * `Rx-Linq` becomes `System.Reactive.Linq`
  * `Rx-PlatformServices` becomes `System.Reactive.PlatformServices`
  * Likewise, the framework-specific libraries `System.Reactive.Windows.Threading`, `System.Reactive.Windows.Forms`, and `System.Reactive.WindowsRuntime` move into eponymous packages
* v4 made a major change: everything is now in a single `System.Reactive` package; the old packages still exist but now contain nothing by type forwarders pointing back into `System.Reactive`; targets `net46`, `uap10`, `uap10.0.16299`, and `netstandard2.0`
* v5 stuck with the same structure as v4; targets `net472`, `uap10.0.16299`, `netcoreapp3.1`, `net5.0`, `net5.0-windows10.0.19041`, and `netstandard2.0`
* v6 stuck with the same structure as v5 but with some newer frameworks; targets `net472`, `uap10.0.16299`, `net6.0`, `net6.0-windows10.0.19041`, and `netstandard2.0`

The historical split between `System.Reactive.Windows.Threading` and `System.Reactive.WindowsRuntime` is a little baffling. It's not clear why some WinRT stuff ended up in its own library, and some shared a library with WPF, Silverlight, and Windows Phone.

Up as far as v2, the `System.Reactive.PlatformServices` was based on a concept called "platform enlightenments". The purpose of this was to avoid having to provide a "lowest common denominator" implementation for the common API surface area. The portable core provided various schedulers, but some of these would be suboptimal on certain target platforms. Some platforms offered APIs that enabled better implementations, but because portable class libraries can access only the functionality common to their set of targets, these core libraries could not exploit that. However the core libraries provided an extensibility point where other libraries could, at runtime, plug in more specialized implementations suitable to the target platform that would provide better performance. These were called "enlightenments".

There was a major change in v4. Superficially, this seemed to "just" merge all the separate components into a single one. The most visible benefit is that it seems simpler: you need just a single NuGet package reference. However, the history behind this is more complex than it may first appear.

### History behind the decision to unify `System.Reactive` in v4

In v2, the core components contained both `net40` and `net45` targets. This could create an interesting problem in .NET applications that offered a plug-in model (e.g., Visual Studio extensions) reported in [issue #97](https://github.com/dotnet/reactive/issues/97). Imagine two plug-ins:

* Plug-in A
  * Targets .NET Framework 4.0
  * Has a reference to `System.Reactive.Linq` v3.0
* Plug-in B 
  * Targets .NET Framework 4.5
  * Has a reference to `System.Reactive.Linq` v3.0

Plug-ins are typically developed independently of one another and independently of the host application. Each plug-in will have its own build process, and that build process is going to need to pick specific DLLs to place in its build output. For example, each of the plug-ins here needs to decide which of the many difference `System.Reactive.Linq.dll` files available in the various subfolders of the `lib` folder of the `System.Reactive.Linq` NuGet package to deploy. Since Plug-in A was built for .NET Framework 4.0, it's going to use the `lib\net40\System.Reactive.Linq.dll`. Plug-in B was built for .NET Framework 4.5, so even though it's built against the exact same NuGet version on `System.Reactive.Linq`, it's going to pick a different file: `lib\net45\System.Reactive.Linq.dll`.

Suppose the host application runs in .NET 4.5, and it loads Plug-in A. This is OK because loading of .NET 4.0 components in a .NET 4.5 host process is supported. This plug-in will at some point load its copy of `System.Reactive.Linq.dll`. As we've established, since Plug-in A was built for .NET 4.0 it will have a copy of the `System.Reactive.Linq.dll` that was in the `lib\net40` folder in the `System.Reactive.Linq` NuGet package. So it gets to use the version it was built for.

So far, so good.

Now suppose the host loads Plug-in B. It will also want to load `System.Reactive.Linq.dll`, but the CLR will notice that it has already loaded that component. The critical detail here is that the `System.Reactive.Linq.dll` files in the `lib\net40` and `lib\net45` folders have **the same name, public key token, and version number**. The .NET Framework assembly loader therefore considers them to have the same identity, despite being different files with different contents. So when Plug-in B comes to try to use `System.Reactive.Linq.dll`, the loader will just use the same one that was loaded on behalf of Plug-in A. (It won't even look at Plug-in B's copy, because it has every reason to believe that it is identical—it has the exact same fully-qualified name, after all.) But if Plug-in B is relying on any functionality specific to the `net45` version of that component, it will be disappointed, and we will runtime errors such as `MethodNotFoundException`.

This tends not to be an issue with modern versions of .NET because we have [`AssemblyLoadContext`]([https://learn.microsoft.com/en-us/dotnet/core/dependency-loading/understanding-assemblyloadcontext) to solve this very problem. But with .NET Framework, this was a serious problem for plug-in hosts. There are some important plug-in hosting apps that still use .NET Framework (notably Visual Studio) so this problem hasn't gone away.

In 2016, Rx v3.0 attempted to address this in [PR #212](https://github.com/dotnet/reactive/pull/212) by giving a slightly different version number to each target. For example, the `net45` DLL had version 3.0.1000.0, while the `net451` DLL was 3.0.2000.0, and so on. This plan is described in [issue #205](https://github.com/dotnet/reactive/issues/205).

At the time this was proposed, it was [acknowledged](https://github.com/dotnet/reactive/issues/205#issuecomment-228577028) that there was a potential problem with binding redirects. Binding redirects often specify version ranges, which means if you upgrade 3.x to 4.x, it's possible that 3.0.2000.0 would get upgraded to 4.0.1000.0, which could actually mean a downgrade in surface area (because the x.x.2000.0 versions might have target-specific functionality that the x.x.2000.0 versions do not).

There were other issues in practice. [Issue #305](https://github.com/dotnet/reactive/issues/305) shows one example of the baffling kinds of problems that could emerge.

This led to the decision to restructure Rx so that it is all in a single DLL. This solved the problem described in #305: if Rx is just one DLL, you can't get into a situation in which different bits of Rx seem to disagree about which Rx version is in use. If Rx is only one binary, then there's no scope for getting a mixture of versions.

### Revisiting the decisions in 2023

To recap, there are two particularly significant factors that led to the decision in 2016 use per-target-framework version numbers (which in turn led to the decision to package Rx as a single DLL):

* There were builds for multiple .NET 4.x frameworks
* The `AssemblyLoadContext` was unavailable

The problem with .NET 4.x is that any component built for any .NET 4.x target could also end up loading on a later version of .NET 4.x (e.g., a `net40` component could load on `net462`) and once that happens in some process, that process will no longer be able to load a different file that is nominally the same version of the same component but which targets a newer framework. (E.g., once .NET 4.8 has loaded the `net40` build of some particular version of some component, then it won't be able to go on to load the `net48` version of the same nominal version of that component.) **Note**: this is only a problem when two plug-ins want the same nominal version of Rx but different targets. .NET Framework is perfectly capable of loading two completely different versions of Rx. (If two plug-ins depend on Rx 5.0 and Rx 6.0, then both the 5.0 and 6.0 versions of Rx can be loaded into the same process, the same AppDomain even. Assembly binding redirects could force both components onto a newer version, but there's no situation in which loading a plug-in with a dependency on an older version of Rx before loading one with a dependency on a newer version will cause that second plug-in to be unable to load. The v2 era problems only arose when both plug-ins wanted the same version of Rx but were targeting different versions of .NET Framework.)

But if you look at Rx 6.0 (the current version in 2023) we now build only a single .NET Framework target, `net472`. It seems that this original problem has gone away, so perhaps we can unwind the decisions that led to the current design.

A possible fly in the ointment is the `netstandard2.0` component, because that could in principle run in a .NET Framework process. We could still end up with that loading first, blocking any attempt to load the `net472` version some time later. However, in the plug-ins scenario above, that shouldn't happen. The build processes for the individual plug-ins know they are targeting .NET Framework, so they should prefer the `net472` version over the `netstandard2.0` one. (If they target an older version such as `net462`, then perhaps they would pick `netstandard2.0` instead. But then the current Rx 6.0 design fails in that scenario too. So unwinding the earlier design decisions won't make things any worse than they already are.)

Another consideration is that modern NuGet tooling is better than it was in 2016 when the current design was established. Alternative solutions might be possible now that would not have worked when Rx 4.0 was introduced.

When it comes to .NET 6.0 and later, these problems should a non-issue because better plug-in architectures exist thanks to `AssemblyLoadContext`.

So for all these reasons, while the current design might have been the best option when Rx 4.0 was released, there are likely to be other options today, and these might enable us to solve the new problems that have increasing use of new UI frameworks, and .NET build techniques such as trimming.

### The problems that any new solution has to solve

To be able to evaluate potential design changes, we need to understand the problems that any solution must solve. There are the original two problems that motivated the current design, and the newer problems that only occurred with more recent frameworks. And there is also a backwards compatibility constraint.

1. Host applications with a plug-in model getting into a state where plug-ins disagree about which `System.Reactive.dll` is loaded for a particular version of Rx (i.e., when there are multiple candidate targets)
2. Incompatible mixes of version numbers of Rx components
3. Applications getting WPF and Windows Forms dependencies even though they use neither of these frameworks.
4. Components that have a dependency on Rx 5.0 or 6.0 running in an application that upgrades that to Rx 7.0 (they must not be broken by this upgrade).

It's worth re-iterating that 1 is a non-issue on .NET 6.0+ hosts. Applications hosting plug-ins on those runtimes will use `AssemblyLoadContext`, enabling each plug-in to make its own decisions about which components. If plug-ins make conflicting decisions, it doesn't matter, because `AssemblyLoadContext` is designed to allow that.

Rx 3.0 solved 1 by using different 'build numbers' (the 3rd part of the assembly version) for different targets. For example, DLLs targeting `net45` would have a version of 3.0.1000.0; `net451` DLLs used 3.0.2000.0; `net46` DLLs used 3.0.3000.0. These assembly version numbers remained the same across multiple NuGet releases, so even in the release that NuGet labels as 3.1.1, the assembly version numbers were all 3.0.xxxx.0. So if a plug-in built for .NET 4.0 used Rx 3.1.1, and was loaded into the same process as another plug-in also using Rx 3.1.1 but built for .NET 4.5, they would be able to load the two different `System.Reactive.Linq.dll` assemblies because those assemblies had different identities by virtue of having different assembly version numbers. Unfortunately, while this solved problem 1, it caused problem 2: it was relatively easy to confuse NuGet into thinking that you had conflicting dependencies.

It's worth noting that problem 2 goes away if you use the modern project system. The problem described in [#305](https://github.com/dotnet/reactive/issues/305) occurs only with the old `packages.config` system. If you try to reproduce the problem in a project that uses the [.NET SDK](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/overview) project system introduced back in MSBuild 15 (Visual Studio 2017), the problem won't occur. Visual Studio 2017 is the oldest version of Visual Studio with mainstream support.

Rx 4.0 solved 2 (and Rx 6.0 uses the same approach) by collapsing everything down to a single NuGet package, `System.Reactive`. To access UI-framework-specific functionality, you no long needed to add a reference to a UI-framework-specific package such as `System.Reactive.Windows.Forms`. From a developer's perspective, the functionality was right there in `System.Reactive`. The exact API surface area you saw was determined by your target platform. If you built a UWP application, you would get the `lib\uap10.0.16299\System.Reactive.dll` which had the UWP-specific dispatcher support built in. If your application was built for `net6.0-windows10.0.19041` (or a .NET 6.0 TFM specifying a later Windows SDK) you would get `lib\net6.0-windows10.0.19041\System.Reactive.dll` which has the Windows Forms and WPF dispatcher and related types build in. If your target was `net472` or a later .NET Framework you would get `lib\net472\System.Reactive.dll`, which also included the Windows Forms and WPF dispatcher support (but built for .NET Framework, instead of .NET 6.0). And if you weren't using any client-side UI framework, then the behaviour depended on whether you were using .NET Framework, or .NET 6.0+. With .NET Framework you'd get the `net472` DLL, which would include the WPF and Windows Forms support, but it didn't really matter because those frameworks were present as part of any .NET Framework installation. And if you target .NET 6.0 you'll get the `lib\net6.0\System.Reactive.dll`, which has no UI framework support, but that's fine because any .NET 6.0+ TFM that doesn't mention 'windows' doesn't offer either WPF or Windows Forms.

Rx 4.0, 5.0, and 6.0 don't have a solution to 3. Initially they didn't need one—in the scenarios where you got WPF and Windows Forms support without really needing it, it was of little consequence. If you were running on .NET Framework, those frameworks were pre-installed, so there was no cost to having a dependency on them. And at the time, if your target was `netcoreapp3.1-windowsXX` it was almost invariably because you were building a WPF or Windows Forms app. However, two important things have changed in recent years: 1) there are now a lot of other UI frameworks besides WPF and Windows Forms, and 2) it's now common to produce some sort of self-contained deployment for client-side apps, at which point dragging along copies of both WPF and Windows Forms when you are using neither makes applications far larger than they should be.

### Types in `System.Reactive` that are partially or completely UI-framework-specific

Unwinding the consolidation that happened in V4 comes with a challenge: `System.Reactive` now has a slightly different API surface area on different platforms. There are some types available only on specific targets. There is one type that is available on all targets but has a slightly different public API on one target. And there are some types with the same API surface area on all targets but with differences in behavior. This section describes the afflicted API surface area.

Types available only in specific targets:

* `net472`
  * `RemotingObservable`
* `net6.0-windows` and `net472`
  * `DispatcherScheduler`
  * `DispatcherObservable`
* `net6.0-windows*` and UWP
  * `CoreDispatcherScheduler`
  * `CoreDispatcherObservable`
  * Windows Foundation Event Handler support:
    * `IEventPatternSource<TSender, TEventArgs>`
    * `EventPatternSource<TSender, TEventArgs>`
    * `WindowsObservable` (see also async operations)
  * Adaptation between `IObservable<T>` and WinRT's async actions/operations/progress:
    * `AsyncInfoObservableExtensions`
    * `AsyncInfoObservable`
    * `WindowsObservable` (see also Windows Foundation Event Handler support)

Types that are in all targets, but which contain UI-frameworks specific public members in relevant targets:

* UWP
  * `ThreadPoolScheduler` has different implementation based on UWP thread pool and provides support for `WorkItemPriority` (a UWPism)

Behavior that is different across platforms:

* UWP
  * `Scheduler` periodic scheduling has workaround for WinRT not supporting &lt;1ms resolution
* `net6.0-windows*` and UWP
  * `IHostLifecycleNotifications` service available from enlightenment provider
  * Periodic scheduling will be aware of windows suspend/resume events only if using `System.Reactive` for these targets


### Design options

Here are some possible design changes that could solve some new problems without re-introducing old ones.

For each option the first question we have to ask is whether we will revert to the old approach of putting framework-specific code in separate components, or maintain the current unified `System.Reactive` structure. Even if we make the choice to reinstate the pre-Rx-v4.0 situation in which UI-framework-specific functionality is accessed through UI-framework-specific NuGet packages, there are still two dimensions to consider. Rx has platform-specific API surface area. (E.g., `System.Reactive.Concurrency.ControlScheduler` is a scheduler that supports the Windows Forms threading model.) More subtly, there have long been platform-specific implementation details. (E.g., on UWP, you aren't allowed to create new `Thread` objects, which constrains the implementation of some schedulers. There are some schedulers for which it would be possible to produce a single implementation that could run on all platforms, but where use of `Thread` would result in better performance where it is available. So although a 'lowest common denominator' implementation is possible, it is undesirable—developers targeting .NET 6.0 shouldn't be penalized for limitations that happen to exist in UWP. For this reason, some of the schedulers compiled into `System.Reactive` in Rx 6.0 have different code on different platforms.) Design options will need to be able to address both of these "multi-platform" aspects.

Back before the 'great unification' in Rx v4.0, the distinction between these two aspects was reflected by the NuGet packaging. As just discussed, framework-specific API surface area was in platform-specific packages. But the second concern of implementation details was handled through the 'platform enlightenment' concept, which was visible through the `System.Reactive.PlatformServices` component.

With that in mind, let's now consider some possible choices.


#### Design option: the status quo

Rx 5.0 and 6.0 have both shipped, and a lot of people use them, so one option is just to continue doing things in the same way. This is not a good solution. Back when Rx 5.0 was the current version, some people seemed to think that the changes we adopted in Rx 6.0 would be sufficient to solve the problems described in this document. But as will be explained, it doesn't.

`System.Reactive` 5.0 targeted `netstandard2.0`, `net472`, `netcoreapp3.1`, `net5.0`, `net5.0-windows10.0.19041`, and `uap10.0.16299`. The idea with this design option was to target `netstandard2.0`, `net472`, `net6.0`, `net6.0-windows10.0.19041`, and `uap10.0.16299`. (In other words, drop .NET Core 3.1 and .,NET 5.0, both of which went out of support in 2022, and effectively upgrade the .NET 5.0 target to .NET 6.0.) This is in fact exactly what we did for Rx 6.0, but despite what some people seemed to believe, this was never going to solve the problems described above.

(Some people also seemed to be demanding `net7.0` targets. I would be very reluctant to do that unless there are .Net 7.0 platform features we could use that would enhance functionality or performance that would justify offering both `net6.0` and `net7.0` targets. So unless we discover some unique benefits from targeting `net7.0`, I (@idg10) would prefer to state clearly that we support running on .Net 7.0, and to run all tests against both .Net 6.0 and 7.0, but not to produce `net7.0` targets if the only reason is to stop people complaining. That would be a technical solution to a social problem. And it's not even a good technical solution.)

Let's look at how this gets on with the three challenges:

**1: Host applications with a plug-in model getting into a state where plug-ins disagree about which `System.Reactive.dll` is loaded**

Since the plug-in issues are only relevant to .NET Framework, and this doesn't change the .NET Framework packaging in any way, this solves the problem in the same way that Rx 4.0 and 5.0 do.

**2: Incompatible mixes of version numbers of Rx components**

Rx 4.0 introduced the unified packaging to solve this problem, and this option retains it, so it will solve the problem in the same way.

**3. Applications getting WPF and Windows Forms dependencies even though they use neither of these frameworks**

This design option does not solve this problem. I think this is a fundamental problem with anything that continues to have a unified structure: if `System.Reactive` inevitably gives you WPF and Windows Forms support whenever you target a `netX.0-windowX`-like framework, you're going to have this problem. There has to be some way to indicate whether or not you want that, and I think separating out those parts is the only way to achieve this.

This design option also doesn't have a good answer for how we provide UI-framework-specific support for other frameworks. (E.g., how would we offer a `DispatcherScheduler` for MAUI's `IScheduler`?)

So in short, I (@idg10) haven't been able to think of a design that maintains the unified approach that doesn't also suffer from problem 3.

The only attraction of this design option is that it is least likely to cause any unanticipated new problems, because it closely resembles the existing design.

#### Design option: deprecate `System.Reactive`

* A `System.Reactive.Core` component (comprising the common public API, and common implementation details) with netstandard2.0, and .net6.0 (no `net472`, `uap10*`, or `*-windows` targets)
* UI-framework-specific components defining Rx types (e.g, `ControlDispatcher`, and `ControlObservable`) are supplied in per-framework components (e.g., `System.Reactive.Windows.Forms`, `System.Reactive.Maui`, `System.Reactive.Wpf`; the old multi-platform `System.Reactive.Windows.Threading` would exist for backcompat, but would just contain type forwarders); a new `UwpThreadPoolScheduler` would be made available for the UWP-specific version of the thread pool scheduler
* `System.Reactive` is deprecated, and retained only for backwards compatibility; it remains a component with platform-specific targets (net472, .net6.0 (with and without `-windows`), uap10) and also `netstandard2.0` and it just provides type forwarders for the entire V6 public surface area
* No version number adjustment within Rx versions—assemblies are all version 7.0.0.0 in Rx 7.0 (or 8.0.0.0 in Rx 8.0, etc.)

Let's address the elephant in the room: this is almost exactly the design we had back in Rx v2.0. (The differences are that some older UI frameworks have gone away, and in this design, instead of having the strange "UI-framework-specific APIs for several, but not all UI frameworks" package known as `System.Reactive.Windows.Threading`, we move to a strict "one package per UI framework" model.) We know this didn't work in 2016, so why would we move back to it?

Well a lot of things have changed since 2016. So let's look at the four problems it needs to solve.

**1: Host applications with a plug-in model getting into a state where plug-ins disagree about which `System.Reactive.dll` is loaded**

This problem ([issue #97](https://github.com/dotnet/reactive/issues/97)) arose because Rx NuGet packages contained two different DLLs with identical strong names, either of which could be loaded into the same .NET Framework host process. In this proposal that can't happen. The following bullet points explain why each of the packages described is immune to this (because the reasoning is slightly different for each set):

* `System.Reactive.Core`: since only one target (`netstandard2.0`) can be loaded by .NET Framework, disagreement can't arise about which DLL represents any single assembly identity
* `System.Reactive`: this is trickier because there are two targets that could be loaded into a .NET Framework process: `net472` and `netstandard2.0`; however all currently tooling will select the `net472` version for any .NET Framework project using 4.7.2 or later (and Rx 5.0 and 6.0 already didn't support plug-in scenarios for hosts using older versions of .NET Framework)
* As for the UI-specific packages, because this plug-in problem exists only for .NET Framework, not .NET 6.0+, we only have to consider the Window Forms and WPF ones, and only the .NET Framework versions of those; these will have just one applicable target (`net472`) so the problem will not arise

**Note:** for future versions of Rx, a critical aspect of this design is that we can't offer both `net472` and `net48` targets. We can certainly _support_ `net48`, but it is supported by offering a `net472` version. Since .NET Framework is at a point where Microsoft is not planning any significant new feature development, this shouldn't be a problem. The only situation in which we envisaging adding `net48` as a target would be if we were dropping support for `net472`, so we'd still have just one .NET Framework target.


**2: Incompatible mixes of version numbers of Rx components**

This problem ([#305](https://github.com/dotnet/reactive/issues/305)) arose because Rx 3.0 played games with .NET assembly version numbers to solve [issue #97](https://github.com/dotnet/reactive/issues/97). But in this design option, we are not playing any such games, because issue 97 is addressed without needing to resort to such tricks.

As it happens, the extent of this second problem has shrunk dramatically thanks to change in tooling. Even if we did use versioning tricks, it turns out that if you use the modern ".NET SDK" style of project, NuGet handles this situation correctly. But since we aren't planning to do that, this design option eliminates this problem even for the old project system.

**3: Applications getting WPF and Windows Forms dependencies even though they use neither of these frameworks**

This problem is relevant only to .NET 6.0+ applications. (It's not a problem on .NET Framework, because WPF and Windows Forms are invariably deployed as part of the runtime—there is no deployment model for .NET Framework in which applications ship their own copy of WPF or Windows Forms unless they have chosen to incorporate a complete .NET Framework installer.)

This design option solves this by not having any `.net6.0-windowsXXX` targets in `System.Reactive.Core`.  It targets `.net6.0` but not `.net6.0-windows10.0.19041`. `System.Reactive` continues to target `-windows` type TFMs, but in this design it is deprecated, and retained only for backwards compatibility. This means `System.Reactive.Core` cannot possibly take a dependency on either WPF or Windows Forms. That means that any applications or libraries targeting other UI frameworks such as WinUI, MAUI, or Avalonia can take a dependency on `System.Reactive.Core` without bringing in any WPF or Windows Forms code. You will only end up with a dependency on those frameworks if you use the corresponding `System.Reactive.Windows.Forms` or `System.Reactive.Wpf` NuGet packages.

Applications wishing to take advantage of this will need to change their references from `System.Reactive` to `System.Reactive.Core`.

One limitation of this scheme is that any applications picking up an indirect reference to `System.Reactive` will continue to have the problem in which they end up with dependencies on all of WPF and Windows Forms.

**4: Components with a dependency on Rx 5.0 or 6.0 must not be broken if an app upgrades that to Rx 7.0**

In normal (non-plug-in) usage, an application might take a dependency on multiple components that depend on different versions of Rx. NuGet has a complete view of the dependency tree (something that wasn't true back in the `packages.config` days). When building an application, it will pick a single version, so no matter which versions individual libraries depended on, a single version will be selected. (If some projects or libraries impose constraints, NuGet might not be able to resolve a suitable single version. For example, if you use a library that declares that it must have exactly Rx 5.0, and not a later version, and another library that declares that it must have Rx 6.0 or later, there is not satisfactory resolution. But that's not something we can solve.)

Imagine a NuGet package `Example.LibUsingRx5` that targets `net6.0-windows10.0.19041`, and that references `System.Reactive` v5.0. Now suppose we have an application that targets `net8.0-windows10.0.22000` that references `Example.LibUsingRx5`. NuGet will determine that `System.Reactive` v5.0 meets all requirements, and it will use the package's `lib\net5.0-windows10.0.19041\System.Reactive.dll`. (The fact that it targets .NET 5.0, and not a .NET 6.0 or 8.0 TFM doesn't matter. All `net5.0` TFMs are compatible with the .NET 6.0, 7.0, and 8.0 runtimes.)

Now imagine our application adds a reference to `Example.LibUsingRx6` that targets `net6.0`, and that references `System.Reactive` v6.0. This means that `System.Reactive` v6.0 now becomes the minimum acceptable version, and so `Example.LibUsingRx5` (which the app is still using, in addition to `Example.LibUsingRx6`) will also get upgraded to `System.Reactive` v6.0. Because the application targets `net8.0-windows10.0.22000`, NuGet is going to pick the Windows-specific TFM, so it will use the package's `lib\net6.0-windows10.0.19041\System.Reactive.dll`.

Note that so far, using just the existing versions of Rx, the behavior is that this makes all the WPF and Windows Forms functionality available, creating dependencies on those frameworks.

So far, that's all stuff that happens with today's version of Rx. So what about this putative Rx 7.0 in the design suggested? Imagine the app getting a third package reference, this time to `Example.LibUsingRx7` that targets `net6.0`, and that references `System.Reactive` v7.0. That makes v7.0 the minimum acceptable version of `System.Reactive`. In this design, `System.Reactive` includes a `net6.0-windows10.0.19041` target, and for that target it also has implicit references to `System.Reactive.Windows.Threading` and `System.Reactive.Windows.Forms`. And all targets have an implicit reference to `System.Reactive.Core` v7.0. So we'd get four DLLs loaded.

* `System.Reactive` v7.0's `lib\net6.0-windows10.0.19041\System.Reactive.dll`
* `System.Reactive.Core` v7.0's `lib\net6.0\System.Reactive.Core.dll`
* `System.Reactive.Windows.Forms` v7.0's `lib\net6.0-windows10.0.19041\System.Reactive.Windows.Forms.dll`
* `System.Reactive.Windows.Threading` v7.0's `lib\net6.0-windows10.0.19041\System.Reactive.Windows.Threading.dll`

In this design, `System.Reactive` is deprecated, and is retained for backwards compatibility. In this case it has caused the Windows Forms and WPF dependencies to be brought in. This is necessary because the `Example.LibUsingRx5` library targets `net6.0-windows10.0.19041`, so it could well be using these features. To ensure backwards compatibility, 

What about projects still using `packages.config`?

### Design option: remove UI dependencies from `System.Reactive`

The scheme described in [Design option: deprecate `System.Reactive`](#design-option-deprecate-systemreactive) had a limitation: any applications with a Windows-specific TFM that pick up an indirect reference to `System.Reactive` will continue to have the problem in which they end up with dependencies on all of WPF and Windows Forms. That means we might have the following extremely annoying situation:

* `Example.NonUiLibUsingRx5`: a library targeting `net6.0` that uses Rx 5.0, and because it has a non-Windows-specific TFM, it can't possibly be using any Windows Forms or WPF features of Rx
* `Example.LibUsingThatLastLib`: a library targeting `net6.0` with a reference to `Example.NonUiLibUsingRx5`; this has an implicit reference to Rx 5.0, so it could be using Rx features, but since it also has a non-Windows-specific TFM, it can't possibly be using any Windows Forms or WPF features of Rx
* `MyAvaloniaApp`: an application that does not use WPF or Windows Forms, and which targets `net6.0-windows10.0.22000`, and which has a dependency on `Example.NonUiLibUsingRx5` and `System.Reactive.Core` v7

This will end up with a dependency on `System.Reactive` v5, and because it has a Windows-specific target, it will get the `lib\net5.0-windows10.0.19041\System.Reactive.dll` file from that package. (It will also have `System.Reactive.Core` from V7. So it will effectively have two different versions of Rx loaded.) Attempting to force that older library to use the newer version by referring to `System.Reactive` v7 won't help because that's the backwards compatibility package that continues to bring in dependencies on WPF and Windows Forms.

This means that the only way the Avalonia app can free itself from this problem is if all of its Rx-using dependencies upgrade to Rx 7.0 and specify `System.Reactive.Core`, not `System.Reactive`.

So could we instead change `System.Reactive` so that it continues to be the way to use Rx, and just make it so that it doesn't bring in WPF and Windows Forms dependencies? It would certainly be a relatively straightforward change: we'd move the relevant types out into separate assemblies as with [Design option: deprecate `System.Reactive`](#design-option-deprecate-systemreactive), but we'd just leave everything else in `System.Reactive` and have that target only `netstandard2.0` and `net6.0`.

This would be a much more satisfying result. `System.Reactive` would continue to be the way to use Rx. But there's one big problem with this: it breaks backwards compatibility.

Consider a WPF application targeting `net6.0-windows10.0.22000`. Imagine a NuGet package `Example.LibUsingRx5` that targets `net6.0-windows10.0.19041`, and that references `System.Reactive` v5.0, and suppose that this library was indeed using some WPF-specific functionality in Rx (e.g., the `DispatcherScheduler`). And suppose that this same application also depends on some other component `Example.NonLibUsingRxUpgradeable`, targeting `.net6.0`, where v1 of this component depends on Rx 5.0. So far all is well—we've got a dependency on WPF via Rx but that's fine because it's a WPF application. Now suppose a new v2 of `Example.NonLibUsingRxUpgradeable` is released, still targetting `net6.0`. and it now depends on Rx 7.0. NuGet is now going to determine that `System.Reactive` v5.0 no longer satisfies all components, so it determines that everything's going to get `System.Reactive` v7.0. But if we've modified that so that it no longer offers any of the Windows Forms or WPF functionality, we're now going to have a problem: when we exercise the code in `Example.LibUsingRx5` that was depending on the `DispatcherScheduler`, we're going to get a `MissingMethodException` at runtime!

The application can't fix this even by explicitly taking a dependency on `System.Reactive.Windows.Threading`, because the `Example.LibUsingRx5` doesn't know the types have moved there.

So the difference between this and [Design option: deprecate `System.Reactive`](#design-option-deprecate-systemreactive) is that you have to choose between `MissingMethodException` and unnecessary but unavoidable dependencies on WPF and Windows Forms. In either case the only way the application can avoid the problem is to avoid taking dependencies on libraries dependent on older versions of Rx.


### Rejected design options

This section describes some ideas that have been rejected as unworkable.

#### Unworkable Option 1

This next option is a slightly less radical change from Rx v6.0's design:

* A `System.Reactive` component targeting `netstandard2.0`, `net472`, `net6.0`, `uap10` (with platform-appropriate implementation details, and a lowest common denominator for `netstandard2.0`), and the `net472` and `uap10` targets include support for the UI frameworks available on those targets (Windows Forms and WPF for `net472`, UWP for `uap10`) but the `net6.0` target has no UI framework support and we do **not** offer a `net6.0-windowsXX` target
* UI-framework-specific Rx types (e.g, `ControlDispatcher`, and `ControlObservable`) are baked into `System.Reactive` for the `net472` and `uap10` targets, but for all other cases are supplied in per-framework components (e.g., `System.Reactive.Windows.Forms`, `System.Reactive.Maui`, `System.Reactive.Wpf`; the old multi-platform `System.Reactive.Windows.Threading` would exist for backcompat, but would just contain type forwarders)

The main attraction of this approach over option 1 is that this is a smaller change. I (@idg10) don't like it as much because it is less internally consistent, but it might be less disruptive for existing users, and it also has fewer separate components. Note that this sort of straddles the "framework-specific code in separate components" and "unified" approach, because it retains some of the aspects of the current unified design. (That's the main source of the inconsistency that I dislike.)

Again, let's look at how this addresses the three challenges.

**1: Host applications with a plug-in model getting into a state where plug-ins disagree about which `System.Reactive.dll` is loaded**

Remember, this problem ([issue #97](https://github.com/dotnet/reactive/issues/97)) arose because Rx NuGet packages contained two different DLLs with identical strong names, either of which could be loaded into the same .NET Framework host process. But although the `netstandard2.0` and `net472` targets for `System.Reactive` could either technically be loaded into a .NET Framework process, in practice any component built for .NET Framework will pick the `net472` version, so as with the preceding option, this won't be a problem.


**2: Incompatible mixes of version numbers of Rx components**

This problem ([#305](https://github.com/dotnet/reactive/issues/305)) arose because Rx 3.0 played games with .NET assembly version numbers to solve [issue #97](https://github.com/dotnet/reactive/issues/97). But in this design option, we are not playing any such games, because issue 97 is addressed without needing to resort to such tricks.

**3: Applications getting WPF and Windows Forms dependencies even though they use neither of these frameworks**

As with option 1, there is no `net6.0-windowsXX` target for `System.Reactive`, so this can't cause dependencies on WPF or Windows Forms.

**4: Components with a dependency on Rx 5.0 or 6.0 must not be broken if an app upgrades that to Rx 7.0**

Since this design option is essentially half-baked version of [Design option: remove UI dependencies from `System.Reactive`](#design-option-remove-ui-dependencies-from-systemreactive) (it leaves the dependencies in place for .NET Framework, but removes them for .NET 6.0) this runs into the same problem as described in that design. The only benefit is that you don't hit this problem if you happen to be targetting .NET Framework.




## Decision

TBD, but currently my preferred approach is option 1.

## Consequences

TBD.