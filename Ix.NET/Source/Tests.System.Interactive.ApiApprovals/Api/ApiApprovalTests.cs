// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using PublicApiGenerator;

using VerifyXunit;

namespace Tests.System.Interactive.ApiApprovals.Api;

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

    [Fact]
    public Task SystemInteractive()
    {
        var publicApi = GeneratePublicApi(typeof(EnumerableEx).Assembly);
        return Verify(publicApi, "cs");
    }

    [Fact]
    public Task SystemInteractiveProviders()
    {
        var publicApi = GeneratePublicApi(typeof(QueryableEx).Assembly);
        return Verify(publicApi, "cs");
    }

    [Fact]
    public Task SystemInteractiveAsync()
    {
        var publicApi = GeneratePublicApi(typeof(AsyncEnumerableEx).Assembly);
        return Verify(publicApi, "cs");
    }

    [Fact]
    public Task SystemInteractiveAsyncProviders()
    {
        var publicApi = GeneratePublicApi(typeof(AsyncQueryableEx).Assembly);
        return Verify(publicApi, "cs");
    }

    [Fact]
    public Task SystemLinqAsync()
    {
        var publicApi = GeneratePublicApi(typeof(AsyncEnumerable).Assembly);
        return Verify(publicApi, "cs");
    }

    [Fact]
    public Task SystemLinqAsyncQueryable()
    {
        var publicApi = GeneratePublicApi(typeof(AsyncQueryable).Assembly);
        return Verify(publicApi, "cs");
    }

    private string GeneratePublicApi(Assembly assembly)
    {
        ApiGeneratorOptions options = new()
        {
            AllowNamespacePrefixes = ["System", "Microsoft"]
        };
        return Filter(ApiGenerator.GeneratePublicApi(assembly, options));
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
