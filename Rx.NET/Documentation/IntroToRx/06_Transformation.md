# Transformation of sequences

The values from the sequences we consume are not always in the format we need. Sometimes there is more information than we need, and we need to pick out just the values of interest. Sometimes each value needs to be expanded either into a richer object or into more values.

Up until now, we have looked at creation of sequences, transition into sequences, and, the reduction of sequences by filtering. In this chapter we will look at _transforming_ sequences.

## Select

The most straightforward transformation method is `Select`. It allows you provide a function that takes a value of `TSource` and return a value of `TResult`. The signature for `Select` reflects its ability to transform a sequence's elements from one type to another type, i.e. `IObservable<TSource>` to `IObservable<TResult>`.

```csharp
IObservable<TResult> Select<TSource, TResult>(
    this IObservable<TSource> source, 
    Func<TSource, TResult> selector)
```

You don't have to change the type—`TSource` and `TResult` can be the same if you want. This first example transforms a sequence of integers by adding 3, resulting in another sequence of integers.

```csharp
IObservable<int> source = Observable.Range(0, 5);
source.Select(i => i+3)
      .Dump("+3")
```

This uses the `Dump` extension method we defined at the start of [the Filtering chapter](05_Filtering.md). It produces the following output:

```
+3 --> 3
+3 --> 4
+3 --> 5
+3 --> 6
+3 --> 7
+3 completed
```

This next example transforms values in a way that changes their type. It converts integer values to characters.

```csharp
Observable.Range(1, 5);
          .Select(i => (char)(i + 64))
          .Dump("char");
```

Output:

```
char --> A
char --> B
char --> C
char --> D
char --> E
char completed
```

This example transforms our sequence of integers to a sequence where the elements have an anonymous type:

```csharp
Observable.Range(1, 5)
          .Select(i => new { Number = i, Character = (char)(i + 64) })
          .Dump("anon");
```

Output:

```
anon --> { Number = 1, Character = A }
anon --> { Number = 2, Character = B }
anon --> { Number = 3, Character = C }
anon --> { Number = 4, Character = D }
anon --> { Number = 5, Character = E }
anon completed
```

`Select` is one of the standard LINQ operators supported by C#'s query expression syntax, so we could have written that last example like this:

```csharp
var query = from i in Observable.Range(1, 5)
            select new {Number = i, Character = (char) (i + 64)};

query.Dump("anon");
```

In Rx, `Select` has another overload, in which the `selector` function takes two values. The additional argument is the element's index in the sequence. Use this method if the index of the element in the sequence is important to your selector function.

## SelectMany

Whereas `Select` produces one output for each input, `SelectMany` enables each input element to be transformed into any number of outputs. To see how this can work, let's first look at an example that uses just `Select`:

```csharp
Observable
    .Range(1, 5)
    .Select(i => new string((char)(i+64), i))
    .Dump("strings");
```

which produces this output:

```
strings-->A
strings-->BB
strings-->CCC
strings-->DDDD
strings-->EEEEE
strings completed
```

As you can see, for each of the numbers produced by `Range`, our output contains a string whose length is that many characters. What if, instead of transforming each number into a string, we transformed it into an `IObservable<char>`. We can do that just by adding `.ToObservable()` after constructing the string:

```csharp
Observable
    .Range(1, 5)
    .Select(i => new string((char)(i+64), i).ToObservable())
    .Dump("sequences");
```

(Alternatively, we could have replaced the selection expression with `i => Observable.Repeat((char)(i+64), i)`. Either has exactly the same effect.) The output isn't terribly useful:

```
strings-->System.Reactive.Linq.ObservableImpl.ToObservableRecursive`1[System.Char]
strings-->System.Reactive.Linq.ObservableImpl.ToObservableRecursive`1[System.Char]
strings-->System.Reactive.Linq.ObservableImpl.ToObservableRecursive`1[System.Char]
strings-->System.Reactive.Linq.ObservableImpl.ToObservableRecursive`1[System.Char]
strings-->System.Reactive.Linq.ObservableImpl.ToObservableRecursive`1[System.Char]
strings completed
```

We have an observable sequence of observable sequences. But look at what happens if we now replace that `Select` with a `SelectMany`:

```csharp
Observable
    .Range(1, 5)
    .SelectMany(i => new string((char)(i+64), i).ToObservable())
    .Dump("chars");
