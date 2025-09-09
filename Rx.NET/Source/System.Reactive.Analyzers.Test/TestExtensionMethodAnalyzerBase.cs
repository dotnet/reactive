// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Analyzers.Test.Verifiers;

using Microsoft.CodeAnalysis.Testing;

namespace System.Reactive.Analyzers.Test
{
    public class TestExtensionMethodAnalyzerBase
    {
        protected static async Task TestExtensionMethod(
            string source,
            string? targetType,
            string extensionMethodName,
            string diagnosticId,
            string diagnosticArgument,
            string? additionalArguments,
            bool annotateArgument)
        {
            // targetType is null when invoking a method that does not take a target argument.
            string target = targetType is null
                ? ""
                : "target";

            // Some diagnostics apply to the argument, and some to the method name.
            string invocation = annotateArgument
                ? $$""".{{extensionMethodName}}({|#0:{{target}}|}{{additionalArguments}})"""
                : $$""".{|#0:{{extensionMethodName}}|}({{target}}{{additionalArguments}})""";


            string? targetDeclaration = targetType is null
                ? null
                : $"{targetType} target = default!;";

            var test = $$"""
                using System;
                using System.Reactive.Linq;
                using System.Reactive.Subjects;
                
                {{targetDeclaration}}

                {{source}}
                    {{invocation}}
                    .Subscribe(Console.WriteLine);
                """;

            var expectedOriginalError = additionalArguments is null
                ? "CS1503" // single-argument overloads
                : "CS1501"; // multi-argument overloads
            var normalError = new DiagnosticResult(expectedOriginalError, Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    .WithLocation(0);
            var customDiagnostic = AddUiFrameworkPackageAnalyzerVerifier.Diagnostic(diagnosticId).WithLocation(0).WithArguments(diagnosticArgument);
            await AddUiFrameworkPackageAnalyzerVerifier.VerifyAnalyzerAsync(
                test,
                normalError,
                customDiagnostic);
        }

        protected static Task TestExtensionMethodOnIObservable(
            string targetDeclaration,
            string extensionMethodName,
            string diagnosticId)
        {
            return TestExtensionMethod(
                "Observable.Interval(TimeSpan.FromSeconds(0.5))",
                targetDeclaration,
                extensionMethodName,
                diagnosticId,
                extensionMethodName,
                additionalArguments: null,
                annotateArgument: true);
         }

        protected static Task TestExtensionMethodOnIObservable(
            string? targetType,
            string extensionMethodName,
            string diagnosticId,
            string diagnosticArgument,
            string? additionalArguments = null)
        {
            return TestExtensionMethod(
                "Observable.Interval(TimeSpan.FromSeconds(0.5))",
                targetType,
                extensionMethodName,
                diagnosticId,
                diagnosticArgument,
                additionalArguments,
                annotateArgument: additionalArguments is null);
        }

        protected static Task TestExtensionMethodOnSubject(
            string targetDeclaration,
            string extensionMethodName,
            string diagnosticId)
        {
            return TestExtensionMethod(
                "new Subject<int>()",
                targetDeclaration,
                extensionMethodName,
                diagnosticId,
                extensionMethodName,
                additionalArguments: null,
                annotateArgument: true);
        }

        protected static Task TestExtensionMethodOnSubject(
            string targetDeclaration,
            string extensionMethodName,
            string diagnosticId,
            string diagnosticArgument)
        {
            return TestExtensionMethod(
                "new Subject<int>()",
                targetDeclaration,
                extensionMethodName,
                diagnosticId,
                diagnosticArgument,
                additionalArguments: null,
                annotateArgument: true);
        }
    }
}
