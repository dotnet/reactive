---
title: Combining sequences
---

#Combining sequences				{#CombiningMultipleSequences}

Data sources are everywhere, and sometimes we need to consume data from more than just a single source. 
Common examples that have many inputs include: multi touch surfaces, news feeds, price feeds, social media aggregators, file watchers, heart-beating/polling servers, etc. 
The way we deal with these multiple stimuli is varied too. 
We may want to consume it all as a deluge of integrated data, or one sequence at a time as sequential data. 
We could also get it in an orderly fashion, pairing data values from two sources to be processed together, or perhaps just consume the data from the first source that responds to the request.

We have uncovered the benefits of operator composition; now we turn our focus to sequence composition. 
Earlier on, we briefly looked at operators that work with multiple sequences such as `SelectMany`, `TakeUntil`/`SkipUntil`, `Catch` and `OnErrorResumeNext`. 
These give us a hint at the potential that sequence composition can deliver. 
By uncovering the features of sequence composition with Rx, we find yet another layer of game changing functionality. 
Sequence composition enables you to create complex queries across multiple data sources. 
This unlocks the possibility to write some very powerful and succinct code.

Now we will build upon the concepts covered in the [Advanced Error Handling](11_AdvancedErrorHandling.html) chapter. 
There we were able to provide continuations for sequences that failed. 
We will now examine operators aimed at composing sequences that are still operational instead of sequences that have terminated due to an error.

##Sequential concatenation			{#SimpleConcatenation}

The first methods we will look at are those that concatenate sequences sequentially.
They are very similar to the methods we have seen before for dealing with faulted sequences.

###Concat							{#Concat}

The `Concat` extension method is probably the most simple composition method.
It simply concatenates two sequences. 
Once the first sequence completes, the second sequence is subscribed to and its values are passed on through to the result sequence.
It behaves just like the `Catch` extension method, but will concatenate operational sequences when they complete, instead of faulted sequences when they `OnError`.
The simple signature for `Concat` is as follows.

	// Concatenates two observable sequences. Returns an observable sequence that contains the
	//  elements of the first sequence, followed by those of the second the sequence.
	public static IObservable<TSource> Concat<TSource>(
		this IObservable<TSource> first, 
		IObservable<TSource> second)
	{
		...
	}

Usage of `Concat` is familiar. 
Just like `Catch` or `OnErrorResumeNext`, we pass the continuation sequence to the extension method.

	//Generate values 0,1,2 
	var s1 = Observable.Range(0, 3);
	//Generate values 5,6,7,8,9 
	var s2 = Observable.Range(5, 5);
	s1.Concat(s2)
		.Subscribe(Console.WriteLine);

Returns:

<div class="marble">
	<pre class="line">s1 --0--1--2-|</pre>
	<pre class="line">s2           -5--6--7--8--|</pre>
	<pre class="line">r  --0--1--2--5--6--7--8--|</pre>
</div>

If either sequence was to fault so too would the result sequence. 
In particular, if `s1` produced an `OnError` notification, then `s2` would never be used. 
If you wanted `s2` to be used regardless of how s1 terminates, then `OnErrorResumeNext` would be your best option.

`Concat` also has two useful overloads. 
These overloads allow you to pass multiple observable sequences as either a `params` array or an `IEnumerable<IObservable<T>>`.

	public static IObservable<TSource> Concat<TSource>(
		params IObservable<TSource>[] sources)
	{...}
	
	public static IObservable<TSource> Concat<TSource>(
		this IEnumerable<IObservable<TSource>> sources)
	{...}

The ability to pass an `IEnumerable<IObservable<T>>` means that the multiple sequences can be lazily evaluated. 
The overload that takes a `params` array is well-suited to times when we know how many sequences we want to merge at compile time, whereas the `IEnumerable<IObservable<T>>` overload is a better fit when we do not know this ahead of time.

