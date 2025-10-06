# Rules for when OnError notifications become thrown exceptions

Rx uses .NET `Exception` objects in a slightly unusual way: they are typically not thrown. Instead they are passed as the argument to an observer's `OnError` method. There are some situations in which an error reported in this way will end up causing an exception to be thrown. For example, if you `await` an `IObservable<T>` that calls `OnError`, the `await` will throw:

```cs
IObservable<int> ts = Observable.Throw<int>(new Exception("Pow!"));

await ts; // Exception thrown here
```

This can cause problems. For example, as [#2187](https://github.com/dotnet/reactive/issues/2187) describes, if you `await` the observable shown in this example multiple times, the exception's `StackTrace` gets longer each time.

Problems arise because the use of singleton exception objects is slightly tricky even with straightforward use of `throw`, but it becomes a good deal more subtle when you start to 'cross the streams' of normal .NET exception handling and Rx's use of `Exception` in `OnError`.

Rx has never previously offered any guidance that would enable a developer to understand that the code shown above might have problems. The purpose of this ADR is to establish suitable rules.

## Status

Proposed


## Authors

@idg10 ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/)).



## Context

Exceptions may appear to be ordinary .NET objects, but they get special handling from the runtime. MSIL has a [`throw`](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.throw?view=net-9.0) instruction for the purpose of raising exceptions, and there is C++ code inside the CLR that directly manipulates fields defined by the `Exception` class. Certain expectations around the use of exception types are baked deeply into the runtime.

Rx does not generally use exceptions in the way the runtime expects. In particular it does not normally use the MSIL `throw` instruction to raise an exception. Instead, when an Rx `IObservable<T>` wants to report an error, it just passes an exception object as an argument to the `IObserver<T>.OnError` method.

This causes no problems when an application remains entirely within Rx's world. But when we want to move into the more conventional .NET approach of throwing exceptions, it raises an interesting question: where should the exception appear to originate from? 

Consider this example:

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

If the attempt to open the file throws an exception, what do we expect to see? A developer familiar with how exceptions generally work with `async` in .NET might reasonably expect the exception to report two stack traces: one for the point at which the exception was originally thrown, and another for where it was rethrown from the `await`. And that's exactly what we see:

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

This is straightforward because the exception here is thrown in the conventional .NET manner. It happens to be caught by Rx—this overload of `Observable.Create` wraps the `Task` returned by the callback in an adapter that detects when the `Task` enters a faulted state, in which case it extracts the exception and passes it to the subscribing `IObserver<T>`. And then the awaiter that Rx provides when you `await` an observable rethrows this same exception.

But what about the earlier example in which the exception originated from `Observable.Throw`? In that code, we construct an `Exception` but we never use the `throw` keyword with it, and nor do we invoke any API that might do that for us. What would you expect the call stack to show in that case? In practice we get this:

```
Unhandled exception. System.Exception: Pow!
   at System.Reactive.PlatformServices.ExceptionServicesImpl.Rethrow(Exception exception)
   at System.Reactive.ExceptionHelpers.Throw(Exception exception)
   at System.Reactive.Subjects.AsyncSubject`1.GetResult()
   at Program.<Main>$(String[] args) in D:\source\RxThrowExamples\RxThrowExamples\Program.cs:line 20
   at Program.<Main>(String[] args)
```

This time, we've got just a single stack trace, effectively showing the `await`. This looks very similar to the 2nd trace from the previous example—the difference here is that we don't have an extra trace showing the original location from which the exception was first thrown. And you could argue that this makes sense: this particular exception wasn't thrown until it emerged from the `await`.

So far so good. But look what happens if we use this same observable source a few times:

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

Since we're doing the same thing three times, you might expect to see the same exception report three times. But that's not what happens:

```
System.Exception: Pow!
   at System.Reactive.PlatformServices.ExceptionServicesImpl.Rethrow(Exception exception)
   at System.Reactive.ExceptionHelpers.Throw(Exception exception)
   at System.Reactive.Subjects.AsyncSubject`1.GetResult()
   at Program.<Main>$(String[] args) in D:\source\RxThrowExamples\RxThrowExamples\Program.cs:line 27


System.Exception: Pow!
   at System.Reactive.PlatformServices.ExceptionServicesImpl.Rethrow(Exception exception)
   at System.Reactive.ExceptionHelpers.Throw(Exception exception)
   at System.Reactive.Subjects.AsyncSubject`1.GetResult()
   at Program.<Main>$(String[] args) in D:\source\RxThrowExamples\RxThrowExamples\Program.cs:line 27
   at System.Reactive.PlatformServices.ExceptionServicesImpl.Rethrow(Exception exception)
   at System.Reactive.ExceptionHelpers.Throw(Exception exception)
   at System.Reactive.Subjects.AsyncSubject`1.GetResult()
   at Program.<Main>$(String[] args) in D:\source\RxThrowExamples\RxThrowExamples\Program.cs:line 27


System.Exception: Pow!
   at System.Reactive.PlatformServices.ExceptionServicesImpl.Rethrow(Exception exception)
   at System.Reactive.ExceptionHelpers.Throw(Exception exception)
   at System.Reactive.Subjects.AsyncSubject`1.GetResult()
   at Program.<Main>$(String[] args) in D:\source\RxThrowExamples\RxThrowExamples\Program.cs:line 27
   at System.Reactive.PlatformServices.ExceptionServicesImpl.Rethrow(Exception exception)
   at System.Reactive.ExceptionHelpers.Throw(Exception exception)
   at System.Reactive.Subjects.AsyncSubject`1.GetResult()
   at Program.<Main>$(String[] args) in D:\source\RxThrowExamples\RxThrowExamples\Program.cs:line 27
   at System.Reactive.PlatformServices.ExceptionServicesImpl.Rethrow(Exception exception)
   at System.Reactive.ExceptionHelpers.Throw(Exception exception)
   at System.Reactive.Subjects.AsyncSubject`1.GetResult()
   at Program.<Main>$(String[] args) in D:\source\RxThrowExamples\RxThrowExamples\Program.cs:line 27
