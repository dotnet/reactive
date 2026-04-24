// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace System.Reactive.Analyzers.UiFrameworkPackages
{
    internal static class UiFrameworkSpecificExtensionMethods
    {
        private sealed record ExtensionMethodDetails(
            string Name,
            string[] ArgumentTypes,
            DiagnosticDescriptor DiagnosticDescriptor);

        private static readonly ExtensionMethodDetails[] Details =
            [
                new("ObserveOn", ["System.Windows.Forms.Control"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsFormsRequiredRule),
                new("ObserveOn", ["System.Windows.Threading.Dispatcher"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
                new("ObserveOn", ["System.Windows.Threading.Dispatcher", "System.Windows.Threading.DispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
                new("ObserveOn", ["System.Windows.Threading.DispatcherObject"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
                new("ObserveOn", ["System.Windows.Threading.DispatcherObject", "System.Windows.Threading.DispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
                new("ObserveOn", ["Windows.UI.Core.CoreDispatcher"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
                new("ObserveOn", ["Windows.UI.Core.CoreDispatcher", "Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
                new("ObserveOn", ["Windows.UI.Xaml.DependencyObject"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
                new("ObserveOnDispatcher", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
                new("ObserveOnDispatcher", ["System.Windows.Threading.DispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
                new("ObserveOnCoreDispatcher", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
                new("ObserveOnCoreDispatcher", ["Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
                new("SubscribeOn", ["System.Windows.Forms.Control"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsFormsRequiredRule),
                new("SubscribeOn", ["System.Windows.Threading.Dispatcher"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
                new("SubscribeOn", ["System.Windows.Threading.Dispatcher", "System.Windows.Threading.DispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
                new("SubscribeOn", ["System.Windows.Threading.DispatcherObject"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
                new("SubscribeOn", ["System.Windows.Threading.DispatcherObject", "System.Windows.Threading.DispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
                new("SubscribeOn", ["Windows.UI.Core.CoreDispatcher"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
                new("SubscribeOn", ["Windows.UI.Core.CoreDispatcher", "Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
                new("SubscribeOn", ["Windows.UI.Xaml.DependencyObject"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
                new("SubscribeOn", ["Windows.UI.Xaml.DependencyObject", "Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
                new("SubscribeOnDispatcher", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
                new("SubscribeOnDispatcher", ["System.Windows.Threading.DispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
                new("SubscribeOnDispatcher", ["Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
                new("SubscribeOnCoreDispatcher", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
                new("SubscribeOnCoreDispatcher", ["Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            ];

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
        /// True if the diagnostic looks likely to be due to a missing package reference.
        /// </returns>
        public static bool CheckForExtensionMethods(
            SemanticModelAnalysisContext context, SyntaxNode? node, Diagnostic diag)
        {
            // TODO: what if the diagnostic is in fact something else entirely?

            // When invoking single-argument overloads such as ObserveOn(Dispatcher), the
            // diagnostic is reported on the argument. That's because the main System.Reactive
            // assembly defines some single-argument overloads, and the compiler reports that the
            // argument supplied doesn't match the available overloads.
            if (node is ArgumentSyntax argument &&
                argument.Parent?.Parent is InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax ma } invocation)
            {
                return ReportDiagnosticIfAppropriate(context, diag, ma, [argument]);
            }

            // When invoking multi-argument overloads such as ObserveOn(Dispatcher,DispatcherPriority),
            // or no-argument overloads such as ObserveOnDispatcher(), then the diagnostic is the
            // invocation and not, as in the single-argument cases, the argument.
            // That's because System.Reactive does not define any multi-argument overloads, so the
            // compiler finds no candidates at all, at which point it reports a problem with the
            // method name itself. This will be inside a member access that is inside invocation, rather than a particular argument.
            if (node?.Parent is MemberAccessExpressionSyntax ma2
                && ma2.Parent is InvocationExpressionSyntax invocation2)
            {
                return ReportDiagnosticIfAppropriate(context, diag, ma2, invocation2.ArgumentList.Arguments);
            }
            return false;
        }

        private static bool ReportDiagnosticIfAppropriate(
            SemanticModelAnalysisContext context,
            Diagnostic diag,
            MemberAccessExpressionSyntax ma,
            IReadOnlyList<ArgumentSyntax> arguments)
        {
            var mn = ma.Name.ToFullString();
            var checkedTargetTypeIsObservable = false;
            ITypeSymbol[]? argumentTypeSymbols = null;

            foreach (var detail in Details)
            {
                if (detail.ArgumentTypes.Length != arguments.Count)
                {
                    continue;
                }

                if (detail.Name == mn)
                {
                    // We defer asking for type information for the target until we recognize a
                    // method name that we care about. This avoids performing type information
                    // lookups in cases that can't possibly be this analyzer's business.
                    if (!checkedTargetTypeIsObservable)
                    {
                        var targetType = context.SemanticModel.GetTypeInfo(ma.Expression).Type;
                        if (targetType is null)
                        {
                            return false;
                        }
                        if (!targetType.IsIObservable())
                        {
                            return false;
                        }

                        checkedTargetTypeIsObservable = true;
                    }

                    if (argumentTypeSymbols is null)
                    {
                        argumentTypeSymbols = new ITypeSymbol[arguments.Count];
                        for (int i = 0; i < arguments.Count; i++)
                        {
                            var argumentType = context.SemanticModel.GetTypeInfo(arguments[i].Expression).Type;
                            if (argumentType is null)
                            {
                                // This analyzer can only produce diagnostics when the types of
                                // all arguments are known.
                                return false;
                            }
                            argumentTypeSymbols[i] = argumentType;
                        }
                    }

                    var argumentTypesMatch = argumentTypeSymbols
                        .Zip(detail.ArgumentTypes, (actual, expected) => (actual, expected))
                        .All(pair =>
                        {
                            var expectedType = context.SemanticModel.Compilation.GetTypeByMetadataName(pair.expected);
                            return expectedType is not null &&
                                pair.actual.InheritsFromOrEquals(expectedType, false);
                        });

                    if (argumentTypesMatch)
                    {
                        var trimmedArgumentTypes = string.Join(
                            ",",
                            detail.ArgumentTypes.Select(at => at.Substring(at.LastIndexOf('.') + 1)));
                        context.ReportDiagnostic(Diagnostic.Create(
                            detail.DiagnosticDescriptor,
                            diag.Location,
                            [$"{detail.Name}({trimmedArgumentTypes})", Resources.ExtensionMethodText]));
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
