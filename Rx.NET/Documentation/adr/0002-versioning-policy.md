# Rx.NET post-v5.0 version policy

At the time of writing this (early 2023), the latest available version of `System.Reactive` is [v5.0.0](https://www.nuget.org/packages/System.Reactive/5.0.0) published 2020/11/10. This ADR describes the policy for subsequent versions.

## Status

Accepted


## Authors

@idg10 ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/)).


## Context

For most of Rx's history, its version numbers represented changes in the library. Rx 3.0 was the first to support .NET Core (v1.0) and .NET Standard. Rx 4.0 got a major version bump because it introduced a major restructuring of the NuGet packages. [Rx 5.0](https://github.com/dotnet/reactive/releases/tag/rxnet-v5.0.0) took advantage of Nullable Reference types, and added `.netcore3.1` as a target (since Nullable Reference Types weren't available in any of the older targets Rx previously supported).

This particular release also declared itself to be "part of the .NET 5.0 release wave." It's true that it was released to coincide with the release of .NET 5.0, but it was a complete coincidence that Rx's own version number happened to hit 5.0 at the point. And this has created a somewhat unfortunate perception that Rx's version numbers should continue to follow those of .NET, even though there's no good technical reason to do that.

It's important to understand that there's a distinction between the _targets_ that a NuGet package lists, and the frameworks on which it is supported. The most obvious example of that is a library that targets one of the .NET Standard TFMs (Target Framework Monikers) such as `netstandard2.0`. There is no version of .NET called ".NET Standard 2.0". There are instead multiple .NET runtimes all of which are able to use libraries that target `netstandard2.0`.

A more subtle point is that when a TFM _does_ align with a particular implementation of .NET (e.g., the `netcoreapp3.1` moniker is associated with .NET Core 3.1), there are also multiple .NET runtimes able to use such a library. And this is a point that causes confusion. The [.NET Core 3.1 runtime went out of support on December 13th 2020](https://learn.microsoft.com/en-us/lifecycle/products/microsoft-net-and-net-core). So when people see a library on NuGet that targets `netcoreapp3.1`, the temptation is to think that the library is now out of date. But this is not necessarily so: at the time of writing this (early 2023) both .NET 6.0 and .NET 7.0 are currently in support, and both of those support loading libraries with a TFM of `netcoreapp3.1`. So `netcoreapp3.1` is a perfectly valid target for a library today, and does not necessarily mean the library is out of date. It would be a slightly curious choice of TFM for a brand new library, but a valid one nonetheless. (And some developers find themselves in a situation where they are obliged to keep old systems running even when they are technically out of support, so there are sometimes good reasons for a library to use these older TFMs.)

The combined effect of Rx being on version 5.0, and its .NET-version-specific TFMs (`netcoreapp3.1`, `net5.0` and `net5.0-windows10.0.19041`) all apparently referring to versions of .NET that are now out of support, contribute to the perception that Rx is somehow 'behind'. People incorrectly conclude that Rx won't work on .NET 6.0 or .NET 7.0. In fact Rx 5.0 works just fine on these runtimes.

One viable option is to just accept that a lot of developers don't have a sufficiently deep understanding of the distinction between runtime versions, TFMs, and NuGet package version numbers, or the compatibility rules for TFMs, and to bump your version number every time a new version of .NET comes out, even if there's absolutely no technical justification for this. There is something to be said for this approach: it might be a more effective way of satisfying developers who complain than trying to explain the relevant facts. But this is essentially using a technical mechanism to address a psycho-social issue (bumping the version number purely to stop people from complaining) which is at best an unsatisfying tactic. Moreover, this approach has a downside: an unnecessary proliferation of versions increases the chances of version conflicts in complex dependency trees. (It's notable that even the `Microsoft.Extensions.*` packages have stepped back slightly from strict policy of following .NET version numbers. With .NET Core 3.1, a complete new set of those components was released for every _patch_-level update. So you find 3.1.0, 3.1.1, 3.1.2, 3.1.3, and so on for every package in the `Microsoft.Extensions.*` world, even though the overwhelming majority of those versions are identical to their predecessors in every respect except for their version number. With the 6.0 and 7.0 versions, new releases only come out when something really changed. So v7.0.1 of some package will definitely be different from v7.0.0.) A policy of just following the runtime's version numbers also makes it much harder to tell when something really changed. Does a bump from v6 to v7 signify a breaking change, as the rules of semantic versioning would suggest? Or was the package merely slavishly following the version numbering of the runtime?

## Decision

Starting with Rx v6.0.0, Rx's version numbers are not coupled to .NET versions. (They might happen to align from time to time, but such coincidences will be of no technical significance.)

Rx v6.0.0 will support .NET 6 and .NET 7. (It might not have any `net7.0` TFMs, but it will be fully tested on and explicitly supported on .NET 7.)

If breaking changes become necessary, Rx's major version number will be bumped according to normal semantic versioning rules. Likewise, if new functionality is added in a backwards compatible way, the minor version number will be bumped. Bug fixes that do not change the API surface area will bump only the third part of the version number, e.g. v6.0.1 would denote a package with the same API surface area as v6.0.0, but with a bug being fixed.

The v6.0.0 release removes TFMs specific to older out-of-support .NET runtimes. Whereas v5.0.0 includes `netcoreapp3.1` and `net5.0` targets, v6.0.0 will not include any .NET-version-specific TFMs older than `net6.0`. We will continue to offer `netstandard2.0`, so it would be technically possible to run Rx 6.0 on some out-of-support runtimes. However, we will only be testing on runtimes that are still in support, meaning we will not in any official way be supporting the use of Rx 6.0 on .NET Core 3.1 or .NET 5.0. This withdrawal of support is effectively a breaking change, which is why we are bumping the major version number.

This removal of testing of out-of-support frameworks is partly driven by the move to bring Rx's code base back into line with current tooling (e.g., see https://github.com/dotnet/reactive/pull/1882). We anticipate that similar changes will be necessary in the future, as runtimes go out of support and the tool chain moves on. So our policy is that if a Rx is released that does not include targets that its predecessor did, we treat this as a breaking change for semantic versioning purposes.

Addition of support for new versions of .NET (whether or not accompanied by the addition of new targets to the NuGet packages) will not necessitate a bump in the major or minor version numbers. Such support will typically be put in place by adding new targets to test projects. If, in the course of testing Rx against a new version of .NET, we discover that it is necessary to make changes, we will bump the patch version. (For example, if Rx v6.0.0 is still the current version of Rx at the time that .NET 8 ships, and we discover that it is necessary to make a change to Rx's internal behaviour to work correctly on .NET 8, we would set the version number to Rx 6.0.1, because we would consider this to be a bug fix.)


## Consequences

The next version of Rx (which we plan to be the first one offering explicit support for .NET 6 and .NET 7) will be v6.0.

If the changes required to address the problems projects such as Avalonia have experienced with Rx (see the *Roadmap and Architectural Decisions* section of https://github.com/dotnet/reactive/discussions/1868) require a breaking change (which seems likely) the version of Rx to address this will be v7.0 (unless something forces us to make another major version bump before addressing that).