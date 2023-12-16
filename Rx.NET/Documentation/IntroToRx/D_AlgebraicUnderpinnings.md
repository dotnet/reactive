# Appendix D: Rx's Algebraic Underpinnings

Rx operators can be combined together in more or less any way you can imagine, and they generally combine without any problems. The fact that this works is not merely a happy accident. In general, integration between software components is often one of the largest sources of pain in software development, so the fact that it works so well is remarkable. This is in large part thanks to the fact that Rx relies on some underlying theory. Rx has been designed so that you don't need to know these details to use it, but curious developers typically want to know these things.

The earlier sections of the book have already talked about one formal aspect of Rx: the contract between observable sources and their observables. There is a clearly defined grammar for what constitutes acceptable use of `IObserver<T>`. This goes beyond what the .NET type system is able to enforce, so we are reliant on code doing the right thing. However, the `System.Reactive` library does always adhere to this contract, and it also has some guard types in place that detect when application code has not quite played by the rules, and to prevent this from wreaking havoc.

The `IObserver<T>` grammar is important. Components rely on it to ensure correct operation. Consider the `Where` operator, for example. It provides its own `IObserver<T>` implementation with which it subscribes to the underlying source. This receives items from that source, and then decides which to forward to the observer that subscribed to the `IObservable<T>` presented by `Where`. You could imagine it looking something like this:

```csharp
public class OverSimplifiedWhereObserver<T> : IObserver<T>
{
    private IObserver<T> downstreamSubscriber;
    private readonly Func<T, bool> predicate;

    public OverSimplifiedWhereObserver(
        IObserver<T> downstreamSubscriber, Func<T, bool> predicate)
    {
        this.downstreamSubscriber = downstreamSubscriber;
        this.predicate = predicate;
    }

    public void OnNext(T value)
    {
        if (this.predicate(value))
        {
            this.downstreamSubscriber.OnNext(value);
        }
    }

    public void OnCompleted()
    {
        this.downstreamSubscriber.OnCompleted();
    }

    public void OnError(Exception x)
    {
        this.downstreamSubscriber.OnCompleted(x);
    }
}
```

This does not take any explicit steps to follow the `IObserver<T>` grammar. It doesn't need to if the source to which it is subscribes also obeys those rules. Since this only ever calls its subscriber's `OnNext` in its own `OnNext`, and likewise for `OnCompleted` and `OnError`, then as long as the underlying source to which this operator is subscribed obeys the rules for how to call those three methods, this class will in turn also follow those rules automatically.

In fact, `System.Reactive` is not quite that trusting. It does have some code that detects certain violations of the grammar, but even these measures just ensure that the grammar is adhered to once execution enters Rx. There are some checks at the boundaries of the system, but Rx's innards rely heavily on the fact that upstream sources will abide by the rules.

However, the grammar for `IObservable<T>` is not the only place where Rx relies on formalism to ensure correct operation. It also depends on a particular set of mathematical concepts:

* Monads
* Catamorphisms
* Anamorphisms

Standard LINQ operators can be expressed purely in terms of these three ideas.

These concepts come from [category theory](https://en.wikipedia.org/wiki/Category_theory), a pretty abstract branch of mathematics concerned with mathematical structures. In the late 1980s, a few computer scientists were exploring this area of maths with a view to using them to model the behaviour of programs. [Eugenio Moggi](https://en.wikipedia.org/wiki/Eugenio_Moggi) (an Italian computer scientist who was, at the time, working at the University of Edinburgh) is generally credited for realising that monads in particular are well suited to describing computations, as his 1991 paper, [Notions of computations and monads](https://person.dibris.unige.it/moggi-eugenio/ftp/ic91.pdf) explains. These theoretical ideas and were incorporated into the Haskell programming language, primarily by Philip Wadler and Simon Peyton Jones, who published a proposal for [monadic handling of IO](https://www.microsoft.com/en-us/research/wp-content/uploads/1993/01/imperative.pdf) in 1992. By 1996, this had been fully incorporated into Haskell in its v1.3 release to enable programs' handling of input and output (e.g., handling user input, or writing data to files) to work in a way that was underpinned by strong mathematical foundations. This has widely been recognized as a significant improvement on Haskell's earlier attempts to model the messy realities of IO in a purely functional language.

Why does any of this matter? These mathematical foundations are exactly why LINQ operators can be freely composed.

The mathematical discipline of category theory has developed a very deep understanding of various mathematical structures, and one of the most useful upshots for programmers is that it offers certain rules which, if followed, can ensure that software elements will behave well when combined together. This is, admittedly, a rather hand-wavey explanation. If you'd like a detailed explanation of exactly how category theory can be applied to programming, and why it is useful to do so, I can highly recommend [Bartosz Milewski's 'Category Theory for Programmers'](https://bartoszmilewski.com/2014/10/28/category-theory-for-programmers-the-preface/). The sheer volume of information available there should make it clear why I'm not about to attempt a full explanation in this appendix. Instead, my goal is just to outline the basic concepts, and explain how they correspond to features of Rx.

## Monads

Monads are the most important mathematical concept underpinning LINQ's (and therefore Rx's) design. It's not necessary to have the faintest idea of what a monad is to be able to use Rx. The most important fact is that their mathematical characteristics (and in particular, their support for composition) are what enable Rx operators to combine together freely. From a practical perspective, all that really matters is that it just works, but if you've read this far, that probably won't satisfy you.

