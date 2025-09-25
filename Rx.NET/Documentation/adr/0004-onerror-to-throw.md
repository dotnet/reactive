# Rules for when OnError notifications become thrown exceptions

Rx uses .NET `Exception` objects in a slightly unusual way: they are typically not thrown. Instead they are passed as the argument to an observer's `OnError` method. There are some situations in which an error reported in this way will end up causing an exception to be thrown. For example, using `await` on an `IObservable<T>` will do this:

```cs
IObservable<int> ts = Observable.Throw<int>(new Exception("Pow!"));

await ts; // Exception thrown here
```

This can cause problems. For example, as [#2187](https://github.com/dotnet/reactive/issues/2187) describes, if you `await` the observable shown in this example multiple times, the exception's `StackTrace` gets longer each time.

Problems arise because there use of singleton exception objects is slightly tricky even with straightforward use of `throw`, but it becomes a good deal more subtle when you start to 'cross the streams' of normal .NET exception handling and Rx's use of `Exception` in `OnError`.

Rx has never previously offered any guidance that would enable a developer to understand that the code shown above might have problems. The goal of this ADR is to establish suitable rules.

## Status

Proposed


## Authors

@idg10 ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/)).



## Context

Exceptions may appear to be ordinary .NET objects, but they get special handling from the runtime. MSIL has a special [`throw`](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.throw?view=net-9.0) instruction for the purpose of raising exceptions, and there is C++ code inside the CLR that directly manipulates certains fields that exist in these .NET objects. So expectations around the use of exception types is baked deeply into the runtime.

Rx does not generally use exceptions in the way the runtime expects. In particular it does not use the MSIL `throw` instruction to raise an exception: instead we just pass an exception object as an argument to the `IObserver<T>.OnError` method.

(This does not accurately reflect the actual runtime behaviour: the call stack does not in fact get deeper. It's just that the `StackTrace` ends up with multiple copies of the )

One of the (apparently unstated) assumptions made by the .NET Runtime is that each call to `ExceptionDispatchInfo.Throw` will correspond to a `throw` of the same exception.