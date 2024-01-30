# Windows TFMs and Desktop Framework Dependencies

When a .NET project that targets Windows takes a dependency on `System.Reactive`, there are circumstances in which this causes unwanted dependencies on the WPF and Windows Forms frameworks. This can add tens of megabytes to the deployable size of applications. It has caused some projects to abandon Rx entirely.

For example, [Avalonia removed all use of Rx.NET](https://github.com/AvaloniaUI/Avalonia/pull/9749) in January 2023. In the discussion of https://github.com/dotnet/reactive/issues/1461 you'll see some people talking about not being able to use Rx because of this problem.

The view of the Rx .NET maintainers is that projects using Rx should not be forced into this situation. There are a lot of subtleties and technical complexity here, but the bottom line is that we want Rx to be an attractive choice.

Since this topic was first raised, we have discovered that there is a [workaround](#the-workaround) to the problem. Back when [endjin](https://endjin.com) took over maintenance and development of Rx .NET at the start of 2023, it was believed that there was no workaround, so our plan was that Rx 7.0 would address this problem, possibly making radical changes (e.g., introducing a new 'main' Rx package, with `System.Reactive` being the sad casualty of an unfortunate technical decision made half a decade ago). But now that a workaround has been identified, the pressure to make changes soon has been removed. It seems that Rx 6.0 can be used in a way that doesn't encounter these problems, so we now think that a less radical, more gradual longer-term plan is a better bet. We can deprecate the parts of the library that caused this problem and introduce replacements in other components, with a long term plan of eventually removing them from `System.Reactive`, at which point the workaround would no longer be required. The process of deprecation could begin now, but it would likely be many years before we reach the ends state.

This document explains the root causes of the problem, the current [workaround](#the-workaround), the eventual desired state of Rx .NET, and the path that will get us there.


## Status

Draft


## Authors

@idg10 ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/)).


## Context

To decide on a good solution, we need to take a lot of information into account. It is first necessary to characterise [the problem](#the-problem) clearly. It is also necessary to understand [the history that led up to the problem](#the-road-to-the-current-problem). The proposed [workaround](#the-workaround) needs to be understood in detail. We [started a public discussion](https://github.com/dotnet/reactive/discussions/2038) of this problem, and have received a great deal of [useful input from the Rx.NET community](#community-input). There are [several ways we could try to solve this](#the-design-options), and them must each be evaluated in the light of all the other information.

The following sections address all of this before moving onto a [decision](#decision).

### The problem

The basic problem is described at the start of this document, but we can characterise it more precisely:

> An application that references `System.Reactive` (directly or transitively) and which has a Windows-specific target specifying a version of `10.0.19041` will acquire a dependency on the [.NET Windows Desktop Runtime](https://github.com/dotnet/windowsdesktop). The [`UseWPF`](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props-desktop#usewpf) and [`UseWindowsForms`](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props-desktop#usewindowsforms) properties will have been set to `true`

That "or transitively" is important but easily overlooked. Some developers have found themselves encountering this problem not because their applications use `System.Reactive` directly, but because they are using some library that depends on it.


### The road to the current problem


1. the long history of confusion in Rx's package structure before Rx 4.0
2. the subtle problems that could occur when plug-ins use Rx
3. the [_great unification_](https://github.com/dotnet/reactive/issues/199) in Rx 4.0 that solved the first two problems
4. the new problem caused by the _great unification_: as described above, an .NET application that runs on Windows might get tens of megabytes larger as a result of adding a reference to `System.Reactive`

#### Rx's history of confusing packaging

The first public previews of Rx appeared before NuGet was a thing. So it was initially distributed in the old-fashioned way: you installed an SDK on development machines that made Rx's assemblies available for local development, and had to arrange to copy the necessary files onto target machines as part of your installation process. By the time the first supported Rx release shipped, NuGet did exist, but it was early days, so for quite a while Rx was available both via NuGet and through an installable SDK.

There were several different versions of .NET around at this time. Silverlight and Windows Phone both had their own runtimes, and a version of Rx was actually preinstalled on the latter. Windows 8 had its own version of .NET that worked quite differently from anything else. These all had very different subsections of the .NET runtime class libraries, especially when it came to threading support. Rx was slightly different on each of these platforms because attempting to target the lowest common denominator would have meant sub-optimal performance. The scheduler support was specialized to work as well as possible on each distinct target.

This was years before .NET Standard was introduced, and at the time, if you wanted to write cross-platform libraries, you had to create something called a Portable Class Library (PCL). Rx wanted to offer a common API across all platforms while also providing optimized platform-specific schedulers, so it introduced a platform abstraction layer and a system it called "enlightenments" (named after a similar feature in Virtual Machine architectures). This worked, but resulted in a somewhat confusing proliferation of DLLs. Understanding which component your applications or libraries should reference in order to use Rx, and understanding which particular DLLs needed to be deployed was not easy, and was something of a barrier to adoption for new users.

With Rx 3.0, things got a little simpler, with NuGet metapackages providing you with a single package you could reference for basic Rx usage, and packages appropriate for using specific UI frameworks with Rx. However, this led to a new problem.

#### Plug-in problems

Because Rx has always supported many different runtimes, each component came in several forms. At one point, there were different copies of Rx for different versions of .NET Framework: there was one targetting .NET Framework 4.0, and another targetting .NET Framework 4.5. NuGet can cope with this—you just end up with `net40` and `net45` subfolders under `lib`. And the idea is that the .NET SDK will work out which one to use based on the runtime you target.

However there was a problem with plug-in systems. People ran into this in practice a few times writing extensions for Visual Studio. If one plug-in was written to use Rx.NET and if that plug-in was compiled for .NET Framework 4.0, deploying that plug-in would entail providing a copy of the `net40` `System.Reactive.dll` file. If another plug-in was also written to use the same version of Rx.NET but was compiled for .NET Framework 4.5, its deployment files would include the `net45` copy of `System.Reactive.dll`. Visual Studio is capable of loading components compiled for older versions of .NET Framework, so it would happily load either of these. But if it ended up loading both, that would mean that each plug-in was trying to supply its own `System.Reactive.dll`. The first one to load would be able to use its copy, but when the second one tried to load, the .NET assembly resolver would notice that it was asking for a version of `System.Reactive.dll` that was already loaded. (The `net40` and `net45` builds both had the same version number.) So the second component would end up getting the `net40` version, and not the `net45` version it shipped. This would result in `MissingMethodException` failures if that second component tried to use features that were present in the `net45` but not the `net40` build.

Rx 3.0 attempted to solve this by using [slightly different version numbers for the same 'logical' component on each supported target](https://github.com/dotnet/reactive/issues/205). But this went on to cause [various new issues](https://github.com/dotnet/reactive/issues/199#issuecomment-266138120).


#### Rx 4.0's great unification

Rx 4.0 tried a different approach: have a single Rx package, `System.Reactive`.


In .NET, components and applications indicate the environments they can run on with a Target Framework Moniker (TFM). These can be very broad. A component with a TFM of `netstandard2.0` can run on any .NET runtime that supports .NET Standard 2.0 (e.g., .NET 8.0, or .NET Framework 4.7.2), and does not care which operating system it runs on. But TFMs can be a good deal more specific. If a component has a TFM of `net6.0-windows10.0.19041`, it requires .NET 6.0 or later (so it won't run on any version of .NET Framework) and will run only on Windows. Moreover, and it has indicated a particular Windows API surface area that it was built for. That `10.0.19041` is an SDK version number but it corresponds to the May 2020 update to Windows 10 (also known as version 2004, or 20H1).

The `System.Reactive` is a multi-target NuGet package. If you download the v6.0 package and unzip it (`.nupkg` files are just ZIP files) you will find the `lib` folder contains subfolders for 5 different TFMs: `net472`, `net6.0`, `net6.0-windows10.0.19041`, `netstandard2.0`, and `uap10.0.18362`. Each contains a `System.Reactive.dll` file, and each is slightly different. The `netstandard2.0` one is effectively a lowest common denominator, and it is missing some types you will find in the more specialized versions. For example, the version in `net472` includes `ControlScheduler`, a type that provides integration between Rx and the Windows Forms desktop client framework. Windows Forms is built into .NET Framework—it's not possible to install .NET Framework without Windows Forms—and so it's possible for the `net472` version of Rx to include that type. But `netstandard2.0` does not include Windows Forms—that version of Rx may find itself running on Linux, where Windows Forms definitely won't be available.

This design was introduced in Rx 4.0. Before that, Rx.NET  was split across multiple NuGet packages, and this caused a certain amount of confusion. The goal of the _great unification_ that happened with Rx 4.0 was that there would be just one NuGet package for Rx. If you reference that package, you get everything NuGet has to offer on whatever platform you are running on. So if you're using .NET Framework, you get Rx's WPF and Windows Forms features because WPF and Windows Forms are built into .NET Framework. If you're writing a UWP application and you add a reference to `System.Reactive`, you get the UWP features of Rx.


#### Problems arising from the great unification

This worked fine until .NET Core 3.1 came out. That threw a spanner in the works, because it undermined a basic assumption that the _great unification_ made: the assumption that your target runtime would determine what UI application frameworks were available. Before .NET Core 3.1, the availability of a UI framework was determined entirely by which runtime you were using. If you were on .NET Framework, both WPF and Windows Forms would be available, and if you were running on any other .NET runtime, they would be unavailable. If you were running on the oddball version of .NET available on UWP (which, confusingly, is associated with TFMs starting with `uap`) the only UI framework available would be the UWP one, and that wasn't available on any other runtime.

But .NET Core 3.1 ended that simple relationship. The answer to the question "Which UI frameworks are available if I run on .NET Core 3.1?" the answer is, unfortunately, "It depends."


### The workaround

If your application has encountered [the problem](#the-problem), you add this to the `csproj`:

```xml
<PropertyGroup>
  <DisableTransitiveFrameworkReferences>true</DisableTransitiveFrameworkReferences>
</PropertyGroup>
```

This needs to go just the project that builds your application's executable output. It does not need to go in every project—if you've split code across multiple libraries, those don't need to have this. The problem afflicts only executables, not DLLs.

Why not just set [`UseWPF`](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props-desktop#usewpf) and [`UseWindowsForms`](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props-desktop#usewindowsforms) back to `false`? That might work in a simple single-project setup, but there are cases involving libraries where it does not. For example, consider this dependency chain:

* MyApp (with no direct `System.Reactive` dependency)
  * SomeThirdPartyLib
    * `System.Reactive`

If `SomeThirdPartyLib` has target that's subject to this problem (e.g., it targets `net80-windows10.0.19041`), and if it did not also set `UseWPF` and `UseWindowsForms` to `false`, then that library will have a dependency on `Microsoft.WindowsDesktop.App`

### Community input


https://github.com/dotnet/reactive/discussions/2038

https://github.com/AvaloniaUI/Avalonia/issues/9549


Is size that big a deal? Ani Betts wanted to dismiss this whole topic on the grounds that in this day and age, 50MB is really nothing. But the reality is that people voted with their feet. This has been a big deal for some people, and we need to take it seriously.


#### The peculiar faith in the power of breaking changes


#### Exploiting radical change as an opportunity

If radical change is unavoidable (and that's a big "if") there is a view that it presents an opportunity to achieve things that would normally be impossible. This is essentially the 'never let a good crisis go to waste' mindset, a notable example being Chris Pulman's [proposal for a greater separation of concerns in Rx.NET](https://github.com/dotnet/reactive/discussions/2038#discussioncomment-7559424).


#### Windows version numbers


### Constraints

Our goal is that upgrading from Rx 6.0 to Rx 7.0 should not be a breaking change. The rules of semantic versioning do in fact permit breaking changes, but because Rx.NET defines types in `System.*` namespaces, and because a lot of people don't seem to have realised that it has not been a Microsoft-supported project for many years now, people have very high expectations about backwards compatibility.

These expectations are not unreasonable because Rx.NET has been positioned as a core, flexible piece of technology. Its widespread use has been strongly encouraged, and as its new(ish) maintainers, we at [endjin](https://endjin.com) continue to encourage this. By doing so we are effectively setting expectations around backwards compatibility similar to those that could reasonably apply to types in `Microsoft.Extensions.*` namespaces, and perhaps even to types in the .NET runtime libraries themselves.

This goal creates some constraints.

#### Can't remove types until a long Obsolete period

The simplest thing we could do to solve the main problem this document describes would be to remove all UI-framework-specific types from the public API surface area of `System.Reactive`. This would entail simply removing the `net6.0-windows10.0.19041`, `net472`, and `uap10.0.18362` targets. Applications using .NET 6.0 or later would get the `net6.0` target, and everything else would use the `netstandard2.0` target. The UI-framework-specific types could be moved into UI-specific NuGet packages, so applications would not simply be left in the lurch: all functionality would remain available, it would simply be distributed slightly differently.

Unfortunately, this would create some serious new problems. Consider an application that depends on two libraries that use different versions of Rx. Let's suppose LibraryBefore depends on Rx 6.0, and LibraryAfter depends on some hypothetical future Rx 7.0 that makes the change just described. So we have this sort of dependency tree:

* `MyApp`
  * `LibraryBefore`
    * `System.Reactive` 6.0
  * `LibraryAfter`
    * `System.Reactive` 7.0 (a hypothetical version with no UI-framework-specific features)
    * `System.Reactive.Wpf` 7.0 (a hypothetical new library containing the WPF-specific features that are currently baked into System.Reactive 6.0)

Suppose `LibraryBefore` is using some WPF-specific feature in Rx 6.0—let's say it calls the [`ObserveOnDispatcher` extension method](subscribeon-and-observeon-in-ui-applications). Since it depends on Rx 6.0, it's going to require that method, and its containing `DispatcherObservable` type, to be in `System.Reactive`.

To fully understand why this creates a problem you need to think about what actually gets compiled into components. Here's how the use of `ObserveOnDispatcher` looks in the IL emitted for a library depending on Rx 6.0:

```cil
call class [System.Runtime]System.IObservable`1<!!0> [System.Reactive]System.Reactive.Linq.DispatcherObservable::ObserveOnDispatcher<int32>(class [System.Runtime]System.IObservable`1<!!0>)
```

If you're not familiar with .NET's IL, I'll just break that down for you. the `call class` part indicates that we're calling a method defined by a class. The `call` instruction needs to identify a specific method. The raw binary for the IL does this with a metadata token—essentially a reference to a particular row in a table of methods. Compiled .NET components contain what is essentially a small, highly specialized relational database, and one of the tables is a list of every single method used by the component. A `call` instruction incorporates a number that's effectively an offset into that table. (I've left out a complication caused by the distinction between internally defined methods and imported ones, but that's more detail than is necessary here.)

The IL shown above is how ILDASM, the IL disassembler, interprets it for us. Instead of just showing us the metadata token, it goes and finds the relevant row in the table. In fact it finds a bunch of related rows—there's a table for parameters, and it also has to go and find all of the rows corresponding to the various types referred to: in this case there's the return type, the type of the one and only normal parameter, and also a type argument because this is a generic method.

In fact there's only one really important part in that IL, which I'll call out here:

```
[System.Reactive]System.Reactive.Linq.DispatcherObservable::ObserveOnDispatcher<int32>
```

This essentially says that the method we want is:

1. defined in the `System.Reactive` assembly
2. defined in the `System.Reactive.Linq.DispatcherObservable` class in that assembly
3. called `ObserveOnDispatcher`
4. a generic method with one type parameter, and we want to use `int32` (what C# calls `int`) as the argument to that type

It's point 1 that matters here. This indicates that the method is defined in `System.Reactive`. That's what's going to cause us problems in this scenario. But why?

With that in mind, let's get back to our example. We've established that `LibraryBefore` is going to contain at least one IL `call` instruction that indicates that it expects to find the `ObserveOnDispatcher` method in the `System.Reactive` assembly.

What's `LibraryAfter` going to look like? Remember in this hypothetical scenario, Rx 7.0 has moved all WPF-specific types out of `System.Reactive` and into some new component we're calling `System.Reactive.Wpf` in this example. So code in `LibraryAfter` calling the exact same method (the `DispatcherObservable` class's `ObserveOnDispatcher` extension method) would look like this in IL:

```cil
call class [System.Runtime]System.IObservable`1<!!0> [System.Reactive.Wpf]System.Reactive.Linq.DispatcherObservable::ObserveOnDispatcher<int32>(class [System.Runtime]System.IObservable`1<!!0>)
```

And again, I'll single out the one bit that matters:

```
[System.Reactive.Wpf]System.Reactive.Linq.DispatcherObservable::ObserveOnDispatcher<int32>
```

This is almost the same as for `LibraryBefore`, with one critical change. We saw from point 1 in the preceding list that `LibraryBefore` says the method it wants is defined in the `System.Reactive` assembly. But `LibraryAfter` is looking for it in `System.Reactive.Wpf`.

So what?

Well, when an application uses two libraries that use two different versions of the same NuGet package, the .NET SDK _unifies_ the reference. In this case, both `LibraryBefore` and `LibraryAfter` use the `System.Reactive` NuGet package, but one wants v6.0.0 and the other wants some hypothetical future v7.0.0.

Unification means that the .NET SDK picks exactly one version of each NuGet package. And the default is that the highest minimum requirement wins. (It's possible for `LibraryBefore` to impose an upper bound: it might state its version requirements as `>= 6.0.0` and `< 7.0.0`. In that case, this would cause a build failure because there's an unresolvable conflict. But most packages specify only a lower bound. When you add a dependency to `System.Reactive` 6.0.0, the .NET SDK interprets that as `>= 6.0.0` unless you say otherwise.)

So `MyApp` is going to get `System.Reactive` 7.0.0. That's the version that will actually be loaded into memory when the application runs.

What does that mean for the `LibraryBefore`? Well if it happens never to run the line of code that invokes `ObserveOnDispatcher`, there won't be a problem. But if it does, we'll get an exception when the CLR attempts to JIT compile the code that invokes that method. It will look at the IL and determine that the method the code wants to invoke is, as we saw earlier:

```
[System.Reactive]System.Reactive.Linq.DispatcherObservable::ObserveOnDispatcher<int32>
```

The JIT compiler will then inspect the `System.Reactive` assembly and discover that it does not define a type called `System.Reactive.Linq.DispatcherObservable`. The JIT compiler will then throw an exception to report that the IL refers to a method that does not in fact exist.

And that's why we can't just remove types from `System.Reactive`.

What we can do is replace them with a type forwarder. If we want to move `ObserveOnDispatcher` out of `System.Reactive` and into `System.Reactive.Wpf`, we can do that in a backwards compatible way by adding a type forwarding entry for the `System.Reactive.Linq.DispatcherObservable`. Basically `System.Reactive` contains a note telling the CLR "I know you've been told that this type is in this assembly, but it's actually in `System.Reactive.Wpf`, so please look there instead."

Doesn't a type forwarder solve the problem? Not really, because if the `System.Reactive` assembly contains type forwarders to some proposed `System.Reactive.Wpf` assembly, the .NET SDK will require the resulting `System.Reactive` NuGet package to have a dependency on `System.Reactive.Wpf`.

And that gets us back to square one: if taking a dependency on `System.Reactive` causes you to acquire a transitive dependency on `System.Reactive.Wpf`, that means using Rx automatically opts you into use WPF whether you want it or not.

It would be technically possible to meddle with the build system's normal behaviour in order to produce a `System.Reactive` assembly with a suitable type forwarder, but for the resulting NuGet package not to have the corresponding dependency. However, this is unsupported, and is likely to cause a lot of confusion for people who actually do want the WPF functionality, because adding a reference to just `System.Reactive` (which has been all that has been required for Rx v4 through v6) would still enable code using WPF features to compile when upgrading to this hypothetical form of v7, but it would result in runtime errors due to the `System.Reactive.Wpf` assembly not being found. So this is not an acceptable workaround.

#### ...except for UWP

We are considering making an exception to the constraint just discussed for UWP. The presence of UWP code causes considerable headaches because UWP is not a properly supported target. The modern .NET SDK build system doesn't fully recognize it, and we end up using the [`MSBuild.Sdk.Extras`](https://github.com/novotnyllc/MSBuildSdkExtras) package to work around this. That repository hasn't had an update since 2021, and it was originally written in the hope of being a stopgap while Microsoft got proper UWP support in place. Proper UWP support never arrived, mainly because UWP is a technology Microsoft has long been telling people not to use.

We don't want to drop UWP support completely, but we are prepared to contemplate removing the UWP-specific target (`uap10.0.16299`). UWP has long supported .NET Standard 2.0, so Rx.NET would still be available. However, the UWP-specific types would no longer be in `System.Reactive`. (We would move them into a separate NuGet package.)

This is problematic for all of the reasons just discussed in the preceding section. However, as far as we know UWP never really became hugely popular, and the fact that Microsoft never added proper support for it to the .NET SDK sets a precedent that makes us comfortable with dropping it relatively abruptly. Existing Rx.NET users using UWP will have two choices: 1) remain on Rx 6.0, or 2) rebuild code that was using UWP-specific types in `System.Reactive` to use the new UWP-specific package we would be adding.


### The design options

The following sections describe the design choices that have been considered to date.

#### Option 1: change nothing

The status quo is always an option. It's the default, but it can also be a deliberate choice. The availability of a [workaround](#the-workaround)

#### Option 2: new main Rx package, demoting `System.Reactive` to a facade

#### Option 3: `System.Reactive` remains the primary package and becomes a facade

We could maintain `System.Reactive` as the face of Rx, but turn it into a facade, with all the real bits being elsewhere. This would give people the option to depend on, say, `System.Reactive.Common` or whatever, to be sure of avoiding any UI dependencies. However, this might not help with transitive dependencies.


#### Option 4: UI-framework specific packages, deprecating their


### Other options less seriously considered

### Change everything

We could do something similar to what the Azure SDK team did a few years back: they introduced a new set of libraries under a completely different set of namespaces. There was no attempt at or pretense of continuity. New NuGet packages were released, and the old ones they replaced have gradually been deprecated.

You could argue that we've already done this. There's a whole new version of Rx at https://github.com/reaqtive/reaqtor that implements functionality not available in `System.Reactive`. (Most notably the ability to persist a subscription. In the ['reaqtive' implementation of Rx](https://reaqtive.net), operators that accumulate state over time, such as [`Aggregate`](https://introtorx.com/chapters/aggregation#aggregate), can migrate across machines, and be checkpointed, enabling reliable, persistent Rx subscriptions to run over the long term, potentially even for years.) The NuGet package names and the namespaces are completely different. There's no attempt to create any continuity here.

An upshot of this is that there is no straightforward way to migrate from `System.Reactive` to the [reaqtive Rx](https://reaqtive.net). (The Azure SDK revamp has the same characteristic. You can't just change your NuGet package references: you need to change your code to use the newer libraries, because lots of things are just different.)

Our view is that we don't want three versions of Rx. The split between `System.Reactive` and [reaqtive Rx](https://reaqtive.net) was essentially a _fait acompli_ by the time the latter was open sourced. And the use cases in which the latter's distinctive features are helpful are sufficiently specialized that in most cases it probably wouldn't make sense to try to migrate from one to the other. But to create yet another public API surface area for Rx in .NET would cause confusion. We don't think it offers enough benefit to offset that.


Another idea: could we introduce a later Windows-specific TFM, so that use of windows10.0.19041 becomes a sort of dead end?


## Decision

## Consequences
