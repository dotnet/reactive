---
title : Creating a sequence
---

<!--
        LC
        Introduce the concept of Lazy. Contrast this to the recently mentioned earger disposal. Lazy eval + eager disposal = miniumn resource lifetime.
        Check for And also, and then continuations through out. Ensure that we are creating sensible an obvious breaks in passage.

        G.A.
I've been up for a while so it's probably me fucking up something obvious, but the code as is gives me this error:
Cannot convert lambda expression to delegate type 'System.Func<System.IObserver<string>,System.Action>' because some of the return types in the block are not implicitly convertible to the delegate return type
 
And if I use the Action variant instead I get:
Cannot convert lambda expression to delegate type 'System.Func<System.IObserver<string>,System.IDisposable>' because some of the return types in the block are not implicitly convertible to the delegate return type
 
-->

#PART 2 - Sequence basics		{#PART2 .SectionHeader}

So you want to get involved and write some Rx code, but how do you get started?
We have looked at the key types, but know that we should not be creating our own implementations of `IObserver<T>` or `IObservable<T>` and should favor factory methods over using subjects. 
Even if we have an observable sequence, how do we pick out the data we want from it? 
We need to understand the basics of creating an observable sequence, getting values into it and picking out the values we want from them.

In Part 2 we discover the basics for constructing and querying observable sequences.
We assert that LINQ is fundamental to using and understanding Rx. 
On deeper inspection, we find that _functional programming_ concepts are core to having a deep understanding of LINQ and therefore enabling you to master Rx. 
To support this understanding, we classify the query operators into three main groups. 
Each of these groups proves to have a root operator that the other operators can be constructed from. 
Not only will this deconstruction exercise provide a deeper insight to Rx, functional programming and query composition; it should arm you with the ability to create custom operators where the general Rx operators do not meet your needs.

#Creating a sequence				{#CreationOfObservables}

In the previous chapters we used our first Rx extension method, the `Subscribe` method and its overloads. 
We also have seen our first factory method in `Subject.Create()`. 
We will start looking at the vast array of other methods that enrich `IObservable<T>` to make Rx what it is. 
It may be surprising to see that there are relatively few public instance methods in the Rx library. 
There are however a large number of public static methods, and more specifically, a large number of extension methods. 
Due to the large number of methods and their overloads, we will break them down into categories.

<p class="comment">
	Some readers may feel that they can skip over parts of the next few chapters. 
	I would only suggest doing so if you are very confident with LINQ and functional composition.
	The intention of this book is to provide a step-by-step introduction to Rx, with the goal of you, the reader, being able to apply Rx to your software. 
	The appropriate	application of Rx will come through a sound understanding of the fundamentals of Rx. 
	The most common mistakes people will make with Rx are due to a misunderstanding of the principles upon which Rx was built. 
	With this in mind, I encourage you to read on.
</p>

It seems sensible to follow on from our examination of our key types where we simply constructed new instances of subjects. 
Our first category of methods will be _creational_ methods: simple ways we can create instances of `IObservable<T>` sequences.
These methods generally take a seed to produce a sequence: either a single value of a type, or just the type itself. 
In functional programming this can be described as _anamorphism_ or referred to as an '_unfold_'.

##Simple factory methods			{#SimpleFactoryMethods}

###Observable.Return				{#ObservableReturn}

In our first and most basic example we introduce `Observable.Return<T>(T value)`.
This method takes a value of `T` and returns an `IObservable<T>` with the single value and then completes. 
It has _unfolded_ a value of `T` into an observable sequence.

	var singleValue = Observable.Return<string>("Value");
	//which could have also been simulated with a replay subject
	var subject = new ReplaySubject<string>();
	subject.OnNext("Value");
	subject.OnCompleted();

Note that in the example above that we could use the factory method or get the same effect by using the replay subject. 
The obvious difference is that the factory method is only one line and it allows for declarative over imperative programming style.
In the example above we specified the type parameter as `string`, this is not necessary as it can be inferred from the argument provided.

	singleValue = Observable.Return<string>("Value");
	//Can be reduced to the following
	singleValue = Observable.Return("Value");

###Observable.Empty					{#ObservableEmpty}

The next two examples only need the type parameter to unfold into an observable sequence. 
The first is `Observable.Empty<T>()`. 
This returns an empty `IObservable<T>` i.e. it just publishes an `OnCompleted` notification.

	var empty = Observable.Empty<string>();
	//Behaviorally equivalent to
	var subject = new ReplaySubject<string>();
	subject.OnCompleted();

