// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Spectre.Console.Cli;

using System.ComponentModel;

namespace RxGauntlet.CommandLine;

public class TestSettings : RxSourceSettings
{
    [Description("The output path to which to write the test results. Defaults to a file named for the test in the current working directory.")]
    [CommandOption("--output")]
    public string? OutputPath { get; init; }

    [Description("The timestamp value for the test run. Defaults to the current date and time. (Used when orchestrating multiple test runners in a single run)")]
    [CommandOption("--test-timestamp")]
    public string? TestTimestamp { get; init; }

    [Description("The id of this test run. Defaults to a value based on the current date and time.")]
    [CommandOption("--test-run-id")]
    public string? TestRunId { get; init; }
}