In the case of the lazily evaluated `IEnumerable<IObservable<T>>`, the `Concat` method will take one sequence, subscribe until it is completed and then switch to the next sequence. 
To help illustrate this, we create a method that returns a sequence of sequences and is sprinkled with logging. 
It returns three observable sequences each with a single value [1], [2] and [3]. 
Each sequence returns its value on a timer delay.

	public IEnumerable<IObservable<long>> GetSequences()
	{
		Console.WriteLine("GetSequences() called");
		Console.WriteLine("Yield 1st sequence");
		yield return Observable.Create<long>(o =>
			{
				Console.WriteLine("1st subscribed to");
				return Observable.Timer(TimeSpan.FromMilliseconds(500))
					.Select(i=>1L)
					.Subscribe(o);
			});
		Console.WriteLine("Yield 2nd sequence");
		yield return Observable.Create<long>(o =>
			{
				Console.WriteLine("2nd subscribed to");
				return Observable.Timer(TimeSpan.FromMilliseconds(300))
					.Select(i=>2L)
					.Subscribe(o);
			});
		Thread.Sleep(1000);     //Force a delay
		Console.WriteLine("Yield 3rd sequence");
		yield return Observable.Create<long>(o =>
			{
				Console.WriteLine("3rd subscribed to");
				return Observable.Timer(TimeSpan.FromMilliseconds(100))
					.Select(i=>3L)
					.Subscribe(o);
			});
		Console.WriteLine("GetSequences() complete");
	}

When we call our `GetSequences` method and concatenate the results, we see the following output using our `Dump` extension method.

	GetSequences().Concat().Dump("Concat");

Output:

<div class="output">
	<div class="line">GetSequences() called</div>
	<div class="line">Yield 1st sequence</div>
	<div class="line">1st subscribed to</div>
	<div class="line">Concat-->1</div>
	<div class="line">Yield 2nd sequence</div>
	<div class="line">2nd subscribed to</div>
	<div class="line">Concat-->2</div>
	<div class="line">Yield 3rd sequence</div>
	<div class="line">3rd subscribed to</div>
	<div class="line">Concat-->3</div>
	<div class="line">GetSequences() complete</div>
	<div class="line">Concat completed</div>
</div>

Below is a marble diagram of the `Concat` operator applied to the `GetSequences` method. 
's1', 's2' and 's3' represent sequence 1, 2 and 3. 
Respectively, 'rs' represents the result sequence.

<div class="marble">
	<pre class="line">s1-----1|</pre>
	<pre class="line">s2      ---2|</pre>
	<pre class="line">s3          -3|</pre>
	<pre class="line">rs-----1---2-3|</pre>
</div>

You should note that the second sequence is only yielded once the first sequence has completed. 
To prove this, we explicitly put in a 500ms delay on producing a value and completing. 
Once that happens, the second sequence is then subscribed to. 
When that sequence completes, then the third sequence is processed in the same fashion.

###Repeat							{#Repeat}

Another simple extension method is `Repeat`. 
It allows you to simply repeat a sequence, either a specified or an infinite number of times.

	// Repeats the observable sequence indefinitely and sequentially.
	public static IObservable<TSource> Repeat<TSource>(
		this IObservable<TSource> source)
	{...}

	//Repeats the observable sequence a specified number of times.
	public static IObservable<TSource> Repeat<TSource>(
		this IObservable<TSource> source, 
		int repeatCount)
	{...}

If you use the overload that loops indefinitely, then the only way the sequence will stop is if there is an error or the subscription is disposed of. 
The overload that specifies a repeat count will stop on error, un-subscription, or when it reaches that count. 
This example shows the sequence [0,1,2] being repeated three times.

	var source = Observable.Range(0, 3);
	var result = source.Repeat(3);

	result.Subscribe(
		Console.WriteLine,
		() => Console.WriteLine("Completed"));

Output:

<div class="output">
	<div class="line">0</div>
	<div class="line">1</div>
	<div class="line">2</div>
	<div class="line">0</div>
	<div class="line">1</div>
	<div class="line">2</div>
	<div class="line">0</div>
	<div class="line">1</div>
	<div class="line">2</div>
	<div class="line">Completed</div>
</div>

###StartWith						{#StartWith}

Another simple concatenation method is the `StartWith` extension method.
It allows you to prefix values to a sequence. 
The method signature takes a `params` array of values so it is easy to pass in as many or as few values as you need.

	// prefixes a sequence of values to an observable sequence.
	public static IObservable<TSource> StartWith<TSource>(
		this IObservable<TSource> source, 
		params TSource[] values)
	{
		...
	}