###Observable.Never					{#ObservableNever}

The `Observable.Never<T>()` method will return infinite sequence without any notifications.

	var never = Observable.Never<string>();
	//similar to a subject without notifications
	var subject = new Subject<string>();

###Observable.Throw					{#ObservableThrow}

`Observable.Throw<T>(Exception)` method needs the type parameter information, it also need the `Exception` that it will `OnError` with. 
This method creates a sequence with just a single `OnError` notification containing the	exception passed to the factory.


	var throws = Observable.Throw<string>(new Exception()); 
	//Behaviorally equivalent to
	var subject = new ReplaySubject<string>(); 
	subject.OnError(new Exception());

###Observable.Create				{#ObservableCreate}

The `Create` factory method is a little different to the above creation methods.
The method signature itself may be a bit overwhelming at first, but becomes quite natural once you have used it.

	//Creates an observable sequence from a specified Subscribe method implementation.
	public static IObservable<TSource> Create<TSource>(
		Func<IObserver<TSource>, IDisposable> subscribe)
	{...}
	public static IObservable<TSource> Create<TSource>(
		Func<IObserver<TSource>, Action> subscribe)
	{...}

Essentially this method allows you to specify a delegate that will be executed anytime a subscription is made. 
The `IObserver<T>` that made the subscription will be passed to your delegate so that you can call the `OnNext`/`OnError`/`OnCompleted` methods as you need. 
This is one of the few scenarios where you will need to concern yourself with the `IObserver<T>` interface. 
Your delegate is a `Func` that returns an `IDisposable`. 
This `IDisposable` will have its `Dispose()` method called when the subscriber disposes from their subscription.

The `Create` factory method is the preferred way to implement custom observable sequences. 
The usage of subjects should largely remain in the realms of samples and testing. 
Subjects are a great way to get started with Rx. 
They reduce the learning curve for new developers, however they pose several concerns that the `Create` method eliminates. 
Rx is effectively a functional programming paradigm. Using subjects means we are now managing state, which is potentially mutating. 
Mutating state and asynchronous programming are very hard to get right. 
Furthermore many of the operators (extension methods) have been carefully written to ensure correct and consistent lifetime of subscriptions and sequences are maintained. 
When you introduce subjects you can break this. 
Future releases may also see significant performance degradation if you explicitly use subjects.

The `Create` method is also preferred over creating custom types that implement the `IObservable` interface. 
There really is no need to implement the observer/observable interfaces yourself. 
Rx tackles the intricacies that you may not think of such as thread safety of notifications and subscriptions.

A significant benefit that the `Create` method has over subjects is that the sequence will be lazily evaluated. 
Lazy evaluation is a very important part of Rx. 
It opens doors to other powerful features such as scheduling and combination of sequences that we will see later. 
The delegate will only be invoked when a subscription is made.

In this example we show how we might first return a sequence via standard blocking eagerly evaluated call, and then we show the correct way to return an observable sequence without blocking by lazy evaluation.

	private IObservable<string> BlockingMethod()
	{
		var subject = new ReplaySubject<string>();
		subject.OnNext("a");
		subject.OnNext("b");
		subject.OnCompleted();
		Thread.Sleep(1000);
		return subject;
	}
	private IObservable<string> NonBlocking()
	{
		return Observable.Create<string>(
			(IObserver<string> observer) =>
			{
				observer.OnNext("a");
				observer.OnNext("b");
				observer.OnCompleted();
				Thread.Sleep(1000);
				return Disposable.Create(() => Console.WriteLine("Observer has unsubscribed"));
				//or can return an Action like 
				//return () => Console.WriteLine("Observer has unsubscribed"); 
			});
	}

While the examples are somewhat contrived, the intention is to show that when a consumer calls the eagerly evaluated, blocking method, they will be blocked for at least 1 second before they even receive the `IObservable<string>`, regardless of if they do actually subscribe to it or not. 
The non blocking method is lazily evaluated so the consumer immediately receives the `IObservable<string>` and will only incur the cost of the thread sleep if they subscribe.

