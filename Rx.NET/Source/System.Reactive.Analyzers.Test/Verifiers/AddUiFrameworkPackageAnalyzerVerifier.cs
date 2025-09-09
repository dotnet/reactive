// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

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
                    var compilationOptions = solution.GetProject(projectId).CompilationOptions;
                    compilationOptions = compilationOptions
                        .WithOutputKind(Microsoft.CodeAnalysis.OutputKind.ConsoleApplication);
                    solution = solution
                        .WithProjectCompilationOptions(projectId, compilationOptions)
                        .AddMetadataReference(projectId, rxMetadataRef);

                    return solution;
                });

                ReferenceAssemblies = ReferenceAssemblies.Net.Net80Windows;

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
