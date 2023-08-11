---
title : Hot and Cold observables
---

<!--TODO: Observable.Synchronize (vNext? Say it is not intro material?)-->
<!--TODO: Observable.Defer - Make cold (vNext? Say it is not intro material?)-->

#Hot and Cold observables			{#HotAndCold}

In this chapter, we will look at how to describe and handle two styles of observable sequences:

 1. Sequences that are passive and start producing notifications on request (when subscribed to), and
 2. Sequences that are active and produce notifications regardless of subscriptions.

In this sense, passive sequences are `Cold` and active are described as being `Hot`. 
You can draw some similarities between implementations of the `IObservable<T>` interface and implementations of the `IEnumerable<T>`interface with regards to hot and cold. 
With `IEnumerable<T>`, you could have an on-demand collection via the yield return syntax, or you could have an eagerly-evaluated collection by returning a populated `List<T>`.
We can compare the two styles by attempting to read just the first value from a sequence. 
We can do this with a method like this:

	public void ReadFirstValue(IEnumerable<int> list)
	{
		foreach (var i in list)
		{
			Console.WriteLine("Read out first value of {0}", i);
			break;
		}
	}

As an alternative to the `break` statement, we could apply a `Take(1)` to the `list`. 
If we then apply this to an eagerly-evaluated sequence, such as a list, we see the entire list is first constructed, and then returned.

	public static void Main()
	{
		ReadFirstValue(EagerEvaluation());
	}
	public IEnumerable<int> EagerEvaluation()
	{
		var result = new List<int>();
		Console.WriteLine("About to return 1");
		result.Add(1);
		//code below is executed but not used.
		Console.WriteLine("About to return 2");
		result.Add(2);
		return result;
	}

Output:

<div class="output">
	<div class="line">About to return 1</div>
	<div class="line">About to return 2</div>
	<div class="line">Read out first value of 1</div>
</div>

We now apply the same code to a lazily-evaluated sequence.

	public IEnumerable<int> LazyEvaluation()
	{
		Console.WriteLine("About to return 1");
		yield return 1;
		//Execution stops here in this example
		Console.WriteLine("About to return 2");
		yield return 2;
	}

Output:

<div class="output">
	<div class="line">About to return 1</div>
	<div class="line">Read out first value of 1</div>
</div>

The lazily-evaluated sequence did not have to yield any more values than required.
Lazy evaluation is good for on-demand queries whereas eager evaluation is good for sharing sequences so as to avoid re-evaluating multiple times. 
Implementations of `IObservable<T>` can exhibit similar variations in style.

Examples of hot observables that could publish regardless of whether there are any subscribers would be:

 * mouse movements 
 * timer events 
 * broadcasts like ESB channels or UDP network packets. 
 * price ticks from a trading exchange 

ome examples of cold observables would be:

 * asynchronous request (e.g. when using `Observable.FromAsyncPattern`)
 * whenever `Observable.Create` is used
 * subscriptions to queues 
 * on-demand sequences

##Cold observables					{#ColdObservables}

In this example, we fetch a list of products from a database. 
In our implementation, we choose to return an `IObservable<string>` and, as we get the results, we publish them until we have the full list, then complete the sequence.

	private const string connectionString = @"Data Source=.\SQLSERVER;"+
		@"Initial Catalog=AdventureWorksLT2008;Integrated Security=SSPI;"
	private static IObservable<string> GetProducts()
	{
		return Observable.Create<string>(
		o =>
		{
			using(var conn = new SqlConnection(connectionString))
			using (var cmd = new SqlCommand("Select Name FROM SalesLT.ProductModel", conn))
			{
				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				while (reader.Read())
				{
					o.OnNext(reader.GetString(0));
				}
				o.OnCompleted();
				return Disposable.Create(()=>Console.WriteLine("--Disposed--"));
			}
		});
	}

