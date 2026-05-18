// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;

namespace System.Reactive.Analyzers.Test.Verifiers
{
    /// <summary>
    /// Analyzer test base that sets up the compilation to resemble a classic UAP project build.
    /// </summary>
    public sealed class UapTest : TestBase
    {
        public UapTest()
            : base(CreateReferenceAssemblies())
        {
        }

        protected override string GetRxDllPath()
        {
            // When testing how the analyzer works with UAP projects, need to locate the UAP
            // build of the component.
            var testProjectOutputDirectory = Path.GetDirectoryName(typeof(AddUiFrameworkPackageAnalyzerVerifier<>).Assembly.Location);
            var configuration = Path.GetFileName(Path.GetDirectoryName(testProjectOutputDirectory));
            var rxRefUapFolder = Path.GetFullPath(Path.Combine(
                testProjectOutputDirectory,
                $"../../../../System.Reactive.MakeRefAssemblies/bin/{configuration}/uap10.0.18362"));
            return Path.Combine(
                rxRefUapFolder,
                "System.Reactive.dll");
        }

        private static ReferenceAssemblies CreateReferenceAssemblies()
        {
            // A classic UAP app gets its .NET runtime library from Microsoft.NETCore.UniversalWindowsPlatform.
            var uap10 = new ReferenceAssemblies(
                    "uap10.0.18362",
                    new PackageIdentity(
                        "Microsoft.NETCore.UniversalWindowsPlatform",
                        "6.2.14"),
                    "ref\\uap10.0.15138");

            // Microsoft.Windows.SDK.NET.Ref defines the bits of the WinRT API that are available
            // to all .NET apps.
            return uap10.AddPackages(
                [
                    new PackageIdentity("Microsoft.Windows.SDK.NET.Ref", "10.0.26100.84")
                ]);
        }

        protected override Solution AdjustSolutionIfRequired(Solution solution, Project project)
        {
            // The UAP-specific parts of the WinRT API are not available in any NuGet package.
            // The only supported way to reference these APIs is to use the locally-installed
            // Windows platform SDK, which includes a Windows.winmd file.
            var platformSdkLocation = Microsoft.Build.Utilities.ToolLocationHelper
                .GetPlatformSDKLocation("Windows", "10.0");
            var uapMdPath = Path.Combine(platformSdkLocation, "UnionMetadata\\10.0.26100.0\\Windows.winmd");
            return solution.AddMetadataReference(
                project.Id,
                MetadataReference.CreateFromFile(uapMdPath));
        }
    }
}
