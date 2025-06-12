# Combining sequences

Data sources are everywhere, and sometimes we need to consume data from more than just a single source. Common examples that have many inputs include: price feeds, sensor networks, news feeds, social media aggregators, file watchers, multi touch surfaces, heart-beating/polling servers, etc. The way we deal with these multiple stimuli is varied too. We may want to consume it all as a deluge of integrated data, or one sequence at a time as sequential data. We could also get it in an orderly fashion, pairing data values from two sources to be processed together, or perhaps just consume the data from the first source that responds to the request.

Earlier chapters have also shown some examples of the _fan out and back in_ style of data processing, where we partition data, and perform processing on each partition to convert high-volume data into lower-volume higher-value events before recombining. This ability to restructure streams greatly enhances the benefits of operator composition. If Rx only enabled us to apply composition as a simple linear processing chain, it would be a good deal less powerful. Being able to pull streams apart gives us much more flexibility. So even when there is a single source of events, we often still need to combine multiple observable streams as part of our processing. Sequence composition enables you to create complex queries across multiple data sources. This unlocks the possibility to write some very powerful yet succinct code.

We've already used [`SelectMany`](06_Transformation.md#selectmany) in earlier chapters. This is one of the fundamental operators in Rx. As we saw in the [Transformation chapter](06_Transformation.md), it's possible to build several other operators from `SelectMany`, and its ability to combine streams is part of what makes it powerful. But there are several more specialized combination operators available, which make it easier to solve certain problems than it would be using `SelectMany`. Also, some operators we've seen before (including `TakeUntil` and `Buffer`) have overloads we've not yet explored that can combine multiple sequences.

## Sequential Combination

We'll start with the simplest kind of combining operators, which do not attempt concurrent combination. They deal with one source sequence at a time.

### Concat

`Concat` is arguably the simplest way to combine sequences. It does the same thing as its namesake in other LINQ providers: it concatenates two sequences. The resulting sequence produces all of the elements from the first sequence, followed by all of the elements from the second sequence. The simplest signature for `Concat` is as follows.

```csharp
public static IObservable<TSource> Concat<TSource>(
    this IObservable<TSource> first, 
    IObservable<TSource> second)
```

Since `Concat` is an extension method, we can invoke it as a method on any sequence, passing the second sequence in as the only argument:

```csharp
IObservable<int> s1 = Observable.Range(0, 3);
IObservable<int> s2 = Observable.Range(5, 5);
IObservable<int> c = s1.Concat(s2);
IDisposable sub = c.Subscribe(Console.WriteLine, x => Console.WriteLine("Error: " + x));
```

This marble diagram shows the items emerging from the two sources, `s1` and `s2`, and how `Concat` combines them into the result, `c`:

![A marble diagram showing three sequences. The first, s1, produces the values 0, 1, and 2, and then completes shortly after. The second, s2 starts after s1 finishes, and produces the values 5, 6, 7, 8, and 9, and then completes. The final sequence, c, produces all of the values from s1 then s2, that is: 0, 1, 2, 5, 6, 7, 8, and 9. The positioning shows that each item in c is produced at the same time as the corresponding value from s1 or s2.](GraphicsIntro/Ch09-CombiningSequences-Marbles-Concat-Marbles.svg)

Rx's `Concat` does nothing with its sources until something subscribes to the `IObservable<T>` it returns. So in this case, when we call `Subscribe` on `c` (the source returned by `Concat`) it will subscribe to its first input, `s1`, and each time that produces a value, the `c` observable will emit that same value to its subscriber. If we went on to call `sub.Dispose()` before `s1` completes, `Concat` would unsubscribe from the first source, and would never subscribe to `s2`. If `s1` were to report an error, `c` would report that same error to is subscriber, and again, it will never subscribe to `s2`. Only if `s1` completes will the `Concat` operator subscribe to `s2`, at which point it will forward any items that second input produces until either the second source completes or fails, or the application unsubscribes from the concatenated observable.

Although Rx's `Concat` has the same logical behaviour as the [LINQ to Objects `Concat`](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.concat), there are some Rx-specific details to be aware of. In particular, timing is often more significant in Rx than with other LINQ implementations. For example, in Rx we distinguish between [_hot_ and _cold_ source](02_KeyTypes.md#hot-and-cold-sources). With a cold source it typically doesn't matter exactly when you subscribe, but hot sources are essentially live, so you only get notified of things that happen while you are subscribed. This can mean that hot sources might not be a good fit with `Concat` The following marble diagram illustrates a scenario in which this produces results that have the potential to surprise:

![A marble diagram showing three sequences. The first, labelled 'cold', produces the values 0, 1, and 2, then completes. The second, labelled 'hot' produces values A, B, C, D, and E, but the positioning shows that these overlap partially with the 'cold' sequence. In particular, 'hot' produces A between items 0 and 1 from 'cold', and it produces B between 1 and 2, meaning that these A and B values occur before cold completes. The final sequence, labelled 'cold.Concat(hot)' shows all three values from 'cold' followed by only those values that 'hot' produced after 'cold' completes, i.e., just C, D, and E.](GraphicsIntro/Ch09-CombiningSequences-Marbles-Concat-Hot-Marbles.svg)

Since `Concat` doesn't subscribe to its second input until the first has finished, it won't see the first couple of items that the `hot` source would deliver to any subscribers that been listening from the start. This might not be the behaviour you would expect: it certainly doesn't look like this concatenated all of the items from the first sequence with all of the items from the second one. It looks like it missed out `A` and `B` from `hot`.

#### Marble Diagram Limitations

This last example reveals that marble diagrams gloss over a detail: they show when a source starts, when it produces values, and when it finishes, but they ignore the fact that to be able to produce items at all, an observable source needs a subscriber. If nothing subscribes to an `IObservable<T>`, then it doesn't really produce anything. `Concat` doesn't subscribe to its second input until the first completes, so arguably instead of the diagram above, it would be more accurate to show this:

