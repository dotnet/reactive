# Plug-Ins

Rx.NET versions up to and including v3.0 could encounter a problem with plug-in systems. The main issue for this in GitHub is [issue 97, 'NET 4.0 and .NET 4.5. versions need to be signed with different keys'](https://github.com/dotnet/reactive/issues/97). That title doesn't give much clue as to the nature of the problem, mainly because it is named for a proposed solution, one that Rx.NET never in fact adopted. Here's a better description:

"A .NET Framework plug-in host can end up running with an Rx.NET assembly built for an older version of .NET than it was built against"

Two plug-ins hosted in the same application could both depend on the same version of Rx.NET, but could disagree about which
actual Rx.NET DLLs to load. Since .NET Framework is not capable of allowing two different plug-ins to load
two different copies of a single DLL when both have the same strong name, one of the plug-ins would not get
the DLL it was expecting. This could result in runtime failures.

Although this problem was fixed in Rx .NET v3.1, there was a regression in Rx .NET v5.0, and the problem
continues to be present in Rx .NET v6.0.

## Versions

Reported for [v2.2.5](https://github.com/dotnet/reactive/releases/tag/v2.2.5)

Fixed in [v3.1.0](https://github.com/dotnet/reactive/releases/tag/v3.1.0) by 

Fix removed in [v4.0.0](https://github.com/dotnet/reactive/releases/tag/rxnet-v4.0.0), but by change, removing the fix didn't cause the issue to reappear in this version.

Regression in [v5.0.0](https://github.com/dotnet/reactive/releases/tag/rxnet-v5.0.0)

## Problem Detail

A plug-in built against the .NET Framework 4.5 version of Rx might find itself using the .NET Framework 4.0 version at runtime, resulting in a `MissingMethodException` if it used functionality only available in the .NET FX 4.5 version.

The root cause is that for each of the assemblies contained in the Rx NuGet packages, there would be two copies either of which _could_ run on .NET FX 4.5, each of which had the same assembly identity (same name, key token, and version number), but with different API surface areas.

For example, if you look at [Rx-Core v2.2.5](https://www.nuget.org/packages/Rx-Core/2.2.5#supportedframeworks-body-tab) you will see that it has `net40` and `net45` components. The NuGet site lists a much wider range of .NET Framework versions on which this package will run, but it highlights those two because the package includes versions specifically for those two runtime versions; all other supported versions end up using either the `net40` or `net45` version. You can see this in the [package explorer](https://nuget.info/packages/Rx-Core/2.2.5)—the package's `lib` folder contains `net40` and `net45` subfolders. Each of these targets had exactly the same strong name.

.NET Framework 4.5 (and all subsequent versions of .NET FX) can load either the `net40` or the `net45` version. But why would you ever end up with the older of the two? In a straightforward .NET application that would never happen, because the NuGet packaging rules would determined at build time that the `net45` version is most appropriate: it would only ever select the `net40` version in a project that targets .NET Framework 4.0.

Plug-ins create a problem because there is no longer a single build process to determine which particular assembly should be used from a NuGet package. With an application that has loaded 2 plug-ins there are three build processes:

* Host application running on .NET Framework 4.8.2, using `System.Reactive` v6.0.0
* Plug-in A, built for an older version that ran on .NET Framework 4.5 using `Rx-Main` v2.2.5
* Plug-in B, built for a much older version that ran on .NET Framework 4.0 using `Rx-Main` v2.2.5

The crucial fact to understand here is that the output of all three build processes is essentially a folder with a bunch of DLLs. If the plug-ins used the `Rx-Main` NuGet package, there will be no direct record of this fact in their build output. The build process for each plug-in will copy DLLs out of any NuGet packages the plug-in uses, putting those copies in the build output, and not providing any record of where they came from.

So when Plug-in A was built, the build tools will have been targeting .NET Framework 4.5. (The plug-in may have been built many years ago. The fact that it targets .NET FX 4.5 suggests it is probably old.) That means that when the build system looked inside the various NuGet packages that `Rx-Main` depends on, it will have determined that the assemblies in `lib\net45` were the best choice, so it will have copied the files from that folder into the build output.

Similarly when Plug-in B was built, the build tools will have been targeting .NET 4.0. (Since Plug-in B uses Rx v2.2.5, it can have been built no earlier than August 2014, so .NET FX 4.5 definitely existed at that time. But there were good reasons to continue to target .NET 4.0 at that time. Perhaps the plug-in author wanted to support running on Visual Studio 2010, which was still in widespread use at that time.) So the build tools will have determined for each of the various NuGet packages that `Rx-Main` depends on that the assemblies in `lib\net40` were the best choice, so it will have copied the files from that folder into the build output.

Now consider what hapens when the host application loads these plug-ins. Imagine the host is fairly modern, and targets .NET Framework 4.8.1. Suppose it happens to load Plug-in B first. The host application will tell the .NET assembly resolver to load the plug-in's main assembly. At some point the plug-in will attempt to use Rx, at which point the assembly resolver will see that it wants (amongst other things) `System.Reactive.Core, Version=2.2.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35`.

It's possible that host app itself uses Rx, but if it uses a newer version (e.g. v6.0.1) the assembly resolver will consider that a different assembly identity. (As it happens the assembly names changed in Rx 4.0, so not even the simple name will match.) So the assembly resolver knows it needs to find the assembly the plug-in needs. The exact rules for where it might try looking are complex, but in this case it will end up finding the copy of `System.Reactive.Core.dll` that is in the plug-in folder. This will be the copy from the NuGet package's `lib\net40` folder.

So the resolver has found an assembly that was built for .NET 4.0, but that's fine: we're running on .NET 4.8.1, but that is perfectly happy to load a .NET 4.0 component.

Now consider what happens if Plug-in A loads some time later. When it first tries to use Rx, the assembly resolver will see that it also uses `System.Reactive.Core, Version=2.2.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35`. It has already loaded an assembly with that exact name, so it will use that. It **won't** load the copy of `System.Reactive.Core.dll` that was in Plug-in B's folder. So Plug-in B gets the copy of this DLL from Plug-in A's folder.

And now we have a problem. Those two copies of `System.Reactive.Core.dll` are different. Plug-in B was built for the copy from the `Rx-Core` package's `lib\net45` folder, and that copy defines a few methods that are unavailable in the version in the `lib\net40` folder. (These new methods depend on functionality that was added to .NET Framework in version 4.5.) If Plug-in B attempts to use any of those methods, the CLR will throw a `MissingMethodException`, because the DLL it actually loaded was the .NET FX 4.0 version.

The basic problem here is that the .NET Framework can't load two different assemblies with the same strong name. Since Rx supplies two materially different copies of the same assembly each with the same strong name, we have a problem.

If Plug-in A had loaded first, it would have been fine: Plug-in B would have been upgraded to the .NET 4.5 version, and since that is fully backwards compatible with the .NET 4.0 version, it would have been happy.

You might think that host applications should detect this, and load plug-ins in the right order to avoid such problems. But they can't easily detect that this has happened, and also there are often good reasons not to load plug-ins until the host application knows they are required.

In essence, .NET Framework plug-in systems contravene a basic assumption of NuGet: that it will be possible
to get a complete overview of all the components required, and to apply a resolution process to pick the
specific DLLs that will be used. (.NET Core and subsequence .NET versions don't have this problem because
the `AssemblyLoadContext` makes it possible for each plug-in to load whatever version of a DLL it wants,
regardless of what other plug-ins may have done.)


#### It's different on .NET (Core)

The problem just described only afflicts .NET Framework. .NET Core introduced a new mechanism that makes it possible to introduce per-plug-in assembly resolution contexts, making it possible for each plug-in to use the DLLs it supplied, even if those happen to have exactly the same names as DLLs already loaded.

.NET Core didn't appear until a few years after this problem came into existence, but I mention it because we need to ensure we don't recreate any of these old problems, so we need to take into account the changes that .NET Core (and subsequent 'modern' versions of .NET that dropped the 'Core' moniker but are part of that lineage: .NET 5.0, 6.0, etc.) introduced.


## The fix in Rx 3.1

[Pull Request #212](https://github.com/dotnet/reactive/pull/212) implemented the fix described in
[Issue #205](https://github.com/dotnet/reactive/issues/205). This change first appeared in Rx v3.1: it gives the DLLs in the `lib/net45` and `lib/net46` folders subtly different version numbers. (By this time (September 2016), Rx was no longer shipping versions targetting .NET 4.0, but it did offer both 4.5 and 4.6 components, so the same problem could have arisen without this fix.)

The .NET 4.5 version's full name was `System.Reactive.Core, Version=3.0.1000.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263` whereas the .NET 4.6 version's name was `System.Reactive.Core, Version=3.0.3000.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263`.

The trick here is to use the 3rd part of the assembly version number to ensure that these two different assemblies have different strong names. (These are all inside the NuGet package with version 3.1.0, so from a package perspective these are all nominally the same version. But from a .NET assembly perspective they have slightly different names.) That way in the plug-in scenario described above, it's not longer a problem if some plug-in already loaded the .NET FX 4.5 version and then a different plug-in wants the .NET FX 4.6 verison. The slight difference in the version in the assembly names means that when the assembly resolve looks for the assembly required by that newer plug-in, it does _not_ think that it already has the required assembly because the version number doesn't match. And so it goes looking for the assembly, and will load it out of the newer plug-in's folder.

So we end up with two copies of `System.Reactive.Core.dll` loaded into the application: the .NET FX 4.5 version and the .NET FX 4.6 version. (And since our host also uses Rx itself, Rx v6 will also be loaded, but as mentioned earlier, the names changed, so it will be using `System.Reactive.dll` v6.)

The solution to [#97](https://github.com/dotnet/reactive/issues/97) that [#205](https://github.com/dotnet/reactive/issues/205) describes was implemented in [PR 212](https://github.com/dotnet/reactive/pull/212).

Unfortunately, this created some new problems.


### The problems caused by the initial solution

In a [comment on #199](https://github.com/dotnet/reactive/issues/199#issuecomment-266138120), Claire Novotny listed various issues that she considered to have arisen from the initial solution ([#205](https://github.com/dotnet/reactive/issues/205)) to [#97](https://github.com/dotnet/reactive/issues/97):

* [#264](https://github.com/dotnet/reactive/issues/264): ILRepack causes issues with the PlatformEnlightenmentProvider
* [#295](https://github.com/dotnet/reactive/issues/295): System.Reactive (3.1.0) forces dependency on NETStandard.Library
* [#296](https://github.com/dotnet/reactive/issues/296): Wrong AssemblyVersion of System.Reactive.Linq in 3.1 for portable
* [#299](https://github.com/dotnet/reactive/issues/299): Version 3.1.1 has different DLL versions, depending on the platform
* [#305](https://github.com/dotnet/reactive/issues/305): Incompatibility between System.Reactive.Core and System.Reactive.Windows.Threading 3.1.1 NuGet packages

With some of these, the main issue is really something else, but the versioning strategy complicated things. But some, like [#296](https://github.com/dotnet/reactive/issues/296), are directly associated with this design.

The basic issue is that if you can end up with multiple dependency paths to what should logically be the exact same Rx component, but where those dependencies end up specifying slightly different version numbers. For example, if, say, `ExampleStd11Lib` is a .NET Standard 1.1 library that uses `System.Reactive.Core` v3.1.0, it will end up depending on `System.Reactive.Core, Version=3.0.1000.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263`. And if `ExampleNet46Lib` is a .NET FX 4.6 library that also uses `System.Reactive.Core` v3.1.0, it will end up depending on `System.Reactive.Core, Version=3.0.3000.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263`.

If we run the resulting application on .NET FX 4.6, we definitely want to use that second one. But the `ExampleStd11Lib` says it wants the first one. We can end up with both versions loaded, which can then cause baffling errors that appear to complain that some object is not of a particular type even though it clearly is. (This is because there are two identically-named versions of the same type.)

This can be handled with binding redirects: you can just configure the CLR to load the 3000 version when asked for the 1000 version, and this will ensure that both libraries are in fact using the same assembly. However, understanding the problem well enough to be able to write suitable binding redirects was a challenge, and not something we should be forcing developers to do.

This became less of a problem over time, because the build system was able to generate binding redirects for you. Even so, this is a problem you'd prefer not to have because in cases where the automated solution fails to do what you need, it is very hard to diagnose and fix.

#### It's different in .NET Core

.NET Core (and its successors, .NET 5.0, 6.0, etc.) made a very significant change in behaviour: they _won't_ load two different versions of the assembly here. They expect to load just one for any particular simple name, and consider an assembly number with a higher version than what was asked for to be acceptable.

So in the example above, if the 3.0.3000.0 version of the assembly gets loaded, the Core-etc. CLR considers that to be a perfectly acceptable resolution for the dependency on the 3.0.1000.0 version.


## How The Great Unification Paved the Way for a Regression

Rx 4.0 saw the "Great Unification" in which all the Rx.NET packages were merged into a single `System.Reactive` package. This move was partly due to the problems that arose from [#205](https://github.com/dotnet/reactive/issues/205)'s attempt to fix [#97](https://github.com/dotnet/reactive/issues/97), and partly because of ongoing confusion caused by the way that Rx was split across packages.

(As an aside, it would have been better if they'd left out the UI-framework-specific parts. Unfortunately, "everything" really did mean everything, which went on to cause some other headaches once the .NET Core versions of WPF and Windows Forms appeared. But that's a topic for other folders in this repo.)

At the same time, the version number hack that had been applied in Rx 3.1 did not continue with the
`System.Reactive.dll` files in Rx 4.0. (The old separate packages continue to this day to have the
version numbers that were applied in v3.1, but those packages are now just type forwarders providing
backwards compatibility with old code built for Rx 3.1.)

The decision to drop the version number hack doesn't appear to have been discussed in detail anywhere
in GitHub (as far as we've been able to tell). We think that the prevailing view was that the hack was
no longer required, because the unification rendered it unnecessary. However, if that was indeed
the view, it was an oversimplification. In fact, the main reason the Rx-3.1-style hack was no longer
necessary was that Rx 4.0 happened to have exactly one TFM that would be used on any .NET Framework
target.

Superficially that might not look true: it offers both `net46` and `netstandard2.0` and the latter
could in theory be used on .NET Framework 4.6.2 and later. However, the .NET build tools consider the
`net46` target to be a better match for any .NET Framework version from 4.6 onwards than `netstandard2.0`,
and `netstandard2.0` is not supported on any version of .NET Framework older than 4.6.2. So on all
versions of .NET Framework in existence that are new enough to be able to use Rx 4.0 at all, the `net46`
target would be selected.

For as long as that was true, the Rx 3.1 era hack was not necessary.

## The Regression in Rx 5.0

Rx 5.0 dropped the `net46` target, and added a `net472` target. Unfortunately, that meant that the
plug-in version problem was back. This is because there are, once again, two TFMs in Rx which are
applicable to different versions of .NET Framework.

If you build for .NET Framework 4.6.2, 4.7, or 4.7.1, you will get the `netstandard2.0` target. (The
`net472` target will not be available on these older versions. But those framework versions _do_ support
.NET Standard 2.0.) And if you build for .NET Framework 4.7.2 or later, you will get the `net472` target.

This means we're back in a new version of the situation that existed for Rx 3.0: two plug-ins could
target the same version of Rx.NET, but could disagree about which actual DLLs to load. To give a concrete
example of a problem that this can cause, suppose an application loads a plug-in that was built for
.NET 4.6.2 and Rx 5.0.0. This will ship a copy of the `netstandard2.0` build of Rx. Now suppose the same
application goes on to load a plug-in that is built for .NET 4.7.2 and Rx 5.0.0, and which uses an Rx type
specific to Windows Forms such as `System.Reactive.Concurrency.ControlScheduler`. This will fail because
it ends up with the `netstandard2.0` version of Rx, which does not include the Windows Forms support.
If the same two plug-ins load in the reverse order, both end up using the `net472` version of Rx, and
all is well—the .NET 4.6.2 plug-in will be unaware of the additional features in the `net472` version of Rx,
and the .NET 4.7.2 plug-in will have what it expects.

This situation continues to exist in current Rx.NET versions. (Again, it only affects .NET Framework,
because that does not have the `AssemblyLoadContext` that enables .NET plug-in hosts to avoid this problem.)


## Demonstrating the Problem

To be able to determine whether future versions of Rx.NET will have the same problems, we need a reliable
way to reproduce the issue. This folder contains a [.NET Framework console application, `PlugIn.HostNetFx`](./PlugIn.HostNetFx/)
that acts as a plug-in host, and also a [.NET console application `PlugIn.HostDotnet`](./PlugIn.HostDotnet/) that verifies that this problem does not occur in .NET Core/5+. There is also a [library project, `PlugIn`](./PlugIn/) that can be built in many different ways to produce various plug-ins targetting various versions of .NET Framework, and with dependencies on various versions of Rx.NET.

The `PlugIn` has code that can distinguish between versions of Rx.NET at runtime by checking for certain behaviours:

* [`PlugInEntryPoint.Net46Behaviour.cs`](./PlugIn/PlugInEntryPoint.Net46Behaviour.cs) can determine whether the version of Rx.NET available has the behaviour that was added in the `net46` target of Rx.NET 3.0.0. We use this when reproducing the problem on Rx 3.0, because it lets us work out whether the `net45` or `net46` DLL was loaded.
* [`PlugInEntryPoint.WindowsFormsAvailable`](./PlugIn/PlugInEntryPoint.WindowsFormsAvailable.cs) can determine whether Windows Forms support is available. We use this for Rx 4.0 and later (because that's when UI framework support got bundled into the main `System.Reactive` component), and it effectively lets us determine whether the .NET Framework DLL (`net46` or `net472`) got loaded (in which case UI features will be present) or we ended up with the `netstandard2.0` version.
* [PlugInEntryPoint.RxInfo.cs](./PlugIn/PlugInEntryPoint.RxInfo.cs) returns information about the specific version of Rx that the plug-in has access to at runtime

The host application takes the path to the two plug-ins it is to load. It reports any problems to standard out.

The [`CheckIssue97` project](./CheckIssue97/) orchestrates the test. It uses the command line argument structure common to the various checks in this repo, telling it which version of Rx.NET to test. It then generates numerous test scenarios, with different host frameworks. (It tests .NET Framework versions 4.62, 4.72, and 4.81. It tests .NET version 8.0.) It then determines various possible pairings of build configurations for the plug-in. It consults a list of known TFMs for which it could build the plug-in, and works out which of these will end up resolving to different Rx.NET target assemblies when building the `PlugIn` project. It then produces a list of every pair of build configurations for `PlugIn` where each plug-in will end up copying a different Rx target into the output, and each member of the pair can run on the same host (i.e., it won't pair a plug-in build targetting `net8.0` with one targetting `net472`, but it will pair `net45` and `netstandard2.0`). It then builds copies of the plug-in matching the two chosen configurations, and then runs the host pointing it these two built plug-ins.

It records the outcome of each pairing in a JSON file describing exactly what behaviour the test host observed.