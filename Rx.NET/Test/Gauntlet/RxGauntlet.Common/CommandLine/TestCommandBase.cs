// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using NodaTime;

using Spectre.Console.Cli;

using System.Globalization;
using System.Text.Json;

namespace RxGauntlet.CommandLine;

public abstract class TestCommandBase<TSettings> : AsyncCommand<TSettings>
    where TSettings : TestSettings
{
    protected abstract string DefaultOutputFilename { get; }

    public override async Task<int> ExecuteAsync(CommandContext context, TSettings settings)
    {
        var testTimestampText = settings.TestTimestamp ?? DateTimeOffset.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss");
        var testRunId = settings.TestRunId ?? $"{testTimestampText}-{System.Security.Cryptography.RandomNumberGenerator.GetHexString(8)}";
        var testDateTime = OffsetDateTime.FromDateTimeOffset(DateTimeOffset.ParseExact(
            testTimestampText,
            "yyyy-MM-dd_HH-mm-ss",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal));

        TestDetails testDetails = new(testDateTime, testRunId);
        var outputPath = settings.OutputPath ?? DefaultOutputFilename;

        using FileStream output = new(outputPath, FileMode.Create, FileAccess.Write, FileShare.Read);
        using Utf8JsonWriter jsonWriter = new(output, new JsonWriterOptions { Indented = true });
        return await ExecuteTestAsync(testDetails, context, settings, jsonWriter);
    }

    protected abstract Task<int> ExecuteTestAsync(
        TestDetails testDetails,
        CommandContext context,
        TSettings settings,
        Utf8JsonWriter jsonWriter);

    protected record TestDetails(
        OffsetDateTime TestRunDateTime,
        string TestRunId);
}
