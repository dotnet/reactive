# Trimming Warnings and `IQbservable<T>`

This ADR describes the approach Rx.NET takes to handling the problems that `IQbservable<T>` creates when using trimming. `IQbservable<T>` relies on .NET expression trees, which are a reflection-heavy mechanism, which can create challenges when using trimming.

## Status

Draft.


## Authors

@idg10 ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/)).


## Context

`IQbservable<T>` is the expression-oriented version of `IObservable<T>`. This is analogous to the way that `IQueryable<T>` is the expression-oriented version of `IEnumerable<T>`. Just as `IQueryable<T>` defines the same LINQ operators that are available for `IEnumerable<T>` but in a form where all lambdas are passed as expression trees (enabling query providers to inspect the structure of queries, and perhaps transform them into something else) `IQbservable<T>` does the same thing in the reactive world.

`System.Reactive` provides just one implementation of `IQbservable<T>`, `System.Reactive.ObservableQuery<T>`. This is an internal type, but you can get hold of this by invoking methods on `Qbservable.Provider`:

```cs
using System.Reactive.Linq;

IQbservable<int> qis = Qbservable.Provider.Range(1, 10);
```

Factory-like operators (i.e. ones that create a sequence out of nothing) such as `Range` are implemented as extension methods for `IQbservableProvider`. Combinator-like operators (i.e. ones that take one or more other sequences as input) such as `Where` are implemented as extension methods for `IQbservable<T>`, meaning that if we've already got an `IQbservable<T>`, we can invoke operators directly on that just like we can with a normal `IObservable<T>` (or like `IEnumerable<T>` for that matter), e.g.:

```cs
qis = qis.Where(i => i % 2 == 0);
```

Whether an operator creates a brand new source from scratch (as in `Range`) or bolts onto an existing sequence (as in `Where`), these `IQbservable<T>` operators are all implemented in much the same way: they call the `IQbservableProvider.CreateQuery<int>` method passing  an `Expression` describing the query. (In the case of combinators like `Where`, they obtain the `IQbservableProvider` from the `Provider` property of the incoming source).

This expression is available as part of `IQbservable<T>` (or more precisely, its base type, `IQbservable`), so we can inspect the expression for an `IQbserable<T>`:

```cs
Console.WriteLine(qis.Expression);
```

That produces this output:

```
value(System.Reactive.ObservableQueryProvider).Range(1, 10).Where(i => ((i % 2) == 0))
```

This states that the query that `qis` currently refers to begins with a specific provider instance of type `System.Reactive.ObservableQueryProvider`. (That is an internal type; it happens to be the type returned by `Qbservable.Provider`.) From there, we invoked the `Range` operator passing the arguments `1` and `10`, and then on the resulting `IQbservable<int>` we invoked the `Where` operator passing the lambda expression shown. (The extra parentheses appear because the code that turns the expression tree representation of a lamdba back into text adds parentheses even in cases where they are not strictly required to avoid ambiguity.)

In theory we could have started from some other implementation of `IQbservableProvider`. (E.g., we could write our own.) But the `Expression` should still come out the same, because all providers are required to make their `Expression` available, and the contents of that expression are actually determined not by the provider itself but by the code in these operator methods. E.g., the `Range` operator looks like this:

```cs
public static IQbservable<int> Range(this IQbservableProvider provider, int start, int count)
{
    if (provider == null)
        throw new ArgumentNullException(nameof(provider));

    return provider.CreateQuery<int>(
        Expression.Call(
            null,
            new Func<IQbservableProvider, int, int, IQbservable<int>>(Range).Method,
            Expression.Constant(provider, typeof(IQbservableProvider)),
            Expression.Constant(start, typeof(int)),
            Expression.Constant(count, typeof(int))
        )
    );
}
```

This doesn't care what provider it uses; it will build up the expression in the same way for any provider. The provider's job here is just to build a new `IQbservable<T>` that holds onto what it has been given. And `Where` is similar:

