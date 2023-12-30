# Key types

Rx is a powerful framework that can greatly simplify code that responds to events. But to write good Reactive code you have to understand the basic concepts. The fundamental building block of Rx is an interface called `IObservable<T>`. Understanding this, and its counterpart `IObserver<T>`, is the key to success with Rx.

The preceding chapter showed this LINQ query expression as the first example:

```csharp
var bigTrades =
    from trade in trades
    where trade.Volume > 1_000_000;
```

Most .NET developers will be familiar with [LINQ](https://learn.microsoft.com/en-us/dotnet/csharp/linq/) in at least one of its many popular forms such as [LINQ to Objects](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/linq-to-objects), or [Entity Framework Core queries](https://learn.microsoft.com/en-us/ef/core/querying/). Most LINQ implementations allow you to query _data at rest_. LINQ to Objects works on arrays or other collections, and LINQ queries in Entity Framework Core run against data in a database, but Rx is different: it offers the ability to define queries over live event streams—what you might call _data in motion_.

If you don't like the query expression syntax, you can write exactly equivalent code by invoking LINQ operators directly:

```csharp
var bigTrades = trades.Where(trade => trade.Volume > 1_000_000);
```

Whichever style we use, this is the LINQ way of saying that we want `bigTrades` to have just those items in `trades` where the `Volume` property is greater than one million.

We can't tell exactly what these examples do because we can't see the type of the `trades` or `bigTrades` variables. The meaning of this code is going to vary greatly depending on these types. If we were using LINQ to objects, these would both likely be `IEnumerable<Trade>`. That would mean that these variables both referred to objects representing collections whose contents we could enumerate with a `foreach` loop. This would represent _data at rest_, data that our code could inspect directly.

But let's make it clear what the code means by being explicit about the type:

```csharp
IObservable<Trade> bigTrades = trades.Where(trade => trade.Volume > 1_000_000);
```

This removes the ambiguity. It is now clear that we're not dealing with data at rest. We're working with an `IObservable<Trade>`. But what exactly is that?

## `IObservable<T>`
    
The [`IObservable<T>` interface](https://learn.microsoft.com/en-us/dotnet/api/system.iobservable-1) represents Rx's fundamental abstraction: a sequence of values of some type `T`. In a very abstract sense, this means it represents the same thing as [`IEnumerable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1). 

The difference is in how code consumes those values. Whereas `IEnumerable<T>` enables code to retrieve values (typically with a `foreach` loop), an `IObservable<T>` provides values when they become available. This distinction is sometimes characterised as _push_ vs _pull_. We can _pull_ values out of an `IEnumerable<T>` by executing a `foreach` loop, but an `IObservable<T>` will _push_ values into our code.

How can an `IObservable<T>` push its values into our code? If we want these values, our code must _subscribe_ to the `IObservable<T>`, which means providing it with some methods it can invoke. In fact, subscription is the only operation an `IObservable<T>` directly supports. Here's the entire definition of the interface:

```csharp
public interface IObservable<out T>
{
    IDisposable Subscribe(IObserver<T> observer);
}
```

You can see [the source for `IObservable<T>` on GitHub](https://github.com/dotnet/runtime/blob/b4008aefaf8e3b262fbb764070ea1dd1abe7d97c/src/libraries/System.Private.CoreLib/src/System/IObservable.cs). Notice that it is part of the .NET runtime libraries, and not the `System.Reactive` NuGet package. `IObservable<T>` represents such a fundamentally important abstraction that it is baked into .NET. (So you might be wondering what the `System.Reactive` NuGet package is for. The .NET runtime libraries define only the `IObservable<T>` and `IObserver<T>` interfaces, and not the LINQ implementation. The `System.Reactive` NuGet package gives us LINQ support, and also deals with threading.)

This interface's only method makes it clear what we can do with an `IObservable<T>`: if we want to receive the events it has to offer, we can _subscribe_ to it. (We can also unsubscribe: the `Subscribe` method returns an `IDisposable`, and if we call `Dispose` on that it cancels our subscription.) The `Subscribe` method requires us to pass in an implementation of `IObserver<T>`, which we will get to shortly.

Observant readers will have noticed that an example in the preceding chapter looks like it shouldn't work. That code created an `IObservable<long>` that produced events once per second, and then it subscribed to it with this code:

```csharp
ticks.Subscribe(
    tick => Console.WriteLine($"Tick {tick}"));
```

That's passing a delegate, and not the `IObserver<T>` that `IObservable<T>.Subscribe` requires. We'll get to `IObserver<T>` shortly, but all that's happening here is that this example is using an extension method from the `System.Reactive` NuGet package:

```csharp
// From the System.Reactive library's ObservableExtensions class
public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext)
```

This is a helper method that wraps a delegate in an implementation of `IObserver<T>` and then passes that to `IObservable<T>.Subscribe`. The effect is that we can write just a simple method (instead of a complete implementation of `IObserver<T>`) and the observable source will invoke our callback each time it wants to supply a value. It's more common to use this kind of helper than to implement Rx's interfaces ourselves.

### Hot and Cold Sources

Since an `IObservable<T>` cannot supply us with values until we subscribe, the time at which we subscribe can be important. Imagine an `IObservable<Trade>` describing trades occurring in some market. If the information it supplies is live, it's not going to tell you about any trades that occurred before you subscribed. In Rx, sources of this kind are described as being _hot_.

Not all sources are _hot_. There's nothing stopping an `IObservable<T>` always supplying the exact same sequence of events to any subscriber no matter when the call to `Subscribe` occurs. (Imagine an `IObservable<Trade>` which, instead of reporting live information, generates notifications based on recorded historical trade data.) Sources where it doesn't matter at all when you subscribe are known as _cold_ sources.

Here are some sources that might be represented as hot observables:

* Measurements from a sensor
* Price ticks from a trading exchange
* An event source that distributes events immediately such as Azure Event Grid
* mouse movements
* timer events
* broadcasts like ESB channels or UDP network packets

And some examples of some sources that might make good cold observables:

* the contents of a collection (such as is returned by the [`ToObservable` extension method for `IEnumerable<T>`](03_CreatingObservableSequences.md#from-ienumerablet))
* a fixed range of values, such as [`Observable.Range`](03_CreatingObservableSequences.md#observablerange) produces
* events generated based on an algorithm, such as [`Observable.Generate`](03_CreatingObservableSequences.md#observablegenerate) produces
* a factory for an asynchronous operation, such as [`FromAsync`](03_CreatingObservableSequences.md#from-task) returns
* events produced by running conventional code such as a loop; you can create such sources with [`Observable.Create`](03_CreatingObservableSequences.md#observablecreate)
* a streaming event provides such as Azure Event Hub or Kafka (or any other streaming-style source which holds onto events from the past to be able to deliver events from a particular moment in the stream; so _not_ an event source in the Azure Event Grid style)

Not all sources are strictly completely _hot_ or _cold_.  For example, you could imagine a slight variation on a live `IObservable<Trade>` where the source always reports the most recent trade to new subscribers. Subscribers can count on immediately receiving something, and will then be kept up to date as new information arrives. The fact that new subscribers will always receive (potentially quite old) information is a _cold_-like characteristic, but it's only that first event that is _cold_. It's still likely that a brand new subscriber will have missed lots of information that would have been available to earlier subscribers, making this source more _hot_ than _cold_.

There's an interesting special case in which a source of events has been designed to enable applications to receive every single event in order, exactly once. Event streaming systems such as Kafka or Azure Event Hub have this characteristic—they retain events for a while, to ensure that consumers don't miss out even if they fall behind from time to time. The standard input (_stdin_) for a process also has this characteristic: if you run a command line tool and start typing input before it is ready to process it, the operating system will hold that input in a buffer, to ensure that nothing is lost. Windows does something similar for desktop applications: each application thread gets a message queue so that if you click or type when it's not able to respond, the input will eventually be processed. We might think of these sources as _cold_-then-_hot_. They're like _cold_ sources in that we won't miss anything just because it took us some time to start receiving events, but once we start retrieving the data, then we can't generally rewind back to the start. So once we're up and running they are more like _hot_ events.

This kind of _cold_-then-_hot_ source can present a problem if we want to attach multiple subscribers. If the source starts providing events as soon as subscription occurs, then that's fine for the very first subscriber: it will receive any events that were backed up waiting for us to start. But if we wanted to attach multiple subscribers, we've got a problem: that first subscriber might receive all the notifications that were sitting waiting in some buffer before we manage to attach the second subscriber. The second subscriber will miss out.

In these cases, we really want some way to rig up all our subscribers before kicking things off. We want subscription to be separate from the act of starting. By default, subscribing to a source implies that we want it to start, but Rx defines a specialised interface that can give us more control: [`IConnectableObservable<T>`](https://github.com/dotnet/reactive/blob/f4f727cf413c5ea7a704cdd4cd9b4a3371105fa8/Rx.NET/Source/src/System.Reactive/Subjects/IConnectableObservable.cs). This derives from `IObservable<T>`, and adds just a single method, `Connect`:

```csharp
public interface IConnectableObservable<out T> : IObservable<T>
{
    IDisposable Connect();
}
```

This is useful in these scenarios where there will be some process that fetches or generates events and we need to make sure we're prepared before that starts.  Because an `IConnectableObservable<T>` won't start until you call `Connect`, it provides you with a way to attach however many subscribers you need before events begin to flow.

The 'temperature' of a source is not necessarily evident from its type. Even when the underlying source is an `IConnectableObservable<T>`, that can often be hidden behind layers of code. So whether a source is hot, cold, or something in between, most of the time we just see an `IObservable<T>`. Since `IObservable<T>` defines just one method, `Subscribe`, you might be wondering how we can do anything interesting with it. The power comes from the LINQ operators that the `System.Reactive` NuGet library supplies.

### LINQ Operators and Composition

So far I've shown only a very simple LINQ example, using the `Where` operator to filter events down to ones that meet certain criteria. To give you a flavour of how we can build more advanced functionality through composition, I'm going to introduce an example scenario.

Suppose you want to write a program that watches some folder on a filesystem, and performs automatic processing any time something in that folder changes. For example, web developers often want to trigger automatic rebuilds of their client side code when they save changes in the editor so they can quickly see the effect of their changes. Filesystem changes often come in bursts. Text editors might perform a few distinct operations when saving a file. (Some save modifications to a new file, then perform a couple of renames once this is complete, because this avoids data loss if a power failure or system crash happens to occur at the moment you save the file.) So you typically won't want to take action as soon as you detect file activity. It would be better to give it a moment to see if any more activity occurs, and take action only after everything has settled down.

So we should not react directly to filesystem activity. We want take action at those moments when everything goes quiet after a flurry of activity. Rx does not offer this functionality directly, but it's possible for us to create a custom operator by combing some of the built-in operators. The following code defines an Rx operator that detects and reports such things. If you're new to Rx (which seems likely if you're reading this) it probably won't be instantly obvious how this works. This is a significant step up in complexity from the examples I've shown so far because this came from a real application. But I'll walk through it step by step, so all will become clear.

```csharp
static class RxExt
{
    public static IObservable<IList<T>> Quiescent<T>(
        this IObservable<T> src,
        TimeSpan minimumInactivityPeriod,
        IScheduler scheduler)
    {
        IObservable<int> onoffs =
            from _ in src
            from delta in 
               Observable.Return(1, scheduler)
                         .Concat(Observable.Return(-1, scheduler)
                                           .Delay(minimumInactivityPeriod, scheduler))
            select delta;
        IObservable<int> outstanding = onoffs.Scan(0, (total, delta) => total + delta);
        IObservable<int> zeroCrossings = outstanding.Where(total => total == 0);
        return src.Buffer(zeroCrossings);
    }
}
```

The first thing to say about this is that we are effectively defining a custom LINQ-style operator: this is an extension method which, like all of the LINQ operators Rx supplies, takes an `IObservable<T>` as its implicit argument, and produces another observable source as its result. The return type is slightly different: it's `IObservable<IList<T>>`. That's because once we return to a state of inactivity, we will want to process everything that just happened, so this operator will produce a list containing every value that the source reported in its most recent flurry of activity.

When we want to show how an Rx operator behaves, we typically draw a 'marble' diagram. This is a diagram showing one or more `IObservable<T>` event sources, with each one being illustrated by a horizontal line. Each event that a source produces is illustrated by a circle (or 'marble') on that line, with the horizontal position representing timing. Typically, the line has a vertical bar on its left indicating the instant at which the application subscribed to the source, unless it happens to produce events immediately, in which case it will start with a marble. If the line has an arrowhead on the right, that indicates that the observable's lifetime extends beyond the diagram. Here's a diagram showing how the `Quiescent` operator above response to a particular input:

![An Rx marble diagram illustrating two observables. The first is labelled 'source', and it shows six events, labelled numerically. These fall into three groups: events 1 and 2 occur close together, and are followed by a gap. Then events 3, 4, and 5 are close together. And then after another gap event 6 occurs, not close to any. The second observable is labelled 'source.Quiescent(TimeSpan.FromSeconds(2), Scheduler.Default)'. It shows three events. The first is labelled '1, 2', and its horizontal position shows that it occurs a little bit after the '2' event on the 'source' observable. The second event on the second observable is labelled '3,4,5' and occurs a bit after the '5' event on the 'source' observable. The third event from on the second observable is labelled '6', and occurs a bit after the '6' event on the 'source' observable. The image conveys the idea that each time the source produces some events and then stops, the second observable will produce an event shortly after the source stops, which will contain a list with all of the events from the source's most recent burst of activity.](GraphicsIntro/Ch02-Quiescent-Marbles-Input-And-Output.svg)

This shows that the source (the top line) produced a couple of events (the values `1` and `2`, in this example), and then stopped for a bit. A little while after it stopped, the observable returned by the `Quiescent` operator (the lower line) produced a single event with a list containing both of those events (`[1,2]`). Then the source started up again, producing the values `3`, `4`, and `5` in fairly quick succession, and then going quiet for a bit. Again, once the quiet spell had gone on for long enough, the source returned by `Quiescent` produced a single event containing all of the source events from this second burst of activity (`[3,4,5]`). And then the final bit of source activity shown in this diagram consists of a single event, `6`, followed by more inactivity, and again, once the inactivity has gone on for long enough the `Quiescent` source produces a single event to report this. And since that last 'burst' of activity from the source contained only a single event, the list reported by this final output from the `Quiescent` observable is a list with a single value: `[6]`.

So how does the code shown achieve this? The first thing to notice about the `Quiescent` method is that it's just using other Rx LINQ operators (the `Return`, `Scan`, `Where`, and `Buffer` operators are explicitly visible, and the query expression will be using the `SelectMany` operator, because that's what C# query expressions do when they contain two `from` clauses in a row) in a combination that produces the final `IObservable<IList<T>>` output.

This is Rx's _compositional_ approach, and it is how we normally use Rx. We use a mixture of operators, combined (_composed_) in a way that produces the effect we want.

But how does this particular combination produce the effect we want? There are a few ways we could get the behaviour that we're looking for from a `Quiescent` operator, but the basic idea of this particular implementation is that it keeps count of how many events have happened recently, and then produces a result every time that number drops back to zero. The `outstanding` variable refers to the `IObservable<int>` that tracks the number of recent events, and this marble diagram shows what it produces in response to the same `source` events as were shown on the preceding diagram:

![How the Quiescent operator counts the number of outstanding events. An Rx marble diagram illustrating two observables. The first is labelled 'source', and it shows the same six events as the preceding figure, labelled numerically, but this time also color-coded so that each event has a different color. As before, these events fall into three groups: events 1 and 2 occur close together, and are followed by a gap. Then events 3, 4, and 5 are close together. And then after another gap event 6 occurs, not close to any. The second observable is labelled 'outstanding' and for each of the events on the 'source' observable, it shows two events. Each such pair has the same color as on the 'source' line; the coloring is just to make it easier to see how events on this line are associated with events on the 'source' line. The first of each pair appears directly below its corresponding event on the 'source' line, and has a number that is always one higher than its immediate predecessor; the very first item shows a number of 1. The first item from the second pair is the next to appear on this line, and therefore has a number of 2. But then the second item from the first pair appears, and this lowers the number back to 1, and it's followed by the second item from the second pair, which shows 0. Since the second batch of events on the first line appear fairly close together, we see values of 1, 2, 1, 2, 1, and then 0 for these. The final event on the first line, labelled 6, has a corresponding pair on the second line reporting values of 1 and then 0. The overall effect is that each value on the second, 'outstanding' line tells us how many items have emerged from the 'source' line in the last 2 seconds.](GraphicsIntro/Ch02-Quiescent-Marbles-Outstanding.svg)

I've colour coded the events this time so that I can show the relationship between `source` events and corresponding events produced by `outstanding`. Each time `source` produces an event, `outstanding` produces an event at the same time, in which the value is one higher than the preceding value produced by `outstanding`. But each such `source` event also causes `outstanding` to produce another event two seconds later. (It's two seconds because in these examples, I've presumed that the first argument to `Quiescent` is `TimeSpan.FromSeconds(2)`, as shown on the first marble diagram.) That second event always produces a value that is one lower than whatever the preceding value was.

This means that each event to emerge from `outstanding` tells us how many events `source` produced within the last two seconds. This diagram shows that same information in a slightly different form: it shows the most recent value produced by `outstanding` as a graph. You can see the value goes up by one each time `source` produces a new value. And two seconds after each value produced by `source`, it drops back down by one.

![The number of outstanding events as a graph. An Rx marble diagram illustrating the 'source' observables, and the second observable from the preceding diagram this time illustrated as a bar graph showing the latest value. This makes it easier to see that the 'outstanding' value goes up each time a new value emerges from 'source', and then goes down again two seconds later, and that when values emerge close together this running total goes higher. It also makes it clear that the value drops to zero between the 'bursts' of activity.](GraphicsIntro/Ch02-Quiescent-Marbles-Outstanding-Value.svg)

In simple cases like the final event `6`, in which it's the only event that happens at around that time, the `outstanding` value goes up by one when the event happens, and drops down again two seconds later. Over on the left of the picture it's a little more complex: we get two events in fairly quick succession, so the `outstanding` value goes up to one and then up to two, before falling back down to one and then down to zero again. The middle section looks a little more messy—the count goes up by one when the `source` produces event `3`, and then up to two when event `4` comes in. It then drops down to one again once two seconds have passed since the `3` event, but then another event, `5`, comes in taking the total back up to two. Shortly after that it drops back to one again because it has now been two seconds since the `4` event happened. And then a bit later, two seconds after the `5` event it drops back to zero again.

That middle section is the messiest, but it's also most representative of the kind of activity this operator is designed to deal with. Remember, the whole point here is that we're expecting to see flurries of activity, and if those represents filesystem activity, they will tend to be slightly chaotic in nature, because storage devices don't always have entirely predictable performance characteristics (especially if it's a magnetic storage device with moving parts, or remote storage in which variable networking delays might come into play).

With this measure of recent activity in hand, we can spot the end of bursts of activity by watching for when `outstanding` drops back to zero, which is what the observable referred to by `zeroCrossing` in the code above does. (That's just using the `Where` operator to filter out everything except the events where `outstanding`'s current value returns to zero.)

But how does `outstanding` itself work? The basic approach here is that every time `source` produces a value, we actually create a brand new `IObservable<int>`, which produces exactly two values. It immediately produces the value 1, and then after the specified timespan (2 seconds in these examples) it produces the value -1. That's what's going in in this clause of the query expression:

```csharp
from delta in Observable
    .Return(1, scheduler)
    .Concat(Observable
        .Return(-1, scheduler)
        .Delay(minimumInactivityPeriod, scheduler))
```

I said Rx is all about composition, and that's certainly the case here. We are using the very simple `Return` operator to create an `IObservable<int>` that immediately produces just a single value and then terminates. This code calls that twice, once to produce the value `1` and again to produce the value `-1`. It uses the `Delay` operator so that instead of getting that `-1` value immediately, we get an observable that waits for the specified time period (2 seconds in these examples, but whatever `minimumInactivityPeriod` is in general) before producing the value. And then we use `Concat` to stitch those two together into a single `IObservable<int>` that produces the value `1`, and then two seconds later produces the value `-1`.

Although this produces a brand new `IObservable<int>` for each `source` event, the `from` clause shown above is part of a query expression of the form `from ... from .. select`, which the C# compiler turns into a call to `SelectMany`, which has the effect of flattening those all back into a single observable, which is what the `onoffs` variable refers to. This marble diagram illustrates that:

![The number of outstanding events as a graph. Several Rx marble diagrams, starting with the 'source' observable from earlier figures, followed by one labelled with the LINQ query expression in the preceding example, which shows 6 separate marble diagrams, one for each of the elements produced by 'source'. Each consists of two events: one with value 1, positioned directly beneath the corresponding event on 'source' to indicate that they happen simultaneously, and then one with the value -1 two seconds later. Beneath this is a marble diagram labelled 'onoffs' which contains all the same events from the preceding 6 diagrams, but merged into a single sequence. These are all colour coded ot make it easier to see how these events correspond to the original events on 'source'. Finally, we have the 'outstanding' marble diagram which is exactly the same as in the preceding figure.](GraphicsIntro/Ch02-Quiescent-Marbles-On-Offs.svg)

This also shows the `outstanding` observable again, but we can now see where that comes from: it is just the running total of the values emitted by the `onoffs` observable. This running total observable is created with this code:

```csharp
IObservable<int> outstanding = onoffs.Scan(0, (total, delta) => total + delta);
```

Rx's `Scan` operator works much like the standard LINQ [`Aggregate`](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/aggregation-operations) operator, in that it cumulatively applies an operation (addition, in this case) to every single item in a sequence. The different is that whereas `Aggregate` produces just the final result once it reaches the end of the sequence, `Scan` shows all of its working, producing the accumulated value so far after each input. So this means that `outstanding` will produce an event every time `onoffs` produces one, and that event's value will be the running total—the sum total of every value from `onoffs` so far.

So that's how `outstanding` comes to tell us how many events `source` produced within the last two seconds (or whatever `minimumActivityPeriod` has been specified).

The final piece of the puzzle is how we go from the `zeroCrossings` (which produces an event every time the source has gone quiescent) to the output `IObservable<IList<T>>`, which provides all of the events that happened in the most recent burst of activity. Here we're just using Rx's `Buffer` operator, which is designed for exactly this scenario: it slices its input into chunks, producing an event for each chunk, the value of which is an `IList<T>` containing the items for the chunk. `Buffer` can slice things up a few ways, but in this case we're using the form that starts a new slice each time some `IObservable<T>` produces an item. Specifically, we're telling `Buffer` to slice up the `source` by creating a new chunk every time `zeroCrossings` produces a new event.

(One last detail, just in case you saw it and were wondering, is that this method requires an `IScheduler`. This is an Rx abstraction for dealing with timing and concurrency. We need it because we need to be able to generate events after a one second delay, and that sort of time-driven activity requires a scheduler.)

We'll get into all of these operators and the workings of schedulers in more detail in later chapters. For now, the key point is that we typically use Rx by creating a combination of LINQ operators that process and combine `IObservable<T>` sources to define the logic that we require.

Notice that nothing in that example actually called the one and only method that `IObservable<T>` defines (`Subscribe`). There will always be something somewhere that ultimately consumes the events, but most of the work of using Rx tends to entail declaratively defining the `IObservable<T>`s we need.

Now that you've seen an example of what Rx programming looks like, we can address some obvious questions about why Rx exists at all.

### What was wrong with .NET Events?

.NET has had built-in support for events from the very first version that shipped over two decades ago—events are part of .NET's type system. The C# language has intrinsic support for this in the form of the `event` keyword, along with specialized syntax for subscribing to events. So why, when Rx turned up some 10 years later, did it feel the need to invent its own representation for streams of events? What was wrong with the `event` keyword?

The basic problem with .NET events is that they get special handling from the .NET type system. Ironically, this makes them less flexible than if there had been no built-in support for the idea of events. Without .NET events, we would have needed some sort of object-based representation of events, at which point you can do all the same things with events that you can do with any other objects: you could store them in fields, pass them as arguments to methods, define methods on them and so on.

To be fair to .NET version 1, it wasn't really possible to define a good object-based representation of events without generics, and .NET didn't get those until version 2 (three and a half years after .NET 1.0 shipped). Different event sources need to be able to report different data, and .NET events provided a way to parameterize events by type. But once generics came along, it became possible to define types such as `IObservable<T>`, and the main advantage that events offered went away. (The other benefit was some language support for implementing and subscribing to events, but in principle that's something that could have been done for Rx if Microsoft had chosen to. It's not a feature that required events to be fundamentally different from other features of the type system.)

Consider the example we've just worked through. It was possible to define our own custom LINQ operator, `Quiescent`, because `IObservable<T>` is just an interface like any other, meaning that we're free to write extension methods for it. You can't write an extension method for an event.

Also, we are able to wrap or adapt `IObservable<T>` sources. `Quiescent` took an `IObservable<T>` as an input, and combined various Rx operators to produce another observable as an output. Its input was a source of events that could be subscribed to, and its output was also a source of events that could be subscribed to. You can't do this with .NET events—you can't write a method that accepts an event as an argument, or that returns an event.

These limitations are sometimes described by saying that .NET events are not _first class citizens_. There are things you can do with values or references in .NET that you can't do with events.

If we represent an event source as a plain old interface, then it _is_ a first class citizen: it can use all of the functionality we expect with other objects and values precisely because it's not something special.

### What about Streams?

I've described `IObservable<T>` as representing a _stream_ of events. This raises an obvious question: .NET already has [`System.IO.Stream`](https://learn.microsoft.com/en-us/dotnet/api/system.io.stream), so why not just use that?

The short answer is that streams are weird because they represent an ancient concept in computing dating back long before the first ever Windows operating system shipped, and as such they have quite a lot of historical baggage. This means that even a scenario as simple as "I have some data, and want to make that available immediately to all interested parties" is surprisingly complex to implement though the `Stream` type.

Moreover, `Stream` doesn't provide any way to indicate what type of data will emerge—it only knows about bytes. Since .NET's type system supports generics, it is natural to want the types that represent event streams to indicate the event type through a type parameter.

So even if you did use `Stream` as part of your implementation, you'd want to introduce some sort of wrapper abstraction. If `IObservable<T>` didn't exist, you'd need to invent it.

It's certainly possible to use IO streams in Rx, but they are not the right primary abstraction.

(If you are unconvinced, see [Appendix A: What's Wrong with Classic IO Streams](A_IoStreams.md) for a far more detailed explanation of exactly why `Stream` is not well suited to this task.)

Now that we've seen why `IObservable<T>` needs to exist, we need to look at its counterpart, `IObserver<T>`.

## `IObserver<T>`

Earlier, I showed the definition of `IObservable<T>`. As you saw, it has just one method, `Subscribe`. And this method takes just one argument, of type [`IObserver<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.iobserver-1). So if you want to observe the events that an `IObservable<T>` has to offer, you must supply it with an `IObserver<T>`. In the examples so far, we've just supplied a simple callback, and Rx has wrapped that in an implementation of `IObserver<T>` for us, but even though this is very often the way we will receive notifications in practice, you still need to understand `IObserver<T>` to use Rx effectively. It is not a complex interface:

```csharp
public interface IObserver<in T>
{
    void OnNext(T value);
    void OnError(Exception error);
    void OnCompleted();
}
```

As with `IObservable<T>`, you can find [the source for `IObserver<T>`](https://github.com/dotnet/runtime/blob/7cf329b773fa5ed544a9377587018713751c73e3/src/libraries/System.Private.CoreLib/src/System/IObserver.cs) in the .NET runtime GitHub repository, because both of these interfaces are built into the runtime libraries.

If we wanted to create an observer that printed values to the console it would be as easy as this:

```csharp
public class MyConsoleObserver<T> : IObserver<T>
{
    public void OnNext(T value)
    {
        Console.WriteLine($"Received value {value}");
    }

    public void OnError(Exception error)
    {
        Console.WriteLine($"Sequence faulted with {error}");
    }

    public void OnCompleted()
    {
        Console.WriteLine("Sequence terminated");
    }
}
```

In the preceding chapter, I used a `Subscribe` extension method that accepted a delegate which it invoked each time the source produced an item. This method is defined by Rx's `ObservableExtensions` class, which also defines various other extension methods for `IObservable<T>`. It includes overloads of `Subscribe` that enable me to write code that has the same effect as the preceding example, without needing to provide my own implementation of `IObserver<T>`:

```csharp
source.Subscribe(
    value => Console.WriteLine($"Received value {value}"),
    error => Console.WriteLine($"Sequence faulted with {error}"),
    () => Console.WriteLine("Sequence terminated")
);
```

The overloads of `Subscribe` where we don't pass all three methods (e.g., my earlier example just supplied a single callback corresponding to `OnNext`) are equivalent to writing an `IObserver<T>` implementation where one or more of the methods simply has an empty body. Whether we find it more convenient to write our own type that implements `IObserver<T>`, or just supply callbacks for some or all of its `OnNext`, `OnError` and `OnCompleted` method, the basic behaviour is the same: an `IObservable<T>` source reports each event with a call to `OnNext`, and tells us that the events have come to an end either by calling `OnError` or `OnCompleted`.

If you're wondering whether the relationship between `IObservable<T>` and `IObserver<T>` is similar to the relationship between [`IEnumerable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1) and [`IEnumerator<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerator-1), then you're onto something. Both `IEnumerable<T>` and `IObservable<T>` represent _potential_ sequences. With both of these interfaces, they will only supply data if we ask them for it. To get values out of an `IEnumerable<T>`, an `IEnumerator<T>` needs to come into existence, and similarly, to get values out of an `IObservable<T>` requires an `IObserver<T>`.

The difference reflects the fundamental _pull vs push_ difference between `IEnumerable<T>` and `IObservable<T>`. Whereas with `IEnumerable<T>` we ask the source to create an `IEnumerator<T>` for us which we can then use to retrieve items (which is what a C# `foreach` loop does), with `IObservable<T>`, the source does not _implement_ `IObserver<T>`: it expects _us_ to supply an `IObserver<T>` and it will then push its values into that observer.

So why does `IObserver<T>` have these three methods? Remember when I said that in an abstract sense, `IObserver<T>` represents the same thing as `IEnumerable<T>`? I meant it. It might be an abstract sense, but it is precise: `IObservable<T>` and `IObserver<T>` were designed to preserve the exact meaning of `IEnumerable<T>` and `IEnumerator<T>`, changing only the detailed mechanism of consumption.

To see what that means, think about what happens when you iterate over an `IEnumerable<T>` (with, say, a `foreach` loop). With each iteration (and more precisely, on each call to the enumerator's [`MoveNext`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.ienumerator.movenext) method) there are three things that could happen:

* `MoveNext` could return `true` to indicate that a value is available in the enumerator's [`Current`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerator-1.current) property
* `MoveNext` could throw an exception
* `MoveNext` could return `false` to indicate that you've reached the end of the collection

These three outcomes correspond precisely to the three methods defined by `IObserver<T>`. We could describe these in slightly more abstract terms:

* Here's another item
* It has all gone wrong
* There are no more items

That describes the three things that either can happen next when consuming either an `IEnumerable<T>` or an `IObservable<T>`. The only difference is the means by which consumers discover this. With an `IEnumerable<T>` source, each call to `MoveNext` will tell us which of these three applies. And with an `IObservable<T>` source, it will tell you one of these three things with a call to the corresponding member of your `IObserver<T>` implementation.

## The Fundamental Rules of Rx Sequences

Notice that two of the three outcomes in the list above are terminal. If you're iterating through an `IEnumerable<T>` with a `foreach` loop, and it throws an exception, the `foreach` loop will terminate. The C# compiler understands that if `MoveNext` throws, the `IEnumerator<T>` is now done, so it disposes it and then allows the exception to propagate. Likewise, if you get to the end of a sequence, then you're done, and the compiler understands that too: the code it generates for a `foreach` loop detects when `MoveNext` returns false and when that happens it disposes the enumerator and then moves onto the code after the loop.

These rules might seem so obvious that we might never even think about them when iterating over `IEnumerable<T>` sequences. What might be less immediately obvious is that exactly the same rules apply for an `IObservable<T>` sequence. If an observable source either tells an observer that the sequence has finished, or reports an error, then in either case, that is the last thing the source is allowed to do to the observer.

That means these examples would be breaking the rules:

```csharp
public static void WrongOnError(IObserver<int> obs)
{
    obs.OnNext(1);
    obs.OnError(new ArgumentException("This isn't an argument!"));
    obs.OnNext(2);  // Against the rules! We already reported failure, so iteration must stop
}

public static void WrongOnCompleted(IObserver<int> obs)
{
    obs.OnNext(1);
    obs.OnCompleted();
    obs.OnNext(2);  // Against the rules! We already said we were done, so iteration must stop
}

public static void WrongOnErrorAndOnCompleted(IObserver<int> obs)
{
    obs.OnNext(1);
    obs.OnError(new ArgumentException("A connected series of statements was not supplied"));

    // This next call is against the rules because we reported an error, and you're not
    // allowed to make any further calls after you did that.
    obs.OnCompleted();
}

public static void WrongOnCompletedAndOnError(IObserver<int> obs)
{
    obs.OnNext(1);
    obs.OnCompleted();

    // This next call is against the rule because already said we were done.
    // When you terminate a sequence you have to pick between OnCompleted or OnError
    obs.OnError(new ArgumentException("Definite proposition not established"));
}
```

These correspond in a pretty straightforward way to things we already know about `IEnumerable<T>`:

* `WrongOnError`: if an enumerator throws from `MoveNext`, it's done and you mustn't call `MoveNext` again, so you won't be getting any more items out of it
* `WrongOnCompleted`: if an enumerator returns `false` from `MoveNext`, it's done and you mustn't call `MoveNext` again, so you won't be getting any more items out of it
* `WrongOnErrorAndOnCompleted`: if an enumerator throws from `MoveNext`, that means its done, it's done and you mustn't call `MoveNext` again, meaning it won't have any opportunity to tell that it's done by returning `false` from `MoveNext`
* `WrongOnCompletedAndOnError`: if an enumerator returns `false` from `MoveNext`, it's done and you mustn't call `MoveNext` again,  meaning it won't have any opportunity to also throw an exception

Because `IObservable<T>` is push-based, the onus for obeying all of these rules fall on the observable source. With `IEnumerable<T>`, which is pull-based, it's up to the code using the `IEnumerator<T>` (e.g. a `foreach` loop) to obey these rules. But they are essentially the same rules.

There's an additional rule for `IObserver<T>`: if you call `OnNext` you must wait for it to return before making any more method calls into the same `IObserver<T>`. That means this code breaks the rules:

```csharp
public static void EverythingEverywhereAllAtOnce(IEnumerable<int> obs)
{
    Random r = new();
    for (int i = 0; i < 10000; ++i)
    {
        int v = r.Next();
        Task.Run(() => obs.OnNext(v)); // Against the rules!
    }}
```

This calls `obs.OnNext` 10,000 times, but it executes these calls as individual tasks to be run on the thread pool. The thread pool is designed to be able to execute work in parallel, and that's a a problem here because nothing here ensures that one call to `OnNext` completes before the next begins. We've broken the rule that says we must wait for each call to `OnNext` to return before calling either `OnNext`, `OnError`, or `OnComplete` on the same observer. (Note: this assumes that the caller won't subscribe the same observer to multiple different sources. If you do that, you can't assume that all calls to its `OnNext` will obey the rules, because the different sources won't have any way of knowing they're talking to the same observer.)

This rule is the only form of back pressure built into Rx.NET: since the rules forbid calling `OnNext` if a previous call to `OnNext` is still in progress, this enables an `IObserver<T>` to limit the rate at which items arrive. If you just don't return from `OnNext` until you're ready, the source is obliged to wait. However, there are some issues with this. Once [schedulers](11_SchedulingAndThreading.md) get involved, the underlying source might not be connected directly to the final observer. If you use something like [`ObserveOn`](11_SchedulingAndThreading.md#subscribeon-and-observeon) it's possible that the `IObserver<T>` subscribed directly to the source just puts items on a queue and immediately returns, and those items will then be delivered to the real observer on a different thread. In these cases, the 'back pressure' caused by taking a long time to return from `OnNext` only propagates as far as the code pulling items off the queue. 

It may be possible to use certain Rx operators (such as [`Buffer`](08_Partitioning.md#buffer) or [`Sample`](12_Timing.md#sample)) to mitigate this, but there are no built-in mechanisms for cross-thread propagation of back pressure. Some Rx implementations on other platforms have attempted to provide integrated solutions for this; in the past when the Rx.NET development community has looked into this, some thought that these solutions were problematic, and there is no consensus on what a good solution looks like. So with Rx.NET, if you need to arrange for sources to slow down when you are struggling to keep up, you will need to introduce some mechanism of your own. (Even with Rx platforms that do offer built-in back pressure, they can't provide a general-purpose answer to the question: how do we make this source provide events more slowly? How (or even whether) you can do that will depend on the nature of the source. So some bespoke adaptation is likely to be necessary in any case.)

This rule in which we must wait for `OnNext` to return is tricky and subtle. It's perhaps less obvious than the others, because there's no equivalent rule for `IEnumerable<T>`—the opportunity to break this rule only arises when the source pushes data into the application. You might look at the example above and think "well who would do that?" However, multithreading is just an easy way to show that it is technically possible to break the rule. The harder cases are where single-threaded re-entrancy occurs. Take this code:

```csharp
public class GoUntilStopped
{
    private readonly IObserver<int> observer;
    private bool running;

    public GoUntilStopped(IObserver<int> observer)
    {
        this.observer = observer;
    }

    public void Go()
    {
        this.running = true;
        for (int i = 0; this.running; ++i)
        {
            this.observer.OnNext(i);
        }
    }

    public void Stop()
    {
        this.running = false;
        this.observer.OnCompleted();
    }
}
```

This class takes an `IObserver<int>` as a constructor argument. When you call its `Go` method, it repeatedly calls the observer's `OnNext` until something calls its `Stop` method.

Can you see the bug?

We can take a look at what happens by supplying an `IObserver<int>` implementation:

```csharp
public class MyObserver : IObserver<int>
{
    private GoUntilStopped? runner;

    public void Run()
    {
        this.runner = new(this);
        Console.WriteLine("Starting...");
        this.runner.Go();
        Console.WriteLine("Finished");
    }

    public void OnCompleted()
    {
        Console.WriteLine("OnCompleted");
    }

    public void OnError(Exception error) { }

    public void OnNext(int value)
    {
        Console.WriteLine($"OnNext {value}");
        if (value > 3)
        {
            Console.WriteLine($"OnNext calling Stop");
            this.runner?.Stop();
        }
        Console.WriteLine($"OnNext returning");
    }
}
```

Notice that the `OnNext` method looks at its input, and if it's greater than 3, it tells the `GoUntilStopped` object to stop.

Let's look at the output:

```
Starting...
OnNext 0
OnNext returning
OnNext 1
OnNext returning
OnNext 2
OnNext returning
OnNext 3
OnNext returning
OnNext 4
OnNext calling Stop
OnCompleted
OnNext returning
Finished
```

The problem is right near the end. Specifically, these two lines:

```
OnCompleted
OnNext returning
```

This tells us that the call to our observer's `OnCompleted` happened before a call in progress to `OnNext` returned. It didn't take multiple threads to make this occur. It happened because the code in `OnNext` decides whether it wants to keep receiving events, and when it wants to stop, it immediately calls the `GoUntilStopped` object's `Stop` method. There's nothing wrong with that. Observers are allowed to make outbound calls to other objects inside `OnNext`, and it's actually quite common for an observer to inspect an incoming event and decide that it wants to stop.

The problem is in the `GoUntilStopped.Stop` method. This calls `OnCompleted` but it makes no attempt to determine whether a call to `OnNext` is in progress.

This can be a surprisingly tricky problem to solve. Suppose `GoUntilStopped` _did_ detect that there was a call in progress to `OnNext`. What then? In the multithreaded case, we could have solved this by using `lock` or some other synchronization primitive to ensure that calls into the observer happened one at at time, but that won't work here: the call to `Stop` has happened on _the same thread_ that called `OnNext`. The call stack will look something like this at the moment where `Stop` has been called and it wants to call `OnCompleted`:

```
`GoUntilStopped.Go`
  `MyObserver.OnNext`
    `GoUntilStopped.Stop`
```
 
 Our `GoUntilStopped.Stop` method needs to wait for `OnNext` to return before calling `OnCompleted`. But notice that the `OnNext` method can't return until our `Stop` method returns. We've managed to create a deadlock with single-threaded code!

In this case it's not all that hard to fix: we could modify `Stop` so it just sets the `running` field to `false`, and then move the call to `OnComplete` into the `Go` method, after the `for` loop. But more generally this can be a hard problem to fix, and it's one of the reasons for using the `System.Reactive` library instead of just attempting to implement `IObservable<T>` and `IObserver<T>` directly. Rx has general purpose mechanisms for solving exactly this kind of problem. (We'll see these when we look at [Scheduling](11_SchedulingAndThreading.md).) Moreover, all of the implementations Rx provides take advantage of these mechanisms for you.

If you're using Rx by composing its built-in operators in a declarative way, you never have to think about these rules. You get to depend on these rules in your callbacks that receive the events, and it's mostly Rx's problem to keep to the rules. So the main effect of these rules is that it makes life simpler for code that consumes events.

These rules are sometimes expressed as a _grammar_. For example, consider this regular expression:

```
(OnNext)*(OnError|OnComplete)
```

This formally captures the basic idea: there can be any number of calls to `OnNext` (maybe even zero calls), that occur in sequence, followed by either an `OnError` or an `OnComplete`, but not both, and there must be nothing after either of these.

One last point: sequences may be infinite. This is true for `IEnumerable<T>`. It's perfectly possible for an enumerator to return `true` every time `MoveNext` is returned, in which case a `foreach` loop iterating over it will never reach the end. It might choose to stop (with a `break` or `return`), or some exception that did not originate from the enumerator might cause the loop to terminate, but it's absolutely acceptable for an `IEnumerable<T>` to produce items for as long as you keep asking for them. The same is true of a `IObservable<T>`. If you subscribe to an observable source, and by the time your program exits you've not received a call to either `OnComplete` or `OnError`, that's not a bug.

So you might argue that this is a slightly better way to describe the rules formally:

```
(OnNext)*(OnError|OnComplete)?
```

More subtly, observable sources are allowed to do nothing at all. In fact there's a built-in implementation to save developers from the effort of writing a source that does nothing: if you call `Observable.Never<int>()` it will return an `IObservable<int>`, and if you subscribe to that, it will never call any methods on your observer. This might not look immediately useful—it is logically equivalent to an `IEnumerable<T>` in which the enumerator's `MoveNext` method never returns, which might not be usefully distinguishable from crashing. It's slightly different with Rx, because when we model this "no items emerge ever" behaviour, we don't need to block a thread forever to do it. We can just decide never to call any methods on the observer. This may seem daft, but as you've seen with the `Quiescent` example, sometimes we create observable sources not because we want the actual items that emerge from it, but because we're interested in the instants when interesting things happen. It can sometimes be useful to be able to model "nothing interesting ever happens" cases. For example, if you have written some code to detect unexpected inactivity (e.g., a sensor that stops producing values), and wanted to test that code, your test could use a `Never` source instead of a real one, to simulate a broken sensor.

We're not quite done with the Rx's rules, but the last one applies only when we choose to unsubscribe from a source before it comes to a natural end.

## Subscription Lifetime

There's one more aspect of the relationship between observers and observables to understand: the lifetime of a subscription.

You already know from the rules of `IObserver<T>` that a call to either `OnComplete` or `OnError` denotes the end of a sequence. We passed an `IObserver<T>` to `IObservable<T>.Subscribe`, and now the subscription is over. But what if we want to stop the subscription earlier?

I mentioned earlier that the `Subscribe` method returns an `IDisposable`, which enables us to cancel our subscription. Perhaps we only subscribed to a source because our application opened some window showing the status of some process, and we wanted to update the window to reflect that's process's progress. If the user closes that window, we no longer have any use for the notifications. And although we could just ignore all further notifications, that could be a problem if the thing we're monitoring never reaches a natural end. Our observer would continue to receive notifications for the lifetime of the application. This is a waste of CPU power (and thus power consumption, with corresponding implications for battery life and environmental impact) and it can also prevent the garbage collector from reclaiming memory that should have become free.

So we are free to indicate that we no longer wish to receive notifications by calling `Dispose` on the object returned by `Subscribe`. There are, however, a few non-obvious details.

### Disposal of Subscriptions is Optional

You are not required to call `Dispose` on the object returned by `Subscribe`. Obviously if you want to remain subscribed to events for the lifetime of your process, this makes sense: you never stop using the object, so of course you don't dispose it. But what might be less obvious is that if you subscribe to an `IObservable<T>` that does come to an end, it automatically tidies up after itself.

`IObservable<T>` implementations are not allowed to assume that you will definitely call `Dispose`, so they are required to perform any necessary cleanup if they stop by calling the observer's `OnCompleted` or `OnError`. This is unusual. In most cases where a .NET API returns a brand new object created on your behalf that implements `IDisposable`, it's an error not to dispose it. But `IDisposable` objects representing Rx subscriptions are an exception to this rule. You only need to dispose them if you want them to stop earlier than they otherwise would.

### Cancelling Subscriptions may be Slow or Even Ineffectual

`Dispose` won't necessarily take effect instantly. Obviously it will take some non-zero amount of time in between your code calling into `Dispose`, and the `Dispose` implementation reaching the point where it actually does something. Less obviously, some observable sources may need to do non-trivial work to shut things down.

A source might create a thread to be able to monitor for and report whatever events it represents. (That would happen with the filesystem source shown above when running on Linux on .NET 8, because the `FileSystemWatcher` class itself creates its own thread on Linux.) It might take a while for the thread to detect that it is supposed to shut down.

It is fairly common practice for an `IObservable<T>` to represent some underlying work. For example, Rx can take any factory method that returns a `Task<T>` and wrap it as an `IObservable<T>`. It will invoke the factory once for each call to `Subscribe`, so if there are multiple subscribers to a single `IObservable<T>` of this kind, each one effectively gets its own `Task<T>`. This wrapper is able to supply the factory with a `CancellationToken`, and if an observer unsubscribes by calling `Dispose` before the task naturally runs to completion, it will put that `CancellationToken` into a cancelled state. This might have the effect of bringing the task to a halt, but that will work only if the task happens to be monitoring the `CancellationToken`. Even if it is, it might take some time to bring things to a complete halt. Crucially, the `Dispose` call doesn't wait for that to happen. It will attempt to initiate cancellation but it may return before cancellation is complete.

### The Rules of Rx Sequences when Unsubscribing

The fundamental rules of Rx sequences described earlier only considered sources that decided when (or whether) to come to a halt. What if a subscriber unsubscribes early? There is only one rule:

Once the call to `Dispose` has returned, the source will make no further calls to the relevant observer. If you call `Dispose` on the object returned by `Subscribe`, then once that call returns you can be certain that the observer you passed in will receive no further calls to any of its three methods (`OnNext`, `OnError`, or `OnComplete`).

That might seem clear enough, but it leaves a grey area: what happens when you've called `Dispose` but it hasn't returned yet? The rules permit sources to continue to emit events in this case. In fact they couldn't very well require otherwise: it will invariably take some non-zero length of time for the `Dispose` implementation to make enough progress to have any effect, so in a multi-threaded world it it's always going to be possible that an event gets delivered in between the call to `Dispose` starting, and the call having any effect. The only situation in which you could depend on no further events emerging would be if your call to `Dispose` happened inside the `OnNext` handler. In this case the source will already have noted a call to `OnNext` is in progress so further calls were already blocked before the call to `Dispose` started.

But assuming that your observer wasn't already in the middle of an `OnNext` call, any of the following would be legal:

* stopping calls to `IObserver<T>` almost immediately after `Dispose` begins, even when it takes a relatively long time to bring any relevant underlying processes to a halt, in which case your observer will never receive an `OnCompleted` or `OnError`
* producing notifications that reflect the process of shutting down (including calling `OnError` if an error occurs while trying to bring things to a neat halt, or `OnCompleted` if it halted without problems)
* producing a few more notifications for some time after the call to `Dispose` begins, but cutting them off at some arbitrary point, potentially losing track even of important things like errors that occurred while trying to bring things to a halt

As it happens, Rx has a preference for the first option. If you're using an `IObservable<T>` implemented by the `System.Reactive` library (e.g., one returned by a LINQ operator) it is highly likely to have this characteristic. This is partly to avoid tricky situations in which observers try to do things to their sources inside their notification callbacks. Re-entrancy tends to be awkward to deal with, and Rx avoids ever having to deal with this particular form of re-entrancy by ensuring that it has already stopped delivering notifications to the observer before it begins the work of shutting down a subscription.

This sometimes catches people out. If you need to be able to cancel some process that you are observing but you need to be able to observe everything it does up until the point that it stops, then you can't use unsubscription as the shutdown mechanism. As soon as you've called `Dispose`, the `IObservable<T>` that returned that `IDisposable` is no longer under any obligation to tell you anything. This can be frustrating, because the `IDisposable` returned by `Subscribe` can sometimes seem like such a natural and easy way to shut something down. But basic truth is this: once you've initiated unsubscription, you can't rely on getting any further notifications associated with that subscription. You _might_ receive some—the source is allowed to carry on supplying items until the call to `Dispose` returns. But you can't rely on it—the source is also allowed to silence itself immediately, and that's what most Rx-implemented sources will do.

One subtle consequence of this is that if an observable source reports an error after a subscriber has unsubscribed, that error might be lost. A source might call `OnError` on its observer, but if that's a wrapper provided by Rx relating to a subscription that has already been disposed, it just ignores the exception. So it's best to think of early unsubscription as inherently messy, a bit like aborting a thread: it can be done but information can be lost, and there are race conditions that will disrupt normal exception handling.

In short, if you unsubscribe, then a source is not obliged to tell you when things stop, and in most cases it definitely won't tell you.

### Subscription Lifetime and Composition

We typically combine multiple LINQ operators to express our processing requirements in Rx. What does this mean for subscription lifetime?

For example, consider this:

```csharp
IObservable<int> source = GetSource();
IObservable<int> filtered = source.Where(i => i % 2 == 0);
IDisposable subscription = filtered.Subscribe(
    i => Console.WriteLine(i),
    error => Console.WriteLine($"OnError: {error}"),
    () => Console.WriteLine("OnCompleted"));
```

We're calling `Subscribe` on the observable returned by `Where`. When we do that, it will in turn call `Subscribe` on the `IObservable<int>` returned by `GetSource` (stored in the `source` variable). So there is in effect a chain of subscriptions here. (We only have access to the `IDisposable` returned by `filtered.Subscribe` but the object that returns will be storing the `IDisposable` that it received when it called `source.Subscribe`.)

If the source comes to an end all by itself (by calling either `OnCompleted` or `OnError`), this cascades through the chain. So `source` will call `OnCompleted` on the `IObserver<int>` that was supplied by the `Where` operator. And that in turn will call `OnCompleted` on the `IObserver<int>` that was passed to `filtered.Subscribe`, and that will have references to the three methods we passed, so it will call our completion handler. So you could look at this by saying that `source` completes, it tells `filtered` that it has completed, which invokes our completion handler. (In reality this is a very slight oversimplification, because `source` doesn't tell `filtered` anything; it's actually talking to the `IObserver<T>` that `filtered` supplied. This distinction matters if you have multiple subscriptions active simultaneously for the same chain of observables. But in this case, the simpler way of describing it is good enough even if it's not absolutely precise.)

In short, completion bubbles up from the source, through all the operators, and arrives at our handler.

What if we unsubscribe early by calling `subscription.Dispose()`? In that case it all happens the other way round. The `subscription` returned by `filtered.Subscribe` is the first to know that we're unsubscribing, but it will then call `Dispose` on the object that was returned when it called `source.Subscribe` for us.

Either way, everything from the source to the observer, including any operators that were sitting in between, gets shut down.

Now that we understand the relationship between an `IObservable<T>` source and the `IObserver<T>` interface that received event notifications, we can look at how we might create an `IObservable<T>` instance to represent events of interest in our application.