As an exercise, try to build the `Empty`, `Return`, `Never` &amp; `Throw` extension methods yourself using the `Create` method.
If you have Visual Studio or [LINQPad](http://www.linqpad.net/) available to you right now, code it up as quickly as you can. 
If you don't (perhaps you are on the train on the way to work), try to conceptualize how you would solve this problem. 
When you are done move forward to see some examples of how it could be done...

<hr style="page-break-after: always" />

Examples of `Empty`, `Return`, `Never` and `Throw` recreated with `Observable.Create`:

	public static IObservable<T> Empty<T>()
	{
		return Observable.Create<T>(o =>
		{
			o.OnCompleted();
			return Disposable.Empty;
		});
	}
	public static IObservable<T> Return<T>(T value)
	{
		return Observable.Create<T>(o =>
		{
			o.OnNext(value);
			o.OnCompleted();
			return Disposable.Empty;
		});
	}
	public static IObservable<T> Never<T>()
	{
		return Observable.Create<T>(o =>
		{
			return Disposable.Empty;
		});
	}
	public static IObservable<T> Throws<T>(Exception exception)
	{
		return Observable.Create<T>(o =>
		{
			o.OnError(exception);
			return Disposable.Empty;
		});
	}

You can see that `Observable.Create` provides the power to build our own factory methods if we wish. 
You may have noticed that in each of the examples we only are able to return our subscription token (the implementation of `IDisposable`) once we have produced all of our `OnNext` notifications. 
This is because inside of the delegate we provide, we are completely sequential. 
It also makes the token rather pointless. 
Now we look at how we can use the return value in a more useful way. 
First is an example where inside our delegate we create a Timer that will call the observer's `OnNext` each time the timer ticks.

	//Example code only
	public void NonBlocking_event_driven()
	{
		var ob = Observable.Create<string>(
			observer =>
			{
				var timer = new System.Timers.Timer();
				timer.Interval = 1000;
				timer.Elapsed += (s, e) => observer.OnNext("tick");
				timer.Elapsed += OnTimerElapsed;
				timer.Start();
				
				return Disposable.Empty;
			});
		var subscription = ob.Subscribe(Console.WriteLine);
		Console.ReadLine();
		subscription.Dispose();
	}
	private void OnTimerElapsed(object sender, ElapsedEventArgs e)
	{
		Console.WriteLine(e.SignalTime);
	}

Output:

<div class="output">
	<div class="line">tick</div>
	<div class="line">01/01/2012 12:00:00</div>
	<div class="line">tick</div>
	<div class="line">01/01/2012 12:00:01</div>
	<div class="line">tick</div>
	<div class="line">01/01/2012 12:00:02</div>
	<div class="line"></div>
	<div class="line">01/01/2012 12:00:03</div>
	<div class="line">01/01/2012 12:00:04</div>
	<div class="line">01/01/2012 12:00:05</div>
</div>

The example above is broken. 
When we dispose of our subscription, we will stop seeing "tick" being written to the screen; however we have not released our second event handler "`OnTimerElasped`" and have not disposed of the instance of the timer, so it will still be writing the `ElapsedEventArgs.SignalTime` to the console after our disposal. 
The extremely simple fix is to return `timer` as the `IDisposable` token.

	//Example code only
	var ob = Observable.Create<string>(
		observer =>
		{
			var timer = new System.Timers.Timer();
			timer.Interval = 1000;
			timer.Elapsed += (s, e) => observer.OnNext("tick");
			timer.Elapsed += OnTimerElapsed;
			timer.Start();
				
			return timer;
		});

Now when a consumer disposes of their subscription, the underlying `Timer` will be disposed of too.

`Observable.Create` also has an overload that requires your `Func` to return an `Action` instead of an `IDisposable`. 
In a similar example to above, this one shows how you could use an action to un-register the event handler, preventing a memory leak by retaining the reference to the timer.

	//Example code only
	var ob = Observable.Create<string>(
		observer =>
		{
			var timer = new System.Timers.Timer();
			timer.Enabled = true;
			timer.Interval = 100;
			timer.Elapsed += OnTimerElapsed;
			timer.Start();
				
			return ()=>{
				timer.Elapsed -= OnTimerElapsed;
				timer.Dispose();
			};
		});

These last few examples showed you how to use the `Observable.Create` method.
These were just examples; there are actually better ways to produce values from a timer that we will look at soon. 
The intention is to show that `Observable.Create` provides you a lazily evaluated way to create observable sequences. 
We will dig much deeper into lazy evaluation and application of the `Create` factory method throughout the book especially when we cover concurrency and scheduling.

##Functional unfolds				{#Unfold}

As a functional programmer you would come to expect the ability to unfold a potentially	infinite sequence. 
An issue we may face with `Observable.Create` is that is that it can be a clumsy way to produce an infinite sequence. 
Our timer example above is an example of an infinite sequence, and while this is a simple implementation it is an annoying amount of code for something that effectively is delegating all the work to the `System.Timers.Timer` class. 
The `Observable.Create` method also has poor support for unfolding sequences using corecursion.

###Corecursion						{#Corecursion}

Corecursion is a function to apply to the current state to produce the next state.
Using corecursion by taking a value, applying a function to it that extends that value and repeating we can create a sequence. 
A simple example might be to take the value 1 as the seed and a function that increments the given value by one. 
This could be used to create sequence of [1,2,3,4,5...].

Using corecursion to create an `IEnumerable<int>` sequence is made simple with the `yield return` syntax.

	private static IEnumerable<T> Unfold<T>(T seed, Func<T, T> accumulator)
	{
		var nextValue = seed;
		while (true)
		{
			yield return nextValue;
			nextValue = accumulator(nextValue);
		}
	}

The code above could be used to produce the sequence of natural numbers like this.

	var naturalNumbers = Unfold(1, i => i + 1);
	Console.WriteLine("1st 10 Natural numbers");
	foreach (var naturalNumber in naturalNumbers.Take(10))
	{
		Console.WriteLine(naturalNumber);
	}

Output:

<div class="output">
	<div class="line">1st 10 Natural numbers</div>
	<div class="line">1</div>
	<div class="line">2</div>
	<div class="line">3</div>
	<div class="line">4</div>
	<div class="line">5</div>
	<div class="line">6</div>
	<div class="line">7</div>
	<div class="line">8</div>
	<div class="line">9</div>
	<div class="line">10</div>
</div>

Note the `Take(10)` is used to terminate the infinite sequence.

Infinite and arbitrary length sequences can be very useful. 
First we will look at some that come with Rx and then consider how we can generalize the creation of infinite observable sequences.

###Observable.Range					{#ObservableRange}

`Observable.Range(int, int)` simply returns a range of integers. 
The first integer is the initial value and the second is the number of values to yield. 
This example will write the values '10' through to '24' and then complete.

	var range = Observable.Range(10, 15);
	range.Subscribe(Console.WriteLine, ()=>Console.WriteLine("Completed"));

###Observable.Generate				{#ObservableGenerate}

It is difficult to emulate the `Range` factory method using `Observable.Create`.
It would be cumbersome to try and respect the principles that the code should be lazily evaluated and the consumer should be able to dispose of the subscription resources when they so choose. 
This is where we can use corecursion to provide a richer unfold. 
In Rx the unfold method is called `Observable.Generate`.

The simple version of `Observable.Generate` takes the following parameters:

 * an initial state
 * a predicate that defines when the sequence should terminate
 * a function to apply to the current state to produce the next state
 * a function to transform the state to the desired output

	public static IObservable<TResult> Generate<TState, TResult>(
		TState initialState, 
		Func<TState, bool> condition, 
		Func<TState, TState> iterate, 
		Func<TState, TResult> resultSelector)

As an exercise, write your own `Range` factory method using `Observable.Generate`.

Consider the `Range` signature `Range(int start, int count)`, which provides the seed and a value for the conditional predicate. 
You know how each new value is derived from the previous one; this becomes your iterate function.
Finally, you probably don't need to transform the state so this makes the result selector function very simple.

Continue when you have built your own version...

<hr style="page-break-after: always" />

Example of how you could use `Observable.Generate` to construct a similar `Range` factory method.


	//Example code only
	public static IObservable<int> Range(int start, int count)
	{
		var max = start + count;
		return Observable.Generate(
			start, 
			value => value < max, 
			value => value + 1, 
			value => value);
	}
	
###Observable.Interval				{#ObservableInterval}

Earlier in the chapter we used a `System.Timers.Timer` in our observable to generate a continuous sequence of notifications. 
As mentioned in the example at the time, this is not the preferred way of working with timers in Rx. 
As Rx provides operators that give us this functionality it could be argued that to not use them is to re-invent the wheel. 
More importantly the Rx operators are the preferred way of working with timers due to their ability to substitute in schedulers which is desirable for easy substitution of the underlying timer. 
There are at least three various timers you could choose from for the example above:

 * `System.Timers.Timer`
 * `System.Threading.Timer`
 * `System.Windows.Threading.DispatcherTimer`

By abstracting the timer away via a scheduler we are able to reuse the same code for multiple platforms. 
More importantly than being able to write platform independent code is the ability to substitute in a test-double scheduler/timer to enable testing.
Schedulers are a complex subject that is out of scope for this chapter, but they are covered in detail in the later chapter on [Scheduling and threading](15_SchedulingAndThreading.html).

There are three better ways of working with constant time events, each being a further generalization of the former. 
The first is `Observable.Interval(TimeSpan)` which will publish incremental values starting from zero, based on a frequency of your choosing. 
This example publishes values every 250 milliseconds.


	var interval = Observable.Interval(TimeSpan.FromMilliseconds(250));
	interval.Subscribe(
		Console.WriteLine, 
		() => Console.WriteLine("completed"));

Output:

<div class="output">
	<div class="line">0</div>
	<div class="line">1</div>
	<div class="line">2</div>
	<div class="line">3</div>
	<div class="line">4</div>
	<div class="line">5</div>
</div>

Once subscribed, you must dispose of your subscription to stop the sequence. 
It is an example of an infinite sequence.

###Observable.Timer					{#ObservableTimer}

The second factory method for producing constant time based sequences is `Observable.Timer`.
It has several overloads; the first of which we will look at being very simple.
The most basic overload of `Observable.Timer` takes just a `TimeSpan` as `Observable.Interval` does. 
The `Observable.Timer` will however only publish one value (0) after the period of time has elapsed, and then it will complete.

	var timer = Observable.Timer(TimeSpan.FromSeconds(1));
	timer.Subscribe(
		Console.WriteLine, 
		() => Console.WriteLine("completed"));

Output:

<div class="output">
	<div class="line">0</div>
	<div class="line">completed</div>
</div>

Alternatively, you can provide a `DateTimeOffset` for the `dueTime` parameter. 
This will produce the value 0 and complete at the due time.

A further set of overloads adds a `TimeSpan` that indicates the period to produce subsequent values. 
This now allows us to produce infinite sequences and also construct `Observable.Interval` from `Observable.Timer`.

	public static IObservable<long> Interval(TimeSpan period)
	{
		return Observable.Timer(period, period);
	}


Note that this now returns an `IObservable` of `long` not `int`.
While `Observable.Interval` would always wait the given period before producing the first value, this `Observable.Timer` overload gives the ability to start the sequence when you choose. 
With `Observable.Timer` you can write the following to have an interval sequence that started immediately.


	Observable.Timer(TimeSpan.Zero, period);


This takes us to our third way and most general way for producing timer related sequences, back to `Observable.Generate`. 
This time however, we are looking at a more complex overload that allows you to provide a function that specifies the due time for the next value.

	public static IObservable<TResult> Generate<TState, TResult>(
		TState initialState, 
		Func<TState, bool> condition, 
		Func<TState, TState> iterate, 
		Func<TState, TResult> resultSelector, 
		Func<TState, TimeSpan> timeSelector)

Using this overload, and specifically the extra `timeSelector` argument, we can produce our own implementation of `Observable.Timer` and in turn, `Observable.Interval`.

	public static IObservable<long> Timer(TimeSpan dueTime)
	{
		return Observable.Generate(
			0l,
			i => i < 1,
			i => i + 1,
			i => i,
			i => dueTime);
	}
	public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period)
	{
		return Observable.Generate(
			0l,
			i => true,
			i => i + 1,
			i => i,
			i => i == 0 ? dueTime : period);
	}
	public static IObservable<long> Interval(TimeSpan period)
	{
		return Observable.Generate(
			0l,
			i => true,
			i => i + 1,
			i => i,
			i => period);
	}