```

This gives us an `IObservable<char>`, with this output:

```
chars-->A
chars-->B
chars-->B
chars-->C
chars-->C
chars-->D
chars-->C
chars-->D
chars-->E
chars-->D
chars-->E
chars-->D
chars-->E
chars-->E
chars-->E
chars completed
```

The order has become a little scrambled, but if you look carefully you'll see that the number of occurrences of each letter is the same as when we were emitting strings. There is just one `A`, for example, but `C` appears three times, and `E` five times.

`SelectMany` expects the transformation function to return an `IObservable<T>` for each input, and it then combines the result of those back into a single result. The LINQ to Objects equivalent is a little less chaotic. If you were to run this:

```csharp
Enumerable
    .Range(1, 5)
    .SelectMany(i => new string((char)(i+64), i))
    .ToList()
```

it would produce a list with these elements:

```
[ A, B, B, C, C, C, D, D, D, D, E, E, E, E, E ]
```

The order is less odd. It's worth exploring the reasons for this in a little more detail.

### `IEnumerable<T>` vs. `IObservable<T>` `SelectMany`

`IEnumerable<T>` is pull based—sequences produce elements only when asked. `Enumerable.SelectMany` pulls items from its sources in a very particular order. It begins by asking its source `IEnumerable<int>` (the one returned by `Range` in the preceding example) for the first value. `SelectMany` then invokes our callback, passing this first item, and then enumerates everything in the `IEnumerable<char>` our callback returns. Only when it has exhausted this does it ask the source (`Range`) for a second item. Again, it passes that second item to our callback and then fully enumerates the `IEnumerable<char>`, we return, and so on. So we get everything from the first nested sequence first, then everything from the second, etc.

`Enumerable.SelectMany` is able to proceed in this way for two reasons. First, the pull-based nature of `IEnumerable<T>` enables it to decide on the order in which it processes things. Second, with `IEnumerable<T>` it is normal for operations to block, i.e., not to return until they have something for us. When the preceding example calls `ToList`, it won't return until it has fully populated a `List<T>` with all of the results.

Rx is not like that. First, consumers don't get to tell sources when to produce each item—sources emit items when they are ready to. Second, Rx typically models ongoing processes, so we don't expect method calls to block until they are done. There are some cases where Rx sequences will naturally produce all of their items very quickly and complete as soon as they can, but the kinds of information sources that we tend to want model with Rx typically don't behave that way. So most operations in Rx do not block—they immediately return something (such as an `IObservable<T>`, or an `IDisposable` representing a subscription) and will then produce values later.

The Rx version of the example we're currently examining is in fact one of these unusual cases where each of the sequences emits items as soon as it can. Logically speaking, all of the nested `IObservable<char>` sequences are in progress concurrently. The result is a mess because each of the observable sources here attempts to produce every element as quickly as the source can consume them. The fact that they end up being interleaved has to do with the way these kinds of observable sources use Rx's _scheduler_ system, which we will describe in the [Scheduling and Threading chapter](11_SchedulingAndThreading.md). Schedulers ensure that even when we are modelling logically concurrent processes, the rules of Rx are maintained, and observers of the output of `SelectMany` will only be given one item at a time. The following marble diagram shows the events that lead to the scrambled output we see:

![An Rx marble diagram showing 7 observables. The first illustrates the Range operator producing the values 1 through 5. These are colour coded as follows: green, blue, yellow, red, and pink. These colours correspond to observables further down in the diagram, as will be described shortly. The items in this first observable are not evenly spaced. The 2nd value immediately follows the 1st, but there are gaps before the 3rd, 4th, and 5th items. These gaps correspond with activity shown further down in the diagram. Beneath the first observable is code invoking the SelectMany operator, passing this lambda: "i => new string((char)(i+64),i).ToObservable()". Beneath this are 6 more observables. The first 5 show the individual observables that the lambda produces for each of the inputs. These are colour coded to show how they correspond: the first observable's item is green, to indicate that this observable corresponds to the first item emitted by Range, the second observable's items are blue showing that it corresponds to the second item emitted by Range, and so on with the same colour sequence as described earlier for Range. Each observable's first item is aligned horizontally with the corresponding item from Range, signifying the fact that each one of these observables starts when the Range observable emits a value. These 5 observables show the values produced by the observable returned by the lambda for each of the 5 values from Range. The first of these child observables shows a single item with value 'A', vertically aligned with the value 1 from the Range observable to indicate that this item is produced immediately when Range produces its first value. This child observable then immediately ends, indicating that only one item was produced. The second child observable contains two 'B' values, the third three 'C' values, the fourth four 'D' values and the fifth give 'E' values. The horizontal positioning of these items indicates that all of first 6 observables in the diagram (the Range observable, and the 5 observables produced by the lambda) overlap to some extent. Initially this overlap is minimal: the first of the lambda-produced observables starts at the same time the Range produces its first value so these two observables overlap, but since this first child completes immediately it overlaps with nothing else. The second child starts when Range produces its second value, and manages to produce two values and then completes before anything else happens, so thus far, the child observables produced by the lambda overlap only with the Range one, and not with each other. However, when Range produces its third value, the resulting child observable produces two 'C' values, but the next thing that happens (as denoted by the horizontal position of the items) is that Range manages to produce its 4th value and its corresponding child observable produces the first of its 'D' values next. After this, the third child observable produces its third and final 'C', so this third child overlaps not just with the Range observable, but also with the fourth child. Then the fourth observable produces its second 'D'. Then the Range produces its fifth and final value, and the corresponding child observable produces its first 'E'. Then the fourth and fifth child observable alternate, producing 'D', 'E' and 'D', at which point the fourth child observable is complete, and now the fifth child observable produces its final three 'E' values without interruption, because by this time it is the only observable still running. At the bottom of the diagram is the 7th observable representing the output of the SelectMany. This shows all the of the values from each of the 5 child observables each with the exact same horizontal position (signifying that the observable returned by SelectMany produces a value whenever any of its child observables produces a value). So we can see that this output observable produces the sequence 'ABBCCDCDEDEDEEE', which is exactly what we saw in the example output earlier.](GraphicsIntro/Ch06-Transformation-Marbles-Select-Many-Marbles.svg)

We can make a small tweak to prevent the child sequences all from trying to run at the same time. (This also uses `Observable.Repeat` instead of the rather indirect route of constructing a `string` and then calling `ToObservable` on that. I did that in earlier examples to emphasize the similarity with the LINQ to Objects example, but you wouldn't really do it that way in Rx.)

```csharp
Observable
    .Range(1, 5)
    .SelectMany(i => 
        Observable.Repeat((char)(i+64), i)
                  .Delay(TimeSpan.FromMilliseconds(i * 100)))
    .Dump("chars");
