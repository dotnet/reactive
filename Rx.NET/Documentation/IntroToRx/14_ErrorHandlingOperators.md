# Error Handling Operators

Exceptions happen. Some exceptions are inherently avoidable, occurring only because of bugs in our code. For example, if we put the CLR into a situation where it has to raise a `DivideByZeroException`, we've done something wrong. But there are plenty of exceptions that cannot be prevented with defensive coding. For example, exceptions relating to I/O or networking failures such as like `FileNotFoundException` or `TimeoutException` can be caused by environmental factors outside of our code's control. In these cases, we need to handle the exception gracefully. The kind of handling will depend on the context. It might be appropriate to provide some sort of error message to the user; in some scenarios logging the error might be a more appropriate response. If the failure is likely to be transient, we could try to recover by retrying the operation that failed.

The `IObserver<T>` interface defines the `OnError` method so that a source can report an error, but since this terminates the sequence, it provides no direct means of working out what to do next. However, Rx provides operators that provide a variety of error handling mechanisms.

## Catch

Rx defines a `Catch` operator. The name is deliberately reminiscent of C#'s `try`/`catch` syntax because it lets you handle errors from an Rx source in a similar way to exceptions that emerge from normal execution of code. It can work in two different ways. You can just supply a function to which Rx will pass the error, and this function can return an `IObservable<T>`, and `Catch` will now forward items from that instead of the original source. Or, instead of passing a function, you can just supply one or more additional sequences, and catch will move onto the next each time the current one fails.

### Examining the exception

`Catch` has an overload that enables you provide a handler to be invoked if the source produces an error:

```csharp
public static IObservable<TSource> Catch<TSource, TException>(
    this IObservable<TSource> source, 
    Func<TException, IObservable<TSource>> handler) 
    where TException : Exception
```

This is conceptually very similar to a C# `catch` block: we can write code that looks at the exception and then decides how to proceed. And as with a `catch` block we can decide which kinds of exceptions we are interested in. For example, we might know that the source will sometimes produce a `TimeoutException`, and we might just want to return an empty sequence in that case, instead of an error:

```csharp
IObservable<int> result = source.Catch<int, TimeoutException>(_ => Observable.Empty<int>());
```

`Catch` will only invoke our function if the exception is of the type specified (or a type derived from that). If the sequence was to terminate with an `Exception` that could not be cast to a `TimeoutException`, then the error would not be caught and would flow through to the subscriber.

This example returns `Observable.Empty<int>()`. This is conceptually similar to 'swallowing' an exception in C#, i.e., choosing to take no action. This can be a reasonable response to an exception that you anticipate, but it is generally a bad idea to do this with the base `Exception` type.

This last example ignored its input, because it was interested only in the exception type. However, we are free to examine the exception, and make more fine-grained decisions about what should emerge from the `Catch`:

```csharp
IObservable<string> result = source.Catch(
    (FileNotFoundException x) => x.FileName == "settings.txt"
        ? Observable.Return(DefaultSettings) : Observable.Throw<string>(x));
```

