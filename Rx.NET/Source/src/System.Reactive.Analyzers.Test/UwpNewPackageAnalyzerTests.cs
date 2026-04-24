// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System.Reactive.Analyzers.Test
{
    // Most of the methods moving out of System.Reactive that are available for UAP are also
    // available to any WinRT app, so the WindowsRuntime tests handle them. However, there
    // are a handful of UAP-only methods.
    [TestClass]
    public sealed class UwpNewPackageAnalyzerTests : AnalyzerTestUapBase
    {
        [TestMethod]
        public async Task DetectIObservableSubscribeOnDependencyObject()
        {
            await TestExtensionMethodOnIObservable(
                targetType: "Windows.UI.Xaml.DependencyObject",
                "SubscribeOn",
                "RXNET0003",
                "SubscribeOn(DependencyObject)",
                expectedOriginalError: "CS1503");
        }

        [TestMethod]
        public async Task DetectIObservableSubscribeOnDependencyObjectWithPriority()
        {
            await TestExtensionMethodOnIObservable(
                "Windows.UI.Xaml.DependencyObject",
                "SubscribeOn",
                "RXNET0003",
                "SubscribeOn(DependencyObject,CoreDispatcherPriority)",
                ", Windows.UI.Core.CoreDispatcherPriority.Normal");
        }

        [TestMethod]
        public async Task DetectIObservableObserveOnDependencyObject()
        {
            await TestExtensionMethodOnIObservable(
                "Windows.UI.Xaml.DependencyObject",
                "ObserveOn",
                "RXNET0003",
                "ObserveOn(DependencyObject)");
        }

        // The following tests are also in WindowsRuntimeNewPackageAnalyzerTests,
        // but we need to test against both UAP and modern .NET WinRT. Since this
        // test class derives from AnalyzerTestUapBase, the compilation will be
        // set up with the UAP reference assemblies.

        [TestMethod]
        public async Task DetectIObservableSubscribeOnImplicitDispatcherWithPriority()
        {
            await TestExtensionMethodOnIObservable(
                targetType: null,
                "SubscribeOnDispatcher",
                "RXNET0003",
                "SubscribeOnDispatcher(CoreDispatcherPriority)",
                "Windows.UI.Core.CoreDispatcherPriority.Normal",
                expectedOriginalError: "CS1061",
                diagnosticTarget: DiagnosticTarget.MethodName);
        }

        [TestMethod]
        public async Task DetectIObservableSubscribeOnImplicitCoreDispatcher()
        {
            await TestExtensionMethodOnIObservableNoArguments(
                targetType: null,
                "SubscribeOnCoreDispatcher",
                "RXNET0003");
        }

        [TestMethod]
        public async Task DetectIObservableSubscribeOnImplicitCoreDispatcherWithPriority()
        {
            await TestExtensionMethodOnIObservable(
                targetType: null,
                "SubscribeOnCoreDispatcher",
                "RXNET0003",
                "SubscribeOnCoreDispatcher(CoreDispatcherPriority)",
                "Windows.UI.Core.CoreDispatcherPriority.Normal",
                expectedOriginalError: "CS1061",
                diagnosticTarget: DiagnosticTarget.MethodName);
        }

        [TestMethod]
        public async Task DetectIObservableSubscribeOnCoreDispatcherImplicit()
        {
            await TestExtensionMethodOnIObservableNoArguments(
                targetType: null,
                "SubscribeOnCoreDispatcher",
                "RXNET0003");
        }

        [TestMethod]
        public async Task DetectIObservableSubscribeOnCoreDispatcherArgument()
        {
            // TODO: how do we ensure we've got a suitably recent windows version in the TFM?
            await TestExtensionMethodOnIObservable(
                "Windows.UI.Core.CoreDispatcher",
                "SubscribeOn",
                "RXNET0003",
                "SubscribeOn(CoreDispatcher)");
        }

        [TestMethod]
        public async Task DetectIObservableSubscribeOnDispatcherWithPriority()
        {
            await TestExtensionMethodOnIObservable(
                "Windows.UI.Core.CoreDispatcher",
                "SubscribeOn",
                "RXNET0003",
                "SubscribeOn(CoreDispatcher,CoreDispatcherPriority)",
                ", Windows.UI.Core.CoreDispatcherPriority.Normal");
        }

        [TestMethod]
        public async Task DetectIObservableObserveOnImplicitCoreDispatcher()
        {
            await TestExtensionMethodOnIObservableNoArguments(
                targetType: null,
                "ObserveOnCoreDispatcher",
                "RXNET0003");
        }

        [TestMethod]
        public async Task DetectIObservableObserveOnImplicitCoreDispatcherWithPriority()
        {
            await TestExtensionMethodOnIObservable(
                targetType: null,
                "ObserveOnCoreDispatcher",
                "RXNET0003",
                "ObserveOnCoreDispatcher(CoreDispatcherPriority)",
                "Windows.UI.Core.CoreDispatcherPriority.Normal",
                expectedOriginalError: "CS1061",
                diagnosticTarget: DiagnosticTarget.MethodName);
        }

        [TestMethod]
        public async Task DetectIObservableObserveOnCoreDispatcher()
        {
            // TODO: how do we ensure we've got a suitably recent windows version in the TFM?
            await TestExtensionMethodOnIObservable(
                "Windows.UI.Core.CoreDispatcher",
                "ObserveOn",
                "RXNET0003",
                "ObserveOn(CoreDispatcher)");
        }

        [TestMethod]
        public async Task DetectIObservableObserveOnDispatcherWithPriority()
        {
            await TestExtensionMethodOnIObservable(
                "Windows.UI.Core.CoreDispatcher",
                "ObserveOn",
                "RXNET0003",
                "ObserveOn(CoreDispatcher,CoreDispatcherPriority)",
                ", Windows.UI.Core.CoreDispatcherPriority.Normal");
        }
    }
}
