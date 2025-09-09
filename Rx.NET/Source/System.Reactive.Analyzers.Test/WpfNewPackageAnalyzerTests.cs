// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Analyzers.Test.Verifiers;

using Microsoft.CodeAnalysis.Testing;

namespace System.Reactive.Analyzers.Test
{
    [TestClass]
    public sealed class WpfNewPackageAnalyzerTests : TestExtensionMethodAnalyzerBase
    {
        // TODO: additional overloads:
        // public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, DispatcherScheduler scheduler)
        // public static IObservable<TSource> ObserveOnDispatcher<TSource>(this IObservable<TSource> source)
        // public static IObservable<TSource> ObserveOnDispatcher<TSource>(this IObservable<TSource> source, DispatcherPriority priority)
        // public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, DispatcherScheduler scheduler)
        // public static IObservable<TSource> SubscribeOnDispatcher<TSource>(this IObservable<TSource> source)
        // public static IObservable<TSource> SubscribeOnDispatcher<TSource>(this IObservable<TSource> source, DispatcherPriority priority)

        [TestMethod]
        public async Task DetectIObservableSubscribeOnImplicitDispatcher()
        {
            await TestExtensionMethodOnIObservable(
                targetType: null,
                "SubscribeOn",
                "RXNET0002",
                "SubscribeOn()");
        }

        [TestMethod]
        public async Task DetectIObservableSubscribeOnDispatcher()
        {
            await TestExtensionMethodOnIObservable(
                "System.Windows.Threading.Dispatcher",
                "SubscribeOn",
                "RXNET0002",
                "SubscribeOn(Dispatcher)");
        }

        [TestMethod]
        public async Task DetectIObservableSubscribeOnDispatcherWithPriority()
        {
            await TestExtensionMethodOnIObservable(
                "System.Windows.Threading.Dispatcher",
                "SubscribeOn",
                "RXNET0002",
                "SubscribeOn(Dispatcher,DispatcherPriority)",
                ", System.Windows.Threading.DispatcherPriority.Background");
        }

        [TestMethod]
        public async Task DetectIObservableSubscribeOnDispatcherObject()
        {
            await TestExtensionMethodOnIObservable(
                "System.Windows.Threading.DispatcherObject",
                "SubscribeOn",
                "RXNET0002",
                "SubscribeOn(DispatcherObject)");
        }

        [TestMethod]
        public async Task DetectIObservableSubscribeOnDispatcherObjectWithPriority()
        {
            await TestExtensionMethodOnIObservable(
                "System.Windows.Threading.DispatcherObject",
                "SubscribeOn",
                "RXNET0002",
                "SubscribeOn(DispatcherObject,DispatcherPriority)",
                ", System.Windows.Threading.DispatcherPriority.Background");
        }

        /// <summary>
        /// Check that we handle SubscribeOn for types that derive from Control (and not just Control itself).
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DetectIObservableSubscribeOnButton()
        {
            await TestExtensionMethodOnIObservable(
                "System.Windows.Controls.Button",
                "SubscribeOn",
                "RXNET0002",
                "SubscribeOn(DispatcherObject)");
        }

        [TestMethod]
        public async Task DetectIObservableObserveOnDispatcher()
        {
            await TestExtensionMethodOnIObservable(
                "System.Windows.Threading.Dispatcher",
                "ObserveOn",
                "RXNET0002",
                "ObserveOn(Dispatcher)");
        }

        [TestMethod]
        public async Task DetectIObservableObserveOnDispatcherWithPriority()
        {
            await TestExtensionMethodOnIObservable(
                "System.Windows.Threading.Dispatcher",
                "ObserveOn",
                "RXNET0002",
                "ObserveOn(Dispatcher,DispatcherPriority)",
                ", System.Windows.Threading.DispatcherPriority.Background");
        }

        [TestMethod]
        public async Task DetectIObservableObserveOnDispatcherObject()
        {
            await TestExtensionMethodOnIObservable(
                "System.Windows.Threading.DispatcherObject",
                "ObserveOn",
                "RXNET0002",
                "ObserveOn(DispatcherObject)");
        }

        [TestMethod]
        public async Task DetectIObservableObserveOnDispatcherObjectWithPriority()
        {
            await TestExtensionMethodOnIObservable(
                "System.Windows.Threading.DispatcherObject",
                "ObserveOn",
                "RXNET0002",
                "ObserveOn(DispatcherObject,DispatcherPriority)",
                ", System.Windows.Threading.DispatcherPriority.Background");
        }

        /// <summary>
        /// Check that we handle ObserveOn for types that derive from Control (and not just Control itself).
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DetectIObservableObserveOnButton()
        {
            await TestExtensionMethodOnIObservable(
                "System.Windows.Controls.Button",
                "ObserveOn",
                "RXNET0002",
                "ObserveOn(DispatcherObject)");
        }

        [TestMethod]
        public async Task DetectConcreteObservableSubscribeOnDispatcher()
        {
            await TestExtensionMethodOnSubject(
                "System.Windows.Threading.Dispatcher",
                "SubscribeOn",
                "RXNET0002",
                "SubscribeOn(Dispatcher)");
        }

        [TestMethod]
        public async Task DetectConcreteObservableSubscribeOnDispatcherObject()
        {
            await TestExtensionMethodOnSubject(
                "System.Windows.Threading.DispatcherObject",
                "SubscribeOn",
                "RXNET0002",
                "SubscribeOn(DispatcherObject)");
        }

        [TestMethod]
        public async Task DetectConcreteObservableSubscribeOnButton()
        {
            await TestExtensionMethodOnSubject(
                "System.Windows.Controls.Button",
                "SubscribeOn",
                "RXNET0002",
                "SubscribeOn(DispatcherObject)");
        }

        [TestMethod]
        public async Task DetectConcreteObservableObserveOnDispatcher()
        {
            await TestExtensionMethodOnSubject(
                "System.Windows.Threading.Dispatcher",
                "ObserveOn",
                "RXNET0002",
                "ObserveOn(Dispatcher)");
        }

        [TestMethod]
        public async Task DetectConcreteObservableObserveOnDispatcherObject()
        {
            await TestExtensionMethodOnSubject(
                "System.Windows.Threading.DispatcherObject",
                "ObserveOn",
                "RXNET0002",
                "ObserveOn(DispatcherObject)");
        }

        [TestMethod]
        public async Task DetectConcreteObservableObserveOnButton()
        {
            await TestExtensionMethodOnSubject(
                "System.Windows.Controls.Button",
                "ObserveOn",
                "RXNET0002",
                "ObserveOn(DispatcherObject)");
        }
    }
}
