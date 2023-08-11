---
title: Side effects
---

<!--TODO: Link on this page to the Art of Scalability and Continuous Delivery on Amazon via affiliates -->

#PART 3 - Taming the sequence		{#PART3 .SectionHeader}

In the third part to this book we will look the features that allow us to apply Rx to more than just sample code. 
When building production quality code we often need to be able to handle error scenarios, log workflow, retry in certain circumstances, dispose of resources and other real-life problems that are regularly excluded from examples and demos.

Part 3 of this book aims to equip you with the tools you need to be able to useRx as more than just a toy. 
If you use Rx properly, you will find it pervasive in your code base. 
You should not shy away from this, just like you would not shy away from using the `foreach` syntax with `IEnumerable` types, or, the `using` syntax with `IDisposable` types. 
Understanding and embracing Rx will improve your code base by reducing it, by making it more declarative, by identifying and eliminating race conditions, and therefore making it more maintainable.

Maintenance of Rx code obviously requires Rx knowledge but this creates a "chicken and egg" problem. 
I choose to believe that Rx is here to stay. 
I believe this because it solves a targeted set of problems very well. 
It is also complimentary to other libraries and features such as TPL (Task Parallel Library) and the future `async`/`await` features of .NET 4.5. 
Considering this, if Rx improves our code base then we should embrace it!

<hr style="page-break-after: always" />

#Side effects						{#SideEffects}

Non-functional requirements of production systems often demand high availability, quality monitoring features and low lead time for defect resolution. 
Logging, debugging, instrumentation and journaling are common non-functional requirements that developers need to consider for production ready systems. 
These artifacts could be considered side effects of the main business workflow. 
Side effects are a real life problem that code samples and how-to guides often ignore, however Rx provides tools to help.

In this chapter we will discuss the consequences of introducing side effects when working with an observable sequence. 
A function is considered to have a side effect if, in addition to any return value, it has some other observable effect. 
Generally the 'observable effect' is a modification of state. 
This observable effect could be


 * modification of a variable with a wider scope than the function (i.e. global, static	or perhaps an argument)
 * I/O such as a read/write from a file or network
<!--TODO:Validate that readers see the display as an I/O device or not?-->
 * updating a display

<!--TODO: Are there other existing paradigms that allow you to modify state, either safely or explicitly-->

##Issues with side effects			{#IssuesWithSideEffects}
<!--TODO: Maybe could be more terse in v2-->

Functional programming in general tries to avoid creating any side effects. 
Functions with side effects, especially which modify state, require the programmer to understand more than just the inputs and outputs of the function. 
The surface area they are required to understand needs to now extend to the history and context of the state being modified. 
This can greatly increase the complexity of a function, and thus make it harder to correctly understand and maintain.

Side effects are not always accidental, nor are they always intentional. 
An easy way to reduce the accidental side effects is to reduce the surface area for change.
The simple actions coders can take are to reduce the visibility or scope of state and to make what you can immutable. 
You can reduce the visibility of a variable by scoping it to a code block like a method. 
You can reduce visibility of class members by making them private or protected. 
By definition immutable data can't be modified so cannot exhibit side effects. 
These are sensible encapsulation rules that will dramatically improve the maintainability of your Rx code.

To provide a simple example of a query that has a side effect, we will try to output the index and value of the elements received by updating a variable (closure).

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

Output:

<div class="output">
	<div class="line">Received A at index 0</div>
	<div class="line">Received B at index 1</div>
	<div class="line">Received C at index 2</div>
	<div class="line">completed</div>
</div>

While this seems harmless enough, imagine if another person sees this code and understands it to be the pattern the team is using. 
They in turn adopt this style themselves.
For the sake of the example, we will add a duplicate subscription to our previous example.

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

Output

<div class="output">
	<div class="line">Received A at index 0</div>
	<div class="line">Received B at index 1</div>
	<div class="line">Received C at index 2</div>
	<div class="line">completed</div>
	<div class="line">Also received A at index 3</div>
	<div class="line">Also received B at index 4</div>
	<div class="line">Also received C at index 5</div>
	<div class="line">2nd completed</div>
</div>

<!--TODO: Apply forward reference. Where do we show better ways of controlling workflow?-->

Now the second person's output is clearly nonsense. 
They will be expecting index values to be 0, 1 and 2 but get 3, 4 and 5 instead. 
I have seen far more sinister versions of side effects in code bases. 
The nasty ones often modify state that is a Boolean value e.g. `hasValues`, `isStreaming` etc. 
We will see in a later chapter far better ways of controlling workflow with observable sequences than using shared state.

