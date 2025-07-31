// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace CheckTransitiveFrameworkReference;

internal record RxPackagingChoice(
    SystemReactiveRole SystemReactiveRole,
    bool UiTypesVisibleInSystemReactive,
    bool SystemReactiveSuppliesDesktopFrameworkReference);


internal enum SystemReactiveRole
{
    MainRxComponent,
    LegacyFacade,
}