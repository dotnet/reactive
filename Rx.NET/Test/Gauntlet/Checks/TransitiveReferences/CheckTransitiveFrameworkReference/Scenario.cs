// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0350 // Use implicitly typed lambda - in OneOf matches, it's typically easier to understand with explicit parameter types

using CheckTransitiveFrameworkReference.ScenarioGeneration;

using OneOf;

namespace CheckTransitiveFrameworkReference;

/// <summary>
/// 
/// </summary>
/// <param name="RxBefore">
/// For the Rx reference that changes in the before and after scenarios, this describes how this reference is acquired
/// in the before scenario.
/// </param>
/// <param name="RxUpgrade">
/// For the Rx reference that changes in the before and after scenarios, this describes how this reference is acquired
/// in the before scenario.
/// </param>
/// <param name="RxBeforeAndAfter">
/// For the Rx reference that is the same in the before and after scenarios, this describes how this reference is
/// acquired.
/// </param>
/// <param name="AppHasCodeUsingNonUiFrameworkSpecificRxDirectly"></param>
internal record Scenario(
    string ApplicationTfm,
    string TfmsOfBeforeAndAfterLibrary,
    RxDependency[] RxDependenciesBefore,
    RxDependency[] RxDependenciesAfter,
    bool UseWpfAndWindowsFormsBefore,
    bool UseWpfAndWindowsFormsAfter,
    bool DisableTransitiveFrameworkReferencesAfter,
    bool AppHasCodeUsingNonUiFrameworkSpecificRxDirectly,
    bool AppHasCodeUsingUiFrameworkSpecificRxDirectly,
    bool AppInvokesLibraryMethodThatUsesNonUiFrameworkSpecificRxFeature,
    bool AppInvokesLibraryMethodThatUsesUiFrameworkSpecificRxFeature) // TODO: do we need before/after/both flavours of this?
{
    private static readonly bool[] BoolValues = [false, true];
    private static readonly bool[] BoolJustFalse = [false];

    public static IEnumerable<Scenario> GetScenarios()
    {
        // Application dimensions.
        // There are essentially three of these.
        //
        // App dimension 1: Rx initially referenced directly by app csproj vs only referenced transitively
        // App dimension 2: latest version via csproj vs reference
        var appDependencyChoices = AppDependenciesScenarioGeneration.Generate();

        // App dimension 3: use of RX UI features
        //
        // This itself has three dimensions:
        //  Does the library's Windows-specific target contain code that uses UI-framework-specific Rx functionality?
        //  Does the app use the code in the library that uses UI-framework-specific Rx functionality?
        //  Does the app uses UI-framework-specific Rx functionality directly?
        // (The first two aren't quite independent, because the app can't use code that the library doesn't have.)
        RxUsageChoices[] GetRxUsages(AppDependencies appChoice)
        {
            // Work out whether the app has a reference to the library that uses Rx. (This determines
            // whether we emit code in the app that calls into that library.)
            bool ReferencesLib(RxDependency[] deps) => deps
                .Any(ac => ac.Match((DirectRxPackageReference _) => false, (TransitiveRxReferenceViaLibrary _) => true));

            var libAvailableBefore = ReferencesLib(appChoice.RxBefore);
            var libAvailableAfter = ReferencesLib(appChoice.RxAfter);
            var libAvailableBeforeAndAfter = libAvailableBefore && libAvailableAfter;

            // Determine whether Rx UI features are available to the library that gives us a transitive
            // Rx reference (if such a library is referenced at all).
            bool ReferenceIsLibThatHasCouldUseRxUi(RxDependency ac) => ac.Match(
                    (DirectRxPackageReference rx) => false,
                    (TransitiveRxReferenceViaLibrary t) =>
                    // Rx UI always available to old version.
                    (t.Tfms.Contains("-windows") && !t.ReferencesNewRxVersion)
                    // Currently, this check is set up so that when using the new Rx, we only build the library
                    // with references to the Rx UI framework packages if the library is going to use them.
                    // In principle, it would be possible for the library to refer to, say, System.Reactive.For.Wpf,
                    // but not use it. Since this doesn't seem like a useful scenario, we don't model it.
                    || t.HasWindowsTargetUsingUiFrameworkSpecificRxFeature);
            bool ReferencesLibThatHasCouldUseRxUi(RxDependency[] deps) => deps.Any(ReferenceIsLibThatHasCouldUseRxUi);
            var uiAvailableToLibInBefore = ReferencesLibThatHasCouldUseRxUi(appChoice.RxBefore);
            var uiAvailableToLibInAfter = ReferencesLibThatHasCouldUseRxUi(appChoice.RxAfter);
            var uiAvailableToLibInBeforeAndAfter = uiAvailableToLibInBefore && uiAvailableToLibInAfter;

            // Determines whether the library will in fact offer a public API that uses UI-specific Rx features.
            bool ReferencesLibThatProvidesUiFeature(RxDependency[] deps) => deps
                .Any(ac => ReferenceIsLibThatHasCouldUseRxUi(ac) && ac.Match(
                    (DirectRxPackageReference rx) => false,
                    (TransitiveRxReferenceViaLibrary t) => t.HasWindowsTargetUsingUiFrameworkSpecificRxFeature));
            var libProvidesUiFeatureInBefore = ReferencesLibThatProvidesUiFeature(appChoice.RxBefore);
            var libProvidesUiFeatureInAfter = ReferencesLibThatProvidesUiFeature(appChoice.RxAfter);
            var libProvidesUiFeatureInBeforeAndAfter = libProvidesUiFeatureInBefore && libProvidesUiFeatureInAfter;

            // This determines whether Rx's UI features are available as a result of the references we
            // have. (Note that even when Rx's UI features are available to the app as a result of an
            // Rx dependency acquired transitively via a library, that library won't necessarily be making
            // use of those Rx UI features.)
            bool ReferencesMakeRxUiAvailable(RxDependency[] deps) => deps
                .Any(ac => ac.Match(
                    (DirectRxPackageReference rx) => rx.Match((OldRx _) => true, (NewRx n) => n.IncludeUiPackages),
                    (TransitiveRxReferenceViaLibrary t) => t.HasWindowsTargetUsingUiFrameworkSpecificRxFeature));
            var uiAvailableViaReferencesBefore = ReferencesMakeRxUiAvailable(appChoice.RxBefore);
            var uiAvailableViaReferencesAfter = ReferencesMakeRxUiAvailable(appChoice.RxAfter);
            var uiAvailableBeforeAndAfter = uiAvailableViaReferencesBefore && uiAvailableViaReferencesAfter;

            IEnumerable<RxUsageChoices> ForAppRxUsage(bool appUseRxNonUiFeatures, bool appUseRxUiFeatures)
            {
                return
                    from appInvokesLibUi in (libProvidesUiFeatureInBeforeAndAfter ? BoolValues : BoolJustFalse)
                    select new RxUsageChoices(
                        AppInvokesLibraryCodePathsUsingRxNonUiFeatures: libAvailableBeforeAndAfter,
                        AppInvokesLibraryCodePathsUsingRxUiFeatures: appInvokesLibUi,
                        AppUseRxNonUiFeaturesDirectly: appUseRxNonUiFeatures,
                        AppUseRxUiFeaturesDirectly: appUseRxUiFeatures);
            }

            return
                (from appUsesNonUiRxDirectly in BoolValues
                 from appUseRxUiFeatures in uiAvailableBeforeAndAfter ? BoolValues : [false]
                 from usageChoice in ForAppRxUsage(appUsesNonUiRxDirectly, appUseRxUiFeatures)
                 select usageChoice)
                 .ToArray();
        }

        bool ShouldTestDisableTransitiveFrameworksWorkaround(AppDependencies appChoice)
        {
            // If the 'after' app continues to have a reference to the old System.Reactive (which happens in
            // some scenarios we test - perhaps the app doesn't use Rx directly, and ends up with transitive
            // references to both old and new Rx, and we don't want that always to mean that the app author
            // now has to take remedial action), then self-contained deployments will include the desktop
            // framework. We'd like to know if disabling transitive frameworks (the 'workaround') is effective
            // in this case. (I.e., does it stop bloat, and not cause any new problems.)
            // If we have a reference to the new System.Reactive, that typically means that the app author
            // found it necessary to do that to fix problems (or they're using some library that did that).
            // We haven't yet decided whether a new System.Reactive-as-legacy-package should still cause
            // an automatic dependency on the desktop framework, but since that's one design option, we
            // need to know if disabling transitive framework references can be used to prevent bloat.
            bool DependencyMayCauseImplicitDesktopFrameworkReference(RxDependency d) =>
                d.Match(
                    (DirectRxPackageReference rx) => rx.Match(
                        (OldRx _) => true,
                        (NewRx n) => n.LegacyPackageChoice is not NewRxLegacyOptions.JustMain),
                    (TransitiveRxReferenceViaLibrary t) => !t.ReferencesNewRxVersion);

            // Note that we are ignoring IncludeUiPackages because that means an explicit choice to do UI things,
            // at which point a self-contained deployment has to include the desktop framework.
            return appChoice.RxAfter.Any(DependencyMayCauseImplicitDesktopFrameworkReference);
        }

        return
            from appChoice in appDependencyChoices
            from rxUsage in GetRxUsages(appChoice)
            from useWpfAndWindowsFormsBefore in BoolValues
            from useWpfAndWindowsFormsAfter in BoolValues
            from disableTransitiveFrameworkReferences in (ShouldTestDisableTransitiveFrameworksWorkaround(appChoice) ? BoolValues : [false])
            select new Scenario(
                ApplicationTfm: "net8.0-windows10.0.19041",
                TfmsOfBeforeAndAfterLibrary: "net8.0;net8.0-windows10.0.19041",
                RxDependenciesBefore: appChoice.RxBefore,
                RxDependenciesAfter: appChoice.RxAfter,
                UseWpfAndWindowsFormsBefore: useWpfAndWindowsFormsBefore,
                UseWpfAndWindowsFormsAfter: useWpfAndWindowsFormsAfter,
                DisableTransitiveFrameworkReferencesAfter: disableTransitiveFrameworkReferences,
                AppHasCodeUsingNonUiFrameworkSpecificRxDirectly: rxUsage.AppUseRxNonUiFeaturesDirectly,
                AppHasCodeUsingUiFrameworkSpecificRxDirectly: rxUsage.AppUseRxUiFeaturesDirectly,
                AppInvokesLibraryMethodThatUsesNonUiFrameworkSpecificRxFeature: rxUsage.AppInvokesLibraryCodePathsUsingRxNonUiFeatures,
                AppInvokesLibraryMethodThatUsesUiFrameworkSpecificRxFeature: rxUsage.AppInvokesLibraryCodePathsUsingRxUiFeatures);
    }

    private record RxUsageChoices(
        bool AppInvokesLibraryCodePathsUsingRxNonUiFeatures,
        bool AppInvokesLibraryCodePathsUsingRxUiFeatures,
        bool AppUseRxNonUiFeaturesDirectly,
        bool AppUseRxUiFeaturesDirectly);
}
internal readonly record struct OldRx();

