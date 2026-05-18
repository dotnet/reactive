// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;

namespace System.Reactive.Analyzers.Test.Verifiers
{
    /// <summary>
    /// Analyzer test base that sets up the compilation to resemble a modern .NET build with
    /// WinRT types available.
    /// </summary>
    public sealed class NetCoreTest : TestBase
    {
        public NetCoreTest()
            : base(CreateReferenceAssemblies())
        {
        }

        protected override string GetRxDllPath()
        {
            // Locate the Windows-specific .NET build of the component.
            var testProjectOutputDirectory = Path.GetDirectoryName(typeof(AddUiFrameworkPackageAnalyzerVerifier<>).Assembly.Location);
            var configuration = Path.GetFileName(Path.GetDirectoryName(testProjectOutputDirectory));
            var rxRefUapFolder = Path.GetFullPath(Path.Combine(
                testProjectOutputDirectory,
                $"../../../../System.Reactive.MakeRefAssemblies/bin/{configuration}/net8.0-windows10.0.19041"));
            return Path.Combine(
                rxRefUapFolder,
                "System.Reactive.dll");
        }


        private static ReferenceAssemblies CreateReferenceAssemblies()
        {
            // Make the winrt refs available.
            return ReferenceAssemblies.Net.Net80Windows.AddPackages([
                new PackageIdentity("Microsoft.WindowsDesktop.App.Ref", "8.0.0"),
                new PackageIdentity("Microsoft.Windows.SDK.NET.Ref", "10.0.19041.57")]);
        }
    }
}
