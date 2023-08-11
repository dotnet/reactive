---
title: Leaving the monad
---

<!--TODO: Enumerators -->

#Leaving the monad					{#LeavingTheMonad}
<!--
        
        TODO: Create a compelling reason that you would want to leave the monad and support 
        it. else Create the argument that you can but don't want to and then prove it.

-->

An observable sequence is a useful construct, especially when we have the power of LINQ to compose complex queries over it. 
Even though we recognize the benefits of the observable sequence, sometimes it is required to leave the `IObservable<T>` paradigm for another paradigm, maybe to enable you to integrate with an existing API (i.e. use events or `Task<T>`). 
You might leave the observable paradigm if you find it easier for testing, or it may simply be easier for you to learn Rx by moving between an observable paradigm and a more familiar one.

##What is a monad					{#WhatIsAMonad}

We have casually referred to the term _monad_ earlier in the book, but to most it will be a very foreign term. 
I am going to try to avoid overcomplicating what a monad is, but give enough of an explanation to help us out with our next category of methods. 
The full definition of a monad is quite abstract. 
[Many others](http://www.haskell.org/haskellwiki/Monad_tutorials_timeline) have tried to provide their definition of a monad using all sorts of metaphors from astronauts to Alice in Wonderland. 
Many of the tutorials for monadic programming use Haskell for the code examples which can add to the confusion.
For us, a monad is effectively a programming structure that represents computations.
Compare this to other programming structures:

<dl>
	<dt>Data structure</dt>
	<dd>
		Purely state e.g. a List, a Tree or a Tuple
	</dd>
	<dt>Contract</dt>
	<dd>
		Contract definition or abstract functionality e.g. an interface or abstract class
	</dd>
	<dt>Object-Orientated structure</dt>
	<dd>
		State and behavior together
	</dd>
</dl>

Generally a monadic structure allows you to chain together operators to produce a pipeline, just as we do with our extension methods.

<cite>Monads are a kind of abstract data type constructor that encapsulate program logic
	instead of data in the domain model. </cite>

This neat definition of a monad lifted from Wikipedia allows us to start viewing sequences as monads; the abstract data type in this case is the `IObservable<T>` type. 
When we use an observable sequence, we compose functions onto the abstract data type (the `IObservable<T>`) to create a query. 
This query becomes our encapsulated programming logic.

The use of monads to define control flows is particularly useful when dealing with typically troublesome areas of programming such as IO, concurrency and exceptions.
This just happens to be some of Rx's strong points!

##Why leave the monad?				{#WhyLeaveTheMonad}

There is a variety of reasons you may want to consume an observable sequence in a different paradigm. 
Libraries that need to expose functionality externally may be required to present it as events or as `Task` instances. 
In demonstration and sample code you may prefer to use blocking methods to limit the number of asynchronous moving parts. 
This may help make the learning curve to Rx a little less steep!

In production code, it is rarely advised to 'break the monad', especially moving from an observable sequence to blocking methods. 
Switching between asynchronous and synchronous paradigms should be done with caution, as this is a common root cause for concurrency problems such as deadlock and scalability issues.

In this chapter, we will look at the methods in Rx which allow you to leave the `IObservable<T>` monad.

##ForEach							{#ForEach}

The `ForEach` method provides a way to process elements as they are received.
The key difference between `ForEach` and `Subscribe` is that `ForEach` will block the current thread until the sequence completes.

	var source = Observable.Interval(TimeSpan.FromSeconds(1))
		.Take(5);
	source.ForEach(i => Console.WriteLine("received {0} @ {1}", i, DateTime.Now));
	Console.WriteLine("completed @ {0}", DateTime.Now);

Output:

<div class="output">
	<div class="line">received 0 @ 01/01/2012 12:00:01 a.m.</div>
	<div class="line">received 1 @ 01/01/2012 12:00:02 a.m.</div>
	<div class="line">received 2 @ 01/01/2012 12:00:03 a.m.</div>
	<div class="line">received 3 @ 01/01/2012 12:00:04 a.m.</div>
	<div class="line">received 4 @ 01/01/2012 12:00:05 a.m.</div>
	<div class="line">completed @ 01/01/2012 12:00:05 a.m.</div>
</div>

Note that the completed line is last, as you would expect. 
To be clear, you can get similar functionality from the `Subscribe` extension method, but the `Subscribe` method will not block. 
So if we substitute the call to `ForEach` with a call to `Subscribe`, we will see the completed line happen first.

	var source = Observable.Interval(TimeSpan.FromSeconds(1))
		.Take(5);
	source.Subscribe(i => Console.WriteLine("received {0} @ {1}", i, DateTime.Now));
	Console.WriteLine("completed @ {0}", DateTime.Now);

Output:

<div class="output">
	<div class="line">completed @ 01/01/2012 12:00:00 a.m.</div>
	<div class="line">received 0 @ 01/01/2012 12:00:01 a.m.</div>
	<div class="line">received 1 @ 01/01/2012 12:00:02 a.m.</div>
	<div class="line">received 2 @ 01/01/2012 12:00:03 a.m.</div>
	<div class="line">received 3 @ 01/01/2012 12:00:04 a.m.</div>
	<div class="line">received 4 @ 01/01/2012 12:00:05 a.m.</div>
</div>

Unlike the `Subscribe` extension method, `ForEach` has only the one overload; the one that take an `Action<T>` as its single argument.
In contrast, previous (pre-release) versions of Rx, the `ForEach` method had most of the same overloads as `Subscribe`. 
Those overloads of `ForEach` have been deprecated, and I think rightly so. 
There is no need to have an `OnCompleted` handler in a synchronous call, it is unnecessary. 
You can just place the call immediately after the `ForEach` call as we have done above. 
Also, the `OnError` handler can now be replaced with standard Structured Exception Handling like you would use for any other synchronous code, with a `try`/`catch` block. 
This also gives symmetry to the `ForEach` instance method on the `List<T>` type.

	var source = Observable.Throw<int>(new Exception("Fail"));
	try
	{
		source.ForEach(Console.WriteLine);
	}
	catch (Exception ex)
	{
		Console.WriteLine("error @ {0} with {1}", DateTime.Now, ex.Message);
	}
	finally
	{
		Console.WriteLine("completed @ {0}", DateTime.Now);    
	}

Output:

<div class="output">
	<div class="line">error @ 01/01/2012 12:00:00 a.m. with Fail</div>
	<div class="line">completed @ 01/01/2012 12:00:00 a.m.</div>
</div>

The `ForEach` method, like its other blocking friends (`First` and `Last` etc.), should be used with care. 
I would leave the `ForEach` method for spikes, tests and demo code only. 
We will discuss the problems with introducing blocking calls when we look at concurrency.

<!--TODO: The  GetEnumerator, Latest, MostRecent and Next operators are not covered. These could be really useful.-->
<!--<a name="ObservableSequencesToEnumerators"></a>
	<h2>Observable sequences to enumerators</h2>
	<p></p>
	<a name="GetEnumerator"></a>
	<h3>GetEnumerator</h3>
	<p></p>
	<a name="Latest"></a>
	<h3>Latest</h3>
	<p></p>
	<a name="MostRecent"></a>
	<h3>MostRecent</h3>
	<p></p>
	<a name="Next"></a>
	<h3>Next</h3>
	<p></p>
-->

##ToEnumerable						{#ToEnumerable}

An alternative way to switch out of the `IObservable<T>` is to call the `ToEnumerable` extension method. 
As a simple example:

	var period = TimeSpan.FromMilliseconds(200);
	var source = Observable.Timer(TimeSpan.Zero, period) 
		.Take(5); 
	var result = source.ToEnumerable();
	foreach (var value in result) 
	{ 
		Console.WriteLine(value); 
	} 
	Console.WriteLine("done");

Output:

<div class="output">
	<div class="line">0</div>
	<div class="line">1</div>
	<div class="line">2</div>
	<div class="line">3</div>
	<div class="line">4</div>
	<div class="line">done</div>
</div>

The source observable sequence will be subscribed to when you start to enumerate the sequence (i.e. lazily). 
In contrast to the `ForEach` extension method, using the `ToEnumerable` method means you are only blocked when you try to move to the next element and it is not available. 
Also, if the sequence produces values faster than you consume them, they will be cached for you.

To cater for errors, you can wrap your `foreach` loop in a `try`/`catch` as you do with any other enumerable sequence:

	try 
	{ 
		foreach (var value in result)
		{ 
			Console.WriteLine(value); 
		} 
	} 
	catch (Exception e) 
	{ 
		Console.WriteLine(e.Message);
	} 

As you are moving from a push to a pull model (non-blocking to blocking), the standard warning applies.

##To a single collection			{#ToBatch}

To avoid having to oscillate between push and pull, you can use one of the next four methods to get the entire list back in a single notification. 
They all have the same semantics, but just produce the data in a different format. 
They are similar to their corresponding `IEnumerable<T>` operators, but the return values differ in order to retain asynchronous behavior.

###ToArray and ToList				{#ToArrayAndToList}

Both `ToArray` and `ToList` take an observable sequence and package it into an array or an instance of `List<T>` respectively. 
Once the observable sequence completes, the array or list will be pushed as the single value of the result sequence.

	var period = TimeSpan.FromMilliseconds(200); 
	var source = Observable.Timer(TimeSpan.Zero, period).Take(5); 
	var result = source.ToArray(); 
	result.Subscribe( 
		arr => { 
			Console.WriteLine("Received array"); 
			foreach (var value in arr) 
			{ 
				Console.WriteLine(value); 
			} 
		}, 
		() => Console.WriteLine("Completed")
	); 
	Console.WriteLine("Subscribed"); 

Output:

<div class="output">
	<div class="line">Subscribed</div>
	<div class="line">Received array</div>
	<div class="line">0</div>
	<div class="line">1</div>
	<div class="line">2</div>
	<div class="line">3</div>
	<div class="line">4</div>
	<div class="line">Completed</div>
</div>

As these methods still return observable sequences we can use our `OnError` handler for errors. 
Note that the source sequence is packaged to a single notification; you either get the whole sequence *or* the error. 
If the source produces values and then errors, you will not receive any of those values. 
All four operators (`ToArray`, `ToList`, `ToDictionary` and `ToLookup`) handle errors like this.

###ToDictionary and ToLookup	{#ToDictionaryAndToLookup}

As an alternative to arrays and lists, Rx can package an observable sequence into a dictionary or lookup with the `ToDictionary` and `ToLookup` methods.
Both methods have the same semantics as the `ToArray` and `ToList` methods, as they return a sequence with a single value and have the same error handling features.

The `ToDictionary` extension method overloads:

	// Creates a dictionary from an observable sequence according to a specified key selector 
	// function, a comparer, and an element selector function.
	public static IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(
		this IObservable<TSource> source, 
		Func<TSource, TKey> keySelector, 
		Func<TSource, TElement> elementSelector, 
		IEqualityComparer<TKey> comparer) 
	{...} 
	// Creates a dictionary from an observable sequence according to a specified key selector 
	// function, and an element selector function. 
	public static IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>( 
		this IObservable<TSource> source, 
		Func<TSource, TKey> keySelector, 
		Func<TSource, TElement> elementSelector) 
	{...} 
	// Creates a dictionary from an observable sequence according to a specified key selector 
	// function, and a comparer. 
	public static IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>( 
		this IObservable<TSource> source, 
		Func<TSource, TKey> keySelector,
		IEqualityComparer<TKey> comparer) 
	{...} 
	// Creates a dictionary from an observable sequence according to a specified key selector 
	// function. 
	public static IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>( 
		this IObservable<TSource> source, 
		Func<TSource, TKey> keySelector) 
	{...} 

The `ToLookup` extension method overloads:

	// Creates a lookup from an observable sequence according to a specified key selector 
	// function, a comparer, and an element selector function. 
	public static IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>( 
		this IObservable<TSource> source, 
		Func<TSource, TKey> keySelector, 
		Func<TSource, TElement> elementSelector,
		IEqualityComparer<TKey> comparer) 
	{...} 
	// Creates a lookup from an observable sequence according to a specified key selector 
	// function, and a comparer. 
	public static IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(
		this IObservable<TSource> source, 
		Func<TSource, TKey> keySelector, 
		IEqualityComparer<TKey> comparer) 
	{...} 
	// Creates a lookup from an observable sequence according to a specified key selector 
	// function, and an element selector function. 
	public static IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>( 
		this IObservable<TSource> source, 
		Func<TSource, TKey> keySelector, 
		Func<TSource, TElement> elementSelector)
	{...} 
	// Creates a lookup from an observable sequence according to a specified key selector 
	// function. 
	public static IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>( 
		this IObservable<TSource> source, 
		Func<TSource,
		TKey> keySelector) 
	{...} 

Both `ToDictionary` and `ToLookup` require a function that can be applied each value to get its key. 
In addition, the `ToDictionary` method overloads mandate that all keys should be unique. 
If a duplicate key is found, it terminate the sequence with a `DuplicateKeyException`. 
On the other hand, the `ILookup<TKey, TElement>` is designed to have multiple values grouped by the key. 
If you have many values per key, then `ToLookup` is probably the better option.

##ToTask							{#ToTask}

We have compared `AsyncSubject<T>` to `Task<T>` and even showed how to [transition from a task](04_CreatingObservableSequences.html#FromTask) to an observable sequence. 
The `ToTask` extension method will allow you to convert an observable sequence into a `Task<T>`. 
Like an `AsyncSubject<T>`, this method will ignore multiple values, only returning the last value.

	// Returns a task that contains the last value of the observable sequence. 
	public static Task<TResult> ToTask<TResult>(
		this IObservable<TResult> observable) 
	{...} 
	// Returns a task that contains the last value of the observable sequence, with state to 
	//  use as the underlying task's AsyncState. 
	public static Task<TResult> ToTask<TResult>(
		this IObservable<TResult> observable,
		object state) 
	{...} 
	// Returns a task that contains the last value of the observable sequence. Requires a 
	//  cancellation token that can be used to cancel the task, causing unsubscription from 
	//  the observable sequence. 
	public static Task<TResult> ToTask<TResult>(
		this IObservable<TResult> observable, 
		CancellationToken cancellationToken) 
	{...} 
	// Returns a task that contains the last value of the observable sequence, with state to 
	//  use as the underlying task's AsyncState. Requires a cancellation token that can be used
	//  to cancel the task, causing unsubscription from the observable sequence. 
	public static Task<TResult> ToTask<TResult>(
		this IObservable<TResult> observable, 
		CancellationToken cancellationToken, 
		object state) 
	{...} 

This is a simple example of how the `ToTask` operator can be used. 
Note, the `ToTask` method is in the `System.Reactive.Threading.Tasks` namespace.

	var source = Observable.Interval(TimeSpan.FromSeconds(1)) 
		.Take(5);
	var result = source.ToTask(); //Will arrive in 5 seconds. 
	Console.WriteLine(result.Result);

Output:

<div class="output">
	<div class="line">4</div>
</div>

If the source sequence was to manifest error then the task would follow the error-handling semantics of tasks.

	var source = Observable.Throw<long>(new Exception("Fail!")); 
	var result = source.ToTask(); 
	try 
	{ 
		Console.WriteLine(result.Result);
	} 
	catch (AggregateException e) 
	{ 
		Console.WriteLine(e.InnerException.Message); 
	}

Output:

<div class="output">
	<div class="line">Fail!</div>
</div>

Once you have your task, you can of course engage in all the features of the TPL such as continuations.

##ToEvent&lt;T&gt;					{#ToEventT}

Just as you can use an event as the source for an observable sequence with [`FromEventPattern`](04_CreatingObservableSequences.html#FromEvent), you can also make your observable sequence look like a standard .NET event with the `ToEvent` extension methods.

	// Exposes an observable sequence as an object with a .NET event. 
	public static IEventSource<unit> ToEvent(this IObservable<Unit> source)
	{...} 
	// Exposes an observable sequence as an object with a .NET event. 
	public static IEventSource<TSource> ToEvent<TSource>(
		this IObservable<TSource> source) 
	{...} 
	// Exposes an observable sequence as an object with a .NET event. 
	public static IEventPatternSource<TEventArgs> ToEventPattern<TEventArgs>(
		this IObservable<EventPattern<TEventArgs>> source) 
		where TEventArgs : EventArgs 
	{...} 

The `ToEvent` method returns an `IEventSource<T>`, which will have a single event member on it: `OnNext`.

	public interface IEventSource<T> 
	{ 
		event Action<T> OnNext; 
	} 

When we convert the observable sequence with the `ToEvent` method, we can just subscribe by providing an `Action<T>`, which we do here with a lambda.

	var source = Observable.Interval(TimeSpan.FromSeconds(1))
		.Take(5); 
	var result = source.ToEvent(); 
	result.OnNext += val => Console.WriteLine(val);

Output:

<div class="output">
	<div class="line">0</div>
	<div class="line">1</div>
	<div class="line">2</div>
	<div class="line">3</div>
	<div class="line">4</div>
</div>

###ToEventPattern					{#ToEventPattern}

Note that this does not follow the standard pattern of events. 
Normally, when you subscribe to an event, you need to handle the `sender` and `EventArgs` parameters. 
In the example above, we just get the value. 
If you want to expose your sequence as an event that follows the standard pattern, you will need to use `ToEventPattern`.

The `ToEventPattern` will take an `IObservable<EventPattern<TEventArgs>>` and convert that into an `IEventPatternSource<TEventArgs>`. 
The public interface for these types is quite simple.

	public class EventPattern<TEventArgs> : IEquatable<EventPattern<TEventArgs>>
		where TEventArgs : EventArgs 
	{ 
		public EventPattern(object sender, TEventArgs e)
		{ 
			this.Sender = sender; 
			this.EventArgs = e; 
		} 
		public object Sender { get; private set; } 
		public TEventArgs EventArgs { get; private set; } 
		//...equality overloads
	} 
	public interface IEventPatternSource<TEventArgs> where TEventArgs : EventArgs
	{ 
		event EventHandler<TEventArgs> OnNext; 
	} 

These look quite easy to work with. 
So if we create an `EventArgs` type and then apply a simple transform using `Select`, we can make a standard sequence fit the pattern.

The `EventArgs` type:

	public class MyEventArgs : EventArgs 
	{ 
		private readonly long _value; 
		public MyEventArgs(long value) 
		{ 
			_value = value; 
		} 
		public long Value 
		{ 
			get { return _value; } 
		} 
	} 

The transform:

	var source = Observable.Interval(TimeSpan.FromSeconds(1))
		.Select(i => new EventPattern<MyEventArgs>(this, new MyEventArgs(i)));

Now that we have a sequence that is compatible, we can use the `ToEventPattern`, and in turn, a standard event handler.

	var result = source.ToEventPattern(); 
	result.OnNext += (sender, eventArgs) => Console.WriteLine(eventArgs.Value);

Now that we know how to get back into .NET events, let's take a break and remember why Rx is a better model.

 * In C#, events have a curious interface. Some find the `+=` and `-=` operators an unnatural way to register a callback
 * Events are difficult to compose
 * Events do not offer the ability to be easily queried over time
 * Events are a common cause of accidental memory leaks
 * Events do not have a standard pattern for signaling completion
 * Events provide almost no help for concurrency or multithreaded applications. For instance, raising an event on a separate thread requires you to do all of the plumbing

---

The set of methods we have looked at in this chapter complete the circle started in the [Creating a Sequence](04_CreatingObservableSequences.html#TransitioningIntoIObservable) chapter. 
We now have the means to enter and leave the observable sequence monad. 
Take care when opting in and out of the `IObservable<T>` monad. 
Doing so excessively can quickly make a mess of your code base, and may indicate a design flaw.

---

<div class="webonly">
	<h1 class="ignoreToc">Additional recommended reading</h1>
	<div align="center">
		<div style="display:inline-block; vertical-align: top; margin: 10px; width: 140px; font-size: 11px; text-align: center">
			<!--Domain Driven Design (Kindle) Amazon.co.uk-->
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=B00794TAUG&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
					style="width:120px;height:240px;" 
					scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>
		</div>
		<div style="display:inline-block; vertical-align: top; margin: 10px; width: 140px; font-size: 11px; text-align: center">
			<!--Purely functional data structures Amazon.co.uk-->
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=0521663504&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
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
			<!--Real-world functional programming Amazon.co.uk-->
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=1933988924&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
					style="width:120px;height:240px;margin: 10px" 
					scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>

		</div>           
	</div></div>