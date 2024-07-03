# Apparent mismatches between lib and ref in the `System.Interactive` NuGet package

The `System.Interactive` NuGet package contains the usual `lib` folder, and also `ref`, the standard folder for reference assemblies. But the set of target frameworks in these two folders is, rather unusually, different. This document explains why.

## Status

Fait acomplis.

This dates back at least as far as 2018. The main purpose of this document (written in 2024) is to explain why it's like it is. The original thinking was lost in the mists of time, and it took considerable effort to work out why on earth it's like this. This ADR is intended to save others from the same extensive archaeology. And the discovery is important: if we hadn't managed to reverse engineer the apparent thinking behind this design choice, we might have dismissed it as a mistake. (Indeed, part of its current implementation _is_ a mistake.) But it turns out to serve an important purpose in a non-obvious way.


## Authors

@idg10 ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/)) wrote this document, but he would like to make it clear that he did not make the original design decision in question. He is merely documenting it.


## Context

At the time of writing this, the current version of `System.Interactive` is 6.0.1. If you inspect the NuGet package you will find that the lib and ref folders have different sets of frameworks:

* `lib`
    * `net6.0`
    * `net48`
    * `netstandard2.0`
* `ref`
    * `net4.8`
    * `net6.0`
    * `netstandard2.1`

  