internal enum NewRxLegacyOptions
{
    JustMain,
    MainAndLegacy,
    JustLegacy,
}
internal readonly record struct NewRx(
    NewRxLegacyOptions LegacyPackageChoice,
    bool IncludeUiPackages);

internal readonly record struct TransitiveRxReferenceViaLibrary(
    string Tfms,
    bool ReferencesNewRxVersion,
    bool HasWindowsTargetUsingUiFrameworkSpecificRxFeature);

[GenerateOneOf]
internal partial class DirectRxPackageReference : OneOfBase<OldRx, NewRx>
{
    // Would be nice if the code generator generated these, but it doesn't today.
    public bool IsOldRx => Match((OldRx _) => true, (NewRx _) => false);
    public bool IsNewRx => Match((OldRx _) => false, (NewRx _) => true);

    public bool TryGetOldRx(out OldRx oldRx)
    {
        return TryPickT0(out oldRx, out _);
    }

    public bool TryGetNewRx(out NewRx newRx)
    {
        return TryPickT1(out newRx, out _);
    }
}

[GenerateOneOf]
internal partial class RxDependency : OneOfBase<DirectRxPackageReference, TransitiveRxReferenceViaLibrary>
{
    // Although the source generator generates conversions for each of the types we specify, it does
    // not appear to handle nesting. And although DirectRxPackageReference in turn has conversions to
    // and from OldRx and NewRx, C# only allows a single level of implicit conversion.
    //
    // I want to be able to use the constituent types of DirectRxPackageReference (OldRx and NewRx)
    // anywhere a RxDependency is required (just like I can use a TransitiveRxReferenceViaLibrary
    // anywhere a RxDependency is required, or like I can use either OldRx or NewRx anywhere a
    // DirectRxPackageReference is required). We enable this by defining conversions for those types
    // here.
    public static implicit operator RxDependency(OldRx _) => new DirectRxPackageReference(_);
    public static explicit operator OldRx(RxDependency _) => _.AsT0.AsT0;