This provides special handling for one particular file, but otherwise rethrows the exception. Returning `Observable.Throw<T>(x)` here (where `x` is the original exception) is conceptually similar to writing `throw` in a catch block. (In C# there's an important distinction between `throw;` and `throw x;` because it changes how exception context is captured, but in Rx, `OnError` doesn't capture a stack trace, so there's no equivalent distinction.)

You're also free to throw a completely different exception, of course. You can return any `IObservable<T>` you like, as long as its element type is the same as the source's.

### Fallback

The other overloads of `Catch` offer less discriminating behaviour: you can supply one or more additional sequences, and any time the current source fails, the exception will be ignored, and `Catch` will simply move onto the next sequence. Since you will never get to know what the exception is, this mechanism gives you no way of knowing whether the exception that occurred was one you anticipated, or a completely unexpected one, so you will normally want to avoid this form. But for completeness, here's how to use it:

```csharp
IObservable<string> settings = settingsSource1.Catch(settingsSource2);
```

That form provides just a single fallback. There's also a static `Observable.Catch` method that takes a `params` array, so you can pass any number of sources. This is exactly equivalent to the preceding example:

```csharp
IObservable<string> settings = Observable.Catch(settingsSource1, settingsSource2);
```

There's also an overload that accepts an `IEnumerable<IObservable<T>>`.

If any of the sources reaches its end without reporting an exception, `Catch` also immediately reports completion and does not subscribe to any of the subsequent sources. If the very last source reports an exception, `Catch` will have no further sources to fall back on, so in that case it won't catch the exception. It will forward that final exception to its subscriber.

## Finally

Similar to a `finally` block in C#, Rx enables us to execute some code on completion of a sequence, regardless of whether it runs to completion naturally or fails. Rx adds a third mode of completion that has no exact equivalent in `catch`/`finally`: the subscriber might unsubscribe before the source has a chance to complete. (This is conceptually similar to using `break` to terminate a `foreach` early.) The `Finally` extension method accepts an `Action` as a parameter. This `Action` will be invoked when the sequence terminates, regardless of whether `OnCompleted` or `OnError` was called. It will also invoke the action if the subscription is disposed of before it completes.

```csharp
public static IObservable<TSource> Finally<TSource>(
    this IObservable<TSource> source, 
    Action finallyAction)
{
    ...
}
```

In this example, we have a sequence that completes. We provide an action and see that it is called after our `OnCompleted` handler.

```csharp
var source = new Subject<int>();
IObservable<int> result = source.Finally(() => Console.WriteLine("Finally action ran"));
result.Dump("Finally");
source.OnNext(1);
source.OnNext(2);
source.OnNext(3);
source.OnCompleted();
```

Output:

```
Finally-->1
Finally-->2
Finally-->3
Finally completed
Finally action ran
```

The source sequence could also have terminated with an exception. In that case, the exception would have been sent to the subscriber's `OnError` (and we'd have seen that in the console output), and then the delegate we provided to `Finally` would have been executed.

Alternatively, we could have disposed of our subscription. In the next example, we see that the `Finally` action is invoked even though the sequence does not complete.

```csharp
var source = new Subject<int>();
var result = source.Finally(() => Console.WriteLine("Finally"));
var subscription = result.Subscribe(
    Console.WriteLine,
    Console.WriteLine,
    () => Console.WriteLine("Completed"));
source.OnNext(1);
source.OnNext(2);
source.OnNext(3);
subscription.Dispose();
```

Output:

```
1
2
3
Finally
```

Note that if the subscriber's `OnError` throws an exception, and if the source calls `OnNext` without a `try`/`catch` block, the CLR's unhandled exception reporting mechanism kicks in, and in some circumstances this can result in the application shutting down before the `Finally` operator has had an opportunity to invoke the callback. We can create this scenario with the following code:

```csharp
var source = new Subject<int>();
var result = source.Finally(() => Console.WriteLine("Finally"));
result.Subscribe(
    Console.WriteLine,
    // Console.WriteLine,
    () => Console.WriteLine("Completed"));
source.OnNext(1);
source.OnNext(2);
source.OnNext(3);

// Brings the app down. Finally action might not be called.
source.OnError(new Exception("Fail"));
```

If you run this directly from the program's entry point, without wrapping it in a `try`/`catch`, you may or may not see the `Finally` message displayed, because exception handling works subtly differently in the case an exception reaches all the way to the top of the stack without being caught. (Oddly, it usually does run, but if you have a debugger attached, the program usually exits without running the `Finally` callback.)

This is mostly just a curiosity: application frameworks such as ASP.NET Core or WPF typically install their own top-of-stack exception handlers, and in any case you shouldn't be subscribing to a source that you know will call `OnError` without supplying an error callback. This problem only emerges because the delegate-based `Subscribe` overload in use here supplies an `IObserver<T>` implementation that throws in its `OnError`. However, if you're building console applications to experiment with Rx's behaviour you are quite likely to run into this. In practice, `Finally` will do the right thing in more normal situations. (But in any case, you shouldn't throw exceptions from an `OnError` handler.)

## Using

The `Using` factory method allows you to bind the lifetime of a resource to the lifetime of an observable sequence. The method takes two callbacks: one to create the disposable resource and one to provide the sequence. This allows everything to be lazily evaluated. These callbacks are invoked when code calls `Subscribe` on the `IObservable<T>` that this method returns.

```csharp
public static IObservable<TSource> Using<TSource, TResource>(
    Func<TResource> resourceFactory, 
    Func<TResource, IObservable<TSource>> observableFactory) 
    where TResource : IDisposable
{
    ...
}
```

