# Ix Release History v7.0


## v7.0.0

Ix.NET's `System.Linq.Async` is being deprecated. Instead, you should use the .NET runtime's [`System.Linq.AsyncEnumerable`](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/asyncenumerable) library (new in .NET 10.0, but available for use on older runtimes). If you are using functionality in `System.Linq.Async` that has not been included in .NET's `System.Linq.AsyncEnumerable`, you can add a reference to `System.Interactive.Async`, which is the new home of this functionality.

Note that `System.Linq.Async` will continue to work. The LINQ implementation has been removed from `System.Linq.Async`'s public-facing API, so when a project upgrades to v7, any code that was previously using the `System.Linq.Async` implementation of LINQ for `IAsyncEnumerable<T>` will now use the `System.Linq.AsyncEnumerable` supplied by .NET 10. (This works even if your project targets older versions of .NET because the new `System.Linq.AsyncEnumerable` library supports these down-level targets, and `System.Linq.Async` supplies a transient reference to it for older targets.)

Binary compatibility is maintained by continuing to supply the full old LINQ implementation in the runtime libraries; we've removed this only from the reference assemblies. This makes `System.Linq.Async`'s deprecated LINQ implementation invisible to the compiler, but it remains available at runtime. (This ensures that if a project upgrades to v7, but is using components that were built against older versions, those components won't encounter exceptions at runtime due to the LINQ implementation not being in the component they expect.)

Note that if a project is using `System.Linq.Async` v6 and upgrades its target framework to .NET 10, it will encounter build errors. The compiler will have access to two implementations of LINQ to `IAsyncEnumerable<T>`, and will report ambiguity errors. The quick fix is to upgrade to v7 of this library. The longer term fix is to remove all references to this library. (This can also happen to projects that target older versions of .NET. If a project using `System.Linq.Async` ends up also acquiring a dependency on `System.Linq.AsyncEnumerable`, again the compiler will report ambiguity errors any place you try to use LINQ for `IAsyncEnumerable<T>`. Again the short term fix is to upgrade to v7 of `System.Linq.Async` and the longer term fix is to stop using it.) We will eventually mark the `System.Linq.Async` package as deprecated on NuGet.

### Why did this move?

The short answer is that Ix.NET was, for many years, plugging a gap in the .NET runtime libraries. The .NET runtime has finally filled in that gap, so it is time for Ix.NET to step out of the way.

Something that is not entirely obvious from the naming is that `System.Linq.Async` never had support from Microsoft. This table may clarify the situation:

| Library | Parent Project | Supported by |
|---|---|---|
| `System.Linq.Async` | Ix.NET | Unfunded community efforts |
| `System.Linq.AsyncEnumerable` | .NET Runtime | Microsoft |


When `IAsyncEnumerable<T>` first moved into the .NET runtime class libraries (in .NET Core 3.0), there was no officially supported LINQ implementation. The `IAsyncEnumerable<T>` interface had originated from the Ix.NET project, which had always supplied a LINQ implementation, so when `IAsyncEnumerable<T>` moved into the .NET runtime libraries, Ix.NET just adjusted its existing LINQ for `IAsyncEnumerable<T>` to work with the interface's new home. Thus `System.Linq.Async` became the unofficial de facto LINQ for `IAsyncEnumerable<T>`. However, there was no official support from Microsoft, and no budget for maintenance. Eventually, Microsoft decided that the .NET runtime libraries really should have a built-in LINQ for `IAsyncEnumerable<T>`, and thus `System.Linq.AsyncEnumerable` was born.

(The .NET runtime class library team decided to define a new library instead of taking over Ix.NET's existing `System.Linq.Async` because class library design guidelines had changed since `System.Linq.Async` was created and it did not align with them.)