```

Now we get output consistent with the `IEnumerable<T>` version:

```
chars-->A
chars-->B
chars-->B
chars-->C
chars-->C
chars-->C
chars-->D
chars-->D
chars-->D
chars-->D
chars-->E
chars-->E
chars-->E
chars-->E
chars-->E
chars completed
```

This clarifies that `SelectMany` lets you produce a sequence for each item that the source produces, and to have all of the items from all of those new sequences flattened back out into one sequence that contains everything. While that might make it easier to understand, you wouldn't want to introduce this sort of delay in reality purely for the goal of making it easier to understand. These delays mean it will take about a second and a half for all the elements to emerge. This marble diagram shows that the code above produces a sensible-looking ordering by making each child observable produce a little bunch of items, and we've just introduced dead time to get the separation:

![An Rx marble diagram which, like the preceding diagram, shows 7 observables. The first illustrates the Range operator producing the values 1 through 5. These are colour coded as follows: green, blue, yellow, red, and pink. These colours correspond to observables further down in the diagram, as will be described shortly.](GraphicsIntro/Ch06-Transformation-Marbles-Select-Many-Marbles-Delay.svg)

I introduced these gaps purely to provide a slightly less confusing example, but if you really wanted this sort of strictly-in-order handling, you wouldn't use `SelectMany` in this way in practice. For one thing, it's not completely guaranteed to work. (If you try this example, but modify it to use shorter and shorter timespans, eventually you reach a point where the items start getting jumbled up again. And since .NET is not a real-time programming system, there's actually no safe timespan you can specific here that guarantees the ordering.) If you absolutely need all the items from the first child sequence before seeing any from the second, there's actually a robust way to ask for that:

```csharp
Observable
    .Range(1, 5)
    .Select(i => Observable.Repeat((char)(i+64), i))
    .Concat())
    .Dump("chars");
