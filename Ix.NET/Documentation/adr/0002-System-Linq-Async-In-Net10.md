# Migration of core `IAsyncEnumerable<T>` LINQ to runtime libraries

.NET 10.0 provides LINQ support for `IAsyncEnumerable<T>` in the runtime class libraries. This effectively renders most of `System.Linq.Async` irrelevant. However, enabling a smooth transition to .NET 10.0 for existing users of this library is not entirely straightforward. This document describes how this will work.

## Status

Accepted.

## Authors

@idg10 ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/))


## Context

As an accident of history, the Rx.NET repository ended up being the de facto implementation of LINQ for `IAsyncEnumerable<T>` from 2019 when .NET Core 3 shipped up until late 2025 when .NET 10 shipped.

This happened because Rx.NET had effectively been the incubator in which `IAsyncEnumerable<T>` was originally developed. Back before .NET Core 3.0, there was no such interface built into .NET, but Rx _did_ define this interface as part of its 'interactive extensions for .NET' feature. (It did this as early as 2010.) It also implemented common LINQ operators for that interface.

.NET Core 3.0 defined its own version of this `IAsyncEnumerable<T>`, but the .NET team did not implement LINQ for it at that time. Since the Rx.NET repository already had a fairly complete implementation of LINQ for its original version of `IAsyncEnumerable<T>`, it was fairly easy to adapt this to the new version of `IAsyncEnumerable<T>` built into .NET. Thus `System.Linq.Async` was born.

