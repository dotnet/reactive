---
title : Reducing a sequence
---

#Reducing a sequence				{#Reduction}

We live in the information age. 
Data is being created, stored and distributed at a phenomenal rate. 
Consuming this data can be overwhelming, like trying to drink directly from the fire hose. 
We need the ability to pick out the data we need, choose what is and is not relevant, and roll up groups of data to make it relevant. 
Users, customers and managers need you do this with more data than ever before, while still delivering higher performance and tighter deadlines.

Given that we know how to create an observable sequence, we will now look at the various methods that can reduce an observable sequence. 
We can categorize operators that reduce a sequence to the following:

<dl>
	<dt>Filter and partition operators</dt>
	<dd>
		Reduce the source sequence to a sequence with at most the same number of elements</dd>
	<dt>Aggregation operators</dt>
	<dd>
		Reduce the source sequence to a sequence with a single element</dd>
	<dt>Fold operators</dt>
	<dd>
		Reduce the source sequence to a single element as a scalar value</dd>
</dl>

We discovered that the creation of an observable sequence from a scalar value is defined as _anamorphism_ or described as an _'unfold'_. 
We can think of the anamorphism from `T` to `IObservable<T>` as an 'unfold'.
This could also be referred to as "entering the monad" where in this case (and for most cases in this book) the monad is `IObservable<T>`. 
What we will now start looking at are methods that eventually get us to the inverse which is defined as _catamorphism_ or a `fold`. 
Other popular names for fold are 'reduce', 'accumulate' and 'inject'.

##Where								{#Where}    
Applying a filter to a sequence is an extremely common exercise and the most common filter is the `Where` clause. 
In Rx you can apply a where clause with the `Where` extension method. 
For those that are unfamiliar, the signature of the `Where` method is as follows:


	IObservable<T> Where(this IObservable<T> source, Fun<T, bool> predicate)

Note that both the source parameter and the return type are the same. 
This allows for a fluent interface, which is used heavily throughout Rx and other LINQ code.
In this example we will use the `Where` to filter out all even values produced from a `Range` sequence.

	var oddNumbers = Observable.Range(0, 10)
		.Where(i => i % 2 == 0)
		.Subscribe(
			Console.WriteLine, 
			() => Console.WriteLine("Completed"));

Output:

<div class="output">
	<div class="line">0</div>
	<div class="line">2</div>
	<div class="line">4</div>
	<div class="line">6</div>
	<div class="line">8</div>
	<div class="line">Completed</div>
</div>

The `Where` operator is one of the many standard LINQ operators. 
This and other LINQ operators are common use in the various implementations of query operators, most notably the `IEnumerable<T>` implementation. 
In most cases the operators behave just as they do in the `IEnumerable<T>` implementations, but there are some exceptions. 
We will discuss each implementation and explain any variation as we go. 
By implementing these common operators Rx also gets language support for free via C# query comprehension syntax. 
For the examples in this book however, we will keep with using extension methods for consistency.

##Distinct and DistinctUntilChanged	{#Distinct}
As I am sure most readers are familiar with the `Where` extension method for `IEnumerable<T>`, some will also know the `Distinct` method.
In Rx, the `Distinct` method has been made available for observable sequences too. 
For those that are unfamiliar with `Distinct`, and as a recap for those that are, `Distinct` will only pass on values from the source that it has not seen before.

	var subject = new Subject<int>();
	var distinct = subject.Distinct();
		
	subject.Subscribe(
		i => Console.WriteLine("{0}", i),
		() => Console.WriteLine("subject.OnCompleted()"));
	distinct.Subscribe(
		i => Console.WriteLine("distinct.OnNext({0})", i),
		() => Console.WriteLine("distinct.OnCompleted()"));

	subject.OnNext(1);
	subject.OnNext(2);
	subject.OnNext(3);
	subject.OnNext(1);
	subject.OnNext(1);
	subject.OnNext(4);
	subject.OnCompleted();

Output:

<div class="output">
	<div class="line">1</div>
	<div class="line">distinct.OnNext(1)</div>
	<div class="line">2</div>
	<div class="line">distinct.OnNext(2)</div>
	<div class="line">3</div>
	<div class="line">distinct.OnNext(3)</div>
	<div class="line">1</div>
	<div class="line">1</div>
	<div class="line">4</div>
	<div class="line">distinct.OnNext(4)</div>
	<div class="line">subject.OnCompleted()</div>
	<div class="line">distinct.OnCompleted()</div>
</div>

Take special note that the value 1 is pushed 3 times but only passed through the first time.
There are overloads to `Distinct` that allow you to specialize the way an item is determined to be distinct or not. 
One way is to provide a function that returns a different value to use for comparison. 
Here we look at an example that uses a property from a custom class to define if a value is distinct.

	public class Account
	{
		public int AccountId { get; set; }
		//... etc
	}
	public void Distinct_with_KeySelector()
	{
		var subject = new Subject<Account>();
		var distinct = subject.Distinct(acc => acc.AccountId);
	}

In addition to the `keySelector` function that can be provided, there is an overload that takes an `IEqualityComparer<T>` instance. 
This is useful if you have a custom implementation that you can reuse to compare instances of your type `T`. 
Lastly there is an overload that takes a `keySelector` and an instance of `IEqualityComparer<TKey>`. 
Note that the equality comparer in this case is aimed at the selected key type (`TKey`), not the type `T`.

A variation of `Distinct`, that is peculiar to Rx, is `DistinctUntilChanged`.
This method will surface values only if they are different from the previous value.
Reusing our first `Distinct` example, note the change in output.

	var subject = new Subject<int>();
	var distinct = subject.DistinctUntilChanged();
		
	subject.Subscribe(
		i => Console.WriteLine("{0}", i),
		() => Console.WriteLine("subject.OnCompleted()"));
	distinct.Subscribe(
		i => Console.WriteLine("distinct.OnNext({0})", i),
		() => Console.WriteLine("distinct.OnCompleted()"));

	subject.OnNext(1);
	subject.OnNext(2);
	subject.OnNext(3);
	subject.OnNext(1);
	subject.OnNext(1);
	subject.OnNext(4);
	subject.OnCompleted();

Output:

<div class="output">
	<div class="line">1</div>
	<div class="line">distinct.OnNext(1)</div>
	<div class="line">2</div>
	<div class="line">distinct.OnNext(2)</div>
	<div class="line">3</div>
	<div class="line">distinct.OnNext(3)</div>
	<div class="line">1</div>
	<div class="line">distinct.OnNext(1)</div>
	<div class="line">1</div>
	<div class="line">4</div>
	<div class="line">distinct.OnNext(4)</div>
	<div class="line">subject.OnCompleted()</div>
	<div class="line">distinct.OnCompleted()</div>
</div>

The difference between the two examples is that the value 1 is pushed twice. 
However the third time that the source pushes the value 1, it is immediately after the second time value 1 is pushed. 
In this case it is ignored. 
Teams I have worked with have found this method to be extremely useful in reducing any noise that a sequence may provide.

##IgnoreElements					{#IgnoreElements}

The `IgnoreElements` extension method is a quirky little tool that allows you to receive the `OnCompleted` or `OnError` notifications. 
We could effectively recreate it by using a `Where` method with a predicate that always returns false.

	var subject = new Subject<int>();
	
	//Could use subject.Where(_=>false);
	var noElements = subject.IgnoreElements();
	subject.Subscribe(
		i=>Console.WriteLine("subject.OnNext({0})", i),
		() => Console.WriteLine("subject.OnCompleted()"));
	noElements.Subscribe(
		i=>Console.WriteLine("noElements.OnNext({0})", i),
		() => Console.WriteLine("noElements.OnCompleted()"));

	subject.OnNext(1);
	subject.OnNext(2);
	subject.OnNext(3);
	subject.OnCompleted();

Output:

<div class="output">
	<div class="line">subject.OnNext(1)</div>
	<div class="line">subject.OnNext(2)</div>
	<div class="line">subject.OnNext(3)</div>
	<div class="line">subject.OnCompleted()</div>
	<div class="line">noElements.OnCompleted()</div>
