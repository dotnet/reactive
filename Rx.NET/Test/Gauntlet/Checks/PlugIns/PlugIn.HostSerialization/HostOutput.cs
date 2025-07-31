// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace PlugIn.HostSerialization;

/// <summary>
/// The output of the plug-in host process.
/// </summary>
/// <remarks>
/// <para>
/// The plug-in hosts produce JSON output based on this type, which is then deserialized by the plug-in host driver.
/// </para>
/// </remarks>
public class HostOutput
{
    public PlugInResult FirstPlugIn { get; set; }
    public PlugInResult SecondPlugIn { get; set; }

    public class PlugInResult
    {
        public string PlugInLocation { get; set; }
     
        public string RxFullAssemblyName { get; set; }
        public string RxLocation { get; set; }
        public string RxTargetFramework { get; set; }

        public bool FlowsCancellationTokenToOperationCancelledException { get; set; }

        public bool SupportsWindowsForms { get; set; }
    }
}