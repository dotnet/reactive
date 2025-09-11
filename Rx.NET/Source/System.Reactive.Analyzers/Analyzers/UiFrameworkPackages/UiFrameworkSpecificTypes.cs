// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace System.Reactive.Analyzers.UiFrameworkPackages
{
    internal static class UiFrameworkSpecificTypes
    {
        /// <summary>
        /// Check whether a diagnostic looks likely to have been caused by a call to obs.ObserveOn(control),
        /// or similar calls to UI-framework-specific extension methods.
        /// </summary>
        /// <param name="context">
        /// Analyzer context from which the diagnostic was reported.
        /// </param>
        /// <param name="node">
        /// The syntax node for which the diagnostic has been reported.
        /// </param>
        /// <param name="diag">
        /// The compile diagnostics.
        /// </param>
        /// <returns>
        /// True if the diagnostic looks likely to be due to a missing package refernce.
        /// </returns>
        public static bool Check(
            SemanticModelAnalysisContext context, SyntaxNode? node, Diagnostic diag)
        {
            if (node?.Parent is QualifiedNameSyntax qualifiedName)
            {
                (DiagnosticDescriptor diagnostic, string type)? info = qualifiedName.ToFullString().Trim() switch
                {
                    "System.Reactive.Concurrency.DispatcherScheduler" =>
                        (AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule, "DispatcherScheduler"),
                    "System.Reactive.Concurrency.ControlScheduler" =>
                        (AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsFormsRequiredRule, "ControlScheduler"),
                    _ => null
                };

                if (info is (DiagnosticDescriptor diagnostic, string type))
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        diagnostic,
                        diag.Location,
                        type,
                        "type"));

                    return true;
                }
            }

            if (node is IdentifierNameSyntax identifierName)
            {
                (DiagnosticDescriptor diagnostic, string type)? info = identifierName.Identifier.ValueText switch
                {
                    "DispatcherScheduler" =>
                        (AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule, "DispatcherScheduler"),
                    "ControlScheduler" =>
                        (AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsFormsRequiredRule, "ControlScheduler"),
                    _ => null
                };
                if (info is (DiagnosticDescriptor diagnostic, string type))
                {
                    // If the compiler has already determined what type this name represents, then the
                    // problem can't be that the Rx DispatcherScheduler type is unavailable, so we only
                    // proceed if the semantic model doesn't understand this identifier name's type.
                    var ti = context.SemanticModel.GetTypeInfo(identifierName);
                    if (ti.Type is null || ti.Type.TypeKind == TypeKind.Error)
                    {
                        var importScopes = context.SemanticModel.GetImportScopes(diag.Location.SourceSpan.Start);
                        bool namespaceInScope = importScopes.SelectMany(s => s.Imports).Any(i => i.NamespaceOrType.ToDisplayString() == "System.Reactive.Concurrency");
                        if (namespaceInScope)
                        {
                            context.ReportDiagnostic(Diagnostic.Create(
                                diagnostic,
                                diag.Location,
                                type,
                                "type"));

                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
