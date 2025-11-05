# Rx Release History v7.0

## 7.0.0

Supported on .NET 8, .NET 9, and .NET 10.0.

New features:

* Applications with a Windows-specific TFM (e.g., `net8.0-windows.10.0.19041`) can now reference the `System.Reactive` package without automatically acquiring a reference to the `Microsoft.Desktop.App` framework (which includes WPF and WinForms). If the application uses either self-contained deployment or AoT, this fixes the problem in which a reference to Rx would massively increase the size of the deployable application.


### Breaking changes

* UI-framework-specific functionality now requires referencing the relevant platform-specific package:
  * `System.Reactive.Windows.Forms` for Windows Forms
  * `System.Reactive.Wpf` for WPF
  * `System.Reactive.WindowsRuntime` for WinRT (e.g., `CoreDispatcher`) support
* If an application with a Windows-specific had been relying on `System.Reactive` to acquire the `Microsoft.Desktop.App` framework dependency, it will need to add `<UseWPF>true</UseWPF>` or `<UseWindowsForms>true</UseWindowsForms>`
* Out-of-support target frameworks (.NET 6.0, .NET 7.0) no longer supported

Note that the packaging changes for UI-specific functionality constitute a source-level breaking change, but not a binary-level breaking change. Although the UI-framework-specific types have been removed from the public API of `System.Reactive`, they remain present at runtime. (The NuGet package has both `ref` and `lib` folders. The .NET build tools use the `ref` folder at compile time, and these types have been removed only from the `ref` assemblies. At runtime the `lib` folder is used, and the full API of `System.Reactive` v6 remains available in the assemblies in `lib`. Thus existing binaries built against Rx 6.0 that find themselves using Rx 7.0 at runtime will continue to work.)

`System.Reactive` has an analyzer that detects when a project has a build error because it was using UI-specific functionality that used to be in `System.Reactive` but now lives in a new package that the project does not yet reference. The analyzer produces diagnostics telling the developer what new reference they will require.


### Deprecation of facades

Back in Rx 4.0 (the last time there was a major upheaval to the packaging), various NuGet packages that had previously been core components of Rx.NET were demoted to compatibility facades. These contained just type forwarders indicating that the various types they used to define have moved into `System.Reactive`.

These packages existed to enable code built against older versions of Rx.NET to continue to work when upgraded to Rx 4.0 or later. We have continued to build new versions of these with each subsequent version of .NET, but all that has typically changed is the exact versions in the TFMs. Nobody should be using these facades any more so there is no reason to continue to produce new ones. (And anyone who is still using the old ones can continue to do so.)

So we no longer produced new versions of these packages.

* `System.Reactive.Compatibility`
* `System.Reactive.Core`
* `System.Reactive.Experimental`
* `System.Reactive.Interfaces`
* `System.Reactive.Linq`
* `System.Reactive.PlatformServices`
* `System.Reactive.Providers`
* `System.Reactive.Runtime.Remoting`
* `System.Reactive.Windows.Threading`

Note that these package were for many years facades:

* `System.Reactive.Windows.Forms`
* `System.Reactive.WindowsRuntime`

With Rx 7, these have returned to their original roles: they are now the home of Windows Forms and WinRT support in Rx. (We have not resurrected the `System.Reactive.Windows.Threading` package, because its name is a somewhat unhelpful accident of history. WPF functionality now lives in the new `System.Reactive.Wpf` component.)