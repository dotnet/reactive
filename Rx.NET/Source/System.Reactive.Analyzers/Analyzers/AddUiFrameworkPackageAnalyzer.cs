// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reactive.Analyzers.UiFrameworkPackages;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace System.Reactive.Analyzers
{
    /// <summary>
    /// Detects when code using UI-framework-specific features of Rx.NET is failing to build due to
    /// missing references to the relevant UI-framework-specific NuGet package.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Rx 7.0 has moved all UI-frameworks-specific code out of the public API of the main
    /// System.Reactive NuGet package. For binary compatibility it has been removed only from
    /// the reference assemblies, and remains present in the runtime libraries, but the types
    /// hidden in this way are present only for binary backwards compatibility. Code wishing to
    /// continue using these types when being built against current versions of Rx needs to use
    /// their new homes: the UI-framework-specific packages such as System.Reactive.For.Wpf.
    /// </para>
    /// <para>
    /// This analyzer detects when a build failure looks likely to have been caused by code trying
    /// to use a UI-framework-specific feature that is unavailable due to a missing package
    /// reference. (This is most likely to have occurred because the project has been upgraded
    /// from an earlier version of Rx in which the relevant code was accessible through the main
    /// System.Reactive package.)
    /// </para>
    /// <para>
    /// We supply this analyzer because we anticipate that one of the most likely points of
    /// friction in the upgrade to Rx 7 is that code that was building without problems on Rx 6 or
    /// earlier now produces surprising build errors. It will not normally be obvious what the
    /// correct remedial action is, and so we provide this analyzer to point people in the right
    /// direction.
    /// </para>
    /// <para>
    /// The reason this needs to exist at all is that we need to remove the UI-framework-specific
    /// types from the System.Reactive public API to fix a long standing problem. If you used Rx 5
    /// or 6 in an application with a Windows-specific TFM, a reference (direct or transitive) to
    /// System.Reactive would force a dependency on the Microsoft.Desktop.App framework. This
    /// includes both WPF and Windows Forms, and for applications using self-contained deployment,
    /// this framework dependency would add tens of megabytes to the size of the application.
    /// </para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AddUiFrameworkPackageAnalyzer : DiagnosticAnalyzer
    {
        // TODO: VB.NET?
        public const string ReferenceToRxWindowsFormsRequiredDiagnosticId = "RXNET0001";

        private static readonly LocalizableString ReferenceToRxWindowsFormsRequiredTitle = new LocalizableResourceString(
            nameof(Resources.ReferenceToRxWindowsFormsRequiredAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString ReferenceToRxWindowsFormsRequiredExtensionMethodMessageFormat = new LocalizableResourceString(
            nameof(Resources.ReferenceToRxWindowsFormsRequiredAnalyzerExtensionMethodMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString ReferenceToRxWindowsFormsRequiredDescription = new LocalizableResourceString(
            nameof(Resources.ReferenceToRxWindowsFormsRequiredAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string PackagingCategory = "NuGet";
        internal static readonly DiagnosticDescriptor ReferenceToRxWindowsFormsRequiredRule = new(
            ReferenceToRxWindowsFormsRequiredDiagnosticId,
            ReferenceToRxWindowsFormsRequiredTitle,
            ReferenceToRxWindowsFormsRequiredExtensionMethodMessageFormat,
            PackagingCategory,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: ReferenceToRxWindowsFormsRequiredDescription,
            helpLinkUri: "https://github.com/dotnet/reactive");

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
        [
            //Rule,
            ReferenceToRxWindowsFormsRequiredRule
        ]);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            if (context is null) {  throw new ArgumentNullException(nameof(context)); }

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSemanticModelAction(AnalyzeSemanticModel);
        }

        private void AnalyzeSemanticModel(SemanticModelAnalysisContext context)
        {
            // Note: our goal is to do as little work as possible in cases where we won't produce a
            // diagnostic. We expect not to need to report anything the majority of the time, so we
            // want our impact to be minimal.
            //
            // We have registered for this callback, and not for syntax node ones, because we only
            // want to run when there are errors.
            var d = context.SemanticModel.GetDiagnostics();
            foreach (var diag in d)
            {
                if (diag.Severity != DiagnosticSeverity.Error)
                {
                    continue;
                }

                var node = diag.Location.SourceTree?.GetRoot().FindNode(diag.Location.SourceSpan);

                UiFrameworkSpecificExtensionMethods.CheckForExtensionMethods(context, node, diag);
            }
        }
    }
}