Using `StartWith` can give a similar effect to a `BehaviorSubject<T>` by ensuring a value is provided as soon as a consumer subscribes. 
It is not the same as a `BehaviorSubject` however, as it will not cache the last value.

In this example, we prefix the values -3, -2 and -1 to the sequence [0,1,2].

	//Generate values 0,1,2 
	var source = Observable.Range(0, 3);
	var result = source.StartWith(-3, -2, -1);
	result.Subscribe(
		Console.WriteLine,
		() => Console.WriteLine("Completed"));

Output:

<div class="output">
	<div class="line">-3</div>
	<div class="line">-2</div>
	<div class="line">-1</div>
	<div class="line">0</div>
	<div class="line">1</div>
	<div class="line">2</div>
	<div class="line">Completed</div>
</div>

##Concurrent sequences				{#ConcurrentSequences}

The next set of methods aims to combine observable sequences that are producing values concurrently. 
This is an important step in our journey to understanding Rx.
For the sake of simplicity, we have avoided introducing concepts related to concurrency until we had a broad understanding of the simple concepts.

###Amb								{#Amb}

The `Amb` method was a new concept to me when I started using Rx. 
It is a non-deterministic function, first introduced by John McCarthy and is an abbreviation of the word _Ambiguous_. 
The Rx implementation will return values from the sequence that is first to produce values, and will completely ignore the other sequences.
In the examples below I have three sequences that all produce values. 
The sequences can be represented as the marble diagram below.

<div class="marble">
	<pre class="line">s1 -1--1--|</pre>
	<pre class="line">s2 --2--2--|</pre>
	<pre class="line">s3 ---3--3--|</pre>
	<pre class="line">r  -1--1--|</pre>
</div>

The code to produce the above is as follows.

	var s1 = new Subject<int>();
	var s2 = new Subject<int>();
	var s3 = new Subject<int>();

	var result = Observable.Amb(s1, s2, s3);

	result.Subscribe(
		Console.WriteLine,
		() => Console.WriteLine("Completed"));

	s1.OnNext(1);
	s2.OnNext(2);
	s3.OnNext(3);
	s1.OnNext(1);
	s2.OnNext(2);
	s3.OnNext(3);
	s1.OnCompleted();
	s2.OnCompleted();
	s3.OnCompleted();

Output:

<div class="output">
	<div class="line">1</div>
	<div class="line">1</div>
	<div class="line">Completed</div>
</div>

If we comment out the first `s1.OnNext(1);` then s2 would produce values first and the marble diagram would look like this.

<div class="marble">
	<pre class="line">s1 ---1--|</pre>
	<pre class="line">s2 -2--2--|</pre>
	<pre class="line">s3 --3--3--|</pre>
	<pre class="line">r  -2--2--|</pre>
</div>

The `Amb` feature can be useful if you have multiple cheap resources that can provide values, but latency is widely variable. 
For an example, you may have servers replicated around the world. 
Issuing a query is cheap for both the client to send and for the server to respond, however due to network conditions the latency is not predictable and varies considerably. 
Using the `Amb` operator, you can send the same request out to many servers and consume the result of the first that responds.

There are other useful variants of the `Amb` method. 
We have used the overload that takes a `params` array of sequences. 
You could alternatively use it as an extension method and chain calls until you have included all the target sequences (e.g. s1.Amb(s2).Amb(s3)). 
Finally, you could pass in an `IEnumerable<IObservable<T>>`.

	// Propagates the observable sequence that reacts first.
	public static IObservable<TSource> Amb<TSource>(
		this IObservable<TSource> first, 
		IObservable<TSource> second)
	{...}
	public static IObservable<TSource> Amb<TSource>(
		params IObservable<TSource>[] sources)
	{...}
	public static IObservable<TSource> Amb<TSource>(
		this IEnumerable<IObservable<TSource>> sources)
	{...}

Reusing the `GetSequences` method from the `Concat` section, we see that the evaluation of the outer (IEnumerable) sequence is eager.

	GetSequences().Amb().Dump("Amb");

Output:

<div class="output">
	<div class="line">GetSequences() called</div>
	<div class="line">Yield 1st sequence</div>
	<div class="line">Yield 2nd sequence</div>
	<div class="line">Yield 3rd sequence</div>
	<div class="line">GetSequences() complete</div>
	<div class="line">1st subscribed to</div>
	<div class="line">2nd subscribed to</div>
	<div class="line">3rd subscribed to</div>
	<div class="line">Amb-->3</div>
	<div class="line">Amb completed</div>
