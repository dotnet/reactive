# UWP Support for Rx.NET (Reactive Extensions for .NET)

This library provides UWP (Universal Windows Platform) support for the Reactive Extensions for .NET (Rx.NET).

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

This package defines one public type, `UwpThreadPoolScheduler`. It provides a replacement for deprecated functionality on the
`ThreadPoolScheduler` in the `uap10.0.18362` target of `System.Reactive`. In a future version of Rx.NET, the UWP-specific
target will be removed the main `System.Reactive` package, at which point UWP applications will use the `netstandard2.0`
target, and will get only the common `ThreadPoolScheduler` functionality available on all platforms.

The specialized `ThreadPoolScheduler` currently still available in the `uap10.0.18362` target of `System.Reactive` is
very nearly the same as the common one. It has some extra properties and constructors providing access to some features
specific to the `Windows.System.Threading` thread pool (which is available only on UWP). It allows the use of
`WorkItemPriority` and `WorkItemOptions`. These features are now deprecated on the `ThreadPoolScheduler` in the
`uap10.0.18362` target of `System.Reactive`, making its remaining non-deprecated surface area the same as in
other targets.

Applications still needing access to the UWP-specific functionality can switch to the `UwpThreadPoolScheduler`
in this library.


## Windows Runtime Support

The `uap10.0.18362` target of `System.Reactive` also offers support for some Windows Runtime types, such as `IAsyncOperation`,
that are often used in UWP applications. Those types are also available outside of UWP, so they are also available in
the Windows-specific .NET target, but they have also been deprecated, as part of the drive to remove all platform- and
UI-framework-specific code from `System.Reactive`. You can find support for those in the `System.Reactive.Integration.WindowsRuntime`
NuGet package.