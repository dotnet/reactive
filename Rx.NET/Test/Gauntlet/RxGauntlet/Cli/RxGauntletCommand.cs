// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Spectre.Console.Cli;

namespace RxGauntlet.Cli;

/// <summary>
/// Base class for all commands.
/// </summary>
/// <typeparam name="TSettings">The type representing the command line args.</typeparam>
/// <remarks>
/// <para>
/// All commands essentially do the same thing: they run a series of tests against one or more sets of Rx packages.
/// So the <c>test-all-published-rx</c> and <c>test-candidate</c> commands both use this class to define the main
/// execution logic.
/// </para>
/// <para>
/// This creates a unique id for the test run (enabling any data analysis to determine when multiple JSON files
/// all correspond to part of the same run). It determines which individual tests to run. (When running the
/// <c>test-all-published-rx</c> command, we omit tests that only make sense for candidate future releases.)
/// </para>
/// </remarks>
internal abstract class RxGauntletCommandBase<TSettings> : AsyncCommand<TSettings>
    where TSettings : CommandSettings, IOrchestrationCommandSettings
{
    /// <summary>
    /// Gets a value indicating whether the Gauntlet is being run for a post-V6 candidate release.
    /// </summary>
    /// <remarks>
    /// The <c>test-all-published-rx</c> command sets this to false, causing us to omit tests that
    /// make no sense for already-published versions.
    /// </remarks>
    protected virtual bool IsForPostV6Releases => true;

    protected abstract TestRunPackageSelection[] GetPackageSelection(TSettings settings);

    public override async Task<int> ExecuteAsync(CommandContext context, TSettings settings)
    {
        var packageSelections = GetPackageSelection(settings);

        IEnumerable<TestType> testTypes = TestType.All;
        if (!IsForPostV6Releases)
        {
            testTypes = testTypes.Where(t => !t.SkipForPreV7);
        }

        var testRunDateTime = DateTimeOffset.UtcNow;
        string outputFolder;
        if (settings.OutputDirectory is not null)
        {
            outputFolder = settings.OutputDirectory;
        }
        else
        {
            outputFolder = Path.Combine(
                AppContext.BaseDirectory,
                testRunDateTime.ToString("yyyy-MM-dd_HH-mm-ss"));
            Console.WriteLine($"Output folder: {outputFolder}");
        }
        var testId = settings.TestRunId ?? testRunDateTime.ToString("yyyy-MM-dd_HH-mm-ss-") + System.Security.Cryptography.RandomNumberGenerator.GetHexString(8);
        var testTimeStamp = settings.TestTimestamp ?? testRunDateTime.ToString("yyyy-MM-dd_HH-mm-ss");

        try
        {
            RunGauntlet runner = new(
                testTypes,
                packageSelections,
                outputFolder,
                testId,
                testTimeStamp);
            var result = await runner.RunAsync();

            return result;
        }
        catch (Exception x)
        {
            Console.Error.WriteLine($"An error occurred while running the tests: {x}");
            return 1;
        }
    }
}
