# Aggregation

Data is not always tractable in its raw form. Sometimes we need to consolidate, collate, combine or condense the mountains of data we receive. This might just be a case of reducing the volume of data to a manageable level. For example, consider fast moving data from domains like instrumentation, finance, signal processing and operational intelligence. This kind of data can change at a rate of over ten values per second for individual sources, and much higher rates if we're observing multiple sources. Can a person actually consume this? For human consumption, aggregate values like averages, minimums and maximums can be of more use.

We can often achieve more than this. The way in which we combine and correlate may enable us to reveal patterns, providing insights that would not be available from any individual message, or from simple reduction to a single statistical measure. Rx's composability enables us to express complex and subtle computations over streams of data enabling us not just to reduce the volume of messages that users have to deal with, but to increase the amount of value in each message a human receives.

We will start with the simplest aggregation functions, which reduce an observable sequence to a sequence with a single value in some specific way. We then move on to more general-purpose operators that enable you to define your own aggregation mechanisms.

## Simple Numeric Aggregation

Rx supports various standard LINQ operators that reduce all of the values in a sequence down to a single numeric result.

### Count

`Count` tells you how many elements a sequence contains. Although this is a standard LINQ operator, Rx's version deviates from the `IEnumerable<T>` version as Rx will return an observable sequence, not a scalar value. As usual, this is because of the push-related nature of Rx. Rx's `Count` can't demand that its source supply all elements immediately, so it just has to wait until the source says that it has finished. The sequence that `Count` returns will always be of type `IObservable<int>`, regardless of the source's element type. This will do nothing until the source completes, at which point it will emit a single value reporting how many elements the source produced, and then it will in turn immediately complete. This example uses `Count` with `Range`, because `Range` generates all of its values as quickly as possible and then completes, meaning we get a result from `Count` immediately:

```csharp
IObservable<int> numbers = Observable.Range(0,3);
numbers.Count().Dump("count");
```

Output:

```
count-->3
count Completed
```

If you are expecting your sequence to have more values than a 32-bit signed integer can count, you can use the `LongCount` operator instead. This is just the same as `Count` except it returns an `IObservable<long>`.

### Sum

The `Sum` operator adds together all the values in its source, producing the total as its only output. As with `Count`, Rx's `Sum` differs from most other LINQ providers in that it does not produce a scalar as its output. It produces an observable sequence that does nothing until its source completes. When the source completes, the observable returned by `Sum` produces a single value and then immediately completes. This example shows it in use:

```csharp
IObservable<int> numbers = Observable.Range(1,5);
numbers.Sum().Dump("sum");
```

The output shows the single result produced by `Sum`:

```
sum-->15
sum completed
```

