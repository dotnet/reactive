# Use .NET SDK Package Validation To Ensure Backwards Compatibility

 Migration from PublicApiGenerator to .NET SDK Package Validation


## Status

Undecided


## Authors

@idg10 ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/)).


## Context

The main `System.Reactive` component must maintain backwards compatibility with previous versions. For the reasons described in [ADR-0005](0005-package-split.md), it was necessary to remove UI-framework-specific components from this package's public API, but we retain the same runtime API.

So we have two slightly different levels of compatibility:

* full binary backwards compatibility
* source compatibility for everything except UI-framework-specific API features


### Problems with the existing API Compatibility Tests

Rx.NET has for years executed tests intended to verify that the public API does not change by accident. These test have used `[PublicApiGenerator`](https://github.com/PublicApiGenerator/PublicApiGenerator) to generate a C# source file that contains the public-facing API of an assembly. For example, here's a fragment of the output it generates:

```cs
namespace System.Reactive
{
    public sealed class AnonymousObservable<T> : System.Reactive.ObservableBase<T>
    {
        public AnonymousObservable(System.Func<System.IObserver<T>, System.IDisposable> subscribe) { }
        protected override System.IDisposable SubscribeCore(System.IObserver<T> observer) { }
    }
```

As this illustrates, this isn't technically legal C#: the compiler would reject that `SubscribeCore` method because the method has a return type but the body has no `return` statement. However, the output of this tool isn't meant to be compiled: its job is only to capture the public-facing types and methods that an assembly defines, so it uses just enough C# syntax to do that.

The Rx test suite included a `Tests.System.Reactive.ApiApprovals` project which generated files of this form for various Rx.NET assemblies and compared them with files containing the expected results. This prevented us from changing the public API accidentally.

This worked fairly well but it had some significant shortcomings:

* only the .NET Framework target was verified
* the legacy facade packages weren't tested (unless you count the slightly curious `System.Reactive.Observable.Aliases` as a facade, which arguably it is, but it provides wrapper types not type forwarders)
* the `PublicApiGenerator` library was, until recently, incapable of understanding type forwarders

The first issues could be overcome with a bit of work. The second issue is an upshot of the third: legacy facade packages expose public types that are actually defined in other packages, using `TypeForwardedToAttribute` assembly-level attributes. These have a completely different representation in the low-level metadata format, and are also slightly tricksy when used through reflection. Tools aiming to describe or compare public APIs need to take special steps to process type forwarders correctly, and until quite recently, `PublicApiGenerator` did not do this.

Although the `PublicApiGenerator` does now support type forwarders, we now have a new problem: `System.Reactive` effectively has two APIs for each target:

* The runtime API (provided by assemblies in the package's `lib` folder) which continues to include UI-framework-specific types
* The build-time API (provided by assemblies in the package's `ref` folder) which excludes UI-framework-specific types

Furthermore, when it comes to verifying the API of all TFMs, we want to ensure consistency not just from one version of Rx.NET to the next within each TFM, but also to ensure that the various TFMs are consistent with one another as far as is possible.

### Possible alternatives

Options:

1. Continue to use `PublicApiGenerator`
2. Use the reference assembly generation tools that Microsoft uses
3. Use the package validation built into the .NET SDK
4. Use `Microsoft.CodeAnalysis.PublicApiAnalyzers`, in which we would define an explicit list of all known APIs. (E.g., Aspire and Orleans use this.)

Right now we're undecided. We thought we would be unable to use option 4, because `PublicApiAnalyzers` also appears not to understand type forwarders, and back when we thought `System.Reactive` was going to become a legacy facade, that would have been a big problem. (We're not yet 100% sure that the means by which we've enabled `System.Reactive` to remain as the main package will definitely work for all scenarios, and we may yet have to revert to the previous plan of demoting it to a legacy facade, at which point this will, once again, matter.)

Option 3 is good for ensuring that the public API conforms to semantic versioning: it will stop us from accidentally removing or changing an existing API. But it has one shortcoming: it doesn't provide a great way to add new items deliberately. By default, baseline checks verify that no existing features have been changed or removed, but will permit new features if the major or minor version number has gone up.

The problem here is that it won't tell us if we _accidentally_ add new public API features.

You can enable a strict mode that reports changes of any kind, which will flag additions as well as removals. This prevents accidental additions, but what are you supposed to do when you mean to add a new item? We can add suppressions to prevent warnings when adding new items, but this is an awkward mechanism when the intention is to add new public members. The old `PublicApiGenerator` approach had an advantage here: there was a specific artifact that reflected precisely what we intend the API to be. When adding new items, we always edit that file, meaning that it's always perfectly clear that a new API feature was added intentionally.

Option 4 might be a better bet, but until we are confident that its lack of type forwarder support isn't going to be a problem, we can't commit to that.

Option 2 has its attractions. Microsoft has tooling that generates compilable source code for producing reference assemblies. Since this gets used for building the .NET runtime class libraries, we know that it is a comprehensive and well-maintained tool. However, currently it appears to be available only if you build things the same way that Microsoft does. For example, we might need to use the 'arcade' build system, because some of the NuGet packages involved (e.g. `Microsoft.DotNet.ApiCompat.Task` and `Microsoft.DotNet.GenAPI.Task`) appear to be available only on special NuGet feeds used in those build systems. Also, looking at those packages, they seem like they might be in a state of perpetual preview. If you're a developer working in the .NET runtime library source tree, keeping up to date with that build tooling is part of the job, but it might not be a good idea for Rx.NET to get on that particular treadmill.


### As Usual, Legacy UWP Makes Things Awkward

Currently we're doing this:

```xml
  <Target Name="_SetUwpTfmForPackageValidation" BeforeTargets="RunPackageValidation">
    <ItemGroup>
      <PackageValidationReferencePath Condition="%(PackageValidationReferencePath.TargetFrameworkMoniker) == '.NETCore,Version=v5.0'" TargetFrameworkMoniker="UAP,Version=10.0.18362.0" TargetPlatformMoniker="Windows,Version=10.0.18362.0" />
    </ItemGroup>
  </Target>
```

We think this is necessary to get the package validation to work without error for the `uap10.0.18362` target.

It's possible that this is necessary only because I got my hackery for making UWP projects build in the modern SDK wrong. I could try specifying these monikers from the start. However, I have a recollection that some bits of the build actually depend on the wrong TFM being set. So this technique of modifying it just before package validation may be the only way.


## Decision

* `System.Reactive` project uses `<EnablePackageValidation>` and `<PackageValidationBaselineVersion>` to ensure compatibility with Rx v6.1
* for now we continue to use the `PublicApiGenerator` tool to guard against accidental addition of new public API features

We need to use `<ApiCompatSuppressionFile>` because the tooling detects not just inconsistency with a baseline version but also inconsistencies between targets within the package. Rx 6.1 already has a couple of internal consistencies as a result of the `ThreadPoolScheduler` it supplied for UWP being slightly different from the type of the same name supplied for all other platforms. And the tool detects our deliberate hiding of the UI-framework-specific API features, so we need suppressions to tell it that we're doing this on purpose.


## Consequences

The built-in package validation means that we are now finally validating _all_ TFMs, which is a significant improvement. Furthermore, the tool now performs internal consistency checks, so if there are unexpected differences between TFMs, or between the `ref` and `lib` assemblies, we will now get told, whereas before this would have gone undetected.

Since we are, for now, continuing to use `PublicApiGenerator`-based tests to guard against accidental additions to the API, we have some ongoing development overhead from this. We would prefer to move to Microsoft-supported tools, but until we can determine whether `Microsoft.CodeAnalysis.PublicApiAnalyzers` will definitely work for our scenarios, we are living with that.