</div>

Marble:

<div class="marble">
	<pre class="line">s1-----1|</pre>
	<pre class="line">s2---2|</pre>
	<pre class="line">s3-3|</pre>
	<pre class="line">rs-3|</pre>
</div>

Take note that the inner observable sequences are not subscribed to until the outer sequence has yielded them all. 
This means that the third sequence is able to return values the fastest even though there are two sequences yielded one second before it (due to the `Thread.Sleep`).

###Merge							{#Merge}

The `Merge` extension method does a primitive combination of multiple concurrent sequences. 
As values from any sequence are produced, those values become part of the result sequence. 
All sequences need to be of the same type, as per the previous methods. 
In this diagram, we can see `s1` and `s2` producing values concurrently and the values falling through to the result sequence as they occur.

<div class="marble">
	<pre class="line">s1 --1--1--1--|</pre>
	<pre class="line">s2 ---2---2---2|</pre>
	<pre class="line">r  --12-1-21--2|</pre>
</div>

The result of a `Merge` will complete only once all input sequences complete.
By contrast, the `Merge` operator will error if any of the input sequences terminates erroneously.

	//Generate values 0,1,2 
	var s1 = Observable.Interval(TimeSpan.FromMilliseconds(250))
		.Take(3);
	//Generate values 100,101,102,103,104 
	var s2 = Observable.Interval(TimeSpan.FromMilliseconds(150))
		.Take(5)
		.Select(i => i + 100);
	s1.Merge(s2)
		.Subscribe(
			Console.WriteLine,
			()=>Console.WriteLine("Completed"));

The code above could be represented by the marble diagram below. 
In this case, each unit of time is 50ms. 
As both sequences produce a value at 750ms, there is a race condition and we cannot be sure which value will be notified first in the result sequence (sR).

<div class="marble">
	<pre class="line">s1 ----0----0----0| </pre>
	<pre class="line">s2 --0--0--0--0--0|</pre>
	<pre class="line">sR --0-00--00-0--00|</pre>
</div>

Output:

<div class="output">
	<div class="line">100</div>
	<div class="line">0</div>
	<div class="line">101</div>
	<div class="line">102</div>
	<div class="line">1</div>
	<div class="line">103</div>
	<div class="line">104 //Note this is a race condition. 2 could be </div>
	<div class="line">2 // published before 104. </div>
</div>

You can chain this overload of the `Merge` operator to merge multiple sequences.
`Merge` also provides numerous other overloads that allow you to pass more than two source sequences. 
You can use the static method `Observable.Merge` which takes a `params` array of sequences that is known at compile time.
You could pass in an `IEnumerable` of sequences like the `Concat` method. 
`Merge` also has the overload that takes an `IObservable<IObservable<T>>`, a nested observable. To summarize:

 * Chain `Merge` operators together e.g. `s1.Merge(s2).Merge(s3)`
 * Pass a `params` array of sequences to the `Observable.Merge` static method. e.g. `Observable.Merge(s1,s2,s3)`
 * Apply the `Merge` operator to an `IEnumerable<IObservable<T>>`.
 * Apply the `Merge` operator to an `IObservable<IObservable<T>>`.

Merge overloads:

	/// Merges two observable sequences into a single observable sequence.
	/// Returns a sequence that merges the elements of the given sequences.
	public static IObservable<TSource> Merge<TSource>(
		this IObservable<TSource> first, 
		IObservable<TSource> second)
	{...}
	
	// Merges all the observable sequences into a single observable sequence.
	// The observable sequence that merges the elements of the observable sequences.
	public static IObservable<TSource> Merge<TSource>(
		params IObservable<TSource>[] sources)
	{...}
	
	// Merges an enumerable sequence of observable sequences into a single observable sequence.
	public static IObservable<TSource> Merge<TSource>(
		this IEnumerable<IObservable<TSource>> sources)
	{...}
	
	// Merges an observable sequence of observable sequences into an observable sequence.
	// Merges all the elements of the inner sequences in to the output sequence.
	public static IObservable<TSource> Merge<TSource>(
		this IObservable<IObservable<TSource>> sources)
	{...}