</div>

As suggested earlier we could use a `Where` to produce the same result

	subject.IgnoreElements();
	//Equivalent to 
	subject.Where(value=>false);
	//Or functional style that implies that the value is ignored.
	subject.Where(_=>false);

Just before we leave `Where` and `IgnoreElements`, I wanted to just quickly look at the last line of code. 
Until recently, I personally was not aware that '`_`' was a valid variable name; however it is commonly used by functional programmers to indicate an ignored parameter. 
This is perfect for the above example; for each value we receive, we ignore it and always return false.
The intention is to improve the readability of the code via convention.

##Skip and Take						{#SkipAndTake}

The other key methods to filtering are so similar I think we can look at them as one big group. 
First we will look at `Skip` and `Take`. 
These act just like they do for the `IEnumerable<T>` implementations. 
These are the most simple and probably the most used of the Skip/Take methods. 
Both methods just have the one parameter; the number of values to skip or to take.

If we first look at `Skip`, in this example we have a range sequence of 10 items and we apply a `Skip(3)` to it.

	Observable.Range(0, 10)
		.Skip(3)
		.Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));

Output:

<div class="output">
	<div class="line">3</div>
	<div class="line">4</div>
	<div class="line">5</div>
	<div class="line">6</div>
	<div class="line">7</div>
	<div class="line">8</div>
	<div class="line">9</div>
	<div class="line">Completed</div>
</div>

Note the first three values (0, 1 &amp; 2) were all ignored from the output. 
Alternatively, if we used `Take(3)` we would get the opposite result; i.e. we would only get the first 3 values and then the Take operator would complete the sequence.

	Observable.Range(0, 10)
		.Take(3)
		.Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));

Output:

<div class="output">
	<div class="line">0</div>
	<div class="line">1</div>
	<div class="line">2</div>
	<div class="line">Completed</div>
</div>

Just in case that slipped past any readers, it is the `Take` operator that completes once it has received its count. 
We can prove this by applying it to an infinite sequence.

	Observable.Interval(TimeSpan.FromMilliseconds(100))
		.Take(3)
		.Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));

Output:

<div class="output">
	<div class="line">0</div>
	<div class="line">1</div>
	<div class="line">2</div>
	<div class="line">Completed</div>
</div>

###SkipWhile and TakeWhile			{#SkipWhileTakeWhile}

The next set of methods allows you to skip or take values from a sequence while a predicate evaluates to true. 
For a `SkipWhile` operation this will filter out all values until a value fails the predicate, then the remaining sequence can be returned.

	var subject = new Subject<int>();
	subject
		.SkipWhile(i => i < 4)
		.Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
	subject.OnNext(1);
	subject.OnNext(2);
	subject.OnNext(3);
	subject.OnNext(4);
	subject.OnNext(3);
	subject.OnNext(2);
	subject.OnNext(1);
	subject.OnNext(0);

	subject.OnCompleted();

Output:

<div class="output">
	<div class="line">4</div>
	<div class="line">3</div>
	<div class="line">2</div>
	<div class="line">1</div>
	<div class="line">0</div>
	<div class="line">Completed</div>
</div>

`TakeWhile` will return all values while the predicate passes, and when the first value fails the sequence will complete.

	var subject = new Subject<int>();
	subject
		.TakeWhile(i => i < 4)
		.Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
	subject.OnNext(1);
	subject.OnNext(2);
	subject.OnNext(3);
	subject.OnNext(4);
	subject.OnNext(3);
	subject.OnNext(2);
	subject.OnNext(1);
	subject.OnNext(0);

	subject.OnCompleted();

Output:

<div class="output">
	<div class="line">1</div>
	<div class="line">2</div>
	<div class="line">3</div>
	<div class="line">Completed</div>
</div>

###SkipLast and TakeLast			{#SkipLastTakeLast}