In addition to creating potentially unpredictable results in existing software, programs that exhibit side effects are far more difficult to test and maintain.
Future refactoring, enhancements or other maintenance on programs that exhibits side effects are far more likely to be brittle. 
This is especially so in asynchronous or concurrent software.

##Composing data in a pipeline		{#ComposingDataInAPipeline}

The preferred way of capturing state is to introduce it to the pipeline. 
Ideally, we want each part of the pipeline to be independent and deterministic. 
That is, each function that makes up the pipeline should have its inputs and output as its only state. 
To correct our example we could enrich the data in the pipeline so that there is no shared state. 
This would be a great example where we could use the `Select` overload that exposes the index.

	var source = Observable.Range(0, 3);
	var result = source.Select(
		(idx, value) => new
							{
								Index = idx,
								Letter = (char) (value + 65)
							});

	result.Subscribe(
		x => Console.WriteLine("Received {0} at index {1}", x.Letter, x.Index),
		() => Console.WriteLine("completed"));

	result.Subscribe(
		x => Console.WriteLine("Also received {0} at index {1}", x.Letter, x.Index),
		() => Console.WriteLine("2nd completed"));

Output:

<div class="output">
	<div class="line">Received A at index 0</div>
	<div class="line">Received B at index 1</div>
	<div class="line">Received C at index 2</div>
	<div class="line">completed</div>
	<div class="line">Also received A at index 0</div>
	<div class="line">Also received B at index 1</div>
	<div class="line">Also received C at index 2</div>
	<div class="line">2nd completed</div>
</div>

Thinking outside of the box, we could also use other features like `Scan` to achieve similar results. 
Here is an example.

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

The key here is to isolate the state, and reduce or remove any side effects like mutating state.

##Do								{#Do}

We should aim to avoid side effects, but in some cases it is unavoidable. 
The `Do` extension method allows you to inject side effect behavior. 
The signature of the `Do` extension method looks very much like the `Select` method;

 * They both have various overloads to cater for combinations of `OnNext`, `OnError` and `OnCompleted` handlers
 * They both return and take an observable sequence

The overloads are as follows:

	// Invokes an action with side effecting behavior for each element in the observable 
	//  sequence.
	public static IObservable<TSource> Do<TSource>(
		this IObservable<TSource> source, 
		Action<TSource> onNext)
	{...}

	// Invokes an action with side effecting behavior for each element in the observable 
	//  sequence and invokes an action with side effecting behavior upon graceful termination
	//  of the observable sequence.
	public static IObservable<TSource> Do<TSource>(
		this IObservable<TSource> source, 
		Action<TSource> onNext, 
		Action onCompleted)
	{...}

	// Invokes an action with side effecting behavior for each element in the observable
	//  sequence and invokes an action with side effecting behavior upon exceptional 
	//  termination of the observable sequence.
	public static IObservable<TSource> Do<TSource>(
		this IObservable<TSource> source, 
		Action<TSource> onNext, 
		Action<Exception> onError)
	{...}

	// Invokes an action with side effecting behavior for each element in the observable
	//  sequence and invokes an action with side effecting behavior upon graceful or
	//  exceptional termination of the observable sequence.
	public static IObservable<TSource> Do<TSource>(
		this IObservable<TSource> source, 
		Action<TSource> onNext, 
		Action<Exception> onError, 
		Action onCompleted)
	{...}

	// Invokes the observer's methods for their side effects.
	public static IObservable<TSource> Do<TSource>(
		this IObservable<TSource> source, 
		IObserver<TSource> observer)
	{...}

The `Select` overloads take `Func` arguments for their `OnNext` handlers and also provide the ability to return an observable sequence that is a different type to the source. 
In contrast, the `Do` methods only take an `Action<T>` for the `OnNext` handler, and therefore can only return a sequence that is the same type as the source. 
As each of the arguments that can be passed to the `Do` overloads are actions, they implicitly cause side effects.

<!--TODO: Maybe guide the user better here so they will follow the path that Actions=side effects-->

For the next example, we first define the following methods for logging:

	private static void Log(object onNextValue)
	{
		Console.WriteLine("Logging OnNext({0}) @ {1}", onNextValue, DateTime.Now);
	}
	private static void Log(Exception onErrorValue)
	{
		Console.WriteLine("Logging OnError({0}) @ {1}", onErrorValue, DateTime.Now);
	}
	private static void Log()
	{
		Console.WriteLine("Logging OnCompleted()@ {0}", DateTime.Now);
	}

