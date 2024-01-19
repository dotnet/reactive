# Windows TFMs and Desktop Framework Dependencies

When a .NET project that targets Windows takes a dependency on `System.Reactive`, there are circumstances in which this causes unwanted dependencies on the WPF and Windows Forms frameworks. This can add tens of megabytes to the deployable size of applications. It has caused some projects to abandon Rx entirely.

The view of the Rx .NET maintainers is that projects using Rx should not be forced into this situation. There are a lot of subtleties and technical complexity here, but the bottom line is that we want Rx to be an attractive choice.

Since this topic was first raised, we have discovered that there is a workaround to the problem. It is not necessary to change Rx in order to avoid the problem. Back when endjin took over maintenance and development of Rx .NET at the start of 2023, it was believed that there was no workaround, so our plan was that Rx 7.0 would address this problem, possibly making radical changes (e.g., introducing a new 'main' Rx package, with `System.Reactive` being the sad casualty of an unfortunate technical decision made half a decade ago). But now that a workaround has been identified, the pressure to make changes soon has been removed—Rx 6.0 can be used in a way that doesn't encounter these problems. So we now think that a better bet is to have a longer-term plan in which we can deprecate the parts of the library that caused this problem and introduce replacements in other components, with a long term plan of eventually removing them from `System.Reactive`, at which point the workaround would no longer be required. The process of deprecation could begin now, but it would likely be many years before we reach the ends state.

This document explains the root causes of the problem, the current workaround, the eventual desired state of Rx .NET, and the path that will get us there.


## Status

Draft


## Authors

@idg10 ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/)).


## Context

There are a few things we need to take into account:

