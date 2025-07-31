// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using RxGauntlet.CommandLine;

using Spectre.Console.Cli;

using System.Text.Json;

namespace CheckTransitiveFrameworkReference;

/// <summary>
/// Implements the only command offered by this program.
/// </summary>
/// <remarks>
/// <para>
/// This builds a list of all the scenarios that need to be tested. It defers to <see cref="RunTransitiveFrameworkReferenceCheck"/>
/// to perform the actual test logic for each scenario, and appends the results into the output JSON file.
/// </para>
/// <para>
/// Note that this produces hundreds of scenarios, so this check can take a while to run.
/// </para>
/// </remarks>
internal class CheckTransitiveFrameworkReferenceCommand : TestCommandBase<TestSettings>
{
    protected override string DefaultOutputFilename => "CheckTransitiveFrameworkReference.json";

    protected override async Task<int> ExecuteTestAsync(
        TestDetails testDetails,
        CommandContext context,
        TestSettings settings,
        Utf8JsonWriter jsonWriter)
    {
        jsonWriter.WriteStartArray();

        using RunTransitiveFrameworkReferenceCheck runCheck = new(
            testDetails.TestRunId,
            testDetails.TestRunDateTime,
            settings.RxMainPackageParsed,
            settings.RxLegacyPackageParsed,
            settings.RxUiFrameworkPackagesParsed,
            settings.PackageSource is string packageSource ? [("loc", packageSource)] : null);
        var scenarios = Scenario.GetScenarios().ToList();
        for (var i = 0; i < scenarios.Count; i++)
        {
            var scenario = scenarios[i];
            Console.WriteLine(scenario);

            // I'd like to do a progress bar on the TaskBar, but it seems there aren't good libraries
            // for this right now. BenchmarkDotNet uses interop:
            // https://github.com/dotnet/BenchmarkDotNet/pull/2158/files#diff-ada7ae88864325b6a0d06cfc63729b11deaa0a9090fa4153c761d96a4318956f
            // So we'll just set the title
            try
            {
                Console.Title = $"Running scenario {i + 1}/{scenarios.Count}";
            }
            catch (IOException)
            {
            }
            catch (PlatformNotSupportedException)
            {
            }

            var testResult = await runCheck.RunScenarioAsync(scenario);

            testResult.WriteTo(jsonWriter);
            await jsonWriter.FlushAsync();
        }

        jsonWriter.WriteEndArray();

        return 0;
    }
}
