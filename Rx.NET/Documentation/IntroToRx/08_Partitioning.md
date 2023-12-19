# Partitioning

Rx can split a single sequence into multiple sequences. This can be useful for distributing items over many subscribers. When performing analytics, it can be useful to take aggregates on partitions. You may already be familiar with the standard LINQ operators `GroupBy`. Rx supports this, and also defines some of its own.

## GroupBy

The `GroupBy` operator allows you to partition your sequence just as `IEnumerable<T>`'s `GroupBy` operator does. Once again, the open source [Ais.Net project](https://github.com/ais-dotnet) can provide a useful example. Its [`ReceiverHost` class](https://github.com/ais-dotnet/Ais.Net.Receiver/blob/15de7b2908c3bd67cf421545578cfca59b24ed2c/Solutions/Ais.Net.Receiver/Ais/Net/Receiver/Receiver/ReceiverHost.cs) makes AIS messages available through Rx, defining a `Messages` property of type `IObservable<IAisMessage>`. This is a very busy source, because it reports every message it is able to access. For example, if you connect the receiver to the AIS message source generously provided by the Norwegian government, it produces a notification every time _any_ ship broadcasts an AIS message anywhere on the Norwegian coast. There are a lot of ships moving around Norway, so this is a bit of a firehose.

If we know exactly which ships we're interested in, you saw how to filter this stream in the [Filtering chapter](05_Filtering.md). But what if we don't, and yet we still want to be able to perform processing relating to individual ships? For example, perhaps we'd like to discover any time any ship changes its `NavigationStatus` (which reports values such as `AtAnchor`, or `Moored`). The [`Distinct` and `DistinctUntilChanged` section of the Filtering chapter](05_Filtering.md#distinct-and-distinctuntilchanged) showed how to do exactly that, but it began by filtering the stream down to messages from a single ship. If we tried to use `DistinctUntilChanged` directly on the all-ships stream it will not produce meaningful information. If ship A is moored and ship B is at anchor, and if we receive alternative status messages from ship A and ship B, `DistinctUntilChanged` would report each message as a change in status, even though neither ship's status has changed.

We can fix this by splitting the "all the ships" sequence into lots of little sequences:

```csharp
IObservable<IGroupedObservable<uint, IAisMessage>> perShipObservables = 
   receiverHost.Messages.GroupBy(message => message.Mmsi);
```

This `perShipObservables` is an observable sequence of observable sequences. More specifically, it's an observable sequence of grouped observable sequences, but as you can see from [the definition of `IGroupedObservable<TKey, T>`](https://github.com/dotnet/reactive/blob/95d9ea9d2786f6ec49a051c5cff47dc42591e54f/Rx.NET/Source/src/System.Reactive/Linq/IGroupedObservable.cs#L18), a grouped observable is just a specialized kind of observable:

```csharp
public interface IGroupedObservable<out TKey, out TElement> : IObservable<TElement>
{
    TKey Key { get; }
}
```

Each time `receiverHost.Message` reports an AIS message, the `GroupBy` operator will invoke the callback to find out which group this item belongs to. We refer to the value returned by the callback as the _key_, and `GroupBy` remembers each key it has already seen. If this is a new key, `GroupBy` creates a new `IGroupedObservable` whose `Key` property will be the value just returned by the callback. It emits this `IGroupedObservable` from the outer observable (the one we put in `perShipObservables`) and then immediately causes that new `IGroupedObservable` to emit the element (an `IAisMessage` in this example) that produced that key. But if the callback produces a key that `GroupBy` has seen before, it finds the `IGroupedObservable` that it already produced for that key, and causes that to emit the value.

So in this example, the effect is that any time the `receiverHost` reports a message from a ship with we've not previously heard from, `perShipObservables` will emit a new observable that reports messages just for that ship. We could use this to report each time we learn about a new ship:

```csharp
perShipObservables.Subscribe(m => Console.WriteLine($"New ship! {m.Key}"));
```

But that doesn't do anything we couldn't have achieved with `Distinct`. The power of `GroupBy` is that we get an observable sequence for each ship here, so we can go on to set up some per-ship processing:

```csharp
IObservable<IObservable<IAisMessageType1to3>> shipStatusChangeObservables =
    perShipObservables.Select(shipMessages => shipMessages
        .OfType<IAisMessageType1to3>()
        .DistinctUntilChanged(m => m.NavigationStatus)
        .Skip(1));
```