1. the long history of confusion in Rx's package structure before Rx 4.0
2. the subtle problems that could occur when plug-ins use Rx
3. the [_great unification_](https://github.com/dotnet/reactive/issues/199) in Rx 4.0 that solved the first two problems
4. the new problem caused by the _great unification_: as described above, an .NET application that runs on Windows might get tens of megabytes larger as a result of adding a reference to `System.Reactive`

### Rx's history of confusing packaging

The first public previews of Rx appeared before NuGet was a thing. So it was initially distributed in the old-fashioned way: you installed an SDK on development machines that made Rx's assemblies available for local development, and had to arrange to copy the necessary files onto target machines as part of your installation process. By the time the first supported Rx release shipped, NuGet did exist, but it was early days, so for quite a while Rx was available both via NuGet and through an installable SDK.

There were several different versions of .NET around at this time. Silverlight and Windows Phone both had their own runtimes, and a version of Rx was actually preinstalled on the latter. Windows 8 had its own version of .NET that worked quite differently from anything else. These all had very different subsections of the .NET runtime class libraries, especially when it came to threading support. Rx was slightly different on each of these platforms because attempting to target the lowest common denominator would have meant sub-optimal performance. The scheduler support was specialized to work as well as possible on each distinct target.

This was years before .NET Standard was introduced, and at the time, if you wanted to write cross-platform libraries, you had to create something called a Portable Class Library (PCL). Rx wanted to offer a common API across all platforms while also providing optimized platform-specific schedulers, so it introduced a platform abstraction layer and a system it called "enlightenments" (named after a similar feature in Virtual Machine architectures). This worked, but resulted in a somewhat confusing proliferation of DLLs. Understanding which component your applications or libraries should reference in order to use Rx, and understanding which particular DLLs needed to be deployed was not easy, and was something of a barrier to adoption for new users.

With Rx 3.0, things got a little simpler, with NuGet metapackages providing you with a single package you could reference for basic Rx usage, and packages appropriate for using specific UI frameworks with Rx. However, this led to a new problem.

### Plug-in problems

Because Rx has always supported many different runtimes, each component came in several forms. At one point, there were different copies of Rx for different versions of .NET Framework: there was one targetting .NET Framework 4.0, and another targetting .NET Framework 4.5. NuGet can cope with this—you just end up with `net40` and `net45` subfolders under `lib`. And the idea is that the .NET SDK will work out which one to use based on the runtime you target.

However there was a problem with plug-in systems. People ran into this in practice a few times writing extensions for Visual Studio. If one plug-in was written to use Rx.NET and if that plug-in was compiled for .NET Framework 4.0, deploying that plug-in would entail providing a copy of the `net40` `System.Reactive.dll` file. If another plug-in was also written to use the same version of Rx.NET but was compiled for .NET Framework 4.5, its deployment files would include the `net45` copy of `System.Reactive.dll`. Visual Studio is capable of loading components compiled for older versions of .NET Framework, so it would happily load either of these. But if it ended up loading both, that would mean that each plug-in was trying to supply its own `System.Reactive.dll`. The first one to load would be able to use its copy, but when the second one tried to load, the .NET assembly resolver would notice that it was asking for a version of `System.Reactive.dll` that was already loaded. (The `net40` and `net45` builds both had the same version number.) So the second component would end up getting the `net40` version, and not the `net45` version it shipped. This would result in `MissingMethodException` failures if that second component tried to use features that were present in the `net45` but not the `net40` build.

Rx 3.0 attempted to solve this by using [slightly different version numbers for the same 'logical' component on each supported target](https://github.com/dotnet/reactive/issues/205). But this went on to cause [various new issues](https://github.com/dotnet/reactive/issues/199#issuecomment-266138120).


### Rx 4.0's great unification

Rx 4.0 tried a different approach: have a single Rx package, `System.Reactive`.


In .NET, components and applications indicate the environments they can run on with a Target Framework Moniker (TFM). These can be very broad. A component with a TFM of `netstandard2.0` can run on any .NET runtime that supports .NET Standard 2.0 (e.g., .NET 8.0, or .NET Framework 4.7.2), and does not care which operating system it runs on. But TFMs can be a good deal more specific. If a component has a TFM of `net6.0-windows10.0.19041`, it requires .NET 6.0 or later (so it won't run on any version of .NET Framework) and will run only on Windows. Moreover, and it has indicated a particular Windows API surface area that it was built for. That `10.0.19041` is an SDK version number but it corresponds to the May 2020 update to Windows 10 (also known as version 2004, or 20H1).

The `System.Reactive` is a multi-target NuGet package. If you download the v6.0 package and unzip it (`.nupkg` files are just ZIP files) you will find the `lib` folder contains subfolders for 5 different TFMs: `net472`, `net6.0`, `net6.0-windows10.0.19041`, `netstandard2.0`, and `uap10.0.18362`. Each contains a `System.Reactive.dll` file, and each is slightly different. The `netstandard2.0` one is effectively a lowest common denominator, and it is missing some types you will find in the more specialized versions. For example, the version in `net472` includes `ControlScheduler`, a type that provides integration between Rx and the Windows Forms desktop client framework. Windows Forms is built into .NET Framework—it's not possible to install .NET Framework without Windows Forms—and so it's possible for the `net472` version of Rx to include that type. But `netstandard2.0` does not include Windows Forms—that version of Rx may find itself running on Linux, where Windows Forms definitely won't be available.

This design was introduced in Rx 4.0. Before that, Rx.NET  was split across multiple NuGet packages, and this caused a certain amount of confusion. The goal of the _great unification_ that happened with Rx 4.0 was that there would be just one NuGet package for Rx. If you reference that package, you get everything NuGet has to offer on whatever platform you are running on. So if you're using .NET Framework, you get Rx's WPF and Windows Forms features because WPF and Windows Forms are built into .NET Framework. If you're writing a UWP application and you add a reference to `System.Reactive`, you get the UWP features of Rx.

This worked fine until .NET Core 3.1 came out. That threw a spanner in the works, because it undermined a basic assumption that the _great unification_ made: the assumption that your target runtime would determine what UI application frameworks were available. Before .NET Core 3.1, the availability of a UI framework was determined entirely by which runtime you were using. If you were on .NET Framework, both WPF and Windows Forms would be available, and if you were running on any other .NET runtime, they would be unavailable. If you were running on the oddball version of .NET available on UWP (which, confusingly, is associated with TFMs starting with `uap`) the only UI framework available would be the UWP one, and that wasn't available on any other runtime.

But .NET Core 3.1 ended that simple relationship. The answer to the question "Which UI frameworks are available if I run on .NET Core 3.1?" the answer is, unfortunately, "It depends."



https://github.com/dotnet/reactive/discussions/2038

https://github.com/AvaloniaUI/Avalonia/issues/9549


Is size that big a deal? Ani Betts wanted to dismiss this whole topic on the grounds that in this day and age, 50MB is really nothing. But the reality is that people voted with their feet. This has been a big deal for some people, and we need to take it seriously.


## Decision

## Consequences
