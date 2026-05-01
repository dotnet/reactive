// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Analyzers.Test
{
    /// <summary>
    /// Verify that the analyzer correctly reports when a problem is caused by code that was
    /// relying on the <c>CoreDispatcherScheduler</c> extension methods supplyed by System.Reactive now needing a reference to
    /// System.Reactive.Wpf because of an upgrade to Rx 7.
    /// </summary>
    [TestClass]
    public sealed class WindowsRuntimeSchedulerNewPackageAnalyzerTest : AnalyzerTestNetFxBase
    {
        [TestMethod]
        public async Task ExplicitNewCoreDispatcherSchedulerFullyQualified()
        {
            await TestAsync($$"""
                var scheduler = new System.Reactive.Concurrency.{|#0:CoreDispatcherScheduler|}(default(Windows.UI.Core.CoreDispatcher));
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task ExplicitNewCoreDispatcherSchedulerWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                var scheduler = new {|#0:CoreDispatcherScheduler|}(default(Windows.UI.Core.CoreDispatcher));
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task ExplicitNewCoreDispatcherSchedulerWithPartialUsingInNestedNamespace()
        {
            await TestAsync($$"""
                namespace System.Reactive
                {
                    using Concurrency;

                    public static class Program
                    {
                        public static void Main()
                        {
                            var scheduler = new {|#0:CoreDispatcherScheduler|}(default(global::Windows.UI.Core.CoreDispatcher));
                        }
                    }
                }
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task CoreDispatcherSchedulerCurrentFullyQualified()
        {
            await TestAsync($$"""
                var scheduler = {|#0:System.Reactive.Concurrency.CoreDispatcherScheduler|}.Current;
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task CoreDispatcherSchedulerCurrentSchedulerWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                var scheduler = {|#0:CoreDispatcherScheduler|}.Current;
                """,
                "CS0103");
        }

        [TestMethod]
        public async Task CoreDispatcherSchedulerCurrentSchedulerWithPartialUsingInNestedNamespace()
        {
            await TestAsync($$"""
                namespace System.Reactive
                {
                    using Concurrency;

                    public static class Program
                    {
                        public static void Main()
                        {
                            var scheduler = {|#0:CoreDispatcherScheduler|}.Current;
                        }
                    }
                }
                """,
                "CS0103");
        }

        [TestMethod]
        public async Task CoreDispatcherSchedulerVariableFullyQualified()
        {
            await TestAsync($$"""
                System.Reactive.Concurrency.{|#0:CoreDispatcherScheduler|} scheduler = null;
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task CoreDispatcherSchedulerVariableWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                {|#0:CoreDispatcherScheduler|}? scheduler = null;
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task CoreDispatcherSchedulerArgumentFullyQualified()
        {
            await TestAsync($$"""
                void Use(System.Reactive.Concurrency.{|#0:CoreDispatcherScheduler|}? s) => s?.Schedule(() => { });
                Use(default);
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task CoreDispatcherSchedulerArgumentWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                void Use({|#0:CoreDispatcherScheduler|}? s) => s?.Schedule(() => { });
                Use(default);
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task CoreDispatcherSchedulerReturnTypeFullyQualified()
        {
            await TestAsync($$"""
                System.Reactive.Concurrency.{|#0:CoreDispatcherScheduler|}? Get() => default;
                _ = Get();
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task CoreDispatcherSchedulerReturnTypeWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                {|#0:CoreDispatcherScheduler|}? Get() => default;
                _ = Get();
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task CoreDispatcherSchedulerPropertyTypeFullyQualified()
        {
            await TestAsync($$"""
                public static class Program
                {
                    public static System.Reactive.Concurrency.{|#0:CoreDispatcherScheduler|}? Scheduler { get; } = default;
                    public static void Main()
                    {
                        _ = Scheduler;
                    }
                }
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task CoreDispatcherSchedulerPropertyTypeWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                public static class Program
                {
                    public static {|#0:CoreDispatcherScheduler|}? Scheduler { get; } = default;
                    public static void Main()
                    {
                        _ = Scheduler;
                    }
                }
                """,
                "CS0246");
        }

        [TestMethod]
        public async Task CoreDispatcherSchedulerFieldTypeFullyQualified()
        {
            await TestAsync($$"""
                public static class Program
                {
                    public static System.Reactive.Concurrency.{|#0:CoreDispatcherScheduler|}? Scheduler = default;
                    public static void Main()
                    {
                        _ = Scheduler;
                    }
                }
                """,
                "CS0234");
        }

        [TestMethod]
        public async Task CoreDispatcherSchedulerFieldTypeWithUsing()
        {
            await TestAsync($$"""
                using System.Reactive.Concurrency;

                public static class Program
                {
                    public static {|#0:CoreDispatcherScheduler|}? Scheduler = default;
                    public static void Main()
                    {
                        _ = Scheduler;
                    }
                }
                """,
                "CS0246");
        }

        private static Task TestAsync(string code, string expectedInitialError) =>
            TestCodeAsync(code, expectedInitialError, "RXNET0003", "CoreDispatcherScheduler", "type");
    }
}
