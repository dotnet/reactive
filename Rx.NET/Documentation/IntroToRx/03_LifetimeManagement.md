---
title: Lifetime management
---

<!--TODO: Link on this page to the C# in a nutshell(?) Amazon via affiliates -->
<!--
    LC.
        re read apply laws of that kindle book.
        Create a Proposition and support it or create and argument and prove it.
        Beginning middle and end. Beginning should give and idea to the destination, should not be a surprise.
        Code and keyword highlights seem inconsistent.
        
        Proposition is that you can explicitly manage the lifetime of your subscriptions to queries.
        

    G.A.
        (overall pretty focused and flows from one point to the next competently; would be even better with some rewriting)
 
    Subscription finalizers
        A caveat is a negative notice, a warning  that doesn't work here.
-->

#Lifetime management				{#LifetimeManagement}

The very nature of Rx code is that you as a consumer do not know when a sequence will provide values or terminate. 
This uncertainty does not prevent your code from providing a level of certainty. 
You can control when you will start accepting values and when you choose to stop accepting values. 
You still need to be the master of your domain. 
Understanding the basics of managing Rx resources allow your applications to be as efficient, bug free and predictable as possible.

Rx provides fine grained control to the lifetime of subscriptions to queries. 
While using familiar interfaces, you can deterministically release resources associated to queries.
This allows you to make the decisions on how to most effectively manage your resources, ideally keeping the scope as tight as possible.

In the previous chapter we introduced you to the key types and got off the ground with some examples. 
For the sake of keeping the initial samples simple we ignored a very important part of the `IObservable<T>` interface. 
The `Subscribe` method takes an `IObserver<T>` parameter, but we did not need to provide that as we used the extension method that took an `Action<T>` instead.
The important part we overlooked is that both `Subscribe` methods have a return value. 
The return type is `IDisposable`. 
In this chapter we will further explore how this return value can be used to management lifetime of our subscriptions.

##Subscribing						{#Subscribe}

