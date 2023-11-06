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
        static ApiApprovalTests()
        {
            VerifierSettings.OnVerifyMismatch((filePair, message) => DiffPlexReporter.Report(filePair.ReceivedPath, filePair.VerifiedPath, message));
        }

        public ApiApprovalTests()
            : base()
        {
        }

        // TODO IDG10: temporarily disabled, because PublicApiGenerator is essentially incapable of
        // validating type exported facades. Even if we pass it a list of all the types we want
        // to export, it makes the assumption that they are all canonically defined in the same
        // assembly.
        //[Fact]
        //public Task LegacySystemReactive()
        //{
        //    var facadeAsm = typeof(System.Reactive.Placeholder).Assembly;
        //    ModuleDefinition module = ModuleDefinition.ReadModule(facadeAsm.Location);
        //    Type[] exportedTypes = module.ExportedTypes
        //        //.Where(et => et.IsPublic)
        //        .Select(et => facadeAsm.GetType(et.FullName.Replace("/", "+"))).ToArray();
        //    var publicApi = GeneratePublicApi(exportedTypes);
        //    return Verify(publicApi, "cs");
        //}

        [Fact]
        public Task Core()
        {
            var publicApi = GeneratePublicApi(typeof(System.Reactive.Unit).Assembly);
            return Verify(publicApi, "cs");
        }

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
            ApiGeneratorOptions options = new()
            {
                AllowNamespacePrefixes = new[] { "System", "Microsoft" },
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
}
