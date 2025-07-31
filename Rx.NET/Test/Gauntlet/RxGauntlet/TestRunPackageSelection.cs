// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using RxGauntlet.Build;

namespace RxGauntlet;

/// <summary>
/// An Rx.NET package selection for a test run.
/// </summary>
/// <param name="MainRxPackage">
/// A package containing code that an application making ordinary use of Rx would reference when using the version of
/// Rx.NET represented by this package selection. For Rx 4-6 this would be <c>System.Reactive</c>. For older
/// versions, applications would typically reference the <c>Rx-Main</c> metapackage, but to keep the code in this
/// project simpler, we just specify <c>System.Reactive.Linq</c> for older versions, since that has the desired effect
/// without Gauntlet needing any code to understand metapackages. In newer versions, depending on the packaging design
/// choice, this might be <c>System.Reactive.Net</c>.
/// </param>
/// <param name="LegacyRxPackage">
/// In candidate future packaging designs which introduces a new main package, this will be non-null (and it will refer
/// to <c>System.Reactive</c>, with the appropriate version information to identify the version being tested). In
/// cases where <c>System.Reactive</c> has not been relegated to being a legacy facade, this will be null.
/// </param>
/// <param name="RxUiPackages">
/// The list of UI-framework-specific packages. For Rx 6 and older this will be empty, because from 4-6, the
/// Great Unification means there are no separate UI packages (and currently nothing in Rx Gauntlet requires
/// UI packages in much older versions, because the UI package issues are really all around .NET, not .NET FX).
/// So this is typically set only for future packaging design candidates.
/// </param>
/// <param name="CustomPackageSource">
/// The URL or local file path of a custom NuGet package source, or <c>null</c> when the public NuGet feed should be
/// used.
/// </param>
internal record TestRunPackageSelection(
    PackageIdAndVersion MainRxPackage,
    PackageIdAndVersion? LegacyRxPackage,
    PackageIdAndVersion[] RxUiPackages,
    string? CustomPackageSource);
