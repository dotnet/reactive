# Migration of core `IAsyncEnumerable<T>` LINQ to runtime libraries

.NET 10.0 provides LINQ support for `IAsyncEnumerable<T>` in the runtime class libraries. This effectively renders most of `System.Linq.Async` irrelevant. However, enabling a smooth transition to .NET 10.0 for existing users of this library is not entirely straightforward. This document describes how this will work.

## Status

Proposed.

## Authors

@idg10 ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/))


## Context

As an accident of history, the Rx.NET repository ended up being the de facto implementation of LINQ for `IAsyncEnumerable<T>` from 2019 when .NET Core 3 shipped up until late 2025 when .NET 10 shipped.

This happened because Rx.NET had effectively been the incubator in which `IAsyncEnumerable<T>` was originally developed. Back before .NET Core 3.0, there was no such interface built into .NET, but Rx _did_ define this interface as part of its 'interactive extensions for .NET' feature. It also implented common LINQ operators for that interface.

.NET Core 3.0 defined its own version of this `IAsyncEnumerable<T>`, but the .NET team did not choose to implement LINQ for it. Since the Rx.NET repository already had a fairly complete implentation of LINQ for its original version of `IAsyncEnumerable<T>`, it took only a fairly small amount of work to adapt this to the new version of `IAsyncEnumerable<T>` built into .NET. Thus `System.Linq.Async` was born.

In .NET 10.0, the .NET team decided to take ownership of this functionality. For various reasons they did not simply adopt the existing code. (One reason is that .NET class library design guidelines have evolved over time, and some of the methods in Rx's `System.Linq.Async` did not align with those guidelines.) So the .NET team took the decision that they were not going to maintain backwards compatibility with the existing Rx.NET-originated `System.Linq.Async` library. Instead, there is a new `System.Linq.AsyncEnumerable` library that defines equivalent functionality, but implemented from scratch, and fully in conformance with current .NET class library design guidelines.

Most of the API changes fall into one of these categories:

1. Where `System.Linq.Async` defined methods taking an `IComparer<T>` and an associated overload without the `IComparer<T>`, `System.Linq.AsyncEnumerable` only defines the overload that takes the `IComparer<T>`, making it optional with a default value of `null`
2. For certain operators (e.g. min, max, sum) `System.Linq.Async` defined methods operating directly on numerical sequences, and also ones that operate on sequences of any type, taking an addition argument to project each element to a numeric value; in `System.Linq.AsyncEnumerable`, these projection-based variants either have a different name (e.g. `MaxByAsync`) or simply don't exist (as with `SumAsync`)
3. `System.Linq.Async` offered some adapters (`ToEnumerable`, `ToObservable`) that handled async operations in potentially risky ways (sync over async, or fire-and-forget respectively); `System.Linq.AsyncEnumerable` has chosen simply not to implement these at all

There are also a couple of cases where functionality simply has not been reproduced. `System.Linq.Async` provides an `AsAsyncEnumerable` to enable deliberate type erasure.  TBD.


## Decision

The next `System.Linq.Async` release will:

1. add a reference to `System.Linq.AsyncEnumerable`
2. remove from publicly visible API (ref assemblies) all `IAsyncEnumerable<T>` extension methods for which direct replacements exist
3. add [Obsolete] attribute for all remaining members of `AsyncEnumerable`
4. mark `IAsyncGrouping` as obsolete
5. TBD `IAsyncIListProvider` relocate?
6. continue to provide the full API in the `lib` assemblies to provide binary compatibility

Note that not all of the XxxAwaitAsync and XxxAwaitWithCancellationAsync are handled the same way. In some cases, these now have replacements. E.g. System.Linq.AsyncEnumerable replaces AggregateAwaitAsync with an overload of AggregateAsync. So System.Linq.Async's ref assembly continues to make AggregateAwaitAsync available but marks it as Obsolete, telling you to use AggregateAsync instead. But there are some methods for which no replacement exists. We move these into System.Interactive.Async, because that's where `IAsyncEnumerable<T>` features that have no equivalents in the .NET runtime libraries live. E.g., although `System.Linq.AsyncEnumerable` defines `AverageAsync`, it does not offer the same range of functionality as `System.Linq.Async` previously did: overloads taking selectors (both sync and async). These methods become hidden in `System.Linq.Async` (available only for binary compatibility) and they have moved to `AsyncEnumerableEx` in `System.Interactive.Async`, and `System.Linq.Async` now adds a transitive reference to `System.Interactive.Async` in order to ensure continued source compatibility until such time as people update their NuGet references.

combinations:

* Method hidden in ref, available in `System.Linq.AsyncEnumerable`
* Method hidden in ref, available in `System.Interactive.Async`
* Method visible but marked as `Obsolete`