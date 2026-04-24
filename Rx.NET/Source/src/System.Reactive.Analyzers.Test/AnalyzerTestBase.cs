// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Analyzers.Test.Verifiers;

using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

namespace System.Reactive.Analyzers.Test
{
#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable - we do in fact derive from this,
    public class AnalyzerTestBase<TVerifier, TTest, TVerifierDispatcher>
        where TVerifier : CSharpAnalyzerVerifier<AddUiFrameworkPackageAnalyzer, TTest, DefaultVerifier>
        where TTest : CSharpAnalyzerTest<AddUiFrameworkPackageAnalyzer, DefaultVerifier>, new()
        where TVerifierDispatcher : IVerifierDispatcher, new()
#pragma warning restore CA1052
    {
        // We would use static abstract methods, but since we need to run in .NET FX (to simulate
        // how the compiler runs), we have to use this hack where we instantiate an instance of
        // a class so we can use normal interface dispatch instead.
        private static readonly TVerifierDispatcher s_verifierDispatcher = new();

        protected enum DiagnosticTarget
        {
            Argument,
            MethodName
        }

        protected static async Task TestCodeAsync(
            string code,
            string expectedInitialError,
            string diagnosticId,
            string diagnosticArgumentName,
            string diagnosticArgumentKind)
        {
            var normalError = new DiagnosticResult(expectedInitialError, Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    .WithLocation(0);
            var customDiagnostic = s_verifierDispatcher.Diagnostic(diagnosticId)
                .WithLocation(0)
                .WithArguments(diagnosticArgumentName, diagnosticArgumentKind);
            await s_verifierDispatcher.VerifyAnalyzerAsync(
                code,
                normalError,
                customDiagnostic);
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
            var target = targetType is null
                ? ""
                : "target";

            // Some diagnostics apply to the argument, and some to the method name.
            var invocation = diagnosticTarget switch
            {
                DiagnosticTarget.Argument => $$""".{{extensionMethodName}}({|#0:{{target}}|}{{additionalArguments}})""",
                DiagnosticTarget.MethodName => $$""".{|#0:{{extensionMethodName}}|}({{target}}{{additionalArguments}})""",
                _ => throw new ArgumentOutOfRangeException(nameof(diagnosticTarget))
            };


            var targetDeclaration = targetType is null
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
            await TestCodeAsync(
                test,
                expectedOriginalError,
                diagnosticId,
                diagnosticArgument,
                "extension method");
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
                $"{extensionMethodName}()",
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