This code can use `Do` to introduce some logging using the methods from above.

	var source = Observable
		.Interval(TimeSpan.FromSeconds(1))
		.Take(3);
	var result = source.Do(
		i => Log(i),
		ex => Log(ex),
		() => Log());

	result.Subscribe(
		Console.WriteLine,
		() => Console.WriteLine("completed"));

Output:

<div class="output">
	<div class="line">Logging OnNext(0) @ 01/01/2012 12:00:00</div>
	<div class="line">0</div>
	<div class="line">Logging OnNext(1) @ 01/01/2012 12:00:01</div>
	<div class="line">1</div>
	<div class="line">Logging OnNext(2) @ 01/01/2012 12:00:02</div>
	<div class="line">2</div>
	<div class="line">Logging OnCompleted() @ 01/01/2012 12:00:02</div>
	<div class="line">completed</div>
</div>

Note that because the `Do` is earlier in the query chain than the `Subscribe`, it will receive the values first and therefore write to the console first. 
I like to think of the `Do` method as a [wire tap](http://en.wikipedia.org/wiki/Telephone_tapping) to a sequence. 
It gives you the ability to listen in on the sequence, without the ability to modify it.

The most common acceptable side effect I see in Rx is the need to log. 
The signature of `Do` allows you to inject it into a query chain. 
This allows us to add logging into our sequence and retain encapsulation. 
When a repository, service agent or provider exposes an observable sequence, they have the ability to add their side effects (e.g. logging) to the sequence before exposing it publicly. 
Consumers can then append operators to the query (e.g. `Where`, `SelectMany`) and this will not affect the logging of the provider.

Consider the method below. 
It produces numbers but also logs what it produces (to the console for simplicity). 
To the consuming code the logging is transparent.

	private static IObservable<long> GetNumbers()
	{
		return Observable.Interval(TimeSpan.FromMilliseconds(250))
			.Do(i => Console.WriteLine("pushing {0} from GetNumbers", i));
	}

We then call it with this code.

	var source = GetNumbers();
	var result = source.Where(i => i%3 == 0)
		.Take(3)
		.Select(i => (char) (i + 65));

	result.Subscribe(
		Console.WriteLine,
		() => Console.WriteLine("completed"));

Output:

<div class="output">
	<div class="line">pushing 0 from GetNumbers</div>
	<div class="line">A</div>
	<div class="line">pushing 1 from GetNumbers</div>
	<div class="line">pushing 2 from GetNumbers</div>
	<div class="line">pushing 3 from GetNumbers</div>
	<div class="line">D</div>
	<div class="line">pushing 4 from GetNumbers</div>
	<div class="line">pushing 5 from GetNumbers</div>
	<div class="line">pushing 6 from GetNumbers</div>
	<div class="line">G</div>
	<div class="line">completed</div>
</div>

This example shows how producers or intermediaries can apply logging to the sequence regardless of what the end consumer does.

One overload to `Do` allows you to pass in an `IObserver<T>`.
In this overload, each of the `OnNext`, `OnError` and `OnCompleted` methods are passed to the other `Do` overload as each of the actions to perform.

Applying a side effect adds complexity to a query. 
If side effects are a necessary evil, then being explicit will help your fellow coder understand your intentions.
Using the `Do` method is the favored approach to doing so. 
This may seem trivial, but given the inherent complexity of a business domain mixed with asynchrony and concurrency, developers don't need the added complication of side effects hidden in a `Subscribe` or `Select` operator.

##Encapsulating with AsObservable			{#AsObservable}

Poor encapsulation is a way developers can leave the door open for unintended side effects.
Here is a handful of scenarios where carelessness leads to leaky abstractions. 
Our first example may seem harmless at a glance, but has numerous problems.

	public class UltraLeakyLetterRepo
	{
		public ReplaySubject<string> Letters { get; set; }

		public UltraLeakyLetterRepo()
		{
			Letters = new ReplaySubject<string>();
			Letters.OnNext("A");
			Letters.OnNext("B");
			Letters.OnNext("C");
		}
	}

In this example we expose our observable sequence as a property. 
The first problem here is that it is a settable property. 
Consumers could change the entire subject out if they wanted. 
This would be a very poor experience for other consumers of this class. 
If we make some simple changes we can make a class that seems safe enough.

	public class LeakyLetterRepo
	{
		private readonly ReplaySubject<string> _letters;

		public LeakyLetterRepo()
		{
			_letters = new ReplaySubject<string>();
			_letters.OnNext("A");
			_letters.OnNext("B");
			_letters.OnNext("C");
		}
		
		public ReplaySubject<string> Letters
		{
			get { return _letters; }
		}

	}

Now the `Letters` property only has a getter and is backed by a read-only field. 
This is much better. 
Keen readers will note that the `Letters` property returns a `ReplaySubject<string>`. 
This is poor encapsulation, as consumers could call `OnNext`/`OnError`/`OnCompleted`.
To close off that loophole we can simply make the return type an `IObservable<string>`.

	public IObservable<string> Letters
	{
		get { return _letters; }
	}

The class now _looks_ much better. 
The improvement, however, is only cosmetic.
There is still nothing preventing consumers from casting the result back to an `ISubject<string>` and then calling whatever methods they like. 
In this example we see external code pushing their values into the sequence.

	var repo = new ObscuredLeakinessLetterRepo();
	var good = repo.GetLetters();
	var evil = repo.GetLetters();
		
	good.Subscribe(
		Console.WriteLine);

	//Be naughty
	var asSubject = evil as ISubject<string>;
	if (asSubject != null)
	{
		//So naughty, 1 is not a letter!
		asSubject.OnNext("1");
	}
	else
	{
		Console.WriteLine("could not sabotage");
	}

Output:

<div class="output">
	<div class="line">A</div>
	<div class="line">B</div>
	<div class="line">C</div>
	<div class="line">1</div>
</div>

The fix to this problem is quite simple. 
By applying the `AsObservable` extension method, the `_letters` field will be wrapped in a type that only implements `IObservable<T>`.

	public IObservable<string> GetLetters()
	{
		return _letters.AsObservable();
	}

Output:

<div class="output">
	<div class="line">A</div>
	<div class="line">B</div>
	<div class="line">C</div>
	<div class="line">could not sabotage</div>
</div>

While I have used words like 'evil' and 'sabotage' in these examples, it is more often than not an oversight rather than malicious intent that causes problems. 
The failing falls first on the programmer who designed the leaky class. 
Designing interfaces is hard, but we should do our best to help consumers of our code fall into [the pit of success](http://blogs.msdn.com/b/brada/archive/2003/10/02/50420.aspx) by giving them discoverable and consistent types. 
Types become more discoverable if we reduce their surface area to expose only the features we intend our consumers to use. 
In this example we reduced the type's surface area.
We did so by removing the property setter and returning a simpler type via the `AsObservable` method.

##Mutable elements cannot be protected		{#MutableElementsCantBeProtected}

While the `AsObservable` method can encapsulate your sequence, you should still be aware that it gives no protection against mutable elements. 
Consider what consumers of a sequence of this class could do:

	public class Account
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

Here is a quick example of the kind of mess we can make if we choose to modify elements	in a sequence.

	var source = new Subject<Account>();

	//Evil code. It modifies the Account object.
	source.Subscribe(account => account.Name = "Garbage");
	//unassuming well behaved code
	source.Subscribe(
		account=>Console.WriteLine("{0} {1}", account.Id, account.Name),
		()=>Console.WriteLine("completed"));

	source.OnNext(new Account {Id = 1, Name = "Microsoft"});
	source.OnNext(new Account {Id = 2, Name = "Google"});
	source.OnNext(new Account {Id = 3, Name = "IBM"});
	source.OnCompleted();

Output:

<div class="output">
	<div class="line">1 Garbage</div>
	<div class="line">2 Garbage</div>
	<div class="line">3 Garbage</div>
	<div class="line">completed</div>
</div>

Here the second consumer was expecting to get 'Microsoft', 'Google' and 'IBM' but received just 'Garbage'.

Observable sequences will be perceived to be a sequence of resolved events: things that have happened as a statement of fact. 
This implies two things: first, each element represents a snapshot of state at the time of publication, secondly, the information emanates from a trustworthy source. 
We want to eliminate the possibility of tampering. 
Ideally the type `T` will be immutable, solving both of these problems. 
This way, consumers of the sequence can be assured that the data they get is the data that the source produced. 
Not being able to mutate elements may seem limiting as a consumer, but these needs are best met via the [Transformation](08_Transformation.html) operators which provide better encapsulation.

Side effects should be avoided where possible. 
Any combination of concurrency with shared state will commonly demand the need for complex locking, deep understanding of CPU architectures and how they work with the locking and optimization features of the language you use. 
The simple and preferred approach is to avoid shared state, favor immutable data types and utilize query composition and transformation. 
Hiding side effects into `Where` or `Select` clauses can make for very confusing code. 
If a side effect is required, then the `Do` method expresses intent that you are creating a side effect by being explicit.

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