In .NET 10.0, the .NET team decided to take ownership of this functionality. For various reasons they did not simply adopt the existing code. (One reason is that .NET class library design guidelines have evolved over time, and some of the methods in Rx's `System.Linq.Async` did not align with those guidelines.) So the .NET team took the decision that they were not going to maintain backwards compatibility with the existing Rx.NET-originated `System.Linq.Async` library. Instead, there is a new `System.Linq.AsyncEnumerable` library that defines equivalent functionality, but implemented from scratch, and fully in conformance with current .NET class library design guidelines.

Most of the API changes fall into one of these categories:

1. Where `System.Linq.Async` defined methods taking an `IComparer<T>` and an associated overload without the `IComparer<T>`, `System.Linq.AsyncEnumerable` only defines the overload that takes the `IComparer<T>`, making it optional with a default value of `null`
2. For certain operators (e.g. `Min`, `Max`, `Sum`) `System.Linq.Async` defined methods operating directly on numerical sequences, and also ones that operate on sequences of any type, taking an addition argument to project each element to a numeric value; in `System.Linq.AsyncEnumerable`, these projection-based variants either have a different name (e.g. `MaxByAsync`) or simply don't exist (as with `SumAsync`)
3. `System.Linq.Async` offered some adapters (e.g. `ToEnumerable`, `ToObservable`) that handled async operations in potentially risky ways (sync over async) or ways that embed opinions about how to do it (e.g. `ToObservable` does not provide the caller with any scheduling options); `System.Linq.AsyncEnumerable` has chosen simply not to implement these at all
4. Operators that accept callbacks (e.g. `Select` and `Where`) can be passed either a normal non-async callback (e.g. `Func<TElement, TResult>` for `Select` or `Func<TElement, bool>` for `Where`), or an `async` callback, in which case the callback returns a `Task<T>` and may support cancellation. `System.Linq.Async` used different names for these methods: it added an `Await` suffix and also a `WithCancellation` suffix to distinguish the forms where the callback takes a cancellation token. `System.Linq.AsyncEnumerable` requires all `async` callbacks to accept a cancellation token (which they are free to ignore of course) and does not use different names for these forms. E.g., in place of `System.Linq.Async`'s `WhereAwait`, `System.Linq.AsyncEnumerable` just offers an additional overload of `Where`.

There are also a couple of cases where functionality simply has not been reproduced. For example, `System.Linq.Async` provides an `AsAsyncEnumerable` to enable deliberate type erasure.

`System.Linq.Async` also defined some interfaces that are not replicated in `System.Linq.AsyncEnumerable`. `System.Linq.Async` defined `IAsyncGrouping` to act as the return type for `GroupBy`. `System.Linq.AsyncEnumerable` just uses `IAsyncEnumerable<IGrouping<TKey, TElement>>`, which is not quite the same: this enables asynchronous iteration of the sequence of groups, but each invidual group's contents are not asynchronously enumerable. `IAsyncGrouping` enabled asynchronous enumeration of both. In practice, `System.Linq.Async` did not exploit this: it fully enumerated the whole source list to split items into groups before returning the first group, so although it compelled you to enumerate at both levels (e.g., with nested `await foreach` loops), in reality only the outer level was asynchronous in practice. So this interface added complication without real benefits. There is also `IAsyncIListProvider<T>`, an interface that arguably should not have been public in the first place, serving only to enable some internal optimizations. (Apparently it was public in `System.Linq.Async` because it is also used in other parts of Ix.NET.) 

A further complication is that some methods in `System.Interactive.Async` clash with methods in `System.Linq.AsyncEnumerable`. For example, `MaxByAsync` and `MinByAsync`. Originally `MinBy` and `MaxBy` were unique to Rx.NET and Ix.NET. But .NET 6.0 added operators with these names to LINQ to Objects. Confusingly, they were slightly different: the Rx.NET and Ix.NET versions recognize that there might not be a single minimum or maximum value, and thus provide a collection of all the entries that are at the maximum value, but the .NET runtime class library versions just pick one arbitrary winner. So at this point, `System.Interactive` renamed its versions to `MinByWithTies` and `MaxByWithTies`. Unfortunately that same change wasn't made in `System.Interactive.Async`, so we now have the same situation with `System.Linq.AsyncEnumerable`: the .NET runtime class libraries now define `MinByAsync` and `MaxByAsync` extension methods for `IAsyncEnumerable<T>`, and these take the same arguments as the ones in `System.Interactive.Async`, but have a different return type, and have different behaviour!

One more important point to consider is that although LINQ to `IAsyncEnumerable<T>` _mostly_ consists of extension methods, there are a few static methods. (E.g., `AsyncEnumerable.Range`, which the .NET library implements, and `AsyncEnumerable.Create`, which it does not.) With extension methods, the compiler does not have a problem with multiple identically-named types in different assemblies all defining extension methods as long as the individual methods do not conflict. However, non-extension methods are a problem. If `System.Linq.Async` were to continue to define a public `AsyncEnumerable` type, then calls to `AsyncEnumerable.Range` would fail to compile: even though there would only be a single `Range` method (supplied by the new `System.Linq.AsyncEnumerable`) this would fail to compile because `AsyncEnumerable` itself is an ambiguous class name. So it will be necessary for the public API of `System.Linq.Async` v7 not to define an `AsyncEnumerable` type. This places some limits on how far we can go with source-level compatibility. (Binary compatibility is not a problem because the runtime assemblies can continue to define this type.)

Since that constraint requires us to define a new type to hold all the obsolete extension methods (which we'll be calling `AsyncEnumerableDeprecated`) this creates a new problem: the runtime API needs to continue to provide all these methods as members of `AsyncEnumerable` (to provide binary compatibility) but any code newly compiled against Ix.NET v7 that is continuing to use these deprecated method (and which is therefore tolerating or suppressing the deprecation warning) will now end up building code that expects these methods to be in the `AsyncEnumerableDeprecated` class. We therefore need to provide all these methods in the runtime assemblies _twice_: once for binary compatibility as members of `AsyncEnumerable` (a class we completely hide at build time) and again as members of `AsyncEnumerableDeprecated` (the class we add to provide source-level backwards compatibility for code using the deprecated methods, in a way that doesn't cause ambiguous type name errors).


## Decision

The next Ix.NET release will:

1. add a reference to `System.Linq.AsyncEnumerable` and `System.Interactive.Async` in `System.Linq.Async`
2. remove from `System.Linq.Async`'s and `System.Interactive.Async`'s publicly visible API (ref assemblies) all `IAsyncEnumerable<T>` extension methods for which direct replacements exist (adding `MinByWithTiesAsync` and `MaxByWithTiesAsync` for the case where the new .NET runtime library methods actually have slightly different functionality)
3. Rename `AsyncEnumerable` to `AsyncEnumerableDeprecated` in the public API (reference assemblies; the old name will be retained in runtime assemblies for binary compatibility) to avoid errors arising from there being two definitions of `AsyncEnumerable` in the same namespace
4. add [Obsolete] attribute for members of `AsyncEnumerableDeprecated` for which `System.Linq.AsyncEnumerable` offers replacements that require code changes to use (e.g., `WhereAwait`, which is replaced by an overload of `Where`)
5. the `AsyncEnumerable.ToEnumerable` method that was a bad idea and that should probably have never existed has been marked as `Obsolete` and will not be replaced; note that although `ToObservable` has issues that meant the .NET team decided not to replicate it, the main issue is that it embeds opinions, and not that there's anything fundamentally broken about it, so we do not include `ToObservable` in this category
6. remaining methods of `AsyncEnumerable` (where `System.Linq.AsyncEnumerable` offers no equivalent) are removed from the publicly visible API of `System.Linq.Async`, with identical replacements being defined by `AsyncEnumerableEx` in `System.Interactive.Async`
7. mark `IAsyncGrouping` as obsolete
8. mark the public `IAsyncIListProvider` as obsolete, and define a non-public version for continued internal use in `System.Interactive.Linq`
9. continue to provide the full `System.Linq.Async` API in the `lib` assemblies to provide binary compatibility
10. in the runtime `System.Linq.Async` assembly provide a facade that duplicates the legacy `AsyncEnumerable` methods on an `AsyncEnumerableDeprecated` type so that code that builds against `System.Linq.Async` v7, and which chooses to continue to use methods marked as `[Obsolete]`, will find those methods at runtime
11. mark the `System.Linq.Async` NuGet package as obsolete, and recommend the use of `System.Linq.AsyncEnumerable` and/or `System.Interactive.Async` instead

The main effect of this is that code that had been using the `System.Linq.Async` implementation of LINQ for `IAsyncEnumerable<T>` will, in most cases, now be using the .NET runtime library implementation if it is rebuilt against this new version of `System.Linq.Async`.

If using .NET 10, developers may find that all they need to do is remove the reference to `System.Linq.Async`. (If using earlier versions of .NET, or .NET FX, they can replace it with a reference to `System.Linq.AsyncEnumerable`.) If they were using any `XxxAwaitAsync` and `XxxAwaitWithCancellationAsync` methods, they will have to change these calls to use the new equivalent overloads.

If developers are using `System.Linq.Async` features that are not available in `System.Linq.AsyncEnumerable`, they should still remove the `System.Linq.Async` reference (since we will be deprecating that package), but they will add a reference to `System.Interactive.Async`. For example, although `System.Linq.AsyncEnumerable` defines `AverageAsync`, it does not offer the same range of functionality as `System.Linq.Async` previously did: overloads taking selectors (both sync and async). These methods become hidden in `System.Linq.Async` (available only for binary compatibility) and they have moved to `AsyncEnumerableEx` in `System.Interactive.Async`. `System.Linq.Async` now adds a transitive reference to `System.Interactive.Async` in order to ensure continued source compatibility until such time as people update their NuGet references.

Developers using the methods we should probably never have provided (the sync-over-async methods such as `ToEnumerable`) will only be able to use these by retaining a reference to the deprecated `System.Linq.Async` package and ignoring or suppressing the obsolete warning. Our position is that these developers should find another approach. Or if they absolutely insist on doing sync-over-async but want to rid their code of obsolete/deprecation warnings, they will have to write their own versions of these methods.

In summary, each of the features previously provided by `System.Linq.Async` will be in one of these categories:

* Method hidden in `ref` assembly, available in `System.Linq.AsyncEnumerable`
* Method hidden in `ref` assembly, available in `System.Interactive.Async`
* Method visible but marked as `Obsolete`, with new but slightly different equivalent available in `System.Linq.AsyncEnumerable`

### TFMs

We want to keep the TFMs for all Ix.NET packages exactly the same in this version, because the only reason for Ix.NET v7 to exist is to deal with the new existence of `System.Linq.AsyncEnumerable`.

There is one issue with this. If a project has a `net6.0` target and tries to use `System.Linq.AsyncEnumerable`, it produces a build warning, saying that it's not supported on that runtime. Although we don't like having this build warning, we are currently intending not to do anything about it, because we believe that messing with the TFMs is likely to have unintended consequences.

If it turns out that this does cause problems, we'll revisit this and do a new release.

## Consequences

Binary compatibility is maintained: any code that was built against `System.Linq.Async` v6 but which finds itself running against v7 at runtime should continue to work exactly as before.

Code that had been written to use `System.Linq.Async` v6 that upgrades to .NET 10 will automatically move to the .NET runtime library implementation without needing any code changes in cases where the .NET 10 implementation is source-compatible with `System.Linq.Async`. Code using methods where .NET 10 has changed (to comply with current class library design rules) will continue to build and run correctly, but the compiler will warn the developer that they are now using obsolete methods, and these warnings will indicate the recommended replacement. Code using methods in `System.Linq.Async` that .NET 10 has chosen not to provide equivalents for will automatically move to using the `System.Interactive.Async` implementations without needing any code changes. Since the `System.Linq.Async` NuGet package will be marked as obsolete, the developer will know that they should stop using it. If they are not using any of the `Obsolete` methods they will be able to remove the method, and might need to add a reference to `System.Interactive.Async`.

The situation is very similar for code written to use `System.Linq.Async` v6 that does _not_ upgrade to .NET 10 (e.g. either it stays on .NET 8 or 9, or it targets .NET Framework or .NET Standard) but which newly acquires a dependency on `System.Linq.AsyncEnumerable` either because the developer adds it, or because they update to a new version of some component which adds it as a new transitive dependency.

Code written to use `System.Linq.Async` v6 that changes nothing at all but, which is rebuilt after `System.Linq.Async` v7 is released, will see a warning that the package is now deprecated. Developers can fix this warning by removing the package and adding a reference to `System.Linq.AsyncEnumerable` or `System.Interactive.Async` or both as required.