# Ix Release History v5.0 and Older

In addition to the original `System.Interactive` library, and the `System.Interactive.Async` library that added the same functionality for `IAsyncEnumerable<T>`, the Ix.NET project also includes `System.Linq.Async`, which defines for `IAsyncEnumerable<T>` the same LINQ operators that are built into the .NET runtime libraries for `IEnumerable<T>`.

## V5.1

Removed various `IEnumerable<T>` min and max extension methods because .NET 6.0 now has built-in equivalents.

Fixed bug causing duplicate emissions from `Delay`.


## V5.0

`System.Linq.Async` adds support for C# 8.0's nullable reference types feature.


## v4.0

Ix Async 4.0 has a breaking change from prior versions due to `IAsyncEnumerable<T>` being added to the .NET runtime libraries. In earlier versions, Ix Async defined its own version of this interface, but v4.0 has been modified to use the definition now built into the runtime. This enables the `IAsyncEnumerable` implementations in Ix Async to be consumed by the [async streams](https://github.com/dotnet/roslyn/blob/master/docs/features/async-streams.md) language feature that was added in C# 8. This means for .NET Standard 2.1 and .NET Core 3 targets, we use the in-box interfaces for `IAsyncEnumerable<T>` and friends. On other platforms, we use the `IAsyncEnumerable` definition from `Microsoft.Bcl.AsyncInterfaces`, supplying a full implementation of the Rx-like LINQ operators Ix has long defined for `IEnumerable<T>`. The types will unify to the system ones where the platform provides it.

The .NET runtime libraries did not add a full LINQ to Objects implementation for `IAsyncEnumerable<T>`. Whereas `IEnumerable<T>` offers standard operators such as `Where`, `Single`, and `GroupBy`, enabling use of the LINQ query expression syntax, this is not available out of the box with .NET for `IAsyncEnumerable<T>`. Since earlier versions of this library had already done most of the relevant work to implement these operators on its pre-v4.0 version of `IAsyncEnumerable<T>`, v4.0 of Ix also builds the `System.Linq.Async` library, making LINQ to Objects available for `IAsyncEnumerable<T>`.