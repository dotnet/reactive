// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace System.Reactive.Analyzers.UiFrameworkPackages
{
    internal static class UiFrameworkSpecificExtensionMethods
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
        public static bool CheckForExtensionMethods(
            SemanticModelAnalysisContext context, SyntaxNode? node, Diagnostic diag)
        {
            // TODO: WPF and UAP.
            // TODO: what if the diagnostic is in fact something else entirely?

            if (node is ArgumentSyntax argument &&
                argument.Parent?.Parent is InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax ma })
            {
                var matchedName = ma.Name.ToFullString() switch
                {
                    "ObserveOn" => "ObserveOn",
                    "SubscribeOn" => "SubscribeOn",
                    _ => null
                };

                if (matchedName is not null)
                {
                    var targetType = context.SemanticModel.GetTypeInfo(ma.Expression).Type;
                    if (targetType is not null && targetType.IsIObservable())
                    {
                        // We're looking at an invocation of one of the methods for which we need to provide help.

                        var argumentType = context.SemanticModel.GetTypeInfo(argument.Expression);

                        var controlType = context.SemanticModel.Compilation.GetTypeByMetadataName("System.Windows.Forms.Control");
                        //if (argumentType.Type is INamedTypeSymbol { Name: "Control", ContainingNamespace: { Name: "System.Windows.Forms" } })
                        //if (SymbolEqualityComparer.Default.Equals(argumentType.Type, controlType))
                        if (controlType is not null &&
                            argumentType.Type is ITypeSymbol argumentTypeSymbol &&
                            argumentTypeSymbol.InheritsFromOrEquals(controlType, false))
                        {
                            context.ReportDiagnostic(Diagnostic.Create(
                                AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsFormsRequiredRule,
                                diag.Location,
                                matchedName));

                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