```

However, that would not have been a good way to show what `SelectMany` does, since this no longer uses it. (It uses `Concat`, which will be discussed in the [Combining Sequences](09_CombiningSequences.md) chapter.) We use `SelectMany` either when we know we're unwrapping a single-valued sequence, or when we don't have specific ordering requirements, and want to take elements as and when they emerge from child observables.

### The Significance of SelectMany

If you've been reading this book's chapters in order, you had already seen two examples of `SelectMany` in earlier chapters. The first example in the [**LINQ Operators and Composition** section of chapter 2](02_KeyTypes.md#linq-operators-and-composition) used it. Here's the relevant code:

```csharp
IObservable<int> onoffs =
    from _ in src
    from delta in 
       Observable.Return(1, scheduler)
                 .Concat(Observable.Return(-1, scheduler)
                                   .Delay(minimumInactivityPeriod, scheduler))
    select delta;
```

(If you're wondering where the call to `SelectMany` is in that, remember that if a Query Expression contains two `from` clauses, the C# compiler turns those into a call to `SelectMany`.) This illustrates a common pattern in Rx, which might be described as fanning out, and then back in again.

As you may recall, this example worked by creating a new, short-lived `IObservable<int>` for each item produced by `src`. (These child sequences, represented by the `delta` range variable in the example, produce the value `1`, and then after the specified `minimumActivityPeriod`, they produce `-1`. This enabled us to keep count of the number of recent events emitted.) This is the _fanning out_ part, where items in a source sequence produce new observable sequences. `SelectMany` is crucial in these scenarios because it enables all of those new sequences to be flattened back out into a single output sequence.

The second place I used `SelectMany` was slightly different: it was the final example of the [**Representing Filesystem Events in Rx** section in chapter 3](03_CreatingObservableSequences.md#representing-filesystem-events-in-rx). Although that example also combined multiple observable sources into a single observable, that list of observables was fixed: there was one for each of the different events from `FileSystemWatcher`. It used a different operator `Merge` (which we'll get to in [Combining Sequences](09_CombiningSequences.md)), which was simpler to use in that scenario because you just pass it the list of all the observables you'd like to combine. However, because of a few other things this code wanted to do (including deferred startup, automated disposal, and sharing a single source when multiple subscribers were active), the particular combination of operators used to achieve this meant our merging code that returned an `IObservable<FileSystemEventArgs>`, needed to be invoked as a transforming step. If we'd just used `Select`, the result would have been an `IObservable<IObservable<FileSystemEventArgs>>`. The structure of the code meant that it would only ever produce a single `IObservable<FileSystemEventArgs>`, so the double-wrapped type would be rather inconvenient. `SelectMany` is very useful in these scenarios. If composition of operators has introduced an extra layer of observables-in-observables that you don't want, `SelectMany` can unwrap one layer for you.

These two cases—fanning out then back in, and removing or avoiding a layer of observables of observables—come up quite often, which makes `SelectMany` an important method. (It's not surprising that I was unable to avoid using it in earlier examples.)

As it happens, `SelectMany` is also a particularly important operator in the mathematical theory that Rx is based on. It is a fundamental operator, in the sense that it is possible to build many other Rx operators with it. [Section 'Recreating other operators with `SelectMany`' in Appendix D](D_AlgebraicUnderpinnings.md#recreating-other-operators-with-selectmany) shows how you can implement `Select` and `Where` using `SelectMany`.

## Cast

C#'s type system is not omniscient. Sometimes we might know something about the type of the values emerging from an observable source that is not reflected in that source's type. This might be based on domain-specific knowledge. For example, with the AIS messages broadcast by ships, we might know that if the message type is 3, it will contain navigation information. That means we could write this:

```csharp
IObservable<IVesselNavigation> type3 = 
   receiverHost.Messages.Where(v => v.MessageType == 3)
                        .Cast<IVesselNavigation>();
