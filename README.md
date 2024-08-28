Reactive Extensions
======================

This repository contains four libraries which are conceptually related in that they are all concerned with LINQ over sequences of things:

* [Reactive Extensions for .NET](Rx.NET/) aka Rx.NET or Rx ([System.Reactive](https://www.nuget.org/packages/System.Reactive/)): a library for event-driven programming with a composable, declarative model
* [AsyncRx.NET](AsyncRx.NET/) (experimental preview) ([System.Reactive.Async](https://www.nuget.org/packages/System.Reactive.Async)): experimental implementation of Rx for `IAsyncObservable<T>` offering deeper `async`/`await` support
* [Interactive Extensions for .NET](Ix.NET/), aka Ix ([System.Interactive](https://www.nuget.org/packages/System.Interactive/)): extended LINQ operators for `IAsyncEnumerable` and `IEnumerable`
* [LINQ for `IAsyncEnumerable`](./Ix.NET/Source/System.Linq.Async/) ([System.Linq.Async](https://www.nuget.org/packages/System.Linq.Async/)): implements standard LINQ operators for `IAsyncEnumerable`

Each will be described later in this README.

## FREE Introduction to Rx.NET 2nd Edition eBook

<a href="https://introtorx.com/"><img align="left" alt="Introduction to Rx.NET 2nd Edition book cover." src="Rx.NET/Resources/Artwork/title_page_sm.png"/></a>

Reactive programming provides clarity when our code needs to respond to events. The Rx.NET libraries were designed to enable cloud-native applications to process live data in reliable, predictable ways.

We've written a FREE book which explains the vital abstractions that underpin Rx, and shows how to exploit the powerful and extensive functionality built into the Rx.NET libraries. 

Based on Lee Campbell's 2010 book (kindly donated to the project), it has been re-written to bring it up to date with Rx.NET v6.0, .NET 8.0, and modern cloud native use cases such as IoT and real-time stream data processing.

Introduction to Rx.NET is available [Online](https://introtorx.com/), [on GitHub](Rx.NET/Documentation/IntroToRx/), as [PDF](https://endjincdn.blob.core.windows.net/assets/ebooks/introduction-to-rx-dotnet/introduction-to-rx-dotnet-2nd-edition.pdf), and [EPUB](https://endjincdn.blob.core.windows.net/assets/ebooks/introduction-to-rx-dotnet/introduction-to-rx-dotnet-2nd-edition.epub).

<br clear="left"/>

## Getting the bits

Channel  | Rx | AsyncRx | Ix | System.Linq.Async
--- | --- | --- | --- |--- |
NuGet.org | [![#](https://img.shields.io/nuget/v/System.Reactive.svg)](https://www.nuget.org/packages/System.Reactive/)| [![#](https://img.shields.io/nuget/v/System.Reactive.Async.svg)](https://www.nuget.org/packages/System.Reactive.Async/) | [![#](https://img.shields.io/nuget/v/System.Interactive.svg)](https://www.nuget.org/packages/System.Interactive/) | [![#](https://img.shields.io/nuget/v/System.Linq.Async.svg)](https://www.nuget.org/packages/System.Linq.Async/)
NuGet.org preview (if newer than release) | [![#](https://img.shields.io/nuget/vpre/System.Reactive.svg)](https://www.nuget.org/packages/System.Reactive/) | [![#](https://img.shields.io/nuget/vpre/System.Reactive.Async.svg)](https://www.nuget.org/packages/System.Reactive.Async/) | [![#](https://img.shields.io/nuget/vpre/System.Interactive.svg)](https://www.nuget.org/packages/System.Interactive/) | [![#](https://img.shields.io/nuget/vpre/System.Linq.Async.svg)](https://www.nuget.org/packages/System.Linq.Async/)
Build | [![Build Status](https://dev.azure.com/dotnet/Rx.NET/_apis/build/status/Rx.NET-CI?branchName=main)](https://dev.azure.com/dotnet/Rx.NET/_build/latest?definitionId=9) | [![Build Status](https://dev.azure.com/dotnet/Rx.NET/_apis/build/status/AsyncRx.NET-CI?branchName=main)](https://dev.azure.com/dotnet/Rx.NET/_build/latest?definitionId=191) | [![Build Status](https://dev.azure.com/dotnet/Rx.NET/_apis/build/status/Ix.NET-CI?branchName=main)](https://dev.azure.com/dotnet/Rx.NET/_build/latest?definitionId=28) | Built as part of Ix
[Azure<br>Artifacts](https://dev.azure.com/dotnet/Rx.NET/_packaging?_a=feed&feed=RxNet) | [![System.Reactive package in RxNet feed in Azure Artifacts](https://azpkgsshield.azurevoodoo.net/dotnet/Rx.NET/RxNet/System.Reactive)](https://dev.azure.com/dotnet/Rx.NET/_packaging?_a=package&feed=5afc77bd-23b4-46f8-b725-40ebedab630c&package=3c02dce4-f7e9-43ec-a014-28ea9fc46f82&preferRelease=true) | [![System.Reactive.Async package in RxNet feed in Azure Artifacts](https://azpkgsshield.azurevoodoo.net/dotnet/Rx.NET/RxNet/System.Reactive.Async)](https://dev.azure.com/dotnet/Rx.NET/_artifacts/feed/RxNet/NuGet/System.Reactive.Async/&preferRelease=true) | [![System.Interactive package in RxNet feed in Azure Artifacts](https://azpkgsshield.azurevoodoo.net/dotnet/Rx.NET/RxNet/System.Interactive)](https://dev.azure.com/dotnet/Rx.NET/_packaging?_a=package&feed=5afc77bd-23b4-46f8-b725-40ebedab630c&package=a3311bc0-c6ea-4460-bea8-b65d633e2583&preferRelease=true) | [![System.Linq.Async package in RxNet feed in Azure Artifacts](https://azpkgsshield.azurevoodoo.net/dotnet/Rx.NET/RxNet/System.Linq.Async)](https://dev.azure.com/dotnet/Rx.NET/_artifacts/feed/RxNet/NuGet/System.Linq.Async/&preferRelease=true)
Release history | [ReleaseHistory](Rx.NET/Documentation/ReleaseHistory/) | [ReleaseHistory](Ix.NET/Documentation/ReleaseHistory/)| [ReleaseHistory](Ix.NET/Documentation/ReleaseHistory/)

For nightly builds, configure NuGet to use this feed: `https://pkgs.dev.azure.com/dotnet/Rx.NET/_packaging/RxNet/nuget/v3/index.json`

### Join the conversation

Catch us in the #rxnet channel over at https://reactivex.slack.com/

## A Brief Introduction to Rx

In this digital age, live data streams are ubiquitous. Financial applications depend on a swift response to timely information. Computer networks have always been able to provide extensive information about their health and operation. Utility companies such as water providers have vast numbers of devices monitoring their operations. User interface and game building frameworks report user interactions in great detail. Delivery vans continuously report their progress. Aircraft provide performance telemetry to detect potential maintenance issues before they become serious problems, and cars are now starting to do the same. Many of us wear or carry devices that track our physical activity and even [vital signs](https://www.youtube.com/watch?v=6yjl_h7-WYA&t=2443s). And the improvements in machine learning have enriched the insights that can be derived from the ever-increasing volume and variety of live data.

But despite being so widespread, live information streams have always been something of a second class citizen. Almost all programming languages have some innate way to work with lists of data (e.g., arrays), but these mechanisms tend to presume that the relevant data is already sitting in memory, ready for us to work with it. What's missing is the liveness—the fact that an information source might produce new data at any moment, on its own schedule.

Rx elevates the support for live streams of information to the same level as we expect for things like arrays. Here's an example:

```cs
var bigTrades =
    from trade in trades
    where trade.Volume > 1_000_000
    select trade;
```

This uses C#'s LINQ feature to filter `trades` down to those entities with a volume greater than one million. This query expression syntax is just a shorthand for method calls, so we could also write it this way:

```cs
var bigTrades = trades.Where(trade => trade.Volume > 1_000_000);
```

The exact behaviour of these two (equivalent) code snippets depends on what type `trades` has. If it were a `IEnumerable<Trade>`, then this query would just iterate through the list, and `bigTrades` would be an enumerable sequence containing just the matching objects. If `trades` were an object representing a database table (e.g., an [Entity Framework](https://learn.microsoft.com/en-us/ef/core/) [DbSet](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbset-1), this would be translated into a database query. But if we're using Rx, `trades` would be an `IObservable<Trade>`, an object reporting live events as they happen. And `bigTrades` would also be an `IObservable<Trade>`, reporting only those trades with a volume over a million. We can provide Rx with a callback to be invoked each time an observable source has something for us:

```cs
bigTrades.Subscribe(t => Console.WriteLine($"{t.Symbol}: trade with volume {t.Volume}"));
```

The two key features of Rx are:

* a clearly defined way to represent and handle live sequences of data ([`IObservable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.iobservable-1))
* a set of operators (such as the `Where` operator just shown) enabling event processing logic to be expressed declaratively

Rx has been particularly successfully applied in user interfaces. (This is also true outside of .NET—[RxJS](https://rxjs.dev/) is a JavaScript spin-off of Rx, and it is very popular in user interface code.) The https://github.com/reactiveui/reactiveui makes deep use of Rx to support .NET UI development.

Ian Griffiths presented a concise 60 minute overview of [Reactive Extensions for .NET](https://endjin.com/what-we-think/talks/reactive-extensions-for-dotnet) at the dotnetsheff meetup in 2020. More videos are available on the [Rx playlist](https://www.youtube.com/playlist?list=PLJt9xcgQpM60Fz20FIXBvj6ku4a7WOLGb).

## AsyncRx.Net

Although Rx is a natural way to model asynchronous processes, its original design presumed that code acting on notifications would run synchronously. This is because Rx's design predates C#'s `async`/`await` language features. So although Rx offer adapters that can convert between [`IObservable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.iobservable-1) and [`Task<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1), there were certain cases where `async` was not an option.

AsyncRx.Net lifts this restriction by defining `IAsyncObservable<T>`. This enables observers to use asynchronous code. For example, if `bigTrades` were an `IAsyncObservable<Trade>` we could write this:

```cs
bigTrades.Subscribe(async t => await bigTradeStore.LogTradeAsync(t));
```

AsyncRx.Net is currently in preview.

## Interactive Extensions

Rx defines all the standard LINQ operators available for other providers, but it also adds numerous additional operators. For example, it defines `Scan`, which performs the same basic processing as the standard [`Aggregate`](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.aggregate) operator, but instead of producing a single result after processing every element, it produces a sequence containing the aggregated value after every single step. (For example, if the operation being aggregated is addition, `Aggregate` would return the sum total as a single output, whereas `Scan` would produce a running total for each input. Given a sequence `[1,2,3]`, `Aggregate((a, x) => a + x)` produces just `6`, whereas `Scan` would produce `[1,3,6]`.)

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

This project is part of the [.NET Foundation](http://www.dotnetfoundation.org/projects) along with other
projects like [the .NET Runtime](https://github.com/dotnet/runtime/). The .NET Foundation provides this project with DevOps infrastructure to compile, test, sign and package this complex solution which has over 100 million downloads. It also provides conservatorship enabling the project to pass from maintainer to maintainer, enabling continuity for the community.

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
        <p><a href="https://endjin.com/who-we-are/our-people/ian-griffiths/">Ian's blog on endjin.com</a>
      </td>
      <td align="center" valign="top">
        <img width="150" height="150" src="https://github.com/HowardvanRooijen.png?s=150">
        <br>
        <a href="https://github.com/HowardvanRooijen">Howard van Rooijen</a>
        <p>Winchester, UK</p>
        <p><a href="https://endjin.com/who-we-are/our-people/howard-van-rooijen/">Howard's blog on endjin.com</a>
      </td>
  </tbody>
</table>

Rx has been around for roughly a decade and a half, so we owe a great deal to its creators, and the many people who have worked on it since. See the [AUTHORS.txt](AUTHORS.txt) for a full list.

## Roadmap

As part of .NET Conf 2023, Ian Griffiths provided an update on the efforts to [modernize Rx.NET for v6.0 and the plans to for v7.0](https://endjin.com/what-we-think/talks/modernizing-reactive-extensions-for-dotnet). 

For more information, see the following discussions:

- [Future Rx.NET Packaging](https://github.com/dotnet/reactive/discussions/2038)
- [Rx.NET v6.0 & v7.0 high-level plan](https://github.com/dotnet/reactive/discussions/1868)

We have set out a [roadmap](Rx.NET/Documentation/Rx-Roadmap-2023.md) explaining our medium term plans for ongoing development of Rx. This diagram illustrates our view of the platforms on which Rx is used, and the planned support lifecycles for these various targets:

![The support lifecycle for various .NET platforms, represented as a set of timelines, showing the published plans for widely used versions that are current as of 2023, with a particular focus on which versions will be current as of November 2023. The top section of the diagram shows .NET releases starting with .NET 6.0 being released in November 2021, and shows for each subsequent release occurring in November of each subsequent year, up as far as .NET 13.0 in November 2028. It also shows that even-numbered releases are Long Term Support (LTS for short) releases, supported for 3 years, while odd-numbered releases are supported only for 18 months. The section beneath this shows that .NET Framework versions 4.7.2, 4.8.0, and 4.8.1 will all be in support as of November 2023, and will continue to be in support beyond the timescale covered by this diagram, i.e., beyond November 2028. The section beneath this shows the release plan for MAUI, starting with version 8.0 on November 2023, and subsequent releases at the same time each subsequent year, up to version 13.0 in November 2028. The diagram shows that each of these versions is supported for only 18 months. Beneath this is are two lines showing Xamarin iOS 16.0, and Xamarin Android 13.0 support being active on November 2023, and running for 18 months. Beneath this is a line showing UWP version 10.0.16299 support being active on November 2023, and running beyond the timescale covered by the diagram. Beneath this is a section showing that Unity 2021 was released in 2021, and will go out of support near the end of 2023, and it shows a Unity 2022 release labelled as "Release soon," with a release date somewhere in the middle of 2023. The bottom of the diagram shows the endjin logo, and endjin's corporate motto: "we help small teams achieve big things."](Rx.NET/Documentation/RX-Platform-Support-Roadmap.png ".NET Platform Support Roadmap")
