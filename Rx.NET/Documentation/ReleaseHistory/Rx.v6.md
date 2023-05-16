# Rx Release History v6.0

## v6.0.0-preview

### Breaking changes

* Out-of-support target frameworks (.NET Core 3.1, .NET 5) removed

### New features

* Tested against .NET 6, and .NET 7
* When unhandled exceptions from `Task` used to cause `TaskScheduler.UnobservedExceptions`, applications can opt into swallowing failures silently (to be consistent with how Rx has always handled unhandled exceptions in the equivalent non-Task-oriented scenarios; this only applies to cases in which Rx has no way of reporting the exception, typically because the relevant observable no longer has any subscribers on which we could call `OnError`)
* `SingleAssignmentDisposableValue` type is now public