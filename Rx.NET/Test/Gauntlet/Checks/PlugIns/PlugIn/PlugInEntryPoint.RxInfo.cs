// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using PlugIn.Api;
using System.Reflection;
using System.Reactive.Linq;

namespace PlugInTest;

// Note: this file is compiled into all of the plug-ins:
//  * the .NET 4.5 and .NET 4.6 plug-ins
//  * all versions of Rx.NET
public partial class PlugInEntryPoint : IRxPlugInApi
{
    public string GetRxFullName() => typeof(Observable).Assembly.FullName ?? throw new InvalidOperationException("Failed to find Rx assembly");
    public string GetRxLocation() => typeof(Observable).Assembly.Location;
    public string GetRxTargetFramework() => typeof(Observable).Assembly?.GetCustomAttribute<System.Runtime.Versioning.TargetFrameworkAttribute>()?.FrameworkName ?? throw new InvalidOperationException("Failed to find TargetFrameworkAttribute on Rx assembly");
}
