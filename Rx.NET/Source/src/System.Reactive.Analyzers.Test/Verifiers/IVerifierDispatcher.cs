// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.CodeAnalysis.Testing;

namespace System.Reactive.Analyzers.Test.Verifiers
{
    public interface IVerifierDispatcher
    {
        DiagnosticResult Diagnostic(string diagnosticId);
        Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected);
    }
}
