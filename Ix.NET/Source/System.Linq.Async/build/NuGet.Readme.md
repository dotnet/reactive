# Legacy LINQ for `IAsyncEnumerable<T>`

You should no longer use this package. Use the .NET 10 runtime libraries' `System.Linq.AsyncEnumerable` instead. (You don't need to be using .NET 10 to use that new package—it works on older runtimes.)

If you were relying on functionality from this package that has not been implemented in the new `System.Linq.AsyncEnumerable`, you will need to add a reference to the Ix.NET project's `System.Interactive.Async` package, which is the new home for these features.