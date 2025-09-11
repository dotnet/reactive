// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Analyzers.Test
{
    [TestClass]
    public sealed class WindowsFormsExtensionsNewPackageAnalyzerTests : TestExtensionMethodAnalyzerBase
    {
        [TestMethod]
        public async Task DetectIObservableSubscribeOnControl()
        {
            await TestExtensionMethodOnIObservable(
                "System.Windows.Forms.Control",
                "SubscribeOn",
                "RXNET0001");
        }

        /// <summary>
        /// Check that we handle SubscribeOn for types that derive from Control (and not just Control itself).
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DetectIObservableSubscribeOnButton()
        {
            await TestExtensionMethodOnIObservable(
                "System.Windows.Forms.Button",
                "SubscribeOn",
                "RXNET0001");
        }

        [TestMethod]
        public async Task DetectIObservableObserveOnControl()
        {
            await TestExtensionMethodOnIObservable(
                "System.Windows.Forms.Control",
                "ObserveOn",
                "RXNET0001");
        }

        /// <summary>
        /// Check that we handle ObserveOn for types that derive from Control (and not just Control itself).
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DetectIObservableObserveOnButton()
        {
            await TestExtensionMethodOnIObservable(
                "System.Windows.Forms.Button",
                "ObserveOn",
                "RXNET0001");
        }

        [TestMethod]
        public async Task DetectConcreteObservableSubscribeOnControl()
        {
            await TestExtensionMethodOnSubject(
                "System.Windows.Forms.Control",
                "SubscribeOn",
                "RXNET0001");
        }

        [TestMethod]
        public async Task DetectConcreteObservableObserveOnControl()
        {
            await TestExtensionMethodOnSubject(
                "System.Windows.Forms.Control",
                "ObserveOn",
                "RXNET0001");
        }

        [TestMethod]
        public async Task DetectConcreteObservableObserveOnButton()
        {
            await TestExtensionMethodOnSubject(
                "System.Windows.Forms.Button",
                "ObserveOn",
                "RXNET0001");
        }
    }
}
