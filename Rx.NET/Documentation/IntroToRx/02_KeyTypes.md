---
title : Key Types
---


#Key types		{#KeyTypes}

To use a framework you need to have a familiarty with the key features and their benefits. 
Without this you find yourself just pasting samples from forums and hacking code until it works, kind of. 
Then the next poor developer to maintain the code base ha to try to figure out what the intention of your code base was. 
Fate is only too kind when that maintenence developer is the same as the original developer.
Rx is powerful, but also allows for a simplification of your code. 
To write good Reactive code you have to know the basics.

There are two key types to understand when working with Rx, and a subset of auxiliary types that will help you to learn Rx more effectively.
The `IObserver<T>` and `IObservable<T>` form the fundamental building blocks for Rx, while implementations of `ISubject<Source, TResult>` reduce the learning curve for developers new to Rx.

Many are familiar with LINQ and its many popular forms like LINQ to Objects, LINQ to SQL &amp; LINQ to XML. 
Each of these common implementations allows you query _data at rest_; Rx offers the ability to query _data in motion_. 
Essentially Rx is built upon the foundations of the [Observer](http://en.wikipedia.org/wiki/Observer_pattern) pattern. 
.NET already exposes some other ways to implement the Observer pattern such as multicast delegates or events (which are usually multicast delegates). 
Multicast delegates are not ideal however as they exhibit the following less desirable features;


 * In C#, events have a curious interface. Some find the `+=` and ` -=` operators an unnatural way to register a callback
 * Events are difficult to compose
 * Events don't offer the ability to be easily queried over time
 * Events are a common cause of accidental memory leaks
 * Events do not have a standard pattern for signaling completion
 * Events provide almost no help for concurrency or multithreaded applications. e.g. To raise an event on a separate thread requires you to do all of the plumbing
 
Rx looks to solve these problems. 
Here I will introduce you to the building blocks and some basic types that make up Rx.

##IObservable&lt;T&gt;			{#IObservable}
    
[`IObservable<T>`](http://msdn.microsoft.com/en-us/library/dd990377.aspx "IObservable(Of T) interface - MSDN") is one of the two new core interfaces for working with Rx. 
It is a simple interface with just a [Subscribe](http://msdn.microsoft.com/en-us/library/dd782981(v=VS.100).aspx) method. 
Microsoft is so confident that this interface will be of use to you it has been included in the BCL as of version 4.0 of .NET. 
You should be able to think of anything that implements `IObservable<T>` as a streaming sequence of `T` objects. 
So if a method returned an `IObservable<Price>` I could think of it as a stream of Prices.

	//Defines a provider for push-based notification.
	public interface IObservable<out T>
	{
		//Notifies the provider that an observer is to receive notifications.
		IDisposable Subscribe(IObserver<T> observer);
	}
	

<p class="comment">
	.NET already has the concept of Streams with the type and sub types of `System.IO.Stream`.
	The `System.IO.Stream` implementations are commonly used to stream data (generally bytes) to or from an I/O device like a file, network or block of memory. 
	`System.IO.Stream` implementations can have both the ability to read and write, and sometimes the ability to seek (i.e. fast forward through a stream or move backwards). 
	When I refer to an instance of `IObservable<T>` as a stream, it does not exhibit the seek or write functionality that streams do. 
	This is a fundamental difference preventing Rx being built on top of the `System.IO.Stream` paradigm. 
	Rx does however have the concept of forward streaming (push), disposing (closing) and completing (eof). 
	Rx also extends the metaphor by introducing concurrency constructs, and query operations like transformation, merging, aggregating and expanding. 
	These features are also not an appropriate fit for the existing `System.IO.Stream` types.
	Some others refer to instances of `IObservable<T>` as Observable Collections, which I find hard to understand. 
	While the observable part makes sense to me, I do not find them like collections at all. 
	You generally cannot sort, insert or remove	items from an `IObservable<T>` instance like I would expect you can	with a collection. 
	Collections generally have some sort of backing store like an internal array. 
	The values from an `IObservable<T>` source are not usually pre-materialized as you would expect from a normal collection. 
	There is also a type in WPF/Silverlight called an `ObservableCollection<T>`	that does exhibit collection-like behavior, and is very well suited to this description.
	In fact `IObservable<T>` integrates very well with `ObservableCollection<T>` instances. 
	So to save on any confusion we will refer to instances of `IObservable<T>` as *sequences*. 
	While instances of `IEnumerable<T>` are also sequences,	we will adopt the convention that they are sequences of _data at rest_, and	`IObservable<T>` instances are sequences of _data in motion_.
</p>

##IObserver&lt;T&gt;			{#IObserver}

[`IObserver<T>`](http://msdn.microsoft.com/en-us/library/dd783449.aspx "IObserver(Of T) interface - MSDN") is the other one of the two core interfaces for working with Rx. 
It too has made it into the BCL as of .NET 4.0. 
Don't worry if you are not on .NET 4.0 yet as the Rx team have included these two interfaces in a separate assembly for .NET 3.5 and Silverlight users. 
`IObservable<T>` is meant to be the &quot;functional dual of `IEnumerable<T>`&quot;.
If you want to know what that last statement means, then enjoy the hours of videos on [Channel9](http://channel9.msdn.com/tags/Rx/) where they discuss the mathematical purity of the types. 
For everyone else it means that where an `IEnumerable<T>` can effectively yield three things (the next value, an exception or the end of the sequence), so too can `IObservable<T>` via `IObserver<T>`'s three methods `OnNext(T)`, `OnError(Exception)` and `OnCompleted()`.

	//Provides a mechanism for receiving push-based notifications.
	public interface IObserver<in T>
	{
		//Provides the observer with new data.
		void OnNext(T value);
		//Notifies the observer that the provider has experienced an error condition.
		void OnError(Exception error);
		//Notifies the observer that the provider has finished sending push-based notifications.
		void OnCompleted();
	}

Rx has an implicit contract that must be followed. An implementation of `IObserver<T>` may have zero or more calls to `OnNext(T)` followed optionally by a call to either `OnError(Exception)` or `OnCompleted()`. 
This protocol ensures that if a sequence terminates, it is always terminated by an `OnError(Exception)`, *or* an `OnCompleted()`. 
This protocol does not however demand that an `OnNext(T)`, `OnError(Exception)` or `OnCompleted()` ever be called. 
This enables to concept of empty and infinite sequences. 
We will look into this more later.

Interestingly, while you will be exposed to the `IObservable<T>` interface frequently if you work with Rx, in general you will not need to be concerned with `IObserver<T>`. 
This is due to Rx providing anonymous implementations via methods like `Subscribe`.

###Implementing IObserver&lt;T&gt; and IObservable&lt;T&gt;			{#ImplementingIObserverAndIObservable}

It is quite easy to implement each interface. 
If we wanted to create an observer that printed values to the console it would be as easy as this.

	public class MyConsoleObserver<T> : IObserver<T>
	{
		public void OnNext(T value)
		{
			Console.WriteLine("Received value {0}", value);
		}

		public void OnError(Exception error)
		{
			Console.WriteLine("Sequence faulted with {0}", error);
		}

		public void OnCompleted()
		{
			Console.WriteLine("Sequence terminated");
		}
	}

Implementing an observable sequence is a little bit harder. 
An overly simplified implementation that returned a sequence of numbers could look like this.

	public class MySequenceOfNumbers : IObservable<int>
	{
		public IDisposable Subscribe(IObserver<int> observer)
		{
			observer.OnNext(1);
			observer.OnNext(2);
			observer.OnNext(3);
			observer.OnCompleted();
			return Disposable.Empty;
		}
	}

We can tie these two implementations together to get the following output

	var numbers = new MySequenceOfNumbers();
	var observer = new MyConsoleObserver<int>();
	numbers.Subscribe(observer);

Output:

<div class="output">
	<div class="line">Received value 1</div>
	<div class="line">Received value 2</div>
	<div class="line">Received value 3</div>
	<div class="line">Sequence terminated</div>
</div>


The problem we have here is that this is not really reactive at all. 
This implementation is blocking, so we may as well use an `IEnumerable<T>` implementation like a `List<T>` or an array.
    
This problem of implementing the interfaces should not concern us too much. 
You will find that when you use Rx, you do not have the need to actually implement these interfaces, Rx provides all of the implementations you need out of the box. 
Let's have a look at the simple ones.


##Subject&lt;T&gt;			{#Subject}
    
I like to think of the `IObserver<T>` and the `IObservable<T>` as the 'reader' and 'writer' or, 'consumer' and 'publisher' interfaces. 
If you were to create your own implementation of `IObservable<T>` you may find that while you want to publicly expose the IObservable characteristics you still need to be able to publish items to the subscribers, throw errors and notify when the sequence is complete. 
Why that sounds just like the methods defined in `IObserver<T>`!
While it may seem odd to have one type implementing both interfaces, it does make life easy. 
This is what [subjects](http://msdn.microsoft.com/en-us/library/hh242969(v=VS.103).aspx "Using Rx Subjects - MSDN") can do for you. 
[`Subject<T>`](http://msdn.microsoft.com/en-us/library/hh229173(v=VS.103).aspx "Subject(Of T) - MSDN") is the most basic of the subjects. 
Effectively you can expose your `Subject<T>` behind a method that returns `IObservable<T>` but internally you can use the `OnNext`, `OnError` and `OnCompleted` methods to control the sequence.

In this very basic example, I create a subject, subscribe to that subject and then publish values to the sequence (by calling `subject.OnNext(T)`).


  static void Main(string[] args)
  {
	var subject = new Subject<string>();
	WriteSequenceToConsole(subject);

	subject.OnNext("a");
	subject.OnNext("b");
	subject.OnNext("c");
	Console.ReadKey();
  }
  
  //Takes an IObservable<string> as its parameter. 
  //Subject<string> implements this interface.
  static void WriteSequenceToConsole(IObservable<string> sequence)
  {
	//The next two lines are equivalent.
	//sequence.Subscribe(value=>Console.WriteLine(value));
	sequence.Subscribe(Console.WriteLine);
  }


Note that the `WriteSequenceToConsole` method takes an `IObservable<string>` as it only wants access to the subscribe method. 
Hang on, doesn't the `Subscribe` method need an `IObserver<string>` as an argument? 
Surely `Console.WriteLine` does not match that interface. 
Well it doesn't, but the Rx team supply me with an Extension Method to `IObservable<T>` that just takes an [`Action<T>`](http://msdn.microsoft.com/en-us/library/018hxwa8.aspx "Action(Of T) Delegate - MSDN").
The action will be executed every time an item is published. 
There are [other overloads to the Subscribe extension method](http://msdn.microsoft.com/en-us/library/system.observableextensions(v=VS.103).aspx "ObservableExtensions class - MSDN") that allows you to pass combinations of delegates to be invoked for `OnNext`, `OnCompleted` and `OnError`. 
This effectively means I don't need to implement `IObserver<T>`.
Cool.

As you can see, `Subject<T>` could be quite useful for getting started in Rx programming. 
`Subject<T>` however, is a basic implementation. 
There are three siblings to `Subject<T>` that offer subtly different implementations which can drastically change the way your program runs.

<!--
	TODO: ReplaySubject<T> - Rewrite second sentence. -GA
-->

##ReplaySubject&lt;T&gt;			{#ReplaySubject}

[`ReplaySubject<T>`](http://msdn.microsoft.com/en-us/library/hh211810(v=VS.103).aspx "ReplaySubject(Of T) - MSDN") provides the feature of caching values and then replaying them for any late subscriptions. 
Consider this example where we have moved our first publication to occur before our subscription

	static void Main(string[] args)
	{
	  var subject = new Subject<string>();

	  subject.OnNext("a");
	  WriteSequenceToConsole(subject);

	  subject.OnNext("b");
	  subject.OnNext("c");
	  Console.ReadKey();
	}

The result of this would be that 'b' and 'c' would be written to the console, but 'a' ignored. 
If we were to make the minor change to make subject a `ReplaySubject<T>` we would see all publications again.

	var subject = new ReplaySubject<string>();

	subject.OnNext("a");
	WriteSequenceToConsole(subject);

	subject.OnNext("b");
	subject.OnNext("c");

This can be very handy for eliminating race conditions. 
Be warned though, the default constructor of the `ReplaySubject<T>` will create an instance that caches every value published to it. 
In many scenarios this could create unnecessary memory pressure on the application. 
`ReplaySubject<T>` allows you to specify simple cache expiry settings that can alleviate this memory issue. 
One option is that you can specify the size of the buffer in the cache. 
In this example we create the `ReplaySubject<T>` with a buffer size of 2, and so only get the last two values published prior to our subscription:
    
	public void ReplaySubjectBufferExample()
	{
		var bufferSize = 2;
		var subject = new ReplaySubject<string>(bufferSize);

		subject.OnNext("a");
		subject.OnNext("b");
		subject.OnNext("c");
		subject.Subscribe(Console.WriteLine);
		subject.OnNext("d");
	}

Here the output would show that the value 'a' had been dropped from the cache, but values 'b' and 'c' were still valid. 
The value 'd' was published after we subscribed so it is also written to the console.
    
<div class="output">
	<div class="line">Output:</div>
	<div class="line">b</div>
	<div class="line">c</div>
	<div class="line">d</div>
</div>

Another option for preventing the endless caching of values by the `ReplaySubject<T>`, is to provide a window for the cache. 
In this example, instead of creating a `ReplaySubject<T>` with a buffer size, we specify a window of time that the cached values are valid for.

	public void ReplaySubjectWindowExample()
	{
		var window = TimeSpan.FromMilliseconds(150);
		var subject = new ReplaySubject<string>(window);

		subject.OnNext("w");
		Thread.Sleep(TimeSpan.FromMilliseconds(100));
		subject.OnNext("x");
		Thread.Sleep(TimeSpan.FromMilliseconds(100));
		subject.OnNext("y");
		subject.Subscribe(Console.WriteLine);
		subject.OnNext("z");
	}

In the above example the window was specified as 150 milliseconds. 
Values are published 100 milliseconds apart. 
Once we have subscribed to the subject, the first value	is 200ms old and as such has expired and been removed from the cache.

<div class="output">
	<div class="line">Output:</div>
	<div class="line">x</div>
	<div class="line">y</div>
	<div class="line">z</div>
</div>

##BehaviorSubject&lt;T&gt;			{#BehaviorSubject}

[`BehaviorSubject<T>`](http://msdn.microsoft.com/en-us/library/hh211949(v=VS.103).aspx "BehaviorSubject(Of T) - MSDN") is similar to `ReplaySubject<T>` except it only remembers the last publication. 
`BehaviorSubject<T>` also requires you to provide it a default value of `T`. 
This means that all subscribers will receive a value immediately (unless it is already completed).

In this example the value 'a' is written to the console:

	public void BehaviorSubjectExample()
	{
		//Need to provide a default value.
		var subject = new BehaviorSubject<string>("a");
		subject.Subscribe(Console.WriteLine);
	}

In this example the value 'b' is written to the console, but not 'a'.

	public void BehaviorSubjectExample2()
	{
	  var subject = new BehaviorSubject<string>("a");
	  subject.OnNext("b");
	  subject.Subscribe(Console.WriteLine);
	}

In this example the values 'b', 'c' &amp; 'd' are all written to the console, but again not 'a'

	public void BehaviorSubjectExample3()
	{
	  var subject = new BehaviorSubject<string>("a");

	  subject.OnNext("b");
	  subject.Subscribe(Console.WriteLine);
	  subject.OnNext("c");
	  subject.OnNext("d");
	}

Finally in this example, no values will be published as the sequence has completed.
Nothing is written to the console.

	public void BehaviorSubjectCompletedExample()
	{
	  var subject = new BehaviorSubject<string>("a");
	  subject.OnNext("b");
	  subject.OnNext("c");
	  subject.OnCompleted();
	  subject.Subscribe(Console.WriteLine);
	}

That note that there is a difference between a `ReplaySubject<T>` with a buffer size of one (commonly called a 'replay one subject') and a `BehaviorSubject<T>`.
A `BehaviorSubject<T>` requires an initial value. 
With the assumption that neither subjects have completed, then you can be sure that the `BehaviorSubject<T>` will have a value. 
You cannot be certain with the `ReplaySubject<T>` however. 
With this in mind, it is unusual to ever complete a `BehaviorSubject<T>`. 
Another difference is that a replay-one-subject will still cache its value once it has been completed.
So subscribing to a completed `BehaviorSubject<T>` we can be sure to not receive any values, but with a `ReplaySubject<T>` it is possible.

`BehaviorSubject<T>`s are often associated with class [properties](http://msdn.microsoft.com/en-us/library/65zdfbdt(v=vs.71).aspx). 
As they always have a value and can provide change notifications, they could be candidates for backing fields to properties.

##AsyncSubject&lt;T&gt;				{#AsyncSubject}

[`AsyncSubject<T>`](http://msdn.microsoft.com/en-us/library/hh229363(v=VS.103).aspx "AsyncSubject(Of T) - MSDN") is similar to the Replay and Behavior subjects in the way that it caches values, however it will only store the last value, and only publish it when the sequence is completed. 
The general usage of the `AsyncSubject<T>` is to only ever publish one value then immediately complete. 
This means that is becomes quite comparable to `Task<T>`.

In this example no values will be published as the sequence never completes. 
No values will be written to the console.

	static void Main(string[] args)
	{
	  var subject = new AsyncSubject<string>();
	  subject.OnNext("a");
	  WriteSequenceToConsole(subject);
	  subject.OnNext("b");
	  subject.OnNext("c");
	  Console.ReadKey();
	}

In this example we invoke the `OnCompleted` method so the last value 'c' is written to the console:

	static void Main(string[] args)
	{
	  var subject = new AsyncSubject<string>();

	  subject.OnNext("a");
	  WriteSequenceToConsole(subject);
	  subject.OnNext("b");
	  subject.OnNext("c");
	  subject.OnCompleted();
	  Console.ReadKey();
	}

##Implicit contracts				{#ImplicitContracts}

There are implicit contacts that need to be upheld when working with Rx as mentioned above. 
The key one is that once a sequence is completed, no more activity can happen on that sequence. 
A sequence can be completed in one of two ways, either by `OnCompleted()` or by `OnError(Exception)`.

The four subjects described in this chapter all cater for this implicit contract by ignoring any attempts to publish values, errors or completions once the sequence has already terminated.

Here we see an attempt to publish the value 'c' on a completed sequence. 
Only values 'a' and 'b' are written to the console.

	public void SubjectInvalidUsageExample()
	{
		var subject = new Subject<string>();

		subject.Subscribe(Console.WriteLine);

		subject.OnNext("a");
		subject.OnNext("b");
		subject.OnCompleted();
		subject.OnNext("c");
	}

##ISubject interfaces				{#ISubject}

While each of the four subjects described in this chapter implement the `IObservable<T>` and `IObserver<T>` interfaces, they do so via another set of interfaces:


	//Represents an object that is both an observable sequence as well as an observer.
	public interface ISubject<in TSource, out TResult> 
		: IObserver<TSource>, IObservable<TResult>
	{
	}

As all the subjects mentioned here have the same type for both `TSource` and `TResult`, they implement this interface which is the superset of all the previous interfaces:

	//Represents an object that is both an observable sequence as well as an observer.
	public interface ISubject<T> : ISubject<T, T>, IObserver<T>, IObservable<T>
	{
	}

These interfaces are not widely used, but prove useful as the subjects do not share a common base class. 
We will see the subject interfaces used later when we discover [Hot and cold observables](14_HotAndColdObservables.html).

##Subject factory					{#SubjectFactory}

Finally it is worth making you aware that you can also create a subject via a factory method. 
Considering that a subject combines the `IObservable<T>` and `IObserver<T>` interfaces, it seems sensible that there should be a factory that allows you to combine them yourself. 
The `Subject.Create(IObserver<TSource>, IObservable<TResult>)` factory method provides just this.

	//Creates a subject from the specified observer used to publish messages to the subject
	//  and observable used to subscribe to messages sent from the subject
	public static ISubject>TSource, TResult< Create>TSource, TResult<(
		IObserver>TSource< observer, 
		IObservable>TResult< observable)
	{...}

Subjects provide a convenient way to poke around Rx, however they are not recommended for day to day use. 
An explanation is in the [Usage Guidelines](18_UsageGuidelines.html) in the appendix. 
Instead of using subjects, favor the factory methods we will look at in [Part 2](04_CreatingObservableSequences.html).

The fundamental types `IObserver<T>` and `IObservable<T>` and the auxiliary subject types create a base from which to build your Rx knowledge.
It is important to understand these simple types and their implicit contracts. 
In production code you may find that you rarely use the `IObserver<T>` interface and subject types, but understanding them and how they fit into the Rx eco-system is still important. 
The `IObservable<T>` interface is the dominant type that you will be exposed to for representing a sequence of data in motion, and therefore will comprise the core concern for most of your work with Rx and most of this book.

---

<div class="webonly">
	<h1 class="ignoreToc">Additional recommended reading</h1>
	<div align="center">
		<!--Head First Design Patterns (Kindle) Amazon.co.uk-->
		<div style="display:inline-block; vertical-align: top; margin: 10px; width: 140px; font-size: 11px; text-align: center">
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=B00AA36RZY&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
					style="width:120px;height:240px;" 
					scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>
		</div>

		<!--Design Patterns (Kindle) Amazon.co.uk-->
		<div style="display:inline-block; vertical-align: top; margin: 10px; width: 140px; font-size: 11px; text-align: center">
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=B000SEIBB8&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
					style="width:120px;height:240px;" 
					scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>
		</div>

		<!--Clean Code (Kindle) Amazon.co.uk-->
		<div style="display:inline-block; vertical-align: top; margin: 10px; width: 140px; font-size: 11px; text-align: center">
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=0132350882&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
					style="width:120px;height:240px;" 
					scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>
		</div>
		
		<!--C# 3.0 Design Patterns (Kindle) Amazon.co.uk-->
		<div style="display:inline-block; vertical-align: top; margin: 10px; width: 140px; font-size: 11px; text-align: center">
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=B0043EWUAC&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
					style="width:120px;height:240px;" 
					scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>
		</div>
	</div></div>