This shows how you can use `Observable.Generate` to produce infinite sequences.
I will leave it up to you the reader, as an exercise using `Observable.Generate`, to produce values at variable rates. 
I find using these methods invaluable not only in day to day work but especially for producing dummy data.

##Transitioning into IObservable&lt;T&gt;		{#TransitioningIntoIObservable}

Generation of an observable sequence covers the complicated aspects of functional programming i.e. corecursion and unfold. 
You can also start a sequence by simply making a transition from an existing synchronous or asynchronous paradigm into the Rx paradigm.

###From delegates					{#ObservableStart}

The `Observable.Start` method allows you to turn a long running `Func<T>` or `Action` into a single value observable sequence. 
By default, the processing will be done asynchronously on a ThreadPool thread. 
If the overload you use is a `Func<T>` then the return type will be `IObservable<T>`.
When the function returns its value, that value will be published and then the sequence completed. 
If you use the overload that takes an `Action`, then the returned sequence will be of type `IObservable<Unit>`. 
The `Unit` type is a functional programming construct and is analogous to `void`. 
In this case `Unit` is used to publish an acknowledgement that the `Action` is complete, however this is rather inconsequential as the sequence is immediately completed straight after `Unit` anyway. 
The `Unit` type itself has no value; it just serves as an empty payload for the `OnNext` notification.
Below is an example of using both overloads.

	static void StartAction()
	{
		var start = Observable.Start(() =>
			{
				Console.Write("Working away");
				for (int i = 0; i < 10; i++)
				{
					Thread.Sleep(100);
					Console.Write(".");
				}
			});
		start.Subscribe(
			unit => Console.WriteLine("Unit published"), 
			() => Console.WriteLine("Action completed"));
	}
	static void StartFunc()
	{
		var start = Observable.Start(() =>
		{
			Console.Write("Working away");
			for (int i = 0; i < 10; i++)
			{
				Thread.Sleep(100);
				Console.Write(".");
			}
			return "Published value";
		});
		start.Subscribe(
			Console.WriteLine, 
			() => Console.WriteLine("Action completed"));
	}
	