For merging a known number of sequences, the first two operators are effectively the same thing and which style you use is a matter of taste: either provide them as a `params` array or chain the operators together. 
The third and fourth overloads allow to you merge sequences that can be evaluated lazily at run time.
The `Merge` operators that take a sequence of sequences make for an interesting concept. 
You can either pull or be pushed observable sequences, which will be subscribed to immediately.

If we again reuse the `GetSequences` method, we can see how the `Merge` operator works with a sequence of sequences.

	GetSequences().Merge().Dump("Merge");

Output:

<div class="output">
	<div class="line">GetSequences() called</div>
	<div class="line">Yield 1st sequence</div>
	<div class="line">1st subscribed to</div>
	<div class="line">Yield 2nd sequence</div>
	<div class="line">2nd subscribed to</div>
	<div class="line">Merge-->2</div>
	<div class="line">Merge-->1</div>
	<div class="line">Yield 3rd sequence</div>
	<div class="line">3rd subscribed to</div>
	<div class="line">GetSequences() complete</div>
	<div class="line">Merge-->3</div>
	<div class="line">Merge completed</div>
</div>

As we can see from the marble diagram, s1 and s2 are yielded and subscribed to immediately.
s3 is not yielded for one second and then is subscribed to. 
Once all input sequences have completed, the result sequence completes.

<div class="marble">
	<pre class="line">s1-----1|</pre>
	<pre class="line">s2---2|</pre>
	<pre class="line">s3          -3|</pre>
	<pre class="line">rs---2-1-----3|</pre>
</div>

###Switch							{#Switch}

Receiving all values from a nested observable sequence is not always what you need.
In some scenarios, instead of receiving everything, you may only want the values from the most recent inner sequence. 
A great example of this is live searches. 
As you type, the text is sent to a search service and the results are returned to you as an observable sequence.
Most implementations have a slight delay before sending the request so that unnecessary work does not happen. 
Imagine I want to search for "Intro to Rx". 
I quickly type in "Into to" and realize I have missed the letter 'r'. 
I stop briefly and change the text to "Intro ". 
By now, two searches have been sent to the server. 
The first search will return results that I do not want. 
Furthermore, if I were to receive data for the first search merged together with results for the second search, it would be a very odd experience for the user. 
This scenario fits perfectly with the `Switch` method.

In this example, there is a source that represents a sequence of search text.
<!--When the user types in a new value, the source sequence OnNext's the value-->
Values the user types are represented as the source sequence. 
Using `Select`, we pass the value of the search to a function that takes a `string` and returns an `IObservable<string>`. 
This creates our resulting nested sequence, `IObservable<IObservable<string>>`.

Search function signature:

	private IObservable<string> SearchResults(string query)
	{
		...
	}

Using `Merge` with overlapping search:

	IObservable<string> searchValues = ....;
	IObservable<IObservable<string>> search = searchValues
		.Select(searchText=>SearchResults(searchText));
					 
	var subscription = search
		.Merge()
		.Subscribe(
			Console.WriteLine);

<!--TODO: Show output here-->

If we were lucky and each search completed before the next element from `searchValues` was produced, the output would look sensible. 
It is much more likely, however that multiple searches will result in overlapped search results. 
This marble diagram shows what the `Merge` function could do in such a situation.

 * `SV` is the searchValues sequence
 * `S1` is the search result sequence for the first value in searchValues/SV
 * `S2` is the search result sequence for the second value in searchValues/SV
 * `S3` is the search result sequence for the third value in searchValues/SV
 * `RM` is the result sequence for the merged (`R`esult `M`erge) sequences

<div class="marble">
	<pre class="line">SV--1---2---3---|</pre>
	<pre class="line">S1  -1--1--1--1|</pre>
	<pre class="line">S2      --2-2--2--2|</pre>
	<pre class="line">S3          -3--3|</pre>
	<pre class="line">RM---1--1-2123123-2|</pre>
</div>

Note how the values from the search results are all mixed together. 
This is not what we want. 
If we use the `Switch` extension method we will get much better results. 
`Switch` will subscribe to the outer sequence and as each inner sequence is yielded it will subscribe to the new inner sequence and dispose of the subscription to the previous inner sequence. 
This will result in the following marble diagram where `RS` is the result sequence for the Switch (`R`esult `S`witch) sequences

