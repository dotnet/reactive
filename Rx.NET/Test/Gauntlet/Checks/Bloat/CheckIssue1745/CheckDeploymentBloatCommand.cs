// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Text.Json;

using RxGauntlet.CommandLine;

using Spectre.Console.Cli;

namespace CheckIssue1745;

/// <summary>
/// Implements the only command offered by this program.
/// </summary>
/// <remarks>
/// This builds a list of all the scenarios that need to be tested. It defers to <see cref="RunDeploymentBloatCheck"/>
/// to perform the actual test logic for each scenario, and appends the results into the output JSON file.
/// </remarks>
internal class CheckDeploymentBloatCommand : TestCommandBase<TestSettings>
{
    protected override string DefaultOutputFilename => "CheckIssue1745.json";

    private static readonly string[] BaseNetTfms =
    [
        //"net6.0",
        "net8.0",
        "net9.0"
    ];

    private static readonly string[] WindowsVersions =
    [
        "windows10.0.18362.0",
        "windows10.0.19041.0",
        "windows10.0.22000.0"
    ];

    private static readonly bool?[] BoolsWithNull = [null, true, false];
    private static readonly bool[] Bools = [true, false];

    protected override async Task<int> ExecuteTestAsync(
        TestDetails testDetails, CommandContext context, TestSettings settings, Utf8JsonWriter jsonWriter)
    {
        jsonWriter.WriteStartArray();

        var scenarios =
            from baseNetTfm in BaseNetTfms
            from windowsVersion in WindowsVersions
            from useWpf in BoolsWithNull
            from useWindowsForms in BoolsWithNull
            from useTransitiveFrameworksWorkaround in Bools
            select new Scenario(baseNetTfm, windowsVersion, useWpf, useWindowsForms, useTransitiveFrameworksWorkaround, settings.RxMainPackageParsed, settings.PackageSource);

        foreach (var scenario in scenarios)
        {
            try
            {
                var result = await RunDeploymentBloatCheck.RunAsync(
                    testDetails.TestRunId, testDetails.TestRunDateTime, scenario, settings.PackageSource);
                result.WriteTo(jsonWriter);
                jsonWriter.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running scenario {scenario}: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
        jsonWriter.WriteEndArray();

        return 0;
    }
}
