// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

namespace System.Reactive.Analyzers.Test.Verifiers
{
#pragma warning disable CA1812 // CA1812 wants this to be a static class, but since it has a base class, it can't be.
    internal sealed class AddUiFrameworkPackageAnalyzerVerifier :
        CSharpAnalyzerVerifier<AddUiFrameworkPackageAnalyzer, AddUiFrameworkPackageAnalyzerVerifier.Test, DefaultVerifier>
#pragma warning restore CA1812
    {
#pragma warning disable CA1812 // CA1812 thinks this is never instantiated, even though it's used as a type argument for a parameter with a new() constrain!
        public sealed class Test : CSharpAnalyzerTest<AddUiFrameworkPackageAnalyzer, DefaultVerifier>
#pragma warning restore CA1812
        {
            public Test()
            {
                SolutionTransforms.Add((solution, projectId) =>
                {
                    // We need to make the System.Reactive.dll assembly available to the compiler.
                    // In particular, it needs to be the reference assembly, because these tests
                    // need to model what developers will see in real projects. (If these tests
                    // supply the compiler with the runtime library, which is the build output of
                    // the System.Reactive project, we get different errors in the tests than the
                    // errors they aim to provoke.)
                    // That's why this test project references System.Reactive.MakeRefAssemblies.
                    // Note: this means we need to be careful never to try to load the
                    // System.Reactive.dll assembly in this project. Being a reference assembly,
                    // it's suitable for use by the compiler, but if the runtime tries to load it,
                    // we'll get:
                    //  System.BadImageFormatException: Could not load file or assembly
                    // So that means we can't use Rx at all in this particular test project.
                    // We can't even refer to any Rx-defined type such as Observable.
                    var rxPath = Path.Combine(
                        Path.GetDirectoryName(typeof(AddUiFrameworkPackageAnalyzerVerifier).Assembly.Location),
                        "System.Reactive.dll");
                    MetadataReference rxMetadataRef = MetadataReference.CreateFromFile(rxPath);
                    var project = solution.GetProject(projectId);
                    var compilationOptions = project.CompilationOptions;
                    compilationOptions = compilationOptions
                        .WithOutputKind(Microsoft.CodeAnalysis.OutputKind.ConsoleApplication);
                    solution = solution
                        .WithProjectCompilationOptions(projectId, compilationOptions)
                        .AddMetadataReference(projectId, rxMetadataRef);
                    project = solution.GetProject(projectId);

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
                    solution = solution.WithProjectMetadataReferences(
                        projectId, referencesWithWorkaround);

                    return solution;
                });

                // Somehow we need to add the winrt refs too.
                var net80win = new ReferenceAssemblies(
                        "net8.0-windows10.0.19041",
                        new PackageIdentity(
                            "Microsoft.NETCore.App.Ref",
                            "8.0.0"),
                        Path.Combine("ref", "net8.0"));
                ReferenceAssemblies = net80win.AddPackages([
                    new PackageIdentity("Microsoft.WindowsDesktop.App.Ref", "8.0.0"),
                    new PackageIdentity("Microsoft.Windows.SDK.NET.Ref", "10.0.19041.57")]);
                //ReferenceAssemblies = ReferenceAssemblies.Net.Net80Windows;

                // Adding a NuGet reference to Rx would more directly represent real developer
                // scenarios, but we don't build new packages in day to day dev in the IDE, so this
                // risks running tests against an out of date build. It also complicates things:
                // we'd need to set up a local package feed and keep it up to date. Also, NuGet
                // caching presumes that if the version didn't change, the package didn't change
                // either, but our versions only change when we commit. So this also makes it quite
                // likely that we end up running these tests against something other than what we
                // meant to.
                // However, if it turns out that at some point in the future we want to change over
                // to package references, we can do it like this:
                //
                // var nugetConfigPath = Path.Combine(
                //     Path.GetDirectoryName(typeof(AddUiFrameworkPackageAnalyzerVerifier).Assembly.Location),
                //     "NuGet.Config");
                // ReferenceAssemblies = ReferenceAssemblies
                //     .AddPackages([new PackageIdentity("System.Reactive", "7.0.0")])
                //     .WithNuGetConfigFilePath(nugetConfigPath);
            }
        }
    }
}
