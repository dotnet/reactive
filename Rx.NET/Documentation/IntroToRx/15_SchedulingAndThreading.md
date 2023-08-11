---
title: Scheduling and threading
---

#PART 4 - Concurrency				{#PART4 .SectionHeader}

Rx is primarily a system for querying _data in motion_ asynchronously. 
To effectively provide the level of asynchrony that developers require, some level of concurrency control is required. 
We need the ability to generate sequence data concurrently to the consumption of the sequence data.

In this fourth and final part of the book, we will look at the various concurrency considerations one must undertake when querying data in motion. 
We will look how to avoid concurrency when possible and use it correctly when justifiable. 
We will look at the excellent abstractions Rx provides, that enable concurrency to become declarative and also unit testable. 
In my opinion, theses two features are enough reason alone to adopt Rx into your code base. 
We will also look at the complex issue of querying concurrent sequences and analyzing data in sliding windows of time.

#Scheduling and threading			{#SchedulingAndThreading}

So far, we have managed to avoid any explicit usage of threading or concurrency.
There are some methods that we have covered that implicitly introduce some level of concurrency to perform their jobs (e.g. `Buffer`, `Delay`, `Sample` each require a separate thread/scheduler/timer to work their magic). 
Most of this however, has been kindly abstracted away from us. 
This chapter will look at the elegant beauty of the Rx API and its ability to effectively remove the need for `WaitHandle` types, and any explicit calls to `Thread`s, the `ThreadPool` or `Task`s.

##Rx is single-threaded by default	{#RxIsSingleThreadedByDefault}

To be more accurate, Rx is a free threaded model.

