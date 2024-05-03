# Migrating `System.Linq.Async` to the .NET Runtime Libraries (BCL)

In February 2023, shortly after endjin became the new maintainers of the Rx.NET repository, [David Fowler](https://github.com/davidfowl) (Microsoft distinguished engineer) posted [this comment](https://github.com/dotnet/reactive/discussions/1868#discussioncomment-4905880):

> Throwing out an idea: it would be good to cede the IX operators back into the BCL given the first class support for IAsyncEnumerable<T>. There’s been many discussions about why we’re not adding async LINQ to the BCL and it’s because this package already does that. While I have no problems in general with an IX dependency, it feels like something that should be built in now given the in the box (including compiler support) for IAE.

We would like this to happen. Our initial focus was on re-aligning the source code with current tools, and then addressing the problems that can occur with self-contained and AOT deployment when using Rx on a Windows-specific target. But we would now like to move this suggestion forward, if the .NET team is still willing.

## Context

The `System.Linq.Async` code is in the Rx.NET repository (this repository) as an accident of history. Specifically:

* This repository is also home to Rx's mirror image, `System.Interactive`, aka **Ix**
* Ix originally introduced its own asynchronous version of `IEnumerable<T>` many years before .NET runtime libraries added `IAsyncEnumerable<T>`
* Ix already had a comprehensive implementation of the standard LINQ operators for its original asynchronous enumerable
* When [IAsyncEnumerable<T>](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1?view=net-8.0) was added in .NET Core 3.0, it made sense for Ix to start using that instead of its original definition
* At that point, Ix had a complete implementation of LINQ for `IAsyncEnumerable<T>`, and the .NET runtime libraries did not, so it make sense for Ix to publish these as the [`System.Linq.Async`](https://www.nuget.org/packages/System.Linq.Async) package we have today

The most significant factor in that last point was that the BCL team had not built their own LINQ for `IAsyncEnumerable<T>`, so Ix was the only game in town. And at the time the BCL team apparently did not want to move this code into the .NET Runtime repository.

If that last fact has changed—if the BCL team _is_ now open to moving this code into the .NET runtime repository—then we want that to occur for the following reasons:

* `System.Linq.Async` is the asynchronous form of LINQ to Objects, which already lives in the .NET Runtime repository, so they belong together
* if the synchronous and asynchronous versions of LINQ to Objects are finally united, the BCL team can maintain feature parity (whereas today, `System.Linq.Async` has fallen behind its synchronous counterpart)
* endjin has a very limited budget for Rx.NET maintenance, and we would prefer not to be on the hook for this functionality, which really has nothing to do with reactive programming

(Technically, there is one connection with Rx.NET. The availability of LINQ for `IAsyncEnumerable<T>` is the reason we believe that adding back pressure into Rx.NET is unnecessary; in fact we think it would be a mistake. But moving this functionality into the .NET runtime libraries wouldn't change that.)

### Compatibility

There are two options. The new built-in library could maintain compatibility with the published library, and we could release a new version of the `System.Linq.Async` package with type forwarders for newer runtimes. Or it could make no such attempt

#### Pros of non-compatibility

[Stephen Toub commented](https://github.com/dotnet/reactive/pull/2102#issuecomment-2090886494) on an earlier draft of this document, saying:

>  were a bunch of choices made in the APIs exposed here that we would not want to bring in, e.g. an AggregateAsync method is fine, but not AggregateAwaitAsync or AggregateAwaitWithCancellationAsync... that's simply not how we'd choose to expose such functionality. We would also be able to design the new methods taking into account features that didn't previously exist, e.g. generic math

Also, there are significant dangers in attempting to make functionality available through a public NuGet package when that functionality is also built into some but not all versions of .NET. If you ever had to deal with the [problems with `System.Net.Http` 4.1.1-4.3.0](https://github.com/dotnet/runtime/issues/20777) you will know how painful it is when packages get this wrong.

#### Pros of compatibility

The existing `System.Linq.Async` library is widely used: it has had 126 million downloads as of May 2024. Although it enjoys no official support from Microsoft, there are some factors that may cause people to perceive it otherwise—its name starts with `System`, and the package gets a blue tick followed by "by .NET Foundation" which strongly suggests that it's part of .NET, even though it isn't. (People continue to believe that Rx.NET is still a Microsoft thing, well over a decade after that stopped being true.) The Microsoft Developer YouTube channel positioned this library as "how to use LINQ with IAsyncEnumerable" in https://www.youtube.com/watch?v=Ktl8K2b1-WU and didn't make it at all clear that this is just a community-supported open source project.

Also, this part of David Fowler's original message is relevant:

> we’re not adding async LINQ to the BCL and it’s because this package already does that

We should therefore consider what it would mean for existing users of this library if the code were to migrate into the .NET runtime libraries. Given its widespread usage and the inaccurate but widespread perception that the existing `System.Linq.Async` is a Microsoft thing, there is a strong case for avoiding breaking changes. Ideally, projects already using `System.Linq.Async` should be able to upgrade to whichever version of .NET incorporates this functionality without needing to change anything in their code. Projects that have acquired a dependency on this package transitively without the developers being aware that they've done so shouldn't get any nasty surprises as a result of this change.

The .NET runtime libraries could offer a `System.Linq.Async` assembly with a public API fully compatible with the existing library. If this were to go into, say, .NET 10.0, we would publish a new version of the `System.Linq.Async` NuGet package with a `net10.0` target consisting entirely of type forwarders pointing into the .NET runtime library implementation.

That way, code using the existing package would continue to work and would automatically migrate to the .NET runtime implementation in applications that target versions of .NET that have `System.Linq.Async` built in. Developers using such versions of .NET could remove the reference to the `System.Linq.Async` package if they wanted, but would not be required to. 

For as long as versions of .NET exist that do not have `System.Linq.Async` built in remain in support, we would want to continue to enable this functionality on those targets. So the `System.Linq.Async` NuGet package would continue to offer full implementations for older versions of .NET.

Ideally, if projects retain a reference to the `System.Linq.Async` package that is redundant (because the application targets only versions of .NET where this functionality is built in) it would be good if some sort of build-time message could let them know that they can safely remove this dependency, but this is not a must-have feature.

## Proposal

This section describes the proposed approach.

### Not backwards compatible

[Stephen Toub's comment](https://github.com/dotnet/reactive/pull/2102#issuecomment-2090886494) seems to make it fairly clear that binary compatibility is not going to happen.

> a) there would be breaking changes (both source and binary) and b) the existing types could not just be type-forwarded to the new ones

So async LINQ functionality built into the .NET runtime libraries would be a new API. There would be no binary compatibility, and no attempt to ensure source compatibility. There are likely to be some specific examples that do not change, but some definitely would change.

For the most part this simplifies, things, but it does also cause some problems that we need to anticipate.

### Renaming Ix's `System.Linq.Async`

Since there is to be no compatibility between the current `System.Linq.Async` provided by Ix.NET, and any future .NET runtime implementation, we believe that it would be a good idea to change the existing component name and namespaces. The naming currently strongly implies that this is an official Microsoft component, which it is not. People may also think there's a guarantee that these APIs will not change; not only is there no such guarantee, we can be sure that they _will_ change if the .NET team builds async LINQ into the runtime libraries.

The fact that the extension methods are defined in `System.Linq` will create a problem if a built-in implementation does ever appear. Today, code using Ix.NET's `System.Linq.Async` can write this sort of thing:

```cs
public async Task<bool> UseIae(IAsyncEnumerable<int> iae)
{
    return await iae.AllAsync(x => x % 2 == 0);
}
```

Since .NET 8.0, new projects have a setting that generates a `global using global::System.Linq;` statement, so you don't even need to write `using System.Linq;` for that `AllAsync` extension method to be available. But imagine some future .NET version that has async LINQ built in: that will also almost certainly define extension methods for `IAsyncEnumerable<T>` in the `System.Linq` namespace.

The effect will be that any project containing code like that, and which upgrades to this hypothetical new version of .NET, will no longer compile. This `AllAsync` method will now be ambiguous: it could refer either to the implementation in `System.Linq.Async` or the new implementation supplied by the .NET runtime. There is no convenient way to disambiguate extension methods when this happens.

In some cases developers might be able to resolve this by removing the reference to `System.Linq.Async`, but if they acquired that transitively, they won't be able to unless they either stop using some other component, or can induce the author of that component to release a new version that doesn't depend on `System.Linq.Async`.

There's a particularly insidious variation on this theme in which an application had never attempted to use `System.Linq.Async` directly, but had a transitive reference to it, and then some time later starts trying to use the new async LINQ built into .NET. To the compiler, this is basically the same situation as the example above—a call to `AllAsync` will produce an error because it is ambiguous. But to the developer it looks a bit different. In the earlier example it's "Well I was using that `System.Linq.Async` to do this before, but I now need to make some adjustments because this is now built into .NET." But in this second scenario, it's a case of "What is this `System.Linq.Async` library, and how can a library with that name be _preventing_ me from using async LINQ?" If such a developer was unfamiliar with the history of these libraries (and why would they know about that?) this will be baffling and vexing.

We can pre-emptively prevent this by deprecating the existing `System.Linq.Async`, and releasing a new library which is identical except for its name and namespaces. We've not yet picked a name, but imagine we called the package `LinqAsyncExtensions`, and we put these extension methods in the `AsyncExtensions.Linq` namespace. Anyone using `System.Linq.Async` today would see in the NuGet package explorer that the package has been deprecated, and that they are being asked to move to `LinqAsyncExtensions`. We would provide documentation explaining the need to add a `using AsyncExtensions.Linq;` declaration. This means that if in the future, .NET ships async LINQ, this code, and also anything depending on this code, will be able to use the .NET async LINQ implementation with `using System.Linq;`. Our implementation won't cause conflicts because it will be visible only if you ask for it explicitly through the `AsyncExtensions.Linq` namespace.

This change would also clarify that `System.Linq.Async` is not, and never has been, an officially supported component from Microsoft. This should help set more realistic expectations around this component.

It would also make it possible for us to produce a version of `LinqAsyncExtensions` that had a target which, on versions of .NET new enough to have built-in async LINQ, simply deferred to that. (This would not be type forwarding. It would reduce `LinqAsyncExtensions` to a facade over the .NET runtime implementation.) This would enable `LinqAsyncExtensions` to make life easier for people who need to target multiple versions of .NET, where only some have the built-in async LINQ. They could program against the single API offered by `LinqAsyncExtensions`, knowing that this will defer to the high-quality Microsoft-supplied implementation where that is available.


### Code contribution

It is not a given that a future .NET runtime library implementation of async LINQ to objects would be based on our existing implementation. It's possible that the .NET team would decide, after reviewing our code, that it does not meet current performance requirements, and that it would be less effort to re-implement it from the ground up than to try to adapt the existing code.

However, it might also be the case that they would consider the current code to be an adequate starting point. In that case, the basic changes would be to move the code into a folder under in the [`src/libraries`](https://github.com/dotnet/runtime/tree/main/src/libraries) folder in the .NET Runtime repository (either a new `System.Linq.Async` folder, or possibly into the existing `System.Linq` folder) as follows:

| Existing code | New location in .NET Runtime repository |
|--|--|
| [`Ix.NET/Source/System.Linq.Async`](https://github.com/dotnet/reactive/tree/main/Ix.NET/Source/System.Linq.Async) folder | `src` subfolder either in the existing `System.Linq` folder or in a new `System.Linq.Async`|
| [`Ix.NET/Source/System.Linq.Async.Tests`](https://github.com/dotnet/reactive/tree/main/Ix.NET/Source/System.Linq.Async.Tests) folder | `tests` subfolder in the existing `System.Linq` folder or in a new `System.Linq.Async` |
| N/A | a new `System.Linq.Async.cs` file in the `ref` subfolder of `System.Linq[.Async]` |

This would be a starting point, but we expect that significant work would then be required to meet the .NET team's current quality bars.

(**Note**: although there is a `Source/refs` subfolder that purports to build ref assemblies, this relies on building the existing source with the `REFERENCE_ASSEMBLY` preprocessor symbold defined. The `System.Linq.Async` project completely ignores this symbol, so the 'reference' assembly for that ends up containing all of the code. In any case, the .NET runtime library seems to define reference assemblies explicitly, so that the public API is specified separately from the implementation. We would need to do the same. This shouldn't be too difficult, though, because we already extract something that looks a lot like this would need to in the tests that verify that we haven't introduced changes to the public API by accident.)


## Use of T4 templates

The existing code has various `.tt` files to generate multiple forms of the same code. The `Sum`, `Min`, `Max` and `Average` operators all use this to provide implementations for each of the built-in numeric types. Of course, as of .NET 7.0 this sort of thing isn't strictly necessary because we can use generic math instead. Unfortunately, the need to maintain binary compatibility means we can't do that in practice, because existing code will be looking for the specialized methods. Also we generate 2-, 3-, and 4- argument forms of some internal combination operators use to improve the efficiency of certain `Select` and `Where` scenarios.

The status of T4 support has always been a little unclear to me. However, it looks like other things in the .NET runtime repository use T4, so we're hopeful that this wouldn't be an obstacle.

It would technically be possible to remove the use of T4 by just incorporating the template outputs into the repository and then abandoning the templates. However, this increases ongoing maintenance overhead, because if changes are ever required to the relevant code, they need to be applied multiple times.


### Remove build-time source generator `System.Linq.Async.SourceGenerator`

In its current form, `System.Linq.Async` relies on a source generator to define some of the public methods of `AsyncEnumerable`. It appears that this was done to enable a choice between two API design options: flat vs explicitly awaitable and cancellable. In the flat mode, all the variations have the same name:

```cs
// 'Flat' API (not what was finally chosen)

// Non-async predicate
bool allOdd = await iae.AllAsync(item => item.IsOdd, cancel);

// Async, non-cancellable predicate.
bool allSlowlyOdd = await iae.AllAsync(async item => await item.GetIsOddAsync(), cancel);

// Async, cancellable predicate.
bool allSlowlyOddCancellable = await iae.AllAsync(async (item, cancel) => await item.GetIsOddAsync(cancel), cancel);
```

or should these be distinguished by name:

```cs
// Non-flat API (as was actually shipped)

// Non-async predicate
bool allOdd = await iae.AllAsync(item => item.IsOdd, cancel);

// Async, non-cancellable predicate.
bool allSlowlyOdd = await iae.AllAwaitAsync(async item => await item.GetIsOddAsync(), cancel);

// Async, cancellable predicate.
bool allSlowlyOddCancellable = await iae.AllAwaitWithCancellationAsync(async (item, cancel) => await item.GetIsOddAsync(cancel), cancel);
```

The code generator makes it possible to switch between these two API styles with a preprocessor symbol (`SUPPORT_FLAT_ASYNC_API`).

The non-flat design was ultimately chosen, for reasons that are lost in the history of time. (I would guess that the problem with the flat API is that we end up with too many too-similar overloads. At best, this could be confusing for developers grappling with a baffling multitude of options in an IntelliSense popup, and perhaps it even causes problems with method overload selection or type inference. But I've not been able to find any discussion of the factors leading to the final choice.)

It doesn't seem useful to retain this flexibility now. The purpose of this seems to be to enable prototypes with both styles to be fully developed, and to enable the choice of style to be deferred right until the last moment. But the first non-preview version shipped years ago, so the 'last moment' was approximate half a decade in the past. This code generator now just adds complexity to the code base. (It's not as though it's generating multiple forms of the same API. It simply decides on the name for the single public facade of the internal implementation for each method that uses it.)

The one question is: does the BCL team agree with the choice of a non-flat API? Based on Stephen Toub's feedback, the answer appears to be: no. So we'd need to make changes here in any case.


### Conform to .NET runtime library standards

Although everything in the Rx.NET repo started out as a Microsoft project, it became a community supported open source project over a decade ago, so it seems likely that the coding standards in this repo and those for the .NET runtime repository will have drifted apart somewhat in that time.

#### Superficial standards

So we expect some changes to be necessary to make this code feel like part of the .NET runtime library source tree when it comes to things like folder layout, project file structure, and naming conventions. But we expect this to be superficial.

One oversight we have noticed in writing up this proposal is that our code is missing doc comments on some of the 'deep cancellation' methods. 'Deep cancellation' was the name for the feature by which operators that accept application-defined callbacks (e.g. `Where`) were able to work with callbacks which took a `CancellationToken`. (All our async operators are cancellable; the 'deep' cancelation meant the ability to forward that to user callbacks. For some operators this wasn't totally straightforward, and it looks like this was, at one point, an experimental feature. However, it seems that it now ships, so although there's a compiler symbol you can set to disable it, we never do that.) We would need to fill that in.

#### Code quality standards

The existing `System.Linq.Async` was written many years ago, and with less sensitivity to efficiency (especially around memory allocation) than is the norm with modern library. It seems likely that substantial changes would be necessary to meet current expectations around performance.

### Parity with LINQ to Objects

LINQ to Objects has seen some additions in recent versions of .NET, and `System.Linq.Async` has not caught up. We would aim to achieve parity before submitting this code to the .NET runtime repo.


### Who publishes `System.Linq.Async`?

As discussed earlier, the `System.Linq.Async` NuGet package would continue to be available on NuGet. It serves three purposes:

1. enabling old code built against existing versions and targeting old .NET versions to carry on unchanged
2. enabling developers who have to write new code that targets older versions of .NET (e.g. .NET FX) to use this functionality
3. providing type forwarders so that old code built against this package will automatically use the .NET runtime library version in applications that target newer versions of .NET

(It's a fairly fine distinction between 1 and 2. You wouldn't necessarily be able to tell which of these two scenarios you were looking at when inspecting an application's source code without knowing the history of that code. But it's important to acknowledge that there are two quite different types of consumer here. There are developers whose apps are already using this, who we do not want to disrupt. But there will also be developers who have no choice but to target a version of .NET where the built-in async LINQ support is unavailable. The key point here is that we're not just supporting legacy projects—new code may have good reasons to use the not-built-in `System.Linq.Async`.)

This means that there would then be two implementations of `System.Linq.Async`: the one built into .NET and the one in the existing NuGet package (and future versions of that package). This raises the question of who maintains and publishes this.

The Rx.NET maintainers could continue to publish and maintain this package. We would essentially freeze it in time: we would not keep parity with any new LINQ to Objects functionality added in the .NET runtime libraries. This might lead to confusion, because there would now be two things called `System.Linq.Async` that are under different ownership, and which are gradually diverging.

The alternative would be for the BCL team to take ownership of the `System.Linq.Async` NuGet package. They could decide whether to offer feature parity for developers stuck on .NET Framework, or to freeze the API in time. But either way this might be a less confusing situation for .NET developers, because all of LINQ to Objects would now be fully back under Microsoft's control resulting in a much clearer position regarding support.

We, the Rx.NET maintainers, would be happy with either approach. Obviously the second option, in which the whole thing becomes Microsoft's problem, would make things easier for us. But if the first option increases the chances of getting `System.Linq.Async` built into all future versions of .NET, we are happy to continue to maintain the legacy NuGet package.
