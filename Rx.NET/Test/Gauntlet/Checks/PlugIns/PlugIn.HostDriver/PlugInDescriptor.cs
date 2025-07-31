// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using RxGauntlet.Build;

namespace PlugIn.HostDriver;

/// <summary>
/// Describes the build choices for a particular plug-in.
/// </summary>
/// <param name="TargetFrameworkMoniker">
/// The TFM to set in the plug-in project's <c>&lt;TargetFramework&gt;</c> property.</param>
/// <param name="RxPackages"></param>
/// <param name="PackageSource"></param>
public record PlugInDescriptor(string TargetFrameworkMoniker, PackageIdAndVersion[] RxPackages, string? PackageSource);