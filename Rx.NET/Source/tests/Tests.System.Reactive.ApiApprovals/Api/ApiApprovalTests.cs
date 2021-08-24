// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using PublicApiGenerator;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace ReactiveTests.Tests.Api
{
    public class ApiApprovalTests : VerifyBase
    {
        private readonly VerifySettings verifySettings;

        public ApiApprovalTests()
            : base()
        {
            verifySettings = new VerifySettings();
            verifySettings.UseExtension("cs");

            VerifierSettings.OnVerifyMismatch((filePair, message) => DiffPlexReporter.Report(filePair.Received, filePair.Verified, message));
        }

        [Fact]
        public Task Core()
        {
            var publicApi = GeneratePublicApi(typeof(System.Reactive.Unit).Assembly);
            return Verify(publicApi, verifySettings);
        }

        [Fact]
        public Task Aliases()
        {
            var publicApi = GeneratePublicApi(typeof(System.Reactive.Observable.Aliases.QueryLanguage).Assembly);
            return Verify(publicApi, verifySettings);
        }

        [Fact]
        public Task Testing()
        {
            var publicApi = GeneratePublicApi(typeof(Microsoft.Reactive.Testing.TestScheduler).Assembly);
            return Verify(publicApi, verifySettings);
        }

        private string GeneratePublicApi(Assembly assembly)
        {
            var namespacePrefixWhitelist = new[] { "System", "Microsoft" };
            return Filter(ApiGenerator.GeneratePublicApi(assembly, whitelistedNamespacePrefixes: namespacePrefixWhitelist));
        }

        private static string Filter(string text)
        {
            return string.Join(Environment.NewLine, text.Split(new[]
                                                        {
                                                            Environment.NewLine
                                                        }, StringSplitOptions.RemoveEmptyEntries)
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
