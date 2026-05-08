// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Analyzers.Test
{
    /// <summary>
    /// Verify that the analyzer correctly reports when a problem is caused by code that was
    /// relying on Rx types specific to Windows Runtime now needing a reference to
    /// System.Reactive.WindowsRuntime because of an upgrade to Rx 7.
    /// </summary>
    [TestClass]
    public sealed class WindowsRuntimeTypesNewPackageAnalyzerTest : AnalyzerTestNetFxBase
    {
        [TestMethod]
        public async Task IEventPatternSourceArity2FullyQualifiedDeclaration()
        {
            await TestAsync($$"""
                System.Reactive.{|#0:IEventPatternSource<Windows.UI.Xaml.Controls.Button, System.EventArgs>|} source = default;
                """,
                "IEventPatternSource<TSender, TEventArgs>",
                "CS0305");
        }

        [TestMethod]
        public async Task IEventPatternSourceArity2VarFullyQualifiedInitializer()
        {
            await TestAsync($$"""
                var source = default(System.Reactive.{|#0:IEventPatternSource<Windows.UI.Xaml.Controls.Button, System.EventArgs>|});
                """,
                "IEventPatternSource<TSender, TEventArgs>",
                "CS0305");
        }

        [TestMethod]
        public async Task IEventPatternSourceArity2DeclarationWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive;

                {|#0:IEventPatternSource<Windows.UI.Xaml.Controls.Button, System.EventArgs>|} source = default;
                """,
                "IEventPatternSource<TSender, TEventArgs>",
                "CS0305");
        }

        [TestMethod]
        public async Task IEventPatternSourceArity2VarWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive;

                var source = default({|#0:IEventPatternSource<Windows.UI.Xaml.Controls.Button, System.EventArgs>|});
                """,
                "IEventPatternSource<TSender, TEventArgs>",
                "CS0305");
        }

        [TestMethod]
        public async Task IEventPatternSourceArity2VarWithPartialUsingInNestedNamespace()
        {
            await TestAsync($$"""
                namespace System
                {
                    using Reactive;

                    public static class Program
                    {
                        public static void Main()
                        {
                            var scheduler = default({|#0:IEventPatternSource<global::Windows.UI.Xaml.Controls.Button, System.EventArgs>|});
                        }
                    }
                }
                """,
                "IEventPatternSource<TSender, TEventArgs>",
                "CS0305");
        }

        [TestMethod]
        public async Task IEventPatternSourceArity2DeclarationWithPartialUsingInNestedNamespace()
        {
            await TestAsync($$"""
                namespace System
                {
                    using Reactive;

                    public static class Program
                    {
                        public static void Main()
                        {
                            {|#0:IEventPatternSource<global::Windows.UI.Xaml.Controls.Button, System.EventArgs>|} scheduler = default;
                        }
                    }
                }
                """,
                "IEventPatternSource<TSender, TEventArgs>",
                "CS0305");
        }

        [TestMethod]
        public async Task WindowsObservableFromEventPatternArity2FullyQualified()
        {
            await TestAsync($$"""
                var w = Windows.UI.Core.CoreWindow.GetForCurrentThread();
                {|#0:System.Reactive.Linq.WindowsObservable|}.FromEventPattern<Windows.UI.Core.CoreWindow, Windows.UI.Core.KeyEventArgs>(h => w.KeyDown += h, h => w.KeyDown -= h);
                """,
                "WindowsObservable",
                "CS0234");
        }


        [TestMethod]
        public async Task WindowsObservableFromEventPatternArity3FullyQualified()
        {
            await TestAsync($$"""
                var p = default(Windows.UI.Xaml.Controls.Page);
                {|#0:System.Reactive.Linq.WindowsObservable|}.FromEventPattern<Windows.UI.Xaml.RoutedEventHandler, Windows.UI.Xaml.Controls.Button, Windows.UI.Xaml.RoutedEventArgs>(
                    th =>
                    {
                        void Conversion(object o, Windows.UI.Xaml.RoutedEventArgs e) => th((Windows.UI.Xaml.Controls.Button)o, e);
                        return Conversion;
                    },
                    h => p.Loaded += h, p => w.Loaded -= h);
                """,
                "WindowsObservable",
                "CS0234");
        }

        [TestMethod]
        public async Task WindowsObservableFromEventPatternArity2WithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Linq;
                using Windows.UI.Core;

                var w = CoreWindow.GetForCurrentThread();
                {|#0:WindowsObservable|}.FromEventPattern<CoreWindow, KeyEventArgs>(h => w.KeyDown += h, h => w.KeyDown -= h);
                """,
                "WindowsObservable",
                "CS0103");
        }

        [TestMethod]
        public async Task WindowsObservableFromEventPatternArity3WithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Linq;
                using Windows.UI.Xaml;
                using Windows.UI.Xaml.Controls;

                var p = default(Page);
                {|#0:WindowsObservable|}.FromEventPattern<RoutedEventHandler, Button, RoutedEventArgs>(
                th =>
                {
                    void Conversion(object o, RoutedEventArgs e) => th((Button)o, e);
                    return Conversion;
                },
                h => p.Loaded += h, h => p.Loaded -= h);
                """,
                "WindowsObservable",
                "CS0103");
        }

        [TestMethod]
        public async Task WindowsObservableFromEventPatternArity2WithPartialUsingInNestedNamespace()
        {
            await TestAsync($$"""
                namespace System.Reactive
                {
                    using Linq;
                
                    public static class Program
                    {
                        public static void Main()
                        {
                            var w = global::Windows.UI.Core.CoreWindow.GetForCurrentThread();
                            {|#0:WindowsObservable|}.FromEventPattern<global::Windows.UI.Core.CoreWindow, global::Windows.UI.Core.KeyEventArgs>(h => w.KeyDown += h, h => w.KeyDown -= h);
                        }
                    }
                }
                """,
                "WindowsObservable",
                "CS0103");
        }

        [TestMethod]
        public async Task WindowsObservableFromEventPatternArity3WithPartialUsingInNestedNamespace()
        {
            await TestAsync($$"""
                namespace System.Reactive
                {
                    using Linq;
                
                    public static class Program
                    {
                        public static void Main()
                        {
                            var p = default(global::Windows.UI.Xaml.Controls.Page);
                            {|#0:WindowsObservable|}.FromEventPattern<global::Windows.UI.Xaml.RoutedEventHandler, global::Windows.UI.Xaml.Controls.Button, global::Windows.UI.Xaml.RoutedEventArgs>(
                                th =>
                                {
                                    void Conversion(object o, global::Windows.UI.Xaml.RoutedEventArgs e) => th((global::Windows.UI.Xaml.Controls.Button)o, e);
                                    return Conversion;
                                },
                                h => p.Loaded += h, h => p.Loaded -= h);                        }
                    }
                }
                """,
                "WindowsObservable",
                "CS0103");
        }


        private static Task TestAsync(string code, string typeName, string expectedInitialError) =>
            TestCodeAsync(code, expectedInitialError, "RXNET0003", typeName, "type");
    }
}
