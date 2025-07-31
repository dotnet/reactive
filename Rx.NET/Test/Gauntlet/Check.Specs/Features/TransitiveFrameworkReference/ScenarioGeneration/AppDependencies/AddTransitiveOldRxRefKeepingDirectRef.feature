Feature: Start with a direct new Rx reference, then add a transitive reference to an old version

Background:
    Given I get all the App Dependency combinations for starting with a direct new Rx reference then adding an old transitive reference


# This models a scenario a developer will typically stumble into:
#  * already using new Rx
#  * adds reference to a library that uses old Rx
# In designs where System.Reactive is no longer the main package, we expect compiler errors
# if the main app code itself use using Rx, because we will now effectively have two
# versions of Rx in scope. If the main app itself doesn't use Rx directly, then we don't
# expect compiler errors—having two versions of Rx around is fine in that case, because
# nobody anywhere is trying to compiler Rx code in the scopes where both versions are
# available.
# TODO: In either case, would we also want to check whether the build issues a warning advising
# you to add a reference to a newer version of the legacy package?
# TODO: work out what errors/diagnostics we might expect.
Scenario: Add transitive reference and keep new main Rx reference
    Given only the combinations where Before App Dependencies are exactly
        | Dependency |
        | NewRxMain  |
    And only the combinations where After App Dependencies are exactly
        | Dependency    |
        | NewRxMain     |
        | LibUsingOldRx |
    Then at least one matching application dependency combination must exist
    And each combination's TransitiveRxReferenceViaLibrary settings must be one of these combinations, the After settings must cover all of these, and where Before and After both have TransitiveRxReferenceViaLibrary, settings they must be the same
        | Tfms                            | ReferencesNewRxVersion | HasWindowsTargetUsingUiFrameworkSpecificRxFeature |
        | net8.0                          | false                  | false                                             |
        | net8.0;net8.0-windows10.0.19041 | false                  | false                                             |
        | net8.0;net8.0-windows10.0.19041 | false                  | true                                              |
    #And all matching scenarios expect library code to get main Rx types from 'OldRx' 
    #And all matching scenarios expect library code to get UI Rx types from 'OldRx'

# This models the case where the developer stumbled into the preceding scenario, and got
# compiler errors (which only happens when we're looking at a future Rx design that relegates
# System.Reactive to a legacy package) but they then added a reference to the new version of
# the legacy System.Reactive package to fix the compiler errors.
# This fixes the compiler errors, but if the legacy System.Reactive package continues to cause
# a framework reference to Microsoft.WindowsDesktop.App, we now get bloat in self-contained
# deployments.
# TODO: work out what errors/diagnostics we might expect.
Scenario: Add transitive reference, keep new main Rx reference, and add new legacy Rx package reference
    Given only the combinations where Before App Dependencies are exactly
        | Dependency    |
        | NewRxMain  |
    And only the combinations where After App Dependencies are exactly
        | Dependency        |
        | NewRxMain         |
        | LibUsingOldRx     |
        | NewRxLegacyFacade |
    Then at least one matching application dependency combination must exist
    #And each combination's TransitiveRxReferenceViaLibrary settings must be one of these combinations, the After settings must cover all of these, and where Before and After both have TransitiveRxReferenceViaLibrary, settings they must be the same
    #    | Tfms                            | ReferencesNewRxVersion | HasWindowsTargetUsingUiFrameworkSpecificRxFeature |
    #    | net8.0                          | false                  | false                                             |
    #    | net8.0;net8.0-windows10.0.19041 | false                  | false                                             |
    #    | net8.0;net8.0-windows10.0.19041 | false                  | true                                              |
    #And all matching scenarios expect library code to get main Rx types from 'NewRxMain' 
    #And all matching scenarios expect library code to get UI Rx types from 'NewRxLegacyFacade'

# Variation in which instead of adding a reference to the new System.Reactive legacy package, they
# *replace* the System.Reactive.Net package reference with a reference to the new System.Reactive
# legacy package. (People who like to minize their package references may well do this because
# the new System.Reactive legacy package depends on System.Reactive.Net, so you don't really need both)
# TODO: work out what errors/diagnostics we might expect.
Scenario: Add transitive reference, and replace main Rx reference with legacy Rx package reference
    Given only the combinations where Before App Dependencies are exactly
        | Dependency    |
        | NewRxMain  |
    And only the combinations where After App Dependencies are exactly
        | Dependency        |
        | LibUsingOldRx     |
        | NewRxLegacyFacade |
    Then at least one matching application dependency combination must exist
    #And each combination's TransitiveRxReferenceViaLibrary settings must be one of these combinations, the After settings must cover all of these, and where Before and After both have TransitiveRxReferenceViaLibrary, settings they must be the same
    #    | Tfms                            | ReferencesNewRxVersion | HasWindowsTargetUsingUiFrameworkSpecificRxFeature |
    #    | net8.0                          | false                  | false                                             |
    #    | net8.0;net8.0-windows10.0.19041 | false                  | false                                             |
    #    | net8.0;net8.0-windows10.0.19041 | false                  | true                                              |