<div class="marble">
	<pre class="line">SV--1---2---3---|</pre>
	<pre class="line">S1  -1--1--1--1|</pre>
	<pre class="line">S2      --2-2--2--2|</pre>
	<pre class="line">S3          -3--3|</pre>
	<pre class="line">RS --1--1-2-23--3|</pre>
</div>

Also note that, even though the results from S1 and S2 are still being pushed, they are ignored as their subscription has been disposed of. 
This eliminates the issue of overlapping values from the nested sequences.

##Pairing sequences					{#ParingSequences}

The previous methods allowed us to flatten multiple sequences sharing a common type into a result sequence of the same type. 
These next sets of methods still take multiple sequences as an input, but attempt to pair values from each sequence to produce a single value for the output sequence. 
In some cases, they also allow you to provide sequences of different types.

###CombineLatest					{#CombineLatest}

The `CombineLatest` extension method allows you to take the most recent value from two sequences, and with a given function transform those into a value for the result sequence. 
Each input sequence has the last value cached like `Replay(1)`.
Once both sequences have produced at least one value, the latest output from each sequence is passed to the `resultSelector` function every time either sequence produces a value. 
The signature is as follows.

	// Composes two observable sequences into one observable sequence by using the selector 
	//  function whenever one of the observable sequences produces an element.
	public static IObservable<TResult> CombineLatest<TFirst, TSecond, TResult>(
		this IObservable<TFirst> first, 
		IObservable<TSecond> second, 
		Func<TFirst, TSecond, TResult> resultSelector)
	{...}

The marble diagram below shows off usage of `CombineLatest` with one sequence that produces numbers (N), and the other letters (L). 
If the `resultSelector` function just joins the number and letter together as a pair, this would be the result (R):

<div class="marble">
	<pre class="line">N---1---2---3---</pre>
	<pre class="line">L--a------bc----</pre>
	<pre class="line">                </pre>
	<pre class="line">R---1---2-223---</pre>
	<pre class="line">    a   a bcc   </pre>
</div>

If we slowly walk through the above marble diagram, we first see that `L` produces the letter 'a'. 
`N` has not produced any value yet so there is nothing to pair, no value is produced for the result (R). 
Next, `N` produces the number '1' so we now have a pair '1a' that is yielded in the result sequence. 
We then receive the number '2' from `N`. The last letter is still 'a' so the next pair is '2a'. 
The letter 'b' is then produced creating the pair '2b', followed by 'c' giving '2c'. 
Finally the number 3 is produced and we get the pair '3c'.

This is great in case you need to evaluate some combination of state which needs to be kept up-to-date when the state changes. 
A simple example would be a monitoring system. 
Each service is represented by a sequence that returns a Boolean indicating the availability of said service. 
The monitoring status is green if all services are available; we can achieve this by having the result selector perform a logical AND. 
Here is an example.

	IObservable<bool> webServerStatus = GetWebStatus();
	IObservable<bool> databaseStatus = GetDBStatus();
	//Yields true when both systems are up.
	var systemStatus = webServerStatus
		.CombineLatest(
			databaseStatus,
			(webStatus, dbStatus) => webStatus &amp;&amp; dbStatus);

Some readers may have noticed that this method could produce a lot of duplicate values. 
For example, if the web server goes down the result sequence will yield '`false`'. 
If the database then goes down, another (unnecessary) '`false`' value will be yielded. 
This would be an appropriate time to use the `DistictUntilChanged` extension method. 
The corrected code would look like the example below.

	//Yields true when both systems are up, and only on change of status
	var systemStatus = webServerStatus
		.CombineLatest(
			databaseStatus,
			(webStatus, dbStatus) => webStatus &amp;&amp; dbStatus)
		.DistinctUntilChanged();

To provide an even better service, we could provide a default value by prefixing `false` to the sequence.

	//Yields true when both systems are up, and only on change of status
	var systemStatus = webServerStatus
		.CombineLatest(
			databaseStatus,
			(webStatus, dbStatus) => webStatus &amp;&amp; dbStatus)
		.DistinctUntilChanged()
		.StartWith(false);


###Zip								{#Zip}

