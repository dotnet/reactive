// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using PlugIn.Api;
using PlugIn.HostSerialization;

using System.Reflection;
using System.Runtime.Loader;

if (args is not [string firstPlugInPath, string secondPlugInPath])
{
    Console.Error.WriteLine("Usage: PlugIn.HostDotnet <firstPlugInDllPath> <secondPlugInDllPath>");
    return 1;
}

var result1 = ExecutePlugIn(firstPlugInPath);
var result2 = ExecutePlugIn(secondPlugInPath);
if (result1 is null || result2 is null)
{
    return 1;
}

HostOutput result = new()
{
    FirstPlugIn = result1,
    SecondPlugIn = result2
};
Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result));

return 0;

static HostOutput.PlugInResult? ExecutePlugIn(string plugInDllPath)
{
    PlugInLoadContext plugInLoadContext = new(plugInDllPath);
    var plugin = plugInLoadContext.LoadFromAssemblyPath(plugInDllPath);

    if (plugin.GetType($"PlugInTest.PlugInEntryPoint") is not Type pluginType)
    {        
        Console.Error.WriteLine("PlugInTest.PlugInEntryPoint type not found.");
        return null;
    }

    if (plugin.CreateInstance($"PlugInTest.PlugInEntryPoint") is not object o)
    {
        Console.Error.WriteLine($"Failed to create instance of {pluginType.FullName}");
        return null;
    }

    if (o is not IRxPlugInApi instance)
    {
        Console.Error.WriteLine($"Plug-in does not implement {nameof(IRxPlugInApi)}");
        return null;
    }

    return new HostOutput.PlugInResult
    {
        PlugInLocation = instance.GetPlugInAssemblyLocation(),

        RxFullAssemblyName = instance.GetRxFullName(),
        RxLocation = instance.GetRxLocation(),
        RxTargetFramework = instance.GetRxTargetFramework(),

        FlowsCancellationTokenToOperationCancelledException =
            instance.GetRxCancellationFlowBehaviour() == RxCancellationFlowBehaviour.FlowedToOperationCanceledException,
        SupportsWindowsForms = instance.IsWindowsFormsSupportAvailable()
    };
}


internal class PlugInLoadContext(string plugInPath) : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver = new(plugInPath);

    protected override Assembly Load(AssemblyName assemblyName)
    {
        if (assemblyName.Name != typeof(IRxPlugInApi).Assembly.GetName().Name)
        {
            if (_resolver.ResolveAssemblyToPath(assemblyName) is string assemblyPath)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }
        }

        return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);
    }
}