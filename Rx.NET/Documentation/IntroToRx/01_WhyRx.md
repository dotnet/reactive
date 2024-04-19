# PART 1 - Getting started

Rx is a .NET library for processing event streams. Why might you want that?

## Why Rx?

Users want timely information. If you're waiting for a parcel to arrive, live reports of the delivery van's progress give you more freedom than a suspect 2 hour delivery window. Financial applications depend on continuous streams of up-to-date data. We expect our phones and computers to provide us with all sorts of important notifications. And some applications simply can't work without live information. Online collaboration tools and multiplayer games absolutely depend on the rapid distribution and delivery of data.

In short, our systems need to react when interesting things happen.

Live information streams are a basic, ubiquitous element of computer systems. Despite this, they are often a second class citizen in programming languages. Most languages support sequences of data through something like an array, which presumes that the data is sitting in memory ready for our code to read at its leisure. If your application deals with events, arrays might work for historical data, but they aren't a good way to represent events that occur while the application is running. And although streamed data is a pretty venerable concept in computing, it tends to be clunky, with the abstractions often surfaced through APIs that are poorly integrated with our programming language's type system.

This is bad. Live data is critical to a wide range of applications. It should be as easy to work with as lists, dictionaries, and other collections.

The [Reactive Extensions for .NET](https://github.com/dotnet/reactive) (Rx.NET or Rx for short, available as the [`System.Reactive` NuGet package](https://www.nuget.org/packages/System.Reactive/)) elevate live data sources to first class citizens. Rx does not require any special programming language support. It exploits .NET's type system to represent streams of data in a way that .NET languages such as C#, F#, and VB.NET can all work with as naturally as they use collection types.

(A brief grammatical aside: although the phrase "Reactive Extensions" is plural, when we reduce it to just Rx.NET or Rx, we treat it as a singular noun. This is inconsistent, but saying "Rx are..." sounds plain weird.)

For example, C# offers integrated query features that we might use to find all of the entries in a list that meet some criteria. If we have some `List<Trade> trades` variable, we might write this:

```csharp
var bigTrades =
    from trade in trades
    where trade.Volume > 1_000_000;
```

With Rx, we could use this exact same code with live data. Instead of being a `List<Trade>`, the `trades` variable could be an `IObservable<Trade>`. `IObservable<T>` is the fundamental abstraction in Rx. It is essentially a live version of `IEnumerable<T>`. In this case, `bigTrades` would also be an `IObservable<Trade>`, a live data source able to notify us of all trades whose `Volume` exceeds one million. Crucially, it can report each such trade immediately—this is what we mean by a 'live' data source.

Rx is a powerfully productive development tool. It enables developers to work with live event streams using language features familiar to all .NET developers. It enables a declarative approach that often allows us to express complex behaviour more elegantly and with less code than would be possible without Rx.

Rx builds on LINQ (Language Integrated Query). This enables us to use the query syntax shown above (or you can use the explicit function call approach that some .NET developers prefer). LINQ is widely used in .NET both for data access (e.g., in Entity Framework Core), but also for working with in-memory collections (with LINQ to Objects), meaning that experienced .NET developers will tend to feel at home with Rx. Crucially, LINQ is a highly composable design: you can connect operators together in any combination you like, expressing potentially complex processing in a straightforward way. This composability arises from the mathematical foundations of its design, but although you can learn about this aspect of LINQ if you want, it's not a prerequisite: developers who aren't interested in the mathematics behind it can just enjoy the fact that LINQ providers such as Rx provide a set of building blocks that can be plugged together in endless different ways, and it all just works.

LINQ has proven track record of handling very high volumes of data. Microsoft has used it extensively in the internal implementation of some of their systems, including services that support tens of millions of active users.

## When is Rx appropriate?

Rx is designed for processing sequences of events, meaning that it suits some scenarios better than others. The next sections describe some of these scenarios, and also cases in which it is a less obvious match but still worth considering. Finally, we describe some cases in which it is possible to use Rx but where alternatives are likely to be better.

### Good Fit with Rx

Rx is well suited to representing events that originate from outside of your code, and which your application needs to respond to, such as:

- Integration events like a broadcast from a message bus, or a push event from WebSockets API, or a message received via MQTT or other low latency middleware like [Azure Event Grid](https://azure.microsoft.com/en-gb/products/event-grid/), [Azure Event Hubs](https://azure.microsoft.com/en-gb/products/event-hubs/) and [Azure Service Bus](https://azure.microsoft.com/en-gb/products/service-bus/), or a non-vendor specific representation such as [cloudevents](https://cloudevents.io/)
- Telemetry from monitoring devices such as a flow sensor in a water utility's infrastructure, or the monitoring and diagnostic features in a broadband provider's networking equipment
- Location data from mobile systems such as [AIS](https://github.com/ais-dotnet/) messages from ships, or automotive telemetry
- Operating system events such as filesystem activity, or WMI events
- Road traffic information, such as notifications of accidents or changes in average speed
- Integration with a [Complex Event Processing (CEP)](https://en.wikipedia.org/wiki/Complex_event_processing) engine
- UI events such as mouse movement or button clicks

Rx is also good way to model domain events. These may occur as a result of some of the events just described, but after processing them to produce events that more directly represent application concepts. These might include:

- Property or state changes on domain objects such as "Order Status Updated", or "Registration Accepted"
- Changes to collections of domain objects, such as "New Registration Created"

Events might also represent insights derived from incoming events (or historical data being analyzed at a later date) such as:

- A broadband customer might have become an unwitting participant in a DDoS attack
- Two ocean-going vessels have engaged in a pattern of movement often associated with illegal activity (e.g., travelling closely alongside one another for an extended period, long enough to transfer cargo or people, while far out at sea)
- [CNC](https://en.wikipedia.org/wiki/Numerical_control) [Milling Machine](https://en.wikipedia.org/wiki/Milling_(machining)) MFZH12's number 4 axis bearing is exhibiting signs of wear at a significantly higher rate than the nominal profile
- If the user wants to arrive on time at their meeting half way across town, the current traffic conditions suggest they should leave in the next 10 minutes

These three sets of examples show how applications might progressively increase the value of the information as they process events. We start with raw events, which we then enhance to produce domain-specific events, and we then perform analysis to produce notifications that the application's users will really care about. Each stage of processing increases the value of the messages that emerge. Each stage will typically also reduce the volume of messages. If we presented the raw events in the first category directly to users, they might be overwhelmed by the volume of messages, making it impossible to spot the important events. But if we only present them with notifications when our processing has detected something important, this will enable them to work more efficiently and accurately, because we have dramatically improved the signal to noise ratio.

The [`System.Reactive` library](https://www.nuget.org/packages/System.Reactive) provides tools for building exactly this kind of value-adding process, in which we tame high-volume raw event sources to produce high-value, live, actionable insights. It provides a suite of operators that enable our code to express this kind of processing declaratively, as you'll see in subsequent chapters.

Rx is also well suited for introducing and managing concurrency for the purpose of _offloading_. 
That is, performing a given set of work concurrently, so that the thread that detected an event doesn't also have to be the thread that handles that event. 
A very popular use of this is maintaining a responsive UI. (UI event handling has become _such_ a popular use of Rx—both in .NET. but also in [RxJS](https://rxjs.dev/), which originated as an offshoot of Rx.NET—that it would be easy to think that this is what it's for. But its success there should not blind us to its wider applicability.)

You should consider using Rx if you have an existing `IEnumerable<T>` that is attempting to model live events. 
While `IEnumerable<T>` _can_ model data in motion (by using lazy evaluation like `yield return`), there's a problem. If the code consuming the collection has reached the point where it wants the next item (e.g., because a `foreach` loop has just completed an iteration) but no item is yet available, the `IEnumerable<T>` implementation would have no choice but to block the calling thread in its `MoveNext` until such time as data is available, which can cause scalability problems in some applications. Even in cases where thread blocking is acceptable (or if you use the newer `IAsyncEnumerable<T>`, which can take advantage of C#'s `await foreach` feature to avoid blocking a thread in these cases) `IEnumerable<T>` and `IAsyncEnumerable<T>` are misleading types for representing live information sources. These interfaces represent a 'pull' programming model: code asks for the next item in the sequence. Rx is a more natural choice for modelling information sources that naturally produce information on their own schedule.

### Possible Fit with Rx

Rx can be used to represent asynchronous operations. .NET's `Task` or `Task<T>` effectively represent a single event, and `IObservable<T>` can be thought if as a generalization of this to a sequence of events. (The relationship between, say, `Task<int>` and `IObservable<int>` is similar to the relationship between `int` and `IEnumerable<int>`.)

This means that there are some scenarios that can be dealt with either using tasks and the `async` keyword or through Rx. If at any point in your processing you need to deal with multiple values as well as single ones, Rx can do both; tasks don't handle multiple items so well. You can have a `Task<IEnumerable<int>>`, which enables you to `await` for a collection, and that's fine if all the items in the collection can be collected in a single step. The limitation with this is that once the task has produced its `IEnumerable<int>` result, your `await` has completed, and you're back to non-asynchronous iteration over that `IEnumerable<int>`. If the data can't be fetched in a single step—perhaps the `IEnumerable<int>` represents data from an API in which results are fetched in batches of 100 items at a time—its `MoveNext` will have to block your thread every time it needs to wait.

For the first 5 years of its existence, Rx was arguably the best way to represent collections that wouldn't necessarily have all the items available immediately. However, the introduction of [`IAsyncEnumerable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1) in .NET Core 3.0 and C# 8 provided a way to handle sequences while remaining in the world of `async`/`await` (and the [`Microsoft.Bcl.AsyncInterfaces` NuGet package](https://www.nuget.org/packages/Microsoft.Bcl.AsyncInterfaces/) makes this available on .NET Framework and .NET Standard 2.0). So the choice to use Rx to now tends to boil down to whether a 'pull' model (exemplified by `foreach` or `await foreach`) or a 'push' model (in which code supplies callbacks to be invoked by the event source when items become available) is a better fit for the concepts being modelled.

Another related feature that was added .NET since Rx first appears is [channels](https://learn.microsoft.com/en-us/dotnet/core/extensions/channels). These allow a source to produce object and a consumer to process them, so there's an obvious superficial similarity to Rx. However, a distinguishing feature of Rx is its support for composition with an extensive set of operators, something with no direct equivalent in channels. Channels on the other hand provide more options for adapting to variations in production and consumption rates.

Earlier, I mentioned _offloading_: using Rx to push work onto other threads. Although this technique can enable Rx to introduce and manage concurrency for the purposes of _scaling_ or performing _parallel_ computations, other dedicated frameworks like [TPL (Task Parallel Library) Dataflow](https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/dataflow-task-parallel-library) or [PLINQ](https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/introduction-to-plinq) are more appropriate for performing parallel compute intensive work. However, TPL Dataflow offers some integration with Rx through its [`AsObserver`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.dataflow.dataflowblock.asobserver) and [`AsObservable`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.dataflow.dataflowblock.asobservable) extension methods. So it is common to use Rx to integrate TPL Dataflow with the rest of your application.

### Poor Fit with Rx

Rx's `IObservable<T>` is not a replacement for `IEnumerable<T>` or `IAsyncEnumerable<T>`. It would be a mistake to take something that is naturally pull based and force it to be push based.

Also, there are some situations in which the simplicity of Rx's programming model can work against you. For example, some message queuing technologies such as MSMQ are by definition sequential, and thus might look like a good fit for Rx. However, they are often chosen for their transaction handling support. Rx does not have any direct way to surface transaction semantics, so in scenarios that require this you might be better off just working directly with the relevant technology's API. (That said, [Reaqtor](https://reaqtive.net/) adds durability and persistence to Rx, so you might be able to use that to integrate these kinds of queueing systems with Rx.)

By choosing the best tool for the job your code should be easier to maintain, it will likely provide better performance and you will probably get better support.

## Rx in action

You can get up and running with a simple Rx example very quickly. If you have the .NET SDK installed, you can run the following at a command line:

```ps1
mkdir TryRx
cd TryRx
dotnet new console
dotnet add package System.Reactive
```

Alternatively, if you have Visual Studio installed, create a new .NET Console project, and then use the NuGet package manager to add a reference to `System.Reactive`.

This code creates an observable source (`ticks`) that produces an event once every second. The code also passes a handler to that source that writes a message to the console for each event:

```csharp
using System.Reactive.Linq;

IObservable<long> ticks = Observable.Timer(
    dueTime: TimeSpan.Zero,
    period: TimeSpan.FromSeconds(1));

ticks.Subscribe(
    tick => Console.WriteLine($"Tick {tick}"));

Console.ReadLine();
```

If this doesn't seem very exciting, it's because it's about as basic an example as it's possible to create, and at its heart, Rx has a very simple programming model. The power comes from composition—we can use the building blocks in the `System.Reactive` library to describe the processing that will takes us from raw, low-level events to high-value insights. But to do that, we must first understand [Rx's key types, `IObservable<T>` and `IObserver<T>`](02_KeyTypes.md).
