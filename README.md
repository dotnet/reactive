Reactive Extensions
======================

Channel  | Rx | Ix |
-------- | :------------: | :-------------: |
AppVeyor<br>([home](https://ci.appveyor.com/project/dotnetfoundation/rx-net)) | ![#](https://img.shields.io/appveyor/ci/dotnetfoundation/rx-net/master.svg) | ![#](https://img.shields.io/appveyor/ci/dotnetfoundation/rx-net/master.svg)
NuGet.org | [![#](https://img.shields.io/nuget/v/System.Reactive.svg)](https://www.nuget.org/packages/System.Reactive/) | [![#](https://img.shields.io/nuget/v/System.Interactive.svg)](https://www.nuget.org/packages/System.Interactive/)
MyGet<br>([gallery](https://dotnet.myget.org/gallery/rx)) | ![#](https://img.shields.io/dotnet.myget/rx/vpre/System.Reactive.svg)<br>![#](https://img.shields.io/dotnet.myget/rx/v/System.Reactive.svg) | ![#](https://img.shields.io/dotnet.myget/rx/vpre/System.Interactive.svg)<br>![#](https://img.shields.io/dotnet.myget/rx/v/System.Interactive.svg)  

### Join the conversation

Catch us in the #rxnet channel over at http://reactiveui.net/slack

### Get nightly builds
- NuGet v3 feed url (VS 2015+): `https://dotnet.myget.org/F/rx/api/v3/index.json`

### v4.0 changes
Due to the [overwhelming](https://github.com/Reactive-Extensions/Rx.NET/issues/299) [pain](https://github.com/Reactive-Extensions/Rx.NET/issues/305) that fixing [#205 - Implement assembly version strategy](https://github.com/Reactive-Extensions/Rx.NET/issues/205) caused, we have refactored the libraries into a single library `System.Reactive`. To prevent breaking existing code that references the v3 libraries, we have facades with TypeForwarders to the new assembly. If you have a reference to a binary built against v3.0, then use the new `System.Reactive.Compatibility` package. 

#### Supported Platforms
Rx 4.0 supports the following platforms

- .NET Framework 4.5+
- .NET Standard 1.3+ (including .NET Core, Xamarin and others)
- UWP

Notably, Windows 8, Windows Phone 8 and legacy PCL libraries are no longer supported. 

### v3.0 breaking changes
The NuGet packages have changed their package naming in the move from v2.x.x to v3.0.0
 * ~~`Rx-Main`~~ is now [`System.Reactive`](https://www.nuget.org/packages/System.Reactive/)
 * ~~`Rx-Core`~~ is now [`System.Reactive.Core`](https://www.nuget.org/packages/System.Reactive.Core/)
 * ~~`Rx-Interfaces`~~  is now [`System.Reactive.Interfaces`](https://www.nuget.org/packages/System.Reactive.Interfaces/)
 * ~~`Rx-Linq`~~  is now [`System.Reactive.Linq`](https://www.nuget.org/packages/System.Reactive.Linq/)
 * ~~`Rx-PlatformServices`~~  is now [`System.Reactive.PlatformServices`](https://www.nuget.org/packages/System.Reactive.PlatformServices/)
 * ~~`Rx-Testing`~~  is now [`Microsoft.Reactive.Testing`](https://www.nuget.org/packages/Microsoft.Reactive.Testing/)

This brings the NuGet package naming in line with NuGet guidelines and also the dominant namespace in each package.
The strong name key has also changed, which is considered a breaking change.
However, there are no expected API changes, therefore, once you make the NuGet change, no code changes should be necessary.

A Brief Intro
-------------------

The Reactive Extensions (Rx) is a library for composing asynchronous and event-based programs using observable sequences and LINQ-style query operators. Using Rx, developers *__represent__* asynchronous data streams with [Observables](http://msdn.microsoft.com/library/dd990377.aspx), *__query__* asynchronous data streams using [LINQ operators](http://msdn.microsoft.com/en-us/library/hh242983.aspx), and *__parameterize__* the concurrency in the asynchronous data streams using [Schedulers](http://msdn.microsoft.com/en-us/library/hh242963.aspx). Simply put, Rx = Observables + LINQ + Schedulers.

Whether you are authoring a traditional desktop or web-based application, you have to deal with asynchronous and event-based programming from time to time. Desktop applications have I/O operations and computationally expensive tasks that might take a long time to complete and potentially block other active threads. Furthermore, handling exceptions, cancellation, and synchronization is difficult and error-prone.

Using Rx, you can represent multiple asynchronous data streams (that come from diverse sources, e.g., stock quote, tweets, computer events, web service requests, etc.), and subscribe to the event stream using the `IObserver<T>` interface. The `IObservable<T>` interface notifies the subscribed `IObserver<T>` interface whenever an event occurs.

Because observable sequences are data streams, you can query them using standard LINQ query operators implemented by the Observable extension methods. Thus you can filter, project, aggregate, compose and perform time-based operations on multiple events easily by using these standard LINQ operators. In addition, there are a number of other reactive stream specific operators that allow powerful queries to be written.  Cancellation, exceptions, and synchronization are also handled gracefully by using the extension methods provided by Rx.

Rx complements and interoperates smoothly with both synchronous data streams (`IEnumerable<T>`) and single-value asynchronous computations (`Task<T>`) as the following diagram shows:


<table>
   <th></th><th>Single return value</th><th>Multiple return values</th>
   <tr>
      <td>Pull/Synchronous/Interactive</td>
      <td>T</td>
      <td>IEnumerable&lt;T&gt;</td>
   </tr>
   <tr>
      <td>Push/Asynchronous/Reactive</td>
      <td>Task&lt;T&gt;</td>
      <td>IObservable&lt;T&gt;</td>
   </tr>
</table>

Additional documentation, video, tutorials and HOL are available on MSDN.

Flavors of Rx
---------------

* __Rx.NET__: *(this repository)* The Reactive Extensions (Rx) is a library for composing asynchronous and event-based programs using observable sequences and LINQ-style query operators.
* [RxJS 4](https://github.com/Reactive-Extensions/RxJS) / [RxJS 5 beta](https://github.com/ReactiveX/rxjs): The Reactive Extensions for JavaScript (RxJS) is a library for composing asynchronous and event-based programs using observable sequences and LINQ-style query operators in JavaScript which can target both the browser and Node.js.
* [RxJava](https://github.com/ReactiveX/RxJava): Reactive Extensions for the JVM – a library for composing asynchronous and event-based programs using observable sequences for the Java VM.
* [RxScala](https://github.com/ReactiveX/RxScala): Reactive Extensions for Scala – a library for composing asynchronous and event-based programs using observable sequences
* [RxCpp](https://github.com/Reactive-Extensions/RxCpp): The Reactive Extensions for Native (RxCpp) is a library for composing asynchronous and event-based programs using observable sequences and LINQ-style query operators in both C and C++.
* [Rx.rb](http://rxrb.codeplex.com): A prototype implementation of Reactive Extensions for Ruby (Rx.rb).
* [RxPy](https://github.com/Reactive-Extensions/RxPy): The Reactive Extensions for Python 3 (Rx.Py) is a set of libraries to compose asynchronous and event-based programs using observable collections and LINQ-style query operators in Python 3.


Interactive Extensions
-----------------------
* __Ix.NET__: *(included in this repository)* The Interactive Extensions (Ix) is a .NET library which extends LINQ to Objects to provide many of the operators available in Rx but targeted for IEnumerable<T>.
* [IxJS](https://github.com/ReactiveX/IxJS): An implementation of LINQ to Objects and the Interactive Extensions (Ix) in JavaScript.
* [IxCpp](https://github.com/Reactive-Extensions/RxCpp): An implementation of LINQ for Native Developers in C++

Applications
-------------
* [Tx](https://github.com/Reactive-Extensions/Tx): a set of code samples showing how to use LINQ to events, such as real-time standing queries and queries on past history from trace and log files, which targets ETW, Windows Event Logs and SQL Server Extended Events.
* [LINQ2Charts](http://linq2charts.codeplex.com): an example for Rx bindings.  Similar to existing APIs like LINQ to XML, it allows developers to use LINQ to create/change/update charts in an easy way and avoid having to deal with XML or other underneath data structures. We would love to see more Rx bindings like this one.

Contributing
------------------

### Source code

* Clone the sources: `git clone https://github.com/Reactive-Extensions/Rx.NET.git`
* [Building, testing and debugging the sources](https://github.com/Reactive-Extensions/Rx.NET/wiki/Building%20Testing%20and%20Debugging)

### Contribute!

Some of the best ways to contribute are to try things out, file bugs, and join in design conversations.

* [How to Contribute](https://github.com/Reactive-Extensions/Rx.NET/wiki/Contributing-Code)
* [Pull requests](https://github.com/Reactive-Extensions/Rx.NET/pulls): [Open](https://github.com/Reactive-Extensions/Rx.NET/pulls?q=is%3Aopen+is%3Apr)/[Closed](https://github.com/Reactive-Extensions/Rx.NET/pulls?q=is%3Apr+is%3Aclosed)

Looking for something to work on? The list of [up for grabs issues](https://github.com/Reactive-Extensions/Rx.NET/issues?q=is%3Aopen+is%3Aissue+label%3A%22Up+for+Grabs%22) is a great place to start.

This project has adopted a code of conduct adapted from the [Contributor Covenant](http://contributor-covenant.org/) to clarify expected behavior in our community. This code of conduct has been [adopted by many other projects](http://contributor-covenant.org/adopters/). For more information see the [Code of conduct](http://www.dotnetfoundation.org/code-of-conduct).

<h2 align="center">.NET Foundation</h2>

System.Reactive is part of the [.NET Foundation](https://www.dotnetfoundation.org/). Other projects that are associated with the foundation include the Microsoft .NET Compiler Platform ("Roslyn") as well as the Microsoft ASP.NET family of projects, Microsoft .NET Core & Xamarin Forms.

<h2 align="center">Core Team</h2>

<table>
  <tbody>
    <tr>
      <td align="center" valign="top">
        <img width="150" height="150" src="https://github.com/ghuntley.png?s=150">
        <br>
        <a href="https://github.com/ghuntley">Geoffrey Huntley</a>
        <p>Sydney, Australia</p>
      </td>
      <td align="center" valign="top">
        <img width="150" height="150" src="https://github.com/onovotny.png?s=150">
        <br>
        <a href="https://github.com/onovotny">Oren Novotny</a>
        <p>New York, USA</p>
      </td>
     </tr>
  </tbody>
</table>


<h2 align="center">Sponsorship</h2>

The core team members, System.Reactive contributors and contributors in the ecosystem do this open source work in their free time. If you use System.Reactive for serious tasks, and you'd like us to invest more time on it, please donate. This project increases your income/productivity too. It makes development and applications faster and it reduces the required bandwidth.

This is how we use the donations:

* Allow the core team to work on System.Reactive
* Thank contributors if they invested a large amount of time in contributing
* Support projects in the ecosystem that are of great value for users
* Support projects that are voted most (work in progress)
* Infrastructure cost
* Fees for money handling

<h2 align="center">Backers</h2>

[Become a backer](https://opencollective.com/reactiveui#backer) and get your image on our README on Github with a link to your site.

<a href="https://opencollective.com/reactiveui/backer/0/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/0/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/1/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/1/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/2/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/2/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/3/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/3/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/4/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/4/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/5/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/5/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/6/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/6/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/7/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/7/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/8/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/8/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/9/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/9/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/10/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/10/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/11/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/11/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/12/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/12/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/13/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/13/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/14/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/14/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/15/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/15/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/16/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/16/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/17/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/17/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/18/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/18/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/19/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/19/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/20/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/20/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/21/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/21/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/22/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/22/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/23/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/23/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/24/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/24/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/25/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/25/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/26/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/26/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/27/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/27/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/28/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/28/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/backer/29/website" target="_blank"><img src="https://opencollective.com/reactiveui/backer/29/avatar.svg"></a>

<h2 align="center">Sponsors</h2>

[Become a sponsor](https://opencollective.com/reactiveui#sponsor) and get your logo on our README on Github with a link to your site.

<a href="https://opencollective.com/reactiveui/sponsor/0/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/0/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/1/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/1/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/2/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/2/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/3/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/3/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/4/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/4/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/5/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/5/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/6/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/6/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/7/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/7/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/8/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/8/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/9/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/9/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/10/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/10/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/11/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/11/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/12/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/12/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/13/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/13/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/14/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/14/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/15/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/15/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/16/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/16/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/17/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/17/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/18/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/18/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/19/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/19/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/20/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/20/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/21/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/21/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/22/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/22/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/23/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/23/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/24/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/24/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/25/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/25/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/26/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/26/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/27/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/27/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/28/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/28/avatar.svg"></a>
<a href="https://opencollective.com/reactiveui/sponsor/29/website" target="_blank"><img src="https://opencollective.com/reactiveui/sponsor/29/avatar.svg"></a>
