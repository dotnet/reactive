# Leaving Rx's World

An observable sequence is a useful construct, especially when we have the power of LINQ to compose complex queries over it. Even though we recognize the benefits of the observable sequence, sometimes we must leave the `IObservable<T>` paradigm. This is necessary when we need to integrate with an existing non-Rx-based API (e.g. one that uses events or `Task<T>`). You might leave the observable paradigm if you find it easier for testing, or it may simply be easier for you to learn Rx by moving between an observable paradigm and a more familiar one.

Rx's compositional nature is the key to its power, but it can look like a problem when you need to integrate with a component that doesn't understand Rx. Most of the Rx library features we've looked at so far express their inputs and outputs as observables. How are you supposed to take some real world source of events and turn that into an observable? How are you supposed to do something meaningful with the output of an observable?

You've already seen some answer to these questions. The [Creating Observable Sequences chapter](03_CreatingObservableSequences.md) showed various ways to create observable sources. But when it comes to handling the items that emerge from an `IObservable<T>`, all we've really seen is how to implement [`IObserver<T>`](02_KeyTypes.md#iobserver), and [how to use the callback based `Subscribe` extension methods  to subscribe to an `IObservable<T>`](02_KeyTypes.md#iobservable).

In this chapter, we will look at the methods in Rx which allow you to leave the `IObservable<T>` world, so you can take action based on the notifications that emerge from an Rx source.

## Integration with `async` and `await`

You can use C#'s `await` keyword with any `IObservable<T>`. We saw this earlier with [`FirstAsync`](05_Filtering.md#blocking-versions-of-firstlastsingleordefault):

```csharp
long v = await Observable.Timer(TimeSpan.FromSeconds(2)).FirstAsync();
Console.WriteLine(v);
```

Although `await` is most often used with `Task`, `Task<T>`, or `ValueTask<T>`, it is actually an extensible language feature. It's possible to make `await` work for more or less any type by supplying a method called `GetAwaiter`, typically as an extension method, and a suitable type for `GetAwaiter` to return, providing C# with the features `await` requires. That's precisely what Rx does. If your source file includes a `using System.Reactive.Linq;` directive, a suitable extension method will be available, so you can `await` any task.

The way this actually works is that the relevant `GetAwaiter` extension method wraps the `IObservable<T>` in an `AsyncSubject<T>`, which provides everything that C# requires to support `await`. These wrappers work in such a way that there will be a call to `Subscribe` each time you execute an `await` against an `IObservable<T>`.

If a source reports an error by invoking its observer's `OnError`, Rx's `await` integration handles this by putting the task into a faulted state, so that the `await` will rethrow the exception.

Sequences can be empty. They might call `OnCompleted` without ever having called `OnNext`. However, since there's no way to tell from the type of a source that it will be empty, this doesn't fit especially well with the `await` paradigm. With tasks, you can know at compile time whether you'll get a result by looking at whether you're awaiting a `Task` or `Task<T>`, so the compiler is able to know whether a particular `await` expression produces a value. But when you `await` and `IObservable<T>`, there's no compile-time distinction, so the only way Rx can report that a sequence is empty when you `await` is to throw an `InvalidOperationException` reporting that the sequence contains no elements.

As you may recall from the [`AsyncSubject<T>` section of chapter 3](03_CreatingObservableSequences.md#asyncsubjectt), `AsyncSubject<T>` reports only the final value to emerge from its source. So if you `await` a sequence that reports multiple items, all but the final item will be ignored. What if you want to see all of the items, but you'd still like to use `await` to handle completion and errors?

## ForEachAsync

The `ForEachAsync` method supports `await`, but it provides a way to process each element. You could think of it as a hybrid of the `await` behaviour described in the preceding section, and the callback-based `Subscribe`. We can still use `await` to detect completion and errors, but we supply a callback enabling us to handle every item:

```csharp
IObservable<long> source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);
await source.ForEachAsync(i => Console.WriteLine($"received {i} @ {DateTime.Now}"));
Console.WriteLine($"finished @ {DateTime.Now}");
```

Output:

```
received 0 @ 02/08/2023 07:53:46
received 1 @ 02/08/2023 07:53:47
received 2 @ 02/08/2023 07:53:48
received 3 @ 02/08/2023 07:53:49
received 4 @ 02/08/2023 07:53:50
finished @ 02/08/2023 07:53:50
```

Note that the `finished` line is last, as you would expect. Let's compare this with the `Subscribe` extension method, which also lets us provide a single callback for handling items:

```csharp
IObservable<long> source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);
source.Subscribe(i => Console.WriteLine($"received {i} @ {DateTime.Now}"));
Console.WriteLine($"finished @ {DateTime.Now}");
```

As the output shows, `Subscribe` returned immediately. Our per-item callback was invoked just like before, but this all happened later on:

```
finished @ 02/08/2023 07:55:42
received 0 @ 02/08/2023 07:55:43
received 1 @ 02/08/2023 07:55:44
received 2 @ 02/08/2023 07:55:45
received 3 @ 02/08/2023 07:55:46
received 4 @ 02/08/2023 07:55:47
```

This can be useful in batch-style programs that perform some work and then exit. The problem with using `Subscribe` in that scenario is that our program could easily exit without having finished the work it started. This is easy to avoid with `ForEachAsync` because we just use `await` to ensure that our method doesn't complete until the work is done.

When we use `await` either directly against an `IObservable<T>`, or through `ForEachAsync`, we are essentially choosing to handle sequence completion in a conventional way, not a reactive way. Error and completion handling are no longer callback driven—Rx supplies the `OnCompleted` and `OnError` handlers for us, and instead represents these through C#'s awaiter mechanism. (Specifically, when we `await` a source directly, Rx supplies a custom awaiter, and when we use `ForEachAsync` it just returns a `Task`.)

Note that there are some circumstances in which `Subscribe` will block until its source completes. [`Observable.Return`](03_CreatingObservableSequences.md#observablereturn) will do this by default, as will [`Observable.Range`](03_CreatingObservableSequences.md#observablerange). We could try to make the last example do it by specifying a different scheduler:

```csharp
// Don't do this!
IObservable<long> source = 
   Observable.Interval(TimeSpan.FromSeconds(1), ImmediateScheduler.Instance)
             .Take(5);
source.Subscribe(i => Console.WriteLine($"received {i} @ {DateTime.Now}"));
Console.WriteLine($"finished @ {DateTime.Now}");
```

However, this highlights the dangers of non-async blocking calls: although this looks like it should work, in practice it deadlocks in the current version of Rx. Rx doesn't consider the `ImmediateScheduler` to be suitable for timer-based operations, which is why it's not the default, and this scenario is a good example of why that is. (The fundamental issue is that the only way to cancel a scheduled work item is to call `Dispose` on the object returned by the call to `Schedule`. `ImmediateScheduler` by definition doesn't return until after it has finished the work, meaning it effectively can't support cancellation. So the call to `Interval` effectively creates a periodically scheduled work item that can't be cancelled, and which is therefore doomed to run forever.)

This is why we need `ForEachAsync`. It might look like we can get the same effect through clever use of schedulers, but in practice if you need to wait for something asynchronous to happen, it's always better to use `await` than to use an approach that entails blocking the calling thread.

## ToEnumerable

The two mechanisms we've explored so far convert completion and error handling from Rx's callback mechanism to a more conventional approach enabled by `await`, but we still had to supply a callback to be able to handle every individual item. But the `ToEnumerable` extension method goes a step further: it enables the entire sequence to be consumed with a conventional `foreach` loop:

```csharp
var period = TimeSpan.FromMilliseconds(200);
IObservable<long> source = Observable.Timer(TimeSpan.Zero, period).Take(5);
IEnumerable<long> result = source.ToEnumerable();

foreach (long value in result)
{
    Console.WriteLine(value);
}

Console.WriteLine("done");
```

Output:

```
0
1
2
3
4
done
```

The source observable sequence will be subscribed to when you start to enumerate the sequence (i.e. lazily). 
If no elements are available yet, or you have consumed all elements produced so far, the call that `foreach` makes to the enumerator's `MoveNext` will block until the source produces an element. So this approach relies on the source being able to generate elements from some other thread. (In this example, `Timer` defaults to the [`DefaultScheduler`](11_SchedulingAndThreading.md#defaultscheduler), which runs timed work on the thread pool.) If the sequence produces values faster than you consume them, they will be queued for you. (This means that it is technically possible to consume and generate items on the same thread when using `ToEnumerable` but this would rely on the producer always remaining ahead. This would be a dangerous approach because if the `foreach` loop ever caught up, it would then deadlock.)

As with `await` and `ForEachAsync`, if the source reports an error, this will be thrown, so you can use ordinary C# exception handling as this example shows:

```csharp
try 
{ 
    foreach (long value in result)
    { 
        Console.WriteLine(value); 
    } 
} 
catch (Exception e) 
{ 
    Console.WriteLine(e.Message);
} 
```

## To a single collection

Sometimes you will want all of the items a source produces as a single list. For example, perhaps you can't just process the elements individually because you will sometimes need to refer back to elements received earlier. The four operations described in following sections gather all of the items into a single collection. They all still produce an `IObservable<T>` (e.g., an `IObservable<int[]>` or an `IObservable<Dictionary<string, long>>`), but these are all single-element observables, and as you've already seen, you can use the `await` keyword to get hold of this single output.

### ToArray and ToList

`ToArray` and `ToList` take an observable sequence and package it into an array or an instance of `List<T>` respectively. As with all of the single collection operations, these return an observable source that waits for their input sequence to complete, and then produces the array or list as the single value, after which they immediately complete. This example uses `ToArray` to collect all 5 elements from a source sequence into an array, and uses `await` to extract that array from the sequence that `ToArray` returns:

```csharp
TimeSpan period = TimeSpan.FromMilliseconds(200);
IObservable<long> source = Observable.Timer(TimeSpan.Zero, period).Take(5);
IObservable<long[]> resultSource = source.ToArray();

long[] result = await resultSource;
foreach (long value in result)
{
    Console.WriteLine(value);
}
```

Output:

```
0
1
2
3
4
```

As these methods still return observable sequences, you can also use the normal Rx `Subscribe` mechanisms, or use these as inputs to other operators.

If the source produces values and then errors, you will not receive any of those values. All values received up to that point will be discarded, and the operator will invoke its observer's `OnError` (and in the example above, that will result in the exception being thrown from the `await`). All four operators (`ToArray`, `ToList`, `ToDictionary` and `ToLookup`) handle errors like this.

### ToDictionary and ToLookup

Rx can package an observable sequence into a dictionary or lookup with the `ToDictionary` and `ToLookup` methods. Both methods take the same basic approach as the `ToArray` and `ToList` methods: they return a single-element sequence that produces the collection once the input source completes.

`ToDictionary` provides four overloads that correspond directly to the `ToDictionary` extension methods for `IEnumerable<T>` defined by LINQ to Objects:

```csharp
// Creates a dictionary from an observable sequence according to a specified 
// key selector function, a comparer, and an element selector function.
public static IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(
    this IObservable<TSource> source, 
    Func<TSource, TKey> keySelector, 
    Func<TSource, TElement> elementSelector, 
    IEqualityComparer<TKey> comparer) 
{...} 

// Creates a dictionary from an observable sequence according to a specified
// key selector function, and an element selector function. 
public static IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>( 
    this IObservable<TSource> source, 
    Func<TSource, TKey> keySelector, 
    Func<TSource, TElement> elementSelector) 
{...} 

// Creates a dictionary from an observable sequence according to a specified
// key selector function, and a comparer. 
public static IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>( 
    this IObservable<TSource> source, 
    Func<TSource, TKey> keySelector,
    IEqualityComparer<TKey> comparer) 
{...} 

// Creates a dictionary from an observable sequence according to a specified 
// key selector function. 
public static IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>( 
    this IObservable<TSource> source, 
    Func<TSource, TKey> keySelector) 
{...} 
```

The `ToLookup` extension offers near-identical-looking overloads, the difference being the return type (and the name, obviously). They all return an `IObservable<ILookup<TKey, TElement>>`. As with LINQ to Objects, the distinction between a dictionary and a lookup is that the `ILookup<TKey, TElement>>` interface allows each key to have any number of values, whereas a dictionary maps each key to one value.

## ToTask

Although Rx provides direct support for using `await` with an `IObservable<T>`, it can sometimes be useful to obtain a `Task<T>` representing an `IObservable<T>`. This is useful because some APIs expect a `Task<T>`. You can call `ToTask()` on any `IObservable<T>`, and this will subscribe to that observable, returning a `Task<T>` that will complete when the task completes, producing the sequence's final output as the task's result. If the source completes without producing an element, the task will enter a faulted state, with an `InvalidOperation` exception complaining that the input sequence contains no elements.

You can optionally pass a cancellation token. If you cancel this before the observable sequence completes, Rx will unsubscribe from the source, and put the task into a cancelled state.

This is a simple example of how the `ToTask` operator can be used. 
Note, the `ToTask` method is in the `System.Reactive.Threading.Tasks` namespace.

```csharp
IObservable<long> source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);
Task<long> resultTask = source.ToTask();
long result = await resultTask; // Will take 5 seconds. 
Console.WriteLine(result);
```

Output:

```
4
```

If the source sequence calls `OnError`, Rx puts the task in a faulted state, using the exception supplied.

Once you have your task, you can of course use all the features of the TPL such as continuations.

## ToEvent

Just as you can use an event as the source for an observable sequence with [`FromEventPattern`](03_CreatingObservableSequences.md#from-events), you can also make your observable sequence look like a standard .NET event with the `ToEvent` extension methods.

```csharp
// Exposes an observable sequence as an object with a .NET event. 
public static IEventSource<unit> ToEvent(this IObservable<Unit> source)
{...}

// Exposes an observable sequence as an object with a .NET event. 
public static IEventSource<TSource> ToEvent<TSource>(this IObservable<TSource> source) 
{...}
```

The `ToEvent` method returns an `IEventSource<T>`, which has a single member: an `OnNext` event.

```csharp
public interface IEventSource<T> 
{ 
    event Action<T> OnNext; 
} 
```

When we convert the observable sequence with the `ToEvent` method, we can just subscribe by providing an `Action<T>`, which we do here with a lambda.

```csharp
var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5); 
var result = source.ToEvent(); 
result.OnNext += val => Console.WriteLine(val);
```

Output:

```
0
1
2
3
4
```

Although this is the simplest way to convert Rx notifications into events, it does not follow the standard .NET event pattern. We use a slightly different approach if we want that.

### ToEventPattern

Normally, .NET events supply `sender` and `EventArgs` arguments to their handlers. In the example above, we just get the value. If you want to expose your sequence as an event that follows the standard pattern, you will need to use `ToEventPattern`.

```csharp
// Exposes an observable sequence as an object with a .NET event. 
public static IEventPatternSource<TEventArgs> ToEventPattern<TEventArgs>(
    this IObservable<EventPattern<TEventArgs>> source) 
    where TEventArgs : EventArgs 
```

The `ToEventPattern` will take an `IObservable<EventPattern<TEventArgs>>` and convert that into an `IEventPatternSource<TEventArgs>`. The public interface for these types is quite simple.

```csharp
public class EventPattern<TEventArgs> : IEquatable<EventPattern<TEventArgs>>
    where TEventArgs : EventArgs 
{ 
    public EventPattern(object sender, TEventArgs e)
    { 
        this.Sender = sender; 
        this.EventArgs = e; 
    } 
    public object Sender { get; private set; } 
    public TEventArgs EventArgs { get; private set; } 
    //...equality overloads
} 

public interface IEventPatternSource<TEventArgs> where TEventArgs : EventArgs
{ 
    event EventHandler<TEventArgs> OnNext; 
} 
```

To use this, we will need a suitable `EventArgs` type. You might be able to use one supplied by the .NET runtime libraries, but if not you can easily write your own:

The `EventArgs` type:

```csharp
public class MyEventArgs : EventArgs 
{ 
    private readonly long _value; 
    
    public MyEventArgs(long value) 
    { 
        _value = value; 
    } 

    public long Value 
    { 
        get { return _value; } 
    } 
} 
```

We can then use this from Rx by applying a simple transform using `Select`:

```csharp
IObservable<EventPattern<MyEventArgs>> source = 
   Observable.Interval(TimeSpan.FromSeconds(1))
             .Select(i => new EventPattern<MyEventArgs>(this, new MyEventArgs(i)));
```

Now that we have a sequence that is compatible, we can use the `ToEventPattern`, and in turn, a standard event handler.

```csharp
IEventPatternSource<MyEventArgs> result = source.ToEventPattern(); 
result.OnNext += (sender, eventArgs) => Console.WriteLine(eventArgs.Value);
```

Now that we know how to get back into .NET events, let's take a break and remember why Rx is a better model.

- Events are difficult to compose
- Events cannot be passed as argument or stored in fields
- Events do not offer the ability to be easily queried over time
- Events do not have a standard pattern for reporting errors
- Events do not have a standard pattern for indicating the end of a sequence of values
- Events provide almost no help for managing concurrency or multithreaded applications

## Do

Non-functional requirements of production systems often demand high availability, quality monitoring features and low lead time for defect resolution. Logging, debugging, instrumentation and journaling are common implementation choices for implementing non-functional requirements. To enable these it can often be useful to 'tap into' your Rx queries, making them deliver monitoring and diagnostic information as a side effect of their normal operation.

The `Do` extension method allows you to inject side effect behaviour. From an Rx perspective, `Do` doesn't appear to do anything: you can apply it to any `IObservable<T>`, and it returns another `IObservable<T>` that reports exactly the same elements and error or completion as its source. However, its various overloads takes callback arguments that look just like those for `Subscribe`: you can provide callbacks for individual items, completion, and errors. Unlike `Subscribe`, `Do` is not the final destination—everything the `Do` callbacks see will also be forwarded onto `Do`'s subscribers.  This makes it useful for logging and similar instrumentation because you can use it to report how information is flowing through an Rx query without changing that query's behaviour.

You have to be a bit careful of course. Using `Do` will have a performance impact. And if the callback you supply to `Do` performs any operations that could change the inputs to the Rx query it is part of, you will have created a feedback loop making the behaviour altogether harder to understand.

Let's first define some logging methods that we can go on to use in an example:

```csharp
private static void Log(object onNextValue)
{
    Console.WriteLine($"Logging OnNext({onNextValue}) @ {DateTime.Now}");
}

private static void Log(Exception error)
{
    Console.WriteLine($"Logging OnError({error}) @ {DateTime.Now}");
}

private static void Log()
{
    Console.WriteLine($"Logging OnCompleted()@ {DateTime.Now}");
}
```

This code uses `Do` to introduce some logging using the methods from above.

```csharp
IObservable<long> source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(3);
IObservable<long> loggedSource = source.Do(
    i => Log(i),
    ex => Log(ex),
    () => Log());

loggedSource.Subscribe(
    Console.WriteLine,
    () => Console.WriteLine("completed"));
```

Output:

```
Logging OnNext(0) @ 01/01/2012 12:00:00
0
Logging OnNext(1) @ 01/01/2012 12:00:01
1
Logging OnNext(2) @ 01/01/2012 12:00:02
2
Logging OnCompleted() @ 01/01/2012 12:00:02
completed
```

Note that because the `Do` is part of the query, it necessarily sees the values earlier than the `Subscribe`, which is the final link in the chain. That's why the log messages appear before the lines produced by the `Subscribe` callbacks. I like to think of the `Do` method as a [wire tap](http://en.wikipedia.org/wiki/Telephone_tapping) to a sequence. It gives you the ability to listen in on the sequence without modifying it.

As with `Subscribe`, instead of passing callbacks, there are overloads that let you supply callbacks for whichever of the OnNext, OnError, and OnCompleted notifications you want, or you can pass `Do` an `IObserver<T>`.

## Encapsulating with AsObservable

Poor encapsulation is a way developers can leave the door open for bugs. Here is a handful of scenarios where carelessness leads to leaky abstractions. Our first example may seem harmless at a glance, but has numerous problems.

```csharp
public class UltraLeakyLetterRepo
{
    public ReplaySubject<string> Letters { get; }

    public UltraLeakyLetterRepo()
    {
        Letters = new ReplaySubject<string>();
        Letters.OnNext("A");
        Letters.OnNext("B");
        Letters.OnNext("C");
    }
}
```

In this example we expose our observable sequence as a property. We've used a `ReplaySubject<string>` so that each subscriber will receive all of the values upon subscription. However, revealing this implementation choice in the public type of the `Letters` property is poor encapsulation, as consumers could call `OnNext`/`OnError`/`OnCompleted`. To close off that loophole we can simply make the publicly visible property type an `IObservable<string>`.

```csharp
public class ObscuredLeakinessLetterRepo
{
    public IObservable<string> Letters { get; }

    public ObscuredLeakinessLetterRepo()
    {
        var letters = new ReplaySubject<string>();
        letters.OnNext("A");
        letters.OnNext("B");
        letters.OnNext("C");
        this.Letters = letters;
    }
}
```

This is a significant improvement: the compiler won't let someone using an instance of this source write `source.Letters.OnNext("1")`. So the API surface area properly encapsulates the implementation detail, but if we were paranoid, we could not that nothing prevents consumers from casting the result back to an `ISubject<string>` and then calling whatever methods they like. In this example we see external code pushing their values into the sequence.

```csharp
var repo = new ObscuredLeakinessLetterRepo();
IObservable<string> good = repo.GetLetters();
    
good.Subscribe(Console.WriteLine);

// Be naughty
if (good is ISubject<string> evil)
{
    // So naughty, 1 is not a letter!
    evil.OnNext("1");
}
else
{
    Console.WriteLine("could not sabotage");
}
```

Output:

```
A
B
C
1
```

Arguably, code that does this sort of thing is asking for trouble, but if we wanted actively to prevent it, the fix to this problem is quite simple. By applying the `AsObservable` extension method, we can modify the line of the constructor that sets `this.Letters` to wrap the subject in a type that only implements `IObservable<T>`.

```csharp
this.Letters = letters.AsObservable();
```

Output:

```
A
B
C
could not sabotage
```

While I have used words like 'evil' and 'sabotage' in these examples, it is [more often than not an oversight rather than malicious intent](https://en.wikipedia.org/wiki/Hanlon%27s_razor) that causes problems. The failing falls first on the programmer who designed the leaky class. Designing interfaces is hard, but we should do our best to help consumers of our code fall into [the pit of success](https://learn.microsoft.com/en-gb/archive/blogs/brada/the-pit-of-success) by giving them discoverable and consistent types. Types become more discoverable if we reduce their surface area to expose only the features we intend our consumers to use. In this example we reduced the type's surface area. We did so by choosing a suitable public-facing type for the property, and then preventing access to the underlying type with the `AsObservable` method.

The set of methods we have looked at in this chapter complete the circle started in the [Creating Sequences chapter](03_CreatingObservableSequences.md). We now have the means to enter and leave Rx's world. Take care when opting in and out of the `IObservable<T>`. It's best not to transition back and forth—having a bit of Rx-based processing, then some more conventional code, and then plumbing the results of that back into Rx can quickly make a mess of your code base, and may indicate a design flaw. Typically it is better to keep all of your Rx logic together, so you only need to integrating with the outside world twice: once for input and once for output.