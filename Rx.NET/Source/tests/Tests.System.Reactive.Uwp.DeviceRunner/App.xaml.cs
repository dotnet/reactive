// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reflection;
using Xunit.Runners.UI;

namespace Tests.Reactive.Uwp.DeviceRunner
{
    sealed partial class App : RunnerApplication
    {
        protected override void OnInitializeRunner()
        {
            // tests can be inside the main assembly
            AddTestAssembly(GetType().GetTypeInfo().Assembly);
            // otherwise you need to ensure that the test assemblies will 
            // become part of the app bundle
            // AddTestAssembly(typeof(PortableTests).GetTypeInfo().Assembly);
        }
    }
}