A popular misconception is that Rx is multithreaded by default. 
It is perhaps more an idle assumption than a strong belief, much in the same way some assume that standard .NET events are multithreaded until they challenge that notion. 
We debunk this myth and assert that events are most certainly single threaded and synchronous in the [Appendix](19_DispellingMyths.html#DispellingEventMyths).

Like events, Rx is just a way of chaining callbacks together for a given notification.
While Rx is a free-threaded model, this does not mean that subscribing or calling `OnNext` will introduce multi-threading to your sequence. 
Being free-threaded means that you are not restricted to which thread you choose to do your work. 
For example, you can choose to do your work such as invoking a subscription, observing or producing notifications, on any thread you like. 
The alternative to a free-threaded model is a _Single Threaded Apartment_ (STA) model where you must interact with the system on a given thread. 
It is common to use the STA model when working with User Interfaces and some COM interop. 
So, just as a recap: if you do not introduce any scheduling, your callbacks will be invoked on the same thread that the `OnNext`/`OnError`/`OnCompleted` methods are invoked from.

In this example, we create a subject then call `OnNext` on various threads and record the threadId in our handler.

	Console.WriteLine("Starting on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
	var subject = new Subject<object>();

	subject.Subscribe(
		o => Console.WriteLine("Received {1} on threadId:{0}", 
			Thread.CurrentThread.ManagedThreadId, 
			o));

	ParameterizedThreadStart notify = obj =>
	{
		Console.WriteLine("OnNext({1}) on threadId:{0}",
							Thread.CurrentThread.ManagedThreadId, 
							obj);
		subject.OnNext(obj);
	};

	notify(1);
	new Thread(notify).Start(2);
	new Thread(notify).Start(3);

Output:

<div class="output">
	<div class="line">Starting on threadId:9</div>
	<div class="line">OnNext(1) on threadId:9</div>
	<div class="line">Received 1 on threadId:9</div>
	<div class="line">OnNext(2) on threadId:10</div>
	<div class="line">Received 2 on threadId:10</div>
	<div class="line">OnNext(3) on threadId:11</div>
	<div class="line">Received 3 on threadId:11</div>
</div>

Note that each `OnNext` was called back on the same thread that it was notified on. 
This is not always what we are looking for. 
Rx introduces a very handy mechanism for introducing concurrency and multithreading to your code: Scheduling.

##SubscribeOn and ObserveOn			{#SubscribeOnObserveOn}

In the Rx world, there are generally two things you want to control the concurrency model for:

 * The invocation of the subscription
 * The observing of notifications


As you could probably guess, these are exposed via two extension methods to `IObservable<T>` called `SubscribeOn` and `ObserveOn`. 
Both methods have an overload that take an `IScheduler` (or `SynchronizationContext`) and return an `IObservable<T>` so you can chain methods together.

	public static class Observable 
	{
		public static IObservable<TSource> ObserveOn<TSource>(
			this IObservable<TSource> source, 
			IScheduler scheduler)
		{...}
		public static IObservable<TSource> ObserveOn<TSource>(
			this IObservable<TSource> source, 
			SynchronizationContext context)
		{...}
		public static IObservable<TSource> SubscribeOn<TSource>(
			this IObservable<TSource> source, 
			IScheduler scheduler)
		{...}
		public static IObservable<TSource> SubscribeOn<TSource>(
			this IObservable<TSource> source, 
			SynchronizationContext context)
		{...}
	}

One pitfall I want to point out here is, the first few times I used these overloads, I was confused as to what they actually do. 
You should use the `SubscribeOn` method to describe how you want any warm-up and background processing code to be scheduled. 
For example, if you were to use `SubscribeOn` with `Observable.Create`, the delegate passed to the `Create` method would be run on the specified scheduler.

In this example, we have a sequence produced by `Observable.Create` with a standard subscription.

	Console.WriteLine("Starting on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
	var source = Observable.Create<int>(
		o =>
		{
			Console.WriteLine("Invoked on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
			o.OnNext(1);
			o.OnNext(2);
			o.OnNext(3);
			o.OnCompleted();
			Console.WriteLine("Finished on threadId:{0}",
			Thread.CurrentThread.ManagedThreadId);
			return Disposable.Empty;
		});

	source
		//.SubscribeOn(Scheduler.ThreadPool)
		.Subscribe(
			o => Console.WriteLine("Received {1} on threadId:{0}",
				Thread.CurrentThread.ManagedThreadId,
				o),
			() => Console.WriteLine("OnCompleted on threadId:{0}",
				Thread.CurrentThread.ManagedThreadId));
	Console.WriteLine("Subscribed on threadId:{0}", Thread.CurrentThread.ManagedThreadId);

Output:

<div class="output">
	<div class="line">Starting on threadId:9</div>
	<div class="line">Invoked on threadId:9</div>
	<div class="line">Received 1 on threadId:9</div>
	<div class="line">Received 2 on threadId:9</div>
	<div class="line">Received 3 on threadId:9</div>
	<div class="line">OnCompleted on threadId:9</div>
	<div class="line">Finished on threadId:9</div>
	<div class="line">Subscribed on threadId:9</div>
</div>

You will notice that all actions were performed on the same thread. 
Also, note that everything is sequential. 
When the subscription is made, the `Create` delegate is called. 
When `OnNext(1)` is called, the `OnNext` handler is called, and so on. 
This all stays synchronous until the `Create` delegate is finished, and the `Subscribe` line can move on to the final line that declares we are subscribed on thread 9.

If we apply `SubscribeOn` to the chain (i.e. un-comment it), the order of execution is quite different.

<div class="output">
	<div class="line">Starting on threadId:9</div>
	<div class="line">Subscribed on threadId:9</div>
	<div class="line">Invoked on threadId:10</div>
	<div class="line">Received 1 on threadId:10</div>
	<div class="line">Received 2 on threadId:10</div>
	<div class="line">Received 3 on threadId:10</div>
	<div class="line">OnCompleted on threadId:10</div>
	<div class="line">Finished on threadId:10</div>
</div>

Observe that the subscribe call is now non-blocking. 
The `Create` delegate is executed on the thread pool and so are all our handlers.

The `ObserveOn` method is used to declare where you want your notifications to be scheduled to. 
I would suggest the `ObserveOn` method is most useful when working with STA systems, most commonly UI applications. 
When writing UI applications, the `SubscribeOn`/`ObserveOn` pair is very useful for two reasons:

 * you do not want to block the UI thread
 * but you do need to update UI objects on the UI thread.


It is critical to avoid blocking the UI thread, as doing so leads to a poor user experience. 
General guidance for Silverlight and WPF is that any work that blocks for longer than 150-250ms should not be performed on the UI thread (Dispatcher).
This is approximately the period of time over which a user can notice a lag in the UI (mouse becomes sticky, animations sluggish). 
In the upcoming Metro style apps for Windows 8, the maximum allowed blocking time is only 50ms. 
This more stringent rule is to ensure a consistent <q>fast and fluid</q> experience across applications.
With the processing power offered by current desktop processors, you can achieve a lot of processing 50ms. 
However, as processor become more varied (single/multi/many core, plus high power desktop vs. lower power ARM tablet/phones), how much you can do in 50ms fluctuates widely. 
In general terms: any I/O, computational intensive work or any processing unrelated to the UI should be marshaled off the UI thread.
The general pattern for creating responsive UI applications is:

 * respond to some sort of user action
 * do work on a background thread
 * pass the result back to the UI thread
 * update the UI

This is a great fit for Rx: responding to events, potentially composing multiple events, passing data to chained method calls. 
With the inclusion of scheduling, we even have the power to get off and back onto the UI thread for that responsive application feel that users demand.

Consider a WPF application that used Rx to populate an `ObservableCollection<T>`.
You would almost certainly want to use `SubscribeOn` to leave the `Dispatcher`, followed by `ObserveOn` to ensure you were notified back on the Dispatcher.
If you failed to use the `ObserveOn` method, then your `OnNext` handlers would be invoked on the same thread that raised the notification. 
In Silverlight/WPF, this would cause some sort of not-supported/cross-threading exception. 
In this example, we subscribe to a sequence of `Customers`. 
We perform the subscription on a new thread and ensure that as we receive `Customer` notifications, we add them to the `Customers` collection on the `Dispatcher`.

	_customerService.GetCustomers()
		.SubscribeOn(Scheduler.NewThread)
		.ObserveOn(DispatcherScheduler.Instance) 
		//or .ObserveOnDispatcher() 
		.Subscribe(Customers.Add);

##Schedulers						{#Schedulers}

The `SubscribeOn` and `ObserveOn` methods required us to pass in an `IScheduler`. 
Here we will dig a little deeper and see what schedulers are, and what implementations are available to us.

There are two main types we use when working with schedulers:

<dl>
	<dt>The `IScheduler` interface</dt>
	<dd>
		A common interface for all schedulers</dd>
	<dt>The static `Scheduler` class</dt>
	<dd>
		Exposes both implementations of `IScheduler` and helpful extension methods
		to the `IScheduler` interface</dd>
</dl>

The `IScheduler` interface is of less importance right now than the types that implement the interface. 
The key concept to understand is that an `IScheduler` in Rx is used to schedule some action to be performed, either as soon as possible or at a given point in the future. 
The implementation of the `IScheduler` defines how that action will be invoked i.e. asynchronously via a thread pool, a new thread or a message pump, or synchronously on the current thread. 
Depending on your platform (Silverlight 4, Silverlight 5, .NET 3.5, .NET 4.0), you will be exposed most of the implementations you will need via a static class `Scheduler`.

Before we look at the `IScheduler` interface in detail, let's look at the extension method we will use the most often and then introduce the common implementations.

This is the most commonly used (extension) method for `IScheduler`. 
It simply sets an action to be performed as soon as possible.

	public static IDisposable Schedule(this IScheduler scheduler, Action action)
	{...}

You could use the method like this:

	IScheduler scheduler = ...;
	scheduler.Schedule(()=>{ Console.WriteLine("Work to be scheduled"); });

These are the static properties that you can find on the `Scheduler` type.

`Scheduler.Immediate` will ensure the action is not scheduled, but rather executed immediately.

`Scheduler.CurrentThread` ensures that the actions are performed on the thread that made the original call. 
This is different from `Immediate`, as `CurrentThread` will queue the action to be performed. 
We will compare these two schedulers using a code example soon.

`Scheduler.NewThread` will schedule work to be done on a new thread.

`Scheduler.ThreadPool` will schedule all actions to take place on the Thread Pool.

`Scheduler.TaskPool` will schedule actions onto the TaskPool. 
This is not available in Silverlight 4 or .NET 3.5 builds.

If you are using WPF or Silverlight, then you will also have access to `DispatcherScheduler.Instance`.
This allows you to schedule tasks onto the `Dispatcher` with the common interface, either now or in the future. 
There is the `SubscribeOnDispatcher()` and `ObserveOnDispatcher()` extension methods to `IObservable<T>`, that also help you access the Dispatcher. 
While they appear useful, you will want to avoid these two methods for production code, and we explain why in the [Testing Rx](16_TestingRx.html) chapter.

Most of the schedulers listed above are quite self explanatory for basic usage.
We will take an in-depth look at all of the implementations of `IScheduler` later in the chapter.

##Concurrency pitfalls			{#ConcurrencyPitfalls}

Introducing concurrency to your application will increase its complexity. 
If your application is not noticeably improved by adding a layer of concurrency, then you should avoid doing so. 
Concurrent applications can exhibit maintenance problems with symptoms surfacing in the areas of debugging, testing and refactoring.

The common problem that concurrency introduces is unpredictable timing. 
Unpredictable timing can be caused by variable load on a system, as well as variations in system configurations (e.g. varying core clock speed and availability of processors). 
These can ultimately can result in race conditions. 
Symptoms of race conditions include out-of-order execution, [deadlocks](http://en.wikipedia.org/wiki/Deadlock), [livelocks](http://en.wikipedia.org/wiki/Deadlock#Livelock) and corrupted state.

In my opinion, the biggest danger when introducing concurrency haphazardly to an application, is that you can silently introduce bugs. 
These defects may slip past Development, QA and UAT and only manifest themselves in Production environments.

Rx, however, does such a good job of simplifying the concurrent processing of observable sequences that many of these concerns can be mitigated. 
You can still create problems, but if you follow the guidelines then you can feel a lot safer in the knowledge that you have heavily reduced the capacity for unwanted race conditions.

In a later chapter, [Testing Rx](16_TestingRx.html), we will look at how Rx improves your ability to test concurrent workflows.

###Lock-ups							{#LockUps}

When working on my first commercial application that used Rx, the team found out the hard way that Rx code can most certainly deadlock. 
When you consider that some calls (like `First`, `Last`, `Single` and `ForEach`) are blocking, and that we can schedule work to be done in the future, it becomes obvious that a race condition can occur. 
This example is the simplest block I could think of. 
Admittedly, it is fairly elementary but it will get the ball rolling.

	var sequence = new Subject&lt;int>();
	Console.WriteLine("Next line should lock the system.");
	var value = sequence.First();
	sequence.OnNext(1);
	Console.WriteLine("I can never execute....");

Hopefully, we won't ever write such code though, and if we did, our tests would give us quick feedback that things went wrong.
More realistically, race conditions often slip into the system at integration points.
The next example may be a little harder to detect, but is only small step away from our first, unrealistic example.
Here, we block in the constructor of a UI element which will always be created on the dispatcher. 
The blocking call is waiting for an event that can only be raised from the dispatcher, thus creating a deadlock.

	public Window1()
	{
		InitializeComponent();
		DataContext = this;
		Value = "Default value";
		//Deadlock! 
		//We need the dispatcher to continue to allow me to click the button to produce a value
		Value = _subject.First();
		//This will give same result but will not be blocking (deadlocking). 
		_subject.Take(1).Subscribe(value => Value = value);
	}
	private void MyButton_Click(object sender, RoutedEventArgs e)
	{
		_subject.OnNext("New Value");
	}
	public string Value
	{
		get { return _value; }
		set
		{
			_value = value;
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs("Value"));
		}
	}

Next, we start seeing things that can become more sinister. 
The button's click handler will try to get the first value from an observable sequence exposed via an interface.

	public partial class Window1 : INotifyPropertyChanged
	{
		//Imagine DI here.
		private readonly IMyService _service = new MyService(); 
		private int _value2;

		public Window1()
		{
			InitializeComponent();
			DataContext = this;
		}
		public int Value2
		{
			get { return _value2; }
			set
			{
				_value2 = value;
				var handler = PropertyChanged;
				if (handler != null) handler(this, new PropertyChangedEventArgs("Value2"));
			}
		}
		#region INotifyPropertyChanged Members
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
		private void MyButton2_Click(object sender, RoutedEventArgs e)
		{
			Value2 = _service.GetTemperature().First();
		}
	}

There is only one small problem here in that we block on the `Dispatcher` thread (`First` is a blocking call), however this manifests itself into a deadlock if the service code is written incorrectly.

	class MyService : IMyService
	{
		public IObservable<int> GetTemperature()
		{
			return Observable.Create<int>(
				o =>
				{
					o.OnNext(27);
					o.OnNext(26);
					o.OnNext(24);
					return () => { };
				})
			   .SubscribeOnDispatcher();
		}
	}

This odd implementation, with explicit scheduling, will cause the three `OnNext` calls to be scheduled once the `First()` call has finished; however, `that` is waiting for an `OnNext` to be called: we are deadlocked.

So far, this chapter may seem to say that concurrency is all doom and gloom by focusing on the problems you could face; this is not the intent though. 
We do not magically avoid classic concurrency problems simply by adopting Rx. 
Rx will however make it easier to get it right, provided you follow these two simple rules.

 * Only the final subscriber should be setting the scheduling
 * Avoid using blocking calls: e.g. `First`, `Last` and `Single`

The last example came unstuck with one simple problem; the service was dictating the scheduling paradigm when, really, it had no business doing so. 
Before we had a clear idea of where we should be doing the scheduling in my first Rx project, we had all sorts of layers adding 'helpful' scheduling code. 
What it ended up creating was a threading nightmare. 
When we removed all the scheduling code and then confined it it in a single layer (at least in the Silverlight client), most of our concurrency problems went away. 
I recommend you do the same. 
At least in WPF/Silverlight applications, the pattern should be simple: "Subscribe on a Background thread; Observe on the Dispatcher".

##Advanced features of schedulers	{#AdvancedFeaturesOfSchedulers}

We have only looked at the most simple usage of schedulers so far:

 * Scheduling an action to be executed as soon as possible
 * Scheduling the subscription of an observable sequence
 * Scheduling the observation of notifications coming from an observable sequence

Schedulers also provide more advanced features that can help you with various problems.

###Passing state					{#PassingState}

In the extension method to `IScheduler` we have looked at, you could only provide an `Action` to execute. 
This `Action` did not accept any parameters.
If you want to pass state to the `Action`, you could use a closure to share the data like this:

	var myName = "Lee";
	Scheduler.NewThread.Schedule(
		() => Console.WriteLine("myName = {0}", myName));

This could create a problem, as you are sharing state across two different scopes.
I could modify the variable `myName` and get unexpected results.

In this example, we use a closure as above to pass state. 
I immediately modify the closure and this creates a race condition: will my modification happen before or after the state is used by the scheduler?

	var myName = "Lee";
	scheduler.Schedule(
		() => Console.WriteLine("myName = {0}", myName));
	myName = "John";//What will get written to the console?


In my tests, "John" is generally written to the console when `scheduler` is an instance of `NewThreadScheduler`. 
If I use the `ImmediateScheduler` then "Lee" would be written. 
The problem with this is the non-deterministic nature of the code.

A preferable way to pass state is to use the `Schedule` overloads that accept state. 
This example takes advantage of this overload, giving us certainty about our state.

	var myName = "Lee";
	scheduler.Schedule(myName, 
		(_, state) =>
		{
			Console.WriteLine(state);
			return Disposable.Empty;
		});
	myName = "John";


Here, we pass `myName` as the state. 
We also pass a delegate that will take the state and return a disposable. 
The disposable is used for cancellation; we will look into that later. 
The delegate also takes an `IScheduler` parameter, which we name "_" (underscore). 
This is the convention to indicate we are ignoring the argument. 
When we pass `myName` as the state, a reference to the state is kept internally. 
So when we update the `myName` variable to "John", the reference to "Lee" is still maintained by the scheduler's internal workings.

Note that in our previous example, we modify the `myName` variable to point to a new instance of a string. 
If we were to instead have an instance that we actually modified, we could still get unpredictable behavior. 
In the next example, we now use a list for our state. 
After scheduling an action to print out the element count of the list, we modify that list.

	var list = new List<int>();
	scheduler.Schedule(list,
		(innerScheduler, state) =>
		{
			Console.WriteLine(state.Count);
			return Disposable.Empty;
		});
	list.Add(1);

Now that we are modifying shared state, we can get unpredictable results. 
In this example, we don't even know what type the scheduler is, so we cannot predict the race conditions we are creating. 
As with any concurrent software, you should avoid modifying shared state.

###Future scheduling				{#FutureScheduling}

As you would expect with a type called "IScheduler", you are able to schedule an action to be executed in the future. 
You can do so by specifying the exact point in time an action should be invoked, or you can specify the period of time to wait until the action is invoked. 
This is clearly useful for features such as buffering, timers etc.

Scheduling in the future is thus made possible by two styles of overloads, one that	takes a `TimeSpan` and one that takes a `DateTimeOffset`. 
These are the two most simple overloads that execute an action in the future.

	public static IDisposable Schedule(
		this IScheduler scheduler, 
		TimeSpan dueTime, 
		Action action)
	{...}
	public static IDisposable Schedule(
		this IScheduler scheduler, 
		DateTimeOffset dueTime, 
		Action action)
	{...}

You can use the `TimeSpan` overload like this:

	var delay = TimeSpan.FromSeconds(1);
	Console.WriteLine("Before schedule at {0:o}", DateTime.Now);
	scheduler.Schedule(delay, 
		() => Console.WriteLine("Inside schedule at {0:o}", DateTime.Now));
	Console.WriteLine("After schedule at  {0:o}", DateTime.Now);

Output:

<div class="output">
	<div class="line">Before schedule at 2012-01-01T12:00:00.000000+00:00</div>
	<div class="line">After schedule at 2012-01-01T12:00:00.058000+00:00</div>
	<div class="line">Inside schedule at 2012-01-01T12:00:01.044000+00:00</div>
</div>

We can see therefore that scheduling is non-blocking as the 'before' and 'after' calls are very close together in time. 
You can also see that approximately one second after the action was scheduled, it was invoked.

You can specify a specific point in time to schedule the task with the `DateTimeOffset` overload. 
If, for some reason, the point in time you specify is in the past, then the action is scheduled as soon as possible.

###Cancelation						{#Cancelation}

Each of the overloads to `Schedule` returns an `IDisposable`; this way, a consumer can cancel the scheduled work. 
In the previous example, we scheduled work to be invoked in one second. 
We could cancel that work by disposing of the cancellation token (i.e. the return value).

	var delay = TimeSpan.FromSeconds(1);
	Console.WriteLine("Before schedule at {0:o}", DateTime.Now);
	var token = scheduler.Schedule(delay, 
		() => Console.WriteLine("Inside schedule at {0:o}", DateTime.Now));
	Console.WriteLine("After schedule at  {0:o}", DateTime.Now);
	token.Dispose();

Output:

<div class="output">
	<div class="line">Before schedule at 2012-01-01T12:00:00.000000+00:00</div>
	<div class="line">After schedule at 2012-01-01T12:00:00.058000+00:00</div>
</div>

Note that the scheduled action never occurs, as we have cancelled it almost immediately.

When the user cancels the scheduled action method before the scheduler is able to invoke it, that action is just removed from the queue of work. 
This is what we see in example above. 
If you want to cancel scheduled work that is already running, then you can use one of the overloads to the `Schedule` method that takes a `Func<IDisposable>`. 
This gives a way for users to cancel out of a job that may already be running. 
This job could be some sort of I/O, heavy computations or perhaps usage of `Task` to perform some work.

Now this may create a problem; if you want to cancel work that has already been started, you need to dispose of an instance of `IDisposable`, but how do you return the disposable if you are still doing the work? 
You could fire up another thread so the work happens concurrently, but creating threads is something we are trying to steer away from.

In this example, we have a method that we will use as the delegate to be scheduled.
It just fakes some work by performing a spin wait and adding values to the `list` argument. 
The key here is that we allow the user to cancel with the `CancellationToken` via the disposable we return.

	public IDisposable Work(IScheduler scheduler, List<int> list)
	{
		var tokenSource = new CancellationTokenSource();
		var cancelToken = tokenSource.Token;
		var task = new Task(() =>
		{
			Console.WriteLine();
			for (int i = 0; i < 1000; i++)
			{
				var sw = new SpinWait();
				for (int j = 0; j < 3000; j++) sw.SpinOnce();
				Console.Write(".");
				list.Add(i);
				if (cancelToken.IsCancellationRequested)
				{
					Console.WriteLine("Cancelation requested");
					//cancelToken.ThrowIfCancellationRequested();
					return;
				}
			}
		}, cancelToken);
		task.Start();
		return Disposable.Create(tokenSource.Cancel);
	}

This code schedules the above code and allows the user to cancel the processing work by pressing Enter

	var list = new List<int>();
	Console.WriteLine("Enter to quit:");
	var token = scheduler.Schedule(list, Work);
	Console.ReadLine();
	Console.WriteLine("Cancelling...");
	token.Dispose();
	Console.WriteLine("Cancelled");

Output:

<div class="output">
	<div class="line">Enter to quit:</div>
	<div class="line">........</div>
	<div class="line">Cancelling...</div>
	<div class="line">Cancelled</div>
	<div class="line">Cancelation requested</div>
</div>

The problem here is that we have introduced explicit use of `Task`. 
We can avoid explicit usage of a concurrency model if we use the Rx recursive scheduler features instead.

###Recursion						{#Recursion}

The more advanced overloads of `Schedule` extension methods take some strange looking delegates as parameters. 
Take special note of the final parameter in each of these overloads of the `Schedule` extension method.

	public static IDisposable Schedule(
		this IScheduler scheduler, 
		Action<Action> action)
	{...}
	public static IDisposable Schedule<TState>(
		this IScheduler scheduler, 
		TState state, 
		Action<TState, Action<TState>> action)
	{...}
	public static IDisposable Schedule(
		this IScheduler scheduler, 
		TimeSpan dueTime, 
		Action<Action<TimeSpan>> action)
	{...}
	public static IDisposable Schedule<TState>(
		this IScheduler scheduler, 
		TState state, 
		TimeSpan dueTime, 
		Action<TState, Action<TState, TimeSpan>> action)
	{...}
	public static IDisposable Schedule(
		this IScheduler scheduler, 
		DateTimeOffset dueTime, 
		Action<Action<DateTimeOffset>> action)
	{...}
	public static IDisposable Schedule<TState>(
		this IScheduler scheduler, 
		TState state, DateTimeOffset dueTime, 
		Action<TState, Action<TState, DateTimeOffset>> action)
	{...}   


Each of these overloads take a delegate "action" that allows you to call "action" recursively. 
This may seem a very odd signature, but it makes for a great API. 
This effectively allows you to create a recursive delegate call. 
This may be best shown with an example.

This example uses the most simple recursive overload. 
We have an `Action` that can be called recursively.

	Action<Action> work = (Action self) 
		=>
		{
			Console.WriteLine("Running");
			self();
		};
	var token = s.Schedule(work);
		
	Console.ReadLine();
	Console.WriteLine("Cancelling");
	token.Dispose();
	Console.WriteLine("Cancelled");

Output:

<div class="output">
	<div class="line">Enter to quit:</div>
	<div class="line">Running</div>
	<div class="line">Running</div>
	<div class="line">Running</div>
	<div class="line">Running</div>
	<div class="line">Cancelling</div>
	<div class="line">Cancelled</div>
	<div class="line">Running</div>
</div>

Note that we didn't have to write any cancellation code in our delegate. 
Rx handled the looping and checked for cancellation on our behalf. 
Brilliant! 
Unlike simple recursive methods in C#, we are also protected from stack overflows, as Rx provides an extra level of abstraction. 
Indeed, Rx takes our recursive method and transforms it to a loop structure instead.

####Creating your own iterator		{#CreatingYourOwnIterator}

Earlier in the book, we looked at how we can use [Rx with APM](04_CreatingObservableSequences.html#FromAPM). 
In our example, we just read the entire file into memory. 
We also referenced Jeffrey van Gogh's [blog post](http://blogs.msdn.com/b/jeffva/archive/2010/07/23/rx-on-the-server-part-1-of-n-asynchronous-system-io-stream-reading.aspx), which sadly is now out of date; however, his concepts are still sound.
Instead of the Iterator method from Jeffrey's post, we can use schedulers to achieve the same result.

The goal of the following sample is to open a file and stream it in chunks. 
This enables us to work with files that are larger than the memory available to us, as we would only ever read and cache a portion of the file at a time. 
In addition to this, we can leverage the compositional nature of Rx to apply multiple transformations to the file such as encryption and compression. 
By reading chunks at a time, we are able to start the other transformations before we have finished reading the file.

First, let us refresh our memory with how to get from the `FileStream`'s APM methods into Rx.

	var source = new FileStream(@"C:\Somefile.txt", FileMode.Open, FileAccess.Read);
	var factory = Observable.FromAsyncPattern<byte[], int, int, int>(
		source.BeginRead, 
		source.EndRead);
	var buffer = new byte[source.Length];
	IObservable<int> reader = factory(buffer, 0, (int)source.Length);
	reader.Subscribe(
		bytesRead => 
			Console.WriteLine("Read {0} bytes from file into buffer", bytesRead));
			
The example above uses `FromAsyncPattern` to create a factory. 
The factory will take a byte array (`buffer`), an offset (`0`) and a length (`source.Length`); it effectively returns the count of the bytes read as a single-value sequence. 
When the sequence (`reader`) is subscribed to, `BeginRead` will read values, starting from the offset, into the buffer. 
In this case, we will read the whole file. 
Once the file has been read into the buffer, the sequence (`reader`) will push the single value (`bytesRead`) in to the sequence.

This is all fine, but if we want to read chunks of data at a time then this is not good enough. 
We need to specify the buffer size we want to use. 
Let's start with 4KB (4096 bytes).

	var bufferSize = 4096;
	var buffer = new byte[bufferSize];
	IObservable<int> reader = factory(buffer, 0, bufferSize);
	reader.Subscribe(
		bytesRead => 
			Console.WriteLine("Read {0} bytes from file", bytesRead));

This works but will only read a max of 4KB from the file. 
If the file is larger, we want to keep reading all of it. 
As the `Position` of the `FileStream` will have advanced to the point it stopped reading, we can reuse the `factory` to reload the buffer. 
Next, we want to start pushing these bytes into an observable sequence. 
Let's start by creating the signature of an extension method.

	public static IObservable<byte> ToObservable(
		this FileStream source, 
		int buffersize, 
		IScheduler scheduler)
	{...}

We can ensure that our extension method is lazily evaluated by using `Observable.Create`.
We can also ensure that the `FileStream` is closed when the consumer disposes of the subscription by taking advantage of the `Observable.Using` operator.

	public static IObservable<byte> ToObservable(
		this FileStream source, 
		int buffersize, 
		IScheduler scheduler)
	{
		var bytes = Observable.Create<byte>(o =>
		{
			...
		});

		return Observable.Using(() => source, _ => bytes);
	}

Next, we want to leverage the scheduler's recursive functionality to continuously read chunks of data while still providing the user with the ability to dispose/cancel when they choose. 
This creates a bit of a pickle; we can only pass in one state parameter but need to manage multiple moving parts (buffer, factory, filestream).
To do this, we create our own private helper class:

	private sealed class StreamReaderState
	{
		private readonly int _bufferSize;
		private readonly Func<byte[], int, int, IObservable<int>> _factory;

		public StreamReaderState(FileStream source, int bufferSize)
		{
			_bufferSize = bufferSize;
			_factory = Observable.FromAsyncPattern<byte[], int, int, int>(
				source.BeginRead, 
				source.EndRead);
			Buffer = new byte[bufferSize];
		}

		public IObservable<int> ReadNext()
		{
			return _factory(Buffer, 0, _bufferSize);
		}

		public byte[] Buffer { get; set; }
	}

This class will allow us to read data into a buffer, then read the next chunk by calling `ReadNext()`. 
In our `Observable.Create` delegate, we instantiate our helper class and use it to push the buffer into our observable sequence.

	public static IObservable<byte> ToObservable(
		this FileStream source, 
		int buffersize, 
		IScheduler scheduler)
	{
		var bytes = Observable.Create<byte>(o =>
		{
			var initialState = new StreamReaderState(source, buffersize);

			initialState
				.ReadNext()
				.Subscribe(bytesRead =>
				{
					for (int i = 0; i < bytesRead; i++)
					{
						o.OnNext(initialState.Buffer[i]);
					}
				});
			...
		});

		return Observable.Using(() => source, _ => bytes);
	}

So this gets us off the ground, but we are still do not support reading files larger than the buffer. 
Now, we need to add recursive scheduling. 
To do this, we need a delegate to fit the required signature. 
We will need one that accepts a `StreamReaderState` and can recursively call an `Action<StreamReaderState>`.

	public static IObservable<byte> ToObservable(
		this FileStream source, 
		int buffersize, 
		IScheduler scheduler)
	{
		var bytes = Observable.Create<byte>(o =>
		{
			var initialState = new StreamReaderState(source, buffersize);

			Action<StreamReaderState, Action<StreamReaderState>> iterator;
			iterator = (state, self) =>
			{
				state.ReadNext()
					 .Subscribe(bytesRead =>
							{
								for (int i = 0; i < bytesRead; i++)
								{
									o.OnNext(state.Buffer[i]);
								}
								self(state);
							});
			};
			return scheduler.Schedule(initialState, iterator);
		});

		return Observable.Using(() => source, _ => bytes);
	}
	
We now have an `iterator` action that will:

 * call `ReadNext()`
 * subscribe to the result
 * push the buffer into the observable sequence
 * and recursively call itself.

We also schedule this recursive action to be called on the provided scheduler. 
Next, we want to complete the sequence when we get to the end of the file. 
This is easy, we maintain the recursion until the `bytesRead` is 0.

	public static IObservable<byte> ToObservable(
		this FileStream source, 
		int buffersize, 
		IScheduler scheduler)
	{
		var bytes = Observable.Create<byte>(o =>
		{
			var initialState = new StreamReaderState(source, buffersize);

			Action<StreamReaderState, Action<StreamReaderState>> iterator;
			iterator = (state, self) =>
			{
				state.ReadNext()
					 .Subscribe(bytesRead =>
							{
								for (int i = 0; i < bytesRead; i++)
								{
									o.OnNext(state.Buffer[i]);
								}
								if (bytesRead > 0)
									self(state);
								else
									o.OnCompleted();
							});
			};
			return scheduler.Schedule(initialState, iterator);
		});

		return Observable.Using(() => source, _ => bytes);
	}

At this point, we have an extension method that iterates on the bytes from a file stream. 
Finally, let us apply some clean up so that we correctly manage our resources and exceptions, and the finished method looks something like this:

	public static IObservable<byte> ToObservable(
		this FileStream source, 
		int buffersize, 
		IScheduler scheduler)
	{
		var bytes = Observable.Create<byte>(o =>
		{
			var initialState = new StreamReaderState(source, buffersize);
			var currentStateSubscription = new SerialDisposable();
			Action<StreamReaderState, Action<StreamReaderState>> iterator =
			(state, self) =>
				currentStateSubscription.Disposable = state.ReadNext()
					 .Subscribe(
						bytesRead =>
						{
							for (int i = 0; i < bytesRead; i++)
							{
								o.OnNext(state.Buffer[i]);
							}

							if (bytesRead > 0)
								self(state);
							else
								o.OnCompleted();
						},
						o.OnError);

			var scheduledWork = scheduler.Schedule(initialState, iterator);
			return new CompositeDisposable(currentStateSubscription, scheduledWork);
		});

		return Observable.Using(() => source, _ => bytes);
	}

This is example code and your mileage may vary. 
I find that increasing the buffer size and returning `IObservable<IList<byte>>` suits me better, but the example above works fine too. 
The goal here was to provide an example of an iterator that provides concurrent I/O access with cancellation and resource-efficient buffering.

<!--<a name="ScheduledExceptions"></a>
<h4>Exceptions from scheduled code</h4>
<p>
	TODO:
</p>-->

####Combinations of scheduler features		{#CombinationsOfSchedulerFeatures}

We have discussed many features that you can use with the `IScheduler` interface.
Most of these examples, however, are actually using extension methods to invoke the functionality that we are looking for. 
The interface itself exposes the richest overloads. 
The extension methods are effectively just making a trade-off; improving usability/discoverability by reducing the richness of the overload. 
If you want access to passing state, cancellation, future scheduling and recursion, it is all available directly from the interface methods.

	namespace System.Reactive.Concurrency
	{
	  public interface IScheduler
	  {
		//Gets the scheduler's notion of current time.
		DateTimeOffset Now { get; }

		// Schedules an action to be executed with given state. 
		//  Returns a disposable object used to cancel the scheduled action (best effort).
		IDisposable Schedule<TState>(
			TState state, 
			Func<IScheduler, TState, IDisposable> action);

		// Schedules an action to be executed after dueTime with given state. 
		//  Returns a disposable object used to cancel the scheduled action (best effort).
		IDisposable Schedule<TState>(
			TState state, 
			TimeSpan dueTime, 
			Func<IScheduler, TState, IDisposable> action);

		//Schedules an action to be executed at dueTime with given state. 
		//  Returns a disposable object used to cancel the scheduled action (best effort).
		IDisposable Schedule<TState>(
			TState state, 
			DateTimeOffset dueTime, 
			Func<IScheduler, TState, IDisposable> action);
	  }
	}
	
##Schedulers in-depth				{#SchedulersIndepth}

We have largely been concerned with the abstract concept of a scheduler and the `IScheduler` interface. 
This abstraction allows low-level plumbing to remain agnostic towards the implementation of the concurrency model. 
As in the file reader example above, there was no need for the code to know which implementation of `IScheduler` was passed, as this is a concern of the consuming code.

Now we take an in-depth look at each implementation of `IScheduler`, consider the benefits and tradeoffs they each make, and when each is appropriate to use.

###ImmediateScheduler				{#ImmediateScheduler}

The `ImmediateScheduler` is exposed via the `Scheduler.Immediate` static property. 
This is the most simple of schedulers as it does not actually schedule anything. 
If you call `Schedule(Action)` then it will just invoke the action.
If you schedule the action to be invoked in the future, the `ImmediateScheduler` will invoke a `Thread.Sleep` for the given period of time and then execute the action. 
In summary, the `ImmediateScheduler` is synchronous.

###CurrentThreadScheduler			{#Current}

Like the `ImmediateScheduler`, the `CurrentThreadScheduler` is single-threaded.
It is exposed via the `Scheduler.Current` static property. 
The key difference is that the `CurrentThreadScheduler` acts like a message queue or a _Trampoline_.
If you schedule an action that itself schedules an action, the `CurrentThreadScheduler` will queue the inner action to be performed later; in contrast, the `ImmediateScheduler` would start working on the inner action straight away. 
This is probably best explained with an example.

In this example, we analyze how `ImmediateScheduler` and `CurrentThreadScheduler` perform nested scheduling differently.

	private static void ScheduleTasks(IScheduler scheduler)
	{
		Action leafAction = () => Console.WriteLine("----leafAction.");
		Action innerAction = () =>
		{
			Console.WriteLine("--innerAction start.");
			scheduler.Schedule(leafAction);
			Console.WriteLine("--innerAction end.");
		};
		Action outerAction = () =>
		{
			Console.WriteLine("outer start.");
			scheduler.Schedule(innerAction);
			Console.WriteLine("outer end.");
		};
		scheduler.Schedule(outerAction);
	}
	public void CurrentThreadExample()
	{
		ScheduleTasks(Scheduler.CurrentThread);
		/*Output: 
		outer start. 
		outer end. 
		--innerAction start. 
		--innerAction end. 
		----leafAction. 
		*/ 
	}
	public void ImmediateExample()
	{
		ScheduleTasks(Scheduler.Immediate);
		/*Output: 
		outer start. 
		--innerAction start. 
		----leafAction. 
		--innerAction end. 
		outer end. 
		*/ 
	}

Note how the `ImmediateScheduler` does not really "schedule" anything at all, all work is performed immediately (synchronously). 
As soon as `Schedule` is called with a delegate, that delegate is invoked. 
The `CurrentThreadScheduler`, however, invokes the first delegate, and, when nested delegates are scheduled, queues them to be invoked later. 
Once the initial delegate is complete, the queue is checked for any remaining delegates (i.e. nested calls to `Schedule`) and they are invoked. 
The difference here is quite important as you can potentially get out-of-order execution, unexpected blocking, or even deadlocks by using the wrong one.

###DispatcherScheduler				{#Dispatcher}

The `DispatcherScheduler` is found in `System.Reactive.Window.Threading.dll` (for WPF, Silverlight 4 and Silverlight 5). 
When actions are scheduled using the `DispatcherScheduler`, they are effectively marshaled to the `Dispatcher`'s `BeginInvoke` method. 
This will add the action to the end of the dispatcher's _Normal_ priority queue of work. 
This provides similar queuing semantics to the `CurrentThreadScheduler` for nested calls to `Schedule`.

When an action is scheduled for future work, then a `DispatcherTimer` is created with a matching interval. 
The callback for the timer's tick will stop the timer and re-schedule the work onto the `DispatcherScheduler`. 
If the `DispatcherScheduler` determines that the `dueTime` is actually not in the future then no timer is created, and the action will just be scheduled normally.


I would like to highlight a hazard of using the `DispatcherScheduler`. 
You can construct your own instance of a `DispatcherScheduler` by passing in a reference to a `Dispatcher`. 
The alternative way is to use the static property `DispatcherScheduler.Instance`. 
This can introduce hard to understand problems if it is not used properly. 
The static property does not return a reference to a static field, but creates a new instance each time, with the static property `Dispatcher.CurrentDispatcher` as the constructor argument. 
If you access `Dispatcher.CurrentDispatcher` from a thread that is not the UI thread, it will thus give you a new instance of a `Dispatcher`, but it will not be the instance you were hoping for.

For example, imagine that we have a WPF application with an `Observable.Create` method. 
In the delegate that we pass to `Observable.Create`, we want to schedule the notifications on the dispatcher. 
We think this is a good idea because any consumers of the sequence would get the notifications on the dispatcher for free.

	var fileLines = Observable.Create<string>(
		o =>
		{
			var dScheduler = DispatcherScheduler.Instance;
			var lines = File.ReadAllLines(filePath);
			foreach (var line in lines)
			{
				var localLine = line;
				dScheduler.Schedule(
					() => o.OnNext(localLine));
			}
			return Disposable.Empty;
		});

This code may intuitively seem correct, but actually takes away power from consumers of the sequence. 
When we subscribe to the sequence, we decide that reading a file on the UI thread is a bad idea. 
So we add in a `SubscribeOn(Scheduler.NewThread)` to the chain as below:

	fileLines
		.SubscribeOn(Scheduler.ThreadPool)
		.Subscribe(line => Lines.Add(line));

This causes the create delegate to be executed on a new thread. 
The delegate will read the file then get an instance of a `DispatcherScheduler`. 
The `DispatcherScheduler` tries to get the `Dispatcher` for the current thread, but we are no longer on the UI thread, so there isn't one. 
As such, it creates a new dispatcher that is used for the `DispatcherScheduler` instance. 
We schedule some work (the notifications), but, as the underlying `Dispatcher` has not been run, nothing happens; we do not even get an exception. 
I have seen this on a commercial project and it left quite a few people scratching their heads.

This takes us to one of our guidelines regarding scheduling: 
<q>the use of `SubscribeOn` and `ObserveOn` should only be invoked by the final subscriber</q>. 
If you introduce scheduling in your own extension methods or service methods, you should allow the consumer to specify their own scheduler. 
We will see more reasons for this guidance in the next chapter.

###EventLoopScheduler				{#EventLoopScheduler}

The `EventLoopScheduler` allows you to designate a specific thread to a scheduler.
Like the `CurrentThreadScheduler` that acts like a trampoline for nested scheduled actions, the `EventLoopScheduler` provides the same trampoline mechanism. 
The difference is that you provide an `EventLoopScheduler` with the thread you want it to use for scheduling instead, of just picking up the current thread.

The `EventLoopScheduler` can be created with an empty constructor, or you can pass it a thread factory delegate.

	// Creates an object that schedules units of work on a designated thread.
	public EventLoopScheduler()
	{...}

	// Creates an object that schedules units of work on a designated thread created by the 
	//  provided factory function.
	public EventLoopScheduler(Func&lt;ThreadStart, Thread> threadFactory)
	{...}

The overload that allows you to pass a factory enables you to customize the thread before it is assigned to the `EventLoopScheduler`. 
For example, you can set the thread name, priority, culture and most importantly whether the thread is a background thread or not. 
Remember that if you do not set the thread's property `IsBackground` to false, then your application will not terminate until it the thread is terminated. 
The `EventLoopScheduler` implements `IDisposable`, and calling Dispose will allow the thread to terminate. 
As with any implementation of `IDisposable`, it is appropriate that you explicitly manage the lifetime of the resources you create.

This can work nicely with the `Observable.Using` method, if you are so inclined.
This allows you to bind the lifetime of your `EventLoopScheduler` to that of an observable sequence - for example, this `GetPrices` method that takes an `IScheduler` for an argument and returns an observable sequence.

	private IObservable&lt;Price> GetPrices(IScheduler scheduler)
	{...}

Here we bind the lifetime of the `EventLoopScheduler` to that of the result from the `GetPrices` method.

	Observable.Using(()=>new EventLoopScheduler(), els=> GetPrices(els))
			.Subscribe(...)

###New Thread						{#NewThread}

If you do not wish to manage the resources of a thread or an `EventLoopScheduler`, then you can use `NewThreadScheduler`. 
You can create your own instance of `NewThreadScheduler` or get access to the static instance via the property `Scheduler.NewThread`. 
Like `EventLoopScheduler`, you can use the parameterless constructor or provide your own thread factory function. 
If you do provide your own factory, be careful to set the `IsBackground` property appropriately.

When you call `Schedule` on the `NewThreadScheduler`, you are actually creating an `EventLoopScheduler` under the covers. 
This way, any nested scheduling will happen on the same thread. 
Subsequent (non-nested) calls to `Schedule` will create a new `EventLoopScheduler` and call the thread factory function for a new thread too.

In this example we run a piece of code reminiscent of our comparison between `Immediate` and `Current` schedulers. 
The difference here, however, is that we track the `ThreadId` that the action is performed on. 
We use the `Schedule` overload that allows us to pass the Scheduler instance into our nested delegates.
This allows us to correctly nest calls.

	private static IDisposable OuterAction(IScheduler scheduler, string state)
	{
		Console.WriteLine("{0} start. ThreadId:{1}", 
			state, 
			Thread.CurrentThread.ManagedThreadId);
		scheduler.Schedule(state + ".inner", InnerAction);
		Console.WriteLine("{0} end. ThreadId:{1}", 
			state, 
			Thread.CurrentThread.ManagedThreadId);
		return Disposable.Empty;
	}
	private static IDisposable InnerAction(IScheduler scheduler, string state)
	{
		Console.WriteLine("{0} start. ThreadId:{1}", 
			state, 
			Thread.CurrentThread.ManagedThreadId);
		scheduler.Schedule(state + ".Leaf", LeafAction);
		Console.WriteLine("{0} end. ThreadId:{1}", 
			state, 
			Thread.CurrentThread.ManagedThreadId);
		return Disposable.Empty;
	}
	private static IDisposable LeafAction(IScheduler scheduler, string state)
	{
		Console.WriteLine("{0}. ThreadId:{1}", 
			state, 
			Thread.CurrentThread.ManagedThreadId);
		return Disposable.Empty;
	}

When executed with the `NewThreadScheduler` like this:

	Console.WriteLine("Starting on thread :{0}", 
		Thread.CurrentThread.ManagedThreadId);
	Scheduler.NewThread.Schedule("A", OuterAction);

Output:

<div class="output">
	<div class="line">Starting on thread :9</div>
	<div class="line">A start. ThreadId:10</div>
	<div class="line">A end. ThreadId:10</div>
	<div class="line">A.inner start . ThreadId:10</div>
	<div class="line">A.inner end. ThreadId:10</div>
	<div class="line">A.inner.Leaf. ThreadId:10</div>
</div>

As you can see, the results are very similar to the `CurrentThreadScheduler`, except that the trampoline happens on a separate thread. 
This is in fact exactly the output we would get if we used an `EventLoopScheduler`. 
The differences between usages of the `EventLoopScheduler` and the `NewThreadScheduler`	start to appear when we introduce a second (non-nested) scheduled task.

	Console.WriteLine("Starting on thread :{0}", 
		Thread.CurrentThread.ManagedThreadId);
	Scheduler.NewThread.Schedule("A", OuterAction);
	Scheduler.NewThread.Schedule("B", OuterAction);

Output:

<div class="output">
	<div class="line">Starting on thread :9</div>
	<div class="line">A start. ThreadId:10</div>
	<div class="line">A end. ThreadId:10</div>
	<div class="line">A.inner start . ThreadId:10</div>
	<div class="line">A.inner end. ThreadId:10</div>
	<div class="line">A.inner.Leaf. ThreadId:10</div>
	<div class="line">B start. ThreadId:11</div>
	<div class="line">B end. ThreadId:11</div>
	<div class="line">B.inner start . ThreadId:11</div>
	<div class="line">B.inner end. ThreadId:11</div>
	<div class="line">B.inner.Leaf. ThreadId:11</div>
</div>

Note that there are now three threads at play here. 
Thread 9 is the thread we started on and threads 10 and 11 are performing the work for our two calls to Schedule.

###Thread Pool						{#ThreadPool}

The `ThreadPoolScheduler` will simply just tunnel requests to the `ThreadPool`.
For requests that are scheduled as soon as possible, the action is just sent to `ThreadPool.QueueUserWorkItem`. 
For requests that are scheduled in the future, a `System.Threading.Timer` is used.

As all actions are sent to the `ThreadPool`, actions can potentially run out of order. 
Unlike the previous schedulers we have looked at, nested calls are not guaranteed to be processed serially. 
We can see this by running the same test as above but with the `ThreadPoolScheduler`.

	Console.WriteLine("Starting on thread :{0}", 
		Thread.CurrentThread.ManagedThreadId);
	Scheduler.ThreadPool.Schedule("A", OuterAction);
	Scheduler.ThreadPool.Schedule("B", OuterAction);

The output

<div class="output">
	<div class="line">Starting on thread :9</div>
	<div class="line">A start. ThreadId:10</div>
	<div class="line">A end. ThreadId:10</div>
	<div class="line">A.inner start . ThreadId:10</div>
	<div class="line">A.inner end. ThreadId:10</div>
	<div class="line">A.inner.Leaf. ThreadId:10</div>
	<div class="line">B start. ThreadId:11</div>
	<div class="line">B end. ThreadId:11</div>
	<div class="line">B.inner start . ThreadId:10</div>
	<div class="line">B.inner end. ThreadId:10</div>
	<div class="line">B.inner.Leaf. ThreadId:11</div>
</div>

Note, that as per the `NewThreadScheduler` test, we initially start on one thread but all the scheduling happens on two other threads. 
The difference is that we can see that part of the second run "B" runs on thread 11 while another part of it runs on 10.

###TaskPool							{#TaskPool}

The `TaskPoolScheduler` is very similar to the `ThreadPoolScheduler` and, when available (depending on your target framework), you should favor it over	the later. 
Like the `ThreadPoolScheduler`, nested scheduled actions are not guaranteed to be run on the same thread. 
Running the same test with the `TaskPoolScheduler` shows us similar results.

	Console.WriteLine("Starting on thread :{0}", 
		Thread.CurrentThread.ManagedThreadId);
	Scheduler.TaskPool.Schedule("A", OuterAction);
	Scheduler.TaskPool.Schedule("B", OuterAction);

Output:

<div class="output">
	<div class="line">Starting on thread :9</div>
	<div class="line">A start. ThreadId:10</div>
	<div class="line">A end. ThreadId:10</div>
	<div class="line">B start. ThreadId:11</div>
	<div class="line">B end. ThreadId:11</div>
	<div class="line">A.inner start . ThreadId:10</div>
	<div class="line">A.inner end. ThreadId:10</div>
	<div class="line">A.inner.Leaf. ThreadId:10</div>
	<div class="line">B.inner start . ThreadId:11</div>
	<div class="line">B.inner end. ThreadId:11</div>
	<div class="line">B.inner.Leaf. ThreadId:10</div>
</div>

###TestScheduler					{#TestScheduler}

It is worth noting that there is also a `TestScheduler` accompanied by its base classes `VirtualTimeScheduler` and `VirtualTimeSchedulerBase`.
The latter two are not really in the scope of an introduction to Rx, but the former is. 
We will cover all things testing including the `TestScheduler` in the next chapter, [Testing Rx](16_TestingRx.html).

##Selecting an appropriate scheduler	{#SelectingAScheduler}

With all of these options to choose from, it can be hard to know which scheduler to use and when. 
Here is a simple check list to help you in this daunting task:

###UI Applications					{#UIApplications}

 * The final subscriber is normally the presentation layer and should control the scheduling.
 * Observe on the `DispatcherScheduler` to allow updating of ViewModels
 * Subscribe on a background thread to prevent the UI from becoming unresponsive
	* If the subscription will not block for more than 50ms then
		* Use the `TaskPoolScheduler` if available, or
		* Use the `ThreadPoolScheduler`
	* If any part of the subscription could block for longer than 50ms, then you should	use the `NewThreadScheduler`. 

###Service layer					{#ServiceLayer}

 * If your service is reading data from a queue of some sort, consider using a dedicated `EventLoopScheduler`. 
This way, you can preserve order of events
 * If processing an item is expensive (>50ms or requires I/O), then consider using a `NewThreadScheduler`
 * If you just need the scheduler for a timer, e.g. for `Observable.Interval` or `Observable.Timer`, then favor the `TaskPool`. 
Use the `ThreadPool` if the `TaskPool` is not available for your platform.


<p class="comment">
	The `ThreadPool` (and the `TaskPool` by proxy) have a time delay before	they will increase the number of threads that they use. 
	This delay is 500ms. 
	Let	us consider a PC with two cores that we will schedule four actions onto. 
	By default,	the thread pool size will be the number of cores (2). 
	If each action takes 1000ms, then two actions will be sitting in the queue for 500ms before the thread pool size is increased. 
	Instead of running all four actions in parallel, which would take one second in total, the work is not completed for 1.5 seconds as two of the actions sat in the queue for 500ms. 
	For this reason, you should only schedule work that	is very fast to execute (guideline 50ms) onto the ThreadPool or TaskPool. 
	Conversely,	creating a new thread is not free, but with the power of processors today the creation of a thread for work over 50ms is a small cost.
</p>

Concurrency is hard. 
We can choose to make our life easier by taking advantage of Rx and its scheduling features. 
We can improve it even further by only using Rx where appropriate.
While Rx has concurrency features, these should not be mistaken for a concurrency framework. 
Rx is designed for querying data, and as discussed in [the first chapter](01_WhyRx.html#Could), parallel computations or composition of asynchronous methods is more appropriate for other frameworks.

Rx solves the issues for concurrently generating and consuming data via the `ObserveOn`/`SubscribeOn` methods. 
By using these appropriately, we can simplify our code base, increase responsiveness and reduce the surface area of our concurrency concerns. 
Schedulers provide a rich platform for processing work concurrently without the need to be exposed directly to threading primitives. 
They also help with common troublesome areas of concurrency such as cancellation, passing state and recursion. 
By reducing the concurrency surface area, Rx provides a (relatively) simple yet powerful set of concurrency features paving the way to the [pit of success](http://blogs.msdn.com/b/brada/archive/2003/10/02/50420.aspx).

---

<div class="webonly">
	<h1 class="ignoreToc">Additional recommended reading</h1>
	<div align="center">
		<!--Concurrent Programming on Windows: Architecture, Principles, and Patterns (Kindle) Amazon.co.uk-->
		<div style="display:inline-block; vertical-align: top; margin: 10px; width: 140px; font-size: 11px; text-align: center">
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=B0015DYKI4&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
					style="width:120px;height:240px;" 
					scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>
		</div>

		<div style="display:inline-block; vertical-align: top;  margin: 10px; width: 140px; font-size: 11px; text-align: center">
			<!--C# in a nutshell Amazon.co.uk-->
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=B008E6I1K8&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
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
			<!--Parallel Programming with Microsoft® .NET: Design Patterns for Decomposition and Coordination on Multicore Architectures (Patterns & Practices) (Kindle) Amazon.co.uk-->
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=B0043EWUG6&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
					style="width:120px;height:240px;" 
					scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>
		</div>
	</div></div>