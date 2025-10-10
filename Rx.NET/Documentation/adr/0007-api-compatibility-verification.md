# Use .NET SDK Package Validation To Ensure Backwards Compatibility

 Migration from PublicApiGenerator to .NET SDK Package Validation


## Status

Proposed


## Authors

@idg10 ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/)).


## Context

The `System.Reactive` component was previously the main Rx.NET component. Unfortunately, for the reasons described in [ADR-0005](0005-package-split.md), it was necessary to demote this package, to enable the UI-framework-specific components to be removed from the main Rx.NET package. `System.Reactive` now has just one job: to provide backwards compatibility. This means it must offer the same API surface area as the v6.0 release.

Additionally, we require that the new main Rx.NET component, `System.Reactive.Net`, maintains backwards compatibility with previous versions. Of course, the very first version to ship will be a special case: there are no older versions with which to maintain binary compatibility. For this first version, the mechanism guaranteeing that `System.Reactive.Net` provides full Rx.NET functionality is the fact that the v7 `System.Reactive` legacy component is compatible with its v6 predecessor: in order for that component to provide full backwards compatibility, all of the Rx.NET v6 functionality must be present _somewhere_ in Rx.NET v7. But once v7.0.0 ships, we will need a mechanism to ensure that all future versions are compatible.

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
* the `PublicApiGenerator` library is incapable of understanding type forwarders

The first issues could be overcome with a bit of work, but the second issue is an upshot of the third: legacy facade packages expose public types that are actually defined in other packages, using `TypeForwardedToAttribute` assembly-level attributes. These have a completely different representation in the low-level metadata format, and are also slightly tricksy when used through reflection. Tools aiming to describe or compare public APIs need to take special steps to process type forwarders correctly, and `PublicApiGenerator` does not do this.

This type forwarder problem means we can't simply add the existing legacy facades to the API Approvals test project, because it is constitutionally incapable of dealing with them. It also means that this existing test project is entirel unsuited to the important job of ensuring that the v7 version of `System.Reactive`, which is now a hollowed-out shell consisting almost entirely of type forwarders, has the exact same API as v6 of `System.Reactive`.

### Possible Replacements for `PublicApiGenerator`

Options:

1. Fix `PublicApiGenerator`
2. Write our own alternative to `PublicApiGenerator`
3. Use the reference assembly generation tools that Microsoft uses
4. Use the package validation built into the .NET SDK and after we ship 7.0.0, enable baselining relative to that release
5. The .publicApi thing with explicit listing of all known APIs. I think this is what Aspire and Orleans are using?

Option 4 looks pretty good for the legacy package, `System.Reactive`, because our intention is for that never to change ever again. (In fact we should also enable it for the other legacy facade packages.)

However, when it comes to ongoing maintenance of the new `System.Reactive.Net`, option 4 has one shortcoming: it doesn't provide a great way to add new items deliberately. By default, baseline checks verify that no existing features have been changed or removed, but will permit new features. You can enable a strict mode that reports changes of any kind, which will flag additions as well as removals. This prevents accidental additions, but what are you supposed to do when you mean to add a new item?

We can add suppressions to prevent warnings when adding new items, but this is an awkward mechanism when the intention is to add new public members. The old `PublicApiGenerator` approach had an advantage here: there was a specific artifact that reflected precisely what we intend the API to be. When adding new items, we always edit that file, meaning that it's always perfectly clear that a new API feature was added intentionally.

TBD: look more into the .publicApi feature to see if it will work for us.

```xml
    <PackageReference Include="Microsoft.CodeAnalysis" Version="4.14.0" PrivateAssets="All" />
    <!--<PackageReference Include="Microsoft.CodeAnalysis.PublicApiAnalyzers" Version="4.14.0">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.DotNet.ApiCompat.Task" Version="9.0.301" />-->
    <PackageReference Include="Microsoft.DotNet.GenAPI.Task" Version="9.0.301-servicing.25272.5" PrivateAssets="All" />
```




### As Usual, Legacy UWP Makes Things Awkward


Currently need `<SuppressTfmSupportBuildWarnings>` and I'm not quite sure why.

Also doing this:

```xml
  <Target Name="_SetUwpTfmForPackageValidation" BeforeTargets="RunPackageValidation">
    <ItemGroup>
      <PackageValidationReferencePath Condition="%(PackageValidationReferencePath.TargetFrameworkMoniker) == '.NETCore,Version=v5.0'" TargetFrameworkMoniker="UAP,Version=10.0.18362.0" TargetPlatformMoniker="Windows,Version=10.0.18362.0" />
    </ItemGroup>
  </Target>
```

It's possible that this is necessary only because I got my hackery for making UWP projects build in the modern SDK wrong. I could try specifying these monikers from the start. However, I have a recollection that some bits of the build actually depend on the wrong TFM being set. So this technique of modifying it just before package validation may be the only way.



## Decision

* Legacy `System.Reactive` package to use `<EnablePackageValidation>` and `<PackageValidationBaselineVersion>` to ensure compatibility with Rx v6.0 (the last version in which `System.Reactive` was the main package)

Need `<ApiCompatSuppressionFile>` because the tooling detects not just inconsistency with a baseline version but also inconsistencies between targets within the package. Rx 6 already has a couple of internal consistencies as a result of the `ThreadPoolScheduler` it supplied for UWP being slightly different from the type of the same name supplied for all other platforms. 


For the new `System.Reactive.Net`: TBD.

## Consequences