    public static implicit operator RxDependency(NewRx _) => new DirectRxPackageReference(_);
    public static explicit operator NewRx(RxDependency _) => _.AsT0.AsT1;

    public bool IsTransitiveRxReferenceViaLibrary => Match((DirectRxPackageReference _) => false, (TransitiveRxReferenceViaLibrary _) => true);

    public bool IsOldRx => Match((DirectRxPackageReference d) => d.IsOldRx, (TransitiveRxReferenceViaLibrary _) => false);
    public bool IsNewRx => Match((DirectRxPackageReference d) => d.IsNewRx, (TransitiveRxReferenceViaLibrary _) => false);

    public bool TryGetNewRx(out NewRx newRx)
    {
        if (TryGetDirectRxPackageReference(out var pr))
        {
            return pr.TryGetNewRx(out newRx);
        }
        newRx = default;
        return false;
    }

    public bool TryGetOldRx(out OldRx oldRx)
    {
        if (TryGetDirectRxPackageReference(out var pr))
        {
            return pr.TryGetOldRx(out oldRx);
        }
        oldRx = default;
        return false;
    }

    public bool TryGetDirectRxPackageReference(out DirectRxPackageReference packageReference)
    {
        return TryPickT0(out packageReference, out _);
    }

    public bool TryGetTransitiveRxReferenceViaLibrary(out TransitiveRxReferenceViaLibrary transitive)
    {
        return TryPickT1(out transitive, out _);
    }
}
