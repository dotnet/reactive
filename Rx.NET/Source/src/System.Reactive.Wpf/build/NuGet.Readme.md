# WPF support for Rx (Reactive Extensions for .NET)

This is part of the Reactive Extensions for .NET (Rx). Rx enables event-driven programming with a composable, declarative model. The main Rx package is `System.Reactive`, which provides the core types and operators. This package, `System.Reactive.Wpf`, provides additional support for using Rx with WPF applications.

If you had been using the WPF support in earlier versions of Rx (pre v7), you'll know that all UI-framework-specific functionality used to live in the main `System.Reactive` package. See [ADR 0005 (Moving UI framework support out of `System.Reactive`)](https://github.com/dotnet/reactive/blob/main/Rx.NET/Documentation/adr/0005-package-split.md) for an in depth explanation of the reason for moving these features out into separate packages.

## Feedback

You can create issues at the https://github.com/dotnet/reactive repository