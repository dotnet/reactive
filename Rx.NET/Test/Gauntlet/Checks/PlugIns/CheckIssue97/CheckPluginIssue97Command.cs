// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using RxGauntlet.CommandLine;

using Spectre.Console.Cli;

using System.Text.Json;

namespace CheckIssue97;

/// <summary>
/// Implements the only command offered by this program.
/// </summary>
/// <remarks>
/// This defers to <see cref="RunCheckPluginIssue97"/> to execute all the scenarios, so this type's
/// job is just to supply the start and end of the array in the JSON output, and to pass the command
/// line arguments in a form convenient to the test logic.
/// </remarks>
internal class CheckPluginIssue97Command : TestCommandBase<TestSettings>
{
    protected override string DefaultOutputFilename => "CheckPluginIssue97.json";

    protected override async Task<int> ExecuteTestAsync(
        TestDetails testDetails,
        CommandContext context,
        TestSettings settings,
        Utf8JsonWriter jsonWriter)
    {
        jsonWriter.WriteStartArray();

        await RunCheckPluginIssue97.RunAsync(
            testDetails.TestRunId,
            testDetails.TestRunDateTime,
            jsonWriter,
            settings.GetAllParsedPackages(),
            settings.PackageSource);

        jsonWriter.WriteEndArray();

        return 0;
    }
}