It is often hard to describe precisely what mathematical objects really are, because they are inherently abstract. So before I get to the definition of a monad, it may be helpful to understand how LINQ uses this concept. LINQ treats a monad as a general purpose representation of a container of items. As developers, we know that there are many kinds of things that can contain items. There are arrays, and other collection types such as `IList<T>`. There are also databases, and although there are many ways in which a database table is quite different from an array, there are also some ways in which they are similar. The basic insight underpinning LINQ is that there is a mathematical abstraction that captures the essence of what containers have in common. If we determine that some .NET type represents a monad, then all of the work that mathematicians have done over the years to understand the characteristics and behaviours of monads will be applicable to that .NET type.

For example, `IEnumerable<T>` is a monad, as is `IQueryable<T>`. And crucially for Rx, `IObservable<T>` is as well. LINQ's design relies on the properties of monads, so if you can determine that some .NET type is a monad, then it is a candidate for a LINQ implementation. (Conversely, if you try to create a LINQ provider for a type that is not a monad, you are likely to have problems.)

So what are these characteristics that LINQ relies on? The first relates directly to containment: it must be possible to take some value and put it inside your monad. You'll notice that all the examples I've given so far are generic types, and that's no coincidence: monads are essentially type constructors, and the type argument indicates the kind of thing you want the monad to contain. So given some value of type `T`, it must be possible to wrap that in a monad for that type. Given an `int` we can get an `IEnumerable<int>`, and if we couldn't do that, `IEnumerable<T>` would not be monadic. The second characteristic is slightly harder to pin down without getting lost in high abstraction, but it essentially boils down to the idea that if we have functions that we can apply to individual contained items, and if those functions compose in useful ways, we can create new functions that operate not on individual values but on the containers, and crucially, those functions can also be composed in the same ways.

This enables us to work with entire containers as freely as we can work with individual values.

### The monadic operations: return and bind

