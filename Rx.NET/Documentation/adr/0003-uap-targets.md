# Building uap10.0 (UWP) targets without MSBuild.SDK.Extras

Rx.NET can no longer depend [MSBuild.SDK.Extras](https://github.com/novotnyllc/MSBuildSdkExtras), because that project is no longer maintained. This document describes why Rx.NET has been using it, and how we are moving off it.

In short, Rx.NET used `MSBuild.SDK.Extras` to support UWP by offering `uap10.0` targets. Modern .NET projects don't support this TFM directly—new UWP projects still use the old project format that predates .NET Core. As of .NET 8.0, there's no official way to build a multi-target .NET NuGet package that includes a `uap10.0` target. For years, we worked around this using the [MSBuild.SDK.Extras](https://github.com/novotnyllc/MSBuildSdkExtras) project, which was written and maintained by someone who also used to be a maintainer on the Rx.NET project. The person no longer works Rx.NET, and nobody has done any work on `MSBuild.SDK.Extras` for several years.

As we started to prepare for the arrival of .NET 9.0, it became clear that it was time to stop using `MSBuild.SDK.Extras`.

## Status

Draft


## Authors

@idg10 ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/)).


## Context

[UWP](https://en.wikipedia.org/wiki/Universal_Windows_Platform) is an API Microsoft introduced with Windows 10 to support building Windows desktop applications (and, at the time, Windows Phone applications). Microsoft now discourages its used, encouraging developers to use [WinUI](https://learn.microsoft.com/en-us/windows/apps/winui/) instead. However, although UWP is no longer under active development, it continues to be supported. Visual Studio 2022, the current version of Visual Studio at the time of writing this, includes UWP tooling.

Rx.NET has supported UWP applications since v3.0.0 in 2016. When UWP first shipped, it could only use NuGet packages that provided a `uap10.0` TFM. At this time, the core Rx functionality was in `System.Reactive.Core`, and [v3.0.0](https://www.nuget.org/packages/System.Reactive.Core/3.0.0) of that included a `uap10.0` TFM. UI-specific functionality (notably schedulers with support for the UWP dispatcher) was in `System.Reactive.WindowsRuntime`, the same package that had historically supported the Windows 8 era Windows Store apps and also Windows Phone 8 apps. The UWP-specific features were implemented as a new target in that existing package because the dispatcher type in UWP was designed to look very similar to its equivalent in those other APIs. It was possible to write source code that could target all three platforms.

In 2017, [UWP got support for .NET Standard 2.0](https://devblogs.microsoft.com/dotnet/announcing-uwp-support-for-net-standard-2-0/). This appeared in the "Windows 10 Fall Creators Update" also known as version 1709. The corresonding SDK version for this was 10.0.16299. At this point, UWP applications weren't restricted to NuGet packages that offered explicit UWP support. Any NuGet package with a `netstandard2.0` target could be used. However, not everyone upgrades to the latest version of Windows immediately. The preceding version, 1703 ("Creators Update") remained in support until October 2019.

(An even older version of Windows 10, 1607, remains available on the [Long Term Servicing Channel](https://learn.microsoft.com/en-us/windows/release-health/release-information) and is currently slated to remain in support until October 2026. So even as I write this in October 2024, versions of Windows running a version of UWP that do _not_ support .NET Standard 2.0 will remain in support for another 2 years! However, since Rx.NET is not a Microsoft product, we don't attempt to offer that kind of extended life support. People who want to use Rx.NET on such ancient versions are free either to use older versions of Rx.NET, or to fork their own versions.)

So in practice, we needed to continue to offer `uap10.0` targets of the main Rx.NET functionality at least until October 2019. And the components of Rx.NET that offer UWP-specific functionality need to do so through `uap10.0` targets even now, because platform-specific functionality is not available on `netstandard2.0`.

So in October 2019 it would have become reasonable to drop `uap10.0` support for the core of Rx.NET (on the grounds that all versions of Windows in mainstream support were by then perfectly capable of using the `netstandard2.0` version in UWP). By this time, Rx.NET was on v4.2.0, so this was some time after the 'great unification' in which all Rx.NET functionality was merged into a single `System.Reactive` package. Although this solved some problems, it has gone on to cause enormous headaches.

One of the problems caused by the _great unification_ is that although UWP had become capable of using `netstandard2.0` it was no longer possible to remove that target from the `System.Reactive` package. This is because the UWP-specific features that used to live in `System.Reactive.WindowsRuntime` (e.g., schedulers offering UWP dispatcher support) had been moved into `System.Reactive`. They were only compiled into the `uap10.0` target, but if we removed them, anyone maintaining a UWP app who upgraded to the latest `System.Reactive` would find that their code no longer worked. If they were lucky they'd get a compile time error, but with complex NuGet dependency trees, it's not uncommon to have dependencies on multiple packages each with depdendencies on different versions of Rx.NET without the application developer ever explicitly having chosen to use Rx.NET. If upgrading some other package upgraded them to a new version of `System.Reactive` that no longer offered the `uap10.0` target (meaning they were using the `netstandard2.0` target in a newer version) they might get runtime errors complaining about a missing class.

So we continued to provide `uap10.0` targets for `System.Reactive` and a few other components.

[MSBuild.SDK.Extras](https://github.com/novotnyllc/MSBuildSdkExtras) made this possible, but it has not been updated since December 2021, meaning that depending on it is problematic.

Microsoft has announced that [.NET 9.0 will finally be getting some support for UWP](https://devblogs.microsoft.com/ifdef-windows/preview-uwp-support-for-dotnet-9-native-aot/), but as far as I can tell, this has a minimum target version of Windows 11 22H2 (26100). Windows 10 will remain in support for a few years yet, so we can't require that.

So we need to find a way to do something similar to what `MSBuild.SDK.Extras` achieved: i.e. to provide the settings required to convince the .NET SDK to build suitable targets.

## Decision

Firstly, we can't just set `<UseUwpTools>True</UseUwpTools>` to use the new UWP support in the .NET 9.0 SDK because that seems to depend on a target version of Window later than the minimum we need to support.

We will do the following:
* Remove all use of `MSBuild.SDK.Extras` from all `csproj` files and the `global.json` file
* Set MSBuild variables as necessary to enable `uap10.0.18362` targets to compile without error
* Add the required metadata references to enable `uap10.0.18362` targets to compile without error

We will *not* do the following:
* Attempt to import the full set of `.props` and `.targets` files normally used when building UWP apps

Our goal is _not_ to recreate as faithfully as possible the build environment that C# code would have inside a conventional UWP project. (That _is_ more or less what `MSBuild.SDK.Extras` attempted to do, because it was trying to be generally useful to any project that ran into this problem.) Our goal is to continue to be able to build binaries that work on UWP systems just like they always have. So we need only to do the bare mininum required for that to work. We don't need any XAML support, for example.

### Implementation details

To achieve this in practice requires us to solve a few problems. The following sections describe what has been done and why.

### `Directory.build.props` and `Directory.build.targets`

MSBuild

### TFM and Platform Versions

First, note that any project with a UWP has to use the plural `<TargetFrameworks>` MSBuild property. This includes the `System.Reactive.Windows.Runtime` project which targets this and nothing else. Normally you'd use the singular `<TargetFramework>` in that case, but this puts the build into a different mode, and the measures we've taken to enable UWP targets only work in multi-target mode.

The precise TFM we use is `uap10.0.18362`. The version number determines the We moved to this (from `uap10.0.16299`) in March 2023 as part of the work required to get Rx.NET building on Visual Studio 2022. This was a very conservative choice: it was the lowest version we could use that would work with the current tooling. The most recent version of Windows not to support 18362 had already been out of support for several years back when we did this.

We don't see any reason to change. Visual Studio 2022 is still the latest version, and it continues to support 10.0.18362 as a target version.

Note that the Windows version in a version-specific TFM doesn't strictly determine the OS minimum version. It determines which OS API version you build against, so in fact it determines how recent a set of OS APIs you can _attempt_ to use, but it is technically possible to write code that attempts to use an OS API but gracefully downgrades behavious if that API is not present. So some components will specify a lower `TargetPlatformMinVersion` than the version in their TFM.

The following versions are of interest. The comments about the status of the later versions was correct on 2024/10/24:

* `10.0.16299`: the version that added `netstandard2.0` support to UWP; this would be the lower bar for us were it not for the next item
* `10.0.17763`: the oldest version to support C#/WinRT, and therefore an absolute lower bar for us (aka 1809, this version is still in extended support)
* `10.0.18362`: the oldest SDK version supported in Visual Studio 2022
* `10.0.19041`: the version targeted by `windows-` TFMs; I never found out why it was this particular version, although it might simply be that this version of Windows (aka 2004, aka 20H1) which shipped in May 2020 was the current version when .NET 5 support was added to Rx.NET
* Windows 19045 (aka Windows 10 22H2) is the most recent version of Windows 10
* `10.0.22621`: the oldest Windows 11 version (22H2) still in GA support (enterprise only)
* `10.0.22631`: the oldest Windows 11 version (23H2) with GA support for Home, Pro and Education
* `10.0.26100`: the latest version of Windows (24H2)


So as it happens, we don't technically need anything newer than 10.0.17763. So we could specify that as the minimum platform version. However, there's no compelling reason to do this, and since 10.0.18362 is as far back as the current tooling fully understands, we make our minmum platform version match the one in the TFM:

```xml   
<TargetPlatformMinVersion>10.0.18362</TargetPlatformMinVersion>
<TargetPlatformVersion>10.0.18362.0</TargetPlatformVersion>
```

Note that it's necessary to specify the `TargetPlatformMinVersion` property. The `Microsoft.NetCore.UniversalWindowsPlatform` (which I'll discuss shortly) uses it, and we get a lot of perplexing compiler errors if we don't set this property.

By default the SDK will pull apart the TFM into various other properties that describe its components. However, if we let it do that with a UAP TFM, the build fails. So we do this:

```xml
<TargetFrameworkMoniker>.NETCore,Version=v5.0</TargetFrameworkMoniker>
<TargetFrameworkIdentifier>.NETCore</TargetFrameworkIdentifier>
<TargetFrameworkVersion>v5.0</TargetFrameworkVersion>
<TargetPlatformIdentifier>UAP</TargetPlatformIdentifier>
<NugetTargetMoniker>UAP,Version=v10.0</NugetTargetMoniker>
```

By default, those first two values would have become `UAP,Version=v10.0` and `UAP`, but this goes on to cause a cascade of errors because the rest of the SDK does not know what those are. The ".NETCore" v5.0 is not the same thing as .NET 5.0. I believe it corresponds to a short TFM of `netcore50` (whereas .NET 5.0 is `net5.0`). I think this is a hangover from the fact that the predecessors of UWP apps, Windows 8 Store Apps, were actually the first to ship a version of .NET Core. They used .NET Core before .NET Core 1.0 shipped, and confusingly they called it `netcore45` (`netcore451` in Windows 8.1). The list of [deprecated TFMs](https://learn.microsoft.com/en-us/dotnet/standard/frameworks#deprecated-target-frameworks) shows that `netcore50` was an old name for `uap10.0`. But apparently some parts of the SDK did't get that memo, and still require the old name to be used!

However, only _some_ properties should use the old name. If you need to set _all_ of those properties, because otherwise, other parts of the build system get confused (e.g., NuGet handling). So we need the ".NETCore" name in some places, and the "UAP" name in others.


### Packages and metadata

Normally, when you specify a TFM, the .NET SDK works out what framework library references are required and adds them for you. So if you write `<TargetFramework>net8.0<TargetFramework>` in a project file, you will automatically have access to all the .NET 8.0 runtime libraries. But because the .NET SDK does not support UWP, this doesn't work at all. So we need to do three things.

First, we need to set this property:

```xml
<NoStdLib>True</NoStdLib>
```

Without this, the build tools attempt to add a reference to `mscorlib.dll`, but they don't seem to realise that a) this is the wrong thing and b) they don't actually have a correct location for that, so the reference ends up being `\mscorlib.dll` (i.e., it looks on the root of the hard drive).

Second we need an `ItemGroup` containing this:

```xml
<PackageReference Include="Microsoft.NETCore.UniversalWindowsPlatform"
                  Version="6.2.12" />
```

The ensures provides references to the .NET runtime library components. (So this successfully achieved what the SDK would unsuccessfully have tried if we hadn't set the `NoStdLib` property).

So this enables normal .NET code to compile. However, Rx.NET also includes code that uses some UWP-specific APIs. (After all, a large part of the issue we're dealing with here exists because of features like schedulers that support UWP dispatchers.) And for that to work, the compiler needs access to `.winmd` files with the metadata for these APIs. So we have this:

```xml
<ReferencePath Include="$(TargetPlatformSdkPath)UnionMetadata\10.0.19041.0\Windows.winmd" />
```

This relies on the `TargetPlatformSdkPath` variable being set. TODO: at one point I was having to set this by hand because the target in `Microsoft.Common.CurrentVersion.targets` that's supposed to set it wasn't working. But now it seems to work. My guess is that it wasn't working due to nonsensical platform or target identifiers, which are now OK.

You might be wondering about that 19041 in there. Why is that not 18362, consistent with the TFM? One reason is that as of 2024/10/24, Azure DevOps Windows build agents have only these Windows SDK versions installed:

* 10.0.17763.0
* 10.0.19041.0
* 10.0.20348.0
* 10.0.22000.0
* 10.0.22621.0

Specifying 18362 would be a problem because we'd somehow need to install it each time we built. In practice you don't need an exact match: the 19041 SDK is capable of building applications that target 18362. So 10.0.19041.0 makes sense here as the first available version after 18362. If at some point anything in the build required a later SDK, we would refer to that instead of 19041.



## Consequences

We can now build using `dotnet build`. It used to be necessary to use `msbuild` to build from the command line.