```

The stack trace gets longer each time!

(This does not accurately reflect the actual runtime behaviour: the call stack does not in fact get deeper. It's just that the `StackTrace` string with which an `Exception` reports this information ends up containing multiple copies of the stack trace.)

This makes no sense.

It occurs as a direct result of the steps Rx takes to produce the stack trace we expect in the earlier example. It uses the .NET runtime library's `ExceptionDispatchInfo.Throw` method to rethrow the exception from the `await`. That method preserves the original context in which the exception was thrown, and appends the context from which it is rethrown: this is how we end up with the multiple stack traces that .NET developers are accustomed to with normal use of `async` and `await`. (In fact, Rx is using exactly the same rethrow mechanism that enables this behaviour in conventional `async` code.) 

This behaviour is not peculiar to Rx. It originates from `ExceptionDispatchInfo.Throw` and we can create a `Task`-based version of this behaviour without using Rx:

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

The stack traces are shorter, but we see the same repeating behaviour:

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

(To be precise, this _doesn't_ happen if you create a single `Task` and `await` that multiple times. It's the combination of `ExceptionDispatchInfo.Capture` and `ExceptionDispatchInfo.Throw` that causes this accumulation, and this `Task` captures exception information at the point when we create it with `Task.FromException`.)

This appending of exception data is by design: `ExceptionDispatchInfo.Throw` is intended to append the current context to whatever was captured in the `ExceptionDispatchInfo`. The .NET runtime assumes that if you want to represent a new exceptional event that you will execute a `throw`. Rx does not do this in `await` (or other mechanisms that can rethrow an exception delivered through `OnError` such as `ToEnumerable`) precisely because it preserves whatever context was present when it received the exception. It does not perform a `throw` (or do anything else to reset the exception context) because this would prevent the full context being preserved in examples such as the `FileNotFoundException` handling shown earlier.

This behaviour makes sense in the context for which it was designed—capturing the context in which an exception was initially thrown and augmenting it with additional information if it is rethrown from a different context. But unless you are aware of that, it's not at all obvious that although there's nothing inherently wrong with using `Observable.Throw<int>()`, it is not compatible with having multiple subscribers that will each rethrow the exception.


## Decision

Rx.NET will explicitly adopt this position: if a developer using Rx chooses to use a mechanism that takes exceptions delivered by an `IObservable<T>` and throws them (e.g. if you `await` an `IObservable<T>`) then it is the developer's responsibility to ensure that either:

* each exception object is used only once

or

* the exception's dispatch state is reset prior to being supplied to the observer that will be rethrowing it (e.g., by executing a `throw`)

Since Rx defines operators that won't conform to the first option (notably `Observable.Throw`, but also `ReplaySubject` and the related `Observable.Replay`) Rx 6.1 introduces a new operator, `ResetExceptionDispatchState`. This passes all notifications through, but effectively performs a `throw` on any `Exception` before forwarding it. It can be used like this:

```cs
var ts = Observable.Throw<int>(new Exception("Aaargh!")).ResetExceptionDispatchState();
```

When an observer subscribes to this, the `Throw` immediately calls `OnError`, and the `ResetExceptionDispatchState` will throw (and immediately catch) that exception before passing it on to the subscriber. (You would _not_ use this in scenarios such as the `Create` example shown earlier, because in that case each exception is freshly thrown, and has useful contextual information so we don't want to reset that. This is for use specifically in cases where the exception would not otherwise be thrown.)


## Consequences

By adopting this position, we make it clear that examples such as the one in [#2187](https://github.com/dotnet/reactive/issues/2187) are not expected to work correctly.

More generally, this clarifies that observable sources that repeatedly produce the same exception object (e.g. `Observable.Throw` or `Observable.Repeat`) are incompatible with multiple calls to `await`.

The addition of the `ResetExceptionDispatchState` operator provides a clear, simple way to fix code that runs into this problem.