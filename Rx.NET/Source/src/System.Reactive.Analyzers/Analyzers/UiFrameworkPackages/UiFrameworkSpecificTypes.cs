// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace System.Reactive.Analyzers.UiFrameworkPackages
{
    internal static class UiFrameworkSpecificTypes
    {
        /// <summary>
        /// Check whether a diagnostic looks likely to have been caused by use of a
        /// UI-framework-specific type.
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
            // TODO: do we need to do anything around ThreadPoolScheduler?
            // TODO: FromEventPattern, ToEventPattern
            // Likewise WindowsObservable (FromEventPattern and ToEventPattern but also SelectMany - do these need to go into the extension methods bit?)

            static (DiagnosticDescriptor diagnostic, string type)? MatchNamespaceQualifiedType(string fullTypeName) =>
                fullTypeName switch
                {
                    "System.Reactive.Concurrency.DispatcherScheduler" =>
                        (AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule, "DispatcherScheduler"),
                    "System.Reactive.Concurrency.ControlScheduler" =>
                        (AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsFormsRequiredRule, "ControlScheduler"),
                    "System.Reactive.Concurrency.CoreDispatcherScheduler" =>
                        (AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule, "CoreDispatcherScheduler"),
                    _ => null
                };

            static (DiagnosticDescriptor diagnostic, string type)? MatchType(string typeName) =>
                MatchNamespaceQualifiedType($"System.Reactive.Concurrency.{typeName}");

            (DiagnosticDescriptor diagnostic, string type)? report = null;

            if (node?.Parent is QualifiedNameSyntax qualifiedName)
            {
                if (MatchNamespaceQualifiedType(qualifiedName.ToFullString().Trim()) is (DiagnosticDescriptor diagnostic, string type))
                {
                    report = (diagnostic, type);
                }
            }

            if (report is null)
            {
                if (node is IdentifierNameSyntax identifierName)
                {
                    if (MatchType(identifierName.Identifier.ValueText) is (DiagnosticDescriptor diagnostic, string type))
                    {
                        // If the compiler has already determined what type this name represents, then the
                        // problem can't be that the Rx DispatcherScheduler type is unavailable, so we only
                        // proceed if the semantic model doesn't understand this identifier name's type.
                        var ti = context.SemanticModel.GetTypeInfo(identifierName);
                        if (ti.Type is null || ti.Type.TypeKind == TypeKind.Error)
                        {
                            var importScopes = context.SemanticModel.GetImportScopes(diag.Location.SourceSpan.Start);
                            var namespaceInScope = importScopes.SelectMany(s => s.Imports).Any(i => i.NamespaceOrType.ToDisplayString() == "System.Reactive.Concurrency");
                            if (namespaceInScope)
                            {
                                report = (diagnostic, type);
                            }
                        }
                    }
                }
                else if (node is MemberAccessExpressionSyntax memberAccess)
                {
                    if (MatchType(memberAccess.Name.Identifier.ValueText) is (DiagnosticDescriptor diagnostic, string type))
                    {
                        // If the compiler has already determined what type this name represents, then the
                        // problem can't be that the Rx DispatcherScheduler type is unavailable, so we only
                        // proceed if the semantic model doesn't understand this identifier name's type.
                        var ti = context.SemanticModel.GetTypeInfo(memberAccess.Name);
                        if (ti.Type is null || ti.Type.TypeKind == TypeKind.Error)
                        {
                            if (memberAccess.Expression.ToFullString() == "System.Reactive.Concurrency")
                            {
                                report = (diagnostic, type);
                            }
                        }
                    }
                }
            }

            if (report is (DiagnosticDescriptor d, string t))
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    d,
                    diag.Location,
                    t,
                    "type"));
                return true;
            }

            return false;
        }
    }
}