Note the difference between `Observable.Start` and `Observable.Return`; `Start` lazily evaluates the value from a function, `Return` provided the value eagerly. This makes `Start` very much like a `Task`. 
This can also lead to some confusion on when to use each of the features. 
Both are valid tools and the choice come down to the context of the problem space. 
Tasks are well suited to parallelizing computational work and providing workflows via continuations for computationally heavy work. 
Tasks also have the benefit of documenting and enforcing single value semantics. 
Using `Start` is a good way to integrate computationally heavy work into an existing code base that is largely made up of observable sequences.
We look at [composition of sequences](12_CombiningSequences.html) in more depth later in the book.

###From events						{#FromEvent}

As we discussed early in the book, .NET already has the event model for providing a reactive, event driven programming model. 
While Rx is a more powerful and useful framework, it is late to the party and so needs to integrate with the existing event model. 
Rx provides methods to take an event and turn it into an observable sequence.
There are several different varieties you can use. 
Here is a selection of common event patterns.

	//Activated delegate is EventHandler
	var appActivated = Observable.FromEventPattern(
			h => Application.Current.Activated += h,
			h => Application.Current.Activated -= h);
	//PropertyChanged is PropertyChangedEventHandler
	var propChanged = Observable.FromEventPattern
		<PropertyChangedEventHandler, PropertyChangedEventArgs>(
			handler => handler.Invoke,
			h => this.PropertyChanged += h,
			h => this.PropertyChanged -= h);
			
	//FirstChanceException is EventHandler<FirstChanceExceptionEventArgs>
	var firstChanceException = Observable.FromEventPattern<FirstChanceExceptionEventArgs>(
			h => AppDomain.CurrentDomain.FirstChanceException += h,
			h => AppDomain.CurrentDomain.FirstChanceException -= h);      