```

This uses `Cast`, a standard LINQ operator that we can use whenever we know that the items in some collection are of some more specific type than the type system has been able to deduce.

The difference between `Cast` and the [`OfType` operator shown in chapter 5](05_Filtering.md#oftype) is the way in which they handle items that are not of the specified type. `OfType` is a filtering operator, so it just filters out any items that are not of the specified type. But with `Cast` (as with a normal C# cast expression) we are asserting that we expect the source items to be of the specified type, so the observable returned by `Cast` will invoke its subscriber's `OnError` if its source produces an item that is not compatible with the specified type.

This distinction might be easier to see if we recreate the functionality of `Cast` and `OfType` using other more fundamental operators.

```csharp
// source.Cast<int>(); is equivalent to
source.Select(i => (int)i);

// source.OfType<int>();
source.Where(i => i is int).Select(i => (int)i);
```

## Materialize and Dematerialize

The `Materialize` operator transforms a source of `IObservable<T>` into one of type `IObservable<Notification<T>>`. It will provide one `Notification<T>` for each item the source produces, and, if the sourced terminates, it will produce one final `Notification<T>` indicating whether it completed successfully or with an error.

This can be useful because it produces objects that describe a whole sequence. If you wanted to record the output of an observable in a way that could later be replayed...well you'd probably use a `ReplaySubject<T>` because it is designed for precisely that job. But if you wanted to be able to do something other than merely replaying the sequence—inspecting the items or maybe even modifying them before replying, you might want to write your own code to store items. `Notification<T>` can be helpful because it enables you to represent everything a source does in a uniform way. You don't need to store information about whether or how the sequence terminates separately—this information is just the final `Notification<T>`.

You could imagine using this in conjunction with `ToArray` in a unit test. This would enable you to get an array of type `Notification<T>[]` containing a complete description of everything the source did, making it easy to write tests that ask, say, what the third item to emerge from the sequence was. (The Rx.NET source code itself uses `Notification<T>` in many of its tests.)

If we materialize a sequence, we can see the wrapped values being returned.

```csharp
Observable.Range(1, 3)
          .Materialize()
          .Dump("Materialize");
```

Output:

```
Materialize --> OnNext(1)
Materialize --> OnNext(2)
Materialize --> OnNext(3)
Materialize --> OnCompleted()
Materialize completed
```

Note that when the source sequence completes, the materialized sequence produces an 'OnCompleted' notification value and then completes. `Notification<T>` is an abstract class with three implementations:

 * OnNextNotification
 * OnErrorNotification
 * OnCompletedNotification

`Notification<T>` exposes four public properties to help you inspect it: `Kind`, `HasValue`, `Value` and `Exception`. Obviously only `OnNextNotification` will return true for `HasValue` and have a useful implementation of `Value`. Similarly, `OnErrorNotification` is the only implementation that will have a value for `Exception`. The `Kind` property returns an `enum` which allows you to know which methods are appropriate to use.

```csharp
public enum NotificationKind
{
    OnNext,
    OnError,
    OnCompleted,
}
```

In this next example we produce a faulted sequence. Note that the final value of the materialized sequence is an `OnErrorNotification`. Also that the materialized sequence does not error, it completes successfully.

```csharp
var source = new Subject<int>();
source.Materialize()
      .Dump("Materialize");

source.OnNext(1);
source.OnNext(2);
source.OnNext(3);

source.OnError(new Exception("Fail?"));
```

Output:

```
Materialize --> OnNext(1)
Materialize --> OnNext(2)
Materialize --> OnNext(3)
Materialize --> OnError(System.Exception)
Materialize completed
```

Materializing a sequence can be very handy for performing analysis or logging of a sequence. You can unwrap a materialized sequence by applying the `Dematerialize` extension method. The `Dematerialize` will only work on `IObservable<Notification<TSource>>`.

This completes our tour of the transformation operators. Their common characteristic is that they produce an output (or, in the case of `SelectMany`, a set of outputs) for each input item. Next we will look at the operators that can combine information from multiple items in their source.