This uses [`Select`](06_Transformation.md#select) (introduced in the Transformation chapter) to apply processing to each group that comes out of `perShipObservables`. Remember, each such group represents a distinct ship, so the callback we've passed to `Select` here will be invoked exactly once for each ship. This means it's now fine for us to use `DistinctUntilChanged`. The input this example supplies to `DistinctUntilChanged` is a sequence representing the messages from just one ship, so this will tell us when that ship changes its status. This is now able to do what we want because each ship gets its own instance of `DistinctUntilChanged`. `DistinctUntilChanged` always forwards the first event it receives—it only drops items when they are the same as the preceding item, and there is no preceding item in this case. But that is unlikely to be the right behaviour here. Suppose that the first message we see from some vessel named `A` reports a status of `Moored`. It's possible that immediately before we started running, it was in some different state, and that the very first report we received happened to represent a change in status. But it's more likely that it has been moored for some time before we started. We can't tell for certain, but the majority of status reports don't represent a change, so `DistinctUntilChanged`'s behaviour of always forwarding the first event is likely to be wrong here. So we use `Skip(1)` to drop the first message from each ship.

At this point we have an observable sequence of observable sequences. The outer sequence produces a nested sequence for each distinct ship that it sees, and that nested sequence will report `NavigationStatus` changes for that particular ship.

I'm going to make a small tweak:

```csharp
IObservable<IAisMessageType1to3> shipStatusChanges =
    perShipObservables.SelectMany(shipMessages => shipMessages
        .OfType<IAisMessageType1to3>()
        .DistinctUntilChanged(m => m.NavigationStatus)
        .Skip(1));
```

I've replaced `Select` with [`SelectMany`, also described in the Transformation chapter](06_Transformation.md#selectmany). As you may recall, `SelectMany` flattens nested observables back into a single flat sequence. You can see this reflected in the return type: now we've got just an `IObservable<IAisMessageType1to3>` instead of a sequence of sequences.

Wait a second! Haven't I just undone the work that `GroupBy` did? I asked it to partition the events by vessel id, so why am I now recombining it back into a single, flat stream? Isn't that what I started with?

It's true that the stream type has the same shape as my original input: this will be a single observable sequence of AIS messages. (It's a little more specialized—the element type is `IAisMessageType1to3`, because that's where I can get `NavigationStatus` from, but these all still implement `IAisMessage`.) And all the different vessels will be mixed together in this one stream. But I've not actually negated the work that `GroupBy` did. This marble diagram illustrates what's going on:

![A marble diagram showing how an input observable named receiverHost.Messages is expanded into groups, processed, and then collapsed back into a single source. The input observable shows events from three different ships, 'A', 'B', and 'C'. Each event is labelled with the ship's reported status. All the messages from A report a status of Moored. B makes two AtAnchor status reports, followed by two UnderwayUsingEngine reports. C reports UnderwaySailing twice, then AtAnchor, and then UnderwaySailing again. The events from the three ships are intermingled—the order on the input line goes A, B, C, B, A, C, B, C, C, B, A. The next section is labelled as perShipObservables, and this shows the effect of grouping the events by vessel. The first line shows only the events from A, the second those from B, and the third those from C. The next section is labelled with the processing code from the preceding example, and shows three more observables, corresponding to the three groups in the preceding part of the diagram. But in this one, the source for A shows no events at all. The second line shows a single event for B, the first one where it reported UnderwayUsingEngine. And it shows two for C: the one where it reported AtAnchor, and then the one after that where it reported UnderwaySailing. The final line of the diagram is a single source, combining the events just described in the preceding section of the diagram.](GraphicsIntro/Ch08-Partitioning-Marbles-Status-Changes.svg)

The `perShipObservables` section shows how `GroupBy` creates a separate observable for each distinct vessel. (This diagram shows three vessels, named `A`, `B`, and `C`. With the real source, there would be a lot more observables coming out of `GroupBy`, but the principle remains the same.) We do a bit of work on these group streams before flattening them. As already described, we use `DistinctUntilChanged` and `Skip(1)` to ensure we only produce an event when we know for certain that a vessel's status has changed. (Since we only ever saw `A` reporting a status of `Moored`, then as far as we know its status never changed, which is why its stream is completely empty.) Only then do we flatten it back into a single observable sequence.

Marble diagrams need to be simple to fit on a page, so let's now take a quick look at some real output. This confirms that this is very different from the raw `receiverHost.Messages`. First, I need to attach a subscriber:

```csharp
shipStatusChanges.Subscribe(m => Console.WriteLine(
   $"Vessel {((IAisMessage)m).Mmsi} changed status to {m.NavigationStatus} at {DateTimeOffset.UtcNow}"));
```

If I then let the receiver run for about ten minutes, I see this output:

```
Vessel 257076860 changed status to UnderwayUsingEngine at 23/06/2023 06:42:48 +00:00
Vessel 257006640 changed status to UnderwayUsingEngine at 23/06/2023 06:43:08 +00:00
Vessel 259005960 changed status to UnderwayUsingEngine at 23/06/2023 06:44:23 +00:00
Vessel 259112000 changed status to UnderwayUsingEngine at 23/06/2023 06:44:33 +00:00
Vessel 259004130 changed status to Moored at 23/06/2023 06:44:43 +00:00
Vessel 257076860 changed status to NotDefined at 23/06/2023 06:44:53 +00:00
Vessel 258024800 changed status to Moored at 23/06/2023 06:45:24 +00:00
Vessel 258006830 changed status to UnderwayUsingEngine at 23/06/2023 06:46:39 +00:00
Vessel 257428000 changed status to Moored at 23/06/2023 06:46:49 +00:00
Vessel 257812800 changed status to Moored at 23/06/2023 06:46:49 +00:00
Vessel 257805000 changed status to Moored at 23/06/2023 06:47:54 +00:00
Vessel 259366000 changed status to UnderwayUsingEngine at 23/06/2023 06:47:59 +00:00
Vessel 257076860 changed status to UnderwayUsingEngine at 23/06/2023 06:48:59 +00:00
Vessel 257020500 changed status to UnderwayUsingEngine at 23/06/2023 06:50:24 +00:00
Vessel 257737000 changed status to UnderwayUsingEngine at 23/06/2023 06:50:39 +00:00
Vessel 257076860 changed status to NotDefined at 23/06/2023 06:51:04 +00:00
Vessel 259366000 changed status to Moored at 23/06/2023 06:51:54 +00:00
Vessel 232026676 changed status to Moored at 23/06/2023 06:51:54 +00:00
Vessel 259638000 changed status to UnderwayUsingEngine at 23/06/2023 06:52:34 +00:00
```

The critical thing to understand here is that in the space of ten minutes, `receiverHost.Messages` produced _thousands_ of messages. (The rate varies by time of day, but it's typically over a thousand messages a minute. The code would have processed roughly ten thousand messages when I ran it to produce that output.) But as you can see, `shipStatusChanges` produced just 19 messages.

This shows how Rx can tame high volume event sources in ways that are much more powerful than mere aggregation. We've not just reduced the data down to some statistical measure that can only provide an overview. Statistical measures such as averages or variance are often very useful, but they aren't always able to provide us with the domain-specific insights we want. They wouldn't be able to tell us anything about any particular ship for example. But here, every message tells us something about a particular ship. We've been able to retain that level of detail, despite the fact that we are looking at every ship. We've been able to instruct Rx to tell us any time any ship changes its status.

It may seem like I'm making too big a deal of this, but it took so little effort to achieve this result that it can be easy to miss just how much work Rx is doing for us here. This code does all of the following:

- monitors every single ship operating in Norwegian waters
- provides per-ship information
- reports events at a rate that a human could reasonably cope with

It can take thousands of messages and perform the necessary processing to find the handful that really matter to us.

This is an example of the "fanning out, and then back in again" technique I described in ['The Significance of SelectMany' in the Transformation chapter](06_Transformation.md#the-significance-of-selectmany). This code uses `GroupBy` to fan out from a single observable to multiple observables. The key to this step is to create nested observables that provide the right level of detail for the processing we want to do. In this example that level of detail was "one specific ship" but it wouldn't have to be. You could imagine wanting to group messages by region—perhaps we're interesting in comparing different ports, so we'd want to partition the source based on whichever port a vessel is closest to, or perhaps by its destination port. (AIS provides a way for vessels to broadcast their intended destination.) Having partitioned the data by whatever criteria we require, we then define the processing to be applied for each group. In this case, we just watched for changes to `NavigationStatus`. This step will typically be where the reduction in volume happens. For example, most vessels will only change their `NavigationStatus` a few times a day at most. Having then reduced the notification stream to just those events we really care about, we can combine it back into a single stream that provides the high-value notifications we want.

This power comes at a cost, of course. It didn't take much code to get Rx to do this work for us, but we're causing it to work reasonably hard: it needs to remember every ship it has seen so far, and to maintain an observable source for each one. If our data source has broad enough reach to receive messages from tens of thousands of vessel, Rx will need to maintain tens of thousands of observable sources, one for each vessel. The example shown has nothing resembling an inactivity timeout—a vessel broadcasting even a single message will be remembered for as long as the program runs. (A malicious actor fabricating AIS messages each with a different made up identifier would eventually cause this code to crash by running out of memory.) Depending on your data sources you might need to take steps to avoid unbounded growth of memory usage, so real examples can become more complex than this, but the basic approach is powerful.

Now that we've seen an example, let's look at `GroupBy` in a bit more detail. It comes in a few different flavours. We just used this overload:

```csharp
public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(
    this IObservable<TSource> source, 
    Func<TSource, TKey> keySelector)
```

That overload uses whatever the default comparison behaviour is for your chosen key type. In our case we used `uint` (the type of the `Mmsi` property that uniquely identifies a vessel in an AIS message), which is just a number, so it's an intrinsically comparable type. In some cases you might want non-standard comparison. For example, if you use `string` as a key, you might want to be able to specify a locale-specific case-insensitive comparison. For these scenarios, there's an overload that takes a comparer:

```csharp
public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(
    this IObservable<TSource> source, 
    Func<TSource, TKey> keySelector, 
    IEqualityComparer<TKey> comparer)
```

There are two more overloads that extend the preceding two with an `elementSelector` argument:

```csharp
public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
    this IObservable<TSource> source, 
    Func<TSource, TKey> keySelector, 
    Func<TSource, TElement> elementSelector)
{...}

public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
    this IObservable<TSource> source, 
    Func<TSource, TKey> keySelector, 
    Func<TSource, TElement> elementSelector, 
    IEqualityComparer<TKey> comparer)
{...}
```

This is functionally equivalent to using the `Select` operator after `GroupBy`.

By the way, when using `GroupBy` you might be tempted to `Subscribe` directly to the nested observables:

```csharp
// Don't do it this way. Use the earlier example.
perShipObservables.Subscribe(shipMessages =>
  shipMessages
    .OfType<IAisMessageType1to3>()
    .DistinctUntilChanged(m => m.NavigationStatus)
    .Skip(1)
    .Subscribe(m => Console.WriteLine(
    $"Ship {((IAisMessage)m).Mmsi} changed status to {m.NavigationStatus} at {DateTimeOffset.UtcNow}")));
```

This may seem to have the same effect: `perShipObservables` here is the sequence returned by `GroupBy`, so it will produce a observable stream for each distinct ship. This example subscribes to that, and then uses the same operators as before on each nested sequence, but instead of collecting the results out into a single output observable with `SelectMany`, this explicitly calls `Subscribe` for each nested stream.

This might seem like a more natural way to work if you're unfamiliar with Rx. But although this will seem to produce he same behaviour, it introduces a problem: Rx doesn't understand that these nested subscriptions are associated with the outer subscription. That won't necessarily cause a problem in this simple example, but it could if we start using additional operators. Consider this modification:

```csharp
IDisposable sub = perShipObservables.Subscribe(shipMessages =>
  shipMessages
    .OfType<IAisMessageType1to3>()
    .DistinctUntilChanged(m => m.NavigationStatus)
    .Skip(1)
    .Finally(() => Console.WriteLine($"Nested sub for {shipMessages.Key} ending"))
    .Subscribe(m => Console.WriteLine(
    $"Ship {((IAisMessage)m).Mmsi} changed status to {m.NavigationStatus} at {DateTimeOffset.UtcNow}")));
```

I've added a `Finally` operator for the nested sequence. This enables us to invoke a callback when a sequence comes to an end for any reason. But even if we unsubscribe from the outer sequence (by calling `sub.Dispose();`) this `Finally` will never do anything. That's because Rx has no way of knowing that these inner subscriptions are part of the outer one.

If we made the same modification to the earlier version, in which these nested sequences were collected into one output sequence by `SelectMany`, Rx understands that subscriptions to the inner sequence exist only because of the subscription to the sequence returned by `SelectMany`. (In fact, `SelectMany` is what subscribes to those inner sequences.) So if we unsubscribe from the output sequence in that example, it will correctly run any `Finally` callbacks on any inners sequences.

More generally, if you have lots of sequences coming into existence as part of a single processing chain, it is usually better to get Rx to manage the process from end to end.

## Buffer

The `Buffer` operator is useful if you need to deal with events in batches. This can be useful for performance, especially if you're storing data about events. Take the AIS example. If you wanted to log notifications to a persistent store, the cost of storing a single record is likely to be almost identical to the cost of storing several. Most storage devices operate with blocks of data often several kilobytes in size, so the amount of work required to store a single byte of data is often identical to the amount of work required to store several thousand bytes. The pattern of buffering up data until we have a reasonably large chunk of work crops up all the time in programming. The .NET runtime library's `Stream` class has built-in buffering for exactly this reason, so it's no surprise that it's built into Rx.

Efficiency concerns are not the only reason you might want to process multiple events in one batch instead of individual ones. Suppose you wanted to generate a stream of continuously updated statistics about some source of data. By carving the source into chunks with `Buffer`, you can calculate, say, an average over the last 10 events.

`Buffer` can partition the elements from a source stream, so it's a similar kind of operator to `GroupBy`, but there are a couple of significant differences. First, `Buffer` doesn't inspect the elements to determine how to partition them—it partitions purely based on the order in which elements emerge. Second, `Buffer` waits until it has completely filled a partition, and then presents all of the elements as an `IList<T>`. This can make certain tasks a lot easier because everything in the partition is available for immediate use—values aren't buried in a nested `IObservable<T>`. Third, `Buffer` offers some overloads that make it possible for a single element to turn up in more than one 'partition'. (In this case, `Buffer` is no longer strictly partitioning the data, but as you'll see, it's just a small variation on the other behaviours.)

The simplest way to use `Buffer` is to gather up adjacent elements into chunks. (LINQ to Objects now has an equivalent operator that it calls [`Chunk`](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.chunk). The reason Rx didn't use the same name is that Rx introduced this operator over 10 years before LINQ to Objects did. So the real question is why LINQ to Objects chose a different name. It might be because `Chunk` doesn't support all of the variations that Rx's `Buffer` does, but you'd need to ask the .NET runtime library team.) This overload of `Buffer` takes a single argument, indicating the chunk size you would like:

```csharp
public static IObservable<IList<TSource>> Buffer<TSource>(
    this IObservable<TSource> source, 
    int count)
{...}
```

This example uses it to split navigation messages into chunks of 4, and then goes on to calculate the average speed across those 4 readings:

```csharp
IObservable<IList<IVesselNavigation>> navigationChunks = 
   receiverHost.Messages.Where(v => v.Mmsi == 235009890)
                        .OfType<IVesselNavigation>()
                        .Where(n => n.SpeedOverGround.HasValue)
                        .Buffer(4);

IObservable<float> recentAverageSpeed = 
    navigationChunks.Select(chunk => chunk.Average(n => n.SpeedOverGround.Value));
```

If the source completes, and has not produced an exact multiple of the chunk size, the final chunk will be smaller. We can see this with the following more artificial example:

```csharp
Observable
    .Range(1, 5)
    .Buffer(2)
    .Select(chunk => string.Join(", ", chunk))
    .Dump("chunks");
```

As you can see from this output, the final chunk has just a single item, even though we asked for 2 at a time:

```
chunks-->1, 2
chunks-->3, 4
chunks-->5
chunks completed
```

`Buffer` had no choice here because the source completed, and if it hadn't produced that final under-sized chunk, we would never have seen the final item. But apart from this end-of-source case, this overload of `Buffer` waits until it has collected enough elements to fill a buffer of the specified size before passing it on. That means that `Buffer` introduces a delay. If source items are quite far apart (e.g., when a ship is not moving it might only report AIS navigation data every few minutes) this can lead to long delays.

In some cases, we might want to handle multiple events in a batch when a source is busy without having to wait a long time when the source is operating more slowly. This would be useful in a user interface. If you want to provide fresh information, it might be better to accept an undersized chunk so that you can provide more timely information. For these scenarios, `Buffer` offers overloads that accept a `TimeSpan`:

```csharp
public static IObservable<IList<TSource>> Buffer<TSource>(
    this IObservable<TSource> source, 
    TimeSpan timeSpan)
{...}

public static IObservable<IList<TSource>> Buffer<TSource>(
    this IObservable<TSource> source, 
    TimeSpan timeSpan, 
    int count)
{...}
```

The first of these partitions the source based on nothing but timing. This will emit one chunk every second no matter the rate at which `source` produces value:

```csharp
IObservable<IList<string>> output = source.Buffer(TimeSpan.FromSeconds(1));
```

If `source` happened to emit no values during any particular chunk's lifetime, `output` will emit an empty list.

The second overload, taking both a `timespan` and a `count`, essentially imposes two upper limits: you'll never have to wait longer than `timespan` between chunks, and you'll never receive a chunk with more than `count` elements. As with the `timespan`-only overload, this can deliver under-full and even empty chunks if the source doesn't produce elements fast enough to fill the buffer within the time specified.

### Overlapping buffers

In the preceding section, I showed an example that collected chunks of 4 `IVesselNavigation` entries for a particular vessel, and calculated the average speed. This sort of averaging over multiple samples can be a useful way of smoothing out slight random variations in readings. So the goal in this case wasn't to process items in batches for efficiency, it was to enable a particular kind of calculation.

But there was a problem with the example: because it was averaging 4 readings, it produced an output only once every 4 input messages. And since vessels might report their speed only once every few minutes if they are not moving, we might be waiting a very long time.

There's an overload of `Buffer` that enables us to do a little better: instead of averaging the first 4 readings, and then the 4 readings after that, and then the 4 after that, and so on, we might want to calculate the average of the last 4 readings _every time the vessel reports a new reading.

This is sometimes called a sliding window. We want to process readings 1, 2, 3, 4, then 2, 3, 4, 5, then 3, 4, 5, 6, and so on. There's an overload of buffer that can do this. This example shows the first statement from the earlier average speed example, but with one small modification:

```csharp
IObservable<IList<IVesselNavigation>> navigationChunks = receiverHost.Messages
    .Where(v => v.Mmsi == 235009890)
    .OfType<IVesselNavigation>()
    .Where(n => n.SpeedOverGround.HasValue)
    .Buffer(4, 1);
```

This calls an overload of `Buffer` that takes two `int` arguments. The first does the same thing as before: it indicates that we want 4 items in each chunk. But the second argument indicates how often to produce a buffer. This says we want a buffer for every `1` element (i.e., every single element) that the source produces. (The overload that accepts just a `count` is equivalent to passing the same value for both arguments to this overload.)

So this will wait until the source has produce 4 suitable messages (i.e., messages that satisfy the `Where` and `OfType` operators here) and will then report those first four readings in the first `IList<VesselNavigation>` to emerge from `navigationChunks`. But the source only has to produce one more suitable message, and then this will emit another `IList<VesselNavigation>`, containing 3 of the same value as were in the first chunk, and then the new value. When the next suitable message emerges, this will emit another list with the 3rd, 4th, 5th, and 6th messages, and so on.

This marble diagram illustrates the behaviour for `Buffer(4, 1)`.

![A marble diagram showing two sequences. The first is labelled "Range(1,6)" and shows the numbers 1 to 6. The second is labelled ".Buffer(4,1)", and it shows three events. The colour coding and horizontal position indicate that these emerge at the same time as he final three events in the top diagram. The first event on this second sequence contains a list of numbers, "1,2,3,4", the second shows "2,3,4,5" and the third shows "3,4,5,6".](GraphicsIntro/Ch08-Partitioning-Marbles-Buffer-Marbles.svg)

If we fed this into the same `recentAverageSpeed` expression as the earlier example, we'd still get no output until the 4th suitable message emerges from the source, but from then on, every single suitable message to emerge from the source will emit a new average value. These average values will still always report the average of the 4 most recently reported speeds, but we will now get these averages four times as often.

We could also use this to improve the example earlier that reported when ships changed their `NavigationStatus`. The last example told you what state a vessel had just entered, but this raises an obvious question: what state was it in before? We can use `Buffer(2, 1)` so that each time we see a message indicating a change in status, we also have access to the preceding change in status:

```csharp
IObservable<IList<IAisMessageType1to3>> shipStatusChanges =
    perShipObservables.SelectMany(shipMessages => shipMessages
        .OfType<IAisMessageType1to3>()
        .DistinctUntilChanged(m => m.NavigationStatus)
        .Buffer(2, 1));

IDisposable sub = shipStatusChanges.Subscribe(m => Console.WriteLine(
    $"Ship {((IAisMessage)m[0]).Mmsi} changed status from" +
    $" {m[1].NavigationStatus} to {m[1].NavigationStatus}" +
    $" at {DateTimeOffset.UtcNow}"));
```

As the output shows, we can now report the previous state as well as the state just entered:

```
Ship 259664000 changed status from UnderwayUsingEngine to Moored at 30/06/2023
 13:36:39 +00:00
Ship 257139000 changed status from AtAnchor to UnderwayUsingEngine at 30/06/20
23 13:38:39 +00:00
Ship 257798800 changed status from UnderwayUsingEngine to Moored at 30/06/2023
 13:38:39 +00:00
```

This change enabled us to remove the `Skip`. The earlier example had that because we can't tell whether the first message we receive from any particular ship after startup represents a change. But since we're telling `Buffer` we want pairs of messages, it won't give us anything for any single ship until it has seen messages with two different statuses.

You can also ask for a sliding window defined by time instead of counts using this overload:

```csharp
public static IObservable<IList<TSource>> Buffer<TSource>(
    this IObservable<TSource> source, 
    TimeSpan timeSpan, 
    TimeSpan timeShift)
{...}
```

The `timeSpan` determines the length of time covered by each window, and the `timeShift` determines the interval at which new windows are started.

## Window

The `Window` operator is very similar to the `Buffer`. It can split the input into chunks based either on element count or time, and it also offers support for overlapping windows. However, it has a different return type. Whereas using `Buffer` on an `IObservable<T>` will return an `IObservable<IList<T>>`, `Window` will return an `IObservable<IObservable<T>>`. This means that `Window` doesn't have to wait until it has filled a complete buffer before producing anything. You could say that `Window` more fully embraces the reactive paradigm than `Buffer`. Then again after some experience you might conclude that `Window` is harder to use than `Buffer` but is very rarely any more useful in practice.

Because `Buffer` returns an `IObservable<IList<T>>`, it can't produce a chunk until it has all of the elements that will go into that chunk. `IList<T>` supports random access—you can ask it how many elements it has, and you can retrieve any element by numeric index, and we expect these operations to complete immediately. (It would be technically possible to write an implementation of `IList<T>` representing as yet unreceived data, and to make its `Count` and indexer properties block if you try to use them before that data is available, but this would be a strange thing to do. Developers expect lists to return information immediately, and the lists produced by Rx's `Buffer` meet that expectation.) So if you write, say, `Buffer(4)`, it can't produce anything until it has all 4 items that will constitute the first chunk.

But because `Window` returns an observable that produces a nested observable to represent each chunk, it can emit that before necessarily having all of the elements. In fact, it emits a new window as soon as it knows it will need one. If you use `Window(4, 1)` for example, the observable it returns emits its first nested observable immediately. And then as soon as the source produces its first element, that nested observable will emit that element, and then the second nested observable will be produced. We passed `1` as the 2nd argument to `Window`, so we get a new window for every element the source produces. As soon as the first element has been emitted, the next item the source emits will appear in the second window (and also the first, since we've specified overlapping windows in this case), so the second window is effectively _open_ from immediately after the emergence of the first element. So the `IObservable<IObservable<T>>` that `Window` return produces a new `IObservable<T>` at that point.

Nested observables produce their items as and when they become available. They complete once `Window` knows there will be no further items in that window (i.e., at exactly the same point `Buffer` would have produced the completed `IList<T>` for that window.)

`Window` can seem like it is better than `Buffer` because it lets you get your hands on the individual items in a chunk the instant they are available. However, if you were doing calculations that required access to every single item in the chunk, this doesn't necessarily help you. You're not going to be able to complete your processing until you've received every item in the chunk, so you're not going to produce a final result any earlier, and your code might be more complicated because it can no longer count on having an `IList<T>` conveniently making all of the items available at once. However, if you're calculating some sort of aggregation over the items in a chunk, `Window` might be more efficient because it enables you to process each item as it emerges and then discard it. If a chunk is very large, `Buffer` would have to hold onto every item until the chunk completes, which might use more memory. Moreover, in cases where you don't necessarily need to see every item in a chunk before you can do something useful with those items, `Window` might enable you to avoid introducing processing delays.

`Window` doesn't help us in the AIS `NavigationStatus` example, because the goal there was to report both the _before_ and _after_ status for each change. We can't do that until we know what the _after_ value is, so we would get no benefit from receiving the _before_ value earlier. We need the second value to do what we're trying to do, so we might as well use `Buffer` because it's easier. But if you wanted to keep track of the number of distinct vessels that have reported movement so far today, `Window` would be an appropriate mechanism: you could set it up to produce one window per day, and you would be able to start seeing information within each window without needing to wait until the end of the day.

In addition to supporting simple count-based or duration-based splitting, there are more flexible ways to define the window boundaries, such as this overload:

```csharp
// Projects each element of an observable sequence into consecutive non-overlapping windows.
// windowClosingSelector : A function invoked to define the boundaries of the produced 
// windows. A new window is started when the previous one is closed.
public static IObservable<IObservable<TSource>> Window<TSource, TWindowClosing>
(
    this IObservable<TSource> source, 
    Func<IObservable<TWindowClosing>> windowClosingSelector
)
```

The first of these complex overloads allows us to control when windows close. The `windowClosingSelector` function is called each time a window is created, and each windows will close when the corresponding sequence from the `windowClosingSelector` produces a value. The value is disregarded so it doesn't matter what type the sequence values are; in fact you can just complete the sequence from `windowClosingSelector` to close the window instead.

In this example, we create a window with a closing selector. We return the same subject from that selector every time, then notify from the subject whenever a user presses enter from the console.

```csharp
int windowIdx = 0;
IObservable<long> source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(10);
var closer = new Subject<Unit>();
source.Window(() => closer)
      .Subscribe(window =>
       {
           int thisWindowIdx = windowIdx++;
           Console.WriteLine("--Starting new window");
           string windowName = $"Window{thisWindowIdx}";
           window.Subscribe(
              value => Console.WriteLine("{0} : {1}", windowName, value),
              ex => Console.WriteLine("{0} : {1}", windowName, ex),
              () => Console.WriteLine("{0} Completed", windowName));
       },
       () => Console.WriteLine("Completed"));

string input = "";
while (input != "exit")
{
    input = Console.ReadLine();
    closer.OnNext(Unit.Default);
}
```

Output (when I hit enter after '1' and '5' are displayed):

```
--Starting new window
window0 : 0
window0 : 1

window0 Completed

--Starting new window
window1 : 2
window1 : 3
window1 : 4
window1 : 5

window1 Completed

--Starting new window
window2 : 6
window2 : 7
window2 : 8
window2 : 9

window2 Completed

Completed
```

The most complex overload of `Window` allows us to create potentially overlapping windows.

```csharp
// Projects each element of an observable sequence into zero or more windows.
// windowOpenings : Observable sequence whose elements denote the creation of new windows.
// windowClosingSelector : A function invoked to define the closing of each produced window.
public static IObservable<IObservable<TSource>> Window
    <TSource, TWindowOpening, TWindowClosing>
(
    this IObservable<TSource> source, 
    IObservable<TWindowOpening> windowOpenings, 
    Func<TWindowOpening, IObservable<TWindowClosing>> windowClosingSelector
)
```

This overload takes three arguments

1. The source sequence
2. A sequence that indicates when a new window should be opened
3. A function that takes a window opening value, and returns a window closing sequence

This overload offers great flexibility in the way windows are opened and closed. Windows can be largely independent from each other; they can overlap, vary in size and even skip values from the source.

To ease our way into this more complex overload, let's first try to use it to recreate a simpler version of `Window` (the overload that takes a count). To do so, we need to open a window once on the initial subscription, and once each time the source has produced then specified count. The window needs to close each time that count is reached. To achieve this we only need the source sequence. We will be subscribing to it multiple times, but for some kinds of sources that might cause problems, so we do so via the [`Publish`](15_PublishingOperators.md#publish) operator, which enables multiple subscribers while making only one subscription to the underlying source.

```csharp
public static IObservable<IObservable<T>> MyWindow<T>(
    this IObservable<T> source, 
    int count)
{
    IObservable<T> shared = source.Publish().RefCount();
    IObservable<int> windowEdge = shared
        .Select((i, idx) => idx % count)
        .Where(mod => mod == 0)
        .Publish()
        .RefCount();
    return shared.Window(windowEdge, _ => windowEdge);
}
```

If we now want to extend this method to offer skip functionality, we need to have two different sequences: one for opening and one for closing. We open a window on subscription and again after the `skip` items have passed. We close those windows after '`count`' items have passed since the window opened.

```csharp
public static IObservable<IObservable<T>> MyWindow<T>(
    this IObservable<T> source, 
    int count, 
    int skip)
{
    if (count <= 0) throw new ArgumentOutOfRangeException();
    if (skip <= 0) throw new ArgumentOutOfRangeException();

    IObservable<T> shared = source.Publish().RefCount();
    IObservable<int> index = shared
        .Select((i, idx) => idx)
        .Publish()
        .RefCount();
 
    IObservable<int> windowOpen = index.Where(idx => idx % skip == 0);
    IObservable<int> windowClose = index.Skip(count-1);
 
    return shared.Window(windowOpen, _ => windowClose);
}
```

We can see here that the `windowClose` sequence is re-subscribed to each time a window is opened, due to it being returned from a function. This allows us to reapply the skip (`Skip(count-1)`) for each window. Currently, we ignore the value that the `windowOpen` pushes to the `windowClose` selector, but if you require it for some logic, it is available to you.

As you can see, the `Window` operator can be quite powerful. We can even use `Window` to replicate other operators; for instance we can create our own implementation of `Buffer` that way. We can have the `SelectMany` operator take a single value (the window) to produce zero or more values of another type (in our case, a single `IList<T>`). To create the `IList<T>` without blocking, we can apply the `Aggregate` method and use a new `List<T>` as the seed.

```csharp
public static IObservable<IList<T>> MyBuffer<T>(this IObservable<T> source, int count)
{
    return source.Window(count)
        .SelectMany(window => 
            window.Aggregate(
                new List<T>(), 
                (list, item) =>
                {
                    list.Add(item);
                    return list;
                }));
}
```

You might find it to be an interesting exercise to try implementing other time shifting methods, like `Sample` or `Throttle`, with `Window`.

We've seen a few useful ways to spread a single stream of items across multiple output sequences, using either data-driven grouping criteria, or time-based chunking with either `Buffer` or `Window`. In the next chapter, we'll look at operators that can combine together data from multiple streams.