# Windows Forms Support for Rx.NET (Reactive Extensions for .NET)

This library provides Windows Forms support for the Reactive Extensions for .NET (Rx.NET).

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

|Type in `System.Reactive` | Replacement | Purpose |
|---|---|---|
|`ControlScheduler` (in `System.Reactive.Concurrency`) | `ControlScheduler` (in `System.Reactive.Integration.WindowsForms`) | Provides a scheduler that schedules work on the UI thread of a Windows Forms application. |
|`ControlObservable` (in `System.Reactive.Linq`) | `WindowsFormsControlObservable` (in `System.Reactive.Linq`) | Provides a set of extension methods for scheduling work on the UI thread of a Windows Forms application. |
