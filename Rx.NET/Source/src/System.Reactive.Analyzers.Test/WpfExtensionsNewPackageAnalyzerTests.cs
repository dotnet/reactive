// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Analyzers.Test
{
    /// <summary>
    /// Verify that the analyzer correctly reports when a problem is caused by code that was
    /// relying on WPF extension methods supplied by System.Reactive now needing a reference to
    /// System.Reactive.For.Wpf because of an upgrade to Rx 7.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This checks most of the ObserveOn and SubscribeOn overloads. But there are two it does not:
    /// </para>
    /// <code>
    /// <![CDATA[
    /// public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, DispatcherScheduler scheduler)
    /// public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, DispatcherScheduler scheduler)
    /// ]]>
    /// </code>
    /// <para>
    /// The reason we don't check these is that if your code is already working directly with the
    /// <c>DispatcherScheduler</c> then the other analyzer that looks for that will already be
    /// telling you to add a reference to the Rx WPF library, so the extension method analyzer
    /// doesn't need to chip in as well.
    /// </para>
    /// <para>
    /// There is one somewhat obscure scenario in which you can end up with an expression of type
    /// <c>DispatcherScheduler</c> without explicitly adding a reference to the Rx WPF library:
    /// if some library built against Rx 6 defines a public type with a public static property
    /// of type <c>DispatcherScheduler</c>, and your code reads that property, the resulting
    /// expression will be of type <c>DispatcherScheduler</c>. If you assign that expression
    /// into a <see langword="var"/> variable, that variable will also be of the this type.
    /// Now suppose you upgrade your code to Rx 7, you do not add a reference to the new
    /// Rx WPF library, and you do not upgrade the library that defined this property.
    /// Expressions that use this static property will continue to have the type
    /// <c>DispatcherScheduler</c>, and since that library was built against Rx 6, the IL
    /// will identify this as the <c>DispatcherScheduler</c> as defined by the <c>System.Reactive</c>
    /// assembly. This will confuse the compiler, because the reference assembly for
    /// <c>System.Reactive</c> v7 does not include this type, so attempting to use it will
    /// cause a compiler error. This can't be resolved simply by adding the Rx WPF package,
    /// because although that makes <c>DispatcherScheduler</c> available, it's defined in
    /// the <c>System.Reactive.For.Wpf</c> assembly, so the CLR will consider it to be a
    /// different type. (And we can't just define a type forwarder in <c>System.Reactive</c>,
    /// because that would mean the main Rx package would go back to imposing a dependency on
    /// WPF and Windows Forms, exactly the problem we're trying to fix.) In these cases, the
    /// ideal solution is for the problematic library to upgrade to Rx 7, but if that isn't
    /// possible, developers can work around this by replacing the <c>PackageReference</c>
    /// to <c>System.Reactive</c> with a <c>PackageDownload</c>, and then instructing the
    /// compiler to reference the runtime assembly (<c>lib\net8.0-windows10.0.19401\System.Reactive.dll</c>)
    /// instead of the reference assembly that gets used with <c>PackageReference</c>. This
    /// makes the hidden UI-framework-specific types in the runtime <c>System.Reactive.dll</c>
    /// available at compile time, enabling code to continue using this sort of property.
    /// We believe this is a very rare problem: typically libraries use <c>IScheduler</c> and
    /// not concrete scheduler types in their public APIs. Furthermore there is already an
    /// established practice of libraries defining their own versions of these UI-framework-specific
    /// schedulers to enable things to work on downlevel TFMs, so the Rx ecosystem already lives
    /// with the fact that there isn't one single well-known home for these scheduler types.
    /// Currently we don't attempt to report anything for this particular scenario, because it's
    /// too difficult to explain the workaround in an analyzer message. Also, we'd like to know
    /// if anyone runs into it in practice.
    /// </para>
    /// </remarks>
    [TestClass]
    public sealed class WpfExtensionsNewPackageAnalyzerTests : TestExtensionMethodAnalyzerBase
    {
        [TestMethod]
        public async Task DetectIObservableSubscribeOnImplicitDispatcher()
        {
            await TestExtensionMethodOnIObservableNoArguments(
                targetType: null,
                "SubscribeOnDispatcher",
                "RXNET0002");
        }

        [TestMethod]
        public async Task DetectIObservableSubscribeOnImplicitDispatcherWithPriority()
        {
            await TestExtensionMethodOnIObservable(
                targetType: null,
                "SubscribeOnDispatcher",
                "RXNET0002",
                "SubscribeOnDispatcher(DispatcherPriority)",
                "System.Windows.Threading.DispatcherPriority.Background",
                expectedOriginalError: "CS1061",
                diagnosticTarget: DiagnosticTarget.MethodName);
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
        public async Task DetectIObservableObserveOnImplicitDispatcher()
        {
            await TestExtensionMethodOnIObservableNoArguments(
                targetType: null,
                "ObserveOnDispatcher",
                "RXNET0002");
        }

        [TestMethod]
        public async Task DetectIObservableObserveOnImplicitDispatcherWithPriority()
        {
            await TestExtensionMethodOnIObservable(
                targetType: null,
                "ObserveOnDispatcher",
                "RXNET0002",
                "ObserveOnDispatcher(DispatcherPriority)",
                "System.Windows.Threading.DispatcherPriority.Background",
                expectedOriginalError: "CS1061",
                diagnosticTarget: DiagnosticTarget.MethodName);
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
