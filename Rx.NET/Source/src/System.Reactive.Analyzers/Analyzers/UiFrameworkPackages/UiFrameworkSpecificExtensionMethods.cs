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
            Func<ITypeSymbol, bool> TargetMatcher,
            string Name,
            string[] ArgumentTypes,
            DiagnosticDescriptor DiagnosticDescriptor);

        // All the extension methods we know to look for.
        // See comments in ReportDiagnosticIfAppropriate for why this is not a dictionary.
        private static readonly ExtensionMethodDetails[] Methods =
        [
            new(CodeAnalysisExtensions.IsIObservable, "ObserveOn", ["System.Windows.Forms.Control"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsFormsRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "ObserveOn", ["System.Windows.Threading.Dispatcher"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "ObserveOn", ["System.Windows.Threading.Dispatcher", "System.Windows.Threading.DispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "ObserveOn", ["System.Windows.Threading.DispatcherObject"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "ObserveOn", ["System.Windows.Threading.DispatcherObject", "System.Windows.Threading.DispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "ObserveOn", ["Windows.UI.Core.CoreDispatcher"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "ObserveOn", ["Windows.UI.Core.CoreDispatcher", "Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "ObserveOn", ["Windows.UI.Xaml.DependencyObject"], AddUiFrameworkPackageAnalyzer.ReferenceToRxUwpRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "ObserveOn", ["Windows.UI.Xaml.DependencyObject", "Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxUwpRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "ObserveOnDispatcher", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "ObserveOnDispatcher", ["System.Windows.Threading.DispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "ObserveOnCoreDispatcher", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "ObserveOnCoreDispatcher", ["Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "SubscribeOn", ["System.Windows.Forms.Control"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsFormsRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "SubscribeOn", ["System.Windows.Threading.Dispatcher"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "SubscribeOn", ["System.Windows.Threading.Dispatcher", "System.Windows.Threading.DispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "SubscribeOn", ["System.Windows.Threading.DispatcherObject"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "SubscribeOn", ["System.Windows.Threading.DispatcherObject", "System.Windows.Threading.DispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "SubscribeOn", ["Windows.UI.Core.CoreDispatcher"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "SubscribeOn", ["Windows.UI.Core.CoreDispatcher", "Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "SubscribeOn", ["Windows.UI.Xaml.DependencyObject"], AddUiFrameworkPackageAnalyzer.ReferenceToRxUwpRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "SubscribeOn", ["Windows.UI.Xaml.DependencyObject", "Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxUwpRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "SubscribeOnDispatcher", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "SubscribeOnDispatcher", ["System.Windows.Threading.DispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "SubscribeOnDispatcher", ["Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "SubscribeOnCoreDispatcher", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIObservable, "SubscribeOnCoreDispatcher", ["Windows.UI.Core.CoreDispatcherPriority"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),

            new(CodeAnalysisExtensions.IsIAsyncAction, "ToObservable", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIAsyncActionWithProgress, "ToObservable", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIAsyncActionWithProgress, "ToObservable", ["System.IProgress`1"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIAsyncActionWithProgress, "ToObservableProgress", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),

            new(CodeAnalysisExtensions.IsIAsyncOperation, "ToObservable", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIAsyncOperationWithProgress, "ToObservable", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIAsyncOperationWithProgress, "ToObservable", ["System.IProgress`1"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIAsyncOperationWithProgress, "ToObservableProgress", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIAsyncOperationWithProgress, "ToObservableMultiple", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new(CodeAnalysisExtensions.IsIAsyncOperationWithProgress, "ToObservableMultiple", ["System.IProgress`1"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),

            new(CodeAnalysisExtensions.IsIObservableOfEventPattern, "ToEventPattern", [], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),

            // Note: there are four methods we're choosing not to detect: the four IAsyncOp-flavoured
            // overloads of SelectMany in WindowsObservable.StandardSequenceOperators.
            // Although this would detect them:
            //  new(CodeAnalysisExtensions.IsIObservable, "SelectMany", ["System.Func`2"], AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            // it could also trigger for unrelated errors. We would need to check not just that the
            // argument is of type Func<TArg, TResult>, but also that the TResult is an
            // IAsyncOperation<T> or IAsyncOperationWithProgress<T>. We're using this table-driven
            // approach so that the same detection logic can be shared across all the extension
            // methods we detect, and right now, this scheme has no place to put the extra logic
            // that would be required to be sufficiently selective in detecting these particular
            // SelectMany overloads.
            // It would be possible to extend it of course, but it seems very unlikely that any
            // project using these particular extensions was not also using other Windows Runtime
            // Rx functionality, in which case those other uses will also produce diagnostics
            // indicating that they need to add a package reference. They only need to do that
            // once, so it's not really necessary to raise these diagnostics in every single place.
            // So for that reason we are not currently detecting these four SelectMany overloads.
            // If this turns out to be wrong, and there are projects that use these SelectMany
            // overloads but not any other Windows Runtime Rx functionality, we can revisit this.
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
            ITypeSymbol? targetType = null;
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
                    targetType ??= context.SemanticModel.GetTypeInfo(ma.Expression).Type;
                    if (targetType is null)
                    {
                        continue;
                    }
                    if (!detail.TargetMatcher(targetType))
                    {
                        continue;
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
                                continue;
                            }
                            argumentTypeSymbols[i] = argumentType;
                        }
                    }

                    var argumentTypesMatch = argumentTypeSymbols
                        .Zip(detail.ArgumentTypes, (actual, expected) => (actual, expected))
                        .All(pair =>
                        {
                            var expectedType = context.SemanticModel.Compilation.GetTypeByMetadataName(pair.expected);
                            if (expectedType is null)
                            {
                                return false;
                            }

                            if (expectedType.Arity != 0)
                            {
                                if (pair.actual is INamedTypeSymbol actual && actual.Arity == expectedType.Arity)
                                {
                                    // Handle generic types with matching arity
                                    var actualGenericDefinition = actual.ConstructedFrom;
                                    return actualGenericDefinition.InheritsFromOrEquals(expectedType, false);
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            return pair.actual.InheritsFromOrEquals(expectedType, false);
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
