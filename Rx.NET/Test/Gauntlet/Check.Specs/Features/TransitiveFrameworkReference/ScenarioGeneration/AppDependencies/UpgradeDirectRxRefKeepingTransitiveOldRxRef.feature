Feature: Upgrading direct Rx dependency when old dependency also exists through a transitive reference

Background:
    Given I get all the App Dependency combinations for upgrading a direct old Rx reference when an old transitive reference also exists


# This corresponds to an application that had happily been Rx 6 directly, and was also
# (possibly oblibiously) using a package that depends on Rx 6,. Having discovered that
# System.Reactive is now deprecated, the app developer replaces the System.Reactive v6
# reference with a reference to the new main rx package.
# TODO: work out what errors/diagnostics we might expect.
Scenario: Upgrade as reference to new main Rx package
    Given only the combinations where Before App Dependencies are exactly
        | Dependency    |
        | LibUsingOldRx |
        | OldRx         |
    And only the combinations where After App Dependencies are exactly
        | Dependency    |
        | LibUsingOldRx |
        | NewRxMain     |
    Then at least one matching application dependency combination must exist
    And each combination's TransitiveRxReferenceViaLibrary settings must be one of these combinations, the After settings must cover all of these, and where Before and After both have TransitiveRxReferenceViaLibrary, settings they must be the same
        | Tfms                            | ReferencesNewRxVersion | HasWindowsTargetUsingUiFrameworkSpecificRxFeature |
        | net8.0                          | false                  | false                                             |
        | net8.0;net8.0-windows10.0.19041 | false                  | false                                             |
        | net8.0;net8.0-windows10.0.19041 | false                  | true                                              |
    #And all matching scenarios expect library code to get main Rx types from 'OldRx' 
    #And all matching scenarios expect library code to get UI Rx types from 'OldRx'

# TODO:
#   Rx should cause legacy package update suggestion (for designs where System.Reactive is a legacy package)
#   Should be able to suppress legacy package update suggestion
#   Presence of and ability to suppress NuGet legacy package warning?

# TODO:
#   Scenarios should express expectation around 'two Rx versions' scenario


# This corresponds to an application that had happily been Rx 6 directly, and was also
# (possibly oblibiously) using a package that depends on Rx 6, but the developer wants
# to upgrade it to use the latest Rx (either because they're getting a deprecation
# warning from the NuGet package manager, or because they are trying to get rid of
# bloat). Unlike the preceding scenario, in which they replace the package reference with
# one to the new main package (which is normally what we'll want people do to), in this case
# they just upgrade to the latest System.Reactive.
# TODO: work out what errors/diagnostics we might expect.
Scenario: Upgrade as reference to new Rx legacy package
    Given only the combinations where Before App Dependencies are exactly
        | Dependency    |
        | LibUsingOldRx |
        | OldRx         |
    And only the combinations where After App Dependencies are exactly
        | Dependency        |
        | LibUsingOldRx     |
        | NewRxLegacyFacade |
    Then at least one matching application dependency combination must exist
    And each combination's TransitiveRxReferenceViaLibrary settings must be one of these combinations, the After settings must cover all of these, and where Before and After both have TransitiveRxReferenceViaLibrary, settings they must be the same
        | Tfms                            | ReferencesNewRxVersion | HasWindowsTargetUsingUiFrameworkSpecificRxFeature |
        | net8.0                          | false                  | false                                             |
        | net8.0;net8.0-windows10.0.19041 | false                  | false                                             |
        | net8.0;net8.0-windows10.0.19041 | false                  | true                                              |
    #And all matching scenarios expect library code to get main Rx types from 'NewRxMain' 
    #And all matching scenarios expect library code to get UI Rx types from 'NewRxLegacyFacade'

# This corresponds to an application that had happily been Rx 6 directly, and was also
# (possibly oblibiously) using a package that depends on Rx 6, but the developer wants
# to upgrade it to use the latest Rx (either because they're getting a deprecation
# warning from the NuGet package manager, or because they are trying to get rid of
# bloat). In this case, they upgrade the existing System.Reactive reference *and* add a
# reference to the new main Rx package. The might have ended up in this situation because
# they initially tried replacing System.Reactive with the latest main package, but ran into
# the two-Rx-versions problem, and the Rx package generated a message suggesting they also
# add the System.Reactive package.
# TODO: work out what errors/diagnostics we might expect.
Scenario: Upgrade transitive ref by adding references to new Rx main and legacy packages
    Given only the combinations where Before App Dependencies are exactly
        | Dependency    |
        | LibUsingOldRx |
        | OldRx         |
    And only the combinations where After App Dependencies are exactly
        | Dependency        |
        | LibUsingOldRx     |
        | NewRxMain         |
        | NewRxLegacyFacade |
    Then at least one matching application dependency combination must exist
    And each combination's TransitiveRxReferenceViaLibrary settings must be one of these combinations, the After settings must cover all of these, and where Before and After both have TransitiveRxReferenceViaLibrary, settings they must be the same
        | Tfms                            | ReferencesNewRxVersion | HasWindowsTargetUsingUiFrameworkSpecificRxFeature |
        | net8.0                          | false                  | false                                             |
        | net8.0;net8.0-windows10.0.19041 | false                  | false                                             |
        | net8.0;net8.0-windows10.0.19041 | false                  | true                                              |

    
##Scenario: 
##    Given only the scenarios where Before App Dependencies include a library that references old Rx
##    And only the scenarios where the Before App Dependencies reference old Rx directly
##
### group by?
##Scenario: Windows-specific and non-OS-specific TFMs
##    #Then the Before App Dependencies contain the following distinct TFM lists
##    #| net8.0                          |
##    #| net8.0-windows10.0.19041        |
##    #| net8.0;net8.0-windows10.0.19041 |
##    #And each combination whose TFM list includes a windows-specific TFM offers the f