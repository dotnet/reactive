// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using PlugIn.Api;
using PlugIn.HostSerialization;

using System;
using System.Reflection;

namespace PlugIn.HostNetFx
{
    internal class Program
    {
#if DEBUG
        private const string Configuration = "Debug";
#else
        const string Configuration = "Release";
#endif

        [STAThread]
        private static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine("Usage: PlugIn.HostNetFx <firstPlugInDllPath> <secondPlugInDllPath>");
                return 1;
            }
            var firstPlugInPath = args[0];
            var secondPlugInPath = args[1];

            //while (!Debugger.IsAttached)
            //{
            //    Thread.Sleep(1000);
            //}

            //Debugger.Break();


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
        }

        private static HostOutput.PlugInResult? ExecutePlugIn(string plugInDllPath)
        {
            var plugin = Assembly.LoadFrom(plugInDllPath);
            
            var pluginType = plugin.GetType($"PlugInTest.PlugInEntryPoint");

            var o = plugin.CreateInstance($"PlugInTest.PlugInEntryPoint");
            if (o == null)
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
    }
}
