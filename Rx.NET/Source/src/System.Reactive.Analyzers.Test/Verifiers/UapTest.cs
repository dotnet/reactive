// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;

namespace System.Reactive.Analyzers.Test.Verifiers
{
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
            // TODO: why didn't this work?
            // return ReferenceAssemblies.Net.Net80Windows;

            // Somehow we need to add the winrt refs too.
            var uap10 = new ReferenceAssemblies(
                    "uap10.0.18362",
                    //new PackageIdentity(
                    //    "Microsoft.NETCore.App.Ref",
                    //    "8.0.0"),
                    new PackageIdentity(
                        "Microsoft.NETCore.UniversalWindowsPlatform",
                        "6.2.14"),
                    "ref\\uap10.0.15138");
            //Path.Combine("ref", "net8.0"));

            // We're not finding the Windows.UI.Xaml namespace (which is where DependencyObject is defined).
            // In an actual UAP project this appears to be here:
            // C:\Program Files (x86)\Windows Kits\10\References\10.0.26100.0\Windows.Foundation.UniversalApiContract\19.0.0.0\Windows.Foundation.UniversalApiContract.winmd
            //
            // Microsoft.Windows.SDK.NET.Ref defines:
            //  The bits of the WinRT API that are available to all .NET apps (so this excludes Xaml types)
            //  A .NET-specific XAML projection (lib\net8.0\Microsoft.Windows.UI.Xaml.dll)
            // Microsoft.NETCore.UniversalWindowsPlatform defines:
            //  Essentially the .NET runtime library, implemented to work in a UAP context.
            //
            // A classic UAP app gets its .NET runtime library from Microsoft.NETCore.UniversalWindowsPlatform,
            // and its WinRT API from the locally-installed SDK. So to simulate that, we need to locate the
            // locally-installed SDK and add a reference to Windows.Foundation.UniversalApiContract.winmd!
            return uap10.AddPackages(
                [
                    new PackageIdentity("Microsoft.Windows.SDK.NET.Ref", "10.0.26100.84"),
                    //new PackageIdentity("Microsoft.NETCore.UniversalWindowsPlatform", "6.2.14")
                ]);
        }

        protected override Solution AdjustSolutionIfRequired(Solution solution, Project project)
        {
            var platformSdkLocation = Microsoft.Build.Utilities.ToolLocationHelper
                .GetPlatformSDKLocation("Windows", "10.0");
            //var refernceFolders = Microsoft.Build.Utilities.ToolLocationHelper
            //    .GetSDKReferenceFolders(platformSdkLocation);
            var uapRefsPath = Path.Combine(platformSdkLocation, "UnionMetadata\\10.0.26100.0\\");
            var uapMdPath = Path.Combine(platformSdkLocation, "UnionMetadata\\10.0.26100.0\\Windows.winmd");
            return solution.AddMetadataReference(
                project.Id,
                MetadataReference.CreateFromFile(uapMdPath));
        }
    }
}
