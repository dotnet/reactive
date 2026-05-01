// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System.Reactive.Analyzers.Test
{
    [TestClass]
    public sealed class WindowsRuntimeExtensionsNewPackageAnalyzerTests : AnalyzerTestNetFxBase
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

        [TestMethod]
        public async Task DetectIAsyncActionToObservable()
        {
            await TestExtensionMethodOnIAsyncAction(
                null,
                "ToObservable",
                "RXNET0003",
                "ToObservable()");
        }

        [TestMethod]
        public async Task DetectIAsyncActionWithProgressToObservable()
        {
            await TestExtensionMethodOnIAsyncActionWithProgress(
                null,
                "ToObservable",
                "RXNET0003",
                "ToObservable()");
        }

        [TestMethod]
        public async Task DetectIAsyncActionWithProgressToObservableProgress()
        {
            await TestExtensionMethodOnIAsyncActionWithProgress(
                null,
                "ToObservableProgress",
                "RXNET0003",
                "ToObservableProgress()");
        }

        [TestMethod]
        public async Task DetectIAsyncActionToObservableWithProgress()
        {
            await TestExtensionMethodOnIAsyncActionWithProgress(
                null,
                "ToObservable",
                "RXNET0003",
                "ToObservable(IProgress`1)",
                "default(System.IProgress<int>)",
                expectedOriginalError: "CS0411");
        }

        [TestMethod]
        public async Task DetectIAsyncOperationToObservable()
        {
            await TestExtensionMethodOnIAsyncOperation(
                null,
                "ToObservable",
                "RXNET0003",
                "ToObservable()");
        }

        [TestMethod]
        public async Task DetectIAsyncOperationWithProgressToObservable()
        {
            await TestExtensionMethodOnIAsyncOperationWithProgress(
                null,
                "ToObservable",
                "RXNET0003",
                "ToObservable()");
        }

        [TestMethod]
        public async Task DetectIAsyncOperationWithProgressToObservableProgress()
        {
            await TestExtensionMethodOnIAsyncOperationWithProgress(
                null,
                "ToObservableProgress",
                "RXNET0003",
                "ToObservableProgress()");
        }

        [TestMethod]
        public async Task DetectIAsyncOperationToObservableWithProgress()
        {
            await TestExtensionMethodOnIAsyncOperationWithProgress(
                null,
                "ToObservable",
                "RXNET0003",
                "ToObservable(IProgress`1)",
                "default(System.IProgress<int>)",
                expectedOriginalError: "CS0411");
        }

        [TestMethod]
        public async Task DetectIAsyncOperationWithProgressToObservableMultiple()
        {
            await TestExtensionMethodOnIAsyncOperationWithProgress(
                null,
                "ToObservableMultiple",
                "RXNET0003",
                "ToObservableMultiple()");
        }

        [TestMethod]
        public async Task DetectIAsyncOperationToObservableMultipleWithProgress()
        {
            await TestExtensionMethodOnIAsyncOperationWithProgress(
                null,
                "ToObservableMultiple",
                "RXNET0003",
                "ToObservableMultiple(IProgress`1)",
                "default(System.IProgress<int>)");
        }

        private static Task TestExtensionMethodOnIAsyncAction(
                    string? targetType,
                    string extensionMethodName,
                    string diagnosticId,
                    string diagnosticArgument,
                    string? additionalArguments = null,
                    string? expectedOriginalError = null,
                    DiagnosticTarget? diagnosticTarget = null)
        {
            return TestExtensionMethod(
                "default(global::Windows.Foundation.IAsyncAction)",
                targetType,
                extensionMethodName,
                diagnosticId,
                diagnosticArgument,
                additionalArguments,
                diagnosticTarget: DiagnosticTarget.MethodName,
                expectedOriginalError: expectedOriginalError ?? "CS1061");
        }

        private static Task TestExtensionMethodOnIAsyncActionWithProgress(
                    string? targetType,
                    string extensionMethodName,
                    string diagnosticId,
                    string diagnosticArgument,
                    string? additionalArguments = null,
                    string? expectedOriginalError = null,
                    DiagnosticTarget? diagnosticTarget = null)
        {
            return TestExtensionMethod(
                "default(global::Windows.Foundation.IAsyncActionWithProgress<int>)",
                targetType,
                extensionMethodName,
                diagnosticId,
                diagnosticArgument,
                additionalArguments,
                diagnosticTarget: DiagnosticTarget.MethodName,
                expectedOriginalError: expectedOriginalError ?? "CS1061");
        }


        private static Task TestExtensionMethodOnIAsyncOperation(
                    string? targetType,
                    string extensionMethodName,
                    string diagnosticId,
                    string diagnosticArgument,
                    string? additionalArguments = null,
                    string? expectedOriginalError = null,
                    DiagnosticTarget? diagnosticTarget = null)
        {
            return TestExtensionMethod(
                "default(global::Windows.Foundation.IAsyncOperation<int>)",
                targetType,
                extensionMethodName,
                diagnosticId,
                diagnosticArgument,
                additionalArguments,
                diagnosticTarget: DiagnosticTarget.MethodName,
                expectedOriginalError: expectedOriginalError ?? "CS1061");
        }

        private static Task TestExtensionMethodOnIAsyncOperationWithProgress(
                    string? targetType,
                    string extensionMethodName,
                    string diagnosticId,
                    string diagnosticArgument,
                    string? additionalArguments = null,
                    string? expectedOriginalError = null,
                    DiagnosticTarget? diagnosticTarget = null)
        {
            return TestExtensionMethod(
                "default(global::Windows.Foundation.IAsyncOperationWithProgress<int, int>)",
                targetType,
                extensionMethodName,
                diagnosticId,
                diagnosticArgument,
                additionalArguments,
                diagnosticTarget: DiagnosticTarget.MethodName,
                expectedOriginalError: expectedOriginalError ?? "CS1061");
        }
    }
}