The `Zip` extension method is another interesting merge feature. 
Just like a zipper on clothing or a bag, the `Zip` method brings together two sequences of values as pairs; two by two. 
Things to note about the `Zip` function is that the result sequence will complete when the first of the sequences complete, it will error if either of the sequences error and it will only publish once it has a pair of fresh values from each source sequence. 
So if one of the source sequences publishes values faster than the other sequence, the rate of publishing will be dictated by the slower of the two sequences.

	//Generate values 0,1,2 
	var nums = Observable.Interval(TimeSpan.FromMilliseconds(250))
		.Take(3);
	//Generate values a,b,c,d,e,f 
	var chars = Observable.Interval(TimeSpan.FromMilliseconds(150))
		.Take(6)
		.Select(i => Char.ConvertFromUtf32((int)i + 97));
	//Zip values together
	nums.Zip(chars, (lhs, rhs) => new { Left = lhs, Right = rhs })
		.Dump("Zip");

This can be seen in the marble diagram below. 
Note that the result uses two lines so that we can represent a complex type, i.e. the anonymous type with the properties Left and Right.

<div class="marble">
	<pre class="line">nums  ----0----1----2| </pre>
	<pre class="line">chars --a--b--c--d--e--f| </pre>
	<pre class="line">                        </pre>
	<pre class="line">result----0----1----2|</pre>
	<pre class="line">          a    b    c|</pre>
</div>

The actual output of the code:

<div class="output">
	<div class="line">{ Left = 0, Right = a }</div>
	<div class="line">{ Left = 1, Right = b }</div>
	<div class="line">{ Left = 2, Right = c }</div>
</div>

Note that the `nums` sequence only produced three values before completing, while the `chars` sequence produced six values. 
The result sequence thus has three values, as this was the most pairs that could be made.

The first use I saw of `Zip` was to showcase drag and drop. 
[The example](http://channel9.msdn.com/Blogs/J.Van.Gogh/Writing-your-first-Rx-Application) tracked mouse movements from a `MouseMove` event that would produce event arguments with its current X,Y coordinates. 
First, the example turns the event into an observable sequence. 
Then they cleverly zipped the sequence with a `Skip(1)` version of the same sequence. 
This allows the code to get a delta of the mouse position, i.e. where it is now (sequence.Skip(1)) minus where it was (sequence). 
It then applied the delta to the control it was dragging.

To visualize the concept, let us look at another marble diagram. 
Here we have the mouse movement (MM) and the Skip 1 (S1). 
The numbers represent the index of the mouse movement.

<div class="marble">
	<pre class="line">MM --1--2--3--4--5</pre>
	<pre class="line">S1    --2--3--4--5</pre>
	<pre class="line">                  </pre>
	<pre class="line">Zip   --1--2--3--4</pre>
	<pre class="line">        2  3  4  5</pre>
</div>

Here is a code sample where we fake out some mouse movements with our own subject.

	var mm = new Subject<Coord>();
	var s1 = mm.Skip(1);

	var delta = mm.Zip(s1,
						(prev, curr) => new Coord
							{
								X = curr.X - prev.X,
								Y = curr.Y - prev.Y
							});

	delta.Subscribe(
		Console.WriteLine,
		() => Console.WriteLine("Completed"));

	mm.OnNext(new Coord { X = 0, Y = 0 });
	mm.OnNext(new Coord { X = 1, Y = 0 }); //Move across 1
	mm.OnNext(new Coord { X = 3, Y = 2 }); //Diagonally up 2
	mm.OnNext(new Coord { X = 0, Y = 0 }); //Back to 0,0
	mm.OnCompleted();

This is the simple Coord(inate) class we use.

	public class Coord
	{
		public int X { get; set; }
		public int Y { get; set; }
		public override string ToString()
		{
			return string.Format("{0},{1}", X, Y);
		}
	}

Output:

<div class="output">
	<div class="line">0,1</div>
	<div class="line">2,2</div>
	<div class="line">-3,-2</div>
	<div class="line">Completed</div>
</div>

It is also worth noting that `Zip` has a second overload that takes an `IEnumerable<T>` as the second input sequence.

	// Merges an observable sequence and an enumerable sequence into one observable sequence 
	//  containing the result of pair-wise combining the elements by using the selector function.
	public static IObservable<TResult> Zip<TFirst, TSecond, TResult>(
		this IObservable<TFirst> first, 
		IEnumerable<TSecond> second, 
		Func<TFirst, TSecond, TResult> resultSelector)
	{...}

This allows us to zip sequences from both `IEnumerable<T>` and `IObservable<T>` paradigms!

###And-Then-When					{#AndThenWhen}

If `Zip` only taking two sequences as an input is a problem, then you can use a combination of the three `And`/`Then`/`When` methods.
These methods are used slightly differently from most of the other Rx methods. 
Out of these three, `And` is the only extension method to `IObservable<T>`.
Unlike most Rx operators, it does not return a sequence; instead, it returns the mysterious type `Pattern<T1, T2>`. 
The `Pattern<T1, T2>` type is public (obviously), but all of its properties are internal. 
The only two (useful) things you can do with a `Pattern<T1, T2>` are invoking its `And` or `Then` methods. 
The `And` method called on the `Pattern<T1, T2>` returns a `Pattern<T1, T2, T3>`. 
On that type, you will also find the `And` and `Then` methods. 
The generic `Pattern` types are there to allow you to chain multiple `And` methods together, each one extending the generic type parameter list by one. 
You then bring them all together with the `Then` method overloads. 
The `Then` methods return you a `Plan` type. 
Finally, you pass this `Plan` to the `Observable.When` method in order to create your sequence.

It may sound very complex, but comparing some code samples should make it easier to understand. 
It will also allow you to see which style you prefer to use.

To `Zip` three sequences together, you can either use `Zip` methods chained together like this:

	var one = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);
	var two = Observable.Interval(TimeSpan.FromMilliseconds(250)).Take(10);
	var three = Observable.Interval(TimeSpan.FromMilliseconds(150)).Take(14);
	
	//lhs represents 'Left Hand Side'
	//rhs represents 'Right Hand Side'
	var zippedSequence = one
		.Zip(two, (lhs, rhs) => new {One = lhs, Two = rhs})
		.Zip(three, (lhs, rhs) => new {One = lhs.One, Two = lhs.Two, Three = rhs});

	zippedSequence.Subscribe(
		Console.WriteLine,
		() => Console.WriteLine("Completed"));

