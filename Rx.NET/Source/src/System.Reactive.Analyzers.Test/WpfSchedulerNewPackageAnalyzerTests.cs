// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Analyzers.Test.Verifiers;

using Microsoft.CodeAnalysis.Testing;

namespace System.Reactive.Analyzers.Test
{
    /// <summary>
    /// Verify that the analyzer correctly reports when a problem is caused by code that was
    /// relying on the <c>DispatcherScheduler</c> extension methods supplyed by System.Reactive now needing a reference to
    /// System.Reactive.Wpf because of an upgrade to Rx 7.
    /// </summary>
    [TestClass]
    public sealed class WpfSchedulerNewPackageAnalyzerTests
    {
        [TestMethod]
        public async Task ExplicitNewDispatcherSchedulerFullyQualified()
        {
            await TestAsync($$"""
                var scheduler = new System.Reactive.Concurrency.{|#0:DispatcherScheduler|}(System.Windows.Threading.Dispatcher.CurrentDispatcher);
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task ExplicitNewDispatcherSchedulerWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                var scheduler = new {|#0:DispatcherScheduler|}(System.Windows.Threading.Dispatcher.CurrentDispatcher);
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task ExplicitNewDispatcherSchedulerWithPartialUsingInNestedNamespace()
        {
            await TestAsync($$"""
                namespace System.Reactive
                {
                    using Concurrency;

                    public static class Program
                    {
                        public static void Main()
                        {
                            var scheduler = new {|#0:DispatcherScheduler|}(System.Windows.Threading.Dispatcher.CurrentDispatcher);
                        }
                    }
                }
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task DispatcherSchedulerCurrentFullyQualified()
        {
            await TestAsync($$"""
                var scheduler = {|#0:System.Reactive.Concurrency.DispatcherScheduler|}.Current;
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task DispatcherSchedulerCurrentSchedulerWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                var scheduler = {|#0:DispatcherScheduler|}.Current;
                """,
                "CS0103");
        }

        [TestMethod]
        public async Task DispatcherSchedulerCurrentSchedulerWithPartialUsingInNestedNamespace()
        {
            await TestAsync($$"""
                namespace System.Reactive
                {
                    using Concurrency;

                    public static class Program
                    {
                        public static void Main()
                        {
                            var scheduler = {|#0:DispatcherScheduler|}.Current;
                        }
                    }
                }
                """,
                "CS0103");
        }

        [TestMethod]
        public async Task DispatcherSchedulerVariableFullyQualified()
        {
            await TestAsync($$"""
                System.Reactive.Concurrency.{|#0:DispatcherScheduler|} scheduler = null;
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task DispatcherSchedulerVariableWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                {|#0:DispatcherScheduler|}? scheduler = null;
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task DispatcherSchedulerArgumentFullyQualified()
        {
            await TestAsync($$"""
                void Use(System.Reactive.Concurrency.{|#0:DispatcherScheduler|}? s) => s?.Schedule(() => { });
                Use(default);
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task DispatcherSchedulerArgumentWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                void Use({|#0:DispatcherScheduler|}? s) => s?.Schedule(() => { });
                Use(default);
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task DispatcherSchedulerReturnTypeFullyQualified()
        {
            await TestAsync($$"""
                System.Reactive.Concurrency.{|#0:DispatcherScheduler|}? Get() => default;
                _ = Get();
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task DispatcherSchedulerReturnTypeWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                {|#0:DispatcherScheduler|}? Get() => default;
                _ = Get();
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task DispatcherSchedulerPropertyTypeFullyQualified()
        {
            await TestAsync($$"""
                public static class Program
                {
                    public static System.Reactive.Concurrency.{|#0:DispatcherScheduler|}? Scheduler { get; } = default;
                    public static void Main()
                    {
                        _ = Scheduler;
                    }
                }
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task DispatcherSchedulerPropertyTypeWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                public static class Program
                {
                    public static {|#0:DispatcherScheduler|}? Scheduler { get; } = default;
                    public static void Main()
                    {
                        _ = Scheduler;
                    }
                }
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task DispatcherSchedulerFieldTypeFullyQualified()
        {
            await TestAsync($$"""
                public static class Program
                {
                    public static System.Reactive.Concurrency.{|#0:DispatcherScheduler|}? Scheduler = default;
                    public static void Main()
                    {
                        _ = Scheduler;
                    }
                }
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task DispatcherSchedulerFieldTypeWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                public static class Program
                {
                    public static {|#0:DispatcherScheduler|}? Scheduler = default;
                    public static void Main()
                    {
                        _ = Scheduler;
                    }
                }
                """,
                "CS0246");
        }

        private static async Task TestAsync(
            string code,
            string expectedInitialError)
        {
            var normalError = new DiagnosticResult(expectedInitialError, Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    .WithLocation(0);
            var customDiagnostic = AddUiFrameworkPackageAnalyzerVerifier.Diagnostic("RXNET0002")
                .WithLocation(0)
                .WithArguments("DispatcherScheduler", "type");
            await AddUiFrameworkPackageAnalyzerVerifier.VerifyAnalyzerAsync(
                code,
                normalError,
                customDiagnostic);

        }
    }
}
