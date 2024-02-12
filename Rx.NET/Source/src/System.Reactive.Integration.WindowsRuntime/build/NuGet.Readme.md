# Windows Runtime Support for Rx.NET (Reactive Extensions for .NET)

This library provides support for using some common Windows Runtime types from the Reactive Extensions for .NET (Rx.NET).

See the main Rx.NET package at https://www.nuget.org/packages/System.Reactive for more information about Rx.NET.

## Rx.NET and UI Frameworks

Up as far as Rx.NET v6.0, UI framework support has been built directly into the main `System.Reactive` package.
Unfortunately, this has caused problems since support for WPF and Windows Forms was added in .NET Core 3.1.
Because .NET Core 3.1, and all subsequent versions of .NET have supported cross-platform use, WPF and Windows
Forms are not universally available. Rx.NET used to make WPF and Windows Forms support if you targetted a
sufficiently recent version of Windows in your application TFM. But this turns out to cause problems in
some deployment models, adding as much as 90MB to the deployable size of an application.

Consequently, starting in Rx.NET v7.0 we are moving all UI-framework-specific types, and also platform-specific
types out into separate packages.

## Features

This package provides replacements for two deprecated types in `System.Reactive`:

| Type in `System.Reactive` | Replacement | Purpose |
|---|---|---|
| `CoreDispatcherScheduler` (in `System.Reactive.Concurrency`) | `CoreDispatcherScheduler` (in `System.Reactive.Integration.WPF`) | Provides a scheduler that schedules work on the UI thread of applications using `CoreDispatcher` (e.g., UWP applications). |
| `CoreDispatcherObservable` (in `System.Reactive.Linq`) | `WindowsRuntimeCoreDispatcherObservable` (in `System.Reactive.Linq`) | Provides a set of extension methods for scheduling work on the UI thread of an application using `CoreDispatcher` (e.g., UWP applications). WPF application. |
| `WindowsObservable` (in `System.Reactive.Linq`) | `WindowsRuntimeObservable` (in `System.Reactive.Linq`) | Provides integration between `TypedEventHandler<TSender, TEventArgs` and `IObservable<T>`, and also `SelectMany` support for callbacks using the Windows Runtime asynchronous operation types (`IAsyncOperation` etc.) and `IObservable<T>`.
| `AsyncInfoObservable` (in `System.Reactive.Linq`) | `WindowsRuntimeAsyncInfoObservable` (in `System.Reactive.Integration.WindowsRuntime`) | Provides conversions `IObservable<T>` top Windows Runtime asynchronous operation types (`IAsyncOperation` etc.). |
| `AsyncInfoObservableExtensions` (in `System.Reactive.Windows.Foundation`) | `AsyncInfoObservableExtensions` (in `System.Reactive.Integration.WindowsRuntime` | Provides conversion from Windows Runtime asynchronous operation types (`IAsyncOperation` etc.) and `IObservable<T>`. 
| `IEventPatternSource<TSender, TEventArgs>` (in `System.Reactive`) | `ITypedEventPatternSource<TSender, TEventArgs>` in `System.Reactive.Integration.WindowsRuntime` | Represents a source of events exposed as a Windows Runtime `TypedEventHandler<TSender, TEventArgs>`. |