# Rx.NET Packaging Gauntlet

Repros old Rx.NET problems related to NuGet packaging design, and tests whether they've these problems exist in candidate new releases.

## Background

The way in which Rx.NET splits its functionality into packages has changed over the project's history, driven by a few factors:

* The changing landscape of supported .NET versions
* Evolution of multi-target mechanisms (e.g., [PCLs](https://learn.microsoft.com/en-us/previous-versions/dotnet/framework/cross-platform/portable-class-library), multi-target NuGet packages, .NET Standard, the [One .NET](https://learn.microsoft.com/en-us/shows/build-2020/bod106) concept introduced by [.NET 5](https://devblogs.microsoft.com/dotnet/introducing-net-5/))
* The problems that multi-target components can cause for plug-in systems such as Visual Studio extensions
* The problems Rx.NET created for itself through its attempts to fix problems
* The ambiguous significance of OS-specific TFMs such as `net8.0-windows10.0.19041`

Each of the packaging approaches tried so far has problems. Unfortunately, some of these are subtle and can't be detected by ordinary unit testing. As a result, it's easy for an attempt to fix one problem to cause a regression for problems that earlier changes fixed. For example, Rx 3.1 fixed a problem for plug-ins, but that fix created some new problems, and when Rx 4.0 fixed those new problems, it also reverted the very thing that fixed the plug-in problems. Some coincidental factors meant that Rx 4.0 didn't in fact cause a regression, but because it no longer featured the design change the ruled out the occurrence of that bug, its re-emergence was inevitable: Rx 5.0 has the exact same plug-in bug that was fixed by Rx 3.1, and that bug continues to be present in Rx 6.0.

It took several years for anyone to notice (or, at any rate, to report) this regression. This illustrates that without tests, regressions happen, so we need some automated way to ensure that we can verify that these complex kinds of issue don't recur.


## Relevant issues

The following issues are relevant to packaging problems:

* [Plug-In problems](Checks/PlugIns/) (originally reported as [issue 97])[https://github.com/dotnet/reactive/issues/97], although the title is slightly misleading)
* [Bloat in self-contained apps](Checks/Bloat/) ([issue 1745, 'Adding System.Reactive to WinUI3/MAUI app increases package size by 44MB because it includes WPF and WinForms references'](https://github.com/dotnet/reactive/issues/1745), positions this as a MAUI issue, but actually it's much broader)
* [Extension method build failures on Windows-specific TFMs](Checks/ExtensionMethods/) can occur with certain attempts to fit the bloat problem; we need to check any potential Rx packaging strategy to verify that it doesn't suffer from this problem
* Subtle [problems occuring with transitive references](Checks/TransitiveReferences/)