This code is just like many existing data access layers that return an `IEnumerable<T>`, however it would be much easier with Rx to access this in an asynchronous manner (using [SubscribeOn and ObserveOn](15_SchedulingAndThreading.html#SubscribeOnObserveOn)). 
This example of a data access layer is lazily evaluated and provides no caching. 
Each time the method is used, we reconnect to the database.
This is typical of cold observables; calling the method does nothing. 
Subscribing to the returned `IObservable<T>` will however invoke the create delegate which connects to the database.

Here we have a consumer of our above code, but it explicitly only wants up to three values (the full set has 128 values). 
This code illustrates that the `Take(3)` expression will restrict what the consumer receives but `GetProducts()` method will still publish _all_ of the values.

	public void ColdSample()
	{
		var products = GetProducts().Take(3);
		products.Subscribe(Console.WriteLine);
		Console.ReadLine();
	}

The `GetProducts()` code above is a pretty naive example, as it lacks the ability to cancel at any time. 
This means all values are read even though only three were requested.
In the later chapter on [scheduling](15_SchedulingAndThreading.html), we cover examples on how to provide cancellation correctly.

##Hot observables					{#HotObservables}

In our example above, the database was not accessed until the consumer of the `GetProducts()` method subscribed to the return value. 
Subsequent or even parallel calls to `GetProducts()` would return independent observable sequences and would each make their own independent calls to the database. 
By contrast, a hot observable is an observable sequence that is producing notifications even if there are no subscribers. 
The classic cases of hot observables are UI Events and Subjects. 
For example, if the mouse moves then the `MouseMove` event will be raised. 
If there are no event handlers registered for the event, then nothing happens. 
If, on the other hand, we create a `Subject<int>`, we can inject values into it using `OnNext`, regardless of whether there are observers subscribed to the subject or not.

Some observable sequences can appear to be hot when they are in fact cold. 
A couple of examples that surprise many is `Observable.Interval` and `Observable.Timer` (though it should not come as a shock to attentive readers of the [Creating observable sequences](04_CreatingObservableSequences.html#Unfold) chapter). 
In the example below, we subscribe twice to the same instance, created via the `Interval` factory method. 
The delay between the two subscriptions should demonstrate that while they are subscribed to the same observable instance, the values each subscription receives are independent, i.e. `Interval` is cold.

	public void SimpleColdSample()
	{
		var period = TimeSpan.FromSeconds(1);
		var observable = Observable.Interval(period);
		observable.Subscribe(i => Console.WriteLine("first subscription : {0}", i));
		Thread.Sleep(period);
		observable.Subscribe(i => Console.WriteLine("second subscription : {0}", i));
		Console.ReadKey();
		/* Output: 
		first subscription : 0 
		first subscription : 1 
		second subscription : 0 
		first subscription : 2 
		second subscription : 1 
		first subscription : 3 
		second subscription : 2 
		*/ 
	}

##Publish and Connect				{#PublishAndConnect}

If we want to be able to share the actual data values and not just the observable instance, we can use the `Publish()` extension method. 
This will return an `IConnectableObservable<T>`, which extends `IObservable<T>` by adding a single `Connect()` method. 
By using the `Publish()` then `Connect()` method, we can get this sharing functionality.

	var period = TimeSpan.FromSeconds(1);
	var observable = Observable.Interval(period).Publish();
	observable.Connect();
	observable.Subscribe(i => Console.WriteLine("first subscription : {0}", i));
	Thread.Sleep(period);
	observable.Subscribe(i => Console.WriteLine("second subscription : {0}", i));

Output:

<div class="output">
	<div class="line">first subscription : 0 </div>
	<div class="line">first subscription : 1 </div>
	<div class="line">second subscription : 1 </div>
	<div class="line">first subscription : 2 </div>
	<div class="line">second subscription : 2 </div>
</div>

In the example above, the `observable` variable is an `IConnectableObservable<T>`, and by calling `Connect()` it will subscribe to the underlying (the `Observable.Interval`).
In this case, we are quick enough to subscribe before the first item is published, but only on the first subscription. 
The second subscription subscribes late and misses the first publication. 
We could move the invocation of the `Connect()` method until after all subscriptions have been made. 
That way, even with the call to `Thread.Sleep` we will not really subscribe to the underlying until after both subscriptions are made. 
This would be done as follows:

	var period = TimeSpan.FromSeconds(1);
	var observable = Observable.Interval(period).Publish();
	observable.Subscribe(i => Console.WriteLine("first subscription : {0}", i));
	Thread.Sleep(period);
	observable.Subscribe(i => Console.WriteLine("second subscription : {0}", i));
	observable.Connect();

<div class="output">
	<div class="line">first subscription : 0 </div>
	<div class="line">second subscription : 0 </div>
	<div class="line">first subscription : 1 </div>
	<div class="line">second subscription : 1 </div>
	<div class="line">first subscription : 2 </div>
	<div class="line">second subscription : 2 </div>
</div>

As you can imagine, this is quite useful whenever an application needs to share sequences of data. 
In a financial trading application, if you wanted to consume a price stream for a certain asset in more than one place, you would want to try to reuse a single, common stream and avoid making another subscription to the server providing that data. 
In a social media application, many widgets may need to be notified whenever someone connects. 
`Publish` and `Connect` are perfect solutions for this.

###Disposal of connections and subscriptions	{#Disposal}

A point of interest is how disposal is performed. 
Indeed, we have not covered yet the fact that `Connect` returns an `IDisposable`. 
By disposing of the 'connection', you can turn the sequence on and off (`Connect()` to toggle it on, disposing toggles it off). 
In this example, we see that the sequence can be connected and disconnected multiple times.

	var period = TimeSpan.FromSeconds(1);
	var observable = Observable.Interval(period).Publish();
	observable.Subscribe(i => Console.WriteLine("subscription : {0}", i));
	var exit = false;
	while (!exit)
	{
		Console.WriteLine("Press enter to connect, esc to exit.");
		var key = Console.ReadKey(true);
		if(key.Key== ConsoleKey.Enter)
		{
			var connection = observable.Connect(); //--Connects here--
			Console.WriteLine("Press any key to dispose of connection.");
			Console.ReadKey();
			connection.Dispose(); //--Disconnects here--
		}
		if(key.Key==ConsoleKey.Escape)
		{
			exit = true;
		}
	}

Output:

<div class="output">
	<div class="line">Press enter to connect, esc to exit. </div>
	<div class="line">Press any key to dispose of connection. </div>
	<div class="line">subscription : 0 </div>
	<div class="line">subscription : 1 </div>
	<div class="line">subscription : 2 </div>
	<div class="line">Press enter to connect, esc to exit. </div>
	<div class="line">Press any key to dispose of connection. </div>
	<div class="line">subscription : 0 </div>
	<div class="line">subscription : 1 </div>
	<div class="line">subscription : 2 </div>
	<div class="line">Press enter to connect, esc to exit. </div>
</div>

Let us finally consider automatic disposal of a connection. 
We want a single sequence to be shared between subscriptions, as per the price stream example mentioned above.
We also want to only have the sequence running hot if there are any subscribers.
It seems therefore, not only obvious that there should be a mechanism for automatically connecting (once a subscription has been made), but also a mechanism for disconnecting (once there are no more subscriptions) from a sequence. 
First let us look at what happens to a sequence when we connect with no subscribers, and then later unsubscribe:

	var period = TimeSpan.FromSeconds(1);
	var observable = Observable.Interval(period)
		.Do(l => Console.WriteLine("Publishing {0}", l)) //Side effect to show it is running
		.Publish();
	observable.Connect();
	Console.WriteLine("Press any key to subscribe");
	Console.ReadKey();
	var subscription = observable.Subscribe(i => Console.WriteLine("subscription : {0}", i));
	Console.WriteLine("Press any key to unsubscribe.");
	Console.ReadKey();
	subscription.Dispose();
	Console.WriteLine("Press any key to exit.");
	Console.ReadKey();

Output:

<div class="output">
	<div class="line">Press any key to subscribe </div>
	<div class="line">Publishing 0 </div>
	<div class="line">Publishing 1 </div>
	<div class="line">Press any key to unsubscribe. </div>
	<div class="line">Publishing 2 </div>
	<div class="line">subscription : 2 </div>
	<div class="line">Publishing 3 </div>
	<div class="line">subscription : 3 </div>
	<div class="line">Press any key to exit. </div>
	<div class="line">Publishing 4 </div>
	<div class="line">Publishing 5 </div>
</div>

A few things to note here:

 1. I use the `Do` extension method to create side effects on the sequence (i.e. write to the console). 
This allows us to see when the sequence is actually connected.
 2. We connect first and then subscribe, which means that we can publish without any live subscriptions i.e. make the sequence hot. 
 3. We dispose of our subscription but do not dispose of the connection, which means the sequence will still be running. 

###RefCount					{#RefCount}

Let us modify that last example by replacing uses of `Connnect()` by the extension method `RefCount`. 
This will "magically" implement our requirements for automatic disposal and lazy connection. 
`RefCount` will take an `IConnectableObservable<T>` and turn it back into an `IObservable<T>` while automatically implementing the "connect" and "disconnect" behavior we are looking for.

	var period = TimeSpan.FromSeconds(1);
	var observable = Observable.Interval(period)
		.Do(l => Console.WriteLine("Publishing {0}", l)) //side effect to show it is running
		.Publish()
		.RefCount();
	//observable.Connect(); Use RefCount instead now 
	Console.WriteLine("Press any key to subscribe");
	Console.ReadKey();
	var subscription = observable.Subscribe(i => Console.WriteLine("subscription : {0}", i));
	Console.WriteLine("Press any key to unsubscribe.");
	Console.ReadKey();
	subscription.Dispose();
	Console.WriteLine("Press any key to exit.");
	Console.ReadKey();

Output:

<div class="output">
	<div class="line">Press any key to subscribe </div>
	<div class="line">Press any key to unsubscribe. </div>
	<div class="line">Publishing 0 </div>
	<div class="line">subscription : 0 </div>
	<div class="line">Publishing 1 </div>
	<div class="line">subscription : 1 </div>
	<div class="line">Publishing 2 </div>
	<div class="line">subscription : 2 </div>
	<div class="line">Press any key to exit. </div>
</div>

The `Publish`/`RefCount` pair is extremely useful for taking a cold observable and sharing it as a hot observable sequence for subsequent observers.
`RefCount()` also allows us to avoid a race condition. 
In the example above, we subscribed to the sequence before a connection was established. 
This is not always possible, especially if we are exposing the sequence from a method. 
By using the `RefCount` method we can mitigate the subscribe/connect race condition because of the auto-connect behavior.

##Other connectable observables			{#OtherConnectables}

The `Connect` method is not the only method that returns `IConnectableObservable<T>` instances. 
The ability to connect or defer an operator's functionality is useful in other areas too.

###PublishLast 						{#PublishLast}

The `PublishLast()` method is effectively a non-blocking `Last()` call. 
You can consider it similar to an `AsyncSubject<T>` wrapping your target sequence. 
You get equivalent semantics to `AsyncSubject<T>` where only the last value is published, and only once the sequence completes.

	var period = TimeSpan.FromSeconds(1);
	var observable = Observable.Interval(period)
		.Take(5)
		.Do(l => Console.WriteLine("Publishing {0}", l)) //side effect to show it is running
		.PublishLast();
	observable.Connect();
	Console.WriteLine("Press any key to subscribe");
	Console.ReadKey();
	var subscription = observable.Subscribe(i => Console.WriteLine("subscription : {0}", i));
	Console.WriteLine("Press any key to unsubscribe.");
	Console.ReadKey();
	subscription.Dispose();
	Console.WriteLine("Press any key to exit.");
	Console.ReadKey();

Output:

<div class="output">
	<div class="line">Press any key to subscribe </div>
	<div class="line">Publishing 0 </div>
	<div class="line">Publishing 1 </div>
	<div class="line">Press any key to unsubscribe. </div>
	<div class="line">Publishing 2 </div>
	<div class="line">Publishing 3 </div>
	<div class="line">Publishing 4 </div>
	<div class="line">subscription : 4 </div>
	<div class="line">Press any key to exit. </div>
</div>

###Replay							{#Replay}

The `Replay` extension method allows you take an existing observable sequence and give it 'replay' semantics as per `ReplaySubject<T>`. 
As a reminder, the `ReplaySubject<T>` will cache all values so that any late subscribers will also get all of the values. 
In this example, two subscriptions are made on time, and then a third subscription can be made after the sequence completes. 
Even though the third subscription is made after the underlying sequence has completed, we can still get all of the values.

	var period = TimeSpan.FromSeconds(1);
	var hot = Observable.Interval(period)
		.Take(3)
		.Publish();
	hot.Connect();
	Thread.Sleep(period); //Run hot and ensure a value is lost.
	var observable = hot.Replay();
	observable.Connect();
	observable.Subscribe(i => Console.WriteLine("first subscription : {0}", i));
	Thread.Sleep(period);
	observable.Subscribe(i => Console.WriteLine("second subscription : {0}", i));
	Console.ReadKey();
	observable.Subscribe(i => Console.WriteLine("third subscription : {0}", i));
	Console.ReadKey();

Output:

<div class="output">
	<div class="line">first subscription : 1 </div>
	<div class="line">second subscription : 1 </div>
	<div class="line">first subscription : 2 </div>
	<div class="line">second subscription : 2 </div>
	<div class="line">third subscription : 1 </div>
	<div class="line">third subscription : 2 </div>
</div>

The `Replay` extension method has several overloads that match the `ReplaySubject<T>` constructor overloads; you are able to specify the buffer size by count or by time.

###Multicast			{#Multicast}

The `PublishLast` and `Replay` methods effectively apply `AsyncSubject<T>` and `ReplaySubject<T>` functionality to the underlying observable sequence.
We could attempt to build a crude implementation ourselves.

	var period = TimeSpan.FromSeconds(1);
	//var observable = Observable.Interval(period).Publish();
	var observable = Observable.Interval(period);
	var shared = new Subject<long>();
	shared.Subscribe(i => Console.WriteLine("first subscription : {0}", i));
	observable.Subscribe(shared);   //'Connect' the observable.
	Thread.Sleep(period);
	Thread.Sleep(period);
	shared.Subscribe(i => Console.WriteLine("second subscription : {0}", i));

Output:

<div class="output">
	<div class="line">first subscription : 0</div>
	<div class="line">first subscription : 1</div>
	<div class="line">second subscription : 1</div>
	<div class="line">first subscription : 2</div>
	<div class="line">second subscription : 2 </div>
</div>

The Rx library supplies us with a great method to do this well though. 
You can apply subject behavior via the `Multicast` extension method. 
This allows you to share or "multicast" an observable sequence with the behavior of a specific subject.
For example

 * `.Publish()` = `.Multicast(new Subject<T>)`
 * `.PublishLast()` = `.Multicast(new AsyncSubject<T>)`
 * `.Replay()` = `.Multicast(new ReplaySubject<T>)`

Hot and cold observables are two different styles of sharing an observable sequence.
Both have equally valid applications but behave in different ways. 
Cold observables allow you to lazily evaluate an observable sequence independently for each subscriber.
Hot observables allow you to share notifications by multicasting your sequence, even if there are no subscribers. 
The use of `RefCount` allows you to have lazily-evaluated, multicast observable sequences, coupled with eager disposal semantics once the last subscription is disposed.

<!--
	<a name="Defer"></a>
	<h2>Defer
	<p></p>

	<a name="Synchronize"></a>
	<h2>Synchronize
	<p></p>
	-->

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