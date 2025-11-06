// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

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
            // TODO: Windows Runtime and UAP.
            // TODO: what if the diagnostic is in fact something else entirely?

            // When invoking single-argument overloads such as ObserveOn(Dispatcher), the
            // diagnostic is reported on the argument. That's because the main System.Reactive
            // assembly defines some single-argument overloads, and the compiler reports that the
            // argument supplied doesn't match the available overloads.
            if (node is ArgumentSyntax argument &&
                argument.Parent?.Parent is InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax ma } invocation)
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
                        if (argumentType.Type is ITypeSymbol argumentTypeSymbol)
                        {
                            var controlType = context.SemanticModel.Compilation.GetTypeByMetadataName("System.Windows.Forms.Control");
                            if (controlType is not null &&
                                argumentTypeSymbol.InheritsFromOrEquals(controlType, false))
                            {
                                context.ReportDiagnostic(Diagnostic.Create(
                                    AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsFormsRequiredRule,
                                    diag.Location,
                                    matchedName));

                                return true;
                            }

                            var dispatcherType = context.SemanticModel.Compilation.GetTypeByMetadataName("System.Windows.Threading.Dispatcher");
                            if (CheckWpfDispatcherWithoutOrWithDispatcherPriority(withDispatcherPriority: false, context, diag, invocation, matchedName, argumentTypeSymbol, dispatcherType))
                            {
                                return true;
                            }

                            var dispatcherObjectType = context.SemanticModel.Compilation.GetTypeByMetadataName("System.Windows.Threading.DispatcherObject");
                            if (CheckWpfDispatcherWithoutOrWithDispatcherPriority(withDispatcherPriority: false, context, diag, invocation, matchedName, argumentTypeSymbol, dispatcherObjectType))
                            {
                                return true;
                            }

                            var coreDispatcherType = context.SemanticModel.Compilation.GetTypeByMetadataName("Windows.UI.Core.CoreDispatcher");
                            if (CheckCoreDispatcherWithoutOrWithDispatcherPriority(withDispatcherPriority: false, context, diag, invocation, matchedName, argumentTypeSymbol, coreDispatcherType))
                            {
                                return true;
                            }
                        }
                    }
                }
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
                if (invocation2.ArgumentList.Arguments.Count == 0)
                {
                    (string? matchedName, bool isWindowsRuntime) = ma2.Name.ToFullString() switch
                    {
                        "ObserveOnDispatcher" => ("ObserveOnDispatcher", false),
                        "SubscribeOnDispatcher" => ("SubscribeOnDispatcher", false),
                        "ObserveOnCoreDispatcher" => ("ObserveOnCoreDispatcher", true),
                        "SubscribeOnCoreDispatcher" => ("SubscribeOnCoreDispatcher", true),
                        _ => (null, false)
                    };

                    if (matchedName is not null)
                    {
                        var targetType = context.SemanticModel.GetTypeInfo(ma2.Expression).Type;
                        if (targetType is not null && targetType.IsIObservable())
                        {
                            context.ReportDiagnostic(Diagnostic.Create(
                                    isWindowsRuntime
                                        ? AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule
                                        : AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule,
                                    diag.Location,
                                    $"{matchedName}()",
                                    Resources.ExtensionMethodText));
                        }
                    }
                }

                // Handle the case where the only argument is a DispatcherPriority
                if (invocation2.ArgumentList.Arguments.Count == 1)
                {
                    (string? matchedName, bool isWindowsRuntime) = ma2.Name.ToFullString() switch
                    {
                        "ObserveOnDispatcher" => ("ObserveOnDispatcher", false),
                        "SubscribeOnDispatcher" => ("SubscribeOnDispatcher", false),
                        "ObserveOnCoreDispatcher" => ("ObserveOnCoreDispatcher", true),
                        "SubscribeOnCoreDispatcher" => ("SubscribeOnCoreDispatcher", true),
                        _ => (null, false)
                    };

                    if (matchedName is not null)
                    {
                        var targetType = context.SemanticModel.GetTypeInfo(ma2.Expression).Type;
                        if (targetType is not null && targetType.IsIObservable())
                        {
                            var expectedPriorityFullType = isWindowsRuntime
                                ? "Windows.UI.Core.CoreDispatcherPriority"
                                : "System.Windows.Threading.DispatcherPriority";
                            var argumentType = context.SemanticModel.GetTypeInfo(invocation2.ArgumentList.Arguments[0].Expression).Type;
                            if (argumentType is not null &&
                                argumentType.ToDisplayString() == expectedPriorityFullType)
                            {
                                var expectedPriorityType = isWindowsRuntime ? "CoreDispatcherPriority" : "DispatcherPriority";
                                context.ReportDiagnostic(Diagnostic.Create(
                                        isWindowsRuntime
                                            ? AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule
                                            : AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule,
                                        diag.Location,
                                        $"{matchedName}({expectedPriorityType})",
                                        Resources.ExtensionMethodText));
                            }
                        }
                    }
                }

                if (invocation2.ArgumentList.Arguments.Count is 1 or 2)
                {
                    var matchedName = ma2.Name.ToFullString() switch
                    {
                        "ObserveOn" => "ObserveOn",
                        "SubscribeOn" => "SubscribeOn",
                        _ => null
                    };

                    if (matchedName is not null)
                    {
                        var targetType = context.SemanticModel.GetTypeInfo(ma2.Expression).Type;
                        if (targetType is not null && targetType.IsIObservable())
                        {
                            var firstArgumentType = context.SemanticModel.GetTypeInfo(invocation2.ArgumentList.Arguments[0].Expression);
                            if (firstArgumentType.Type is ITypeSymbol firstArgumentTypeSymbol)
                            {
                                var dispatcherType = context.SemanticModel.Compilation.GetTypeByMetadataName("System.Windows.Threading.Dispatcher");
                                if (CheckWpfDispatcherWithoutOrWithDispatcherPriority(withDispatcherPriority: true, context, diag, invocation2, matchedName, firstArgumentTypeSymbol, dispatcherType))
                                {
                                    return true;
                                }

                                var dispatcherObjectType = context.SemanticModel.Compilation.GetTypeByMetadataName("System.Windows.Threading.DispatcherObject");
                                if (CheckWpfDispatcherWithoutOrWithDispatcherPriority(withDispatcherPriority: true, context, diag, invocation2, matchedName, firstArgumentTypeSymbol, dispatcherObjectType))
                                {
                                    return true;
                                }

                                var coreDispatcherType = context.SemanticModel.Compilation.GetTypeByMetadataName("Windows.UI.Core.CoreDispatcher");
                                if (CheckCoreDispatcherWithoutOrWithDispatcherPriority(withDispatcherPriority: true, context, diag, invocation2, matchedName, firstArgumentTypeSymbol, coreDispatcherType))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private static bool CheckWpfDispatcherWithoutOrWithDispatcherPriority(
            bool withDispatcherPriority,
            SemanticModelAnalysisContext context,
            Diagnostic diag,
            InvocationExpressionSyntax invocation,
            string matchedName,
            ITypeSymbol argumentTypeSymbol,
            INamedTypeSymbol? candidateArgumentType)
        {
            if (candidateArgumentType is not null &&
                argumentTypeSymbol.InheritsFromOrEquals(candidateArgumentType, false))
            {
                string? argumentTypes = null;
                if (!withDispatcherPriority && invocation.ArgumentList.Arguments.Count == 1)
                {
                    argumentTypes = candidateArgumentType.Name;
                }
                else if (withDispatcherPriority && invocation.ArgumentList.Arguments.Count == 2)
                {
                    var secondArgType = context.SemanticModel.GetTypeInfo(invocation.ArgumentList.Arguments[1].Expression).Type;
                    if (secondArgType is not null &&
                        secondArgType.ToDisplayString() == "System.Windows.Threading.DispatcherPriority")
                    {
                        argumentTypes = $"{candidateArgumentType.Name},DispatcherPriority";
                    }
                }

                if (argumentTypes is not null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                            AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule,
                            diag.Location,
                            $"{matchedName}({argumentTypes})",
                            Resources.ExtensionMethodText));
                    return true;
                }
            }

            return false;
        }

        private static bool CheckCoreDispatcherWithoutOrWithDispatcherPriority(
            bool withDispatcherPriority,
            SemanticModelAnalysisContext context,
            Diagnostic diag,
            InvocationExpressionSyntax invocation,
            string matchedName,
            ITypeSymbol argumentTypeSymbol,
            INamedTypeSymbol? candidateArgumentType)
        {
            if (candidateArgumentType is not null &&
                argumentTypeSymbol.InheritsFromOrEquals(candidateArgumentType, false))
            {
                string? argumentTypes = null;
                if (!withDispatcherPriority && invocation.ArgumentList.Arguments.Count == 1)
                {
                    argumentTypes = candidateArgumentType.Name;
                }
                else if (withDispatcherPriority && invocation.ArgumentList.Arguments.Count == 2)
                {
                    var secondArgType = context.SemanticModel.GetTypeInfo(invocation.ArgumentList.Arguments[1].Expression).Type;
                    if (secondArgType is not null &&
                        secondArgType.ToDisplayString() == "Windows.UI.Core.CoreDispatcherPriority")
                    {
                        argumentTypes = $"{candidateArgumentType.Name},CoreDispatcherPriority";
                    }
                }

                if (argumentTypes is not null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                            AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule,
                            diag.Location,
                            $"{matchedName}({argumentTypes})",
                            Resources.ExtensionMethodText));
                    return true;
                }
            }

            return false;
        }
    }
}
