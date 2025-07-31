// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Corvus.Json;

using RxGauntlet.Build;
using RxGauntlet.CommandLine;
using RxGauntlet.LogModel;

using Spectre.Console.Cli;

using System.Diagnostics;
using System.Text.Json;

namespace CheckDisableTransitiveFailingExtensionMethod;


/// <summary>
/// Implements the only command offered by this program.
/// </summary>
/// <remarks>
/// <para>
/// This builds a list of all the scenarios that need to be tested. Executes each one, and appends
/// the results into the output JSON file.
/// </para>
/// <para>
/// For each scenario, this creates a copy of the <c>CheckDisableTransitiveFailingExtensionMethod</c>
/// project, modifying settings according to the scenario being tested, and then tries to build it.
/// It reports any build failures.
/// </para>
/// </remarks>
internal sealed class CheckDisableTransitiveFailingExtensionMethodCommand : TestCommandBase<TestSettings>
{
    protected override string DefaultOutputFilename => "CheckExtensionMethodsWorkaround.json";

    protected override async Task<int> ExecuteTestAsync(
        TestDetails testDetails, CommandContext context, TestSettings settings, Utf8JsonWriter jsonWriter)
    {
        // TODO: check that using only the main package is the right thing to do here.
        PackageIdAndVersion[] replaceSystemReactiveWith = [settings.RxMainPackageParsed];

        var templateProjectFolder =
            Path.Combine(AppContext.BaseDirectory, "../../../../ExtensionMethods.DisableTransitiveWorkaroundFail/");

        string[] baseNetTfms = ["net8.0"];
        string?[] windowsVersions = [null, "windows10.0.19041.0"];
        bool?[] boolsWithNull = [null, true, false];
        bool[] bools = [true, false];

        var scenarios =
            from baseNetTfm in baseNetTfms
            from windowsVersion in windowsVersions
            from useWpf in (windowsVersion is null ? [false] : boolsWithNull)
            from useWindowsForms in (windowsVersion is null ? [false] : boolsWithNull)
            from useTransitiveFrameworksWorkaround in bools
            select new Scenario(baseNetTfm, windowsVersion, useWpf, useWindowsForms, useTransitiveFrameworksWorkaround);

        jsonWriter.WriteStartArray();
        foreach (var scenario in scenarios)
        {
            var result = await RunScenario(scenario);
            result.WriteTo(jsonWriter);
            jsonWriter.Flush();
        }
        jsonWriter.WriteEndArray();

        return 0;

        async Task<ExtensionMethodsWorkaroundTestRun> RunScenario(Scenario scenario)
        {
            Console.WriteLine(scenario);
            var tfm = scenario.WindowsVersion is string windowsVersion
                ? $"{scenario.BaseNetTfm}-{windowsVersion}"
                : scenario.BaseNetTfm;

            string rxPackage, rxVersion;
            rxPackage = rxVersion = string.Empty;
#pragma warning disable IDE0063 // Use simple 'using' statement
            using (var projectClone = ModifiedProjectClone.Create(
                templateProjectFolder,
                "CheckDisableTransitiveFailingExtensionMethod",
                (project) =>
                {
                    project.SetTargetFramework(tfm);

                    if (replaceSystemReactiveWith is not null)
                    {
                        project.ReplacePackageReference("System.Reactive", replaceSystemReactiveWith);
                        (rxPackage, rxVersion) = (replaceSystemReactiveWith[0].PackageId, replaceSystemReactiveWith[0].Version);
                    }

                    project.AddUseUiFrameworksIfRequired(scenario.UseWpf, scenario.UseWindowsForms);

                    if (scenario.EmitDisableTransitiveFrameworkReferences)
                    {
                        project.AddPropertyGroup([new("DisableTransitiveFrameworkReferences", "True")]);
                    }
                },
                settings.PackageSource is string packageSource ? [("loc", packageSource)] : null))
            {
                var buildResult = await projectClone.RunDotnetBuild("ExtensionMethods.DisableTransitiveWorkaroundFail.csproj");

                Console.WriteLine($"{scenario}: {buildResult}");

                (var includesWpf, var includesWindowsForms) = buildResult.CheckForUiComponentsInOutput();

                Debug.Assert(!string.IsNullOrWhiteSpace(rxPackage), "rxPackage should not be null or empty.");
                Debug.Assert(!string.IsNullOrWhiteSpace(rxVersion), "rxVersion should not be null or empty.");
                var rxVersionPackage = NuGetPackage.Create(
                    id: rxPackage,
                    version: rxVersion,
                    packageSource: settings.PackageSource.AsNullableJsonString());
                var config = TestRunConfigWithUiFrameworkSettings.Create(
                    baseNetTfm: scenario.BaseNetTfm,
                    emitDisableTransitiveFrameworkReferences: scenario.EmitDisableTransitiveFrameworkReferences,
                    rxVersion: rxVersionPackage,
                    useWindowsForms: scenario.UseWindowsForms,
                    windowsVersion: scenario.WindowsVersion.AsNullableJsonString(),
                    useWpf: scenario.UseWpf);
                if (scenario.WindowsVersion is string wv)
                {
                    config = config.WithWindowsVersion(wv);
                }
                return ExtensionMethodsWorkaroundTestRun.Create(
                    config: config,
                    buildSucceeded: buildResult.BuildSucceeded,
                    deployedWindowsForms: includesWindowsForms,
                    deployedWpf: includesWpf,
                    testRunDateTime: testDetails.TestRunDateTime,
                    testRunId: testDetails.TestRunId);
            }
#pragma warning restore IDE0063 // Use simple 'using' statement
        }
    }
    internal record Scenario(
        string BaseNetTfm,
        string? WindowsVersion,
        bool? UseWpf,
        bool? UseWindowsForms,
        bool EmitDisableTransitiveFrameworkReferences);
}
