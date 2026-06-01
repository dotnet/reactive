# Windows Forms support for Rx (Reactive Extensions for .NET)

This is part of the Reactive Extensions for .NET (Rx). Rx enables event-driven programming with a composable, declarative model. The main Rx package is `System.Reactive`, which provides the core types and operators. This package, `System.Reactive.Windows.Forms`, provides additional support for using Rx with Windows Forms applications.

If you had been using the Windows Forms support in earlier versions of Rx (pre v7), you'll know that all UI-framework-specific functionality used to live in the main `System.Reactive` package. See [ADR 0005 (Moving UI framework support out of `System.Reactive`)](https://github.com/dotnet/reactive/blob/main/Rx.NET/Documentation/adr/0005-package-split.md) for an in depth explanation of the reason for moving these features out into separate packages.


## Getting started

Run the following at a command line:

```ps1
mkdir TryRxWinforms
cd TryRxWinforms
dotnet new winforms
dotnet add package System.Reactive.Windows.Forms
```

Alternatively, if you have Visual Studio installed, create a new .NET WPF project, and then use the NuGet package manager to add a reference to `System.Reactive.Wpf`.

You can then add the following code to your `Form1.cs`. First, add this `using` directive at the top of the file:

```cs
using System.Reactive.Linq;
```

Then inside the constructor, **after** the call to `InitializeComponent()` add this:


```cs
IObservable<long> ticks = Observable.Timer(
    dueTime: TimeSpan.Zero,
    period: TimeSpan.FromSeconds(1));

ticks
    .ObserveOn(this)
    .Subscribe(tick => this.Text = $"Tick {tick}");
```

This creates an observable source (`ticks`) that produces an event once every second. It adds a handler to that source that updates the main window's title bar text. By default, an `Observable.Timer` created in this way will raise events on a thread pool thread, which would result in an exception when trying to set the window title bar. This example avoids that problem by calling `ObserveOn(this)`. This invokes an overload of `ObserveOn` that is specific to the `System.Reactive.Windows.Forms` library. It declares that we want a wrapper around the `ticks` observable that will raise notifications through the event loop for the `Control` passed to `ObserveOn`. So when the handler specified in `Subscribe` executes, it does so on the UI thread, meaning it is able to update the window title successfully.

## Feedback

You can create issues at the https://github.com/dotnet/reactive repository