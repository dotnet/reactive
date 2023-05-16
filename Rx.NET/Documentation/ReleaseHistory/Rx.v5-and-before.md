# Rx Release History v5.0 and Older

## v5.0 changes

Rx.NET v5 is required for .NET 5 support of CSWinRT; earlier versions of Rx won't work correctly. To get the Windows API, you need to use the latest Windows SDK (19041), though you can target earlier versions of Windows. 

**Breaking changes**
In Windows, since the `net5.0-windows10.0.19041` target now supports all Windows desktop object models, on UWP `DispatcherObservable` and releated SubscribeOn/ObserveOn were renamed to be `CoreDispatcherObservable`, `SubscribeOnCoreDispatcher`, and `ObserveOnCoreDispatcher`. This reflects the OS type names. `Dispatcher` refers only to the WPF type.

### Supported Platforms
Rx 5.x supports the following platforms

- .NET 5
- .NET 5 with the Windows 10 19041 SDK (able to target earlier Windows versions)
- .NET Framework 4.7
- UWP 10.0.16299
- .NET Standard 2.0

Support for older versions of UWP 10.0 has been removed.

## v4.0 changes

Due to the [overwhelming](https://github.com/dotnet/reactive/issues/299) [pain](https://github.com/dotnet/reactive/issues/305) that fixing [#205 - Implement assembly version strategy](https://github.com/dotnet/reactive/issues/205) caused, we have refactored the libraries into a single library `System.Reactive`. To prevent breaking existing code that references the v3 libraries, we have facades with TypeForwarders to the new assembly. If you have a reference to a binary built against v3.0, then use the new `System.Reactive.Compatibility` package. 

#### Supported Platforms
Rx 4.1 supports the following platforms

- .NET Framework 4.6+
- .NET Standard 2.0+ (including .NET Core, Xamarin and others)
- UWP

Notably, Windows 8, Windows Phone 8 and legacy PCL libraries are no longer supported. 

## v3.0 breaking changes
The NuGet packages have changed their package naming in the move from v2.x.x to v3.0.0
 * ~~`Rx-Main`~~ is now [`System.Reactive`](https://www.nuget.org/packages/System.Reactive/)
 * ~~`Rx-Core`~~ is now [`System.Reactive.Core`](https://www.nuget.org/packages/System.Reactive.Core/)
 * ~~`Rx-Interfaces`~~  is now [`System.Reactive.Interfaces`](https://www.nuget.org/packages/System.Reactive.Interfaces/)
 * ~~`Rx-Linq`~~  is now [`System.Reactive.Linq`](https://www.nuget.org/packages/System.Reactive.Linq/)
 * ~~`Rx-PlatformServices`~~  is now [`System.Reactive.PlatformServices`](https://www.nuget.org/packages/System.Reactive.PlatformServices/)
 * ~~`Rx-Testing`~~  is now [`Microsoft.Reactive.Testing`](https://www.nuget.org/packages/Microsoft.Reactive.Testing/)

This brings the NuGet package naming in line with NuGet guidelines and also the dominant namespace in each package.
The strong name key has also changed, which is considered a breaking change.
However, there are no expected API changes, therefore, once you make the NuGet change, no code changes should be necessary.
