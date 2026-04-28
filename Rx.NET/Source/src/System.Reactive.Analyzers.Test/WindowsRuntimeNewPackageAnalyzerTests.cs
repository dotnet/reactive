// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System.Reactive.Analyzers.Test
{
    [TestClass]
    public sealed class WindowsRuntimeNewPackageAnalyzerTests : AnalyzerTestNetFxBase
    {
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
        public async Task DetectIObservableSubscribeOnCoreDispatcher()
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
