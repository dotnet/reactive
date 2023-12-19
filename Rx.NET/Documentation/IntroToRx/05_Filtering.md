# Filtering

Rx provides us with tools to take potentially vast quantities of events and process these to produce higher level insights. This can often involve a reduction in volume. A small number of events may be more useful than a large number if the individual events in that lower-volume stream are, on average, more informative. The simplest mechanisms for achieving this involve simply filtering out events we don't want. Rx defines several operators that can do this.

Just before we move on to introducing the new operators, we will quickly define an extension method to help illuminate several of the examples. This `Dump` extension method subscribes to any `IObservable<T>` with handlers that display messages for each notification the source produces. This method takes a `name` argument, which will be shown as part of each message, enabling us to see where events came from in examples that subscribe to more than one source.

```csharp
public static class SampleExtensions
{
    public static void Dump<T>(this IObservable<T> source, string name)
    {
        source.Subscribe(
            value =>Console.WriteLine($"{name}-->{value}"), 
            ex => Console.WriteLine($"{name} failed-->{ex.Message}"),
            () => Console.WriteLine($"{name} completed"));
    }
}
```

## Where

Applying a filter to a sequence is an extremely common exercise and the most straightforward filter in LINQ is the `Where` operator. As usual with LINQ, Rx provides its operators in the form of extension methods. If you are already familiar with LINQ, the signature of Rx's `Where` method will come as no surprise:

```csharp
IObservable<T> Where<T>(this IObservable<T> source, Func<T, bool> predicate)
```

Note that the element type is the same for the `source` parameter as it is for the return type. This is because `Where` doesn't modify elements. It can filter some out, but those that it does not remove are passed through unaltered.

This example uses `Where` to filter out all odd values produced from a `Range` sequence, meaning only even numbers will emerge.

```csharp
IObservable<int> xs = Observable.Range(0, 10); // The numbers 0-9
IObservable<int> evenNumbers = xs.Where(i => i % 2 == 0);

evenNumbers.Dump("Where");
```

Output:

```
Where-->0
Where-->2
Where-->4
Where-->6
Where-->8
Where completed
```

The `Where` operator is one of the many standard LINQ operators you'll find on all LINQ providers. LINQ to Objects, the `IEnumerable<T>` implementation, provides an equivalent method, for example. In most cases, Rx's operators behave just as they do in the `IEnumerable<T>` implementations, although there are some exceptions as we'll see later. We will discuss each implementation and explain any variation as we go. By implementing these common operators Rx also gets language support for free via C# query expression syntax. For example, we could have written the first statement this way, and it would have compiled to effectively identical code:

```csharp
IObservable<int> evenNumbers =
    from i in xs
    where i % 2 == 0
    select i;
```

The examples in this book mostly use extension methods, not query expressions, partly because Rx implements some operators for which there is no corresponding query syntax, and partly because the method call approach can sometimes make it easier to see what is happening.

