# UWP support for Rx (Reactive Extensions for .NET)

This is part of the Reactive Extensions for .NET (Rx). Rx enables event-driven programming with a composable, declarative model. The main Rx package is `System.Reactive`, which provides the core types and operators. This package, `System.Reactive.Uwp`, provides additional support for using Rx with UWP applications (on either .NET, or the legacy Native .NET `uap` platform).

If you had been using the UWP support in earlier versions of Rx (pre v7), you'll know that all UI-framework-specific functionality used to live in the main `System.Reactive` package. See [ADR 0005 (Moving UI framework support out of `System.Reactive`)](https://github.com/dotnet/reactive/blob/main/Rx.NET/Documentation/adr/0005-package-split.md) for an in depth explanation of the reason for moving these features out into separate packages.


## Getting started

In Visual Studio, create a new _UWP Blank App_.

Open the NuGet Package Manager for the new project, and in the **Browse** tab, search for `System.Reactive.Uwp`. Install the package.

You can then add the following code to your `MainPage.xaml.cs`. First, add this `using` directive at the top of the file:

```cs
using System.Reactive.Linq;
```

Then inside the constructor, **after** the call to `InitializeComponent()` add this:


```cs
TextBlock t = new() { FontSize = 24 };
((Grid)this.Content).Children.Add(t);

IObservable<long> ticks = Observable.Timer(
    dueTime: TimeSpan.Zero,
    period: TimeSpan.FromSeconds(1));

ticks
    .ObserveOn(this)
    .Subscribe(tick => t.Text = $"Tick {tick}");
```

This creates an observable source (`ticks`) that produces an event once every second. It adds a handler to that source that updates text in a `TextBlock` (which this constructor adds to the window's main `Grid`). By default, an `Observable.Timer` created in this way will raise events on a thread pool thread, which would result in an exception when trying to access the `TextBlock`. This example avoids that problem by calling `ObserveOn(this)`. This invokes an overload of `ObserveOn` that is specific to the `System.Reactive.Uwp` library. It declares that we want a wrapper around the `ticks` observable that will raise notifications through the specified `Dispatcher`. So when the handler specified in `Subscribe` executes, it does so on the dispatcher thread, meaning it is able to update the text successfully.


## Feedback

You can create issues at the https://github.com/dotnet/reactive repository