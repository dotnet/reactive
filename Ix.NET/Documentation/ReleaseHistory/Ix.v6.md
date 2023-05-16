# Ix Release History v6.0


## v6.0.1

First release with version number updated to v6.0.x. (At the time, Rx and Ix were attempting to follow a policy of keeping version numbers aligned with the .NET runtime libraries.)

Added `MinByWithTies` and `MaxByWithTies` to reinstate functionality that was lost in v5.1. (When .NET 6.0 added its own MinBy/MaxBy, Ix v5.1 removed its methods, but some of those did things the .NET 6.0 versions can't.)

