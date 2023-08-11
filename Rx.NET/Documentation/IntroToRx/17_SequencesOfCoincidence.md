---
title: Sequences of coincidence title
---

#Sequences of coincidence {#SequencesOfCoincidence}

We can conceptualize events that have _duration_ as `windows`. For example;

 * a server is up
 * a person is in a room
 * a button is pressed (and not yet released).

 
The first example could be re-worded as "for this window of time, the server was up". 
An event from one source may have a greater value if it coincides with an event from another source. 
For example, while at a music festival, you may only be interested in tweets (event) about an artist while they are playing (window). 
In finance, you may only be interested in trades (event) for a certain instrument while the New York market is open (window). 
In operations, you may be interested in the user sessions (window) that remained active during an upgrade of a system (window). 
In that example, we would be querying for coinciding windows.
    
Rx provides the power to query sequences of coincidence, sometimes called 'sliding windows'. 
We already recognize the benefit that Rx delivers when querying data in	motion. 
By additionally providing the power to query sequences of coincidence, Rx exposes yet another dimension of possibilities.

##Buffer revisited					{#BufferRevisted}

[`Buffer`](13_TimeShiftedSequences.html#Buffer) is not a new operator to us; however, it can now be conceptually grouped with the window operators. 
Each of these windowing operators act on a sequence and a window of time. 
Each operator will open a window when the source sequence produces a value. 
The way the window is closed, and which values are exposed, are the main differences between each of the operators.
Let us just quickly recap the internal working of the `Buffer` operator and see how this maps to the concept of "windows of time".

`Buffer` will create a window when the first value is produced. 
It will then put that value into an internal cache. 
The window will stay open until the count of values has been reached; each of these values will have been cached. 
When the count has been reached, the window will close and the cache will be published to the result sequence as an `IList<T>`. 
When the next value is produced from the source, the cache is cleared and we start again. 
This means that `Buffer` will take an `IObservable<T>` and return an `IObservable<IList<T>>`.

`Example Buffer with count of 3`

<div class="marble">
<pre class="line">source|-0-1-2-3-4-5-6-7-8-9|</pre>
<pre class="line">result|-----0-----3-----6-9|</pre>
<pre class="line">            1     4     7</pre>
<pre class="line">            2     5     8</pre>
</div>

<p class="comment">
In this marble diagram, I have represented the list of values being returned at a point in time as a column of data. 
That is, the values 0, 1 &amp; 2 are all returned in the first buffer.
</p>

Understanding buffer with time is only a small step away from understanding buffer with count; instead of passing a count, we pass a `TimeSpan`. 
The closing of the window (and therefore the buffer's cache) is now dictated by time instead of the number of values. 
This is now more complicated as we have introduced some sort of scheduling. 
To produce the `IList<T>` at the correct point in time, we need a scheduler assigned to perform the timing. 
Incidentally, this makes testing a lot easier.

`Example Buffer with time of 5 units`

<div class="marble">
<pre class="line">source|-0-1-2-3-4-5-6-7-8-9-|</pre>
<pre class="line">result|----0----2----5----7-|</pre>
<pre class="line">           1    3    6    8</pre>
<pre class="line">                4         9</pre>
</div>

##Window							{#Window}

The `Window` operators are very similar to the `Buffer` operators; they only really differ by their return type. 
Where `Buffer` would take an `IObservable<T>` and return an `IObservable<IList<T>>`, the Window operators return an `IObservable<IObservable<T>>`.
It is also worth noting that the `Buffer` operators will not yield their buffers until the window closes.

Here we can see the simple overloads to `Window`. 
There is a surprising symmetry with the `Window` and `Buffer` overloads.



	public static IObservable<IObservable<TSource>> Window<TSource>(
		this IObservable<TSource> source, 
		int count)
	{...}
	public static IObservable<IObservable<TSource>> Window<TSource>(
		this IObservable<TSource> source, 
		int count, 
		int skip)
	{...}
	public static IObservable<IObservable<TSource>> Window<TSource>(
		this IObservable<TSource> source, 
		TimeSpan timeSpan)
	{...}
	public static IObservable<IObservable<TSource>> Window<TSource>(
		this IObservable<TSource> source, 
		TimeSpan timeSpan, 
		int count)
	{...}
	public static IObservable<IObservable<TSource>> Window<TSource>(
		this IObservable<TSource> source, 
		TimeSpan timeSpan, 
		TimeSpan timeShift)
	{...}
	public static IObservable<IObservable<TSource>> Window<TSource>(
		this IObservable<TSource> source, 
		TimeSpan timeSpan, 
		IScheduler scheduler)
	{...}
	public static IObservable<IObservable<TSource>> Window<TSource>(
		this IObservable<TSource> source, 
		TimeSpan timeSpan, 
		TimeSpan timeShift, 
		IScheduler scheduler)
	{...}
	public static IObservable<IObservable<TSource>> Window<TSource>(
		this IObservable<TSource> source, 
		TimeSpan timeSpan, 
		int count, 
		IScheduler scheduler)
	{...}


This is an example of `Window` with a count of 3 as a marble diagram:

<div class="marble">
<pre class="line">source |-0-1-2-3-4-5-6-7-8-9|</pre>
<pre class="line">window0|-0-1-2|</pre>
<pre class="line">window1        3-4-5|</pre>
<pre class="line">window2              6-7-8|</pre>
<pre class="line">window3                    9|</pre>
</div>

For demonstration purposes, we could reconstruct that with this code.

	var windowIdx = 0;
	var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(10);
	source.Window(3)
			.Subscribe(window =>
			{
				var id = windowIdx++;
				Console.WriteLine("--Starting new window");
				var windowName = "Window" + thisWindowIdx;
				window.Subscribe(
					value => Console.WriteLine("{0} : {1}", windowName, value),
					ex => Console.WriteLine("{0} : {1}", windowName, ex),
					() => Console.WriteLine("{0} Completed", windowName));
			},
			() => Console.WriteLine("Completed"));

Output:

<div class="output">
<div class="line">--Starting new window</div>
<div class="line">window0 : 0</div>
<div class="line">window0 : 1</div>
<div class="line">window0 : 2</div>
<div class="line">window0 Completed</div>
<div class="line">--Starting new window</div>
<div class="line">window1 : 3</div>
<div class="line">window1 : 4</div>
<div class="line">window1 : 5</div>
<div class="line">window1 Completed</div>
<div class="line">--Starting new window</div>
<div class="line">window2 : 6</div>
<div class="line">window2 : 7</div>
<div class="line">window2 : 8</div>
<div class="line">window2 Completed</div>
<div class="line">--Starting new window</div>
<div class="line">window3 : 9</div>
<div class="line">window3 Completed</div>
<div class="line">Completed</div>
</div>

`Example of Window with time of 5 units`

<div class="marble">
<pre class="line">source |-0-1-2-3-4-5-6-7-8-9|</pre>
<pre class="line">window0|-0-1-|</pre>
<pre class="line">window1      2-3-4|</pre>
<pre class="line">window2           -5-6-|</pre>
<pre class="line">window3                7-8-9|</pre>
</div>

A major difference we see here is that the `Window` operators can notify you of values from the source as soon as they are produced. 
The `Buffer` operators, on the other hand, must wait until the window closes before the values can be notified as an entire list.

###Flattening a Window operation 	{#FlatteningAWindowOperation}
I think it is worth noting, at least from an academic standpoint, that the `Window` operators produce `IObservable<IObservable<T>>`. 
We have explored the concept of [nested observables](07_Aggregation.html#NestedObservables) in the earlier chapter on [Aggregation](07_Aggregation.html).
`Concat`, `Merge` and `Switch` each have an overload that takes an `IObservable<IObservable<T>>` and returns an `IObservable<T>`. 
As the `Window` operators ensure that the windows (child sequences) do not overlap, we can use either of the `Concat`, `Switch` or `Merge` operators to turn a windowed sequence back into its original form.

	//is the same as Observable.Interval(TimeSpan.FromMilliseconds(200)).Take(10) 
	 var switchedWindow = Observable.Interval(TimeSpan.FromMilliseconds(200)).Take(10)
		.Window(TimeSpan.FromMilliseconds(500))
		.Switch();

###Customizing windows				{#CustomizingWindows}

The overloads above provide simple ways to break a sequence into smaller nested windows using a count and/or a time span. 
Now we will look at the other overloads, that provide more flexibility over how windows are managed.

	//Projects each element of an observable sequence into consecutive non-overlapping windows.
	//windowClosingSelector : A function invoked to define the boundaries of the produced 
	//  windows. A new window is started when the previous one is closed.
	public static IObservable<IObservable<TSource>> Window<TSource, TWindowClosing>
	(
		this IObservable<TSource> source, 
		Func<IObservable<TWindowClosing>> windowClosingSelector
	)
	{...}


The first of these complex overloads allows us to control when windows should close.
The `windowClosingSelector` function is called each time a window is created.
Windows are created on subscription and immediately after a window closes; windows close when the sequence from the `windowClosingSelector` produces a value.
The value is disregarded so it doesn't matter what type the sequence values are; in fact you can just complete the sequence from `windowClosingSelector` to close the window instead.

In this example, we create a window with a closing selector. 
We return the same subject from that selector every time, then notify from the subject whenever a user presses enter from the console.

	var windowIdx = 0;
	var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(10);
	var closer = new Subject<Unit>();
	source.Window(() => closer)
			.Subscribe(window =>
			{
				var thisWindowIdx = windowIdx++;
				Console.WriteLine("--Starting new window");
				var windowName = "Window" + thisWindowIdx;
				window.Subscribe(
					value => Console.WriteLine("{0} : {1}", windowName, value),
					ex => Console.WriteLine("{0} : {1}", windowName, ex),
					() => Console.WriteLine("{0} Completed", windowName));
			},
			() => Console.WriteLine("Completed"));

	var input = "";
	while (input!="exit")
	{
		input = Console.ReadLine();
		closer.OnNext(Unit.Default);
	}


Output (when I hit enter after '1' and '5' are displayed):
<div class="output">
<div class="line">--Starting new window</div>
<div class="line">window0 : 0</div>
<div class="line">window0 : 1</div>
<div class="line"></div>
<div class="line">window0 Completed</div>
<div class="line">--Starting new window</div>
<div class="line">window1 : 2</div>
<div class="line">window1 : 3</div>
<div class="line">window1 : 4</div>
<div class="line">window1 : 5</div>
<div class="line"></div>
<div class="line">window1 Completed</div>
<div class="line">--Starting new window</div>
<div class="line">window2 : 6</div>
<div class="line">window2 : 7</div>
<div class="line">window2 : 8</div>
<div class="line">window2 : 9</div>
<div class="line">window2 Completed</div>
<div class="line">Completed</div>
</div>

The most complex overload of `Window` allows us to create potentially overlapping windows.

	//Projects each element of an observable sequence into zero or more windows.
	// windowOpenings : Observable sequence whose elements denote the creation of new windows.
	// windowClosingSelector : A function invoked to define the closing of each produced window.
	public static IObservable<IObservable<TSource>> Window
		<TSource, TWindowOpening, TWindowClosing>
	(
		this IObservable<TSource> source, 
		IObservable<TWindowOpening> windowOpenings, 
		Func<TWindowOpening, IObservable<TWindowClosing>> windowClosingSelector
	)
	{...}

This overload takes three arguments

 1. The source sequence
 2. A sequence that indicates when a new window should be opened
 3. A function that takes a window opening value, and returns a window closing sequence


This overload offers great flexibility in the way windows are opened and closed.
Windows can be largely independent from each other; they can overlap, vary in size and even skip values from the source.

To ease our way into this more complex overload, let's first try to use it to recreate a simpler version of `Window` (the overload that takes a count). 
To do so, we need to open a window once on the initial subscription, and once each time the source has produced then specified count. 
The window needs to close each time that count is reached. 
To achieve this we only need the source sequence. 
We will share it by using the `Publish` method, then supply 'views' of the source as each of the arguments.

	public static IObservable<IObservable<T>> MyWindow<T>(
		this IObservable<T> source, 
		int count)
	{
		var shared = source.Publish().RefCount();
		var windowEdge = shared
			.Select((i, idx) => idx % count)
			.Where(mod => mod == 0)
			.Publish()
			.RefCount();
		return shared.Window(windowEdge, _ => windowEdge);
	}

If we now want to extend this method to offer skip functionality, we need to havetwo different sequences: one for opening and one for closing. 
We open a window on subscription and again after the `skip` items have passed. 
We close thosewindows after '`count`' items have passed since the window opened.

	public static IObservable<IObservable<T>> MyWindow<T>(
		this IObservable<T> source, 
		int count, 
		int skip)
	{
		if (count <= 0) throw new ArgumentOutOfRangeException();
		if (skip <= 0) throw new ArgumentOutOfRangeException();

		var shared = source.Publish().RefCount();
		var index = shared
			.Select((i, idx) => idx)
			.Publish()
			.RefCount();
		var windowOpen = index.Where(idx => idx % skip == 0);
		var windowClose = index.Skip(count-1);
		return shared.Window(windowOpen, _ => windowClose);
	}

We can see here that the `windowClose` sequence is re-subscribed to each time a window is opened, due to it being returned from a function. 
This allows us to reapply the skip (`Skip(count-1)`) for each window. 
Currently, we ignore the value that the `windowOpen` pushes to the `windowClose` selector, but if you require it for some logic, it is available to you.

As you can see, the `Window` operator can be quite powerful. 
We can even use `Window` to replicate other operators; for instance we can create our own implementation of `Buffer` that way. 
We can have the `SelectMany` operator take a single value (the window) to produce zero or more values of another type (in our case, a single `IList<T>`). 
To create the `IList<T>` without blocking, we can apply the `Aggregate` method and use a new `List<T>` as the seed.

	public static IObservable<IList<T>> MyBuffer<T>(
		this IObservable<T> source, 
		int count)
	{
		return source.Window(count)
			.SelectMany(window => 
				window.Aggregate(
					new List<T>(), 
					(list, item) =>
					{
						list.Add(item);
						return list;
					}));
	}

It may be an interesting exercise to try implementing other time shifting methods, like `Sample` or `Throttle`, with `Window`.

##Join								{#Join}

The `Join` operator allows you to logically join two sequences. 
Whereas the `Zip` operator would pair values from the two sequences together by index, the `Join` operator allows you join sequences by intersecting windows. 
Like the `Window` overload we just looked at, you can specify when a window should close via an observable sequence; this sequence is returned from a function that takes an opening value. 
The `Join` operator has two such functions, one for the first source sequence and one for the second source sequence. 
Like the `Zip` operator, we also need to provide a selector function to produce the result item from the pair of values.

	public static IObservable<TResult> Join
		<TLeft, TRight, TLeftDuration, TRightDuration, TResult>
	(
		this IObservable<TLeft> left,
		IObservable<TRight> right,
		Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
		Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
		Func<TLeft, TRight, TResult> resultSelector
	)

This is a complex signature to try and understand in one go, so let's take it one parameter at a time.

`IObservable<TLeft> left` is the source sequence that defines when a window starts. 
This is just like the `Buffer` and `Window` operators, except that every value published from this source opens a new window. 
In `Buffer` and `Window`, by contrast, some values just fell into an existing window.

I like to think of `IObservable<TRight> right` as the window value sequence. 
While the left sequence controls opening the windows, the right sequence will try to pair up with a value from the left sequence.

Let us imagine that our left sequence produces a value, which creates a new window.
If the right sequence produces a value while the window is open, then the `resultSelector` function is called with the two values. 
This is the crux of join, pairing two values from a sequence that occur within the same window. 
This then leads us to our next question; when does the window close? 
The answer illustrates both the power and the complexity of the `Join` operator.

When `left` produces a value, a window is opened. 
That value is also passed, at that time, to the `leftDurationSelector` function, which returns an `IObservable<TLeftDuration>`.
When that sequence produces a value or completes, the window for that value is closed.
Note that it is irrelevant what the type of `TLeftDuration` is. 
This initially left me with the feeling that `IObservable<TLeftDuration>` was a bit excessive as you effectively just need some sort of event to say 'Closed'. 
However, by being allowed to use `IObservable<T>`, you can do some clever manipulation as we will see later.

Let us now imagine a scenario where the left sequence produces values twice as fast as the right sequence. 
Imagine that in addition we never close the windows; we could do this by always returning `Observable.Never<Unit>()` from the `leftDurationSelector` function. 
This would result in the following pairs being produced.

Left Sequence

<div class="marble">
<pre class="line">L 0-1-2-3-4-5-</pre>
</div>

Right Sequence

<div class="marble">
<pre class="line">R --A---B---C-</pre>
</div>
<div class="output">
<div class="line">0, A</div>
<div class="line">1, A</div>
<div class="line">0, B</div>
<div class="line">1, B</div>
<div class="line">2, B</div>
<div class="line">3, B</div>
<div class="line">0, C</div>
<div class="line">1, C</div>
<div class="line">2, C</div>
<div class="line">3, C</div>
<div class="line">4, C</div>
<div class="line">5, C</div>
</div>

As you can see, the left values are cached and replayed each time the right produces a value.

Now it seems fairly obvious that, if I immediately closed the window by returning `Observable.Empty<Unit>`, or perhaps `Observable.Return(0)`, windows would never be opened thus no pairs would ever get produced. 
However, what could I do to make sure that these windows did not overlap- so that, once a second value was produced I would no longer see the first value? 
Well, if we returned the `left` sequence from the `leftDurationSelector`, that could do the trick. 
But wait, when we return the sequence `left` from the `leftDurationSelector`, it would try to create another subscription and that may introduce side effects. 
The quick answer to that is to `Publish` and `RefCount` the `left` sequence. 
If we do that, the results look more like this.

<div class="marble">
<pre class="line">left  |-0-1-2-3-4-5|</pre>
<pre class="line">right |---A---B---C|</pre>
<pre class="line">result|---1---3---5</pre>
<pre class="line">          A   B   C</pre>
</div>

The last example is very similar to `CombineLatest`, except that it is only producing a pair when the right sequence changes. 
We could use `Join` to produce our own version of [`CombineLatest`](12_CombiningSequences.html#CombineLatest).
If the values from the left sequence expire when the next value from left was notified, then I would be well on my way to implementing my version of `CombineLatest`.
However I need the same thing to happen for the right.
Luckily the `Join` operator provides a `rightDurationSelector` that works just like the `leftDurationSelector`. 
This is simple to implement; all I need to do is return a reference to the same left sequence when a left value is produced, and do the same for the right. 
The code looks like this.

	public static IObservable<TResult> MyCombineLatest<TLeft, TRight, TResult>
	(
		IObservable<TLeft> left,
		IObservable<TRight> right,
		Func<TLeft, TRight, TResult> resultSelector
	)
	{
		var refcountedLeft = left.Publish().RefCount();
		var refcountedRight = right.Publish().RefCount();
		return Observable.Join(
			refcountedLeft,
			refcountedRight,
			value => refcountedLeft,
			value => refcountedRight,
			resultSelector);
	}

While the code above is not production quality (it would need to have some gates in place to mitigate race conditions), it shows how powerful `Join` is; we can actually use it to create other operators!

##GroupJoin							{#GroupJoin}

When the `Join` operator pairs up values that coincide within a window, it will pass the scalar values left and right to the `resultSelector`. 
The `GroupJoin` operator takes this one step further by passing the left (scalar) value immediately to the `resultSelector` with the right (sequence) value. 
The right parameter represents all of the values from the right sequences that occur within the window.
Its signature is very similar to `Join`, but note the difference in the `resultSelector` parameter.

	public static IObservable<TResult> GroupJoin
		<TLeft, TRight, TLeftDuration, TRightDuration, TResult>
	(
		this IObservable<TLeft> left,
		IObservable<TRight> right,
		Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
		Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
		Func<TLeft, IObservable<TRight>, TResult> resultSelector
	)


If we went back to our first `Join` example where we had

 * the `left` producing values twice as fast as the right,
 * the left never expiring
 * the right immediately expiring


this is what the result may look like

<div class="marble">
<pre class="line">left              |-0-1-2-3-4-5|</pre>
<pre class="line">right             |---A---B---C|</pre>
<pre class="line">0th window values   --A---B---C|</pre>
<pre class="line">1st window values     A---B---C|</pre>
<pre class="line">2nd window values       --B---C|</pre>
<pre class="line">3rd window values         B---C|</pre>
<pre class="line">4th window values           --C|</pre>
<pre class="line">5th window values             C|</pre>
</div>

We could switch it around and have the left expired immediately and the right never expire. 
The result would then look like this:

<div class="marble">
<pre class="line">left              |-0-1-2-3-4-5|</pre>
<pre class="line">right             |---A---B---C|</pre>
<pre class="line">0th window values   |</pre>
<pre class="line">1st window values     A|</pre>
<pre class="line">2nd window values       A|</pre>
<pre class="line">3rd window values         AB|</pre>
<pre class="line">4th window values           AB|</pre>
<pre class="line">5th window values             ABC|</pre>
</div>

This starts to make things interesting. 
Perceptive readers may have noticed that with `GroupJoin` you could effectively re-create your own `Join` method by doing something like this:

	public IObservable<TResult> MyJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
		IObservable<TLeft> left,
		IObservable<TRight> right,
		Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
		Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
		Func<TLeft, TRight, TResult> resultSelector)
	{
		return Observable.GroupJoin
		(
			left,
			right,
			leftDurationSelector,
			rightDurationSelector,
			(leftValue, rightValues)=>
				rightValues.Select(rightValue=>resultSelector(leftValue, rightValue))
		)
		.Merge();
	}


You could even create a crude version of `Window` with this code:

	public IObservable<IObservable<T>> MyWindow<T>(
		IObservable<T> source, 
		TimeSpan windowPeriod)
	{
		return Observable.Create<IObservable<T>>(o 
			=>;
			{
			var sharedSource = source
				.Publish()
				.RefCount();

			var intervals = Observable.Return(0L)
				.Concat(Observable.Interval(windowPeriod))
				.TakeUntil(sharedSource.TakeLast(1))
				.Publish()
				.RefCount();

			return intervals.GroupJoin(
					sharedSource, 
					_ => intervals, 
					_ => Observable.Empty<Unit>(), 
					(left, sourceValues) => sourceValues)
				.Subscribe(o);
		});
	}


For an alternative summary of reducing operators to a primitive set see Bart DeSmet's
[excellent MINLINQ post](http://blogs.bartdesmet.net/blogs/bart/archive/2010/01/01/the-essence-of-linq-minlinq.aspx "The essence of LINQ - MinLINQ")
(and [follow-up video](http://channel9.msdn.com/Shows/Going+Deep/Bart-De-Smet-MinLINQ-The-Essence-of-LINQ "The essence of LINQ - MINLINQ - Channel9") ). 
Bart is one of the key members of the team that built Rx, so it is great to get some insight on how the creators of Rx think.

Showcasing `GroupJoin` and the use of other operators turned out to be a fun academic exercise. 
While watching videos and reading books on Rx will increase your familiarity with it, nothing replaces the experience of actually picking it apart and using it in earnest.

`GroupJoin` and other window operators reduce the need for low-level plumbing of state and concurrency. 
By exposing a high-level API, code that would be otherwise difficult to write, becomes a cinch to put together. 
For example, those in the finance industry could use `GroupJoin` to easily produce real-time Volume or Time Weighted Average Prices (VWAP/TWAP).

Rx delivers yet another way to query data in motion by allowing you to interrogate sequences of coincidence. 
This enables you to solve the intrinsically complex problem of managing state and concurrency while performing matching from multiple sources.
By encapsulating these low level operations, you are able to leverage Rx to design your software in an expressive and testable fashion.
Using the Rx operators as building blocks, your code effectively becomes a composition of many simple operators. 
This allows the complexity of the domain code to be the focus, not the otherwise incidental supporting code.

---

#Summary							{#Summary}

When LINQ was first released, it brought the ability to query static data sources directly into the language. 
With the volume of data produced in modern times, only being able to query data-at-rest, limits your competitive advantage. 
Being able to make sense of information as it flows, opens an entirely new spectrum of software.
We need more than just the ability to react to events, we have been able to do this for years. 
We need the ability to construct complex queries across multiple sources of flowing data.

Rx brings event processing to the masses by allowing you to query data-in-motion directly from your favorite .NET language. 
Composition is king: you compose operators to create queries and you compose sequences to enrich the data. 
Rx leverages common types, patterns and language features to deliver an incredibly powerful library that can change the way you write modern software.

Throughout the book you will have learnt the basic types and principle of Rx. 
You have discovered functional programming concepts and how they apply to observable sequences. 
You can identify potential pitfalls of certain patterns and how to avoid them. 
You understand the internal working of the operators and are even able to build your own implementations of many of them. 
Finally you are able to construct complex queries that manage concurrency in a safe and declarative way while still being testable.

You have everything you need to confidently build applications using the Reactive Extensions for .NET. 
If you do find yourself at any time stuck, and not sure how to solve a problem or need help, you can probably solve it without outside stimulus.
Remember to first draw a marble diagram of what you think the problem space is.
This should allow you to see the patterns in the flow which will help you choose the correct operators. 
Secondly, remember to follow the [Guidelines](18_UsageGuidelines.html). 
Third, write a spike. 
Use [LINQPad](http://www.linqpad.net/) or a blank Visual Studio project to flesh out a small sample. 
Finally, if you are still stuck, your best place to look for help is the MSDN [Rx forum](http://social.msdn.microsoft.com/Forums/en-US/rx/). 
[StackOverflow.com](http://stackoverflow.com/) is another useful resource too, but with regards to Rx questions, the MSDN forum is dedicated to Rx and seems to have a higher quality of answers.

---

<div class="webonly">
<h1 class="ignoreToc">Additional recommended reading</h1>
<div align="center">
<div style="display:inline-block; vertical-align: top;  margin: 10px; width: 140px; font-size: 11px; text-align: center">
<!--C# in a nutshell Amazon.co.uk-->
<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=B008E6I1K8&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
	style="width:120px;height:240px;margin: 10px" 
	scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>
</div>
<div style="display:inline-block; vertical-align: top;  margin: 10px; width: 140px; font-size: 11px; text-align: center">
<!--C# Linq pocket reference Amazon.co.uk-->
<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=0596519249&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
		style="width:120px;height:240px;margin: 10px" 
		scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>
</div>
<div style="display:inline-block; vertical-align: top; margin: 10px; width: 140px; font-size: 11px; text-align: center">
<!--CLR via C# v4 Amazon.co.uk-->
<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=B00AA36R4U&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
		style="width:120px;height:240px;margin: 10px" 
		scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>
</div>
<div style="display:inline-block; vertical-align: top; margin: 10px; width: 140px; font-size: 11px; text-align: center">
<!--Real-world functional programming Amazon.co.uk-->
<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=1933988924&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
		style="width:120px;height:240px;margin: 10px" 
		scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>
</div>           
</div></div>