The use of `net4.8` in `ref` seems to have been a bug: that should have been `net48`. (The main reason I am confident it's a bug, and not a clever but obscure trick that we've not understood, is that the [commit of 2021/12/06 that added this](https://github.com/dotnet/reactive/commit/a2410b2267abe193191f3894d243771ae4b126fd) used [`net48` in reference assemblies for one of the other packages](https://github.com/dotnet/reactive/commit/a2410b2267abe193191f3894d243771ae4b126fd#diff-3b568c93a468dab1b1a619a450bf1c4d88d3ec9539737d09fa6fb7659bc0ae5fR7), so this just seems to have been a slip.)

The other discrepancy is that we have `netstandard2.0` in the `lib` folder but `netstandard2.1` in the ref folder. At first glance, this too looks quite a lot like a mistake, particularly when you examine the history. Here is the point in the release history at which the `ref` folder first started having a `netstandard2.1` folder:

* v3.1.1:
    * no reference assemblies
    * folder supports .NET FX 4.5 and .NET Standard 1.0
* v3.2.0
    * reference assemblies for netcoreapp2.0, netstandard1.0, and netstandard2.1
    * lib folder supports .NET FX 4.5 and .NET Standard 1.0, and .NET Standard 2.1

When v3.2.0 shipped in July 2018, the latest .NET Core was  2.1, which had shipped in May 2018. .NET Standard 2.1 was not yet a thing, because that didn't appear until November 2018, when the oldest .NET to support it (.NET Core 3.0) shipped. So on a first inspection, Ix.NET declared reference assemblies for a target framework that simply didn't exist!

And yet, on closer inspection, this appears to be deliberate. Looking at this commit:

  https://github.com/dotnet/reactive/commit/0252fb537c9d335b9bc863b65291f152c07ba385

  we see a [comment in Ix.NET/Source/refs/Directory.build.props](https://github.com/dotnet/reactive/commit/0252fb537c9d335b9bc863b65291f152c07ba385#diff-909504334cbab5c432709c95ae78c24fb2910d850958af2ef6de444b18e5c8ecR6) saying:
  
> This is here so we can create a fake .NET Standard 2.1 facade

I can only guess that they knew .NET Standard 2.1 was coming, and wanted to ensure that `System.Interactive` was ready for it when it shipped.
  
So it was deliberate. But offering reference assemblies for a platform without any corresponding implementation for that platform is an odd choice. (And although at the time this was a placholder for a forthcoming .NET Standard version, it continued to look like this after .NET Standard 2.1 shipped. All subsequent Ix.NET releases have continued to provide `netstandard2.1` in the `ref` folder with no matching folder in `lib`. So it wasn't just a temporary measure.) What purpose does this serve?

Some of the features that Ix offers eventually became available in .NET Core, such as `EnumerableEx.SkipLast`. This method exists in the implementation assemblies for every TFM of Ix.NET, but the `netstandard2.1` and `net6.0` reference assemblies omit it. This has the effect that if you're targetting any version of .NET recent enough to have these methods built into the .NET runtime libraries, the Ix.NET equivalents will:

* appear not to exist at build time
* still be available at runtime

The non-availability at build time is important because these are extension methods. If multiple extension methods of the same name and for same type are available in the same namespace, you get compiler errors when you try to use them in the normal way. (E.g., .NET defines `System.Linq.Enumerable.SkipLast` and Ix.NET defines `System.Linq.EnumerableEx.SkipLast`, and both are defined as extension methods for `IEnumerable<T>`. If it weren't for this NuGet package trickery, any file that has `using System.Linq;` would bring both the Ix and .NET versions into scope, with no straightforward way to indicate which one you want. You can resolve this with some quite obscure use of `extern` aliases, but it's not at all obvious how. If anyone using `System.Interactive` had run into this the moment they upgraded to .NET Core 3.1, that would have been a bad experience.)

By arranging for the `netstandard2.1` and `net6.0` reference assemblies to omit these methods, Ix.NET ensures that the C# compiler has no idea these methods even exist, avoiding the problem. But why do we need reference assemblies for this? Why not just omit the methods from the main assemblies? That's because you might depend on some library, `OldLib`, that was built for `netstandard2.0`, where, say, `SkipLast` is unavailable. `OldLib` might use Ix.NET's `EnumerableEx.SkipLast`, so that method really has to be there at runtime, even if you're running on, say, .NET 8.0. .NET 8.0 provides `Enumerable.SkipLast` but if `OldLib` is only available in `netstandard2.0` form it won't have access to that. It can only use the `Ix.NET` one. So that method has to be there at runtime.

So the basic trick here is that Ix.NET provides one API surface area for backwards compatibility purposes and a slightly smaller API surface that it advertises to new code targeting the latest Ix.NET. The `lib` folder contains complete assemblies providing the former, and the `ref` folder contains reference assemblies providing the latter.

This still doesn't make it obvious why it's useful for `ref` to include `netstandard2.1` when `lib` does not. If I've understood the original design here, the thinking is that on any runtime where `netstandard2.1` is available, methods like `Enumerable.SkipLast` are available in the runtime libraries, so libraries built for `netstandard2.1` should be using that built-in `Enumerable.SkipLast`, and not Ix.NET's `EnumerableEx.SkipLast`. At runtime, `netstandard2.1` libraries with a dependency on `System.Interactive` may well find themselves using the `lib\netstandard2.0` version (because there is no `lib\netstandard2.1` version). E.g.:

| `MyLib` TFM | Ix Ref Assembly TFM (for `MyLib` build) | Runtime | Ix Lib Assembly TFM | `SkipLast` used by `MyLib`
|--|--|--|--|--|
| `netstandard2.0` | `netstandard2.0` | .NET Core 2.1 | `netstandard2.0` | Ix |
| `netstandard2.0` | `netstandard2.0` | .NET 6.0 | `net6.0` | Ix |
| `netstandard2.0` | `netstandard2.0` | .NET Framework | `net48` | Ix |
| `netstandard2.1` | `netstandard2.1` | Mono | `netstandard2.0` | .NET Runtime Libraries |
| `netstandard2.1` | `netstandard2.1` | Unity | `netstandard2.0` | .NET Runtime Libraries |
| `netstandard2.1` | `netstandard2.1` | .NET Core 3.1 | `netstandard2.0` (Ix.NET 6.0.1 has no .NET Core 3.1 version so this is the only available option) | .NET Runtime Libraries |
| `netstandard2.1` | `netstandard2.1` | .NET 6.0 | `net6.0` | .NET Runtime Libraries |
| `netcore3.1` | `netstandard2.1` | .NET Core 3.1 | `netstandard2.0` | .NET Runtime Libraries |
| `netcore3.1` | `netstandard2.1` | .NET 6.0 | `net6.0` | .NET Runtime Libraries |
| `net48` | `net48`* | .NET 4.8 | `net48` | Ix |
| `net6.0` | `net6.0` | .NET 6.0 | `net6.0` | .NET Runtime Libraries |

\* although this table represents the design intent, the fact that the reference assemblies were accidentally given the non-existent TFM `net4.8` instead of `net48` means that things didn't work out as described in that row in practice. That mistake meant that for that row, it instead looked like this:

| `MyLib` TFM | Ix Ref Assembly TFM (for `MyLib` build) | Runtime | Ix Lib Assembly TFM | `SkipLast` used by `MyLib`
|--|--|--|--|--|
| `net48` | `netstandard2.0` (oops) | .NET 4.8 | `net48` | Ix |

The reason this went undetected is that it happened to produce the required outcome: the library uses Ix's `SkipLast`, which is what we need here because .NET 4.8 doesn't have that.

The most interesting row of the first table is the second one: it's the one case where the Ix implementation is used even though the runtime (.NET 6.0) does offer `SkipLast`. It works out this way because the library is built against `netstandard2.0`, so that runtime-supplied `SkipLast` simply isn't part of the available surface area (and in the other two rows where the library was built against `netstandard2.0`, it's not just absent from the compile-time surface area on those rows, it's also absent at runtime, which is exactly why `netstandard2.0` library can't use it). This has to work: any library compiled in a world where the runtime doesn't offer `Enumerable.SkipLast`, and which chose to use Ix's `EnumerableEx.SkipLast`, must continue to work even if loaded into a newer runtime that does offer `Enumerable.SkipLast`. .NET does not provide any method-level equivalent of type forwarding, so there's no way to arrange for the library to get the runtime's `Enumerable.SkipLast` in these situations, so it just has to use the Ix.NET one. That's why when Ix.NET is loaded onto runtimes that offer `Enumerable.SkipLast`, it's necessary for it still to make `EnumerableEx.SkipLast` available in case there are libraries that need it. But it also needs to ensure that this API appears not to exist for any code compiling against a target where `Enumerable.SkipLast` will be available.


## Decision

We will continue to use the same technique, although with two changes:

* we will fix the `net4.8`/`net48` bug
* we will no longer use `MSBuild.Extras.SDK`

This somewhat unusual mismatched `lib` vs `ref` folder setup was previously handled by `MSBuild.Extras.SDK`, but that package is no longer maintained. (It hasn't been updated since .NET 5.0 was the latest runtime.) When we moved to the 8.0 SDK, the reference assembly generation stopped working, so we've had to recreate the same result by supported means. (That is what necessitated the git archaeological dig that led to the writing of this document.)


## Consequences

By continuing the established practice, Ix.NET's `SkipLast` will continue to be available to library authors targeting `netstandard2.0`, and its presence will continue not to cause hard to resolve ambiguous method errors when using newer TFMs. By droping `MSBuild.Extras.SDK`, we'll be able to get things building again on .NET SDK 8.0. Future maintainers will likely continue to be confused by the mismatched `lib` and `ref` folders when they first come across it, but we're hopeful that the existence of this ADR will help.