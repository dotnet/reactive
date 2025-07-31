// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace RxGauntlet;

/// <summary>
/// Describes one of the types of test that should be executed as part of a Gauntlet run.
/// </summary>
/// <param name="Name">
/// A descriptive name to display when running this test.
/// </param>
/// <param name="SrcFolderRelativePath">
/// The location of the folder containing the project that executes this test.
/// </param>
/// <param name="ExecutableName">
/// The name of the executable build by the project that executes this test.
/// </param>
/// <param name="OutputName">
/// The name of the JSON file that the test produces in its output folder to capture the test results.
/// </param>
/// <param name="SkipForPreV7">
/// True if this test should be skipped when running the <see cref="RxGauntlet.Cli.RxGauntletAllPublishedRxCommand" />
/// Tests that check for problems that have existed in previously published versions of Rx will set this to
/// <see langword="false" />, because it's important to verify that these checks do actually work. We need to verify
/// that the tests correctly report problems where problems are known to exist. However, some tests work by creating
/// 'before' and 'after' scenarios in which there are two versions of Rx: the one being tested, and an 'old' version,
/// which is fixed to v6.0.1. It makes no sense to run these tests against older already-published versions of Rx, because
/// we'd end up with (for example) Rx 5.0.0 being the 'new' and 6.0.1 as the 'old', or we'd be comparing 6.0.1 with
/// itself. So tests that only make sense for candidate future versions set this to <see langword="true" />.
/// </param>
internal record TestType(
    string Name,
    string SrcFolderRelativePath,
    string ExecutableName,
    string OutputName,
    bool SkipForPreV7 = false)
{
    /// <summary>
    /// Gets all of the test types to be executed when running the Gauntlet.
    /// </summary>
    public static TestType[] All { get; } =
    [
        new TestType("Bloat (Issue 1745)", @"Checks\Bloat\CheckIssue1745", "CheckIssue1745.exe", "CheckIssue1745.json"),
        new TestType("Extension Method Fail with DisableTransitiveFrameworkReferences", @"Checks\ExtensionMethods\CheckDisableTransitiveFailingExtensionMethod", "CheckDisableTransitiveFailingExtensionMethod.exe", "CheckDisableTransitiveFailingExtensionMethod.json"),
        new TestType("Plug-in gets Wrong Rx (Issue 97)", @"Checks\PlugIns\CheckIssue97", "CheckIssue97.exe", "CheckIssue97.json"),
        new TestType("Transitive References", @"Checks\TransitiveReferences\CheckTransitiveFrameworkReference", "CheckTransitiveFrameworkReference.exe", "CheckTransitiveFrameworkReference.json", SkipForPreV7: true),
    ];
}
