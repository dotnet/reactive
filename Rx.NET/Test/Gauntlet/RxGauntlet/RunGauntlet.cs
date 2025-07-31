// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using RxGauntlet.Build;

using System.Diagnostics;
using System.Threading.Tasks.Dataflow;

namespace RxGauntlet;

/// <summary>
/// Runs the specified tests against a particular version of Rx.NET.
/// </summary>
/// <param name="testTypes">
/// The tests to include in this Gauntlet run. (Some tests are designed specifically for post-V6 releases, so
/// we don't include those when running against older published versions of Rx.NET)
/// </param>
/// <param name="packageSelections">
/// Describes the various Rx.NET packages comprising the version of Rx.NET to be tested. This will always include
/// a main package (<see cref="TestRunPackageSelection.MainRxPackage"/>) but depending on the packaging design choice,
/// there may also be a legacy package and UI-framework-specific packages.
/// </param>
/// <param name="outputFolder">
/// The folder to which the outputs of the various test types should be written.
/// </param>
/// <param name="testId">
/// The unique test identifier that each test should write into its outputs (enabling data analysis to determine which
/// results are all part of the same test suite.)
/// </param>
/// <param name="testTimeStamp">
/// The timestamp string that each test should write into its outputs.
/// </param>
internal class RunGauntlet(
    IEnumerable<TestType> testTypes,
    TestRunPackageSelection[] packageSelections,
    string outputFolder,
    string testId,
    string testTimeStamp)
{
    /// <summary>
    /// The TFM for which all the runners described in <see cref="testTypes"/> are built.
    /// </summary>
    /// <remarks>
    /// This runner requires that each of the executables described in the various <see cref="TestType"/> entries
    /// all target the same version of .NET. (We could add a property to <see cref="TestType"/> to enable them to be
    /// different, but there's no good reason to, so this is simpler.)
    /// </remarks>
    private const string CheckRunnersTfm = "net9.0";

#if DEBUG
    private const string Configuration = "Debug";
#else
    private const string Configuration = "Release";
#endif

    internal async Task<int> RunAsync()
    {
        if (Directory.Exists(outputFolder))
        {
            Console.Error.WriteLine($"Output folder {outputFolder} already exists. Each test run should create a new output folder.");
            return 1;
        }

        Directory.CreateDirectory(outputFolder);

        // Note: although this was set up to enable parallelization (because we expect the tests to take
        // a while to execute) we have not yet enabled parallelization. That's partly because it's a lot
        // harder to diagnose failures when things run in parallel, and partly because we haven't really
        // needed it yet. (Speeding up the slowest individual checks would likely have more impact.)
        // But if we do eventually integrate this into the Rx.NET CI/CD processes we probably will want
        // to switch on parallelism here.
        TransformManyBlock<IEnumerable<TestType>, TestType> expandTestTypes = new(types => types);
        TransformManyBlock<TestType, TestTypeAndPackageSelection> expandPackageSelections = new(type => packageSelections
            .Select(packageSelection => new TestTypeAndPackageSelection(type, packageSelection)));

        ActionBlock<TestTypeAndPackageSelection> runTest = new(RunTestAsync);

        expandTestTypes.LinkTo(expandPackageSelections, new() { PropagateCompletion = true });
        expandPackageSelections.LinkTo(runTest, new() { PropagateCompletion = true });

        expandTestTypes.Post(testTypes);
        expandTestTypes.Complete();
        await runTest.Completion.ConfigureAwait(false);

        return 0;
    }

    private async Task RunTestAsync(TestTypeAndPackageSelection typeAndPackageSelection)
    {
        Console.WriteLine(typeAndPackageSelection);

        var testType = typeAndPackageSelection.Type;
        var testRunnerExecutableFolder = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            $@"..\..\..\..\{testType.SrcFolderRelativePath}\bin\{Configuration}\{CheckRunnersTfm}\"));
        var testRunnerExecutablePath = Path.Combine(
            testRunnerExecutableFolder,
            testType.ExecutableName);

        var packageArguments = MakePackageArguments(typeAndPackageSelection);
        var customFeedArgumentIfRequired = typeAndPackageSelection.PackageSelection.CustomPackageSource is string packageSource
            ? $" --package-source {packageSource}"
            : string.Empty;
        var testIdArgument = $" --test-run-id {testId}";
        var timestampArgument = $" --test-timestamp {testTimeStamp}";

        var outputBaseName = Path.GetFileNameWithoutExtension(testType.OutputName);
        var outputExtension = Path.GetExtension(testType.OutputName);
        var outputForThisPackageSelection = $"{outputBaseName}-{typeAndPackageSelection.PackageSelection.MainRxPackage.PackageId}-{typeAndPackageSelection.PackageSelection.MainRxPackage.Version}{outputExtension}";
        var outputPath = Path.Combine(outputFolder, outputForThisPackageSelection);
        var outputArgument = $" --output {outputPath}";
        var startInfo = new ProcessStartInfo
        {
            FileName = testRunnerExecutablePath,
            UseShellExecute = false,
            //CreateNoWindow = true,
            Arguments = packageArguments + customFeedArgumentIfRequired + testIdArgument + timestampArgument + outputArgument,
            WorkingDirectory = testRunnerExecutableFolder,
        };

        Console.WriteLine($"{startInfo.FileName} {startInfo.Arguments}");
        try
        {
            using var process = new Process { StartInfo = startInfo };
            process.Start();

            await process.WaitForExitAsync().ConfigureAwait(false);

            Console.WriteLine();
        }
        catch (Exception x)
        {
            Console.WriteLine(x);
            throw;
        }
    }

    private static string MakePackageArguments(TestTypeAndPackageSelection typeAndPackageSelection)
    {
        var uiPackageArguments = string.Join(
                    " ",
                    typeAndPackageSelection.PackageSelection.RxUiPackages.Select(package =>
                        $"--rx-package {package.PackageId},{package.Version} " +
                        $""));

        uiPackageArguments = uiPackageArguments == "" ? "" : " " + uiPackageArguments; // Add space where necessary
        var mainRxPackage = typeAndPackageSelection.PackageSelection.MainRxPackage;
        var packageArguments = $"--rx-main-package {mainRxPackage.PackageId},{mainRxPackage.Version}{uiPackageArguments}";
        packageArguments = typeAndPackageSelection.PackageSelection.LegacyRxPackage is PackageIdAndVersion legacyRxPackage
            ? $"{packageArguments} --rx-legacy-package {legacyRxPackage.PackageId},{legacyRxPackage.Version}"
            : packageArguments;
        return packageArguments;
    }

    private record TestTypeAndPackageSelection(TestType Type, TestRunPackageSelection PackageSelection);
}
