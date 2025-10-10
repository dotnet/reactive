// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Analyzers.Test.Verifiers;

using Microsoft.CodeAnalysis.Testing;

namespace System.Reactive.Analyzers.Test
{
#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable - we do in fact derive from this,
    public class TestExtensionMethodAnalyzerBase
#pragma warning restore CA1052
    {
        protected enum DiagnosticTarget
        {
            Argument,
            MethodName
        }

        protected static async Task TestExtensionMethod(
            string source,
            string? targetType,
            string extensionMethodName,
            string diagnosticId,
            string diagnosticArgument,
            string? additionalArguments,
            DiagnosticTarget diagnosticTarget,
            string? expectedOriginalError = null)
        {
            // targetType is null when invoking a method that does not take a target argument.
            string target = targetType is null
                ? ""
                : "target";

            // Some diagnostics apply to the argument, and some to the method name.
            string invocation = diagnosticTarget switch
            {
                DiagnosticTarget.Argument => $$""".{{extensionMethodName}}({|#0:{{target}}|}{{additionalArguments}})""",
                DiagnosticTarget.MethodName => $$""".{|#0:{{extensionMethodName}}|}({{target}}{{additionalArguments}})""",
                _ => throw new ArgumentOutOfRangeException(nameof(diagnosticTarget))
            };


            string? targetDeclaration = targetType is null
                ? null
                : $"{targetType} target = default!;";

            var test = $$"""
                using System;
                using System.Reactive.Linq;
                
                {{targetDeclaration}}

                {{source}}
                    {{invocation}}
                    .Subscribe(Console.WriteLine);
                """;

            expectedOriginalError ??= additionalArguments is null
                ? "CS1503" // single-argument overloads
                : "CS1501"; // multi-argument overloads
            var normalError = new DiagnosticResult(expectedOriginalError, Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    .WithLocation(0);
            var customDiagnostic = AddUiFrameworkPackageAnalyzerVerifier.Diagnostic(diagnosticId)
                .WithLocation(0)
                .WithArguments(diagnosticArgument, "extension method");
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
                diagnosticTarget: DiagnosticTarget.Argument);
         }

        protected static Task TestExtensionMethodOnIObservable(
            string? targetType,
            string extensionMethodName,
            string diagnosticId,
            string diagnosticArgument,
            string? additionalArguments = null,
            string? expectedOriginalError = null,
            DiagnosticTarget? diagnosticTarget = null)
        {
            return TestExtensionMethod(
                "Observable.Interval(TimeSpan.FromSeconds(0.5))",
                targetType,
                extensionMethodName,
                diagnosticId,
                diagnosticArgument,
                additionalArguments,
                diagnosticTarget: diagnosticTarget ?? (additionalArguments is null ? DiagnosticTarget.Argument : DiagnosticTarget.MethodName),
                expectedOriginalError: expectedOriginalError);
        }
        protected static Task TestExtensionMethodOnIObservableNoArguments(
            string? targetType,
            string extensionMethodName,
            string diagnosticId)
        {
            return TestExtensionMethod(
                "Observable.Interval(TimeSpan.FromSeconds(0.5))",
                targetType,
                extensionMethodName,
                diagnosticId,
                $"{extensionMethodName}()",
                additionalArguments: null,
                expectedOriginalError: "CS1061",
                diagnosticTarget: DiagnosticTarget.MethodName);
        }

        protected static Task TestExtensionMethodOnSubject(
            string targetDeclaration,
            string extensionMethodName,
            string diagnosticId)
        {
            return TestExtensionMethod(
                "new System.Reactive.Subjects.Subject<int>()",
                targetDeclaration,
                extensionMethodName,
                diagnosticId,
                extensionMethodName,
                additionalArguments: null,
                diagnosticTarget: DiagnosticTarget.Argument);
        }

        protected static Task TestExtensionMethodOnSubject(
            string targetDeclaration,
            string extensionMethodName,
            string diagnosticId,
            string diagnosticArgument)
        {
            return TestExtensionMethod(
                "new System.Reactive.Subjects.Subject<int>()",
                targetDeclaration,
                extensionMethodName,
                diagnosticId,
                diagnosticArgument,
                additionalArguments: null,
                diagnosticTarget: DiagnosticTarget.Argument);
        }
    }
}
