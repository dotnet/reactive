// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

namespace System.Reactive.Analyzers.Test.Verifiers
{
#pragma warning disable CA1812 // CA1812 wants this to be a static class, but since it has a base class, it can't be.
    public sealed class AddUiFrameworkPackageAnalyzerVerifier<TTest> :
        CSharpAnalyzerVerifier<AddUiFrameworkPackageAnalyzer, TTest, DefaultVerifier>
        where TTest : CSharpAnalyzerTest<AddUiFrameworkPackageAnalyzer, DefaultVerifier>, new()
#pragma warning restore CA1812
    {
#pragma warning disable CA1034 // Nested types should not be visible
        public sealed class VerifierDispatcher : IVerifierDispatcher
#pragma warning restore CA1034 // Nested types should not be visible
        {
            public DiagnosticResult Diagnostic(string diagnosticId)
                => AddUiFrameworkPackageAnalyzerVerifier<TTest>.Diagnostic(diagnosticId);

            public Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
                => AddUiFrameworkPackageAnalyzerVerifier<TTest>.VerifyAnalyzerAsync(source, expected);
        }
    }
}