The resource will be disposed of when the sequence terminates either with `OnCompleted` or `OnError`, or when the subscription is disposed.

## OnErrorResumeNext

Just the title of this section will send a shudder down the spines of old VB developers! (For those not familiar with this murky language feature, the VB language lets you instruct it to ignore any errors that occur during execution, and to just continue with the next statement after any failure.) In Rx, there is an extension method called `OnErrorResumeNext` that has similar semantics to the VB keywords/statement that share the same name. This extension method allows the continuation of a sequence with another sequence regardless of whether the first sequence completes gracefully or due to an error.

This is very similar to the second form of `Catch` (as described in [Fallback](#fallback)). The difference is that with `Catch`, if any source sequence reaches its end without reporting an error, `Catch` will not move onto the next sequence. `OnErrorResumeNext` will forward all elements produced by all of its inputs, so it is similar to [`Concat`](09_CombiningSequences.md#concat), it just ignores all errors.

Just as the `OnErrorResumeNext` keyword was best avoided for anything other than throwaway code in VB, so should it be used with caution in Rx. It will swallow exceptions quietly and can leave your program in an unknown state. Generally, this will make your code harder to maintain and debug. (The same applies for the fallback forms of `Catch`.)

## Retry

If you are expecting your sequence to encounter predictable failures, you might simply want to retry. For example, if you are running in a cloud environment, it's very common for operations to fail occasionally for no obvious reason. Cloud platforms often relocate services on a fairly regular basis for operational reasons, which means it's not unusual for operations to fail—you might make a request to a service just before the cloud provider decided to move that service to a different compute node—but for the exact same operation to succeed if you immediately retry it (because the retried request gets routed to the new node). Rx's `Retry` extension method offers the ability to retry on failure a specified number of times or until it succeeds. This works by resubscribing to the source if it reports an error.

This example uses the simple overload, which will always retry on any exception.

```csharp
public static void RetrySample<T>(IObservable<T> source)
{
    source.Retry().Subscribe(t => Console.WriteLine(t)); // Will always retry
    Console.ReadKey();
}
```

Given a source that produces the values 0, 1, and 2, and then calls `OnError`, the output would be the numbers 0, 1, 2 repeating over an over endlessly. This output would continue forever because this example never unsubscribes, and `Retry` will retry forever if you don't tell it otherwise.

We can specify the maximum number of retries. In this next example, we only retry once, therefore the error that gets published on the second subscription will be passed up to the final subscription. Note that we tell `Retry` the maximum number of attempts, so if we want it to retry once, you pass a value of 2—that's the initial attempt plus one retry.

```csharp
source.Retry(2).Dump("Retry(2)"); 
```

Output:

```
Retry(2)-->0
Retry(2)-->1
Retry(2)-->2
Retry(2)-->0
Retry(2)-->1
Retry(2)-->2
Retry(2) failed-->Test Exception
```

Proper care should be taken when using the infinite repeat overload. Obviously if there is a persistent problem with your underlying sequence, you may find yourself stuck in an infinite loop. Also, take note that there is no overload that allows you to specify the type of exception to retry on.

Rx also offers a `RetryWhen` method. This is similar to the first `Catch` overload we looked at: instead of handling all exceptions indiscriminately, it lets you supply code that can decide what to do. It works slightly differently: instead of invoking this callback once per error, it invokes it once passing in an `IObservable<Exception>` through which it will supply all of the exceptions, and the callback returns an `IObservable<T>` referred to as the _signal_ observable. The `T` can be anything, because the values this observable may return will be ignored: all that matters is which of the three `IObserver<T>` methods is invoked.

If, when receiving an exception, the signal observable calls `OnError`, `RetryWhen` will not retry, and will report that same error to its subscribers. If on the other hand the signal observable calls `OnCompleted`, again `RetryWhen` will not retry, and will complete without reporting an error. But if the signal observable calls `OnNext`, this causes `RetryWhen` to retry by resubscribing to the source.

<!--TODO: Build BackOffRetry with the reader-->

Applications often need exception management logic that goes beyond simple `OnError` handlers. Rx delivers exception handling operators similar to those we are used to in C#, which you can use to compose complex and robust queries. In this chapter we have covered advanced error handling and some more resource management features from Rx.