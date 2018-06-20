// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using ApprovalTests;
using ApprovalTests.Reporters;
using PublicApiGenerator;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace ReactiveTests.Tests.Api
{
    [UseReporter(typeof(DiffReporter))]
    [IgnoreLineEndings(true)]
    public class ApiApprovalTests
    {
        [Fact]
        public void Core()
        {
            var publicApi = GeneratePublicApi(typeof(System.Reactive.Unit).Assembly);
            Approvals.Verify(publicApi);
        }

        [Fact]
        public void Aliases()
        {
            var publicApi = GeneratePublicApi(typeof(System.Reactive.Observable.Aliases.QueryLanguage).Assembly);
            Approvals.Verify(publicApi);
        }

        [Fact]
        public void Testing()
        {
            var publicApi = GeneratePublicApi(typeof(Microsoft.Reactive.Testing.TestScheduler).Assembly);
            Approvals.Verify(publicApi);
        }

        string GeneratePublicApi(Assembly assembly)
        {
            var namespacePrefixWhitelist = new[] { "System", "Microsoft" };
            return Filter(ApiGenerator.GeneratePublicApi(assembly, whitelistedNamespacePrefixes: namespacePrefixWhitelist));
        }

        static string Filter(string text)
        {
            return string.Join(Environment.NewLine, text.Split(new[]
                                                        {
                                                            Environment.NewLine
                                                        }, StringSplitOptions.RemoveEmptyEntries)
                                                        .Where(l => !l.StartsWith("[assembly: AssemblyVersion("))
                                                        .Where(l => !l.StartsWith("[assembly: AssemblyFileVersion("))
                                                        .Where(l => !l.StartsWith("[assembly: AssemblyInformationalVersion("))
                                                        .Where(l => !l.StartsWith("[assembly: System.Reflection.AssemblyMetadataAttribute(\"CommitHash\""))
                                                        .Where(l => !string.IsNullOrWhiteSpace(l))
            );
        }
    }
}
