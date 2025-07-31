// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace RxGauntlet.Cli;

/// <summary>
/// Handles the CLI's <c>test-all-published-rx</c> command.
/// </summary>
/// <remarks>
/// We run the gauntlet against already-published versions primarily to verify that the tests are functioning
/// correctly. We need to be confident that these tests will detect the problems they are looking for, and in
/// cases where we're checking for regressions, we can do that by running against old versions known to have
/// specific problems, and verifying that Gauntlet detects them correctly (and also that it does not falsely
/// report a problem in versions known not to have that problem).
/// </remarks>
internal class RxGauntletAllPublishedRxCommand : RxGauntletCommandBase<RxGauntletAllPublishedRxCommandSettings>
{
    protected override bool IsForPostV6Releases => false;

    protected override TestRunPackageSelection[] GetPackageSelection(RxGauntletAllPublishedRxCommandSettings settings)
    {
        return
            [
                // TODO: we could actually supply UI-framework-specific ids for earlier versions. Do we need to?
                new(new("System.Reactive.Linq", "3.0.0"), null, [], null),
                new(new("System.Reactive.Linq", "3.1.0"), null, [], null),
                new(new("System.Reactive", "4.4.1"), null, [], null),
                new(new("System.Reactive", "5.0.0"), null, [], null),
                new(new("System.Reactive", "6.0.1"), null, [], null),
            ];
    }
}
