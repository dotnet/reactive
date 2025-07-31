// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0350 // Use implicitly typed lambda - in OneOf matches, it's typically easier to understand with explicit parameter types

using CheckTransitiveFrameworkReference;
using CheckTransitiveFrameworkReference.ScenarioGeneration;

namespace Check.Specs.StepDefinitions;

[Binding]
public class AppDependenciesSteps
{
    private AppDependencies[]? _dependenciesCombinations;

    private AppDependencies[] DependenciesCombinations => _dependenciesCombinations
        ?? throw new InvalidOperationException("Dependencies combinations have not been initialized. Please call the appropriate Given step first.");

    [Given("I get all the App Dependency combinations for upgrading an old Rx reference acquired transitively")]
    public void GivenIGetAllTheAppDependencyCombinationsForUpgradingAnOldRxReferenceAcquiredTransitively()
    {
        _dependenciesCombinations = AppDependenciesScenarioGeneration.GenerateOldTransitiveRefThenAddCombinations().ToArray();
    }

    [Given("I get all the App Dependency combinations for upgrading a direct old Rx reference when an old transitive reference also exists")]
    public void GivenIGetAllTheAppDependencyCombinationsForUpgradingADirectOldRxReferenceWhenAnOldTransitiveReferenceAlsoExists()
    {
        _dependenciesCombinations = AppDependenciesScenarioGeneration.GenerateOldDirectAndTransitiveRefThenUpgradeCombinations().ToArray();
    }

    [Given("I get all the App Dependency combinations for starting with a direct new Rx reference then adding an old transitive reference")]
    public void GivenIGetAllTheAppDependencyCombinationsForStartingWithADirectNewRxReferenceThenAddingAnOldTransitiveReference()
    {
        _dependenciesCombinations = AppDependenciesScenarioGeneration.GenerateNewDirectThenAddTransitiveRefToOldCombinations().ToArray();
    }

    [Given("only the combinations where Before App Dependencies are exactly")]
    public void GivenOnlyTheScenariosWhereBeforeAppDependenciesAreExactly(Table table)
    {
        GivenOnlyTheScenariosWhereBeforeAppDependenciesAreExactly(table, dep => dep.RxBefore);
    }

    [Given("only the combinations where After App Dependencies are exactly")]
    public void GivenOnlyTheScenariosWhereAfterAppDependenciesAreExactly(Table table)
    {
        GivenOnlyTheScenariosWhereBeforeAppDependenciesAreExactly(table, dep => dep.RxAfter);
    }

    [Then("at least one matching application dependency combination must exist")]
    public void ThenAtLeastOneMatchingScenarioMustExist()
    {
        Assert.AreNotEqual(0, DependenciesCombinations.Length);
    }

    [Then("each combination's TransitiveRxReferenceViaLibrary settings must be one of these combinations, the After settings must cover all of these, and where Before and After both have TransitiveRxReferenceViaLibrary, settings they must be the same")]
    public void ThenEachCombinationTransitiveRxReferenceViaLibrarySettingsMustBeOneOf(
        DataTable dataTable)
    {
        HashSet<TransitiveRxReferenceViaLibrary> expectedDependencies = new(
            dataTable.CreateSet<TransitiveRxReferenceViaLibrary>());
        HashSet<TransitiveRxReferenceViaLibrary> allAfterDependenciesSeen = [];

        foreach (var combination in DependenciesCombinations)
        {
            var beforeOrNull = combination.RxBefore
                .SelectMany<RxDependency, TransitiveRxReferenceViaLibrary?>(dep =>
                    dep.TryGetTransitiveRxReferenceViaLibrary(out var r)
                        ? [r] : [])
                .SingleOrDefault();

            var afterOrNull = combination.RxAfter
                .SelectMany<RxDependency, TransitiveRxReferenceViaLibrary?>(dep =>
                    dep.TryGetTransitiveRxReferenceViaLibrary(out var r)
                        ? [r] : [])
                .SingleOrDefault();

            if (beforeOrNull is TransitiveRxReferenceViaLibrary before)
            {
                Assert.IsTrue(expectedDependencies.Contains(before), $"RxBefore contains unexpected TransitiveRxReferenceViaLibrary: {before}");

                if (afterOrNull is not null)
                {
                    Assert.AreEqual(before, afterOrNull, $"RxBefore contai different TransitiveRxReferenceViaLibrary: {before} vs {afterOrNull}");
                }
            }
            if (afterOrNull is TransitiveRxReferenceViaLibrary after)
            {
                allAfterDependenciesSeen.Add(after);
                Assert.IsTrue(expectedDependencies.Contains(after), $"RxAfter contains unexpected TransitiveRxReferenceViaLibrary: {after}");
            }
        }

        Assert.IsTrue(
            allAfterDependenciesSeen.SetEquals(expectedDependencies),
            $"The RxAfter TransitiveRxReferenceViaLibrary settings do not cover all expected combinations. " +
            $"Missing: [{string.Join(", ", expectedDependencies.Except(allAfterDependenciesSeen))}.] " +
            $"Unexpected: [{string.Join(", ", allAfterDependenciesSeen.Except(expectedDependencies))}].");
    }

    private void GivenOnlyTheScenariosWhereBeforeAppDependenciesAreExactly(
        Table tableWithDependenciesToMatch,
        Func<AppDependencies, RxDependency[]> getActualDependencies)
    {
        var expectedDependencies =
            tableWithDependenciesToMatch.CreateSet<ExpectedAppDependencyRow>().ToArray();
        _dependenciesCombinations = DependenciesCombinations
            .Where(dep => DependenciesMatchExactly(expectedDependencies, getActualDependencies(dep)))
            .ToArray();
    }

    private bool DependenciesMatchExactly(
        ExpectedAppDependencyRow[] expected,
        RxDependency[] actual)
    {
        HashSet<ExpectedAppDependency> expectedSet = new(expected.Select(e => e.Dependency));
        HashSet<ExpectedAppDependency> actualSet = new(actual.SelectMany(ConvertRxDependencyToExpectedAppDependencies));

        return actualSet.SetEquals(expectedSet);
    }

    private ExpectedAppDependency[] ConvertRxDependencyToExpectedAppDependencies(
        RxDependency dependency)
    {
        return dependency.Match(
            (DirectRxPackageReference rx) => rx.Match<ExpectedAppDependency[]>(
                (OldRx _) => [ExpectedAppDependency.OldRx],
                (NewRx n) => n.LegacyPackageChoice switch
                {
                    NewRxLegacyOptions.JustMain => [ExpectedAppDependency.NewRxMain],
                    NewRxLegacyOptions.JustLegacy => [ExpectedAppDependency.NewRxLegacyFacade],
                    NewRxLegacyOptions.MainAndLegacy => [ExpectedAppDependency.NewRxMain, ExpectedAppDependency.NewRxLegacyFacade],
                    _ => throw new InvalidOperationException($"Uknown NewRxLegacyOptions: {n.LegacyPackageChoice}")
                }),
            (TransitiveRxReferenceViaLibrary tr) => tr.ReferencesNewRxVersion
                ? [ExpectedAppDependency.LibUsingNewRx]
                : [ExpectedAppDependency.LibUsingOldRx]);

    }

    private enum ExpectedAppDependency
    {
        LibUsingOldRx,
        LibUsingNewRx,
        OldRx,
        NewRxMain,
        NewRxLegacyFacade
    }

    private record ExpectedAppDependencyRow(ExpectedAppDependency Dependency);
}
