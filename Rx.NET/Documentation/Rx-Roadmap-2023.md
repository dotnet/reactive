# Rx Roadmap for 2023

As of February 2023, `System.Reactive` (aka Rx.NET, Rx for short in this document) has been moribund for a while. This document describes the plan for bringing it back to life.

## Proposal in brief

* an initial V.next which we plan to release as [v6.0](#rx-v60) bringing the build process and TFMs up to date (dropping out-of-support targets such as `netcoreapp3.1` and `net5.0` and replacing them with the current LTS equivalent such as `net6.0`) and adding automated testing on .NET 6 and .NET 7
* work in parallel on an `alpha`-labeled release of NuGet packages for [`System.Reactive.Async`](#rxasync)
* a V.next.next which we plan to release as [v7.0](#rx-v70) addressing code bloat and related issues experienced by some UI frameworks
* an initiative to triage the backlog of issues, and determine which issues should be addressed in each of the releases described above

We intend that in November 2023, Rx.NET will work against the mainstream .NET runtimes in support at that point, including .NET 8.0. In the longer run, our target platform support policy will be informed by the lifecycle policies of the various runtimes as illustrated in this diagram:

![The support lifecycle for various .NET platforms, represented as a set of timelines, showing the published plans for widely used versions that are current as of 2023, with a particular focus on which versions will be current as of November 2023. The top section of the diagram shows .NET releases starting with .NET 6.0 being released in November 2021, and shows for each subsequent release occurring in November of each subsequent year, up as far as .NET 13.0 in November 2028. It also shows that even-numbered releases are Long Term Support (LTS for short) releases, supported for 3 years, while odd-numbered releases are supported only for 18 months. The section beneath this shows that .NET Framework versions 4.7.2, 4.8.0, and 4.8.1 will all be in support as of November 2023, and will continue to be in support beyond the timescale covered by this diagram, i.e., beyond November 2028. The section beneath this shows the release plan for MAUI, starting with version 8.0 on November 2023, and subsequent releases at the same time each subsequent year, up to version 13.0 in November 2028. The diagram shows that each of these versions is supported for only 18 months. Beneath this is are two lines showing Xamarin iOS 16.0, and Xamarin Android 13.0 support being active on November 2023, and running for 18 months. Beneath this is a line showing UWP version 10.0.16299 support being active on November 2023, and running beyond the timescale covered by the diagram. Beneath this is a section showing that Unity 2021 was released in 2021, and will go out of support near the end of 2023, and it shows a Unity 2022 release labelled as "Release soon," with a release date somewhere in the middle of 2023. The bottom of the diagram shows the endjin logo, and endjin's corporate motto: "we help small teams achieve big things."](RX-Platform-Support-Roadmap.png ".NET Platform Support Roadmap")

(If you feel we are missing any important runtimes, please comment at https://github.com/dotnet/reactive/discussions/1868)

The remainder of this document describes the thinking behind this work plan.

## Current problems

There are five main problems we want to address.

* there have been no recent releases
* the build has fallen behind current tooling
* Rx causes unnecessary bloat if you use it in conjunction with certain modern build techniques such as self-contained deployments and ahead-of-time compilation
* the backlog of issues has been neglected
* the asynchronous Rx code in this repo is in an experimental state, and has never been released in any form despite demand

### No recent releases

This table shows the most recent releases of the various libraries in this repo:

| Library | Version | Date |
|---|---|---|
| `System.Reactive` (Rx) | [5.0.0](https://github.com/dotnet/reactive/releases/tag/rxnet-v5.0.0) | 2020/11/10 |
| `System.Interactive` (Ix) | [6.0.1](https://github.com/dotnet/reactive/releases/tag/ixnet-v6.0.1) | 2022/02/01 |
| `System.Linq.Async` | 6.0.1 | 2022/02/01 |
| `System.Reactive.Async` (AsyncRx) | None | None |

Note that the `System.Linq.Async` family of NuGet packages is built from the `Ix.NET.sln` repository, which is why it has exactly the same release date and version as `Ix`, and also why there is no distinct release for it in GitHub. It is not strictly an Rx feature, but then again neither is `System.Reactive`.

Note that for years, Rx had been on its own series of version numbers, and it was a coincidence that it happened to be on v4 shortly before .NET 5.0 was released. However, the Rx.NET 5.0.0 release declares itself to be "part of the .NET 5.0 release wave." This has led to the unfortunate perception that there should have been a new release of Rx for each version of .NET, with matching version numbers. There is no technical requirement for this—Rx 5.0.0 works just fine on .NET 7.0. The real issue here is just that development has stopped—there are bugs and feature requests, but there has been no new Rx release for well over 2 years.

### Build problems on current tooling

The tools used to build Rx—notably the .NET SDK and Visual Studio—evolve fairly quickly, with the effect that if you install the latest versions of the tools and open up the `System.Reactive.sln`, you will not get a clean build. Problems include:

* VS complains that the project targets versions of .NET that are not installed, unless you install out-of-support .NET Core SDKs (going back to .NET Core 2.1)
* If you do install the older .NET and .NET Core SDKs, Visual Studio will emit warnings telling you that the relevant frameworks are out of support and will not receive security updates
* There are numerous problems with the UWP tests:
  * The project enabling tests to execute on UWP (`Tests.System.Reactive.Uwp.DeviceRunner.csproj`) won't load due to incompatibilities with the version of `Coverlet.Collector` in use
  * If you remove the reference to `Coverlet.Collector`, Visual Studio reports that the UWP test project's references to the `System.Reactive` and `Microsoft.Reactive.Testing` projects are not allowed (with no explanation as to why) although it appears to build anyway
  * When you attempt to run the unit tests in Visual Studio's Test Explorer, none of the UWP tests run—they all just show a blue exclamation mark, with no explanation provided as to why they were not run; this is because Rx uses the [xUnit.net](https://xunit.net/) unit testing tools, and it appears that the [UWP support in these tools no longer works](https://github.com/xunit/devices.xunit/issues/171)—if you follow the [xUnit instructions for creating a brand new UWP project from scratch and adding a test](https://xunit.net/docs/getting-started/uwp/devices-runner) it does not work; it appears that the only update to the [xUnit UWP test runner](https://github.com/xunit/devices.xunit/commits/master) since November 2019 was a change to the logo; in fact the last release was in January 2019 (back when Visual Studio 2017 was still the latest version), with almost all subsequent updates to the repo being dependabot updates rather than new development
* Some of the certificates used for building certain tests have expired
* There are some warnings relating to C#'s nullable reference types feature
* Newer analyzer rules cause a huge number of warnings and diagnostic messages to appear
* Visual Studio modifies some Xamarin-related auto-generated files due to changes in the tool versions

### Unnecessary bloat with certain build techniques

If you use .NET to build a desktop application, and if you create a self-contained deployment, adding a reference to Rx can cause tens of megabytes of components to be added to your application unnecessarily.

There are a few instances in which projects have effectively built in their own miniature versions of Rx because of the problems that can occur when using the real `System.Reactive`. The Avalonia team have run into exactly this sort of problem (see https://github.com/AvaloniaUI/Avalonia/issues/9549) with the result that they have removed Rx from their codebase (see https://github.com/AvaloniaUI/Avalonia/pull/9749) and replaced it with just such an embedded version.

The basic problem is that the current Rx codebase makes WPF and Windows Forms support available to any application that targets a form of .NET in which those frameworks are available.

It might not be obvious why that's a problem. In fact, for .NET Framework it's not a problem: if you target .NET Framework, you will be deploying your application to a machine that already has all of the WPF and Windows Forms components, because those are installed as an integral part of .NET Framework. So there's no real downside to Rx baking in its support—the UI-framework-specific code accounts for a tiny fraction of the overall size of Rx, and it could be argued that the complexity of separating support for these libraries out into separate libraries offers no meaningful component size benefits.

The first few versions of .NET Core did not offer Windows Forms and WPF. However, .NET Core 3.0 brought these frameworks to the .NET Core lineage, and they continue to be available in the latest (non-.NET-Framework) versions of .NET. This is when baking support for these UI frameworks into Rx became problematic, because there are often good reasons to include copies of runtime libraries in application installers. But this means that taking a dependency on WPF can cause a copy of all of the WPF libraries to be included as part of the application.

Rx's NuGet packages include both `net5.0` and `net5.0-windows` targets. The `net5.0` one does not include the Windows Forms or WPF Rx features, whereas the `net5.0-windows` one does. Any application that targets `net5.0-windows` will end up with the `net5.0-windows` flavour of Rx, which means it will end up with dependencies on WPF and Windows Forms. If an application running on Windows uses some other UI framework such as Avalonia, it will still target `net5.0-windows` which means it will end up with a dependency on both WPF and Windows Forms despite using neither. And in either self-contained deployments, or certain ahead-of-time scenarios, this means those libraries will get deployed as part of the application, and they are tens of megabytes in size!

There's one more issue related to bloat: in to use trimming in .NET 6.0 it is necessary for components to be built in a way that specifies that they can be trimmed. These settings are not present, preventing Rx from being trimmed.
 
### Unaddressed backlog of issues

There are over 101 items on the Issues list. These need to be triaged, and a roadmap formed that will guide if and when to address them.

 
### AsyncRx.NET unreleased

The experimental AsyncRx.NET project was added to the repository in 2017. There has not yet been a release (even a preview) of these libraries. A lot of people would like to use them.


## UWP is a problem

Before proceeding with proposals, it's important to understand how many problems UWP causes, and also to understand why Rx still supports it despite these problems.

The [Universal Windows Platform, UWP](https://learn.microsoft.com/en-us/windows/uwp/get-started/universal-application-platform-guide) was originally conceived as a way to build applications that would run not just on the normal editions of Windows (specifically, Windows 10, at the time UWP was introduced) but also various other Windows 10 based operating systems including XBox, HoloLens and the (now defunct) Windows 10 Mobile platforms.

.NET on UWP is a bit strange. It uses neither the old .NET Framework, nor the current .NET runtime. UWP has its origins in the older Windows 8 store app platform (briefly but memorably called Metro), which had its very own CLR. The Windows 8 versions had TFMs of `netcore`, `netcore45` and `netcore451` (not to be confused with the `netcoreappX.X` family of TFMs used by .NET Core), and UWP recognizes the `netcore50` moniker (which has absolutely nothing to do with .NET 5.0) although this is considered deprecated, with `uap`, `uap10.0` or SDK-version-specific forms such as `uap10.0.16299` being preferred. This particular lineage of .NET supported ahead-of-time (AoT) compilation long before it came to more mainstream versions of .NET (mainly because the early ARM-based Windows 8 devices needed AoT to deliver acceptable app startup time). It also has a load of constraints arising from the goal for Windows Store Apps to be 'trusted'. Certain fairly basic functionality such as creating new threads was off limits, to prevent individual applications from having an adverse impact on overall system performance or power consumption on small devices.

This caused problems for libraries that need to do things with threads, with Rx's schedulers being a prime example. What's the `NewThreadScheduler` supposed to do on a platform that doesn't allow applications to create new threads directly? There are workarounds, such as using the thread pool, but those workarounds are suboptimal if you're running on unrestricted platforms. This leads to a choice between using an unsatisfactory lowest common denominator, or somehow arranging for different behaviour on UWP vs other platforms.

(Older versions of Rx solved these kinds of problems with a 'platform enlightenments' strategy enabling a single core set of RX binaries to be produced along with small platform-specific components that could be discovered at runtime that would supply the best implementations possible on the platform in use. However this was dropped, partly due to an unfortunate sequence of events starting with the problems described in https://github.com/dotnet/reactive/issues/97 then followed by the need to fix some of the problems that arose from the first fix to that, described at https://github.com/dotnet/reactive/issues/199#issuecomment-266138120. (It's possible, by the way, that the original problems that drove this are no longer issues thanks to changes in the range of supported .NET platforms and improvements to NuGet.) This why today, the main `System.Reactive` package includes a UWP target.)

Microsoft's own [guidance for writing Windows apps](https://learn.microsoft.com/en-us/windows/apps/get-started/) strongly de-emphasizes UWP. In the version of that page visible in February 2023, UWP is hiding as the fourth and final tab in the "Other app types" section. If you show that tab, it ends by warning you that you will not have access to APIs in the newer Windows App SDK, and encourages you to use WinUI instead of UWP. It even goes as far as to encourage you to migrate existing UWP apps to WinUI 3.

Nonetheless, UWP is still supported, and Microsoft continues to update the tooling for UWP. You can create new UWP applications in Visual Studio 2022. Microsoft ensures that many libraries and tools (including their own MSTest test framework) operate correctly when used with UWP. However, it is very much end-of-lifecycle support: everything works, but nothing is being updated. In particular, the project system for .NET UWP apps has not been updated to use the modern [.NET project SDK](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/overview) system that all other .NET projects have.

This lack of .NET project SDK support makes UWP a massive headache for multi-target libraries such as Rx.

A .NET SDK style project can be *multi-target*, meaning that it can create a NuGet package targeting multiple target framework monikers (TFMs) such as `netstandard2.0` and `net6.0`. However, if you try to include UWP-specific TFMs such as `uap10.0`, you will be confronted with a bewildering array of perplexing build errors, because Microsoft has not done the work that would be required to provide full UWP support in the .NET SDK project system. One way to deal with this is to use the [MSBuildSdkExtras](https://github.com/novotnyllc/MSBuildSdkExtras) project developed by Claire Novotny. That project does a remarkable job of getting the tools to do something they don't really want to do (i.e., to target UWP) but it can never do a perfect job, because it is trying to provide functionality that should really be intrinsic to the tooling. For example, the [MSBuildSdkExtras readme](https://github.com/novotnyllc/MSBuildSdkExtras/tree/b58e1d25b530e02ce4d1b937ccf99082019cdc47#important-to-note) notes that you can't run `dotnet build` at the command line if you're using this; you have to run MSBuild instead.

The lack of official support for UWP in a .NET project SDK world is also behind at least one of the build problems described above. Fundamentally, getting UWP to play in this world requires some hacks, and as a result, a few things don't quite work properly.

UWP is also not widely supported in the .NET ecosystem. It does not appear to be possible to use  xUnit with UWP on current versions of Visual Studio. We appear to have only two options: drop support for UWP, or move from xUnit to MSTest.

Given that it causes all these problems, why on earth are we supporting UWP if Microsoft is strongly discouraging its use, and pushing people onto WinUI instead?

### Why UWP in 2020?

Back when Rx 5.0 was released, the need for UWP support was stronger, because the WinUI libraries being recommended now in 2023 were relatively new back then. UWP was the most practical option for building Windows applications in .NET that made use of features that Windows exposes through its WinRT-style APIs. For example, if you wanted to write a .NET app that could be offered through the Windows Store, and which made use of the camera, you had to use UWP.

Since UWP was the only practical option for many scenarios, it would have been wholly inappropriate to drop support for it at that time.

What is less clear is why UWP support could not have been provided via .NET Standard 2.0 in Rx 5.0. This was not possible when Rx first introduced UWP support because of the lowest common denominator problems described earlier. For many years, writing a single-target `System.Reactive` library with no special UWP support would produce a suboptimal experience for some platforms, or possibly a broken experience on UWP. However, that changed in 2017 with the Windows 10 'Fall Creators Update' (version 1709). Starting with that version, UWP provided full support for .NET Standard, which meant implementing the various threading APIs that had been off limits in earlier versions. The last version of Windows **not** to support this (Version 1703) went out of support in October 2018, meaning that Rx v4 (first release May 2018) could not depend on it, but by the time [Rx.NET v5.0.0](https://github.com/dotnet/reactive/releases/tag/rxnet-v5.0.0) shipped in November 2020, .NET Standard 2.0 was something that had been available on UWP for all versions of Windows still in support for over two years.

### Why UWP in 2023?

Over two years have passed since Rx 5.0 was released. Microsoft actively discourages the use of UWP, so why do we still target it? One obvious answer is that there has been no new release of Rx in the intervening time. However, it's not as simple as that.

Although Microsoft pushes people strongly in the direction of WinUI v3, the fact is that if you want to write a .NET Windows Store application today, certain Windows features are only available to you if you target UWP. For example, full access to the camera (including direct access to the raw pixels from captured images) is currently (February 2023) only possible with UWP.

We would dearly love to rip out all of the UWP-specific code in Rx, because it would simplify the build and fix a lot of problems. But since developers still sometimes find themselves with no option but to use UWP, Rx needs to continue to work on UWP. However, it's worth noting that these two things are not the same:

1. supporting UWP
2. providing UWP-specific targets in `System.Reactive`

We believe that 1 continues to be necessary, but we would prefer to stop doing 2. We will provide full support for UWP but ideally, we would do this with all UWP-specific code separated out of the main solution. Unfortunately, a small amount of UWP-specific API surface area has crept into `System.Interactive`. (The `ThreadPoolScheduler` looks slightly different in the UWP targets, because it provides properties enabling you to set the [`WorkItemPriority`](https://learn.microsoft.com/en-us/uwp/api/windows.system.threading.workitempriority) and [`WorkItemOptions`](https://learn.microsoft.com/en-us/uwp/api/windows.system.threading.workitemoptions) it should use.) Our current view is that since this already represents a divergence between the API surface area of the UWP target from every other target, that it would be worth tolerating a breaking change that affects only UWP code in order to remove the UWP-specific target from `System.Reactive`. But regardless of how we achieve it, one way or another we need to continue to provide support for running on UWP for at least the next couple of versions.

We have produced a prototype demonstrating what Rx might look like if the UWP parts were separated: https://github.com/idg10/reactive/pull/3. Since this removes the UWP target from `System.Reactive`, this includes a breaking change for UWP clients: they will find that the `ThreadPoolScheduler` now looks identical to the ones available to all other Rx clients; if they need the old functionality it is available through a new `WindowsThreadPoolScheduler` in the `System.Reactive.Windows.Threading` library, which is identical to the old UWP-specific `ThreadPoolScheduler` in everything but name.


## Plans for upcoming release

We need to decide whether we should try to solve all of the problems in one step, or whether we produce any interim releases that get us part way to a solution. Our current position is that we will not attempt to do everything at once. We would like to get automated tests running against .NET 6.0 and .NET 7.0 in place as soon as possible so that we can verify that these platforms are supported. (This necessarily entails fixing the problems that prevent Rx from being built using the latest tool versions.) And we would like to make preview builds of AsyncRx.NET available as quickly as possible.

The issues around UI framework dependencies are arguably trickier, because fixing these will necessarily be more disruptive. Fixing the code bloat problems will mean ensuring that projects targeting some Windows-specific TFM won't automatically get the WPF and Windows Forms support just because they use Rx. This seems like it would need to be a breaking change (unless we moved all of Rx into some completely new NuGet packages, and turned the existing `System.Reactive` package into a façade offered only for backwards compatibility). Moreover, there are various subtle issues surrounding this, and we would need to be certain that we were not causing any regressions. For example, we would need to be certain that we were not re-introducing the old bug (https://github.com/dotnet/reactive/issues/97) in which plug-in hosts (notably Visual Studio) could load different plug-ins that had different opinions over whether to load the `netstandard2.0` or the `net4x` flavour or Rx, resulting in unresolvable conflicts and runtime errors. We would also need to avoid re-tracing the steps that caused the problems in https://github.com/dotnet/reactive/issues/199#issuecomment-266138120. We don't want to delay everything else while we test strategies for this problem. Moreover, there are now numerous .NET frameworks (including, but not limited to Avalonia, Unity, Xamarin, WinUI) that we would want to be confident of working well with. So although we want to resolve these particular problems as soon as possible, we think this will take time, and don't want to delay the other goals.

### Rx v6.0
The first priority has to be to update the build so that it is possible to work on Rx with current development tools. We can't fix the other issues if we can't work on Rx. This will entail removing older, unsupported targets. So the next drop is likely to target:

* `net6.0` (with test projects also targeting `net7.0`)
* `net6.0-windows.XXXXX`, most likely with one of the following versions:
  * `net6.0-windows.18362`, the oldest TFM we can target with VS 2022, which corresponds to Windows 10 version 1903, which shipped in May of 2019 and went out of support in November of 2020
  * `net6.0-windows.19041`, the same version of Windows as in the Rx 5.0 (which targets `net5.0-windows.19041`), which corresponds to Windows 10 version 2004, which shipped in May of 2020 and went out of support in December of 2021
  * `net6.0-windows.10.0.20348`, the oldest SDK version that is supported by all versions of Windows 10 and Windows 11 that are currently still in support (which means that this is the newest version we can target while still working on all versions of Windows that are still in support)
* `net472`
* `netstandard2.0`
* `uap10.0.XXXXX`, with the following versions being the obvious candidates:
  * ~~`uap10.0.16299`~~: this is the version Rx 5.0 targets, but the Windows 10 SDK 10.0.16299 is not supported in Visual Studio 2022, so this is not an option
  * `uap10.0.18362`: as described above, this is the oldest option with the current tooling
  * `uap10.0.20348`: as described above, this is the newest version that works for all versions of Windows currently in support

Unless xUnit updates its support for UWP very soon, we would most likely replace the use of xUnit with MSTest, because that seems to be the only way to get the UWP unit tests to run.

We should consider whether there are easily addressed .NET 6.0-related issues that we should consider including in this first release, including:

* https://github.com/dotnet/reactive/issues/1683 - mark assembly as trimmable

Since we want to get this release done quickly, we don't want to make it dependent on fixing every outstanding issue, but once a v5.1 (or whatever) is in place, we could look at fixing outstanding bugs in subsequent v5.1.x releases while the releases described below are tackled. And we would want to continue to do that if we do end up needing to make breaking changes in the next major release—if it turns out to be impossible to provide a backwards-compatible façade over the packages in which UI frameworks are separated out, we will want to keep this v5.1 (or whatever) version as a supported series of releases for those who cannot or do not want to move to the later version.

A more detailed description of how this first step would be achieved can be found in [ADR 0001-net7.0-era-tooling-update](./adr/0001-net7.0-era-tooling-update.md).

Note that it is an unfortunate coincidence that the version number for this release, v6.0, makes it sound like it is somehow connected with .NET 6. We are in fact support .NET 7 in this release too. (There is no need for a `.net7.0` TFM, because the `net6.0` target will run perfectly well on .NET 7, and there is no .NET 7-specific functionality that this release will be using.) We are bumping the major version number because we are dropping support for some old TFMs, which seems like a sufficiently major change to warrant a new major version number. (There is no intention to make any breaking changes other than dropping support for runtimes which are themselves out of support.)

### Rx.Async

We want to make preview builds of Rx.Async available as soon as possible. We don't know of any technical reason to tie this to the release described in the preceding section. In the long run we expect that we are likely to new want Rx.NET and Rx.NET.Async releases as older TFMs go out of support, and we will probably keep major version numbers in sync to make it clear that these two libraries are in the same era. But aside from that, there's no reason to tie releases of these two libraries tightly together. (Bugfixes and new functionality are likely to be released independently for each library.) Moreover, since Rx.Async is currently in a preview state, we wouldn't want to unify things yet even if that were a long term goal—it would be better to make `alpha` builds available as soon as possible. So the work to do this could proceed concurrently with the work described in the preceding section.

### Rx v7.0

This is the release in which we would want to address the code bloat problems experienced by Avalonia and other UI frameworks. We believe this will entail moving all UI-framework-specific code out into separate components. (This would be only a partial unwinding of the [great unification](https://github.com/dotnet/reactive/issues/199)—we would not be reinstating the separation into interfaces, core, etc.)

We also want to make it possible for third parties to produce their own integrations with Rx, and for it to be possible for these to work every bit as well as the WPF, Windows Forms, and UWP integration—we want to avoid a situation where certain UI frameworks are privileged, if possible. Related to this, we want to seriously consider splitting the Rx repository up. A way to ensure that 3rd party frameworks aren't 2nd class citizens is to make our own UI framework support work the same way. (An additional motivation for this is that we would really like to remove all trace of UWP from the main Rx repo, because the poor state of UWP tooling means that wherever there is UWP, there tends to be trouble.) We would need to solve some problems around testing: how would we ensure that changes in the main repo don't cause unnoticed failures in certain frameworks? How would we make the core suite of tests available for frameworks that would need to run them against a different platform? (Today, UWP testing is achieved by linking all the files from the main test project into the UWP test runner. That wouldn't work if the UWP code was in a separate repo.) Moving Rx into its own organization with separate repositories for the core and framework-specific parts would also make it easier to provide separate repos for documentation and samples.


## Platforms and frameworks to test

Rx is very widely used, on many different flavours of .NET, and in many different frameworks. One of the challenges with updating it is that we might break some scenarios without realising it. It would therefore be a good idea for us to curate a list of frameworks which, although we don't build any specific support for, should be able to use Rx via one of its TFMs without problems. One of the quality bars for any Rx release should be that we are confident that it can be used without problems from any of these frameworks.

This is not an exhaustive list, but it should include:

* Avalonia
* Unity
* Xamarin
* WinUI
* Visual Studio plug-ins
* MAUI
* Blazor
* Uno

Obviously, WPF, Windows Forms, and UWP support are also important, but since we already provide framework-specific features for those, they are different. This list is for platforms that we don't intend to provide any explicit features for, but which we want to work anyway.