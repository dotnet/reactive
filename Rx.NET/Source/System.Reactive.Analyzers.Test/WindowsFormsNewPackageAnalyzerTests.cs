// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Analyzers.Test.Verifiers;

using Microsoft;
using Microsoft.CodeAnalysis.Testing;

namespace System.Reactive.Analyzers.Test
{
    [TestClass]
    public sealed class WindowsFormsNewPackageAnalyzerTests
    {
        [TestMethod]
        public async Task DetectIObservableSubscribeOnControl()
        {
            var test = """
                using System;
                using System.Reactive.Linq;
                using System.Reactive.Subjects;
                
                System.Windows.Forms.Control control = default!;

                Observable.Interval(TimeSpan.FromSeconds(0.5))
                    .SubscribeOn({|#0:control|})
                    .Subscribe(Console.WriteLine);
                """;

            DiagnosticResult normalError = new DiagnosticResult("CS1503", Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    .WithLocation(0);
            var customDiagnostic = AddUiFrameworkPackageAnalyzerVerifier.Diagnostic("RXNET0001").WithLocation(0).WithArguments("SubscribeOn");
            await AddUiFrameworkPackageAnalyzerVerifier.VerifyAnalyzerAsync(
                test,
                normalError,
                customDiagnostic);
        }

        /// <summary>
        /// Check that we handle SubscribeOn for types that derive from Control (and not just Control itself).
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DetectIObservableSubscribeOnButton()
        {
            var test = """
                using System;
                using System.Reactive.Linq;
                using System.Reactive.Subjects;
                
                System.Windows.Forms.Button button = default!;

                Observable.Interval(TimeSpan.FromSeconds(0.5))
                    .SubscribeOn({|#0:button|})
                    .Subscribe(Console.WriteLine);
                """;

            DiagnosticResult normalError = new DiagnosticResult("CS1503", Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    .WithLocation(0);
            var customDiagnostic = AddUiFrameworkPackageAnalyzerVerifier.Diagnostic("RXNET0001").WithLocation(0).WithArguments("SubscribeOn");
            await AddUiFrameworkPackageAnalyzerVerifier.VerifyAnalyzerAsync(
                test,
                normalError,
                customDiagnostic);
        }


        [TestMethod]
        public async Task DetectIObservableObserveOnControl()
        {
            var test = """
                using System;
                using System.Reactive.Linq;
                using System.Reactive.Subjects;
                
                System.Windows.Forms.Control control = default!;

                Observable.Interval(TimeSpan.FromSeconds(0.5))
                    .ObserveOn({|#0:control|})
                    .Subscribe(Console.WriteLine);
                """;

            DiagnosticResult normalError = new DiagnosticResult("CS1503", Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    .WithLocation(0);
            var customDiagnostic = AddUiFrameworkPackageAnalyzerVerifier.Diagnostic("RXNET0001").WithLocation(0).WithArguments("ObserveOn");
            await AddUiFrameworkPackageAnalyzerVerifier.VerifyAnalyzerAsync(
                test,
                normalError,
                customDiagnostic);
        }

        /// <summary>
        /// Check that we handle ObserveOn for types that derive from Control (and not just Control itself).
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DetectIObservableObserveOnButton()
        {
            var test = """
                using System;
                using System.Reactive.Linq;
                using System.Reactive.Subjects;
                
                System.Windows.Forms.Button button = default!;

                Observable.Interval(TimeSpan.FromSeconds(0.5))
                    .ObserveOn({|#0:button|})
                    .Subscribe(Console.WriteLine);
                """;

            DiagnosticResult normalError = new DiagnosticResult("CS1503", Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    .WithLocation(0);
            var customDiagnostic = AddUiFrameworkPackageAnalyzerVerifier.Diagnostic("RXNET0001").WithLocation(0).WithArguments("ObserveOn");
            await AddUiFrameworkPackageAnalyzerVerifier.VerifyAnalyzerAsync(
                test,
                normalError,
                customDiagnostic);
        }

        [TestMethod]
        public async Task DetectConcreteObservableSubscribeOnControl()
        {
            var test = """
                using System;
                using System.Reactive.Linq;
                using System.Reactive.Subjects;
                
                System.Windows.Forms.Control control = default!;

                new Subject<int>()
                    .SubscribeOn({|#0:control|})
                    .Subscribe(Console.WriteLine);
                """;

            DiagnosticResult normalError = new DiagnosticResult("CS1503", Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    .WithLocation(0);
            var customDiagnostic = AddUiFrameworkPackageAnalyzerVerifier.Diagnostic("RXNET0001").WithLocation(0).WithArguments("SubscribeOn");
            await AddUiFrameworkPackageAnalyzerVerifier.VerifyAnalyzerAsync(
                test,
                normalError,
                customDiagnostic);
        }

        [TestMethod]
        public async Task DetectConcreteObservableObserveOnControl()
        {
            var test = """
                using System;
                using System.Reactive.Linq;
                using System.Reactive.Subjects;
                
                System.Windows.Forms.Control control = default!;

                new Subject<int>()
                    .ObserveOn({|#0:control|})
                    .Subscribe(Console.WriteLine);
                """;

            DiagnosticResult normalError = new DiagnosticResult("CS1503", Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    .WithLocation(0);
            var customDiagnostic = AddUiFrameworkPackageAnalyzerVerifier.Diagnostic("RXNET0001").WithLocation(0).WithArguments("ObserveOn");
            await AddUiFrameworkPackageAnalyzerVerifier.VerifyAnalyzerAsync(
                test,
                normalError,
                customDiagnostic);
        }
    }
}
