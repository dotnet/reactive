Reactive Extensions
======================

This repository contains four libraries:

* [Rx.NET](Rx.NET/) ([System.Reactive](https://www.nuget.org/packages/System.Reactive/)): a library for event-driven programming with a composable, declarative model
* [AsyncRx.NET](AsyncRx.NET/) (experimental preview) ([System.Reactive.Async](https://www.nuget.org/packages/System.Reactive.Async)): experimental implementation of Rx for `IAsyncObservable<T>` offering deeper `async`/`await` support
* [Interactive Extensions for .NET](Ix.NET/) ([System.Interactive](https://www.nuget.org/packages/System.Interactive/)): extended LINQ operators for `IAsyncEnumerable` and `IEnumerable`
* [LINQ for `IAsyncEnumerable`](./Ix.NET/Source/System.Linq.Async/) ([System.Linq.Async](https://www.nuget.org/packages/System.Linq.Async/)): implements standard LINQ operators for `IAsyncEnumerable`

These are conceptually related in that they are all concerned with _LINQ over sequences of things_. Each is described in the following sections of this README.

Channel  | Rx | System.Linq.Async | Ix |
-------- | :------------: | :-------------: | :-------------: |
Build | [![Build Status](https://dev.azure.com/dotnet/Rx.NET/_apis/build/status/Rx.NET-CI?branchName=master)](https://dev.azure.com/dotnet/Rx.NET/_build/latest?definitionId=9) | Built as part of Ix | [![Build Status](https://dev.azure.com/dotnet/Rx.NET/_apis/build/status/Ix.NET-CI?branchName=master)](https://dev.azure.com/dotnet/Rx.NET/_build/latest?definitionId=28)
NuGet.org | [![#](https://img.shields.io/nuget/v/System.Reactive.svg)](https://www.nuget.org/packages/System.Reactive/) | [![#](https://img.shields.io/nuget/v/System.Linq.Async.svg)](https://www.nuget.org/packages/System.Linq.Async/) | [![#](https://img.shields.io/nuget/v/System.Interactive.svg)](https://www.nuget.org/packages/System.Interactive/)
NuGet.org preview (if newer than release) | [![#](https://img.shields.io/nuget/vpre/System.Reactive.svg)](https://www.nuget.org/packages/System.Reactive/) | [![#](https://img.shields.io/nuget/vpre/System.Linq.Async.svg)](https://www.nuget.org/packages/System.Linq.Async/) | [![#](https://img.shields.io/nuget/vpre/System.Interactive.svg)](https://www.nuget.org/packages/System.Interactive/)
[Azure<br>Artifacts](https://dev.azure.com/dotnet/Rx.NET/_packaging?_a=feed&feed=RxNet) | [![System.Reactive package in RxNet feed in Azure Artifacts](https://azpkgsshield.azurevoodoo.net/dotnet/Rx.NET/RxNet/System.Reactive)](https://dev.azure.com/dotnet/Rx.NET/_packaging?_a=package&feed=5afc77bd-23b4-46f8-b725-40ebedab630c&package=3c02dce4-f7e9-43ec-a014-28ea9fc46f82&preferRelease=true) | Built as part of Ix | [![System.Interactive package in RxNet feed in Azure Artifacts](https://azpkgsshield.azurevoodoo.net/dotnet/Rx.NET/RxNet/System.Interactive)](https://dev.azure.com/dotnet/Rx.NET/_packaging?_a=package&feed=5afc77bd-23b4-46f8-b725-40ebedab630c&package=a3311bc0-c6ea-4460-bea8-b65d633e2583&preferRelease=true) 

For nightly builds, configure NuGet to use this feed: `https://pkgs.dev.azure.com/dotnet/Rx.NET/_packaging/RxNet/nuget/v3/index.json`

### Join the conversation

Catch us in the #rxnet channel over at http://reactiveui.net/slack


## A Brief Introduction to Rx

In this digital age, live streams of data are ubiquitous. Financial applications depend on a swift response to timely information. Computer networks have always been able to provide extensive information about their health and operation. Delivery vans continuously report their progress. Aircraft provide performance telemetry to detect potential maintenance issues before they become serious problems, and cars are now starting to do the same. Many of us wear or carry devices that track our physical activity and even vital signs. And the improvements in machine learning have enriched the insights that can be derived from the ever-increasing volume and variety of live data.

But despite being so widespread, live information streams have always been something of a second class citizen. Almost all programming languages have some innate way to work with lists of data (e.g., arrays), but these mechanisms tends to presume that the relevant data is already sitting in memory, ready for us to work with it. What's missing is the liveness—the fact that an information source might produce new data at any moment, on its own schedule.

Rx elevates the support for live streams of information to the same level as we expect for things like arrays. Here's an example:

```cs
var bigTrades =
    from trade in trades
    where trade.Volume > 1_000_000;
```

This uses C#'s LINQ feature to filter `trades` down to those entities with a volume greater than one million. This query expression syntax is just a shorthand for method calls, so we could also write it this way:

```cs
var bigTrades = trades.Where(trade => trade.Volume > 1_000_000);
```

The exact behaviour of these two (equivalent) code snippets depends on what type `trades` has. If it were a `List<Trade>`, then this query would just iterate through the list, and `bigTrades` would be a sequence containing just the matching objects. If `trades` were an object representing a database table (e.g., an [Entity Framework](https://learn.microsoft.com/en-us/ef/core/) [DbSet](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbset-1), this would be translated into a database query. But if we're using Rx, `trades` would be an `IObservable<Trade>`, an object reporting live events as they happen. And `bigTrades` would also be an `IObservable<Trade>`, reporting only those trades with a volume over a million. We can provide Rx with a callback to be invoked each time an observable source has something for us:

```cs
bigTrades.Subscribe(t => Console.WriteLine($"{t.Symbol}: trade with volume {t.Volume}"));
```

The two key features of Rx are:

* a clearly defined way to represent and handle live sequences of data ([`IObservable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.iobservable-1))
* a set of operators (such as the `Where` operator just shown) enabling event processing logic to be expressed declaratively


## AsyncRx.Net

Although Rx is a natural way to model asynchronous processes, its original design presumed that code acting on notifications would run synchronously. This is because Rx's design predates C#'s `async`/`await` language features. So although Rx offer adapters that can convert between [`IObservable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.iobservable-1) and [`Task<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1), there were certain cases where `async` was not an option.

AsyncRx.Net lifts this restriction by defining `IAsyncObservable<T>`. This enables observers to use asynchronous code. For example, if `bigTrades` were an `IAsyncObservable<Trade>` we could write this:

```cs
bigTrades.Subscribe(async t => await bigTradeStore.LogTradeAsync(t));
```

AsyncRx.Net is currently in preview.

## Interactive Extensions

Rx defines all the standard LINQ operators available for other providers, but it also adds numerous additional operators. For example, it defines `Scan`, which performs the same basic processing as the standard [`Aggregate`](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.aggregate) operator, but instead of producing a single result at the end after processing every element, it produces a sequence containing the aggregated value after every single step. (For example, if the operation being aggregated is addition, `Aggregate` would return the sum total as a single output, whereas `Scan` would produce a running total for each input. Given a sequence `[1,2,3]`, `Aggregate((a, x) => a + x)` produces just `6`, whereas `Scan` would produce `[1,3,6]`.)

Some of the additional operators Rx defines are useful only when you're working with events. But some are applicable to sequences of any kind. So the Interactive Extensions (Ix for short) define implementations for `IEnumerable<T>`. Ix is effectively an extension of LINQ to Objects, adding numerous additional operators. (Its usefulness is borne out by the fact that the .NET runtime libraries have, over time, added some of the operators that used to be available only in Ix. For example, .NET 6 added [`MinBy`](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.minby) and [`MaxBy`](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.maxby), operators previously only defined by Ix.)

This library is called the "Interactive Extensions" because "Interactive" is in a sense the opposite of "Reactive". (The name does not refer to user interactions.)

## LINQ for `IAsyncEnumerable` (`System.Linq.Async`)

One of the features pioneered by Ix was an asynchronous version of `IEnumerable<T>`. This is another example of a feature so useful that it was eventually added to the .NET runtime libraries: .NET Core 3.0 introduced [`IAsyncEnumerable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1), and the associated version C# (8.0) added intrinsic support for this interface with its `await foreach` construct.

Although .NET Core 3.0 defined `IAsyncEnumerable<T>`, it did not add any corresponding LINQ implementation. Whereas [`IEnumerable<T>` supports all the standard operators](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable) such as `Where`, `GroupBy`, and `SelectMany`, .NET does not have built-in implementations of any of these for `IAsyncEnumerable<T>`. However, Ix had provided LINQ operators for its prototype version of `IAsyncEnumerable<T>` from the start, so when .NET Core 3.0 shipped, it was a relatively straightforward task to update all those existing LINQ operators to work with the new, official [`IAsyncEnumerable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1).

Thus, the [System.Linq.Async](https://www.nuget.org/packages/System.Linq.Async/) NuGet package was created, providing a LINQ to Objects implementation for `IAsyncEnumerable<T>` to match the one already built into .NET for `IEnumerable<T>`.

Since all of the relevant code was already part of the Ix project (with `IAsyncEnumerable<T>` also originally having been defined by this project), the [System.Linq.Async](https://www.nuget.org/packages/System.Linq.Async/) NuGet package is built as part of the [Ix project](Ix.NET/).



## Contributing

Some of the best ways to contribute are to try things out, file bugs, and join in design conversations.

* Clone the sources: `git clone https://github.com/dotnet/reactive`
* [Building, testing and debugging the sources](https://github.com/dotnet/reactive/wiki/Building%20Testing%20and%20Debugging)
* [How to Contribute](https://github.com/dotnet/reactive/wiki/Contributing-Code)
* [Pull requests](https://github.com/dotnet/reactive/pulls): [Open](https://github.com/dotnet/reactive/pulls?q=is%3Aopen+is%3Apr)/[Closed](https://github.com/dotnet/reactive/pulls?q=is%3Apr+is%3Aclosed)

Looking for something to work on? The list of [up for grabs issues](https://github.com/dotnet/reactive/issues?q=is%3Aopen+is%3Aissue+label%3A%22Up+for+Grabs%22) is a great place to start.

This project has adopted a code of conduct adapted from the [Contributor Covenant](http://contributor-covenant.org/) to clarify expected behavior in our community. This code of conduct has been [adopted by many other projects](http://contributor-covenant.org/adopters/). For more information see the [Code of conduct](http://www.dotnetfoundation.org/code-of-conduct).

## .NET Foundation

System.Reactive is part of the [.NET Foundation](https://www.dotnetfoundation.org/). Other projects that are associated with the foundation include the Microsoft .NET Compiler Platform ("Roslyn") as well as the Microsoft ASP.NET family of projects, Microsoft .NET Core & Xamarin Forms.

## Current Core Team

The people currently maintaining Rx are:

<table>
  <tbody>
    <tr>
      <td align="center" valign="top">
        <img width="150" height="150" src="https://github.com/idg10.png?s=150">
        <br>
        <a href="https://github.com/idg10">Ian Griffiths</a>
        <p>Hove, UK</p>
      </td>
      <td align="center" valign="top">
        <img width="150" height="150" src="https://github.com/HowardvanRooijen.png?s=150">
        <br>
        <a href="https://github.com/HowardvanRooijen">Howard van Rooijen</a>
        <p>London, UK</p>
      </td>
  </tbody>
</table>

Rx has been around for roughly a decade and a half, so we owe a great deal to its creators, and the many people who have worked on it since. See the [AUTHORS.txt](AUTHORS.txt) for a full list.