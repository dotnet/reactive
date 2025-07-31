// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace CheckTransitiveFrameworkReference.ScenarioGeneration;

internal static class AppDependenciesScenarioGeneration
{
    private static readonly bool[] BoolValues = [false, true];
    private static readonly bool[] BoolJustFalse = [false];

    private static readonly string[] TfmLists = ["net8.0", "net8.0;net8.0-windows10.0.19041"];
    private static readonly NewRxLegacyOptions[] AddOptions = [NewRxLegacyOptions.JustMain, NewRxLegacyOptions.JustLegacy, NewRxLegacyOptions.MainAndLegacy];

    public static IEnumerable<AppDependencies> GenerateOldTransitiveRefThenAddCombinations()
    {
        // Dim 1: initially transitive reference to old
        // Dim 2: latest acquired by adding package ref to project
        // If System.Reactive is relegated to being a legacy facade, there are actually three variations here:
        //  Just add a reference to new main Rx package
        //  Just add a reference to new legacy Rx package
        //  Add references to both
        return
            from v in GetVariations()
            let libRef = new TransitiveRxReferenceViaLibrary(
                v.LibTfms, ReferencesNewRxVersion: false, HasWindowsTargetUsingUiFrameworkSpecificRxFeature: v.LibHasUiCode)
            select new AppDependencies(
               RxBefore:
               [
                    libRef
               ],
               RxAfter:
               [
                   libRef,
                   new NewRx(LegacyPackageChoice: v.AddOption, IncludeUiPackages: false),
               ]);
    }

    public static IEnumerable<AppDependencies> GenerateOldDirectAndTransitiveRefThenUpgradeCombinations()
    {
        // Dim 1: initially transitive AND package reference to old
        // Dim 2: latest acquired by updating package ref in project
        // As with the preceding cases, if System.Reactive is relegated to being a legacy facade, there are actually
        // three variations here:
        //  Replace reference with new main Rx package  (System.Reactive v6 -> System.Reactive.Net v7)
        //  Update reference to new legacy Rx package (System.Reactive v6 -> System.Reactive v7)
        //  Update legacy package reference AND add new main (System.Reactive v6 ->
        //                                                     System.Reactive v7 + System.Reactive.Net v7)
        // This first one, in which we replace the app's reference with the new main package will hit build errors
        // because we end up with two versions of Rx in scope.
        return
            from v in GetVariations()
            let libRef = new TransitiveRxReferenceViaLibrary(
                v.LibTfms, ReferencesNewRxVersion: false, HasWindowsTargetUsingUiFrameworkSpecificRxFeature: v.LibHasUiCode)
            select new AppDependencies(
               RxBefore:
               [
                    libRef,
                    new OldRx()
               ],
               RxAfter:
               [
                   libRef,
                   new NewRx(LegacyPackageChoice: v.AddOption, IncludeUiPackages: false),
               ]);
    }

    private static IEnumerable<(NewRxLegacyOptions AddOption, string LibTfms, bool LibHasUiCode)> GetVariations()
    {
        return
            from add in AddOptions
            from tfms in TfmLists
            from libHasUiCode in (tfms.Contains("-windows") ? BoolValues : BoolJustFalse)
            select (add, tfms, libHasUiCode);
    }

    public static IEnumerable<AppDependencies> GenerateNewDirectThenAddTransitiveRefToOldCombinations()
    {
        // Dim 1: initially package reference to new, then we add a transitive ref to the old
        // Dim 2: direct ref to latest package in csproj (both before and after)
        //
        // TODO: I'm starting to think that this AppChoice needs to include whether we have UseWpf/UseWindowsForms,
        // and also a more flexible before/after spec: it might need to be able to provide a list of what to
        // do there, and when we specify an Rx package reference direct from the app, that needs to be able
        // to say whether it's old or new, and whether it should include the legacy System.Reactive where
        // that's appropriate.

        return
            from v in GetVariations()
            select new AppDependencies(
               RxBefore:
               [
                    new NewRx(LegacyPackageChoice: NewRxLegacyOptions.JustMain, IncludeUiPackages: false)
               ],
               RxAfter:
               [
                   new NewRx(LegacyPackageChoice: v.AddOption, IncludeUiPackages: false),
                   new TransitiveRxReferenceViaLibrary(
                    v.LibTfms, ReferencesNewRxVersion: false, HasWindowsTargetUsingUiFrameworkSpecificRxFeature: v.LibHasUiCode)
               ]);
    }


    public static AppDependencies[] Generate()
    {
        var hasOldTransitiveRefThenAdd = GenerateOldTransitiveRefThenAddCombinations();
        var oldDirectAndTransitiveRefThenUpgradeCombinations = GenerateOldDirectAndTransitiveRefThenUpgradeCombinations();
        var newDirectRefThenAddOldTransitiveCombinations = GenerateNewDirectThenAddTransitiveRefToOldCombinations();

        // TODO: do we need to model starting with a new direct ref then adding a new transitive ref? There's no reason to
        // expect that not to work. But what if we have transitive refs to both old and new?

        AppDependencies[] appDependencyChoices =
        [
            ..hasOldTransitiveRefThenAdd,
            ..oldDirectAndTransitiveRefThenUpgradeCombinations,
            ..newDirectRefThenAddOldTransitiveCombinations,
        ];

        // TODO: are we covering cases where an app uses Rx but had been relying on a transitive dependency,
        // and was using the UI-specific features this way, but then they upgrade to a newer version of the
        // lib that supplied that dependency, and now the app code using UI-specific Rx features no longer builds?

        return appDependencyChoices;
    }


}
