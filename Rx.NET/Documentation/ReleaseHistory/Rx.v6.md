# Rx Release History v6.0

## v6.0.2

This release fixes:

* [Issue #2173: "RefCount with MinObservers > 1 Behaves Unexpectedly"](https://github.com/dotnet/reactive/issues/2173)
* [Issue #2214: "RefCount with disconnect delay and minObservers throws InvalidOperationException when resubscribing to synchronously completing source"](https://github.com/dotnet/reactive/issues/2214)
* [Issue #2215: "RefCount with disconnect delay and minObservers disconnects prematurely if observer count drops to zero and then goes back up"](https://github.com/dotnet/reactive/issues/2215)

Note: the test suite now tests on .NET 9.0. No changes were required as a result of this.

## v6.0.1

This release fixes:

* [Issue #1942: "AOT compilation produces single trim analysis warning"](https://github.com/dotnet/reactive/issues/1942)
* [Issue #2005: "Observable completion exhibits O(n^2) behavior in GroupBy+[SelectMany/Merge] in the number of groups"](https://github.com/dotnet/reactive/issues/2005)

Note: the test suite now tests on .NET 8.0. No changes were required as a result of this.

## v6.0.0

### Breaking changes

* Out-of-support target frameworks (.NET Core 3.1, .NET 5) removed
* Minimum target platform for UWP apps raised from 10.0.16299.0 to 10.0.18362.0

### New features

* Tested against .NET 6, and .NET 7
* When unhandled exceptions from `Task` used to cause `TaskScheduler.UnobservedExceptions`, applications can opt into swallowing failures silently (to be consistent with how Rx has always handled unhandled exceptions in the equivalent non-Task-oriented scenarios; this only applies to cases in which Rx has no way of reporting the exception, typically because the relevant observable no longer has any subscribers on which we could call `OnError`)
* `SingleAssignmentDisposableValue` type is now public
* Trimming annotations now present
* debug symbols now available as separate `snupkg` instead of being built in, reducing assembly size