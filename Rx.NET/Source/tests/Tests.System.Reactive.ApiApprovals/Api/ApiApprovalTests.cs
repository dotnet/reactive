// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using PublicApiGenerator;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace ReactiveTests.Tests.Api
{
    public class ApiApprovalTests : VerifyBase
    {
        static ApiApprovalTests()
        {
            VerifierSettings.OnVerifyMismatch((filePair, message, autoVerify) => DiffPlexReporter.Report(filePair.ReceivedPath, filePair.VerifiedPath, message));
        }

        public ApiApprovalTests()
            : base()
        {
        }

        // Note:
        //  System.Reactive uses the .NET SDK's built in package validation, specifically the
        //      PackageValidationBaselineVersion feature to ensure backwards compatibility
        //  System.Reactive is using Microsoft.CodeAnalysis.PublicApiAnalyzers to ensure stability of
        //      its public API.
        // TODO:
        //  Move Aliases and Testing packages over to one of the mechanisms above
        //  Add similar API checking to the new FrameworkIntegrations packages

        [Fact]
        public Task Aliases()
        {
            var publicApi = GeneratePublicApi(typeof(System.Reactive.Observable.Aliases.QueryLanguage).Assembly);
            return Verify(publicApi, "cs");
        }

        [Fact]
        public Task Testing()
        {
            var publicApi = GeneratePublicApi(typeof(Microsoft.Reactive.Testing.TestScheduler).Assembly);
            return Verify(publicApi, "cs");
        }

        private string GeneratePublicApi(Assembly assembly)
        {
            var options = MakeGeneratorOptions();
            return Filter(ApiGenerator.GeneratePublicApi(assembly, options));
        }

        private static ApiGeneratorOptions MakeGeneratorOptions()
        {
            return new()
            {
                AllowNamespacePrefixes = ["System", "Microsoft"]
            };
        }

        private static string Filter(string text)
        {
            return string.Join(Environment.NewLine, text.Split(
                                                        [
                                                            Environment.NewLine
                                                        ], StringSplitOptions.RemoveEmptyEntries)
                                                        .Where(l => !l.StartsWith("[assembly: AssemblyVersion("))
                                                        .Where(l => !l.StartsWith("[assembly: AssemblyFileVersion("))
                                                        .Where(l => !l.StartsWith("[assembly: AssemblyInformationalVersion("))
                                                        .Where(l => !l.StartsWith("[assembly: System.Reflection.AssemblyMetadata(\"CommitHash\""))
                                                        .Where(l => !l.StartsWith("[assembly: System.Reflection.AssemblyMetadata(\"RepositoryUrl\""))
                                                        .Where(l => !string.IsNullOrWhiteSpace(l))
            );
        }
    }
}
