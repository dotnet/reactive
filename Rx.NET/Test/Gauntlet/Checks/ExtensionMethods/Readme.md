# Extension Method Build Failures on Windows-Specific TFMs

One possible workaround for the [bloat problem](../Bloat/) is to block the reference to the `Microsoft.Desktop.App` framework. But this can cause a new problem.

There are two ways the transitive reference to the `Microsoft.Desktop.App` framework might be blocked:

* An application might add `<DisableTransitiveFrameworkReferences>true</DisableTransitiveFrameworkReferences>` to its project file (which is something you can do today with Rx 6.0.1)
* A candidate future version of Rx might remove the problematic `<frameworkReference>` from the NuGet package's `nuspec` file

Either of these approaches will mean that Rx.NET no longer forces `Microsoft.Desktop.App` to be deployed, which does fix the bloat problem. However, a new problem arises if your application does something like this:

```cs
IObservable<int> xs = Observable.Range(1, 10);
xs.ObserveOn(SynchronizationContext.Current).Subscribe(x => Console.WriteLine($"OnNext({x})"));
```

This code does not attempt to use any UI-framework-specific Rx features. (The overload of `ObserveOn` that takes a `SynchronizationContext` is available on all targets, including `netstandard2.0`.) And yet you will find that if:

* Your application includes code like the above using Rx 6.0.1
* Your application has a Windows-specific TFM for v10.0.19041 or later
* You have used `DisableTransitiveFrameworkReferences` to prevent the bloat problem

you will get errors reporting that the compiler is unable to find certain WPF and Windows Forms types.

For certain ways of attempting to fix the [bloat problem](../Bloat/) by changing how Rx is packaged problems, this same problem could occur with just these criteria:

* Your application includes code like the above using Rx v.next
* Your application has a Windows-specific TFM for v10.0.19041 or later

This would be unacceptable, which is why we test for this problem.


## Problem Detail

This problem arises from the fact that the `System.Reactive.dll` assembly contains various public APIs that refer to WPF and Windows Forms types. This is true for Rx versions 4 through 6.0.1. What may be less obvious is that it also needs to be true for any candidate future version.

If some future version of Rx.NET did not continue to provide WPF and Windows Forms types in `System.Reactive`, this would be a breaking change. And although breaking changes can sometimes be the least bad solution, in this case they are especially bad because they can put application authors in a situation where upgrading some component (not necessarily an Rx component) stops their code from working, and the only way they can fix it is either to roll back the change, or to stop using Rx (and possibly to stop using any libraries that use Rx internally). This doesn't occur for simple secanrios, but see the [TransitiveReferences](../TransitiveReferences/) folder for information on why simply deciding to remove these from `System.Reactive` v.next isn't a reasonable solution, even if we do continue to make them available through new packages.)

The bottom line is that future versions of `System.Reactive` need to continue to make WPF and Windows Forms support available at runtime.

Given that this is the case, preventing the `Microsoft.Desktop.App` framework then creates the problem with methods like `ObserveOn`. The `net6.0-windows10.0.19041` TFM of `System.Reactive` defines WPF- and Windows-Forms-specific overloads for the `ObserveOn` extension method for `IObservable<T>`. When the compiler encounters this line from the example above:

```cs
xs.ObserveOn(SynchronizationContext.Current).Subscribe(x => Console.WriteLine($"OnNext({x})"));
```

it attempts to resolve that `ObserveOn` method invocation. To do this, it looks in all referenced libraries to see if any of the types in scope due to `using` directives define extension methods of that name for `IObservable<T>`. It will find all the normal non-UI ones including the `SynchronizationContext` overload that this code is trying to use. It will also find overloads that take a WPF's `Dispatcher` and `DispatcherObject` types, and Windows Forms' `Control` type.

This code is not trying to use those UI-specific overloads. But the compiler doesn't know that yet. To determine whether those might be candidates it has to look at their definitions. But when it does that, it realises that it can't find the `Dispatcher`, `DispatcherObject`, or `Control` types that these overloads expect. This means the compiler is unable to understand these overloads fully, so it doesn't really know whether they are candidates or not. And that's why it reports an error.


## Possible Fixes

One partial solution to this is to move most of Rx into a new package. (We have a new `System.Reactive.Net` package in some prototypes.) This new package can be entirely free from UI-framework-specific code, thus avoiding the bloat problem, and also avoiding this method overload problem. Applications that genuinely want the UI-framework-specific behaviour can get it by using new packages for that purpose.

This is a partial solution because the old `System.Reactive` needs to exist. And we'd need to publish a new version that would consist of type forwarders to the new `System.Reactive.Net`. (Otherwise, you could end up with two copies of Rx in scope simultaneously. That breaks _all_ Rx extension methods, not just the ones for which UI-framework-specific overloads exist.) But now you've got a reference to `System.Reactive` again, which seems like it would mean either bloat or the extension method resolution problem.

In fact, it is possible to fix this by playing tricks with reference assemblies. But obviously we need to test how successful any attempt to fix this is.


## Demonstrating the Problem

This folder contains a [`ExtensionMethods.DisableTransitiveWorkaroundFail`](ExtensionMethods.DisableTransitiveWorkaroundFail/) project that contains code much like that already shown, using the `ObserveOn` overload that takes a `SynchronizationContext`. It has a very straightforward configuration, but the [`CheckDisableTransitiveFailingExtensionMethod`](CheckDisableTransitiveFailingExtensionMethod/) project builds several variations on this project.

 It uses the command line argument structure common to the various checks in this repo, telling it which version of Rx.NET to test. It then builds versions of the `ExtensionMethods.DisableTransitiveWorkaroundFail` project both with Windows-specific and non-OS-specific TFMs. For the Windows-specific ones, it builds with and without `<UseWpf>` and `<UseWindowsForms>`. It also builds each variation with and without the `<DisableTransitiveReferences>` setting. It then records which, if any, of these encountered build failures.