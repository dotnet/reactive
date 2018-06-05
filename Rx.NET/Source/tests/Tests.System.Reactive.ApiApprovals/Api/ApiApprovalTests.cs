// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using ApprovalTests;
using ApprovalTests.Reporters;
using PublicApiGenerator;
using System.Reflection;
using Xunit;

namespace ReactiveTests.Tests.Api
{
    [UseReporter(typeof(DiffReporter))]
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
            return ApiGenerator.GeneratePublicApi(assembly, whitelistedNamespacePrefixes: namespacePrefixWhitelist);
        }
    }
}
