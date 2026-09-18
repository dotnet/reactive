# Testing utilities for AsyncRx.NET (Reactive Extensions for .NET, async edition)

This package is the AsyncRx.NET counterpart of `Microsoft.Reactive.Testing`. It enables virtual-time-based testing of `IAsyncObservable<T>` operators built with `System.Reactive.Async`, using the same `OnNext`/`OnCompleted`/`Subscribe` vocabulary as the synchronous testing library, so it may be useful to libraries defining their own custom AsyncRx operators.

Like `System.Reactive.Async` itself, this package is an experimental preview. It is mainly designed for internal use in the https://github.com/dotnet/reactive repository. Its use is currently unsupported, and there is no commitment to backwards compatibility.
