// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Analyzers.Test.Verifiers;

using Microsoft.CodeAnalysis.Testing;

namespace System.Reactive.Analyzers.Test
{
    /// <summary>
    /// Verify that the analyzer correctly reports when a problem is caused by code that was
    /// relying on the <c>ControlScheduler</c> extension methods supplyed by System.Reactive now needing a reference to
    /// System.Reactive.For.Wpf because of an upgrade to Rx 7.
    /// </summary>
    [TestClass]
    public sealed class WindowsFormsSchedulerNewPackageAnalyzerTests
    {
        [TestMethod]
        public async Task ExplicitNewControlSchedulerFullyQualified()
        {
            await TestAsync($$"""
                var scheduler = new System.Reactive.Concurrency.{|#0:ControlScheduler|}(default(System.Windows.Forms.Control));
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task ExplicitNewControlSchedulerWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                var scheduler = new {|#0:ControlScheduler|}(default(System.Windows.Forms.Control));
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task ExplicitNewControlSchedulerWithPartialUsingInNestedNamespace()
        {
            await TestAsync($$"""
                namespace System.Reactive
                {
                    using Concurrency;

                    public static class Program
                    {
                        public static void Main()
                        {
                            var scheduler = new {|#0:ControlScheduler|}(default(System.Windows.Forms.Control));
                        }
                    }
                }
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task ControlSchedulerVariableFullyQualified()
        {
            await TestAsync($$"""
                System.Reactive.Concurrency.{|#0:ControlScheduler|} scheduler = null;
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task ControlSchedulerVariableWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                {|#0:ControlScheduler|}? scheduler = null;
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task ControlSchedulerArgumentFullyQualified()
        {
            await TestAsync($$"""
                void Use(System.Reactive.Concurrency.{|#0:ControlScheduler|}? s) => s?.Schedule(() => { });
                Use(default);
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task ControlSchedulerArgumentWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                void Use({|#0:ControlScheduler|}? s) => s?.Schedule(() => { });
                Use(default);
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task ControlSchedulerReturnTypeFullyQualified()
        {
            await TestAsync($$"""
                System.Reactive.Concurrency.{|#0:ControlScheduler|}? Get() => default;
                _ = Get();
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task ControlSchedulerReturnTypeWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                {|#0:ControlScheduler|}? Get() => default;
                _ = Get();
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task ControlSchedulerPropertyTypeFullyQualified()
        {
            await TestAsync($$"""
                public static class Program
                {
                    public static System.Reactive.Concurrency.{|#0:ControlScheduler|}? Scheduler { get; } = default;
                    public static void Main()
                    {
                        _ = Scheduler;
                    }
                }
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task ControlSchedulerPropertyTypeWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                public static class Program
                {
                    public static {|#0:ControlScheduler|}? Scheduler { get; } = default;
                    public static void Main()
                    {
                        _ = Scheduler;
                    }
                }
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task ControlSchedulerFieldTypeFullyQualified()
        {
            await TestAsync($$"""
                public static class Program
                {
                    public static System.Reactive.Concurrency.{|#0:ControlScheduler|}? Scheduler = default;
                    public static void Main()
                    {
                        _ = Scheduler;
                    }
                }
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task ControlSchedulerFieldTypeWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                public static class Program
                {
                    public static {|#0:ControlScheduler|}? Scheduler = default;
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
            var customDiagnostic = AddUiFrameworkPackageAnalyzerVerifier.Diagnostic("RXNET0001")
                .WithLocation(0)
                .WithArguments("ControlScheduler", "type");
            await AddUiFrameworkPackageAnalyzerVerifier.VerifyAnalyzerAsync(
                code,
                normalError,
                customDiagnostic);

        }
    }
}
