// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace PlugIn.Api;

public interface IRxPlugInApi
{
    public string GetPlugInAssemblyLocation();

    public string GetRxFullName();
    public string GetRxLocation();
    public string GetRxTargetFramework();

    public RxCancellationFlowBehaviour GetRxCancellationFlowBehaviour();

    public bool IsWindowsFormsSupportAvailable();
}