These methods become quite self explanatory now that we understand Skip/Take and SkipWhile/TakeWhile. 
Both methods require a number of elements at the end of a sequence to either skip or take. 
The implementation of the `SkipLast` could cache all values, wait for the source sequence to complete, and then replay all the values except for the last number of elements. 
The Rx team however, has been a bit smarter than that. 
The real implementation will queue the specified number of notifications and once the queue size exceeds the value, it can be sure that it may drain a value from the queue.

	var subject = new Subject<int>();
	subject
		.SkipLast(2)
		.Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
	Console.WriteLine("Pushing 1");
	subject.OnNext(1);
	Console.WriteLine("Pushing 2");
	subject.OnNext(2);
	Console.WriteLine("Pushing 3");
	subject.OnNext(3);
	Console.WriteLine("Pushing 4");
	subject.OnNext(4);
	subject.OnCompleted();

Output:

<div class="output">
	<div class="line">Pushing 1</div>
	<div class="line">Pushing 2</div>
	<div class="line">Pushing 3</div>
	<div class="line">1</div>
	<div class="line">Pushing 4</div>
	<div class="line">2</div>
	<div class="line">Completed</div>
</div>

Unlike `SkipLast`, `TakeLast` does have to wait for the source sequence to complete to be able to push its results.
As per the example above, there are `Console.WriteLine` calls to indicate what the program is doing at each stage.

	var subject = new Subject<int>();
	subject
		.TakeLast(2)
		.Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
	Console.WriteLine("Pushing 1");
	subject.OnNext(1);
	Console.WriteLine("Pushing 2");
	subject.OnNext(2);
	Console.WriteLine("Pushing 3");
	subject.OnNext(3);
	Console.WriteLine("Pushing 4");
	subject.OnNext(4);
	Console.WriteLine("Completing");
	subject.OnCompleted();

Output:

<div class="output">
	<div class="line">Pushing 1</div>
	<div class="line">Pushing 2</div>
	<div class="line">Pushing 3</div>
	<div class="line">Pushing 4</div>
	<div class="line">Completing</div>
	<div class="line">3</div>
	<div class="line">4</div>
	<div class="line">Completed</div>
</div>

###SkipUntil and TakeUntil			{#SkipUntilTakeUntil}

Our last two methods make an exciting change to the methods we have previously looked.
These will be the first two methods that we have discovered together that require two observable sequences.

`SkipUntil` will skip all values until any value is produced by a secondary observable sequence.

	var subject = new Subject<int>();
	var otherSubject = new Subject<Unit>();
	subject
		.SkipUntil(otherSubject)
		.Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
	subject.OnNext(1);
	subject.OnNext(2);
	subject.OnNext(3);
	otherSubject.OnNext(Unit.Default);
	subject.OnNext(4);
	subject.OnNext(5);
	subject.OnNext(6);
	subject.OnNext(7);
	subject.OnNext(8);

	subject.OnCompleted();

Output:

<div class="output">
	<div class="line">4</div>
	<div class="line">5</div>
	<div class="line">6</div>
	<div class="line">7</div>
	<div class="line">Completed</div>
</div>

Obviously, the converse is true for `TakeWhile`. 
When the secondary sequence produces a value, then the `TakeWhile` operator will complete the output sequence.

	var subject = new Subject<int>();
	var otherSubject = new Subject<Unit>();
	subject
		.TakeUntil(otherSubject)
		.Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
	subject.OnNext(1);
	subject.OnNext(2);
	subject.OnNext(3);
	otherSubject.OnNext(Unit.Default);
	subject.OnNext(4);
	subject.OnNext(5);
	subject.OnNext(6);
	subject.OnNext(7);
	subject.OnNext(8);

	subject.OnCompleted();

Output:

<div class="output">
	<div class="line">1</div>
	<div class="line">2</div>
	<div class="line">3</div>
	<div class="line">Completed</div>
</div>

That was our quick run through of the filtering methods available in Rx. 
While they are pretty simple, as we will see, the power in Rx is down to the composability of its operators.

These operators provide a good introduction to the filtering in Rx. 
The filter operators are your first stop for managing the potential deluge of data we can face in the information age. 
We now know how to remove unmatched data, duplicate data or excess data. 
Next, we will move on to the other two sub classifications of the reduction operators, inspection and aggregation.

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