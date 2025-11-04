# Rx Release History v7.0

## 7.0.0

New features:

* Applications with a Windows-specific TFM (e.g., `net8.0-windows.10.0.19041`) can now reference the `System.Reactive` package without automatically acquiring a reference to the `Microsoft.Desktop.App` framework (which includes WPF and WinForms). If the application uses either self-contained deployment or AoT, this fixes the problem in which a reference to Rx would massively increase the size of the deployable application
* Tested against .NET 10.0 (in addition to .NET 8 and .NET 9, which Rx 6 already supported)


### Breaking changes

* UI-framework-specific functionality now requires referencing the relevant platform-specific package:
  * `System.Reactive.Windows.Forms` for Windows Forms
  * `System.Reactive.Wpf` for WPF
  * `System.Reactive.WindowsRuntime` for WinRT (e.g., `CoreDispatcher`) support
* If an application with a Windows-specific had been relying on `System.Reactive` to acquire the `Microsoft.Desktop.App` framework dependency, it will need to add `<UseWPF>true</UseWPF>` or `<UseWindowsForms>true</UseWindowsForms>`
* Out-of-support target frameworks (.NET 6.0, .NET 7.0) no longer supported

Note that the packaging changes for UI-specific functionality is a source-level breaking change, but not a binary-level breaking change. Although the UI-framework-specific types have been removed from the public API of `System.Reactive`, they remain present at runtime. (The NuGet package has both `ref` and `lib` folders. The .NET build tools use the `ref` folder at compile time, and these types have been removed only from the `ref` assembly. At runtime the `lib` folder is used, and the full API of `System.Reactive` v6 remains available in the assemblies in `lib`. Thus existing binaries built against Rx 6.0 that find themselves using Rx 7.0 at runtime will continue to work.)


### Deprecation of facades

* `System.Reactive.Compatibility`
* `System.Reactive.Core`
* `System.Reactive.Experimental`
* `System.Reactive.Interfaces`
* `System.Reactive.Linq`
* `System.Reactive.PlatformServices`
* `System.Reactive.Providers`
* `System.Reactive.Runtime.Remoting`
* `System.Reactive.Windows.Threading`