We've just seen that monads aren't just a type. They need to supply certain operations. This first operation, the ability to wrap a value in the monad, is sometimes called _unit_ in mathematical texts, but in a computing context it is more often known as _return_. This is how [`Observable.Return`](03_CreatingObservableSequences.md#observablereturn) got its name.

There doesn't technically need to be an actual function. The monadic laws are satisfied as long as some mechanism is available to put a value into the monad. For example, unlike `Observable`, the `Enumerable` type does _not_ define a `Return` method, but it doesn't matter. You can just write `new[] { value }`, and that's enough.

Monads are required to provide just one other operation. The mathematical literature calls it _bind_, some programming systems call it `flatMap`, and LINQ refers to it as `SelectMany`. This is the one that tends to cause the most head scratching, because although it has a clear formal definition, it's harder to say what it really does than with _return_. However, we're looking at monads through their ability to represent containers, and this offers a fairly straightforward way to understand bind/`SelectMany`: it lets us take a container where every item is a nested container (e.g., an array of arrays, or an `IEnumerable<IEnumerable<T>>`) and flatten it out. For example, a list of lists would become one list, containing every item from every list. As we'll soon see, this is not obviously related to the formal mathematical definition of bind, which is altogether more abstract, but it is compatible with it, which is all that's needed for us to enjoy the fruits of the mathematicians' labours.

Critically, to qualify as a monad, the two operations just described (return and bind) must conform to certain rules, or _laws_ as they are often described in the literature. There are three laws. All of them govern how the bind operation works, and two of these are concerned with how return and bind interact with one another. These laws are the foundation of the composability of operations based on monads. The laws are somewhat abstract, so it isn't exactly obvious _why_ they enable this, but they are non-negotiable. If your type and operations don't follow these laws, then you don't have a monad, so you can't rely on the characteristics monads guarantee.

So what does bind actually look like? Here's how it looks for `IEnumerable<T>`:

```csharp
public static IEnumerable<TResult> SelectMany<TSource, TResult> (
    this IEnumerable<TSource> source,
    Func<TSource,IEnumerable<TResult>> selector);
```

So it is a function that takes two inputs. The first is an `IEnumerable<TSource>`. The second input is itself a function which, when supplied with a `TSource` produces an `IEnumerable<TResult>`. And when you invoke `SelectMany` (aka _bind_) with these two arguments, you get back an `IEnumerable<TResult>`. Although formal definition of bind requires it to have this shape, it doesn't dictate any particular behaviour—anything that conforms to the laws is acceptable. But in the context of LINQ, we do expect a specific behaviour: this will invoke the function (the second argument) once for every `TSource` in the source enumerable (the first argument), and then collect all of the `TResult` values produced by all of the `IEnumerable<TResult>` collections returned by all of the invocations of that function, wrapping them as a one big `IEnumerable<TResult>`. In this specific case of `IEnumerable<T>` we could describe `SelectMany` as getting one output collection for each input value, and then concatenating all of those output collections.

But we've now got a little too specific. Even if we're looking specifically at LINQ's use of monads to represent generalised containers, `SelectMany` doesn't necessarily entail concatenation. It merely requires that the container returned by `SelectMany` contains all of the items produced by the function. Concatenation is one strategy, but Rx does something different. Since observables tend to produce values as and when they want to, the `IObservable<TResult>` returned by `Observable.SelectMany` just produces a value each time any of the individual per-`TSource` `IObservable<TResult>`s produced by the function produces a value. (It performs some synchronization to ensure that it follows Rx's rules for calls into `IObserver<T>`, so if one of these observables produces a value while a call to the subscriber's `OnNext` is in progress, it will wait for that to return before pushing the next value. But other than that, it just pushes all values straight through.) So the source values are essentially interleaved here, instead of being concatenated. But the broader principle—that the result is a container with every value produced by the callback for the individual inputs—applies.

The mathematical definition of a monadic bind has the same essential shape, it just doesn't dictate a particular behaviour. So any monad will have a bind operation that takes two inputs: an instance of the monadic type constructed for some input value type (`TSource`), and a function that takes a `TSource` as its input and produces an instance of the monadic type constructed for some output value type (`TResult`). When you invoke bind with these two inputs the result is an instance of the monadic type constructed for the output value type. We can't precisely represent this general idea in C#'s type system, but this sort of gives the broad flavour:

```csharp
// An impressionistic sketch of the general form of a monadic bind
public static M<TResult> SelectMany<TSource, TResult> (
    this M<TSource> source,
    Func<TSource, M<TResult>> selector);
```

Substitute your chosen monadic type (`IObservable<T>`, `IEnumerable<T>`, `IQueryable<T>`, or whatever) for `M<T>`, and that tells you what bind should look like for that particular type.

But it's not enough to provide the two functions, return and bind. Not only must they have the correct shape, they must also abide by the laws.

### The monadic laws

So a monad consists of a type constructor (e.g., `IObservable<T>`) and two functions, `Return` and `SelectMany`. (From now on I'm just going to use these LINQy names.) But to qualify as a monad, these  features must abide by three "laws" (given in a very compact form here, which I'll explain in the following sections):

1. `Return` is a 'left-identity' for `SelectMany`
2. `Return` is a 'right-identity' for `SelectMany`
3. `SelectMany` should be, in effect, associative

Let's look at each of these in a bit more detail

#### Monadic law 1: `Return` is a 'left-identity' for `SelectMany`

This law means that if you pass some value `x` into `Return` and then pass the result as one of the inputs to `SelectMany` where the other input is a function `SomeFunc`, then the result should be identical to just passing `x` directly into `SomeFunc`. For example:

```csharp
// Given a function like this:
//  IObservable<bool> SomeFunc(int)
// then these two should be identical.
IObservable<bool> o1 = Observable.Return(42).SelectMany(SomeFunc);
IObservable<bool> o2 = SomeFunc(42);
```

Here's an informal way to understand this. `SelectMany` pushes every item in its input container through `SomeFunc`, and each such call produces a container of type `IObservable<bool>`, and it collects all these containers together into one big `IObservable<bool>` that contains items from all of the individual `IObservable<bool>` containers. But in this example, the input we provide to `SelectMany` contains just a single item, meaning that there's no collection work to be done. `SelectMany` is going to invoke our function just once with that one and only input, and that's going to produce just one output `IObservable<bool>`. `SelectMany` is obliged to return an `IObservable<bool>` that contains everything in the single `IObservable<bool>` it got from that single call to `SomeFunc`. There's no actual further processing for it to do in this case. Since there was only one call to `SomeFunc` it doesn't need to combine items from multiple containers in this case: that single output produced by the single call to `SomeFunc` contains everything that should be in the container that `SelectMany` is going to return. We can therefore just invoke `SomeFunc` directly with the single input item.

It would be odd if `SelectMany` did anything else. If `o1` were different in some way, that would mean one of three things:

* `o1` would contain items that aren't in `o2` (meaning it had somehow included items _not_ produced by `SomeFunc`)
* `o2` would contain items that aren't in `o1` (meaning that `SelectMany` had omitted some of the items produced by `SomeFunc`)
* `o1` and `o2` contain the same items but are different in some detectable sense specific to the monad type in use (e.g., the items came out in a different order)

So this law essentially formalizes the idea that `SelectMany` shouldn't add or remove items, or fail to preserve characteristics that the monad in use would normally preserve such as ordering. (Note that in .NET LINQ providers, this doesn't generally require these to be exactly the same objects. They normally won't be. It just means that they must represent exactly the same thing. For example, in this case `o1` and `o2` are both `IEnumerable<bool>`, so it means they should each produce exactly the same sequence of `bool` values.)

#### Monadic law 2: `Return` is a 'left-identity' for `SelectMany`

This law means that if you pass `Return` as the function input to `SelectMany`, and then pass some value of the constructed monadic type in as the other argument, you should get that same value as the output. For example:

```csharp
// These two should be identical.
IObservable<int> o1 = GetAnySource();
IObservable<int> o2 = o1.SelectMany(Observable.Return);
```

By using `Return` as the function for `SelectMany`, we are essentially asking to take every item in the input container and to wrap it in its very own container (`Return` wraps a single item) and then to flatten all of those containers back out into a single container. We are adding a layer of wrapping and then removing it again, so it makes sense that this should have no effect.

#### Monadic law 3: `SelectMany` should be, in effect, associative

Suppose we have two functions, `Tx1` and `Tx2`, each of a form suitable for passing as the argument to `SelectMany`. There are two ways we could apply these:

```csharp
// These two should be identical.
IObservable<int> o1 = source.SelectMany(x => Tx1(x).SelectMany(Tx2));
IObservable<int> o2 = source.SelectMany(x => Tx1(x)).SelectMany(Tx2);
```

The difference here is just a slight change in the placements of the parentheses: all that changes is whether the call to `SelectMany` on the right-hand side is invoked inside the function passed to the other `SelectMany`, or it is invoked on the result of the other `SelectMany`. This next example adjusts the layout, and also replaces the lambda `x => Tx1(x)` with the exactly equivalent `Tx1`, which might make the difference in structure a bit easier to see:

```csharp
IObservable<int> o1 = source
    .SelectMany(x => Tx1(x).SelectMany(Tx2));
IObservable<int> o2 = source
    .SelectMany(Tx1)
    .SelectMany(Tx2);
```

The third law says that either of these should have the same effect. It shouldn't matter whether the second `SelectMany` call (for `Tx2`) happens "inside" or after the first `SelectMany` call.

An informal way to think about this is that `SelectMany` effectively applies two operations: a transformation and an unwrap. The transformation is defined by whatever function you pass to `SelectMany`, but because that function returns the monad type (in LINQ terms it returns a container which may contain any number of items) `SelectMany` unwraps each container returned when it passes an item to the function, in order to collect all the items together into the single container it ultimately returns. When you nest this sort of operation, it doesn't matter which order that unwrapping occurs in. For example, consider these functions:

```csharp
IObservable<int> Tx1(int i) => Observable.Range(1, i);
IObservable<string> Tx2(int i) => Observable.Return(i.ToString());
```

The first converts a number into a range of numbers of the same length. `1` becomes `[1]`, `3` becomes `[1,2,3]` and so on. Before we get to `SelectMany`, imagine what will happen if we use this with `Select` on an observable source that produces a range of numbers:

```csharp
IObservable<int> input = Observable.Range(1, 3); // [1,2,3]
IObservable<IObservable<int>> expandTx1 = input.Select(Tx1);
```

We get a sequence of sequences. `expand2` is effectively this:

```
[
    [1],
    [1,2],
    [1,2,3],
]
```

If instead we had used `SelectMany`:

```csharp
IObservable<int> expandTx1Collect = input.SelectMany(Tx1);
```

it would apply the same transformation, but then flatten the results back out into a single list:

```
[
    1,
    1,2,
    1,2,3,
]
```

I've kept the line breaks to emphasize the connection between this and the preceding output, but I could just have written `[1,1,2,1,2,3]`.

If we then want to apply the second transform, we could use `Select`:

```csharp
IObservable<IObservable<string>> expandTx1CollectExpandTx2 = expandTx1Collect
    .SelectMany(Tx1)
    .Select(Tx2);
```

This passes each number in `expandTx1Collect` to `Tx2`, which converts it into a sequence containing a single string:

```
[
    ["1"],
    ["1"],["2"],
    ["1"],["2"],["3"]
]
```

But if we use `SelectMany` on that final position too:

```csharp
IObservable<string> expandTx1CollectExpandTx2Collect = expandTx1Collect
    .SelectMany(Tx1)
    .SelectMany(Tx2);
```

it flattens these back out into just the strings:

```
[
    "1",
    "1","2",
    "1","2","3"
]
```

The associative-like requirement says it shouldn't matter if we apply `Tx1` inside the function passed to the first `SelectMany` instead of applying it to the result of that first `SelectMany`. So instead of starting with this:

```csharp
IObservable<IObservable<int>> expandTx1 = input.Select(Tx1);
```

we might write this:

```csharp
IObservable<IObservable<IObservable<string>>> expandTx1ExpandTx2 =
    input.Select(x => Tx1(x).Select(Tx2));
```

That's going to produce this:

```
[
    [["1"]],
    [["1"],["2"]],
    [["1"],["2"],["3"]]
]
```

If we change that to use `SelectMany` for the nested call:

```csharp
IObservable<IObservable<string>> expandTx1ExpandTx2Collect =
    input.Select(x => Tx1(x).SelectMany(Tx2));
```

That's going to flatten out the inner items (but we're still using `Select` on the outside, so we still get a list of lists) producing this:

