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

        // All the extension methods we know to look for.
        // See comments in ReportDiagnosticIfAppropriate for why this is not a dictionary.
        private static readonly ExtensionMethodDetails[] Methods =
        [
            new("ObserveOn", ["System.Windows.Forms.Control"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsFormsRequiredRule),
            new("ObserveOn", ["System.Windows.Threading.Dispatcher"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new("ObserveOn", ["System.Windows.Threading.Dispatcher", "System.Windows.Threading.DispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new("ObserveOn", ["System.Windows.Threading.DispatcherObject"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new("ObserveOn", ["System.Windows.Threading.DispatcherObject", "System.Windows.Threading.DispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new("ObserveOn", ["Windows.UI.Core.CoreDispatcher"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new("ObserveOn", ["Windows.UI.Core.CoreDispatcher", "Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new("ObserveOn", ["Windows.UI.Xaml.DependencyObject"], AddUiFrameworkPackageAnalyzer.ReferenceToRxUwpRequiredRule),
            new("ObserveOn", ["Windows.UI.Xaml.DependencyObject", "Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxUwpRequiredRule),
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
            new("SubscribeOn", ["Windows.UI.Xaml.DependencyObject"], AddUiFrameworkPackageAnalyzer.ReferenceToRxUwpRequiredRule),
            new("SubscribeOn", ["Windows.UI.Xaml.DependencyObject", "Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxUwpRequiredRule),
            new("SubscribeOnDispatcher", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new("SubscribeOnDispatcher", ["System.Windows.Threading.DispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new("SubscribeOnDispatcher", ["Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new("SubscribeOnCoreDispatcher", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new("SubscribeOnCoreDispatcher", ["Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
        ];

        /// <summary>
        /// Check whether a diagnostic looks likely to have been caused by a call to a
        /// UI-framework-specific extension method (e.g. obs.ObserveOn(control)), and if so, adds
        /// an additional diagnostic telling the developer that this method has moved, and that
        /// they need to add a new package.
        /// reference.
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
        /// True if the incoming diagnostic looks likely to be due to a missing package reference,
        /// and a new diagnostic was emitted to inform the developer.
        /// </returns>
        public static bool CheckForExtensionMethods(
            SemanticModelAnalysisContext context, SyntaxNode? node, Diagnostic diag)
        {
            // Note: we don't inspect the incoming diagnostic type to see if it's one we expect to
            // arise when there's a missing package reference. Since we only raise a diagnostic
            // when we've determined that the code looks like it's trying to call one of the
            // extension methods that we know has moved, there shouldn't be any risk of raising a
            // diagnostic inappropriately, and depending on a particular error might make the
            // analyzer more brittle.

            // Some UI-framework-specific methods are overloads of methods available in the main
            // library. (E.g. ObserveOn(Dispatcher) is a WPF-specific method, and since the main
            // library defines an ObserveOn(IScheduler) extension method (amongst others), this
            // WPF-specific method is an overload.)
            // In these cases, when code attempts to use the UI-specific method without the
            // relevant package reference, the compiler will think that the code has simply
            // provided the wrong argument type. It won't understand that the problem is actually
            // a missing library reference, and so the incoming diagnostic here will be on the
            // argument, not the invocation.
            if (node is ArgumentSyntax argument &&
                argument.Parent?.Parent is InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax ma } invocation)
            {
                return ReportDiagnosticIfAppropriate(context, diag, ma, invocation.ArgumentList.Arguments);
            }

            // Some UI-framework-specific methods are not overloads of methods in the main library.
            // For example the main library has no multi-argument overloads of ObserveOn, so with
            // a framework-specific method such as ObserveOn(Dispatcher, DispatcherPriority), the
            // compiler won't think we've passed the wrong argument type to an existing method.
            // Instead it will think that we've tried to invoke a method that doesn't exist. The
            // same is true for no-argument methods with names that are UI-framework-specific, such
            // as ObserveOnDispatcher(),
            // In these cases, the incoming diagnostic is on the invocation and not, as in the
            // overload cases, the argument.
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
            var mn = ma.Name.ToString();
            var checkedTargetTypeIsObservable = false;
            ITypeSymbol[]? argumentTypeSymbols = null;

            // Although we have enough method entries to exceed the threshold where a dictionary
            // lookup is typically faster than a linear search, it is complicated by the fact that
            // there are multiple entries with the same method name, so this loop will often match
            // and consider more than one entry. Also, this is an analyzer, so it may run on .NET FX,
            // where the threshold for dictionary vs linear search can be around 25 items, which
            // is how many we have, so this is exactly the kind of code where trying to be 'clever'
            // might actually perform worse in practice. We would need to do some careful
            // benchmarking to determine whether any proposed more complex approach actually helps
            // in practice.
            // If we are going to get clever, we would need to consider the following:
            //  1. does retrieving the method name as a string cause an avoidable allocation?
            //  2. is it worth trying to defer to creation of the arrays of ExtensionMethodDetails
            //      (e.g. should we use Lazy<T> or something similar to avoid creating these
            //      in static initialization?)
            //  3. if we do defer creation of ExtensionMethodDetails, is there some cheaper way
            //      we can do an initial check of the method name so that we only create the full
            //      array of details when it looks like we probably need it?
            // The case we would want to optimize for is the case where we don't need to emit a
            // diagnostic. And it would be acceptable to make the performance worse in cases where
            // we do emit a diagnostic if the payoff is to improve the no-diagnostic-emitted cases,
            // because we should only ever be emitting diagnostics in cases where the developer
            // needs to change their code. That's a one-time fix, and we want to avoid imposing
            // overhead after they've made the fix. But this might all be a micro-optimization
            // that doesn't actually help in practice, because we never even run this code unless
            // there are already compiler errors. Without very careful analysis and experimentation
            // we can't be sure, so for now we will keep it simple.
            foreach (var detail in Methods)
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