So while the overloads can be confusing, they key is to find out what the event's signature is. 
If the signature is just the base `EventHandler` delegate then you can use the first example. 
If the delegate is a sub-class of the `EventHandler`, then you need to use the second example and provide the `EventHandler` sub-class and also its specific type of `EventArgs`. 
Alternatively, if the delegate is the newer generic `EventHandler<TEventArgs>`, then you need to use the third example and just specify what the generic type of the event argument is.

It is very common to want to expose property changed events as observable sequences.
These events can be exposed via `INotifyPropertyChanged` interface, a `DependencyProperty` or perhaps by events named appropriately to the Property they are representing.
If you are looking at writing your own wrappers to do this sort of thing, I would strongly suggest looking at the Rxx library on [http://Rxx.codeplex.com](http://Rxx.codeplex.com) first. 
Many of these have been catered for in a very elegant fashion.

###From Task						{#FromTask}

Rx provides a useful, and well named set of overloads for transforming from other existing paradigms to the Observable paradigm. 
The `ToObservable()` method overloads provide a simple route to make the transition.

As we mentioned earlier, the `AsyncSubject<T>` is similar to a `Task<T>`.
They both return you a single value from an asynchronous source. 
They also both cache the result for any repeated or late requests for the value. 
The first `ToObservable()` extension method overload we look at is an extension to `Task<T>`.
The implementation is simple;


 * if the task is already in a status of `RanToCompletion` then the value is added to the sequence and then the sequence completed
 * if the task is Cancelled then the sequence will error with a `TaskCanceledException`
 * if the task is Faulted then the sequence will error with the task's inner exception
 * if the task has not yet completed, then a continuation is added to the task to perform the above actions appropriately

There are two reasons to use the extension method:

 * From Framework 4.5, almost all I/O-bound functions return `Task<T>`
 * If `Task<T>` is a good fit, it's preferable to use it over `IObservable<T>` - because it communicates single-value result in the type system. 
In other words,	a function that returns a single value in the future should return a `Task<T>`,	not an `IObservable<T>`. 
Then if you need to combine it with other observables, use `ToObservable()`.

Usage of the extension method is also simple.

	var t = Task.Factory.StartNew(()=>"Test");
	var source = t.ToObservable();
	source.Subscribe(
		Console.WriteLine,
		() => Console.WriteLine("completed"));

Output:

<div class="output">
	<div class="line">Test</div>
	<div class="line">completed</div>
</div>

There is also an overload that converts a `Task` (non generic) to an `IObservable<Unit>`.

###From IEnumerable&lt;T&gt;		{#FromIEnumerable}

The final overload of `ToObservable` takes an `IEnumerable<T>`.
This is semantically like a helper method for an `Observable.Create` with a `foreach` loop in it.

	//Example code only
	public static IObservable<T> ToObservable<T>(this IEnumerable<T> source)
	{
		return Observable.Create<T>(o =>
		{
			foreach (var item in source)
			{
				o.OnNext(item);
			}
			//Incorrect disposal pattern
			return Disposable.Empty;
		});
	}

This crude implementation however is naive. 
It does not allow for correct disposal, it does not handle exceptions correctly and as we will see later in the book, it does not have a very nice concurrency model. 
The version in Rx of course caters for all of these tricky details so you don't need to worry.

When transitioning from `IEnumerable<T>` to `IObservable<T>`, you should carefully consider what you are really trying to achieve. 
You should also carefully test and measure the performance impacts of your decisions. 
Consider that the blocking synchronous (pull) nature of `IEnumerable<T>` sometimes just does not mix well with the asynchronous (push) nature of `IObservable<T>`.
Remember that it is completely valid to pass `IEnumerable`, `IEnumerable<T>`, arrays or collections as the data type for an observable sequence. 
If the sequence can be materialized all at once, then you may want to avoid exposing it as an `IEnumerable`.
If this seems like a fit for you then also consider passing immutable types like an array or a `ReadOnlyCollection<T>`. 
We will see the use of `IObservable<IList<T>>` later for operators that provide batching of data.

###From APM							{#FromAPM}

Finally we look at a set of overloads that take you from the [Asynchronous Programming Model](http://msdn.microsoft.com/en-us/magazine/cc163467.aspx) (APM) to an observable sequence. 
This is the style of programming found in .NET that can be identified with the use of two methods prefixed with `Begin...` and `End...` and the iconic `IAsyncResult` parameter type. 
This is commonly seen in the I/O APIs.

	class webrequest
	{    
		public webresponse getresponse() 
		{...}
		public iasyncresult begingetresponse(
			asynccallback callback, 
			object state) 
		{...}
		public webresponse endgetresponse(iasyncresult asyncresult) 
		{...}
		...
	}
	class stream
	{
		public int read(
			byte[] buffer, 
			int offset, 
			int count) 
		{...}
		public iasyncresult beginread(
			byte[] buffer, 
			int offset, 
			int count, 
			asynccallback callback, 
			object state) 
		{...}
		public int endread(iasyncresult asyncresult) 
		{...}
		...
	}
</pre>
<p class="comment">
	At time of writing .NET 4.5 was still in preview release. 
	Moving forward with .NET 4.5 the APM model will be replaced with `Task` and new `async` and `await` keywords. 
	Rx 2.0 which is also in a beta release will integrate with these features. .NET 4.5 and Rx 2.0 are not in the scope of this book.
</p>

APM, or the Async Pattern, has enabled a very powerful, yet clumsy way of for .NET programs to perform long running I/O bound work. 
If we were to use the synchronous access to IO, e.g. `WebRequest.GetResponse()` or `Stream.Read(...)`, we would be blocking a thread but not performing any work while we waited for the IO. 
This can be quite wasteful on busy servers performing a lot of concurrent work to hold a thread idle while waiting for I/O to complete. 
Depending on the implementation, APM can work at the hardware device driver layer and not require any threads while blocking. 
Information on how to follow the APM model is scarce. 
Of the documentation you can find it is pretty shaky, however, for more information on APM, see Jeffrey Richter's brilliant book <cite>CLR via C#</cite> or Joe Duffy's comprehensive <cite>Concurrent Programming on Windows</cite>. 
Most stuff on the internet is blatant plagiary of Richter's examples from his book. 
An in-depth examination of APM is outside of the scope of this book.

To utilize the Asynchronous Programming Model but avoid its awkward API, we can use the `Observable.FromAsyncPattern` method. 
Jeffrey van Gogh gives a brilliant walk through of the `Observable.FromAsyncPattern` in [Part 1](http://blogs.msdn.com/b/jeffva/archive/2010/07/23/rx-on-the-server-part-1-of-n-asynchronous-system-io-stream-reading.aspx) of his <cite>Rx on the Server</cite> blog series. 
While the theory backing the Rx on the Server series is sound, it was written in mid 2010 and targets an old version of Rx.

With 30 overloads of `Observable.FromAsyncPattern` we will look at the general concept so that you can pick the appropriate overload for yourself. 
First if we look at the normal pattern of APM we will see that the BeginXXX method will take zero or more data arguments followed by an `AsyncCallback` and an `Object`.
The BeginXXX method will also return an `IAsyncResult` token.

	//Standard Begin signature
	IAsyncResult BeginXXX(AsyncCallback callback, Object state);
	//Standard Begin signature with data
	IAsyncResult BeginYYY(string someParam1, AsyncCallback callback, object state);

The EndXXX method will accept an `IAsyncResult` which should be the token returned from the BeginXXX method. The EndXXX can also return a value.

	//Standard EndXXX Signature
	void EndXXX(IAsyncResult asyncResult);
	//Standard EndXXX Signature with data
	int EndYYY(IAsyncResult asyncResult);

The generic arguments for the `FromAsyncPattern` method are just the BeginXXX data arguments if any, followed by the EndXXX return type if any. 
If we apply that to our `Stream.Read(byte[], int, int, AsyncResult, object)` example above we see that we have a `byte[]`, an `int` and another `int` as our data parameters for `BeginRead` method.

	//IAsyncResult BeginRead(
	//  byte[] buffer, 
	//  int offset, 
	//  int count, 
	//  AsyncCallback callback, object state) {...}
	Observable.FromAsyncPattern<byte[], int, int ...

Now we look at the EndXXX method and see it returns an `int`, which completes the generic signature of our `FromAsyncPattern` call.

	//int EndRead(
	//  IAsyncResult asyncResult) {...}
	Observable.FromAsyncPattern<byte[], int, int, int>

The result of the call to `Observable.FromAsyncPattern` does _not_ return an observable sequence. 
It returns a delegate that returns an observable sequence.
The signature for this delegate will match the generic arguments of the call to	`FromAsyncPattern`, except that the return type will be wrapped in an observable sequence.

	var fileLength = (int) stream.Length;
	//read is a Func<byte[], int, int, IObservable<int>>
	var read = Observable.FromAsyncPattern<byte[], int, int, int>(
		stream.BeginRead, 
		stream.EndRead);
	var buffer = new byte[fileLength];
	var bytesReadStream = read(buffer, 0, fileLength);
	bytesReadStream.Subscribe(
		byteCount =>
		{
			Console.WriteLine("Number of bytes read={0}, buffer should be populated with data now.",
			byteCount);
		});

Note that this implementation is just an example. 
For a very well designed implementation that is built against the latest version of Rx you should look at the Rxx project on [http://rxx.codeplex.com](http://rxx.codeplex.com).

This covers the first classification of query operators: creating observable sequences.
We have looked at the various eager and lazy ways to create a sequence. 
We have introduced the concept of corecursion and show how we can use it with the `Generate` method to unfold potentially infinite sequences. 
We can now produce timer based sequences using the various factory methods. 
We should also be familiar with ways to transition from other synchronous and asynchronous paradigms and be able to decide when it is or is not appropriate to do so. 
As a quick recap:

 * Factory Methods
   * Observable.Return
   * Observable.Empty
   * Observable.Never
   * Observable.Throw
   * Observable.Create

 * Unfold methods
   * Observable.Range
   * Observable.Interval
   * Observable.Timer
   * Observable.Generate

 * Paradigm Transition
   * Observable.Start
   * Observable.FromEventPattern
   * Task.ToObservable
   * Task&lt;T&gt;.ToObservable
   * IEnumerable&lt;T&gt;.ToObservable
   * Observable.FromAsyncPattern

Creating an observable sequence is our first step to practical application of Rx: create the sequence and then expose it for consumption. 
Now that we have a firm grasp on how to create an observable sequence, we can discover the operators that allow us to query an observable sequence.

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