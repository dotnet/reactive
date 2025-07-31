// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace RxGauntlet.Cli;

/// <summary>
/// Command settings common to the <c>test-all-published-rx</c> and <c>test-candidate</c> commands.
/// </summary>
/// <remarks>
/// <para>
/// Annoyingly, we end up having implement these properties in <see cref="RxGauntletAllPublishedRxCommandSettings"/>
/// and <see cref="RxGauntletCandidateCommandSettings"/>. That's because the only mechanism Spectre Console CLI
/// currently offers for reusing command settings is inheritance, which isn't flexible enough to support the usage we
/// need. The problem is that we have three sets of common settings:
/// </para>
/// <list type="number">
/// <item>
/// Common check: <c>--output</c> (file), <c>--test-timestamp</c>, and <c>--test-run-id</c>
/// </item>
/// <item>
/// Rx source: <c>--rx-main-package</c>, <c>--rx-legacy-package</c>, <c>--rx-ui-package</c>, and <c>--package-source</c>
/// </item>
/// <item>
/// Gauntlet orchestration: <c>--output</c> (directory), <c>--test-id</c>
/// </item>
/// </list>
/// <para>
/// And then we need the following combinations of those common settings:
/// </para>
/// <list type="bullet">
/// <item>RxGauntlet in test-all-published-rx: 3</item>
/// <item>RxGauntlet in test-candidate: 2, 3</item>
/// <item>Checks: 1, 2</item>
/// </list>
/// <para>
/// There is no single-inheritance hierarchy that enables each of these common settings to be defined just once,
/// and to offer the combinations listed above.
/// </para>
/// </remarks>
internal interface IOrchestrationCommandSettings
{
    public string? OutputDirectory { get; }
    public string? TestRunId { get;  }
    public string? TestTimestamp { get; init; }
}
