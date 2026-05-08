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
    /// <summary>
    /// Detect the use of UI-framework-specific Rx.NET types.
    /// </summary>
    /// <remarks>
    /// <para>
    /// There are two things we need to avoid: 1) false positives, 2) imposing overhead in
    /// scenarios where we won't ultimately report a diagnostic. 1 is a matter of correctness, but
    /// it is somewhat fiddly to get right, because by definition, the compiler doesn't fully
    /// understand the developer's intentions if we run. (This code only runs when the compiler has
    /// already reported a diagnostic.) As for 2, it's important to remember that we expect these
    /// diagnostics to be useful only in the short window of time between someone upgrading to
    /// Rx 7.0 or later, and the point where they've realised they need additional NuGet package
    /// references. As soon as they've fixed that, we never have any reason to report a problem.
    /// But we will run for every compiler diagnostic reported in any build using Rx. So we need
    /// to ensure that we can detect as quickly and efficiently as possible when we won't need
    /// to do anything, because otherwise, we just slow things down for everyone even after they
    /// have fixed the problem we are trying to detect.
    /// </para>
    /// <para>
    /// This analysis can't rely on the compiler's semantic analysis, because we need to detect the
    /// use of types which are unavailable. The whole point is to detect when an error has occurred
    /// because the developer is using a type that used to be in <c>System.Reactive</c>, and which
    /// has now moved to a different package. So if the semantic analysis has correctly determined
    /// the type of some symbol, then it definitely isn't for us! (There's one awkward case where
    /// the compiler thinks it has correctly identified the <c>IEventPatternSource`1</c> type,
    /// and reports that the wrong number of type arguments are present, when in fact what's
    /// happened is that the developer is trying to use <c>IEventPatternSource`2</c>, and that
    /// type has moved to <c>System.Reactive.WindowsRuntime</c>.) So we have to proceed almost
    /// entirely syntactically.
    /// </para>
    /// <para>
    /// In cases where we think we've got a match but the type name has been used without full
    /// namespace qualification (which will be the norm - developers don't often write out the full
    /// containing namespace when using at type) we will then inspect the semantic model to verify
    /// that the relevant namespace really is in scope. We do that to prevent false positives -
    /// if there turns out to be some type called <c>DispatcherScheduler</c> that is not in the
    /// <c>System.Reactive.Concurrency</c> namespace, then we don't want to report a diagnostic for
    /// that, because the developer is clearly not trying to use Rx.
    /// </para>
    /// </remarks>
    internal static class UiFrameworkSpecificTypes
    {
        private sealed record MovedTypeDetails(
            string SimpleName,
            string Namespace,
            int Arity,
            DiagnosticDescriptor DiagnosticDescriptor)
        {
            public string NameForDiagnosticPurposes { get; init; } = SimpleName;
        }

        private static readonly MovedTypeDetails[] MovedTypes =
        [
            new MovedTypeDetails("DispatcherScheduler", "System.Reactive.Concurrency", 0, AddUiFrameworkPackageAnalyzer.ReferenceToRxWpfRequiredRule),
            new MovedTypeDetails("ControlScheduler", "System.Reactive.Concurrency", 0, AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsFormsRequiredRule),
            new MovedTypeDetails("CoreDispatcherScheduler", "System.Reactive.Concurrency", 0, AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new MovedTypeDetails("WindowsObservable", "System.Reactive.Linq", 0, AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule),
            new MovedTypeDetails("IEventPatternSource", "System.Reactive", 2, AddUiFrameworkPackageAnalyzer.ReferenceToRxWindowsRuntimeRequiredRule)
            {
                NameForDiagnosticPurposes = "IEventPatternSource<TSender, TEventArgs>"
            }
        ];

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
            static MovedTypeDetails? MatchNamespaceQualifiedType(string ns, string sn, int arity) =>
                MovedTypes.FirstOrDefault(mt =>
                    (mt.Arity == arity)
                    && (ns == mt.Namespace)
                    && (sn == mt.SimpleName));

            static MovedTypeDetails? MatchTypeBySimpleName(string typeName, int arity) => MovedTypes
                .FirstOrDefault(mt => mt.Arity == arity && mt.SimpleName == typeName);

            (DiagnosticDescriptor diagnostic, string type)? report = null;

            if (node?.Parent is QualifiedNameSyntax qualifiedName)
            {
                MovedTypeDetails? mt;
                if (qualifiedName.Right is GenericNameSyntax gn)
                {
                    mt = MatchNamespaceQualifiedType(qualifiedName.Left.ToFullString(), gn.Identifier.Text, gn.Arity);
                }
                else
                {
                    mt = MatchNamespaceQualifiedType(qualifiedName.Left.ToFullString().Trim(), qualifiedName.Right.Identifier.Text, 0);
                }
                if (mt is not null)
                {
                    report = (mt.DiagnosticDescriptor, mt.NameForDiagnosticPurposes);
                }
            }

            if (report is null)
            {
                if (node is SimpleNameSyntax identifierName)
                {
                    var arity = identifierName is GenericNameSyntax gn ? gn.Arity : 0;
                    if (MatchTypeBySimpleName(identifierName.Identifier.ValueText, arity) is MovedTypeDetails mt)
                    {
                        // If the compiler has already determined what type this name represents, then the
                        // problem can't be that the Rx DispatcherScheduler type is unavailable, so we only
                        // proceed if the semantic model doesn't understand this identifier name's type.
                        // Except, there's a special case: when the code wants to use
                        // IEventSource<TSender, TEventArgs>, the semantic model incorrectly thinks that
                        // it has identified the type - it thinks it is IEventSource<TEventArgs> and that
                        // the code simply has the wrong number of type arguments.
                        var ti = context.SemanticModel.GetTypeInfo(identifierName);
                        if (ti.Type is null || ti.Type.TypeKind == TypeKind.Error ||
                            (ti.Type is INamedTypeSymbol nts && nts.IsGenericType && mt.Arity > 0 && nts.Arity != mt.Arity))
                        {
                            var importScopes = context.SemanticModel.GetImportScopes(diag.Location.SourceSpan.Start);
                            var namespaceInScope = importScopes.SelectMany(s => s.Imports).Any(i => i.NamespaceOrType.ToDisplayString() == mt.Namespace);
                            if (namespaceInScope)
                            {
                                report = (mt.DiagnosticDescriptor, mt.NameForDiagnosticPurposes);
                            }
                        }
                    }
                }
                else if (node is MemberAccessExpressionSyntax memberAccess)
                {
                    if (MatchTypeBySimpleName(memberAccess.Name.Identifier.ValueText, 0) is MovedTypeDetails mt)
                    {
                        // If the compiler has already determined what type this name represents, then the
                        // problem can't be that the Rx DispatcherScheduler type is unavailable, so we only
                        // proceed if the semantic model doesn't understand this identifier name's type.
                        // (Unlike earlier, we don't worry about the IEventSource<TSender, TEventArgs>
                        // case here, because that type is very unlikely to appear in a member access expression.)
                        var ti = context.SemanticModel.GetTypeInfo(memberAccess.Name);
                        if (ti.Type is null || ti.Type.TypeKind == TypeKind.Error)
                        {
                            if (memberAccess.Expression.ToFullString() == mt.Namespace)
                            {
                                report = (mt.DiagnosticDescriptor, mt.NameForDiagnosticPurposes);
                            }
                        }
                    }
                }
            }

            if (report is (DiagnosticDescriptor d, string t))
            {
                context.ReportDiagnostic(Diagnostic.Create(d, diag.Location, t, "type"));
                return true;
            }

            return false;
        }
    }
}
