# Bloat in Self-Contained Apps

Adding a reference to Rx.NET in self-contained applications that target Windows can cause the deployment to grow by tens of megabytes. [Issue 1745](https://github.com/dotnet/reactive/issues/1745), positions this as a MAUI issue, but actually it's much broader. Even a console application can have this problem. Here's a project file that will repro the issue:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0-windows10.0.19041</TargetFramework>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <PropertyGroup>
    <SelfContained>true</SelfContained>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="System.Reactive" Version="6.0.1" />
  </ItemGroup>

</Project>
```

The `Program.cs` can be as simple as this:

```cs
Console.WriteLine("Hello, world!");
```

Build this with `dotnet publish`, and the `bin\Release\net9.0-windows10.0.19041\win-x64\publish` folder will be 194MB in size. Without Rx, it's 100MB. (Still large, because it includes a copy of .NET, and the Windows target means we also get 25MB of `Microsoft.Windows.SDK.NET.dll`. But obviously being almost twice the size is a lot worse.)

A .NET application will encounter this problem if all of the following are true:

* It has a Windows-specific TFM specifying version 10.0.19041 or later (e.g. `net6.0-windows10.0.19041`)
* It uses self-contained deployment
* It has a reference to `System.Reactive` (either v5 or v6)

## Problem Detail

The fundamental problem here is that the `System.Reactive` NuGet package is built in a way that assumes that if you have a Windows-specific TFM, you must definitely want to use the `Microsoft.Desktop.App` framework. The `nuspec` file in the Rx 6.0 packages include this (and Rx 5.0 has something similar)

```xml
    <frameworkReferences>
      <group targetFramework="net6.0" />
      <group targetFramework="net6.0-windows10.0.19041">
        <frameworkReference name="Microsoft.WindowsDesktop.App" />
      </group>
      <group targetFramework=".NETFramework4.7.2" />
      <group targetFramework=".NETStandard2.0" />
      <group targetFramework="UAP10.0.18362" />
    </frameworkReferences>
```

That `<frameworkReference name="Microsoft.WindowsDesktop.App" />` element is the culprit. It is possible to instruct the build system to ignore it (by setting `<DisableTransitiveFrameworkReferences>true</DisableTransitiveFrameworkReferences>` in your project file). Unfortunately, although that solves the bloat problem, it creates a new one: the application will still use the `System.Reactive.dll` assembly in the `lib\net6.0-windows10.0.19041` folder, and if it attempts to use certain Rx features, this results in compiler errors. See the [`ExtensionMethods`](../ExtensionMethods/) folder for more information about that problem.


## Demonstrating the Problem

This folder includes [`Bloat.ConsoleWinRtTemplate`](Bloat.ConsoleWinRtTemplate/), a simple console project that happens to use the WinRT `Windows.Networking.Connectivity` API. That's a Windows API that has nothing to do with UI. The project specifies a Windows-specific TFM to be able to use this API. It also uses Rx; it uses only non-UI-specific features. The project is configured for self-contained deployment.

The [CheckIssue1745](CheckIssue1745/) project builds several variations of the [`Bloat.ConsoleWinRtTemplate`](Bloat.ConsoleWinRtTemplate/) project. It chooses various different TFMs, and it builds the project with and without `<UseWpf>` and `<UseWindowsForms>`. And in the case where it doesn't set those, it also builds it with and without the `<DisableTransitiveReferences>` setting.

In each case it records whether the build output includes a copy of the WPF and Windows Forms frameworks. Of course, in some cases, it's not a bug for that to occur: if the application project file includes `<UseWpf>true</UseWpf>`, then that is telling the build system explicitly that we want to use WPF, in which case it has to include a reference to the `Microsoft.Desktop.App` framework, (which in turn means it will include a copy of WPF in the self-contained deployment build output, and also a copy of Windows Forms because .NET doesn't support deploying one without the other).