Just before we move on, it is worth briefly looking at all of the overloads of the `Subscribe` extension method. 
The overload we used in the previous chapter was the simple [Overload to Subscribe](http://msdn.microsoft.com/en-us/library/ff626574(v=VS.92).aspx "Subscribe Extension method overloads on MSDN") which allowed us to pass just an `Action<T>` to be performed when `OnNext` was invoked. 
Each of these further overloads allows you to avoid having to create and then pass in an instance of `IObserver<T>`.

	//Just subscribes to the Observable for its side effects. 
	// All OnNext and OnCompleted notifications are ignored.
	// OnError notifications are re-thrown as Exceptions.
	IDisposable Subscribe<TSource>(this IObservable<TSource> source);
		
	//The onNext Action provided is invoked for each value.
	//OnError notifications are re-thrown as Exceptions.
	IDisposable Subscribe<TSource>(this IObservable<TSource> source, 
		Action<TSource> onNext);
		
	//The onNext Action is invoked for each value.
	//The onError Action is invoked for errors
	IDisposable Subscribe<TSource>(this IObservable<TSource> source, 
		Action<TSource> onNext, 
		Action<Exception> onError);
		
	//The onNext Action is invoked for each value.
	//The onCompleted Action is invoked when the source completes.
	//OnError notifications are re-thrown as Exceptions.
	IDisposable Subscribe<TSource>(this IObservable<TSource> source, 
		Action<TSource> onNext, 
		Action onCompleted);
		
	//The complete implementation
	IDisposable Subscribe<TSource>(this IObservable<TSource> source, 
		Action<TSource> onNext, 
		Action<Exception> onError, 
		Action onCompleted);

Each of these overloads allows you to pass various combinations of delegates that you want executed for each of the notifications an `IObservable<T>` instance could produce. 
A key point to note is that if you use an overload that does not specify a delegate for the `OnError` notification, any `OnError` notifications will be re-thrown as an exception. 
Considering that the error could be raised at any time, this can make debugging quite difficult. 
It is normally best to use an overload that specifies a delegate to cater for `OnError` notifications.

In this example we attempt to catch error using standard .NET Structured Exception Handling:

	var values = new Subject<int>();
	try
	{
		values.Subscribe(value => Console.WriteLine("1st subscription received {0}", value));
	}
	catch (Exception ex)
	{
		Console.WriteLine("Won't catch anything here!");
	}
		
	values.OnNext(0);
	//Exception will be thrown here causing the app to fail.
	values.OnError(new Exception("Dummy exception"));

The correct way to way to handle exceptions is to provide a delegate for `OnError` notifications as in this example.

	var values = new Subject<int>();
		
	values.Subscribe(
		value => Console.WriteLine("1st subscription received {0}", value),
		ex => Console.WriteLine("Caught an exception : {0}", ex));

	values.OnNext(0);
	values.OnError(new Exception("Dummy exception"));

We will look at other interesting ways to deal with errors on a sequence in later chapters in the book.

##Unsubscribing						{#Unsubscribing}

We have yet to look at how we could unsubscribe from a subscription. 
If you were to look for an _Unsubscribe_ method in the Rx public API you would not find any. 
Instead of supplying an Unsubscribe method, Rx will return an `IDisposable` whenever a subscription is made. 
This disposable can be thought of as the subscription itself, or perhaps a token representing the subscription. 
Disposing it will dispose the subscription and effectively `unsubscribe`. 
Note that calling `Dispose` on the result of a Subscribe call will not cause any side effects for other subscribers; it just removes the subscription from the observable's internal list of subscriptions.
This then allows us to call `Subscribe` many times on a single `IObservable<T>`, allowing subscriptions to come and go without affecting each other. 
In this example we initially have two subscriptions, we then dispose of one subscription early which still allows the other to continue to receive publications from the underlying sequence:

	var values = new Subject<int>();
	var firstSubscription = values.Subscribe(value => 
		Console.WriteLine("1st subscription received {0}", value));
	var secondSubscription = values.Subscribe(value => 
		Console.WriteLine("2nd subscription received {0}", value));
	values.OnNext(0);
	values.OnNext(1);
	values.OnNext(2);
	values.OnNext(3);
	firstSubscription.Dispose();
	Console.WriteLine("Disposed of 1st subscription");
	values.OnNext(4);
	values.OnNext(5);

Output:

<div class="output">
	<div class="line">1st subscription received 0</div>
	<div class="line">2nd subscription received 0</div>
	<div class="line">1st subscription received 1</div>
	<div class="line">2nd subscription received 1</div>
	<div class="line">1st subscription received 2</div>
	<div class="line">2nd subscription received 2</div>
	<div class="line">1st subscription received 3</div>
	<div class="line">2nd subscription received 3</div>
	<div class="line">Disposed of 1st subscription</div>
	<div class="line">2nd subscription received 4</div>
	<div class="line">2nd subscription received 5</div>
</div>

The team building Rx could have created a new interface like _ISubscription_ or _IUnsubscribe_ to facilitate unsubscribing. 
They could have added an _Unsubscribe_ method to the existing `IObservable<T>` interface. 
By using the `IDisposable` type instead we get the following benefits for free:

 * The type already exists
 * People understand the type
 * `IDisposable` has standard usages and patterns
 * Language support via the `using` keyword
 * Static analysis tools like FxCop can help you with its usage
 * The `IObservable<T>` interface remains very simple.

As per the `IDisposable` guidelines, you can call `Dispose` as many times as you like. 
The first call will unsubscribe and any further calls will do nothing as the subscription will have already been disposed.

##OnError and OnCompleted			{#OnErrorAndOnCompleted}

Both the `OnError` and `OnCompleted` signify the completion of a sequence.
If your sequence publishes an `OnError` or `OnCompleted` it will be the last publication and no further calls to `OnNext` can be performed. 
In this example we try to publish an `OnNext` call after an `OnCompleted` and the `OnNext` is ignored:

	var subject = new Subject<int>();
	subject.Subscribe(
		Console.WriteLine, 
		() => Console.WriteLine("Completed"));
	subject.OnCompleted();
	subject.OnNext(2);

Of course, you could implement your own `IObservable<T>` that allows publishing after an `OnCompleted` or an `OnError`, however it would not follow the precedence of the current Subject types and would be a non-standard implementation. 
I think it would be safe to say that the inconsistent behavior would cause unpredictable behavior in the applications that consumed your code.

An interesting thing to consider is that when a sequence completes or errors, you should still dispose of your subscription.

##IDisposable						{#IDisposable}

The `IDisposable` interface is a handy type to have around and it is also integral to Rx. 
I like to think of types that implement `IDisposable` as having explicit lifetime management. 
I should be able to say "I am done with that" by calling the `Dispose()` method.

By applying this kind of thinking, and then leveraging the C# `using` statement, you can create handy ways to create scope. 
As a reminder, the `using` statement is effectively a `try`/`finally` block that will always call `Dispose` on your instance when leaving the scope.

If we consider that we can use the `IDisposable` interface to effectively create a scope, you can create some fun little classes to leverage this. 
For example here is a simple class to log timing events:

	public class TimeIt : IDisposable
	{
		private readonly string _name;
		private readonly Stopwatch _watch;

		public TimeIt(string name)
		{
			_name = name;
			_watch = Stopwatch.StartNew();
		}

		public void Dispose()
		{
			_watch.Stop();
			Console.WriteLine("{0} took {1}", _name, _watch.Elapsed);
		}
	}

This handy little class allows you to create scope and measure the time certain sections of your code base take to run. 
You could use it like this:

	using (new TimeIt("Outer scope"))
	{
		using (new TimeIt("Inner scope A"))
		{
			DoSomeWork("A");
		}
		using (new TimeIt("Inner scope B"))
		{
			DoSomeWork("B");
		}
		Cleanup();
	}

Output:

<div class="output">
	<div class="line">Inner scope A took 00:00:01.0000000</div>
	<div class="line">Inner scope B took 00:00:01.5000000</div>
	<div class="line">Outer scope took 00:00:02.8000000</div>
</div>

You could also use the concept to set the color of text in a console application:

	//Creates a scope for a console foreground color. When disposed, will return to 
	//  the previous Console.ForegroundColor
	public class ConsoleColor : IDisposable
	{
		private readonly System.ConsoleColor _previousColor;

		public ConsoleColor(System.ConsoleColor color)
		{
			_previousColor = Console.ForegroundColor;
			Console.ForegroundColor = color;
		}

		public void Dispose()
		{
			Console.ForegroundColor = _previousColor;
		}
	}

I find this handy for easily switching between colors in little _spike_ console	applications:

	Console.WriteLine("Normal color");
	using (new ConsoleColor(System.ConsoleColor.Red))
	{
		Console.WriteLine("Now I am Red");
		using (new ConsoleColor(System.ConsoleColor.Green))
		{
			Console.WriteLine("Now I am Green");
		}
		Console.WriteLine("and back to Red");
	}

Output:

<div class="output">
	<div class="line" style="color: #C0C0C0;">Normal color</div>
	<div class="line" style="color: #FF0000;">Now I am Red</div>
	<div class="line" style="color: #00FF00;">Now I am Green</div>
	<div class="line" style="color: #FF0000;">and back to Red</div>
</div>

So we can see that you can use the `IDisposable` interface for more than just common use of deterministically releasing unmanaged resources. 
It is a useful tool for managing lifetime or scope of anything; from a stopwatch timer, to the current color of the console text, to the subscription to a sequence of notifications.

The Rx library itself adopts this liberal usage of the `IDisposable` interface and introduces several of its own custom implementations:

 * Disposable
 * BooleanDisposable
 * CancellationDisposable
 * CompositeDisposable
 * ContextDisposable
 * MultipleAssignmentDisposable
 * RefCountDisposable
 * ScheduledDisposable
 * SerialDisposable
 * SingleAssignmentDisposable

For a full rundown of each of the implementations see the [Disposables](20_Disposables.html) reference in the Appendix. 
For now we will look at the extremely simple and useful `Disposable` static class:

	namespace System.Reactive.Disposables
	{
	  public static class Disposable
	  {
		// Gets the disposable that does nothing when disposed.
		public static IDisposable Empty { get {...} }

		// Creates the disposable that invokes the specified action when disposed.
		public static IDisposable Create(Action dispose)
		{...}
	  }
	}

As you can see it exposes two members: `Empty` and `Create`. 
The `Empty` method allows you get a stub instance of an `IDisposable` that does nothing when `Dispose()` is called. 
This is useful for when you need to fulfil an interface requirement that returns an `IDisposable` but you have no specific implementation that is relevant.

The other overload is the `Create` factory method which allows you to pass an `Action` to be invoked when the instance is disposed. 
The `Create` method will ensure the standard Dispose semantics, so calling `Dispose()` multiple times will only invoke the delegate you provide once:

	var disposable = Disposable.Create(() => Console.WriteLine("Being disposed."));
	Console.WriteLine("Calling dispose...");
	disposable.Dispose();
	Console.WriteLine("Calling again...");
	disposable.Dispose();

Output:

<div class="output">
	<div class="line">Calling dispose...</div>
	<div class="line">Being disposed.</div>
	<div class="line">Calling again...</div>
</div>

Note that "Being disposed." is only printed once. 
In a later chapter we cover another	useful method for binding the lifetime of a resource to that of a subscription in the [Observable.Using](11_AdvancedErrorHandling.html#Using) method.

<!--
G.A.
Subscription finalizers
 A caveat is a negative notice, a warning  that doesn't work here.
-->
##Resource management vs. memory management		{#Finalizers}

It seems many .NET developers only have a vague understanding of the .NET runtime's Garbage Collector and specifically how it interacts with Finalizers and `IDisposable`.
As the author of the [Framework Design Guidelines](http://msdn.microsoft.com/en-us/library/ms229042.aspx) points out, this may be due to the confusion between 'resource management' and 'memory management':

<p class="comment">
	Many people who hear about the Dispose pattern for the first time complain that
	the GC isn't doing its job. They think it should collect resources, and that this
	is just like having to manage resources as you did in the unmanaged world. The truth
	is that the GC was never meant to manage resources. It was designed to manage memory
	and it is excellent in doing just that. - <a href="http://blogs.msdn.com/b/kcwalina/">
		Krzysztof Cwalina</a> from <a href="http://www.bluebytesoftware.com/blog/2005/04/08/DGUpdateDisposeFinalizationAndResourceManagement.aspx">
			Joe Duffy's blog</a>
</p>

This is both a testament to Microsoft for making .NET so easy to work with and also a problem as it is a key part of the runtime to misunderstand. 
Considering this, I thought it was prudent to note that _subscriptions will not be automatically disposed of_. 
You can safely assume that the instance of `IDisposable` that is returned to you does not have a finalizer and will not be collected when it goes out of scope. 
If you call a `Subscribe` method and ignore the return value, you have lost your only handle to unsubscribe. 
The subscription will still exist, and you have effectively lost access to this resource, which could result in leaking memory and running unwanted processes.

The exception to this cautionary note is when using the `Subscribe` extension methods. 
These methods will internally construct behavior that will _automatically detach_ subscriptions when the sequence completes or errors. 
Even with the automatic detach behavior; you still need to consider sequences that never terminate (by `OnCompleted` or `OnError`). 
You will need the instance of `IDisposable` to terminate the subscription to these infinite sequences explicitly.

<p class="comment">
	You will find many of the examples in this book will not allocate the `IDisposable`
	return value. This is only for brevity and clarity of the sample. <a href="18_UsageGuidelines.html">
		Usage guidelines</a> and best practice information can be found in the appendix.
</p>

By leveraging the common `IDisposable` interface, Rx offers the ability to have deterministic control over the lifetime of your subscriptions. 
Subscriptions are independent, so the disposable of one will not affect another. 
While some `Subscribe` extension methods utilize an automatically detaching observer, it is still considered best practice to explicitly manage your subscriptions, as you would with any other resource implementing `IDisposable`. 
As we will see in later chapters, a subscription may actually incur the cost of other resources such as event handles, caches and threads. 
It is also best practice to always provide an `OnError` handler to prevent an exception being thrown in an otherwise difficult to handle manner.

With the knowledge of subscription lifetime management, you are able to keep a tight leash on subscriptions and their underlying resources. 
With judicious application of standard disposal patterns to your Rx code, you can keep your applications predictable, easier to maintain, easier to extend and hopefully bug free.

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