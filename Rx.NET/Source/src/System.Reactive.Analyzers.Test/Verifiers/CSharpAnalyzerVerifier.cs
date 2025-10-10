// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace System.Reactive.Analyzers.Test.Verifiers
{
    /// <summary>
    /// A C# analyzer verifier with a parameterised test type.
    /// </summary>
    /// <typeparam name="TAnalyzer"></typeparam>
    /// <typeparam name="TTest"></typeparam>
    /// <typeparam name="TVerifier"></typeparam>
    /// <remarks>
    /// Oddly, this seems to be missing from the test libraries. The only C#-specific verifier
    /// supplies its own Test class with no ability to customize it, and since it defaults to
    /// a .NET Core 3.1 build, that's not very useful!
    /// </remarks>
    internal class CSharpAnalyzerVerifier<TAnalyzer, TTest, TVerifier> :
        AnalyzerVerifier<AddUiFrameworkPackageAnalyzer, TTest, TVerifier>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TTest : CSharpAnalyzerTest<TAnalyzer, TVerifier>, new()
        where TVerifier : IVerifier, new()
    {
    }
}
