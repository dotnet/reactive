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
        //  System.Reactive (now a facade) uses the .NET SDK's built in package validation, specifically the
        //      PackageValidationBaselineVersion feature to ensure backwards compatibility
        //  System.Reactive is using Microsoft.CodeAnalysis.PublicApiAnalyzers to ensure stability of
        //      its public API.
        // TODO:
        //  Move Aliases and Testing packages over to one of the mechanisms above
        //  Add similar API checking to:
        //      The old facade packages
        //      The new FrameworkIntegrations packages

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

        private string GeneratePublicApiIncludingTypeForwarders(Assembly assembly)
        {
            var ts = assembly.GetTypes();
            var ets = assembly.GetExportedTypes();
            var ets2 = assembly.ExportedTypes;
            var attrs = assembly.GetCustomAttributes();

            var asmDef = Mono.Cecil.AssemblyDefinition.ReadAssembly(assembly.Location);
            //foreach (var exportedType in asmDef.MainModule.ExportedTypes)
            //{
            //    if (exportedType.IsForwarder)
            //    {
            //        Console.WriteLine($"Forwarded Type: {exportedType.FullName}");
            //    }
            //}


            var options = MakeGeneratorOptions();
            Type[] types = asmDef.MainModule.ExportedTypes
                .Where(t => t.IsForwarder)
                .Select(t =>
                {
                    var type = assembly.GetType(t.FullName);
                    if (type == null)
                    {
                        Debugger.Break();
                    }
                    return type;
                })
                .Concat(assembly.ExportedTypes) // DOESN'T WORK!
                .ToArray();

            return Filter(ApiGenerator.GeneratePublicApi(types, options));

                //.GroupBy(t => t.Namespace)
                //.OrderBy(ns => ns.Key)
                //.Select(ns =>
                //{
                //    StringBuilder sb = new();
                //    sb.AppendLine($"namespace {ns.Key}");
                //    sb.AppendLine("{");
                //    foreach (var type in ns.OrderBy(t => t.Name))
                //    {v
                //        string typePublicApi = ApiGenerator.GeneratePublicApi(type, options);
                //        sb.AppendLine(typePublicApi);
                //    }
                //    sb.AppendLine("}");

            //    return sb.ToString();
            //}));
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