```
[
    ["1"],
    ["1","2"],
    ["1","2","3"]
]
```

And then if we change that first `Select` to `SelectMany`:

```csharp
IObservable<string> expandTx1ExpandTx2CollectCollect =
    input.SelectMany(x => Tx1(x).SelectMany(Tx2));
```

it will flatten that outer layer of lists, giving us:

```
[
    "1",
    "1","2",
    "1","2","3"
]
```

That's the same final result we got earlier, as the 3rd monad law requires.

To summarize, the two processes here were:

* expand and transform Tx1, flatten, expand and transform Tx2, flatten
* expand and transform Tx1, expand and transform Tx2, flatten, flatten

Both of these apply both transforms, and flatten out the extra layers of containment added by these transforms, and so although the intermediate steps looked different, we ended up with the same result, because it doesn't matter whether you unwrap after each transform, or you perform both transforms before unwrapping.

#### Why these laws matter

These three laws directly reflect laws that hold true for composition of straightforward functions over numbers. If we have two functions, $f$, and $g$, we could write a new function $h$, defined as $g(f(x))$. This way of combining function is called _composition_, and is often written as $g \circ f$. If the identity function is called $id$, then the following statements are true:

* $id \circ f$ is equivalent to just $f$
* $f \circ id$ is equivalent to just $f$
* $(f \circ g) \circ s$ is equivalent to $f \circ (g \circ s)$

