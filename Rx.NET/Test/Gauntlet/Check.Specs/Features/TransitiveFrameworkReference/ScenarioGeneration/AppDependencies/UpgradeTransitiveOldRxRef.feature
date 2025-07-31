Feature: Upgrading when Rx dependency acquired through a transitive reference

Background:
    Given I get all the App Dependency combinations for upgrading an old Rx reference acquired transitively


# This corresponds to an application that had happily (perhaps obliviously) been using a package
# that depends on Rx 6, and the app developer now decides to use Rx in the main app, and adds a
# reference to the new main rx package.
# TODO: If System.Reactive is relegated to a legacy facade, we expect this to cause build
#   errors if the main app tries to use Rx directly, because there are now two versions of
#   Rx available: the old via System.Reactive v6, and the new via System.Reactive.Net v7.
#  (Even though the main app had no direct package reference to Rx, it may still have been
#  using it because the developer may just have been relying on the implicit reference.)
# TODO: Hopefully we'll be able to emit a build message suggesting that they want to add a
#   reference to the new System.Reactive (which will resolve the build error).
Scenario: Upgrade transitive ref by adding a reference to new main Rx package
    Given only the combinations where Before App Dependencies are exactly
        | Dependency    |
        | LibUsingOldRx |
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


# This corresponds to a situation where an application had been using a package that depends on
# Rx 6, and wants to upgrade it to use the latest Rx (either because they're getting a
# deprecation warning from the NuGet package manager, or because they are trying to get rid of
# bloat) but doesn't add a reference to the main version.
# TODO: We won't expect build errors with this.
# TODO: We do expect NuGet to identify this as a deprecated package.
Scenario: Upgrade transitive ref by adding a reference to new Rx legacy package
    Given only the combinations where Before App Dependencies are exactly
        | Dependency    |
        | LibUsingOldRx |
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

# This is the 'Add references to both' version, which will typically correspond to what an app
# developer does next after trying the 'Just add a reference to new main Rx package' version and
# getting a build error.
# TODO: We won't expect build errors with this.
Scenario: Upgrade transitive ref by adding references to new Rx main and legacy packages
    Given only the combinations where Before App Dependencies are exactly
        | Dependency    |
        | LibUsingOldRx |
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