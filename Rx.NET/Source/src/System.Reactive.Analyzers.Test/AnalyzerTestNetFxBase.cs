// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Analyzers.Test.Verifiers;

namespace System.Reactive.Analyzers.Test
{
    public class AnalyzerTestNetFxBase : AnalyzerTestBase<
        AddUiFrameworkPackageAnalyzerVerifier<NetCoreTest>,
        NetCoreTest,
        AddUiFrameworkPackageAnalyzerVerifier<NetCoreTest>.VerifierDispatcher>
    {
    }
}
