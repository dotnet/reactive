// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;

namespace System.Reactive.Analyzers.Test.Verifiers
{
    public sealed class NetCoreTest : TestBase
    {
        public NetCoreTest()
            : base(CreateReferenceAssemblies())
        {
        }

        protected override string GetRxDllPath()
        {
            // TODO: but this next bit isn't actually right! We set up the ReferenceAssemblies to be
            // net8.0-windows. (Should we actually be running a set of tests against .NET FX too?)

            // The test assembly is built against .NET Framework 4.8, so when testing how the
            // analyzer works with .NET FX projects, we can just use the copy of System.Reactive.dll
            // that's in the test project's output directory.
            return Path.Combine(
                Path.GetDirectoryName(typeof(AddUiFrameworkPackageAnalyzerVerifier<>).Assembly.Location),
                "System.Reactive.dll");
        }

        protected override Solution AdjustSolutionIfRequired(Solution solution, Project project)
        {
            // There appears to be a bug in Microsoft.CodeAnalysis.Analyzer.Testing 1.1.2. Specifically here:
            //  https://github.com/dotnet/roslyn-sdk/blob/9a8d9e80debe3ad4c811542df27de175cdaf117a/src/Microsoft.CodeAnalysis.Testing/Microsoft.CodeAnalysis.Analyzer.Testing/ReferenceAssemblies.cs#L463
            // This causes it to prefer the WindowsBase.dll in the .NET 8.0 reference assemblies over the
            // one in the desktop framework reference assemblies. This is wrong, because when an application
            // has chosen to use the desktop framework, it will normally get that second one, so the analyzer
            // test libraries are not reproducing the real-world scenario correctly.
            // There have been several changes to that code since the 1.1.2 release, including one that selects
            // the higher-versioned assembly when there are multiple candidates. That would fix this problem
            // because the WindowsBase.dll in the main net8.0-windowsXXX reference assemblies is version 4.0.0.0,
            // but the one in the desktop framework reference assemblies is version 8.0.0.0. (It's slightly weird
            // that there are two different versions, but it's because one of the possible deployments for .NET
            // on Windows is to support various Windows Runtime APIs without needing the full desktop framework
            // including WPF and Windows Forms to be installed. Some of the types available in this 'Windows
            // Runtime, no UI frameworks' mode live in WindowsBase.dll. But it has to be a trimmed down version
            // because the full WindowsBase.dll contains various WPF-specific functionality. So the core .NET
            // runtime on Windows provides a version of WindowsBase.dll with all the WPF code removed.
            // But if you're targetting the desktop framework, you're supposed to get the full version of
            // WindowsBase.dll.
            // Unfortunately, no newer versions of Microsoft.CodeAnalysis.Analyzer.Testing have been made
            // available on NuGet as of 2025/09/09 (not even preview builds), so although this bug appears
            // to have been fixed, we can't yet take advantage of the fix.
            // So we need to work around it by removing the incorrect WindowsBase.dll from the references,
            // and replacing it with the correct one.
            var windowsBasesInCoreFramework = project.MetadataReferences.Where(r => r.Display!.Contains("WindowsBase.dll")).ToList();
            var windowsBaseInCoreFramework = project.MetadataReferences.Single(r => r.Display!.Contains("WindowsBase.dll"));
            var assembliesInDesktopFramework = project.MetadataReferences.Where(r => r.Display!.Contains("PresentationFramework.dll")).ToList();
            var assemblyInDesktopFramework = project.MetadataReferences.Single(r => r.Display!.Contains("PresentationFramework.dll"));

            // Ideally we'd retrieve the FilePath but that's private, so we have to extract it from the Display.
            // This is a bit fragile, but as soon as the next version of Microsoft.CodeAnalysis.Analyzer.Testing ships,
            // we can remove this workaround anyway.
            var desktopFrameworkUnpackFolder = Path.GetDirectoryName(assemblyInDesktopFramework.Display)!;

            var referencesWithWorkaround = project.MetadataReferences.ToList();
            referencesWithWorkaround.Remove(windowsBaseInCoreFramework);
            referencesWithWorkaround.Add(MetadataReference.CreateFromFile(Path.Combine(desktopFrameworkUnpackFolder, "WindowsBase.dll")));
            return solution.WithProjectMetadataReferences(
                project.Id, referencesWithWorkaround);
        }

        private static ReferenceAssemblies CreateReferenceAssemblies()
        {
            // TODO: why didn't this work?
            // return ReferenceAssemblies.Net.Net80Windows;

            // Somehow we need to add the winrt refs too.
            var net80win = new ReferenceAssemblies(
                    "net8.0-windows10.0.19041",
                    new PackageIdentity(
                        "Microsoft.NETCore.App.Ref",
                        "8.0.0"),
                    Path.Combine("ref", "net8.0"));
            return net80win.AddPackages([
                new PackageIdentity("Microsoft.WindowsDesktop.App.Ref", "8.0.0"),
                    new PackageIdentity("Microsoft.Windows.SDK.NET.Ref", "10.0.19041.57")]);
        }
    }
}
