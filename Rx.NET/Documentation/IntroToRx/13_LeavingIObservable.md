# Leaving Rx's World

An observable sequence is a useful construct, especially when we have the power of LINQ to compose complex queries over it. Even though we recognize the benefits of the observable sequence, sometimes we must leave the `IObservable<T>` paradigm. This is necessary when we need to integrate with an existing non-Rx-based API (e.g. one that uses events or `Task<T>`). You might leave the observable paradigm if you find it easier for testing, or it may simply be easier for you to learn Rx by moving between an observable paradigm and a more familiar one.

Rx's compositional nature is the key to its power, but it can look like a problem when you need to integrate with a component that doesn't understand Rx. Most of the Rx library features we've looked at so far express their inputs and outputs as observables. How are you supposed to take some real world source of events and turn that into an observable? How are you supposed to do something meaningful with the output of an observable?

You've already seen some answer to these questions. The [Creating Observable Sequences chapter](03_CreatingObservableSequences.md) showed various ways to create observable sources. But when it comes to handling the items that emerge from an `IObservable<T>`, all we've really seen is how to implement [`IObserver<T>`](02_KeyTypes.md#iobserver), and [how to use the callback based `Subscribe` extension methods  to subscribe to an `IObservable<T>`](02_KeyTypes.md#iobservable).

In this chapter, we will look at the methods in Rx which allow you to leave the `IObservable<T>` world, so you can take action based on the notifications that emerge from an Rx source. But first, we need to look at the challenge this creates for error handling.

## Exception state

There's a rule you must conform to when exceptions leave Rx's world: if a developer using Rx chooses to use a mechanism that takes exceptions delivered by an `IObservable<T>` and throws them (e.g. if you `await` an `IObservable<T>`) then it is the developer's responsibility to ensure that either:

* each exception object is used only once

or

* the exception's dispatch state is reset prior to being supplied to the observer that will be rethrowing it (e.g., by executing a `throw`)

For example, if you use the [`Throw`](03_CreatingObservableSequences.md#observablethrow) operator, this creates a problem if you plan to `await` the resulting sequence: by design, `Throw` does nothing at all to the state of the exception you supply. This is deliberate, because you might be giving it an exception that already has important state there and it does not want to destroy that information. However, in cases where the exception has never actually been thrown, the state never gets set (and therefore never reset). There is no good way to detect whether exception state is present, so `Throw` must conservatively assume that it might be. This means that if you were to `await` such a sequence twice, you'd be breaking these rules. Rx 6.1 introduced a new operator, [`ResetExceptionDispatchState`](#resetexceptiondispatchstate), to deal with this scenario. If you write something like this:

```cs
IObservable<int> t = Observable
    .Throw<int>(new Exception("Boom!"))
    .ResetExceptionDispatchState();
```

the presence of that `ResetExceptionDispatchState` ensures that when the `Throw` reports the error by calling `OnError`, the exception state is reset at that instant.

But why do these rules exist?

Code that lives entirely within Rx's world reports errors by having an `IObservable<T>` invoke an observer's `OnError` method, passing an `Exception` representing the error. Exceptions are reported, discovered, and handled without ever using `throw` or `catch`. This makes Rx's use of exceptions somewhat unusual, and there's a consequence: if you construct but do not throw an exception, its _exception state_ is never set.

Exception state is the information describing the context in which the exception was thrown. It includes a textual description of the call stack, which can help developers diagnose the problem. It also includes more machine-friendly information, describing the crash site with technical identifiers that can help automated systems analyze errors. [Windows Error Reporting](https://learn.microsoft.com/en-us/windows/win32/wer/windows-error-reporting) can use this to distinguish between different kinds of application failures. For desktop applications, this information can (with user consent) be collected centrally, enabling developers to discover which crashes users encounter most often in practical use. Failures are identified by the particular combination of technical identifiers (which effectively identify the particular location in the code that failed, and also the type of failure that occurred). Each unique combination is known informally as a _fault bucket_. You can see this information in the Windows Event Viewer. If a .NET application throws an unhandled exception, you typically get three events in the event log:

* a **.NET Runtime** entry (typically with Event ID 1026) reporting the application executable name, .NET version, and .NET stack trace text
* an **Application Crashing Events** entry (typically with Event ID 1000) with generic (non-.NET-specific) details in a form that would appear for any application crash, such as the executable path, Win32 SEH exception type, the DLL in which the exception originated and the offset within that DLL
* a **Windows Error Reporting** entry (typically with Event ID 1001) containing the _fault bucket_ details

Here's an example of that third kind from a crashing .NET application:

```
Fault bucket 1162564721043373785, type 4
Event Name: APPCRASH
Response: Not available
Cab Id: 0

Problem signature:
P1: TestResetExceptionDispatchState.exe
P2: 1.0.0.0
P3: 68a40000
P4: KERNELBASE.dll
P5: 10.0.26100.6584
P6: 0a9b38fe
P7: e0434352
P8: 00000000000c66ca
P9: 
P10: 
```

This is harder to understand than a stack trace, but these numeric identifiers make it easier to determine automatically whether two crashes are in some sense 'the same'. The idea here is to make it possible to discover when the same kind of error is happening a lot, enabling developers to focus their efforts on fixing the types of crashes causing the most trouble.

With a .NET application, we want this information to reflect the origin of the failure. But it's quite common for exceptions to be caught and rethrown. What we _don't_ want is for every single error to be categorised as the same type just because we wrote some common error handling code. .NET itself has to contend with this: when we use `await` to wait for an asynchronous operation that has failed, that failure exception will have been put into a `Task` or `ValueTask`, and .NET rethrows that into the code that calls `await`. It would be deeply unhelpful if every single failing asynchronous operation was reported to Windows Error Handling as having occurred inside the .NET runtime library code that rethrows exceptions captured by a `Task`!

The purpose of _exception state_ is to hold onto the exception's origin information even if the exception gets rethrown. If you rethrow an exception from within a `catch` block by using `throw;` this exceptions state mostly gets preserved. ('Mostly', because the stack information gets augmented: the stack trace will report both the original throw location and the rethrow location.) This does _not_ work correctly if you try to rethrow the exception with `throw ex;` inside the `catch` block, which is why it's important to use the no-arguments `throw;` form when rethrowing. As for exceptions that end up being caught in one place and rethrown in a completely different place (e.g. on a different thread because `await` is being used) the .NET runtime libraries provide a helper called [`ExceptionDispatchInfo`](https://learn.microsoft.com/dotnet/api/system.runtime.exceptionservices.exceptiondispatchinfo) that can help manage this exception state, and ensure that it is used when rethrowing an exception in a completely different context from which it was thrown.

Rx uses this to ensure that examples such as the following behave as expected:

```cs
IObservable<string> fileLines = Observable.Create<string>(async obs =>
{
    using var reader = new StreamReader(@"c:\temp\test.txt");

    while ((await reader.ReadLineAsync()) is string line)
    {
        obs.OnNext(line);
    }
});

string firstNonEmptyLine = await fileLines
    .FirstAsync(line => line.Length > 0);
Console.WriteLine(firstNonEmptyLine);
```

If the attempt to open the file throws an exception, what do we expect to see? A developer familiar with how exceptions generally work with `async`/`await` in .NET might reasonably expect the exception to report two stack traces: one for the point at which the exception was originally thrown, and another for where it was rethrown from the `await`. And that's exactly what we see:

```
Unhandled exception. System.IO.FileNotFoundException: Could not find file 'c:\temp\test.txt'.
File name: 'c:\temp\test.txt'
   at Microsoft.Win32.SafeHandles.SafeFileHandle.CreateFile(String fullPath, FileMode mode, FileAccess access, FileShare share, FileOptions options)
   at Microsoft.Win32.SafeHandles.SafeFileHandle.Open(String fullPath, FileMode mode, FileAccess access, FileShare share, FileOptions options, Int64 preallocationSize, Nullable`1 unixCreateMode)
   at System.IO.Strategies.OSFileStreamStrategy..ctor(String path, FileMode mode, FileAccess access, FileShare share, FileOptions options, Int64 preallocationSize, Nullable`1 unixCreateMode)
   at System.IO.Strategies.FileStreamHelpers.ChooseStrategyCore(String path, FileMode mode, FileAccess access, FileShare share, FileOptions options, Int64 preallocationSize, Nullable`1 unixCreateMode)
   at System.IO.StreamReader.ValidateArgsAndOpenPath(String path, Encoding encoding, Int32 bufferSize)
   at System.IO.StreamReader..ctor(String path)
   at Program.<>c.<<<Main>$>b__0_0>d.MoveNext() in D:\source\RxThrowExamples\RxThrowExamples\Program.cs:line 5
--- End of stack trace from previous location ---
   at System.Reactive.PlatformServices.ExceptionServicesImpl.Rethrow(Exception exception)
   at System.Reactive.ExceptionHelpers.Throw(Exception exception)
   at System.Reactive.Subjects.AsyncSubject`1.GetResult()
   at Program.<Main>$(String[] args) in D:\source\RxThrowExamples\RxThrowExamples\Program.cs:line 13
   at Program.<Main>(String[] args)
```

This conforms to the rules described above because the exception is thrown in the conventional .NET manner here. Rx then catches it—this overload of `Observable.Create` wraps the `Task` returned by the callback in an adapter that detects when the `Task` enters a faulted state, in which case it extracts the exception and passes it to the subscribing `IObserver<T>`. And then the awaiter that Rx provides when you `await` an observable rethrows this same exception. Rx uses the `ExceptionDispatchInfo` mechanisms to ensure that the exception state captured at the point where the exception was originally thrown remains present when the exception is eventually rethrown to the code that used `await` on the observable.

This is fine when the exception originates by being thrown in the conventional way, which it does in that last example. Where it can go wrong is if the exception never gets thrown at all: the rethrowing mechanism provided by `ExceptionDispatchInfo` gets a little confused in this case. This problem is not specific to Rx by the way, as the following code illustrates:

```cs
Exception ox = new("Kaboom!");

for (int i = 0; i < 3; ++i)
{
    Console.WriteLine();
    Console.WriteLine();

    try
    {
        await Task.FromException(ox); // Exception thrown here

    }
    catch (Exception x)
    {
        Console.WriteLine(x);
    }
}
```

This creates a single `Exception` object and then wraps it in a `Task` (using `Task.FromException`) which it immediately `await`s. There is no Rx code here. Running this we get the following slightly surprising behaviour:

```
System.Exception: Kaboom!
   at Program.<Main>$(String[] args) in D:\source\RxThrowExamples\ThrowStackDupWithoutRx\Program.cs:line 10


System.Exception: Kaboom!
   at Program.<Main>$(String[] args) in D:\source\RxThrowExamples\ThrowStackDupWithoutRx\Program.cs:line 10
   at Program.<Main>$(String[] args) in D:\source\RxThrowExamples\ThrowStackDupWithoutRx\Program.cs:line 10


System.Exception: Kaboom!
   at Program.<Main>$(String[] args) in D:\source\RxThrowExamples\ThrowStackDupWithoutRx\Program.cs:line 10
   at Program.<Main>$(String[] args) in D:\source\RxThrowExamples\ThrowStackDupWithoutRx\Program.cs:line 10
   at Program.<Main>$(String[] args) in D:\source\RxThrowExamples\ThrowStackDupWithoutRx\Program.cs:line 10
```

Notice that each time around the loop, the stack trace gets longer. The problem here is that nothing ever resets the exception dispatch state. Normally that happens when the original `throw` occurs, but in this example we never use `throw`, and so that reset never happens. When you `await` a `Task`, the .NET runtime uses `ExceptionDispatchInfo` to rethrow the exception, and the point of that mechanism is that it preserves the original exceptions state, and appends the current stack so that you get a complete record of the exception's history. It wasn't designed to cope with an exception being rethrown multiple times without corresponding multiple `throw`s.

Since Rx uses the same mechanism when exceptions emerge out of Rx's world, the Rx equivalent of that `Task` example has exactly the same problem:

```cs
IObservable<int> ts = Observable.Throw<int>(new Exception("Pow!"));

for (int i = 0; i < 3; ++i)
{
    Console.WriteLine();
    Console.WriteLine();

	try
	{
		await ts; // Exception thrown here

	}
	catch (Exception x)
	{
        Console.WriteLine(x);
	}
}
```

This will also produce a longer stack trace each time round the loop, because again, we're rethrowing the same exception object multiple times without ever resetting its state.

Remember, Rx deliberately preserves the state because for all it knows, that state was important. (This is how the example with the `FileNotFoundException` above reports the correct information). This is why Rx 6.1 introduced the [`ResetExceptionDispatchState`](#resetexceptiondispatchstate) operator. It enables us to tell Rx that we do in fact want it to reset the exception state each time an error emerges from an observable. If we change the first statement of the example above to this:

```cs
IObservable<int> ts = Observable
   .Throw<int>(new Exception("Pow!"))
   .ResetExceptionDispatchState();
```

this now conforms to the rules described at the start of this section, which prevents the ever-growing stack trace problem.


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

## ResetExceptionDispatchState

The `ResetExceptionDispatchState` instructs Rx to reset the _exception dispatch state_ of any exception passed to `OnError` before passing the exception on to its observer. Other than that, this operator just passes all notifications through unmodified.

The purpose of this operator is to make it easier to conform with the rules described in the [Exception state](#exception-state) section above. You typically only need to use if when an exception originates from an Rx observable without ever having executed a `throw`, and you then go on to cause that exception to leave Rx's world and to be rethrown in the normal .NET way.

For example, [`Observable.Throw`](03_CreatingObservableSequences.md#observablethrow) does not reset the state of the exception you give it (in case there was anything important in that state), but if you might be using a world-crossing mechanism like `await` that will rethrow the exception, and if you might do so multiple times for the same exception object, you will need to use this operator to ensure that the exception state gets reset each time.

This can also be useful when using the [`Replay`](15_PublishingOperators.md#replay) operator, because even if the underlying source is using `throw` (thus resetting the exception state), `Replay` will hold onto the exception, and will deliver the same object to any subsequent subscribers. The use of `Replay` effectively prevents the reset that would otherwise have happened. So if you then go on to use `await` (or any other of the mechanisms in this chapter that would cause an exception delivered to `OnError` to be rethrown) you will no longer be following the rules described in [Exception state](#exception-state), which can result in problems such as ever-longer stack traces.

If you remain entirely within Rx's world, you should not need to use `ResetExceptionDispatchState`. This operator exists only to deal with a problem that can occur when crossing from Rx's world to the world of conventional exception throwing.

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