Or perhaps use the nicer syntax of the `And`/`Then`/`When`:

	var pattern = one.And(two).And(three);
	var plan = pattern.Then((first, second, third)=>new{One=first, Two=second, Three=third});
	var zippedSequence = Observable.When(plan);

	zippedSequence.Subscribe(
		Console.WriteLine,
		() => Console.WriteLine("Completed"));

This can be further reduced, if you prefer, to:

	var zippedSequence = Observable.When(
			one.And(two)
			   .And(three)
			   .Then((first, second, third) => 
					new { 
						One = first, 
						Two = second, 
						Three = third 
					})
			);

	zippedSequence.Subscribe(
		Console.WriteLine,
		() => Console.WriteLine("Completed"));

The `And`/`Then`/`When` trio has more overloads that enable you to group an even greater number of sequences. 
They also allow you to provide more than one 'plan' (the output of the `Then` method). 
This gives you the `Merge` feature but on the collection of 'plans'. 
I would suggest playing around with them if this functionality is of interest to you. 
The verbosity of enumerating all of the combinations of these methods would be of low value.
You will get far more value out of using them and discovering for yourself.

As we delve deeper into the depths of what the Rx libraries provide us, we can see more practical usages for it. 
Composing sequences with Rx allows us to easily make sense of the multiple data sources a problem domain is exposed to. 
We can concatenate values or sequences together sequentially with `StartWith`, `Concat` and `Repeat`. 
We can process multiple sequences concurrently with `Merge`, or process a single sequence at a time with `Amb` and `Switch`. 
Pairing values with `CombineLatest`, `Zip` and the `And`/`Then`/`When` operators can simplify otherwise fiddly operations like our drag-and-drop examples and monitoring system status.

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
			<!--Essential Linq Amazon.co.uk-->
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=B001XT616O&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
					style="width:120px;height:240px;" 
					scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>
		</div>
		<div style="display:inline-block; vertical-align: top; margin: 10px; width: 140px; font-size: 11px; text-align: center">
			<!--Real-world functional programming Amazon.co.uk-->
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=1933988924&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
					style="width:120px;height:240px;margin: 10px" 
					scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>

		</div>           
	</div></div>