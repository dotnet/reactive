# Appendix B: Disposables
    
Rx represents subscriptions using the existing `IDisposable` interface. This design choice means we can use existing language features that know how to work with this interface. Rx also provides several public implementations of `IDisposable`. These can be found in the `System.Reactive.Disposables` namespace. This appendix briefly describes each of them.

With the exception of [`ScheduledDisposable`](#scheduleddisposable), these have no particular connection to Rx, and can be useful in any code that needs to work with `IDisposable`. (This code all lives in `System.Reactive` though, so although you could uses these features entirely outside of Rx-based code, you will still be taking a dependency on Rx.NET if you do so.)

## `Disposable.Empty`
This static property exposes an implementation of `IDisposable` that performs no action when the `Dispose` method is invoked. This can be useful when you are obliged to supply an `IDisposable` (which can happen if you use `Observable.Create`) but don't need to do anything upon disposal.

## `Disposable.Create(Action)`

This static method exposes an implementation of `IDisposable` that invokes the method supplied when the `Dispose` method is invoked. As the implementation follows the guidance to be idempotent, the action will only be called on the first time the `Dispose` method is invoked.

## `BooleanDisposable`

This class implements `IDisposable.Dispose` method and also defines a read-only property `IsDisposed`. `IsDisposed` is <code>false</code> when the class is constructed, and is set to <code>true</code> when the `Dispose` method is invoked.

## `CancellationDisposable`

The `CancellationDisposable` class offers an integration point between the .NET [cancellation paradigm](https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation) (`CancellationTokenSource`) and the resource management paradigm (`IDisposable`). You can create an instance of the `CancellationDisposable` class by providing a `CancellationTokenSource` to the constructor, or by having the parameterless constructor create one for you. Calling `Dispose` will invoke the `Cancel` method on the `CancellationTokenSource`. There are two properties (`Token` and `IsDisposed`) that `CancellationDisposable` exposes; they are wrappers for the `CancellationTokenSource` properties, respectively `Token` and `IsCancellationRequested`.

## `CompositeDisposable`

The `CompositeDisposable` type allows you to treat many disposable resources as one. You can create an instance of `CompositeDisposable` by passing in a <code>params</code> array of disposable resources. Calling `Dispose` on the `CompositeDisposable` will call dispose on each of these resources in the order they were provided. Additionally, the `CompositeDisposable` class implements `ICollection<IDisposable>`; this allows you to add and remove resources from the collection. After the `CompositeDisposable` has been disposed of, any further resources that are added to this collection will be disposed of instantly. Any item that is removed from the collection is also disposed of, regardless of whether the collection itself has been disposed of. This includes usage of both the `Remove` and `Clear` methods.

## `ContextDisposable`
`ContextDisposable` allows you to enforce that disposal of a resource is performed on a given `SynchronizationContext`. The constructor requires both a `SynchronizationContext` and an `IDisposable` resource. When the `Dispose` method is invoked on the `ContextDisposable`, the provided resource will be disposed of on the specified context.

## `MultipleAssignmentDisposable`

The `MultipleAssignmentDisposable` exposes a read-only `IsDisposed` property and a read/write property `Disposable`. Invoking the `Dispose` method on the `MultipleAssignmentDisposable` will dispose of the current value held by the `Disposable` property. It will then set that value to null. As long as the `MultipleAssignmentDisposable` has not been disposed of, you are able to set the `Disposable` property to `IDisposable` values as you would expect. Once the `MultipleAssignmentDisposable` has been disposed, attempting to set the `Disposable` property will cause the value to be instantly disposed of; meanwhile, `Disposable` will remain null.

## `RefCountDisposable`

The `RefCountDisposable` offers the ability to prevent the disposal of an underlying resource until all dependent resources have been disposed. You need an underlying `IDisposable` value to construct a `RefCountDisposable`. You can then call the `GetDisposable` method on the `RefCountDisposable` instance to retrieve a dependent resource. Each time a call to `GetDisposable` is made, an internal counter is incremented. Each time one of the dependent disposables from `GetDisposable` is disposed, the counter is decremented. Only if the counter reaches zero will the underlying be disposed of. This allows you to call `Dispose` on the `RefCountDisposable` itself before or after the count is zero.

## `ScheduledDisposable`

In a similar fashion to `ContextDisposable`, the `ScheduledDisposable` type allows you to specify a scheduler, onto which the underlying resource will be disposed. You need to pass both the instance of `IScheduler` and instance of `IDisposable` to the constructor. When the `ScheduledDisposable` instance is disposed of, the disposal of the underlying resource will be executed through the provided scheduler.

## `SerialDisposable`

`SerialDisposable` is very similar to `MultipleAssignmentDisposable`, as they both expose a read/write `Disposable` property. The contrast between them is that whenever the `Disposable` property is set on a `SerialDisposable`, the previous value is disposed of. Like the `MultipleAssignmentDisposable`, once the `SerialDisposable` has been disposed of, the `Disposable` property will be set to null and any further attempts to set it will have the value disposed of. The value will remain as null.

## `SingleAssignmentDisposable`

The `SingleAssignmentDisposable` class also exposes `IsDisposed` and `Disposable` properties. Like `MultipleAssignmentDisposable` and `SerialDisposable`, the `Disposable` value will be set to null when the `SingleAssignmentDisposable` is disposed of. The difference in implementation here is that the `SingleAssignmentDisposable` will throw an `InvalidOperationException` if there is an attempt to set the `Disposable` property while the value is not null and the `SingleAssignmentDisposable` has not been disposed of.

<!-- 
TODO: we recently made SingleAssignmentDisposableValue public after a request to do so. This also doesn't mention MultipleAssignmentDisposableValue, which has been around for a while.

TODO: ICancelable?

TODO: StableCompositeDisposable?

TODO: fit this in?

```csharp
namespace System.Reactive.Disposables
{
    public static class Disposable
    {
    // Gets the disposable that does nothing when disposed.
    public static IDisposable Empty { get {...} }

    // Creates the disposable that invokes the specified action when disposed.
    public static IDisposable Create(Action dispose)
    {...}
    }
}
```

As you can see it exposes two members: `Empty` and `Create`. The `Empty` method allows you get a stub instance of an `IDisposable` that does nothing when `Dispose()` is called. This is useful for when you need to fulfil an interface requirement that returns an `IDisposable` but you have no specific implementation that is relevant.

The other overload is the `Create` factory method which allows you to pass an `Action` to be invoked when the instance is disposed. The `Create` method will ensure the standard Dispose semantics, so calling `Dispose()` multiple times will only invoke the delegate you provide once:

```csharp
var disposable = Disposable.Create(() => Console.WriteLine("Being disposed."));
Console.WriteLine("Calling dispose...");
disposable.Dispose();
Console.WriteLine("Calling again...");
disposable.Dispose();
```

Output:

```
Calling dispose...
Being disposed.
Calling again...
```

Note that "Being disposed." is only printed once. -->