`Sum` is only able to work with values of type `int`, `long`, `float`, `double` `decimal`, or the nullable versions of these. This means that there are types you might expect to be able to `Sum` that you can't. For example the `BigInteger` type in the `System.Numerics` namespace represents integer values whose size is limited only by available memory, and how long you're prepared to wait for it to perform calculations. (Even basic arithmetic gets very slow on numbers with millions of digits.) You can use `+` to add these together because the type defines an overload for that operator. But `Sum` has historically had no way to find that. The introduction of [generic math in C# 11.0](https://learn.microsoft.com/en-us/dotnet/standard/generics/math#operator-interfaces) means that it would technically be possible to introduce a version of `Sum` that would work for any type `T` that implemented [`IAdditionOperators<T, T, T>`](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iadditionoperators-3). However, that would mean a dependency on .NET 7.0 or later (because generic math is not available in older versions), and at the time of writing this, Rx supports .NET 7.0 through its `net6.0` target. It could introduce a separate `net7.0` and/or `net8.0` target to enable this, but has not yet done so. (To be fair, [`Sum` in LINQ to Objects also doesn't support this yet](https://github.com/dotnet/runtime/issues/64031).)

If you supply `Sum` with the nullable versions of these types (e.g., your source is an `IObservable<int?>`) then `Sum` will also return a sequence with a nullable item type, and it will produce `null` if any of the input values is `null`.

Although `Sum` can work only with a small, fixed list of numeric types, your source doesn't necessarily have to produce values of those types. `Sum` offers overloads that accept a lambda that extracts a suitable numeric value from each input element. For example, suppose you wanted to answer the following unlikely question: if the next 10 ships that happen to broadcast descriptions of themselves over AIS were put side by side, would they all fit in a channel of some particular width? We could do this by filtering the AIS messages down to those that provide ship size information, using `Take` to collect the next 10 such messages, and then using `Sum`. The Ais.NET library's `IVesselDimensions` interface does not implement addition (and even if it did, we already just saw that Rx wouldn't be able to exploit that), but that's fine: all we need to do is supply a lambda that can take an `IVesselDimensions` and return a value of some numeric type that `Sum` can process:

```csharp
IObservable<IVesselDimensions> vesselDimensions = receiverHost.Messages
    .OfType<IVesselDimensions>();

IObservable<int> totalVesselWidths = vesselDimensions
    .Take(10)
    .Sum(dimensions => 
            checked((int)(dimensions.DimensionToPort + dimensions.DimensionToStarboard)));
```

(If you're wondering what's with cast and the `checked` keyword here, AIS defines these values as unsigned integers, so the Ais.NET library reports them as `uint`, which is not a type Rx's `Sum` supports. In practice, it's very unlikely that a vessel will be wide enough to overflow a 32-bit signed integer, so we just cast it to `int`, and the `checked` keyword will throw an exception in the unlikely event that we encounter ship more than 2.1 billion metres wide.)

### Average

The standard LINQ operator `Average` effectively calculates the value that `Sum` would calculate, and then divides it by the value that `Count` would calculate. And once again, whereas most LINQ implementations would return a scalar, Rx's `Average` produces an observable.

Although `Average` can process values of the same numeric types as `Sum`, the output type will be different in some cases. If the source is `IObservable<int>`, or if you use one of the overloads that takes a lambda that extracts the value from the source, and that lambda returns an `int`, the result will be a `double.` This is because the average of a set of whole numbers is not necessarily a whole number. Likewise, averaging `long` values produces a `double`. However, inputs of type `decimal` produce outputs of type `decimal`, and likewise `float` inputs produce a `float` output.

As with `Sum`, if the inputs to `Average` are nullable, the output will be too.

### Min and Max

Rx implements the standard LINQ `Min` and `Max` operators which find the element with the highest or lowest value. As with all the other operators in this section, these do not return scalars, and instead return an `IObservable<T>` that produces a single value.

Rx defines specialized implementations for the same numeric types that `Sum` and `Average` support. However, unlike those operators it also defines an overload that will accept source items of any type. When you use `Min` or `Max` on a source type where Rx does not define a specialized implementation, it uses [`Comparer<T>.Default`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.comparer-1.default) to compare items. There is also an overload enabling you to pass a comparer.

As with `Sum` and `Average` there are overloads that accept a callback. If you use these overloads, `Min` and `Max` will invoke this callback for each source item, and will look for the lowest or highest value that your callback returns. Note that the single output they eventually produce will be a value returned by the callback, and not the original source item from which that value was derived. To see what that means, look at this example:

```csharp
IObservable<int> widthOfWidestVessel = vesselDimensions
    .Take(10)
    .Max(dimensions => 
            checked((int)(dimensions.DimensionToPort + dimensions.DimensionToStarboard)));
```

`Max` returns an `IObservable<int>` here, which will be the width of the widest vessel out of the next 10 messages that report vessel dimensions. But what if you didn't just want to see the width? What if you wanted the whole message?

## MinBy and MaxBy

Rx offers two subtle variations on `Min` and `Max`: `MinBy` and `MaxBy`. These are similar to the callback-based `Min` and `Max` we just saw that enable us to work with sequences of elements that are not numeric values, but which may have numeric properties. The difference is that instead of returning the minimum or maximum value, `MinBy` and `MaxBy` tell you which source element produced that value. For example, suppose that instead of just discovering the width of the widest ship, we wanted to know what ship that actually was:

```csharp
IObservable<IVesselDimensions> widthOfWidestVessel = vesselDimensions
    .Take(10)
    .MaxBy(dimensions => 
              checked((int)(dimensions.DimensionToPort + dimensions.DimensionToStarboard)));
```

This is very similar to the example in the preceding section. We're working with a sequence where the element type is `IVesselDimensions`, so we've supplied a callback that extracts the value we want to use for comparison purposes. (The same callback as last time, in fact.) Just like `Max`, `MaxBy` is trying to work out which element produces the highest value when passed to this callback. It can't know which that is until the source completes. If the source hasn't completed yet, all it can know is the highest _yet_, but that might be exceeded by a future value. So as with all the other operators we've looked at in this chapter, this produces nothing until the source completes, which is why I've put a `Take(10)` in there.

However, the type of sequence we get is different. `Max` returned an `IObservable<int>`, because it invokes the callback for every item in the source, and then produces the highest of the values that our callback returned. But with `MaxBy`, we get back an `IObservable<IVesselDimensions>` because `MaxBy` tells us which source element produced that value.

Of course, there might be more than one item that has the highest width—there might be three equally large ships, for example. With `Max` this doesn't matter because it's only trying to return the actual value: it doesn't matter how many source items had the maximum value, because it's the same value in all cases. But with `MaxBy`  we get back the 
original items that produce the maximum, and if there were three that all did this, we wouldn't want Rx to pick just one arbitrarily.

So unlike the other aggregation operators we've seen so far, an observable returned by `MinBy` or `MaxBy` doesn't necessarily produce just a single value. It might produce several. You might ask whether it really is an aggregation operator, since it's not reducing the input sequence to one output. But it is reducing it to a single value: the minimum (or maximum) returned by the callback. It's just that it presents the result slightly differently. It produces a sequence based on the result of the aggregation process. You could think of it as a combination of aggregation and filtering: it performs aggregation to determine the minimum or maximum, and then filters the source sequence down just to the elements for which the callback produces that value.

**Note**: LINQ to Objects also defines [`MinBy`](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.minby) and [`MaxBy`](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.maxby) methods, but they are slightly different. These LINQ to Objects versions do in fact arbitrarily pick a single source element—if there are multiple source values all producing the minimum (or maximum) result, LINQ to Objects gives you just one whereas Rx gives you all of them. Rx defined its versions of these operators years before .NET 6.0 added their LINQ to Objects namesakes, so if you're wondering why Rx does it differently, the real question is why did LINQ to Objects not follow Rx's precedent.

## Simple Boolean Aggregation

LINQ defines several standard operators that reduce entire sequences to a single boolean value.

### Any

The `Any` operator has two forms. The parameterless overload effectively asks the question "are there any elements in this sequence?" It returns an observable sequence that will produce a single value of `false` if the source completes without emitting any values. If the source does produce a value however, then when the first value is produced, the result sequence will immediately produce `true` and then complete. If the first notification it gets is an error, then it will pass that error on.

```csharp
var subject = new Subject<int>();
subject.Subscribe(Console.WriteLine, () => Console.WriteLine("Subject completed"));
var any = subject.Any();

any.Subscribe(b => Console.WriteLine("The subject has any values? {0}", b));

subject.OnNext(1);
subject.OnCompleted();
```

Output:

```
1
The subject has any values? True
subject completed
```

If we now remove the OnNext(1), the output will change to the following

```
subject completed
The subject has any values? False
```

In the case where the source does produce a value, `Any` immediately unsubscribes from it. So if the source wants to report an error, `Any` will only see this if that is the first notification it produces.

```csharp
var subject = new Subject<int>();
subject.Subscribe(Console.WriteLine,
    ex => Console.WriteLine("subject OnError : {0}", ex),
    () => Console.WriteLine("Subject completed"));

IObservable<bool> any = subject.Any();

any.Subscribe(b => Console.WriteLine("The subject has any values? {0}", b),
    ex => Console.WriteLine(".Any() OnError : {0}", ex),
    () => Console.WriteLine(".Any() completed"));

subject.OnError(new Exception());
```

Output:

```
subject OnError : System.Exception: Exception of type 'System.Exception' was thrown.
.Any() OnError : System.Exception: Exception of type 'System.Exception' was thrown.
```

But if the source were to generate a value before an exception, e.g.:

```csharp
subject.OnNext(42);
subject.OnError(new Exception());
```

we'd see this output instead:

```
42
The subject has any values? True
.Any() completed
subject OnError : System.Exception: Exception of type 'System.Exception' was thrown.
```

Although the handler that subscribed directly to the source subject still sees the error, our `any` observable reported a value of `True` and then completed, meaning it did not report the error that followed.

The `Any` method also has an overload that takes a predicate. This effectively asks a slightly different question: "are there any elements in this sequence that meet these criteria?" The effect is similar to using `Where` followed by the no-arguments form of `Any`.

```csharp
IObservable<bool> any = subject.Any(i => i > 2);
// Functionally equivalent to 
IObservable<bool> longWindedAny = subject.Where(i => i > 2).Any();
```

### All

The `All` operator is similar to the `Any` method that takes a predicate, except that all values must satisfy the predicate. As soon as the predicate rejects a value, the observable returned by `All` produces a `false` value and then completes. If the source reaches its end without producing any elements that do not satisfy the predicate, then `All` will push `true` as its value. (A consequence of this is that if you use `All` on an empty sequence, the result will be a sequence that produces `true`. This is consistent with how `All` works in other LINQ providers, but it might be surprising for anyone not familiar with the formal logic convention known as [vacuous truth](https://en.wikipedia.org/wiki/Vacuous_truth).)

Once `All` decides to produce a `false` value, it immediately unsubscribes from the source (just like `Any` does as soon as it determines that it can produce `true`.) If the source produces an error before this happens, the error will be passed along to the subscriber of the `All` method. 

```csharp
var subject = new Subject<int>();
subject.Subscribe(Console.WriteLine, () => Console.WriteLine("Subject completed"));
IEnumerable<bool> all = subject.All(i => i < 5);
all.Subscribe(b => Console.WriteLine($"All values less than 5? {b}"));

subject.OnNext(1);
subject.OnNext(2);
subject.OnNext(6);
subject.OnNext(2);
subject.OnNext(1);
subject.OnCompleted();
```

Output:

```
1
2
6
All values less than 5? False
all completed
2
1
subject completed
```

### IsEmpty

The LINQ `IsEmpty` operator is logically the opposite of the no-arguments `Any` method. It returns `true` if and only if the source completes without producing any elements. If the source produces an item, `IsEmpty` produces `false` and immediately unsubscribes. If the source produces an error, this forwards that error.

### Contains

The `Contains` operator determines whether a particular element is present in a sequence. You could implement it using `Any`, just supplying a callback that compares each item with the value you're looking for. However, it will typically be slightly more succinct, and may be a more direct expression of intent to write `Contains`.

```csharp
var subject = new Subject<int>();
subject.Subscribe(
    Console.WriteLine, 
    () => Console.WriteLine("Subject completed"));

IEnumerable<bool> contains = subject.Contains(2);

contains.Subscribe(
    b => Console.WriteLine("Contains the value 2? {0}", b),
    () => Console.WriteLine("contains completed"));

subject.OnNext(1);
subject.OnNext(2);
subject.OnNext(3);
    
subject.OnCompleted();
```

Output:

```
1
2
Contains the value 2? True
contains completed
3
Subject completed
```

There is also an overload to `Contains` that allows you to specify an implementation of `IEqualityComparer<T>` other than the default for the type. This can be useful if you have a sequence of custom types that may have some special rules for equality depending on the use case.

## Build your own aggregations

If the built-in aggregations described in the preceding sections do not meet your needs, you can build your own. Rx provides two different ways to do this.

### Aggregate

The `Aggregate` method is very flexible: it is possible to build any of the operators shown so far in this chapter with it. You supply it with a function, and it invokes that function once for every element. But it doesn't just pass the element into your function: it also provides a way for your function to _aggregate_ information. As well as the current element, it also passes in an _accumulator_. The accumulator can be any type you like—it will depend on what sort of information you're looking to accumulate. Whatever value your function returns becomes the new accumulator value, and it will pass that into the function along with the next element from the source. There are a few variations on this, but the simplest overload looks like this:

```csharp
IObservable<TSource> Aggregate<TSource>(
    this IObservable<TSource> source, 
    Func<TSource, TSource, TSource> accumulator)
```

If you wanted to produce your own version of `Count` for `int` values, you could do so by providing a function that just adds 1 for each value the source produces:

```csharp
IObservable<int> sum = source.Aggregate((acc, element) => acc + 1);
```

To understand exactly what this is doing, let's look at how `Aggregate` will call this lambda. To make that slightly easier to see, suppose we put that lambda in its own variable:

```csharp
Func<int, int, int> c = (acc, element) => acc + 1;
```

Now suppose the source produces an item with the value 100. `Aggregate` will invoke our function:

```csharp
c(0, 100) // returns 1
```

The first argument is the current accumulator. `Aggregate` has used `default(int)` for the initial accumulator value, which is 0. Our function returns 1, which becomes the new accumulator value. So if the source produces a second value, say, 200, `Aggregate` will pass the new accumulator, along with the second value from the source:

```csharp
c(1, 200) // returns 2
```

This particular function completely ignores its second argument (the element from the source). It just adds one to the accumulator each time. So the accumulator is nothing more than a record of the number of times our function has been called.

Now let's look at how we might implement `Sum` using `Aggregate`:

```csharp
Func<int, int, int> s = (acc, element) => acc + element
IObservable<int> sum = source.Aggregate(s);
```

For the first element, `Aggregate` will again pass the default value for our chosen accumulator type, `int`: 0. And it will pass the first element value. So again if the first element is 100 it does this:

```csharp
s(0, 100) // returns 100
```

And then if the second element is 200, `Aggregate` will make this call:

```csharp
s(100, 200) // returns 300
```

Notice that this time, the first argument was 100, because that's what the previous invocation of `s` returned. So in this case, after seeing elements 100 and 200, the accumulator's value is 300, which is the sum of all the elements.

What if we want the initial accumulator value to be something other than `default(TAccumulator)`? There's an overload for that. For example, here's how we could implement something like `All` with `Aggregate`:

```csharp
IObservable<bool> all = source.Aggregate(true, (acc, element) => acc && element);
```

This isn't exactly equivalent to the real `All` by the way: it handles errors differently. `All` instantly unsubscribes from its source if it sees a single element that is `false`, because it knows that nothing else the source produces can possibly change the outcome. That means that if the source had been about to produce an error, it will no longer have the opportunity to do so because `All` unsubscribed. But `Aggregate` has no way of knowing that the accumulator has entered a state from which it can never leave, so it will remain subscribed to the source until the source completes (or until whichever code subscribed to the `IObservable<T>` returned by `Aggregate` unsubscribes). This means that if the source were to produce `true`, then `false`, `Aggregate` would, unlike `All`, remain subscribed to the source, so if the source goes on to call `OnError`, `Aggregate` will receive that error, and pass it on to its subscriber.

Here's a way to think about `Aggregate` that some people find helpful. If your source produces the values 1 through 5, and if the function we pass to `Aggregate` is called `f`, then the value that `Aggregate` produces once the source completes will be this:

```csharp
T result = f(f(f(f(f(default(T), 1), 2), 3), 4), 5);
```

So in the case of our recreation of `Count`, the accumulator type was `int`, so that becomes:

```csharp
int sum = s(s(s(s(s(0, 1), 2), 3), 4), 5);
// Note: Aggregate doesn't return this directly -
// it returns an IObservable<int> that produces this value.
```

Rx's `Aggregate` doesn't perform all those invocations at once: it invokes the function each time the source produces an element, so the calculations will be spread out over time. If your callback is a _pure function_—one that is unaffected by global variables and other environmental factors, and which will always return the same result for any particular input—this doesn't matter. The result of `Aggregate` will be the same as if it had all happened in one big expression like the preceding example. But if your callback's behaviour is affected by, say, a global variable, or by the current contents of the filesystem, then the fact that it will be invoked when the source produces each value may be more significant.

`Aggregate` has other names in some programming systems by the way. Some systems call it `reduce`. It is also often referred to as a _fold_. (Specifically a _left fold_. A right fold proceeds in reverse. Conventionally its function takes arguments in the reverse order, so it would look like `s(1, s(2, s(3, s(4, s(5, 0)))))`. Rx does not offer a built-in right fold. It would not be a natural fit because it would have to wait until it received the final element before it could begin, meaning it would need to hold onto every element in the entire sequence, and then evaluate the entire fold at once when the sequence completes.)

You might have spotted that in my quest to re-implement some of the built-in aggregation operators, I went straight from `Sum` to `Any`. What about `Average`? It turns out we can't do that with the overloads I've shown you so far. And that's because `Average` needs to accumulate two pieces of information—the running total and the count—and it also needs to perform once final step right at the end: it needs to divide the total by the count. With the overloads shown so far, we can only get part way there:

```csharp
IObservable<int> nums = Observable.Range(1, 5);

IObservable<(int Count, int Sum)> avgAcc = nums.Aggregate(
    (Count: 0, Sum: 0),
    (acc, element) => (Count: acc.Count + 1, Sum: acc.Sum + element));
```

This uses a tuple as the accumulator, enabling it to accumulate two values: the count and the sum. But the final accumulator value becomes the result, and that's not what we want. We're missing that final step that calculates the average by dividing the sum by the count. Fortunately, `Aggregate` offers a 3rd overload that enables us to provide this final step. We pass a second callback which will be invoked just once when the source completes. `Aggregate` passes the final accumulator value into this lambda, and then whatever it returns becomes the single item produced by the observable that `Aggregate` returns.

```csharp
IObservable<double> avg = nums.Aggregate(
    (Count: 0, Sum: 0),
    (acc, element) => (Count: acc.Count + 1, Sum: acc.Sum + element),
    acc => ((double) acc.Sum) / acc.Count);
```

I've been showing how `Aggregate` can re-implement some of the built-in aggregation operators to illustrate that it is a powerful and very general operator. However, that's not what we use it for. `Aggregate` is useful precisely because it lets us define custom aggregation.

For example, suppose I wanted to build up a list of the names of all the ships that have broadcast their details over AIS. Here's one way to do that:

```csharp
IObservable<IReadOnlySet<string>> allNames = vesselNames
    .Take(10)
    .Aggregate(
        ImmutableHashSet<string>.Empty,
        (set, name) => set.Add(name.VesselName));
```

I've used `ImmutableHashSet<string>` here because its usage patterns happen to fit `Aggregate` neatly. An ordinary `HashSet<string>` would also have worked, but is a little less convenient because its `Add` method doesn't return the set, so our function needs an extra statement to return the accumulated set:

```csharp
IObservable<IReadOnlySet<string>> allNames = vesselNames
    .Take(10)
    .Aggregate(
        new HashSet<string>(),
        (set, name) =>
        {
            set.Add(name.VesselName);
            return set;
        });
```

With either of these implementations, `vesselNames` will produce a single value that is a `IReadOnlySet<string>` containing each vessel name seen in the first 10 messages that report a name.

I've had to fudge an issue in these last two examples. I've made them work over just the first 10 suitable messages to emerge. Think about what would happen if I didn't have the `Take(10)` in there. The code would compile, but we'd have a problem. The AIS message source I've been using in various examples is an endless source. Ships will continue to move around the oceans for the foreseeable future. Ais.NET does not contain any code designed to detect either the end of civilisation, or the invention of technologies that will render the use of ships obsolete, so it will never call `OnCompleted` on its subscribers. The observable returned by `Aggregate` reports nothing until its source either completes or fails. So if we remove that `Take(10)`, the behaviour would be identical `Observable.Never<IReadOnlySet<string>>`. I had to force the input to `Aggregate` to come to an end to make it produce something. But there is another way.

### Scan

While `Aggregate` allows us to reduce complete sequences to a single, final value, sometimes this is not what we need. If we are dealing with an endless source, we might want something more like a running total, updated each time we receive a value. The `Scan` operator is designed for exactly this requirement. The signatures for both `Scan` and `Aggregate` are the same; the difference is that `Scan` doesn't wait for the end of its input. It produces the aggregated value after every item.

We can use this to build up a set of vessel names as in the preceding section, but with `Scan` we don't have to wait until the end. This will report the current list every time it receives a message containing a name:

```csharp
IObservable<IReadOnlySet<string>> allNames = vesselNames
    .Scan(
        ImmutableHashSet<string>.Empty,
        (set, name) => set.Add(name.VesselName));
```

Note that this `allNames` observable will produce elements even if nothing has changed. If the accumulated set of names already contained the name that just emerged from `vesselNames`, the call to `set.Add` will do nothing, because that name will already be in the set. But `Scan` scan produces one output for each input, and doesn't care if the accumulator didn't change. Whether this matters will depend on what you are planning to do with this `allNames` observable, but if you need to, you can fix this easily with the [`DistinctUntilChanged` operator shown in chapter 5](05_Filtering.md#distinct-and-distinctuntilchanged).

You could think of `Scan` as being a version of `Aggregate` that shows its working. If we wanted to see how the process of calculating an average aggregates the count and sum, we could write this:

```csharp
IObservable<int> nums = Observable.Range(1, 5);

IObservable<(int Count, int Sum)> avgAcc = nums.Scan(
    (Count: 0, Sum: 0),
    (acc, element) => (Count: acc.Count + 1, Sum: acc.Sum + element));

avgAcc.Dump("acc");
```

That produces this output:

```
acc-->(1, 1)
acc-->(2, 3)
acc-->(3, 6)
acc-->(4, 10)
acc-->(5, 15)
acc completed
```

You can see clearly here that `Scan` is emitting the current accumulated values each time the source produces a value.

Unlike `Aggregate`, `Scan` doesn't offer an overload taking a second function to transform the accumulator into the result. So we can see the tuple containing the count and sum here, but not the actual average value we want. But we can achieve that by using the [`Select`](06_Transformation.md#select) operator described in the [Transformation chapter](06_Transformation.md):

```csharp
IObservable<double> avg = nums.Scan(
    (Count: 0, Sum: 0),
    (acc, element) => (Count: acc.Count + 1, Sum: acc.Sum + element))
    .Select(acc => ((double) acc.Sum) / acc.Count);

avg.Dump("avg");
```

Now we get this output:

```
avg-->1
avg-->1.5
avg-->2
avg-->2.5
avg-->3
avg completed
```

`Scan` is a more generalised operator than `Aggregate`. You could implement `Aggregate` by combining `Scan` with the [`TakeLast()` operator described in the Filtering chapter](05_Filtering.md#takelast).

```csharp
source.Aggregate(0, (acc, current) => acc + current);
// is equivalent to 
source.Scan(0, (acc, current) => acc + current).TakeLast();
```

Aggregation is useful for reducing volumes of data or combining multiple elements to produce averages, or other measures that incorporate information from multiple elements. But to perform some kinds of analysis we will also need to slice up or otherwise restructure our data before calculating aggregated values. So in the next chapter we'll look at the various mechanisms Rx offers for partitioning data.