These correspond directly to the three monad laws. Informally speaking, this reflects the fact that the monadic bind operation (`SelectMany`) has deep structurally similarity to function composition. This is why we can combine LINQ operators together freely.

### Recreating other operators with `SelectMany`

Remember that there are three mathematical concepts at the heart of LINQ: monads, anamorphisms and catamorphisms. So although the preceding discussion has focused on `SelectMany`, the significance is much wider because we can express other standard LINQ operators in terms of these primitives. For example, this shows how we could implement [`Where`](05_Filtering.md#where) using just `Return` and `SelectMany`:

```csharp
public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, bool> predicate)
{
    return source.SelectMany(item =>
        predicate(item)
            ? Observable.Return(item)
            : Observable.Empty<T>());
}
```

This implements `Select`:

```csharp
public static IObservable<TResult> Select<TSource, TResult>(
    this IObservable<TSource> source, Func<TSource, TResult> f)
{
    return source.SelectMany(item => Observable.Return(f(item)));
}
```

Some operators require anamorphisms or catamorphisms, so let's look at those now.

## Catamorphisms

A catamorphism is essentially a generalization of any kind of processing that takes every item in a container into account. In practice in LINQ, this typically means processes that inspect all of the values, and produce a single value as a result, such as [Observable.Sum](07_Aggregation.md#sum). More generally, aggregation of any kind constitutes catamorphism. The mathematical definition of catamorphism is more general than this—it doesn't necessarily have to reduce things all the way down to a single value for example—but for the purposes of understanding LINQ, this container-oriented viewpoint is the most straightforward way to think about this construct.

Catamorphisms are one of the fundamental building blocks of LINQ because you can't construct catamorphisms out of the other elements. But there are numerous LINQ operators that can be built out of LINQ's most elemental catamorphism, the [`Aggregate`](07_Aggregation.md#aggregate) operator. For example, here's one way to implement `Count` in terms of `Aggregate`:

```csharp
public static IObservable<int> MyCount<T>(this IObservable<T> items)
    => items.Aggregate(0, (total, _) => total + 1);
```

We could implement `Sum` thus:

```csharp
public static IObservable<T> MySum<T>(this IObservable<T> items)
    where T : INumber<T>
    => items.Aggregate(T.Zero, (total, x) => x + total);
```

This is more flexible than the similar sum example I showed in the [Aggregation chapter](07_Aggregation.md), because that worked only with an `IObservable<int>`. Here I'm using the _generic math_ feature added in C# 11.0 and .NET 7.0 to enable `MySum` to work across any number-like type. But the basic principle of operation is the same.

If you came here for the theory, it probably won't be enough for you just to see that the various aggregating operators are all special cases of `Aggregate`. What really is a catamorphism? One definition is as "the unique homomorphism from an initial algebra into some other algebra" but as is typical with category theory, that's one of those explanations that's easiest to understand if you already understand the concepts it's trying to describe. If you try to understand this description in terms of the school mathematics form of algebra, in which we write equations where some values are represented by letters, it's hard to make sense of this definition. That's because catamorphisms take a much more general view of what constitutes "algebra," meaning essentially some system by which expressions of some kind can be constructed and evaluated.

To be more precise, Catamorphisms are described in relation to something called an F-algebra. That's a combination of three things:

1. a Functor, _F_, that defines some sort of structure over some category _C_
2. some object _A_ in the category _C_
3. a morphism from _F A_ to _A_ that effectively evaluates the structure

But that opens up more questions than it answers. So let's start with the obvious one: what's a Functor? From a LINQ perspective, it's essentially anything that implements `Select`. (Some programming systems call this `fmap`.) From our container-oriented viewpoint it's two things: 1) a type constructor that is container-like (e.g. something like `IEnumerable<T>` or `IObservable<T>`) and 2) some means of applying a function to everything in the container. So if you have a function that converts from `string` to `int`, a Functor lets you apply that to everything it contains in a single step.

The combination of `IEnumerable<T>` and its `Select` extension method is a Functor. You can use `Select` to convert an `IEnumerable<string>` to an `IEnumerable<int>`. `IObservable<T>` and its `Select` form another Functor, and we can use these to get from an `IObservable<string>` to an `IObservable<int>`. What about that "over some category _C_" part? That alludes to the fact that the mathematical description of a Functor is rather broader. When developers use category theory, we generally stick to a category that represents types (as in programming language types like `int`) and functions. (Strictly speaking a Functor maps from one category to another, so in the most general case, a Functor maps objects and morphisms in some category _C_ into objects and morphisms in some category _D_. But for programming purposes, we are always using the category representing types, so for the Functors we use _C_ and _D_ will be the same thing. Strictly speaking this means we should be calling them Endofunctors, but nobody seems to bother. In practice we use the name for the more general form, Functor, and it's just taken as read that we mean an Endofunctor over the category of types and functions.)

So, that's the Functor part. Let's move onto 2, "some object _A_ in the category _C_." Well _C_ is the Functor's category, and we just established that objects in that category are types, so _A_ here might be the `string` type. If our chosen Functor is the combination of `IObservable<T>` and its `Select` method, then _F A_ would be `IObservable<string>`.

So what about the "morphisms" in 3? Again, for our purposes we're just using Endofunctors over types and functions, so in this context, morphisms are just functions. So we could recast the definition of an F-algebra in more familiar terms as:

1. some container-like generic type such as `IObservable<T>`
2. an item type `A` (e.g., `string`, or `int`)
3. a function that takes an `IObservable<A>` and returns a value of type `A` (e.g. `Observable.Aggregate<A>`)

This is a good deal more specific. Category theory is typically concerned with capturing the most general truths about mathematical structures, and this reformulation throws that generality away. However, from the perspective of a programmer looking to lean on mathematical theory, this is fine. As long as what we're doing fits the F-algebra mould, all the general results that mathematicians have derived will apply to our more specialized application of the theory.

Nonetheless, to give you an idea of the sorts of things the general concept of F-algebras can enable, it's possible for the Functor to be a type that represents expressions in a programming language, and you could create an F-algebra that evaluates those expressions. That's a similar idea to LINQ's `Aggregate`, in that it walks over the entire structure represented by the Functor (every element in a list if it's an `IEnumerable<T>`; every subexpression if you're representing an expression) and reduces the whole thing to a single value, but instead of our Functor representing a sequence of things, it has a more complex structure: expressions in some programming language.

So that's an F-algebra. And from a theory point of view, it's important that the third part doesn't necessarily have to reduce things. Theoretically, the types can be recursive, with the item type _A_ being _F A_. (This is important for inherently recursive structures such as expressions.) And there is typically a maximally general F-algebra in which the function (or morphism) in 3 only deals with the structure, and which doesn't actually perform any reduction at all. (E.g., given some expression syntax, you could imagine code that embodies all of the knowledge required to walk through every single subexpression of an expression, but which has no particular opinion on what processing to apply.) The idea of a catamorphism is that there are less other F-algebras available for the same Functor that are less general.

For example, with `IObservable<T>` the general purpose notion is that every item produced by some source can be processed by repeatedly applying some function of two arguments, one of which is a value of type `T` from the container, and the other of which is some sort of accumulator, representing all information aggregated so far. And this function would return the updated accumulator, ready to be passed into the function again along with the next `T`. And then there are more specific forms in which specific accumulation logic (e.g., summation, or determination of a maximum value) is applied. Technically, the catamorphism here is the connection from the general form to the more specialized form. But in practice it's common to refer to the specific specialized forms (such as [`Sum`](07_Aggregation.md#sum) or [`Average`](07_Aggregation.md#average)) as catamorphisms.

### Remaining inside the container

Although in general a catamorphism can strip off the container (e.g., `Sum` for `IEnumerable<int>` produces an `int`), this isn't absolutely necessary, and with Rx most catamorphisms don't do this. As described in the threading and scheduling chapter's [Lock-ups](11_SchedulingAndThreading.md#lock-ups) section, blocking some thread while waiting for a result that will only occur once an `IObservable<T>` has done something in particular (e.g., if you want to calculate the sum of items, you have to wait until you've seen all the items) is a recipe for deadlock in practice.

For this reason, most of the catamorphisms perform some sort of reduction but continue to produce a result wrapped in an `IObservable<T>`.

## Anamorphisms

Anamorphisms are, roughly speaking, the opposite of catamorphisms. While catamorphisms essentially collapse some sort of structure down to something simpler, an anamorphism expands some input into a more complex structure. For example, given some number (e.g., 5) we could imagine a mechanism for turning that into a sequence with the specified number of elements in it (e.g., [0,1,2,3,4]).

In fact we don't have to imagine such a thing: that's what [`Observable.Range`](03_CreatingObservableSequences.md#observablerange) does.

We could think of the monadic `Return` operation as a very simple anamorphism. Given some value of type `T`, [`Observable.Return`](03_CreatingObservableSequences.md#observablereturn) expands this into an `IObservable<T>`. Anamorphisms are essentially the generalization of this sort of idea.

The mathematical definition of an anamorphism is "the assignment of a coalgebra to its unique morphism to the final coalgebra of an endofunctor." This is the "dual" of the definition of a catamorphism, which from a category theory point of view essentially means that you reverse the direction of all of the morphisms. In our not-completely-general application of category theory, the morphisms in question here are the reduction of items to some output in a catamorphism, and so with an anamorphism this turns into the expansion of some value into the some instance of the container type (e.g., from an `int` to an `IObservable<int>`).

I'm not going to go into as much detail as with catamorphisms. Instead, I'm going to point out the key part at the heart of this: the most general F-algebra for a Functor embodies some understanding of the essential structure of the Functor, and catamorphisms make use of that to define various reductions. Similarly, the most general coalgebra for a Functor also embodies some understanding of the essential structure of the Functor and anamorphisms make use of that to define various expansions.

[`Observable.Generate`](03_CreatingObservableSequences.md#observablegenerate) represents this most general capability: it has the capability to produce an `IObservable<T>` but needs to be supplied with some specialized expansion function to generate any particular observable.

## So much for theory

Now we've reviewed the theoretical concepts behind LINQ, let's step back and look at how we use them. We have three kinds of operations:

* Anamorphisms enter the sequence: `T1 --> IObservable<T2>`
* Bind modifies the sequence. `IObservable<T1> --> IObservable<T2>`
* Catamorphisms leave the sequence. Logically `IObservable<T1> --> T2`, but in practice typically `IObservable<T1> --> IObservable<T2>` where the output observable produces just a single value

As an aside, bind and catamorphism were made famous by Google's [MapReduce](http://en.wikipedia.org/wiki/MapReduce) framework from Google. Here Google, refer to Bind and Catamorphism by names more commonly used in some functional languages, Map and Reduce.

Most Rx operators are actually specializations of the higher order functional concepts. To give a few examples:

- Anamorphisms:
  - [`Generate`](03_CreatingObservableSequences.md#observablegenerate)
  - [`Range`](03_CreatingObservableSequences.md#observablerange)
  - [`Return`](03_CreatingObservableSequences.md#observablereturn)
- Bind:
  - [`SelectMany`](06_Transformation.md#selectmany)
  - [`Select`](06_Transformation.md#select)
  - [`Where`](05_Filtering.md)
- Catamorphism:
  - [`Aggregate`](07_Aggregation.md#aggregate)
  - [`Sum`](07_Aggregation.md#sum)
  - [`Min` and `Max`](07_Aggregation.md#min-and-max)

## Amb

The `Amb` method was a new concept to me when I started using Rx. This function was first introduced by [John McCarthy](https://en.wikipedia.org/wiki/John_McCarthy_(computer_scientist)), in his 1961 paper ['A Basis for a Mathematical Theory of Computation'](https://www.cambridge.org/core/journals/journal-of-symbolic-logic/article/abs/john-mccarthy-a-basis-for-a-mathematical-theory-of-computation-preliminary-report-proceedings-of-the-western-joint-computer-conference-papers-presented-at-the-joint-ireaieeacm-computer-conference-los-angeles-calif-may-911-1961-western-joint-computer-conference-1961-pp-225238-john-mccarthy-a-basis-for-a-mathematical-theory-of-computation-computer-programming-and-formal-systems-edited-by-p-braffort-and-d-hirschberg-studies-in-logic-and-the-foundations-of-mathematics-northholland-publishing-company-amsterdam1963-pp-3370/D1AD4E0CDB7FBE099B04BB4DAF24AFFA) in the Proceedings of the Western Joint Computer Conference. (A digital copy of this is hard to find, but a later version was published in [1963](http://www-formal.stanford.edu/jmc/basis1.pdf) in 'Computer Programming and Format Systems'.) It is an abbreviation of the word _Ambiguous_. Rx diverges slightly from normal .NET class library naming conventions here in using this abbreviation, partly because `amb` is the established name for this operator, but also as a tribute to McCarthy, whose work was an inspiration for the design of Rx.

But what does `Amb` do? The basic idea of an [_ambiguous function_](http://www-formal.stanford.edu/jmc/basis1/node7.html) is that we are allowed to define multiple ways to produce a result, and that some or all of these might in practice prove unable to produce a result. Suppose we've defined some ambiguous function called `equivocate`, and perhaps that for some particular input value, all of `equivocate`'s component parts—all the different ways we gave it of calculating a result—are unable to process the value. (Maybe every one of them divides a number by the input. If we supply an input of `0`, then none of the components can produce a value for this input because they would all attempt to divide by 0.) In cases such as these where none of `equivocate`'s component parts is able to produce a result, `equivocate` itself is unable to produce a result. But suppose we supply some input where exactly one of its component parts is able to produce a result. In that case this result becomes the result of `equivocate` for that input.

So in essence, we're supplying a bunch of different ways to process the input, and if exactly one of those is able to produce a result, we select that result. And if none of the ways of processing the input produces anything, then our ambiguous function also produces nothing.

Where it gets slightly more weird (and where Rx departs from the original definition of `amb`) is when more than one of an ambiguous function's constituents produces a result. In McCarthy's theoretical formulation, the ambiguous function effectively produces all of the results as possible outputs. (This is technically known as _nondeterministic_ computation, although that name can be misleading: it makes it sound like the result will be unpredictable. But that's not what we mean by _nondeterministic_ when talking about computation. It is as though the computer evaluating the ambiguous function clones itself, producing a copy for each possible result, continuing to execute every single copy. You could imagine an multithreaded implementation of such a system, where every time an ambiguous function produces multiple possible results, we create that many new threads so as to be able to evaluate all possible outcomes. This is a reasonable mental model for nondeterministic computation, but it's not what actually happens with Rx's `Amb` operator.) In the kinds of theoretical work ambiguous functions were introduced for, the ambiguity often vanishes in the end. There may have been an enormous number of ways in which a computation could have proceeded, but they might all, finally, produce the same result. However, such theoretical concerns are taking us away from what Rx's `Amb` does, and how we might use it in practice.

[Rx's `Amb`](09_CombiningSequences.md#amb) provides the behaviour described in the cases where either none of the inputs produces anything, or exactly one of them does. However, it makes no attempt to support non-deterministic computation, so its handling of the case where multiple constituents are able to produce value is oversimplified, but then McCarthy's `amb` was first and foremost an analytical construct, so any practical implementation of it is always going to fall short.

## Staying inside the monad

It can be tempting to flip between programming styles when using Rx. For the parts where it's easy to see how Rx applies, then we will naturally use Rx. But when things get tricky, it might seem easiest to change tracks. It might seem like the easiest thing to do would be to `await` an observable, and then proceed with ordinary sequential code. Or maybe it might seem simplest to make callbacks passed to operators like `Select` or `Where` perform operations in addition to their main jobs—to have side effects that do useful things.

Although this can sometimes work, switching between paradigms should be done with caution, as this is a common root cause for concurrency problems such as deadlock and scalability issues. The basic reason for this is that for as long as you remain within Rx's way of doing things, you will benefit from the basic soundness of the mathematical underpinnings. But for this to work, you need to use a functional style. Functions should process their inputs and deterministically produce outputs based on those inputs, and they should neither depend on external state nor change it. This can be a tall order, and it won't always be possible, but a lot of the theory falls apart if you break these rules. Composition doesn't work as reliably as it can. So using a functional style, and keeping your code within Rx's idiom will tend to improve reliability.

## Issues with side effects

Programs always have to have some side effects if they are to do anything useful—if the world is no different as a result of a program having run, then you may as well not have run it—so it can be useful to explore the issues with side effects, so that we can know how best to deal with them when they are necessary. So we will now discuss the consequences of introducing side effects when working with an observable sequence. A function is considered to have a side effect if, in addition to any return value, it has some other observable effect. Generally the 'observable effect' is a modification of state. This observable effect could be:

* modification of a variable with a wider scope than the function (i.e. global, static or perhaps an argument)
* I/O such as a read from or modifying a file, sending or receiving network messages, or updating a display
* causing physical activity, such as when a vending machine dispenses an item, or directs a coin into its coin box

Functional programming in general tries to avoid creating any side effects. Functions with side effects, especially those which modify state, require the programmer to understand more than just the inputs and outputs of the function. Fully understanding the function's operation could entail knowing the full history and context of the state being modified. This can greatly increase the complexity of a function, and making it harder to correctly understand and maintain.

Side effects are not always intentional. An easy way to reduce accidental side effects is to reduce the surface area for change. Here are two simple action coders can take: reduce the visibility or scope of state and make what you can immutable. You can reduce the visibility of a variable by scoping it to a code block like a method (instead of a field or property). You can reduce visibility of class members by making them private or protected. By definition immutable data can't be modified so it can't exhibit side effects. These are sensible encapsulation rules that will dramatically improve the maintainability of your Rx code.

To provide a simple example of a query that has a side effect, we will try to output the index and value of the elements that a subscription receives by updating a variable (closure).

```csharp
IObservable<char> letters = Observable
    .Range(0, 3)
    .Select(i => (char)(i + 65));

int index = -1;
IObservable<char> result = letters.Select(
    c =>
    {
        index++;
        return c;
    });

result.Subscribe(
    c => Console.WriteLine("Received {0} at index {1}", c, index),
    () => Console.WriteLine("completed"));
```

Output:

```
Received A at index 0
Received B at index 1
Received C at index 2
completed
```

While this seems harmless enough, imagine if another person sees this code and understands it to be the pattern the team is using. They in turn adopt this style themselves. For the sake of the example, we will add a duplicate subscription to our previous example.

```csharp
var letters = Observable.Range(0, 3)
                        .Select(i => (char)(i + 65));

var index = -1;
var result = letters.Select(
    c =>
    {
        index++;
        return c;
    });

result.Subscribe(
    c => Console.WriteLine("Received {0} at index {1}", c, index),
    () => Console.WriteLine("completed"));

result.Subscribe(
    c => Console.WriteLine("Also received {0} at index {1}", c, index),
    () => Console.WriteLine("2nd completed"));
```

Output

```
Received A at index 0
Received B at index 1
Received C at index 2
completed
Also received A at index 3
Also received B at index 4
Also received C at index 5
2nd completed
```

Now the second person's output is clearly nonsense. They will be expecting index values to be 0, 1 and 2 but get 3, 4 and 5 instead. I have seen far more sinister versions of side effects in code bases. The nasty ones often modify state that is a Boolean value e.g. `hasValues`, `isStreaming` etc.

In addition to creating potentially unpredictable results in existing software, programs that exhibit side effects are far more difficult to test and maintain. Future refactoring, enhancements or other maintenance on programs that exhibits side effects are far more likely to be brittle. This is especially so in asynchronous or concurrent software.

## Composing data in a pipeline

The preferred way of capturing state is as part of the information flowing through the pipeline of Rx operators making up your subscription. Ideally, we want each part of the pipeline to be independent and deterministic. That is, each function that makes up the pipeline should have its inputs and output as its only state. To correct our example we could enrich the data in the pipeline so that there is no shared state. This would be a great example where we could use the `Select` overload that exposes the index.

```csharp
IObservable<int> source = Observable.Range(0, 3);
IObservable<(int Index, char Letter)> result = source.Select(
    (idx, value) => (Index: idx, Letter: (char) (value + 65)));

result.Subscribe(
    x => Console.WriteLine($"Received {x.Letter} at index {x.Index}"),
    () => Console.WriteLine("completed"));

result.Subscribe(
    x => Console.WriteLine($"Also received {x.Letter} at index {x.Index}"),
    () => Console.WriteLine("2nd completed"));
```

Output:

```
Received A at index 0
Received B at index 1
Received C at index 2
completed
Also received A at index 0
Also received B at index 1
Also received C at index 2
2nd completed
```

Thinking outside of the box, we could also use other features like `Scan` to achieve similar results. Here is an example.

```csharp
var result = source.Scan(
                new
                {
                    Index = -1,
                    Letter = new char()
                },
                (acc, value) => new
                {
                    Index = acc.Index + 1,
                    Letter = (char)(value + 65)
                });
```

The key here is to isolate the state, and reduce or remove any side effects like mutating state.