```cs
public static IQbservable<TSource> Where<TSource>(this IQbservable<TSource> source, Expression<Func<TSource, bool>> predicate)
{
    if (source == null)
        throw new ArgumentNullException(nameof(source));
    if (predicate == null)
        throw new ArgumentNullException(nameof(predicate));

    return source.Provider.CreateQuery<TSource>(
        Expression.Call(
            null,
            new Func<IQbservable<TSource>, Expression<Func<TSource, bool>>, IQbservable<TSource>>(Where<TSource>).Method,
            source.Expression,
            predicate
        )
    );
}
```

What distinguishes one provider from another is what happens when you actually ask for the items the query describes. (This is also true for `IQueryable<T>`: when you use a provider such as EF Core, it will also build up expressions in exactly the same way, and it's only when you actually try to evaluate the query that anything interesting happens.) And we do that by calling `Subscribe`, just like we would with any `IObservable<T>`. (`IQbservable<T>` inherits from `IObservable<T>`.)

The one and only provider that `System.Reactive` supplies (the one we get from `Qbservable.Provider`) does this when you call its `Subscribe` method: it generates code at runtime that executes exactly the same code you would have got if you had started with `Observable` instead of `Qbservable.Provide`. Thus the starting point of the expression and the following factory method (the part of the expression represented as `value(System.Reactive.ObservableQueryProvider).Range(1, 10)`) gets turned into code that invokes `Observable.Range(1, 10)`, and then the call to the `Qbservable.Where` extension method (represented by the `.Where(i => ((i % 2) == 0))` text above) gets turned into a call to `Observable.Where`. It will compile that lambda expression, because `Observable.Where` requires `Func<T, bool>`, and not the `Expression<Func<T, bool>>` that will be present in the `IQbservable.Expression`.

This is analogous to the `IQueryable<T>` implementation you get if you start to enumerate one of those. (Just as `IQbservable<T>` inherits from `IObservable<T>`, so `IQueryable<T>` inherits from `IEnumerable<T>`, so any of the means of getting the elements out of an `IEnumerable<T>` also work for an `IQueryable<T>`. For example, you can write a `foreach` loop.)

With that in mind, let's now look at some of the problems that exist in Rx 6.

### Issue: IL2060 and IL2026 diagnostics

The .NET 10 SDK produces compiler diagnostics relating to trimming. In fact, this happened with earlier SDKs, and we already had pragmas in there to suppress IL2060. This happened because prior to Rx 7, we were using a different way to get the `MethodInfo` argument for the `Expression.Call` method than the code shown above uses. Instead of constructing a `Func`, we were using something like this:

```cs
((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond)),
```
Even with older .NET SDKs, this produced an IL2060 diagnostic. That was due to `MakeGenericMethod`: the analyzer reports that it's "not possible to guarantee the availability of requirements of the generic method". The newer SDKs, this code will also produce an IL2026 diagnostic due to `GetCurrentMethod`, which is now marked with `RequiresUnreferencedCodeAttribute`. Apparently that came in with .NET 7.0 to address https://github.com/dotnet/runtime/issues/53242 and it seems a little OTT to me: the issue says this can "allow reflecting on basically everything in the program." It's not clear to me how asking for the `MethodInfo` only for the calling method means "everything in the program". Sure, you can walk out from this `MethodInfo` to other things, but that's true for more or less any reflection type.

The .NET runtime's own `Queryable` class also needs to retrieve metadata for the current method, and it avoids this problem with a delegate:

```cs
new Func<IQbservable<TFirst>, IObservable<TSecond>, IQbservable<(TFirst First, TSecond Second)>>(SomeMethod).Method
```

This constructs a delegate and uses it purely to be able to retrieve its `MethodInfo`. Since this retrieves the exact same metadata as `MethodInfo.GetCurrentMethod` it's not at all clear why this delegate-based approach is considered to be OK when we're getting the very same `MethodInfo` either way. The description in that issue implies that when you retrieve this with `GetCurrentMethod` the tools discard more of the metadata (e.g. parameter names) than they would when you obtain the same `MethodInfo` through other means.

### Issue: Qbservable methods imply dependency on Observable methods

If you use an `IQbservable<T>` operator, it is likely that this will cause a runtime dependency on the corresponding `IObservable<T>` operator. This is because if you `Subscribe` to any `IQbservable<T>` that Rx created (which will in practice be an instance of `ObservableQuery<T>`) it will use the `ObservableQuery<T>.ObservableRewriter` to convert the `IQbservable<T>`'s expression into the equivalent `IObservable<T>` based implementation, which it then compiles at runtime.

For example, suppose you used the `Where` operator. The `IQbservable<T>` implementation of that is provided as the `Qbservable.Where` extension method, and the compiler will be able to see that you used this, and will therefore know not to trim it. However, when you call `Subscribe`, the expression tree rewriter will convert that into a call to `Observable.Where`, as part of an `Expression<Func<IObservable<T>>>`. It then calls `Compile` on this.

The upshot is that using `Qbservable.Where` may imply that you are also use `Observable.Where`, but indirectly in a way that the IL trimmer might not be able to see directly.

The exact same issue exists with `IQueryable<T>` and LINQ to Objects: you can convert a `IQueryable<T>` into an `IEnumerable<T>`, and this causes runtime code generation that uses the `Enumerable` equivalent of each `Queryable` operator used. The .NET Runtime Libraries source code tells the trimming tooling by adding `DynamicDependency` attributes, e.g.:

```cs
[DynamicDependency("Where`1", typeof(Enumerable))]
public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
```

This is a hint to the IL trimmer that any program that uses the `IQueryable<T>` version of `Where` probably also uses the `Enumerable.Where` method, and so the trimmer should include the latter in any app that uses the former.


### Issue: the generated query-space files were exempt from compiler diagnostics

It appears that simply having the text `.Generated.` in a filename suppresses some compiler diagnostics. We don't want that for the generated query-space code, because we do actually want to know if there are problems. Typically the reason you need to suppress diagnostics in generated code is that there's no way to fix them. But in our case, we control the code generation. (It is done by the HomoIcon tool in this repository.) So we can and should fix any issues rather than suppressing them.


## Decisions

### Replace `GetCurrentMethod`

Since the IL trimming tooling doesn't seem to fully support `GetCurrentMethod`, we have replaced all of the code that used that with code that creates a delegate and returns its `Method` instead. This is the approach used by the .NET Runtime Libraries in `IQueryable<T>` operators, so it seems safest for us to do the same.

### Implied Dynamic Dependency

For the time being we will not be adding `DynamicDependency` attributes because this implies a level of support for dynamic code generation that is not backed up by our tests. We do not want to mislead developers into thinking that this is a fully supported scenario.

If we get requests for better support for scenarios that would be helped by `DynamicDependency`, we will revisit this. (We will most likely also need to add `RequiresDynamicCode` in some places.) Right now, we suspect nobody using `IQbservable<T>` is attempting to do so in conjunction with trimming.

### Change generated filenames for Q-space operators

Since the presence of ".Generated." in a filename seems to stop the trim warnings from appearing, the files generated by the HomoIcon tool now have names such as `Qbservable.Homoicon.cs` instead of `Qbservable.Generated.cs`.


## Consequences

By removing `GetCurrentMethod`, we no longer get the IL2026 warnings, and we've also been able to remove the `#pragma` lines that had been suppressing the IL2060 warnings. Furthermore, since we're now using an approach that appears to be better understood by the trimming tools, it is possible that this will improve the effectiveness of its static analysis around these methods.

Since we are not adding `DynamicDependency` attributes at this time, we continue not to support the use of runtime code generation from `IQueryable<T>` in scenarios where trimming is in use.

By changing the naming convention for files generated by the HomoIcon tool, we will now discover sooner when the code it generates has problems that can be detected by analyzers build into the .NET SDK.