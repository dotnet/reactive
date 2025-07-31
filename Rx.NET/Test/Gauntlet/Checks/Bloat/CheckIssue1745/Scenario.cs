// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using RxGauntlet.Build;

namespace CheckIssue1745;

internal record Scenario(
    string BaseNetTfm,
    string WindowsVersion,
    bool? UseWpf,
    bool? UseWindowsForms,
    bool EmitDisableTransitiveFrameworkReferences,
    PackageIdAndVersion RxMainPackage,
    string? PackageSource);