As with most Rx operators, `Where` does not subscribe immediately to its source. (Rx LINQ operators are much like those in LINQ to Objects: the `IEnumerable<T>` version of `Where` returns without attempting to enumerate its source. It's only when something attempts to enumerate the `IEnumerable<T>` that `Where` returns that it will in turn start enumerating the source.) Only when something calls `Subscribe` on the `IObservable<T>` returned by `Where` will it call `Subscribe` on its source. And it will do so once for each such call to `Subscribe`. More generally, when you chain LINQ operators together, each `Subscribe` call on the resulting `IObservable<T>` results in a cascading series of calls to `Subscribe` all the way down the chain.

A side effect of this cascading `Subscribe` is that `Where` (like most other LINQ operators) is neither inherently _hot_ or _cold_: since it just subscribes to its source, then it will be hot if its source is hot, and cold if its source is cold.

The `Where` operator passes on all elements for which its `predicate` callback returns `true`. To be more precise, when you subscript to `Where`, it will create its own `IObserver<T>` which it passes as the argument to `source.Subscribe`, and this observer invokes the `predicate` for each call to `OnNext`. If that predicate returns `true`, then and only then will the observer created by `Where` call `OnNext` on the observer that you passed to `Where`.

`Where` always passes the final call to either `OnComplete` or `OnError` through. That means that if you were to write this:

```csharp
IObservable<int> dropEverything = xs.Where(_ => false);
```

then although this would filter out all elements (because the predicate ignores its argument and always returns `false`, instructing `Where` to drop everything), this won't filter out an error or completion.

In fact if that's what you want—an operator that drops all the elements and just tells you when a source completes or fails—there's a simpler way.

## IgnoreElements

The `IgnoreElements` extension method allows you to receive just the `OnCompleted` or `OnError` notifications. It is equivalent to using the `Where` operator with a predicate that always returns `false`, as this example illustrates:

```csharp
IObservable<int> xs = Observable.Range(1, 3);
IObservable<int> dropEverything = xs.IgnoreElements();

xs.Dump("Unfiltered");
dropEverything.Dump("IgnoreElements");
```

As the output shows, the `xs` source produces the numbers 1 to 3 then completes, but if we run that through `IgnoreElements`, all we see is the `OnCompleted`.

```
Unfiltered-->1
Unfiltered-->2
Unfiltered-->3
Unfiltered completed
IgnoreElements completed
```

## OfType

Some observable sequences produce items of various types. For example, consider an application that wants to keep track of ships as they move. This is possible with an AIS receiver. AIS is the Automatic Identification System, which most ocean-going ships use to report their location, heading, speed, and other information. There are numerous kinds of AIS message. Some report a ship's location and speed, but its name is reported in a different kind of message. (This is because most ships move more often than they change their names, so they broadcast these two types of information at quite different intervals.)

Imagine how this might look in Rx. Actually you don't have to imagine it. The open source [Ais.Net project](https://github.com/ais-dotnet) includes a [`ReceiverHost` class](https://github.com/ais-dotnet/Ais.Net.Receiver/blob/15de7b2908c3bd67cf421545578cfca59b24ed2c/Solutions/Ais.Net.Receiver/Ais/Net/Receiver/Receiver/ReceiverHost.cs) that makes AIS messages available through Rx. The `ReceiverHost` defines a `Messages` property of type `IObservable<IAisMessage>`. Since AIS defines numerous message types, this observable source can produce many different kinds of objects. Everything it emits will implement the [`IAisMessage` interface](https://github.com/ais-dotnet/Ais.Net.Receiver/blob/15de7b2908c3bd67cf421545578cfca59b24ed2c/Solutions/Ais.Net.Models/Ais/Net/Models/Abstractions/IAisMessage.cs), which reports the ship's unique identifier, but not much else. But the [`Ais.Net.Models` library](https://www.nuget.org/packages/Ais.Net.Models/) defines numerous other interfaces, including [`IVesselNavigation`](https://github.com/ais-dotnet/Ais.Net.Receiver/blob/15de7b2908c3bd67cf421545578cfca59b24ed2c/Solutions/Ais.Net.Models/Ais/Net/Models/Abstractions/IVesselNavigation.cs), which reports location, speed, and heading, and [`IVesselName`](https://github.com/ais-dotnet/Ais.Net.Receiver/blob/15de7b2908c3bd67cf421545578cfca59b24ed2c/Solutions/Ais.Net.Models/Ais/Net/Models/Abstractions/IVesselName.cs), which tells you the vessel's name.

Suppose you are interested only in the locations of vessels in the water, and you don't care about the vessels' names. You will want to see all messages that implement the `IVesselNavigation` interface, and to ignore all those that don't. You could try to achieve this with the `Where` operator:

```csharp
// Won't compile!
IObservable<IVesselNavigation> vesselMovements = 
   receiverHost.Messages.Where(m => m is IVesselNavigation);
```

However, that won't compile. You will get this error:

```
Cannot implicitly convert type 
'System.IObservable<Ais.Net.Models.Abstractions.IAisMessage>' 
to 
'System.IObservable<Ais.Net.Models.Abstractions.IVesselNavigation>'
```

Remember that the return type of `Where` is always the same as its input. Since `receiverHost.Messages` is of type `IObservable<IAisMessage>`, that's is also the type that `Where` will return. It so happens that our predicate ensures that only those messages that implement `IVesselNavigation` make it through, but there's no way for the C# compiler to understand the relationship between the predicate and the output. (For all it knows, `Where` might do the exact opposite, including only those elements for which the predicate returns `false`. In fact the compiler can't guess anything about how `Where` might use its predicate.)

Fortunately, Rx provides an operator specialized for this case. `OfType` filters items down to just those that are of a particular type. Items must be either the exact type specified, or inherit from it, or, if it's an interface, they must implement it. This enables us to fix the last example:

```csharp
IObservable<IVesselNavigation> vesselMovements = 
   receiverHost.Messages.OfType<IVesselNavigation>();
```

## Positional Filtering

Sometimes, we don't care about what an element is, so much as where it is in the sequence. Rx defines a few operators that can help us with this.

### FirstAsync and FirstOrDefaultAsync

LINQ providers typically implement a `First` operator that provides the first element of a sequence. Rx is no exception, but the nature of Rx means we typically need this to work slightly differently. With providers for data at rest (such as LINQ to Objects or Entity Framework Core) the source elements already exist, so retrieving the first item is just a matter of reading it. But with Rx, sources produce data when they choose, so there's no way of knowing when the first item will become available.

So with Rx, we typically use `FirstAsync`. This returns an `IObservable<T>` that will produce the first value that emerges from the source sequence and will then complete. (Rx does also offer a more conventional `First` method, but it can be problematic. See the [**Blocking Versions of First/Last/Single[OrDefault]** section later](#blocking-versions-of-firstlastsingleordefault) for details.)

For example, this code uses the AIS.NET source introduced earlier to report the first time a particular boat (the aptly named HMS Example, as it happens) reports that it is moving:

```csharp
uint exampleMmsi = 235009890;
IObservable<IVesselNavigation> moving = 
   receiverHost.Messages
    .Where(v => v.Mmsi == exampleMmsi)
    .OfType<IVesselNavigation>()
    .Where(vn => vn.SpeedOverGround > 1f)
    .FirstAsync();
```

As well as using `FirstAsync`, this also uses a couple of the other filter elements already described. It starts with a [`Where`](#where) step that filters messages down to those from the one boat we happen to be interested in. (Specifically, we filter based on that boat's [Maritime Mobile Service Identity, or MMSI](https://en.wikipedia.org/wiki/Maritime_Mobile_Service_Identity).) Then we use [`OfType`](#oftype) so that we are looking only at those messages that report how/whether the vessel is moving. Then we use another `Where` clause so that we can ignore messages indicating that the boat is not actually moving, finally, we use `FirstAsync` so that we get only the first message indicating movement. As soon as the boat moves, this `moving` source will emit a single `IVesselNavigation` event and will then immediately complete.

We can simplify that query slightly, because `FirstAsync` optionally takes a predicate. This enables us to collapse the final `Where` and `FirstAsync` into a single operator:

```csharp
IObservable<IVesselNavigation> moving = 
   receiverHost.Messages
    .Where(v => v.Mmsi == exampleMmsi)
    .OfType<IVesselNavigation>()
    .FirstAsync(vn => vn.SpeedOverGround > 1f);
```

What if the input to `FirstAsync` is empty? If its completes without ever producing an item, `FirstAsync` invokes its subscriber's `OnError`, passing an `InvalidOperationException` with an error message reporting that the sequence contains no elements. The same is true if we're using the form that takes a predicate (as in this second example), and no elements matching the predicate emerged. This is consistent with the LINQ to Objects `First` operator. (Note that we wouldn't expect this to happen with the examples just shown, because the source will continue to report AIS messages for as long as the application is running, meaning there's no reason for it ever to complete.)

Sometimes, we might want to tolerate this kind of absence of events. Most LINQ providers offer not just `First` but `FirstOrDefault`. We can use this by modify the preceding example. This uses the [`TakeUntil` operator](#skipuntil-and-takeuntil) to introduce a cut-off time: this example is prepared to wait for 5 minutes, but gives up after that. (So although the AIS receiver can produce messages endlessly, this example has decided it won't wait forever.) And since that means we might complete without ever seeing the boat move, we've replaced `FirstAsync` with `FirstOrDefaultAsync`:

```csharp
IObservable<IVesselNavigation?> moving = 
   receiverHost.Messages
    .Where(v => v.Mmsi == exampleMmsi)
    .OfType<IVesselNavigation>()
    .TakeUntil(DateTimeOffset.Now.AddMinutes(5))
    .FirstOrDefaultAsync(vn => vn.SpeedOverGround > 1f);
```

If, after 5 minutes, we've not seen a message from the boat indicating that it's moving at 1 knot or faster, `TakeUntil` will unsubscribe from its upstream source and will call `OnCompleted` on the observer supplied by `FirstOrDefaultAsync`. Whereas `FirstAsync` would treat this as an error, `FirstOrDefaultAsync` will produce the default value for its element type (`IVesselNavigation` in this case; the default value for an interface type is `null`), pass that to its subscriber's `OnNext`, and then call `OnCompleted`.

In short, this `moving` observable will always produce exactly one item. Either it will produce an `IVesselNavigation` indicating that the boat has moved, or it will produce `null` to indicate that this didn't happen in the 5 minutes that this code has allowed.

This production of a `null` might be an OK way to indicate that something didn't happen, but there's something slightly clunky about it: anything consuming this `moving` source now has to work out whether a notification signifies the event of interest, or the absence of any such event. If that happens to be convenient for your code, then great, but Rx provides a more direct way to represent the absence of an event: an empty sequence.

You could imagine a _first or empty_ operator that worked this way. This wouldn't make sense for LINQ providers that return an actual value. For example, as LINQ to Objects' `First` returns `T`, not `IEnumerable<T>`, so there's no way for it to return an empty sequence. But because Rx's offers `First`-like operators that return `IObservable<T>`, it would be technically possible to have an operator that returns either the first item or no items at all. There is no such operator built into Rx, but we can get exactly the same effect by using a more generalised operator, `Take`.

### Take

`Take` is a standard LINQ operator that takes the first few items from a sequence and then discards the rest.

In a sense, `Take` is a generalization of `First`: `Take(1)` returns only the first item, so you could think of LINQ's `First` as being a special case of `Take`. That's not strictly correct because these operators respond differently to missing elements: as we've just seen, `First` (and Rx's `FirstAsync`) insists on receiving at least one element, producing an `InvalidOperationException` if you supply it with an empty sequence. Even the more existentially relaxed `FirstOrDefault` still insists on producing something. `Take` works slightly differently.

If the input to `Take` completes before producing as many elements as have been specified, `Take` does not complain—it just forwards whatever the source has provided. If the source did nothing other than call `OnCompleted`, then `Take` just calls `OnCompleted` on its observer. If we used `Take(5)`, but the source produced three items and then completed, `Take(5)` will forward those three items to its subscriber, and will then complete. This means we could use `Take` to implement the hypothetical `FirstOrEmpty` discussed in the preceding section:

```csharp
public static IObservable<T> FirstOrEmpty<T>(this IObservable<T> src) => src.Take(1);
```

Now would be a good time to remind you that most Rx operators (and all the ones in this chapter) are not intrinsically either hot or cold. They defer to their source. Given some hot `source`, `source.Take(1)` is also hot. The AIS.NET `receiverHost.Messages` source I've been using in these examples is hot (because it reports live message broadcasts from ships), so observable sequences derived from it are also hot. Why is now a good time to discuss this? Because it enables me to make the following absolutely dreadful pun:

```csharp
IObservable<IAisMessage> hotTake = receiverHost.Messages.Take(1);
```

Thank you. I'm here all week.

The `FirstAsync` and `Take` operators work from the start of the sequence. What if we're interested only in the tail end?

### LastAsync, LastOrDefaultAsync, and PublishLast

LINQ providers typically provide `Last` and `LastOrDefault`. These do almost exactly the same thing as `First` or `FirstOrDefault` except, as the name suggests, they return the final element instead of the first one. As with `First`, the nature of Rx means that unlike with LINQ providers working with data at rest, the final element might not be just sitting there waiting to be fetched. So just as Rx offers `FirstAsync` and `FirstOrDefault`, it offers `LastAsync` and `LastOrDefaultAsync`. (It does also offer `Last`, but again, as the [Blocking Versions of First/Last/Single[OrDefault]](#blocking-versions-of-firstlastsingleordefault) section discusses, this can be problematic.)

There is also [`PublishLast`](15_PublishingOperators.md#publishlast). This has similar semantics to `LastAsync` but it handles multiple subscriptions differently. Each time you subscribe to the `IObservable<T>` that `LastAsync` returns, it will subscribe to the underlying source, but `PublishLast` makes only a single `Subscribe` call to the underlying source. (To provide control over exactly when this happens, `PublishLast` returns an `IConnectableObservable<T>`. As the [Hot and Cold Sources section of chapter 2](02_KeyTypes.md#hot-and-cold-sources) described, this provides a `Connect` method, and the connectable observable returned by `PublishLast` subscribes to its underlying source when you call this.) Once this single subscription receives an `OnComplete` notification from the source, it will deliver the final value to all subscribers. (It also remembers the final value, so if any new observers subscribe after the final value has been produced, they will immediately receive that value when they subscribe.) The final value is immediately followed by an `OnCompleted` notification. This is one of a family of operators based on the [`Multicast`](15_PublishingOperators.md#multicast) operator described in more detail in later chapters.

The distinction between `LastAsync` and `LastOrDefaultAsync` is the same as with `FirstAsync` and `FirstOrDefaultAsync`. If the source completes having produced nothing, `LastAsync` reports an error, whereas `LastOrDefaultAsync` emits the default value for its element type and then completes. `PublishLast` handles an empty source differently again: if the source completes without producing any elements, the observable returned by `PublishLast` will do the same: it produces neither an error nor a default value in this scenario.

Reporting the final element of a sequence entails a challenge that `First` does not face. It's very easy to know when you've received the first element from a source: if the source produces an element, and it hasn't previously produced an element, then that's the first element right there. This means that operators such as `FirstAsync` can report the first element immediately. But `LastAsync` and `LastOrDefaultAsync` don't have that luxury.

If you receive an element from a source, how do you know that it is the last element? In general, you can't know this at the instant that you receive it. You will only know that you have received the last element when the source goes on to invoke your `OnCompleted` method. This won't necessarily happen immediately. An earlier example used `TakeUntil(DateTimeOffset.Now.AddMinutes(5))` to bring a sequence to an end after 5 minutes, and if you do that, it's entirely possible that a significant amount of time might elapse between the final element being emitted, and `TakeUntil` shutting things down. In the AIS scenario, boats might only emit messages once every few minutes, so it's quite plausible that we could end up with `TakeUntil` forwarding a message, and then discovering a few minutes later that the cutoff time has been reached without any further messages coming in. Several minutes could have elapsed between the final `OnNext` and the `OnComplete`.

Because of this. `LastAsync` and `LastOrDefaultAsync` emit nothing at all until their source completes. **This has an important consequence:** there might be a significant delay between `LastAsync` receiving the final element from the source, and it forwarding that element to its subscriber.

### TakeLast

Earlier we saw that Rx implements the standard `Take` operator, which forwards up to a specified number of elements from the start of a sequence and then stops. `TakeLast` forwards the elements at the end of a sequence. For example, `TakeLast(3)` asks for the final 3 elements of the source sequence. As with `Take`, `TakeLast` is tolerant of sources that produce too few items. If a source produces fewer than 3 items, `TaskLast(3)` will just forward the entire sequence.

`TakeLast` faces the same challenge as `Last`: it doesn't know when it is near the end of the sequence. It therefore has to hold onto copies of the most recently seen values. It needs memory to hold onto however many values you've specified. If you write `TakeLast(1_000_000)`, it will need to allocate a buffer large enough to store 1,000,000 values. It doesn't know if the first element it receives will be one of the final million. It can't know that until either the source completes, or the source has emitted more than 1,000,000 items. When the source finally does complete, `TakeLast` will now know what the final million elements were and will need to pass all of them to its subscriber's `OnNext` method one after another.

### Skip and SkipLast

What if we want the exact opposite of the `Take` or `TakeLast` operators? Instead of taking the first 5 items from a source, maybe I want to discard the first 5 items instead? Perhaps I have some `IObservable<float>` taking readings from a sensor, and I have discovered that the sensor produces garbage values for its first few readings, so I'd like to ignore those, and only start listening once it has settled down. I can achieve this with `Skip(5)`.

`SkipLast` does the same thing at the end of the sequence: it omits the specified number of elements at the tail end. As with some of the other operators we've just been looking at, this has to deal with the problem that it can't tell when it's near the end of the sequence. It only gets to discover which were the last (say) 4 elements after the source has emitted all of them, followed by an `OnComplete`. So `SkipLast` will introduce a delay. If you use `SkipLast(4)`, it won't forward the first element that the source produces until the source produces a 5th element. So it doesn't need to wait for `OnCompleted` or `OnError` before it can start doing things, it just has to wait until its certain that an element is not one of the ones we want to discard.

The other key methods to filtering are so similar I think we can look at them as one big group. First we will look at `Skip` and `Take`. These act just like they do for the `IEnumerable<T>` implementations. These are the most simple and probably the most used of the Skip/Take methods. Both methods just have the one parameter; the number of values to skip or to take.

### SingleAsync and SingleOrDefaultAsync

LINQ operators typically provide a `Single` operator, for use when a source should provide exactly one item, and it would be an error for it to contain more, or for it to be empty. The same Rx considerations apply here as for `First` and `Last`, so you will probably be unsurprised to learn that Rx offers a `SingleAsync` method that returns an `IObservable<T>` that will either call its observer's `OnNext` exactly once, or will call its `OnError` to indicate either that the source reported an error, or that the source did not produce exactly one item.

With `SingleAsync`, you will get an error if the source is empty, just like with `FirstAsync` and `LastAsync`, but you will also get an error if the source contains multiple items. There is a `SingleOrDefault` which, like its first/last counterparts, tolerates an empty input sequence, generating a single element with the element type's default value in that case.

`Single` and `SingleAsync` share with `Last` and `LastAsync` the characteristic that they don't initially know when they receive an item from the source whether it should be the output. That may seem odd: since `Single` requires the source stream to provide just one item, surely it must know that the item it will deliver to its subscriber will be the first item it receives. This is true, but the thing it doesn't yet know when it receives the first item is whether the source is going to produce a second one. It can't forward the first item unless and until the source completes. We could say that `SingleAsync`'s job is to first verify that the source contains exactly one item, and then to forward that item if it does, but to report an error if it does not. In the error case, `SingleAsync` will know it has gone wrong if it ever receives a second item, so it can immediately call `OnError` on its subscriber at that point. But in the success scenario, it can't know that all is well until the source confirms that nothing more is coming by completing. Only then will `SingleAsync` emit the result.

### Blocking Versions of First/Last/Single[OrDefault]

Several of the operators described in the preceding sections end in the name `Async`. This is a little strange because normally, .NET methods that end in `Async` return a `Task` or `Task<T>`, and yet these all return an `IObservable<T>`. Also, as already discussed, each of these methods corresponds to a standard LINQ operator which does not generally end in `Async`. (And to further add to the confusion, some LINQ providers such as Entity Framework Core do include `Async` versions of some of these operators, but they are different. Unlike Rx, these do in fact return a `Task<T>`, so they still produce a single value, and not an `IQueryable<T>` or `IEnumerable<T>`.) This naming arises from an unfortunate choice early in Rx's design.

If Rx were being designed from scratch today, the relevant operators in the preceding section would just have the normal names: `First`, and `FirstOrDefault`, and so on. The reason they all end with `Async` is that these were added in Rx 2.0, and Rx 1.0 had already defined operators with those names. This example uses the `First` operator:

```csharp
int v = Observable.Range(1, 10).First();
Console.WriteLine(v);
```

This prints out the value `1`, which is the first item returned by `Range` here. But look at the type of that variable `v`. It's not an `IObservable<int>`, it's just an `int`. What would happen if we used this on an Rx operator that didn't immediately produce values upon subscription? Here's one example:

```csharp
long v = Observable.Timer(TimeSpan.FromSeconds(2)).First();
Console.WriteLine(v);
```

If you run this, you'll find that the call to `First` doesn't return until a value is produced. It is a _blocking_ operator. We typically avoid blocking operators in Rx, because it's easy to create deadlocks with them. The whole point of Rx is that we can create code that reacts to events, so to just sit and wait until a specific observable source produces a value is not really in the spirit of things. If you find yourself wanting to do that, there are often better ways to achieve the results you're looking for. (Or perhaps Rx isn't good model for whatever you're doing.)

If you really do need to wait for a value like this, it might be better to use the `Async` forms in conjunction with Rx's integrated support for C#'s `async`/`await`:

```csharp
long v = await Observable.Timer(TimeSpan.FromSeconds(2)).FirstAsync();
Console.WriteLine(v);
```

This logically has the same effect, but because we're using `await`, this won't block the calling thread while it waits for the observable source to produce a value. This might reduce the chances of deadlock.

The fact that we're able to use `await` makes some sense of the fact that these methods end with `Async`, but you might be wondering what's going on here. We've seen that these methods all return `IObservable<T>`, not `Task<T>`, so how are we able to use `await`? There's a [full explanation in the Leaving Rx's World chapter](13_LeavingIObservable.md#integration-with-async-and-await), but the short answer is that Rx provides extension methods that enable this to work. When you `await` an observable sequence, the `await` will complete once the source completes, and it will return the final value that emerges from the source. This works well for operators such as `FirstAsync` and `LastAsync` that produce exactly one item.

Note that there are occasionally situations in which values are available immediately. For example, the [`BehaviourSubject<T>` section in chapter 3](03_CreatingObservableSequences.md#behaviorsubjectt), showed that the defining feature of `BehaviourSubject<T>` is that it always has a current value. That means that Rx's `First` method won't actually block—it will subscribe to the `BehaviourSubject<T>`, and `BehaviourSubject<T>.Subscribe` calls `OnNext` on its subscriber's observable before returning. That enables `First` to return a value immediately without blocking. (Of course, if you use the overload of `First` that accepts a predicate, and if the `BehaviourSubject<T>`'s value doesn't satisfy the predicate, `First` will then block.)

### ElementAt

There is yet another standard LINQ operator for selecting one particular element from the source: `ElementAt`. You provide this with a number indicating the position in the sequence of the element you require. In data-at-rest LINQ providers, this is logically equivalent to accessing an array element by index. Rx implements this operator, but whereas most LINQ providers' `ElementAt<T>` implementation returns a `T`, Rx's returns an `IObservable<T>`. Unlike with `First`, `Last`, and `Single`, Rx does not provide a blocking form of `ElementAt<T>`. But since you can await any `IObservable<T>`, you can always do this:

```csharp
IAisMessage fourth = await receiverHost.Message.ElementAt(4);
```

If your source sequence only produces five values and we ask for `ElementAt(5)`, the sequence that `ElementAt` returns will report an `ArgumentOutOfRangeException` error to its subscriber when the source completes. There are three ways we can deal with this:

- Handle the OnError gracefully
- Use `.Skip(5).Take(1);` This will ignore the first 5 values and the only take the 6th value. 
If the sequence has less than 6 elements we just get an empty sequence, but no errors.
- Use `ElementAtOrDefault`

`ElementAtOrDefault` extension method will protect us in case the index is out of range, by pushing the `default(T)` value. Currently there is not an option to provide your own default value.

## Temporal Filtering

The `Take` and `TakeLast` operators let us filter out everything except elements either at the very start or very end (and `Skip` and `SkipLast` let us see everything but those), but these all require us to know the exact number of elements. What if we want to specify the cut-off not in terms of an element count, but in terms of a particular instant in time?

In fact you've already seen one example: earlier I used `TakeUntil` to convert an endless `IObservable<T>` into one that would complete after five minutes. This is one of a family of operators.

### SkipWhile and TakeWhile

In the [`Skip` and `SkipLast` section](#skip-and-skiplast), I described a sensor that produces garbage values for its first few readings. This is quite common. For example, gas monitoring sensors often need to get some component up to a correct operating temperature before they can produce accurate readings. In the example in that section, I used `Skip(5)` to ignore the first few readings, but that is a crude solution. How do we know that 5 is enough? Or might it be ready sooner, in which case 5 is too few.

What we really want to do is discard readings until we know the readings will be valid. And that's exactly the kind of scenario that `SkipWhile` can be useful for. Suppose we have a gas sensor that reports concentrations of some particular gas, but which also reports the temperature of the sensor plate that is performing the detection. Instead of hoping that 5 readings is a sensible number to skip, we could express the actual requirement:

```csharp
const int MinimumSensorTemperature = 74;
IObservable<SensorReading> readings = sensor.RawReadings
    .SkipUntil(r => r.SensorTemperature >= MinimumSensorTemperature);
```

This directly expresses the logic we require: this will discard readings until the device is up to its minimum operating temperature.

The next set of methods allows you to skip or take values from a sequence while a predicate evaluates to true. For a `SkipWhile` operation this will filter out all values until a value fails the predicate, then the remaining sequence can be returned.

```csharp
var subject = new Subject<int>();
subject
    .SkipWhile(i => i < 4)
    .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
subject.OnNext(1);
subject.OnNext(2);
subject.OnNext(3);
subject.OnNext(4);
subject.OnNext(3);
subject.OnNext(2);
subject.OnNext(1);
subject.OnNext(0);

subject.OnCompleted();
```

Output:

```
4
3
2
1
0
Completed

```

`TakeWhile` will return all values while the predicate passes, and when the first value fails the sequence will complete.

```csharp
var subject = new Subject<int>();
subject
    .TakeWhile(i => i < 4)
    .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
subject.OnNext(1);
subject.OnNext(2);
subject.OnNext(3);
subject.OnNext(4);
subject.OnNext(3);
subject.OnNext(2);
subject.OnNext(1);
subject.OnNext(0);

subject.OnCompleted();
```

Output:

```
1
2
3
Completed

```

### SkipUntil and TakeUntil

In addition to `SkipWhile` and `TakeWhile`, Rx defines `SkipUntil` and `TakeUntil`. These may sound like nothing more than an alternate expression of the same idea: you might expect `SkipUntil` to do almost exactly the same thing as `SkipWhile`, with the only difference being that `SkipWhile` runs for as long as its predicate returns `true`, whereas `SkipUntil` runs for as long as its predicate returns `false`. And there is an overload of `SkipUntil` that does exactly that (and a corresponding one for `TakeUntil`). If that's all these were they wouldn't be interesting. However, there are overloads of `SkipUntil` and `TakeUntil` that enable us to do things we can't do with `SkipWhile` and `TakeWhile`.

You've already seen one example. The [`FirstAsync` and `FirstOrDefaultAsync`](#firstasync-and-firstordefaultasync) included an example that used an overload of `TakeUntil` that accepted a `DateTimeOffset`. This wraps any `IObservable<T>`, returning an `IObservable<T>` that will forward everything from the source until the specified time, at which point it will immediately complete (and will unsubscribe from the underlying source).

We couldn't have achieved this with `TakeWhile`, because that consults its predicate only when the source produces an item. If we want the source to complete at a specific time, the only way we could do that with `TakeWhile` is if its source happens to produce an item at the exact moment we wanted to finish. `TakeWhile` will only ever complete as a result of its source producing an item. `TakeUntil` can complete asynchronously. If we specified a time 5 minutes into the future, it doesn't matter if the source is completely idle when that time arrives. `TakeUntil` will complete anyway. (It relies on [Schedulers](11_SchedulingAndThreading.md#schedulers) to be able to do this.)

We don't have to use a time, `TakeUntil` offers an overload that accept a second `IObservable<T>`. This enables us to tell it to stop when something interesting happens, without needing to know in advance exactly when that will occur. This overload of `TakeUntil` forwards items from the source until that second `IObservable<T>` produces a value. `SkipUntil` offers a similar overload in which the second `IObservable<T>` determines when it should start forwarding items from the source.

**Note**: these overloads require the second observable to produce a value in order to trigger the start or end. If that second observable completes without producing a single notification, then it has no effect—`TakeUntil` will continue to take items indefinitely; `SkipUntil` will never produce anything. In other words, these operators would treat `Observable.Empty<T>()` as being effectively equivalent to `Observable.Never<T>()`.

### Distinct and DistinctUntilChanged

`Distinct` is yet another standard LINQ operator. It removes duplicates from a sequence. To do this, it needs to remember all the values that its source has ever produced, so that it can filter out any items that it has seen before. Rx includes an implementation of `Distinct`, and this example uses it to display the unique identifier of vessels generating AIS messages, but ensuring that we only display each such identifier the first time we see it:

```csharp
IObservable<uint> newIds = receiverHost.Messages
    .Select(m => m.Mmsi)
    .Distinct();

newIds.Subscribe(id => Console.WriteLine($"New vessel: {id}"));
```

(This is leaping ahead a little—it uses `Select`, which we'll get to in [the Transformation of Sequences chapter](06_Transformation.md). However, this is a very widely used LINQ operator, so you are probably already familiar with it. I'm using it here to extract just the MMSI—the vessel identifier—from the message.)

This example is fine if we are only interested in vessels' identifiers. But what if we want to inspect the detail of these messages? How can we retain the ability to see messages only for vessels we've never previously heard of, but still be able to look at the information in those message? The use of `Select` to extract the id stops us from doing this. Fortunately, `Distinct` provides an overload enabling us to change how it determines uniqueness. Instead of getting `Distinct` to look at the values it is processing, we can provide it with a function that lets us pick whatever characteristics we like. So instead of filtering the stream down to values that have never been seen before, we can instead filter the stream down to values that have some particular property or combination of properties we've never seen before. Here's a simple example:

```csharp
IObservable<IAisMessage> newVesselMessages = 
   receiverHost.Messages.Distinct(m => m.Mmsi);
```

Here, the input to `Distinct` is now an `IObservable<IAisMessage>`. (In the preceding example it was actually `IObservable<uint>`, because the `Select` clause picked out just the MMSI.) So `Distinct` now receives the entire `IAisMessage` each time the source emits one. But because we've supplied a callback, it's not going try and compare entire `IAisMessage` messages with one another. Instead, each time it receives one, it passes that to our callback, and then looks at the value our callback returns, and compares that with the values the callback returned for all previously seen messages, and lets the message through only if that's new.

So the effect is similar to before. Messages will be allowed through only if they have an MMSI not previously seen. But the difference is that the `Distinct` operator's output here is `IObservable<IAisMessage>`, so when `Distinct` lets an item through, the entire original message remains available.

In addition to the standard LINQ `Distinct` operator, Rx also provides `DistinctUntilChanged`. This only lets through notifications when something has changed, which it achieved by filtering out only adjacent duplicates. For example, given the sequence `1,2,2,3,4,4,5,4,3,3,2,1,1` it would produce `1,2,3,4,5,4,3,2,1`. Whereas `Distinct` remembers every value ever produced, `DistinctUntilChanged` remembers only the most recently emitted value, and filters out new values if and only if they match that most recent value.

This example uses `DistinctUntilChanged` to detect when a particular vessel reports a change in `NavigationStatus`.

```csharp
uint exampleMmsi = 235009890;
IObservable<IAisMessageType1to3> statusChanges = receiverHost.Messages
    .Where(v => v.Mmsi == exampleMmsi)
    .OfType<IAisMessageType1to3>()
    .DistinctUntilChanged(m => m.NavigationStatus)
    .Skip(1);
```

For example, if the vessel had repeatedly been reporting a status of `AtAnchor`, `DistinctUntilChanged` would drop each such message because the status was the same as it had previously been. But if the status were to change to `UnderwayUsingEngine`, that would cause `DistinctUntilChanged` to let the first message reporting that status through. It would then not allow any further messages through until there was another change in value, either back to `AtAnchor`, or to something else such as `Moored`. (The `Skip(1)` on the end is there because `DistinctUntilChanged` always lets through the very first message it sees. We have no way of knowing whether that actually represents a change in status, but it is very likely not to: ships report their status every few minutes, but they change that status much less often, so the first time we receive a report of a ship's status, it probably doesn't represent a change of status. By dropping that first item, we ensure that `statusChanges` provides notifications only when we can be certain that the status changed.)

That was our quick run through of the filtering methods available in Rx. While they are relatively simple, as we have already begun to see, the power in Rx is down to the composability of its operators.

The filter operators are your first stop for managing the potential deluge of data we can face in this information-rich age. We now know how to apply various criteria to remove data. Next, we will move on to operators that can transform data.