![A marble diagram showing three sequences. The first, labelled 'cold', produces the values 0, 1, and 2, then completes. The second, labelled 'hot' produces values C, D, and E, and the positioning shows that these are produced after the 'cold' sequence completes. The final sequence, labelled 'cold.Concat(hot)' shows all three values from 'cold' followed by all three values from 'hot', i.e., 0, 1, 2, C, D, and E. This entirely diagram looks almost the same as the preceding one, except the 'hot' sequence doesn't produce the A or B values, and starts directly after 'cold' finishes.](GraphicsIntro/Ch09-CombiningSequences-Marbles-Concat-Hot-Marbles-SubOnly.svg)

This makes it easier to see why `Concat` produces the output it does. But since `hot` is a hot source here, this diagram fails to convey the fact that `hot` is producing items entirely on its own schedule. In a scenario where `hot` had multiple subscribers, then the earlier diagram would arguably be better because it correctly reflects every event available from `hot` (regardless of however many listeners might be subscribed at any particular moment). But although this convention works for hot sources, it doesn't work for cold ones, which typically start producing items upon subscription. A source returned by [`Timer`](03_CreatingObservableSequences.md#observabletimer) produces items on a regular schedule, but that schedule starts at the instant when subscription occurs. That means that if there are multiple subscriptions, there are multiple schedules. Even if I have just a single `IObservable<long>` returned by `Observable.Timer`, each distinct subscriber will get items on its own schedule—subscribers receive events at a regular interval _starting from whenever they happened to subscribe_. So for cold observables, it typically makes sense to use the convention used by this second diagram, in which we're looking at the events received by one particular subscription to a source.

Most of the time we can get away with ignoring this subtlety, quietly using whichever convention suits us. To paraphrase [Humpty Dumpty: when I use a marble diagram, it means just what I choose it to mean—neither more nor less](https://www.goodreads.com/quotes/12608-when-i-use-a-word-humpty-dumpty-said-in-rather). But when you're combining hot and cold sources together, there might not be one obviously best way to represent this in a marble diagram. We could even do something like this, where we describe the events that `hot` represents separately from the events seen by a particular subscription to `hot`.

![This essentially combines the preceding two diagrams. It has the same first and last sequences. In between these it has a sequence labelled 'Events available from hot' which shows the same as the 'hot' sequence in the diagram before last. It then has a sequence labelled 'Concat subscription to hot' which shows the same as the 'hot' sequence from the preceding diagram.](GraphicsIntro/Ch09-CombiningSequences-Marbles-Concat-Hot-Marbles-SourceAndSub.svg)


We're using a distinct 'lane' in the marble diagram to represent the events seen by a particular subscription to a source. With this technique, we can also show what would happen if you pass the same cold source into `Concat` twice:

![A marble diagram showing three sequences. The first, labelled 'Concat subscription to cold', produces the values 0, 1, and 2, then completes. The second, also labelled 'Concat subscription to cold' produces the values again, but positioned to shows that these are produced after the first sequence completes. The final sequence, labelled 'cold.Concat(cold)' shows the values twice, i.e., 0, 1, 2, 0, 1, and 2.](GraphicsIntro/Ch09-CombiningSequences-Marbles-Concat-Marbles-Cold-Twice.svg)

This highlights the fact that that being a cold source, `cold` provides items separately to each subscription. We see the same three values emerging from the same source, but at different times.

#### Concatenating Multiple Sources

What if you wanted to concatenate more than two sequences? `Concat` has an overload accepting multiple observable sequences as an array. This is annotated with the `params` keyword, so you don't need to construct the array explicitly. You can just pass any number of arguments, and the C# compiler will generate the code to create the array for you. There's also an overload taking an `IEnumerable<IObservable<T>>`, in case the observables you want to concatenate are already in some collection.

```csharp
public static IObservable<TSource> Concat<TSource>(
    params IObservable<TSource>[] sources)

public static IObservable<TSource> Concat<TSource>(
    this IEnumerable<IObservable<TSource>> sources)
```

The `IEnumerable<IObservable<T>>` overload evaluates `sources` lazily. It won't begin to ask it for source observables until someone subscribes to the observable that `Concat` returns, and it only calls `MoveNext` again on the resulting `IEnumerator<IObservable<T>>` when the current source completes meaning it's ready to start on the text. To illustrate this, the following example is an iterator method that returns a sequence of sequences and is sprinkled with logging. It returns three observable sequences each with a single value [1], [2] and [3]. Each sequence returns its value on a timer delay.

```csharp
public IEnumerable<IObservable<long>> GetSequences()
{
    Console.WriteLine("GetSequences() called");
    Console.WriteLine("Yield 1st sequence");

    yield return Observable.Create<long>(o =>
    {
        Console.WriteLine("1st subscribed to");
        return Observable.Timer(TimeSpan.FromMilliseconds(500))
            .Select(i => 1L)
            .Finally(() => Console.WriteLine("1st finished"))
            .Subscribe(o);
    });

    Console.WriteLine("Yield 2nd sequence");

    yield return Observable.Create<long>(o =>
    {
        Console.WriteLine("2nd subscribed to");
        return Observable.Timer(TimeSpan.FromMilliseconds(300))
            .Select(i => 2L)
            .Finally(() => Console.WriteLine("2nd finished"))
            .Subscribe(o);
    });

    Thread.Sleep(1000); // Force a delay

    Console.WriteLine("Yield 3rd sequence");

    yield return Observable.Create<long>(o =>
    {
        Console.WriteLine("3rd subscribed to");
        return Observable.Timer(TimeSpan.FromMilliseconds(100))
            .Select(i=>3L)
            .Finally(() => Console.WriteLine("3rd finished"))
            .Subscribe(o);
    });

    Console.WriteLine("GetSequences() complete");
}
```

We can call this `GetSequences` method and pass the results to `Concat`, and then use our `Dump` extension method to watch what happens:

```csharp
GetSequences().Concat().Dump("Concat");
```

Here's the output:

```
GetSequences() called
Yield 1st sequence
1st subscribed to
Concat-->1
1st finished
Yield 2nd sequence
2nd subscribed to
Concat-->2
2nd finished
Yield 3rd sequence
3rd subscribed to
Concat-->3
3rd finished
GetSequences() complete
Concat completed
```

Below is a marble diagram of the `Concat` operator applied to the `GetSequences` method. 's1', 's2' and 's3' represent sequence 1, 2 and 3. Respectively, 'rs' represents the result sequence.

![A marble diagram showing 4 sequences .The first, s1, waits for a while, then produces a single value, 1, then immediate completes. The second, s2, starts immediately after s1 completes. It waits for a slightly shorter interval, then produces the value 2, then immediately completes. The third, s3, starts some time after s2 completes, waits an even shorter time, produces the value 3, and then immediately completes. The final sequence, r, starts at the same time as s1, produces the values 1, 2, and 3 at exactly the same time as these values are produces by the earlier sources, and completes at the same time as s3.](GraphicsIntro/Ch09-CombiningSequences-Marbles-Concat-Marbles-Three.svg)

You should note that once the iterator has executed its first `yield return` to return the first sequence, the iterator does not continue until the first sequence has completed. The iterator calls `Console.WriteLine` to display the text `Yield 2nd sequence` immediately after that first `yield return`, but you can see that message doesn't appear in the output until after we see the `Concat-->1` message showing the first output from `Concat`, and also the `1st finished` message, produced by the `Finally` operator, which runs only after that first sequence has completed. (The code also makes that first source delay for 500ms before producing its value, so that if you run this, you can see that everything stops for a bit until that first source produces its single value then completes.) Once the first source completes, the `GetSequences` method continues (because `Concat` will ask it for the next item once the first observable source completes). When `GetSequences` provides the second sequence with another `yield return`, `Concat` subscribes to that, and again `GetSequences` makes no further progress until that second observable sequence completes. When asked for the third sequence, the iterator itself waits for a second before producing that third and final value, which you can see from the gap between the end of `s2` and the start of `s3` in the diagram.

### Prepend

There's one particular scenario that `Concat` supports, but in a slightly cumbersome way. It can sometimes be useful to make a sequence that always emits some initial value immediately. Take the example I've been using a lot in this book, where ships transmit AIS messages to report their location and other information: in some applications you might not want to wait until the ship happens next to transmit a message. You could imagine an application that records the last known location of any vessel. This would make it possible for the application to offer, say, an `IObservable<IVesselNavigation>` which instantly reports the last known information upon subscription, and which then goes on to supply any newer messages if the vessel produces any.

How would we implement this? We want initially cold-source-like behaviour, but transitioning into hot. So we could just concatenate two sources. We could use [`Observable.Return`](03_CreatingObservableSequences.md#observablereturn) to create a single-element cold source, and then concatenate that with the live stream:

```csharp
IVesselNavigation lastKnown = ais.GetLastReportedNavigationForVessel(mmsi);
IObservable<IVesselNavigation> live = ais.GetNavigationMessagesForVessel(mmsi);

IObservable<IVesselNavigation> lastKnownThenLive = Observable.Concat(
    Observable.Return(lastKnown), live);
```

This is a common enough requirement that Rx supplies `Prepend` that has a similar effect. We can replace the final line with:

```csharp
IObservable<IVesselNavigation> lastKnownThenLive = live.Prepend(lastKnown);
```

This observable will do exactly the same thing: subscribers will immediately receive the `lastKnown`, and then if the vessel should emit further navigation messages, they will receive those too. By the way, for this scenario you'd probably also want to ensure that the look up of the "last known" message happens as late as possible. We can delay this until the point of subscription by using [`Defer`](03_CreatingObservableSequences.md#observabledefer):

```csharp
public static IObservable<IVesselNavigation> GetLastKnownAndSubsequenceNavigationForVessel(uint mmsi)
{
    return Observable.Defer<IVesselNavigation>(() =>
    {
        // This lambda will run each time someone subscribes.
        IVesselNavigation lastKnown = ais.GetLastReportedNavigationForVessel(mmsi);
        IObservable<IVesselNavigation> live = ais.GetNavigationMessagesForVessel(mmsi);

        return live.Prepend(lastKnown);
    }
}
```

`StartWith` might remind you of [`BehaviorSubject<T>`](03_CreatingObservableSequences.md#behaviorsubjectt), because that also ensures that consumers receive a value as soon as they subscribe. It's not quite the same: `BehaviorSubject<T>` caches the last value its own source emits. You might think that would make it a better way to implement this vessel navigation example. However, since this example is able to return a source for any vessel (the `mmsi` argument is a [Maritime Mobile Service Identity](https://en.wikipedia.org/wiki/Maritime_Mobile_Service_Identity) uniquely identifying a vessel) it would need to keep a `BehaviorSubject<T>` running for every single vessel you were interested in, which might be impractical.

`BehaviorSubject<T>` can hold onto only one value, which is fine for this AIS scenario, and `Prepend` shares this limitation. But what if you need a source to begin with some particular sequence?

### StartWith

`StartWith` is a generalization of `Prepend` that enables us to provide any number of values to emit immediately upon subscription. As with `Prepend`, it will then go on to forward any further notifications that emerge from the source.

As you can see from its signature, this method takes a `params` array of values so you can pass in as many or as few values as you need:

```csharp
// prefixes a sequence of values to an observable sequence.
public static IObservable<TSource> StartWith<TSource>(
    this IObservable<TSource> source, 
    params TSource[] values)
```

There's also an overload that accepts an `IEnumerable<T>`. Note that Rx will _not_ defer its enumeration of this. `StartWith` immediately converts the `IEnumerable<T>` into an array before returning.

`StartsWith` is not a common LINQ operator, and its existence is peculiar to Rx. If you imagine what `StartsWith` would look like in LINQ to Objects, it would not be meaningfully different from [`Concat`](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.concat). There's a difference in Rx because `StartsWith` effectively bridges between _pull_ and _push_ worlds. It effectively converts the items we supply into an observable, and it then concatenates the `source` argument onto that.

### Append

The existence of `Prepend` might lead you to wonder whether there is an `Append` for adding a single item onto the end of any `IObservable<T>`. After all, this is a common LINQ operator; [LINQ to Objects has an `Append` implementation](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.append), for example. And Rx does indeed supply such a thing:

```csharp
IObservable<string> oneMore = arguments.Append("And another thing...");
```

There is no corresponding `EndWith`. There's no fundamental reason that there couldn't be such a thing it's just that apparently there's not much demand—the [Rx repository](https://github.com/dotnet/reactive) has not yet had a feature request. So although the symmetry of `Prepend` and `Append` does suggest that there could be a similar symmetry between `StartWith` and an as-yet-hypothetical `EndWith`, the absence of this counterpart doesn't seem to have caused any problems. There's an obvious value to being able to create observable sources that always immediately produce a useful output; it's not clear what `EndWith` would be useful for, besides satisfying a craving for symmetry.

### DefaultIfEmpty

The next operator we'll examine doesn't strictly performs sequential combination. However, it's a very close relative of `Append` and `Prepend`. Like those operators, this will emit everything their source does. And like those operators, `DefaultIfEmpty` takes one additional item. The difference is that it won't always emit that additional item.

Whereas `Prepend` emits its additional item at the start, and `Append` emits its additional item at the end, `DefaultIfEmpty` emits the additional item only if the source completes without producing anything. So this provides a way of guaranteeing that an observable will not be empty.

You don't have to supply `DefaultIfEmpty` with a value. If you use the overload in which you supply no such value, it will just use `default(T)`. This will be a zero-like value for _struct_ types and `null` for reference types.

### Repeat

The final operator that combines sequences sequentially is `Repeat`. It allows you to simply repeat a sequence. It offers overloads where you can specify the number of times to repeat the input, and one that repeats infinitely:

```csharp
// Repeats the observable sequence a specified number of times.
public static IObservable<TSource> Repeat<TSource>(
    this IObservable<TSource> source, 
    int repeatCount)

// Repeats the observable sequence indefinitely and sequentially.
public static IObservable<TSource> Repeat<TSource>(
    this IObservable<TSource> source)
```

`Repeat` resubscribes to the source for each repetition. This means that this will only strictly repeat if the source produces the same items each time you subscribe. Unlike the [`ReplaySubject<T>`](03_CreatingObservableSequences.md#replaysubjectt), this doesn't store and replay the items that emerge from the source. This means that you normally won't want to call `Repeat` on a hot source. (If you really want repetition of the output of a hot source, a combination of [`Replay`](15_PublishingOperators.md#replay) and `Repeat` might fit the bill.)

If you use the overload that repeats indefinitely, then the only way the sequence will stop is if there is an error or the subscription is disposed of. The overload that specifies a repeat count will stop on error, un-subscription, or when it reaches that count. This example shows the sequence [0,1,2] being repeated three times.

```csharp
var source = Observable.Range(0, 3);
var result = source.Repeat(3);

result.Subscribe(
    Console.WriteLine,
    () => Console.WriteLine("Completed"));
```

Output:

```
0
1
2
0
1
2
0
1
2
Completed
```

## Concurrent sequences

We'll now move on to operators for combining observable sequences that might produce values concurrently.

### Amb

`Amb` is a strangely named operator. It's short for _ambiguous_, but that doesn't tell us much more than `Amb`. If you're curious about the name you can read about the [origins of `Amb` in Appendix D](D_AlgebraicUnderpinnings.md#amb), but for now, let's look at what it actually does. 
Rx's `Amb` takes any number of `IObservable<T>` sources as inputs, and waits to see which, if any, first produces some sort of output. As soon as this happens, it immediately unsubscribes from all of the other sources, and forwards all notifications from the source that reacted first.

Why is that useful?

A common use case for `Amb` is when you want to produce some sort of result as quickly as possible, and you have multiple options for obtaining that result, but you don't know in advance which will be fastest. Perhaps there are multiple servers that could all potentially give you the answer you want, and it's impossible to predict which will have the lowest response time. You could send requests to all of them, and then just use the first to respond. If you model each individual request as its own `IObservable<T>`, `Amb` can handle this for you. Note that this isn't very efficient: you're asking several servers all to do the same work, and you're going to discard the results from most of them. (Since `Amb` unsubscribes from all the sources it's not going to use as soon as the first reacts, it's possible that you might be able to send a message to all the other servers to cancel the request. But this is still somewhat wasteful.) But there may be scenarios in which timeliness is crucial, and for those cases it might be worth tolerating a bit of wasted effort to produce faster results.

`Amb` is broadly similar to `Task.WhenAny`, in that it lets you detect when the first of multiple sources does something. However, the analogy is not precise. `Amb` automatically unsubscribes from all of the other sources, ensuring that everything is cleaned up. With `Task` you should always ensure that you eventually observe all tasks in case any of them faulted.

To illustrate `Amb`'s behaviour, here's a marble diagram showing three sequences, `s1`, `s2`, and `s3`, each able to produce a sequence values. The line labelled `r` shows the result of passing all three sequences into `Amb`. As you can see, `r` provides exactly the same notifications as `s1`, because in this example, `s1` was the first sequence to produce a value.

![A marble diagram showing 4 sequences. The first, s1, produces the values 1, 2, 3, and 4. The second, s2, starts at the same time as s1, but produces its first value, 99, after s1 produces 1, and produces its second value, 88, after s1 produces 2, and it then completes between s1 producing 2 and 3. The third source, s3, produces its first value, 8, after s2 produced 99, and before s1 produced 2, and it goes on to produce two more values, 7 and 6, interleaved with the activity from the earlier sources. The final sequence, r, is identical to s1.](GraphicsIntro/Ch09-CombiningSequences-Marbles-Amb-Marbles.svg)

This code creates exactly the situation described in that marble diagram, to verify that this is indeed how `Amb` behaves:

```csharp
var s1 = new Subject<int>();
var s2 = new Subject<int>();
var s3 = new Subject<int>();

var result = Observable.Amb(s1, s2, s3);

result.Subscribe(
    Console.WriteLine,
    () => Console.WriteLine("Completed"));

s1.OnNext(1);
s2.OnNext(99);
s3.OnNext(8);
s1.OnNext(2);
s2.OnNext(88);
s3.OnNext(7);
s2.OnCompleted();
s1.OnNext(3);
s3.OnNext(6);
s1.OnNext(4);
s1.OnCompleted();
s3.OnCompleted();
```

Output:

```
1
2
3
4
Completed
```

If we changed the order so that `s2.OnNext(99)` came before the call to `s1.OnNext(1);` then s2 would produce values first and the marble diagram would look like this.

![A marble diagram showing 4 sequences. The first, s1, produces the values 1, 2, 3, and 4. The second, s2, starts at the same time as s1, but produces its first value, 99, before s1 produces 1 (this being the key difference from the preceding diagram), and produces its second value, 88, after s1 produces 2, and it then completes between s1 producing 2 and 3. The third source, s3, produces its first value, 8, after s2 produced 99, and before s1 produced 2, and it goes on to produce two more values, 7 and 6, interleaved with the activity from the earlier sources. The final sequence, r, is identical to s2 (and not, as in the preceding diagram, s1).](GraphicsIntro/Ch09-CombiningSequences-Marbles-Amb-Marbles2.svg)

There are a few overloads of `Amb`. The preceding example used the overload that takes a `params` array of sequences. There's also an overload that takes exactly two sources, avoiding the array allocation that occurs with `params`. Finally, you could pass in an `IEnumerable<IObservable<T>>`. (Note that there are no overloads that take an `IObservable<IObservable<T>>`. `Amb` requires all of the source observables it monitors to be supplied up front.)

```csharp
// Propagates the observable sequence that reacts first.
public static IObservable<TSource> Amb<TSource>(
    this IObservable<TSource> first, 
    IObservable<TSource> second)
{...}
public static IObservable<TSource> Amb<TSource>(
    params IObservable<TSource>[] sources)
{...}
public static IObservable<TSource> Amb<TSource>(
    this IEnumerable<IObservable<TSource>> sources)
{...}
```

Reusing the `GetSequences` method from the `Concat` section, we see that `Amb` evaluates the outer (IEnumerable) sequence completely before subscribing to any of the sequences it returns.

```csharp
GetSequences().Amb().Dump("Amb");
```

Output:

```
GetSequences() called
Yield 1st sequence
Yield 2nd sequence
Yield 3rd sequence
GetSequences() complete
1st subscribed to
2nd subscribed to
3rd subscribed to
Amb-->3
Amb completed
```

Here is the marble diagram illustrating how this code behaves:

![A marble diagram showing four sequences. The first three all start at the same time, but significantly later than the fourth. The first, s1, waits for a while and then produces the value 1 and then completes. The second, s2, produces a value 2 before s1 produced its value, and immediately completes. The third, s3, produces its value, 3, before s2 produced 2, and then immediately completes. The final sequence, r, starts long before all the rest, and then produces 3 at the same time as s3 produced 3, and then immediately completes.](GraphicsIntro/Ch09-CombiningSequences-Marbles-Amb-Marbles3.svg)

Remember that `GetSequences` produces its first two observables as soon as it is asked for them, and then waits for 1 second before producing the third and final one. But unlike `Concat`, `Amb` won't subscribe to any of its sources until it has retrieved all of them from the iterator, which is why this marble diagram shows the subscriptions to all three sources starting after 1 second. (The first two sources were available earlier—`Amb` would have started enumerating the sources as soon as subscription occurred, but it waited until it had all three before subscribing, which is why they all appear over on the right.) The third sequence has the shortest delay between subscription and producing its value, so although it's the last observable returned, it is able to produce its value the fastest even though there are two sequences yielded one second before it (due to the `Thread.Sleep`).

### Merge

The `Merge` extension method takes multiple sequences as its input. Any time any of those input sequences produces a value, the observable returned by `Merge` produces that same value. If the input sequences produce values at the same time on different threads, `Merge` handles this safely, ensuring that it delivers items one at a time.

Since `Merge` returns a single observable sequence that includes all of the values from all of its input sequences, there's a sense in which it is similar to `Concat`. But whereas `Concat` waits until each input sequence completes before moving onto the next, `Merge` supports concurrently active sequences. As soon as you subscribe to the observable returned by `Merge`, it immediately subscribes to all of its inputs, forwarding everything any of them produces. This marble diagram shows two sequences, `s1` and `s2`, running concurrently and `r` shows the effect of combining these with `Merge`: the values from both source sequences emerge from the merged sequence.

![A marble diagram showing three sequences. The first, s1, produces the value 1 three times in a row, with a gap between each value. The second, s2, produces the value 2 three times in a row, and it does so at the same interval as the values from s2, but starting slightly later. The third sequence, c, contains all the same values as s1 and s2 combined, and at the same time as they emerge from their respective source sequences. So c produces 1, 2, 1, 2, 1, 2.](GraphicsIntro/Ch09-CombiningSequences-Marbles-Merge-Marbles.svg)

The result of a `Merge` will complete only once all input sequences complete. However, the `Merge` operator will error if any of the input sequences terminates erroneously (at which point it will unsubscribe from all its other inputs).

If you read the [Creating Observables chapter](03_CreatingObservableSequences.md), you've already seen one example of `Merge`. I used it to combine the individual sequences representing the various events provided by a `FileSystemWatcher` into a single stream at the end of the ['Representing Filesystem Events in Rx'](03_CreatingObservableSequences.md#representing-filesystem-events-in-rx) section. As another example, let's look at AIS once again. There is no publicly available single global source that can provide all AIS messages across the entire globe as an `IObservable<IAisMessage>`. Any single source is likely to cover just one area, or maybe even just a single AIS receiver. With `Merge`, it's straightforward to combine these into a single source:

```csharp
IObservable<IAisMessage> station1 = aisStations.GetMessagesFromStation("AdurStation");
IObservable<IAisMessage> station2 = aisStations.GetMessagesFromStation("EastbourneStation");

IObservable<IAisMessage> allMessages = station1.Merge(station2);
```

If you want to combine more than two sources, you have a few options:

- Chain `Merge` operators together e.g. `s1.Merge(s2).Merge(s3)`
- Pass a `params` array of sequences to the `Observable.Merge` static method. e.g. `Observable.Merge(s1,s2,s3)`
- Apply the `Merge` operator to an `IEnumerable<IObservable<T>>`.
- Apply the `Merge` operator to an `IObservable<IObservable<T>>`.

The overloads look like this:

```csharp
/// Merges two observable sequences into a single observable sequence.
/// Returns a sequence that merges the elements of the given sequences.
public static IObservable<TSource> Merge<TSource>(
    this IObservable<TSource> first, 
    IObservable<TSource> second)
{...}

// Merges all the observable sequences into a single observable sequence.
// The observable sequence that merges the elements of the observable sequences.
public static IObservable<TSource> Merge<TSource>(
    params IObservable<TSource>[] sources)
{...}

// Merges an enumerable sequence of observable sequences into a single observable sequence.
public static IObservable<TSource> Merge<TSource>(
    this IEnumerable<IObservable<TSource>> sources)
{...}

// Merges an observable sequence of observable sequences into an observable sequence.
// Merges all the elements of the inner sequences in to the output sequence.
public static IObservable<TSource> Merge<TSource>(
    this IObservable<IObservable<TSource>> sources)
{...}
```

As the number of sources being merged goes up, the operators that take collections have an advantage over the first overload. (I.e., `s1.Merge(s2).Merge(s3)` performs slightly less well than `Observable.Merge(new[] { s1, s2, s3 })`, or the equivalent `Observable.Merge(s1, s2, s3)`.) However, for just three or four, the differences are small, so in practice you can choose between the first two overloads as a matter of your preferred style. (If you're merging 100 sources or more the differences are more pronounced, but by that stage, the you probably wouldn't want to use the chained call style anyway.) The third and fourth overloads allow to you merge sequences that can be evaluated lazily at run time.

That last `Merge` overload that takes a sequence of sequences is particularly interesting, because it makes it possible for the set of sources being merged to grow over time. `Merge` will remain subscribed to `sources` for as long as your code remains subscribed to the `IObservable<T>` that `Merge` returns. So if `sources` emits more and more `IObservable<T>`s over time, these will all be included by `Merge`.

That might sound familiar. The [`SelectMany` operator](06_Transformation.md#selectmany), which is able to flatten multiple observable sources back out into a single observable source. This is just another illustration of why I've described `SelectMany` as a fundamental operator in Rx: strictly speaking we don't need a lot of the operators that Rx gives us because we could build them using `SelectMany`. Here's a simple re-implementation of that last `Merge` overload using `SelectMany`:

```csharp
public static IObservable<T> MyMerge<T>(this IObservable<IObservable<T>> sources) =>
    sources.SelectMany(source => source);
```

As well as illustrating that we don't technically need Rx to provide that last `Merge` for us, it's also a good illustration of why it's helpful that it does. It's not immediately obvious what this does. Why are we passing a lambda that just returns its argument? Unless you've seen this before, it can take some thought to work out that `SelectMany` expects us to pass a callback that it invokes for each incoming item, but that our input items are already nested sequences, so we can just return each item directly, and `SelectMany` will then take that and merge everything it produces into its output stream. And even if you have internalized `SelectMany` so completely that you know right away that this will just flatten `sources`, you'd still probably find `Observable.Merge(sources)` a more direct expression of intent. (Also, since `Merge` is a more specialized operator, Rx is able to provide a very slightly more efficient implementation of it than the `SelectMany` version shown above.)

If we again reuse the `GetSequences` method, we can see how the `Merge` operator works with a sequence of sequences.

```csharp
GetSequences().Merge().Dump("Merge");
```

Output:

```
GetSequences() called
Yield 1st sequence
1st subscribed to
Yield 2nd sequence
2nd subscribed to
Merge --> 2
Merge --> 1
Yield 3rd sequence
3rd subscribed to
GetSequences() complete
Merge --> 3
Merge completed
```

As we can see from the marble diagram, s1 and s2 are yielded and subscribed to immediately. s3 is not yielded for one second and then is subscribed to. Once all input sequences have completed, the result sequence completes.

![A marble diagram showing four sources. The first, s1, waits for a while and then produces the value 1 and immediately completes. The second, s2, starts at the same time as s1, but produces a single value, 2 and immediately completes before s1 produced its value. The third, s3, starts long after s1 and s3 have finished, waits a short while, produces the value 3, then immediately completes. The final source, r, starts when s1 and s2 start, completes when s3 completes, and produces the three values from each of the three other sources at the same as they do, so it shows 2, 1, then 3.](GraphicsIntro/Ch09-CombiningSequences-Marbles-Merge-Marbles-Multi.svg)


For each of the `Merge` overloads that accept variable numbers of sources (either via an array, an `IEnumerable<IObservable<T>>`, or an `IObservable<IObservable<T>>`) there's an additional overload adding a `maxconcurrent` parameter. For example:

```csharp
public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, int maxConcurrent)
```

This enables you to limit the number of sources that `Merge` accepts inputs from at any single time. If the number of sources available exceeds `maxConcurrent` (either because you passed in a collection with more sources, or because you used the `IObservable<IObservable<T>`-based overload and the source emitted more nested sources than `maxConcurrent`) `Merge` will wait for existing sources to complete before moving onto new ones. A `maxConcurrent` of 1 makes `Merge` behave in the same way as `Concat`.

### Switch

Rx's `Switch` operator takes an `IObservable<IObservable<T>>`, and produces notifications from the most recent nested observable. Each time its source produces a new nested `IObservable<T>`, `Switch` unsubscribes from the previous nested source (unless this is the first source, in which case there won't be a previous one) and subscribes to the latest one.

`Switch` can be used in a 'time to leave' type feature for a calendar application. In fact you can see the source code for a modified version of [how Bing provides (or at least provided; the implementation may have changed) notifications telling you that it's time to leave for an appointment](https://github.com/reaqtive/reaqtor/blob/c3ae17f93ae57f3fb75a53f76e60ae69299a509e/Reaqtor/Samples/Remoting/Reaqtor.Remoting.Samples/DomainFeeds.cs#L33-L76). Since that's derived from a real example, it's a little complex, so I'll describe just the essence here.

The basic idea with a 'time to leave' notification is that we using map and route finding services to work out the expected journey time to get to wherever the appointment is, and to use the [`Timer` operator](03_CreatingObservableSequences.md#observabletimer) to create an `IObservable<T>` that will produce a notification when it's time to leave. (Specifically this code produces an `IObservable<TrafficInfo>` which reports the proposed route for the journey, and expected travel time.) However, there are two things that can change, rendering the initial predicted journey time useless. First, traffic conditions can change. When the user created their appointment, we have to guess the expected journey time based on how traffic normally flows at the time of day in question. However, if there turns out to be really bad traffic on the day, the estimate will need to be revised upwards, and we'll need to notify the user earlier.

The other thing that can change is the user's location. This will also obviously affect the predicted journey time.

To handle this, the system will need observable sources that can report changes in the user's location, and changes in traffic conditions affecting the proposed journey. Every time either of these reports a change, we will need to produce a new estimated journey time, and a new `IObservable<TrafficInfo>` that will produce a notification when it's time to leave.

Every time we revise our estimate, we want to abandon the previously created `IObservable<TrafficInfo>`. (Otherwise, the user will receive a bewildering number of notifications telling them to leave, one for every time we recalculated the journey time.) We just want to use the latest one. And that's exactly what `Switch` does.

You can see the [example for that scenario in the Reaqtor repo](https://github.com/reaqtive/reaqtor/blob/c3ae17f93ae57f3fb75a53f76e60ae69299a509e/Reaqtor/Samples/Remoting/Reaqtor.Remoting.Samples/DomainFeeds.cs#L33-L76). Here, I'm going to present a different, simpler scenario: live searches. As you type, the text is sent to a search service and the results are returned to you as an observable sequence. Most implementations have a slight delay before sending the request so that unnecessary work does not happen. Imagine I want to search for "Intro to Rx". I quickly type in "Into to" and realize I have missed the letter 'r'. I stop briefly and change the text to "Intro ". By now, two searches have been sent to the server. The first search will return results that I do not want. Furthermore, if I were to receive data for the first search merged together with results for the second search, it would be a very odd experience for the user. I really only want results corresponding to the latest search text. This scenario fits perfectly with the `Switch` method.

In this example, there is an `IObservable<string>` source that represents the search text—each new value the user types emerges from this source sequence. We also have a search function that produces a single search result for a given search term:

```csharp
private IObservable<string> SearchResults(string query)
{
    ...
}
```

This returns just a single value, but we model it as an `IObservable<string>` partly to deal with the fact that it might take some time to perform the search, and also to be enable to use it with Rx. We can take our source of search terms, and then use `Select` to pass each new search value to this `SearchResults` function. This creates our resulting nested sequence, `IObservable<IObservable<string>>`.

Suppose we were to then use `Merge` to process the results:

```csharp
IObservable<string> searchValues = ....;
IObservable<IObservable<string>> search = searchValues.Select(searchText => SearchResults(searchText));
                    
var subscription = search
    .Merge()
    .Subscribe(Console.WriteLine);
```

If we were lucky and each search completed before the next element from `searchValues` was produced, the output would look sensible. However, it is much more likely, however that multiple searches will result in overlapped search results. This marble diagram shows what the `Merge` function could do in such a situation.

![A marble diagram showing 6 sources. The first, searchValues, produces the values I, In, Int, and Intr, and is shown as continuing on beyond the time represented by the diagram. The second, 'results (I)', starts when `searchValues` produces its first value, I, and then a while later produces a single value, Self, before immediately completing. It is significant that this single value is produced after the searchValues source has already produced its second value, In. The third source is labelled 'results (In)'. It starts at the same time that searchValues produces its second value, In, and a while later produces a single value, Into, before immediately completing. It is significant that it produces its value after searchValues has already produced its third value, Int. The fourth source is labelled 'results (Int)'. It starts at the same time that searchValues produces its third value, Int, and a while later produces a single value, 42, before immediately completing. It is significant that it produces its value after searchValues has already produced its fourth value, Intr. The fifth source is labelled 'results (Intr)'. It starts at the same time that searchValues produces its fourth value, Intr, and a while later produces a single value, Start, before immediately completing. It is significant that it produces its value before  the previous sequence produced its value. The final source is labelled 'Merged results'. It starts at the same time that searchValues starts, and it contains each of the items produced by the 2nd, 3rd, 4th, and 5th sequences. It does not complete.](GraphicsIntro/Ch09-CombiningSequences-Marbles-Switch-Marbles-Bad-Merge.svg)

Note how the values from the search results are all mixed together. The fact that some search terms took longer to get a search result than others has also meant that they have come out in the wrong order. This is not what we want. If we use the `Switch` extension method we will get much better results. `Switch` will subscribe to the outer sequence and as each inner sequence is yielded it will subscribe to the new inner sequence and dispose of the subscription to the previous inner sequence. This will result in the following marble diagram:

![A marble diagram showing 6 sources. The first, searchValues, produces the values I, In, Int, and Intr, and is shown as continuing on beyond the time represented by the diagram. The second, 'results (I)', starts when `searchValues` produces its first value, I, and then a while later produces a single value, Self, before immediately completing. It is significant that this single value is produced after the searchValues source has already produced its second value, In. The third source is labelled 'results (In)'. It starts at the same time that searchValues produces its second value, In, and a while later produces a single value, Into, before immediately completing. It is significant that it produces its value after searchValues has already produced its third value, Int. The fourth source is labelled 'results (Int)'. It starts at the same time that searchValues produces its third value, Int, and a while later produces a single value, 42, before immediately completing. It is significant that it produces its value after searchValues has already produced its fourth value, Intr. The fifth source is labelled 'results (Intr)'. It starts at the same time that searchValues produces its fourth value, Intr, and a while later produces a single value, Start, before immediately completing. It is significant that it produces its value before  the previous sequence produced its value. The final source is labelled 'Merged results'. It starts at the same time that searchValues starts, and it reports just a single value, Start, at exactly the same time the `results (Intr)` source produces the same value. It does not complete.](GraphicsIntro/Ch09-CombiningSequences-Marbles-Switch-Marbles.svg)

Now, each time a new search term arrives, causing a new search to be kicked off, a corresponding new `IObservable<string>` for that search's results appears, causing `Switch` to unsubscribe from the previous results. This means that any results that arrive too late (i.e., when the result is for a search term that is no longer the one in the search box) will be dropped. As it happens, in this particular example, this means that we only see the result for the final search term. All the intermediate values that we saw as the user was typing didn't hang around for long, because the user kept on pressing the next key before we'd received the previous value's results. Only at the end, when the user stopped typing for long enough that the search results came back before they became out of date, do we finally see a value from `Switch`. The net effect is that we've eliminated confusing results that are out of date.

This is another diagram where the ambiguity of marble diagrams causes a slight issue. I've shown each of the single-value observables produced by each of the calls to `SearchResults`, but in practice `Switch` unsubscribes from all but the last of these before they've had a chance to produce a value. So this diagram is showing the values those sources could potentially produce, and not the values that they actually delivered as part of the subscription, because the subscriptions were cut short.

## Pairing sequences

The previous methods allowed us to flatten multiple sequences sharing a common type into a result sequence of the same type (with various strategies for deciding what to include and what to discard). The operators in this section still take multiple sequences as an input, but attempt to pair values from each sequence to produce a single value for the output sequence. In some cases, they also allow you to provide sequences of different types.

### Zip

`Zip` combines pairs of items from two sequences. So its first output is created by combining the first item from one input with the first item from the other. The second output combines the second item from each input. And so on. The name is meant to evoke a zipper on clothing or a bag, which brings the teeth on each half of the zipper together one pair at a time.

Since `Zip` combines pairs of item in strict order, it will complete when the first of the sequences complete. If one of the sequence has reached its end, then even if the other continues to emit values, there will be nothing to pair any of these values with, so `Zip` just unsubscribes at this point, discards the unpairable values, and reports completion.

If either of the sequences produces an error, the sequence returned by `Zip` will report that same error.

If one of the source sequences publishes values faster than the other sequence, the rate of publishing will be dictated by the slower of the two sequences, because it can only emit an item when it has one from each source.

Here's an example:

```csharp
// Generate values 0,1,2 
var nums = Observable.Interval(TimeSpan.FromMilliseconds(250))
    .Take(3);

// Generate values a,b,c,d,e,f 
var chars = Observable.Interval(TimeSpan.FromMilliseconds(150))
    .Take(6)
    .Select(i => Char.ConvertFromUtf32((int)i + 97));

// Zip values together
nums.Zip(chars, (lhs, rhs) => (lhs, rhs)))
    .Dump("Zip");
 ```

The effect can be seen in this marble diagram below.:

![A marble diagram showing three sequences. The first, s1 waits for a while and then produces the values 0, 1, and 2, with some time between each value and completes immediately after producing 2. The second source, s2, waits for slightly less time, producing the value a before s1 produce 0, and then it produces b and c between s1's 0 and 1, and then between s1's 1 and 2, it produces d. It produces e at roughly the same time as s1 produces 2 (and for the purposes of this example, it doesn't really matter whether those happen at exactly the same time, or before or after one another) and then goes on to produce f, then immediately completes. The third sequence, c, shows the value '0,a' at the same time s1 produces 0, then '1,b' when s1 produces 1, and '2,c` when s1 produces 2, and then immediately completes (at the same time s1 completes).](GraphicsIntro/Ch09-CombiningSequences-Marbles-Zip-Marbles.svg)

Here's the actual output of the code:

```
{ Left = 0, Right = a }
{ Left = 1, Right = b }
{ Left = 2, Right = c }
```

Note that the `nums` sequence only produced three values before completing, while the `chars` sequence produced six values. The result sequence produced three values, this was as many pairs is it could make.

It is also worth noting that `Zip` has a second overload that takes an `IEnumerable<T>` as the second input sequence.

```csharp
// Merges an observable sequence and an enumerable sequence into one observable sequence 
// containing the result of pair-wise combining the elements by using the selector function.
public static IObservable<TResult> Zip<TFirst, TSecond, TResult>(
    this IObservable<TFirst> first, 
    IEnumerable<TSecond> second, 
    Func<TFirst, TSecond, TResult> resultSelector)
{...}
```

This allows us to zip sequences from both `IEnumerable<T>` and `IObservable<T>` paradigms!

### SequenceEqual

There's another operator that processes pairs of items from two sources: `SequenceEqual`. But instead of producing an output for each pair of inputs, this compares each pair, and ultimately produces a single value indicating whether every pair of inputs was equal or not.

In the case where the sources produce different values, `SequenceEqual` produces a single `false` value as soon as it detects this. But if the sources are equal, it can only report this when both have completed because until that happens, it doesn't yet know if there might a difference coming later. Here's an example illustrating its behaviour:

```csharp
var subject1 = new Subject<int>();

subject1.Subscribe(
    i => Console.WriteLine($"subject1.OnNext({i})"),
    () => Console.WriteLine("subject1 completed"));

var subject2 = new Subject<int>();

subject2.Subscribe(
    i => Console.WriteLine($"subject2.OnNext({i})"),
    () => Console.WriteLine("subject2 completed"));

var areEqual = subject1.SequenceEqual(subject2);

areEqual.Subscribe(
    i => Console.WriteLine($"areEqual.OnNext({i})"),
    () => Console.WriteLine("areEqual completed"));

subject1.OnNext(1);
subject1.OnNext(2);

subject2.OnNext(1);
subject2.OnNext(2);
subject2.OnNext(3);

subject1.OnNext(3);

subject1.OnCompleted();
subject2.OnCompleted();
```

Output:

```
subject1.OnNext(1)
subject1.OnNext(2)
subject2.OnNext(1)
subject2.OnNext(2)
subject2.OnNext(3)
subject1.OnNext(3)
subject1 completed
subject2 completed
areEqual.OnNext(True)
areEqual completed
```

### CombineLatest

The `CombineLatest` operator is similar to `Zip` in that it combines pairs of items from its sources. However, instead of pairing the first items, then the second, and so on, `CombineLatest` produces an output any time _either_ of its inputs produces a new value. For each new value to emerge from an input, `CombineLatest` uses that along with the most recently seen value from the other input. (To be precise, it doesn't produce anything until each input has produced at least one value, so if one input takes longer to get started than the other, there will be a period in which `CombineLatest` doesn't in fact produce an output each time one of its inputs does, because it's waiting for the other to produce its first value.) The signature is as follows.

```csharp
// Composes two observable sequences into one observable sequence by using the selector 
// function whenever one of the observable sequences produces an element.
public static IObservable<TResult> CombineLatest<TFirst, TSecond, TResult>(
    this IObservable<TFirst> first, 
    IObservable<TSecond> second, 
    Func<TFirst, TSecond, TResult> resultSelector)
{...}
```

The marble diagram below shows off usage of `CombineLatest` with one sequence that produces numbers (`s1`), and the other letters (`s2`). If the `resultSelector` function just joins the number and letter together as a pair, this would produce the result shown on the bottom line. I've colour coded each output to indicate which of the two sources caused it to emit that particular result, but as you can see, each output includes a value from each source.

![A marble diagram showing three sequences. The first, s1, waits for a while then produces the values 1, 2, and 3, spaced out over time. The second, s2, starts at the same time as s1, and waits for less time, producing its first value, a, before s1 produces 1. Then after s1 has produced 2, s2 produces b and then c, both being produced before s1 produces 3. The third sequence, CombineLatest, shows '1,a' at the same time as s1 produces 1, then '2,a' when s1 produces 2, then '2,b' when s2 produces b, then '2,c' when s2 produces c, then '3,c' when s1 produces 3. All three sequences do not end within the time shown in the diagram.](GraphicsIntro/Ch09-CombiningSequences-Marbles-CombineLatest-Marbles.svg)

If we slowly walk through the above marble diagram, we first see that `s2` produces the letter 'a'. `s1` has not produced any value yet so there is nothing to pair, meaning that no value is produced for the result. Next, `s1` produces the number '1' so the result sequence can now produce a pair '1,a'. We then receive the number '2' from `s1`. The last letter is still 'a' so the next pair is '2,a'. The letter 'b' is then produced creating the pair '2,b', followed by 'c' giving '2,c'. Finally the number 3 is produced and we get the pair '3,c'.

This is great in case you need to evaluate some combination of state which needs to be kept up-to-date when any single component of that state changes. A simple example would be a monitoring system. Each service is represented by a sequence that returns a Boolean indicating the availability of said service. The monitoring status is green if all services are available; we can achieve this by having the result selector perform a logical AND. 
Here is an example.

```csharp
IObservable<bool> webServerStatus = GetWebStatus();
IObservable<bool> databaseStatus = GetDBStatus();

// Yields true when both systems are up.
var systemStatus = webServerStatus
    .CombineLatest(
        databaseStatus,
        (webStatus, dbStatus) => webStatus && dbStatus);
```

You may have noticed that this method could produce a lot of duplicate values. For example, if the web server goes down the result sequence will yield '`false`'. If the database then goes down, another (unnecessary) '`false`' value will be yielded. This would be an appropriate time to use the `DistinctUntilChanged` extension method. The corrected code would look like the example below.

```csharp
// Yields true when both systems are up, and only on change of status
var systemStatus = webServerStatus
    .CombineLatest(
        databaseStatus,
        (webStatus, dbStatus) => webStatus && dbStatus)
    .DistinctUntilChanged();
```

## Join

The `Join` operator allows you to logically join two sequences. Whereas the `Zip` operator would pair values from the two sequences based on their position within the sequence, the `Join` operator allows you join sequences based on when elements are emitted.

Since the production of a value by an observable source is logically an instantaneous event, joins use a model of intersecting windows. Recall that with the [`Window`](08_Partitioning.md#window) operator, you can define the duration of each window using an observable sequence. The `Join` operator uses a similar concept: for each source, we can define a time window over which each element is considered to be 'current' and two elements from different sources will be joined if their time windows overlap. As the `Zip` operator, we also need to provide a selector function to produce the result item from each pair of values. Here's the `Join` operator:

```csharp
public static IObservable<TResult> Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>
(
    this IObservable<TLeft> left,
    IObservable<TRight> right,
    Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
    Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
    Func<TLeft, TRight, TResult> resultSelector
)
```

This is a complex signature to try and understand in one go, so let's take it one parameter at a time.

`IObservable<TLeft> left` is the first source sequence. `IObservable<TRight> right` is the second source sequence. `Join` is looking to produce pairs of items, with each pair containing one element from `left` and one element from `right`.

The `leftDurationSelector` argument enables us to define the time window for each item from `left`. A source item's time window begins when the source emits the item. To determine when the window for an item from `left` should close, `Join` will invoke the `leftDurationSelector`, passing in the value just produced by `left`. This selector must return an observable source. (It doesn't matter at all what the element type of this source is, because `Join` is only interested in _when_ it does things.) The item's time window ends as soon as the source returned for that item by `leftDurationSelector` either produces a value or completes.

The `rightDurationSelector` argument defines the time window for each item from `right`. It works in exactly the same way as the `leftDurationSelector`.

Initially, there are no current items. But as `left` and `right` produce items, these items' windows will start, so `Join` might have multiple items all with their windows currently open. Each time `left` produces a new item, `Join` looks to see if any items from `right` still have their windows open. If they do, `left` is now paired with each of them. (So a single item from one source might be joined with multiple items from the other source.) `Join` calls the `resultSelector` for each such pairing. Likewise, each time `right` produces an item, then if there are any currently open windows for items from `left`, that new item from `right` will be paired with each of these, and again, `resultSelector` will be called for each such pairing.

The observable returned by `Join` produces the result of each call to `resultSelector`.

Let us now imagine a scenario where the left sequence produces values twice as fast as the right sequence. Imagine that in addition we never close the left windows; we could do this by always returning `Observable.Never<Unit>()` from the `leftDurationSelector` function. And imagine that we make the right windows close as soon as they possibly can, which we can achieve by making `rightDurationSelector` return `Observable.Empty<Unit>()`. The following marble diagram illustrates this:

![A marble diagram showing five groups of sequences. The first group, labelled left, contains a single sequence which immediately produces the value 0, then, at evenly space intervals, the values 1, 2, 3, 4, and 5. The second group, labelled 'left durations' shows a sequence for each of the values produced by left, each starting at exactly the moment left produces one of its value. None of these sequences produces any values or completes. The third group is labelled right. It waits until after left has produced its second value (1), and then produces A. Between left's 3 and 4, it produces B. After left's 5 it produces C. The next group is labelled 'right durations', and it shows three sequences, each starting at the time right produces one of its values, and each immediately ending—these are effectively instantaneously short sequences. The final group, Join, shows a single sequence. When right produces A, this immediately produces '0,A' and then '1,A'. When right produces B, it produces '0,B', '1,B', '2,B', and `3,B`. Then right produces C, the Join sequence produces '0,C', '1,C', '2,C', '3,C', '4,C', '5,C'. The diagram happens to show each set of value stacked vertically, but that's only because they are produced in such quick succession that we wouldn't otherwise be able to see them.](GraphicsIntro/Ch09-CombiningSequences-Marbles-Join-Marbles1.svg)

Each time a left duration window intersects with a right duration window, we get an output. The right duration windows are all effectively of zero length, but this doesn't stop them from intersecting with the left duration windows, because those all never end. So the first item from right has a (zero-length) window that falls inside two of the windows for the `left` items, and so `Join` produces two results. I've stacked these vertically on the diagram to show that they happen at virtually the same time. Of course, the rules of `IObserver<T>` mean that they can't actually happen at the same time: `Join` has to wait until the consumer's `OnNext` has finished processing `0,A` before it can go on to produce `1,A`. But it will produce all the pairs as quickly as possible any time a single event from one source overlaps with multiple windows for the other.

If I also immediately closed the left window by returning `Observable.Empty<Unit>`, or perhaps `Observable.Return(0)`, the windows would never overlap, so no pairs would ever get produced. (In theory if both left and right produce items at _exactly_ the same time, then perhaps we might get a pair, but since the timing of events is never absolutely precise, it would be a bad idea to design a system that depended on this.)

What if I wanted to ensure that items from `right` only ever intersected with a single value from `left`? In that case, I'd need to ensure that the left durations did not overlap. One way to do that would be to have my `leftDurationSelector` always return the same sequence that I passed as the `left` sequence. This will result in `Join` making multiple subscriptions to the same source, and for some kinds of sources that might introduce unwanted side effects, but the [`Publish`](15_PublishingOperators.md#publish) and [`RefCount`](15_PublishingOperators.md#refcount) operators provide a way to deal with that, so this is in fact a reasonably strategy. If we do that, the results look more like this.

![A marble diagram showing five groups of sequences. The first group, labelled left, contains a single sequence which immediately produces the value 0, then, at evenly space intervals, the values 1, 2, 3, 4, and 5. The second group, labelled 'left durations' shows a sequence for each of the values produced by left, each starting at exactly the moment left produces one of its value, and finishing at the same moment that the next one starts.The third group is labelled right. It waits until after left has produced its second value (1), and then produces A. Between left's 3 and 4, it produces B. After left's 5 it produces C. The next group is labelled 'right durations', and it shows three sequences, each starting at the time right produces one of its values, and each immediately ending—these are effectively instantaneously short sequences. The final group, Join, shows a single sequence. When right produces A, this immediately produces '1,A'. When right produces B, it produces `3,B`. Then right produces C, the Join sequence produces '5,C'.](GraphicsIntro/Ch09-CombiningSequences-Marbles-Join-Marbles2.svg)

The last example is very similar to [`CombineLatest`](12_CombiningSequences.md#CombineLatest), except that it is only producing a pair when the right sequence changes. We can easily make it work the same way by changing the right durations to work in the same way as the left durations. This code shows how (including the use of `Publish` and `RefCount` to ensure that we only get a single subscription to the underlying `left` and `right` sources despite providing then to `Join` many times over).

```csharp
public static IObservable<TResult> MyCombineLatest<TLeft, TRight, TResult>
(
    IObservable<TLeft> left,
    IObservable<TRight> right,
    Func<TLeft, TRight, TResult> resultSelector
)
{
    var refcountedLeft = left.Publish().RefCount();
    var refcountedRight = right.Publish().RefCount();

    return Observable.Join(
        refcountedLeft,
        refcountedRight,
        value => refcountedLeft,
        value => refcountedRight,
        resultSelector);
}
```

Obviously there's no need to write this—you can just use the built-in `CombineLatest`. (And that will be slightly more efficient because it has a specialized implementation.) But it shows that `Join` is a powerful operator.

## GroupJoin

When the `Join` operator pairs up values whose windows overlap, it will pass the scalar values left and right to the `resultSelector`. The `GroupJoin` operator is based on the same concept of overlapping windows, but its selector works slightly differently: `GroupJoin` still passes a single (scalar) value from the left source, but it passes an `IObservable<TRight>` as the second argument. This argument represents all of the values from the right sequence that occur within the window for the particular left value for which it was invoked.

So this lacks the symmetry of `Join`, because the left and right sources are handled differently. `GroupJoin` will call the `resultSelector` exactly once for each item produced by the `left` source. When a left value's window overlaps with the windows of multiple right values, `Group` would deal with that by calling the selector once for each such pairing, but `GroupJoin` deals with this by having the observable passed as the second argument to `resultSelector` emit each of the right items that overlap with that left item. (If a left item overlaps with nothing from the right, `resultSelector` will still be called with that item, it'll just be passed an `IObservable<TRight>` that doesn't produce any items.)

The `GroupJoin` signature is very similar to `Join`, but note the difference in the `resultSelector` parameter.

```csharp
public static IObservable<TResult> GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>
(
    this IObservable<TLeft> left,
    IObservable<TRight> right,
    Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
    Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
    Func<TLeft, IObservable<TRight>, TResult> resultSelector
)
```

If we went back to our first `Join` example where we had

* the `left` producing values twice as fast as the right,
* the left never expiring
* the right immediately expiring

This diagram shows those same inputs again, and also shows the observables `GroupJoin` would pass to the `resultSelector` for each of the items produced by `left`:

![A marble diagram showing five groups of sequences. The first group, labelled left, contains a single sequence which immediately produces the value 0, then, at evenly space intervals, the values 1, 2, 3, 4, and 5. The second group, labelled 'left durations' shows a sequence for each of the values produced by left, each starting at exactly the moment left produces one of its value. None of these sequences produces any values or completes. The third group is labelled right. It waits until after left has produced its second value (1), and then produces A. Between left's 3 and 4, it produces B. After left's 5 it produces C. The next group is labelled 'right durations', and it shows three sequences, each starting at the time right produces one of its values, and each immediately ending—these are effectively instantaneously short sequences. The final group contains 6 sequences, each of which has a label of the form `Right observable passed to selector for 0`, with the digit at the end changing for each sequence (so "...for 1" then "...for 2" and so on). Each of these sequences in the final group starts at the same time as a corresponding value from the left sequence. The first two show A, B, and C at the same time that the right sequence produces these values. The next two start after right has produced A, so they show only B and C. The last two start after right produces B so they show only C.](GraphicsIntro/Ch09-CombiningSequences-Marbles-GroupJoin-Marbles.svg)

This produces events corresponding to all of the same events that `Join` produced, they're just distributed across six different `IObservable<TRight>` sources. It may have occurred to you that with `GroupJoin` you could effectively re-create your own `Join` method by doing something like this:

```csharp
public IObservable<TResult> MyJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
    IObservable<TLeft> left,
    IObservable<TRight> right,
    Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
    Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
    Func<TLeft, TRight, TResult> resultSelector)
{
    return Observable.GroupJoin
    (
        left,
        right,
        leftDurationSelector,
        rightDurationSelector,
        (leftValue, rightValues) => 
          rightValues.Select(rightValue=>resultSelector(leftValue, rightValue))
    )
    .Merge();
}
```

You could even create a crude version of `Window` with this code:

```csharp
public IObservable<IObservable<T>> MyWindow<T>(IObservable<T> source, TimeSpan windowPeriod)
{
    return Observable.Create<IObservable<T>>(o =>;
    {
        var sharedSource = source
            .Publish()
            .RefCount();

        var intervals = Observable.Return(0L)
            .Concat(Observable.Interval(windowPeriod))
            .TakeUntil(sharedSource.TakeLast(1))
            .Publish()
            .RefCount();

        return intervals.GroupJoin(
                sharedSource, 
                _ => intervals, 
                _ => Observable.Empty<Unit>(), 
                (left, sourceValues) => sourceValues)
            .Subscribe(o);
    });
}
```

Rx delivers yet another way to query data in motion by allowing you to interrogate sequences of coincidence. This enables you to solve the intrinsically complex problem of managing state and concurrency while performing matching from multiple sources. By encapsulating these low level operations, you are able to leverage Rx to design your software in an expressive and testable fashion. Using the Rx operators as building blocks, your code effectively becomes a composition of many simple operators. This allows the complexity of the domain code to be the focus, not the otherwise incidental supporting code.

### And-Then-When

`Zip` can take only two sequences as an input. If that is a problem, then you can use a combination of the three `And`/`Then`/`When` methods. These methods are used slightly differently from most of the other Rx methods. Out of these three, `And` is the only extension method to `IObservable<T>`. Unlike most Rx operators, it does not return a sequence; instead, it returns the mysterious type `Pattern<T1, T2>`. The `Pattern<T1, T2>` type is public (obviously), but all of its properties are internal. The only two (useful) things you can do with a `Pattern<T1, T2>` are invoking its `And` or `Then` methods. The `And` method called on the `Pattern<T1, T2>` returns a `Pattern<T1, T2, T3>`. On that type, you will also find the `And` and `Then` methods. The generic `Pattern` types are there to allow you to chain multiple `And` methods together, each one extending the generic type parameter list by one. You then bring them all together with the `Then` method overloads. The `Then` methods return you a `Plan` type. Finally, you pass this `Plan` to the `Observable.When` method in order to create your sequence.

It may sound very complex, but comparing some code samples should make it easier to understand. It will also allow you to see which style you prefer to use.

To `Zip` three sequences together, you can either use `Zip` methods chained together like this:

```csharp
IObservable<long> one = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);
IObservable<long> two = Observable.Interval(TimeSpan.FromMilliseconds(250)).Take(10);
IObservable<long> three = Observable.Interval(TimeSpan.FromMilliseconds(150)).Take(14);

// lhs represents 'Left Hand Side'
// rhs represents 'Right Hand Side'
IObservable<(long One, long Two, long Three)> zippedSequence = one
    .Zip(two, (lhs, rhs) => (One: lhs, Two: rhs))
    .Zip(three, (lhs, rhs) => (lhs.One, lhs.Two, Three: rhs));

zippedSequence.Subscribe(
    v => Console.WriteLine($"One: {v.One}, Two: {v.Two}, Three: {v.Three}"),
    () => Console.WriteLine("Completed"));
```

Or perhaps use the nicer syntax of the `And`/`Then`/`When`:

```csharp
Pattern<long, long, long> pattern =
    one.And(two).And(three);
Plan<(long One, long Two, long Three)> plan =
    pattern.Then((first, second, third) => (One: first, Two: second, Three: third));
IObservable<(long One, long Two, long Three)> zippedSequence = Observable.When(plan);

zippedSequence.Subscribe(
    v => Console.WriteLine($"One: {v.One}, Two: {v.Two}, Three: {v.Three}"),
    () => Console.WriteLine("Completed"));
```

This can be further reduced, if you prefer, to:

```csharp
IObservable<(long One, long Two, long Three)> zippedSequence = Observable.When(
    one.And(two).And(three)
        .Then((first, second, third) =>
            (One: first, Two: second, Three: third))
    );  

zippedSequence.Subscribe(
    v => Console.WriteLine($"One: {v.One}, Two: {v.Two}, Three: {v.Three}"),
    () => Console.WriteLine("Completed"));
```

The `And`/`Then`/`When` trio has more overloads that enable you to group an even greater number of sequences. They also allow you to provide more than one 'plan' (the output of the `Then` method). This gives you the `Merge` feature but on the collection of 'plans'. I would suggest playing around with them if this functionality is of interest to you. The verbosity of enumerating all of the combinations of these methods would be of low value. You will get far more value out of using them and discovering for yourself.

## Summary

This chapter covered a set of methods that allow us to combine observable sequences. This brings us to a close on Part 2. We've looked at the operators that are mostly concerned with defining the computations we want to perform on the data. In Part 3 we will move onto practical concerns such as managing scheduling, side effects, and error handling.
