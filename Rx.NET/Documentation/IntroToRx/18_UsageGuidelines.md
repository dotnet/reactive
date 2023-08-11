---
title : Appendix A Usage guidelines
---

#Appendix				{#Appendix .SectionHeader}
 
#Usage guidelines		{#UsageGuidelines}

This is a list of quick guidelines intended to help you when writing Rx queries.

 * Members that return a sequence should never return null. 
This applies to `IEnumerable<T>` and `IObservable<T>` sequences. 
Return an empty sequence instead.
 * Dispose of subscriptions.
 * Subscriptions should match their scope.
 * Always provide an `OnError` handler.
 * Avoid breaking the monad with blocking operators such as `First`, `FirstOrDefault`, `Last`, `LastOrDefault`, `Single`, `SingleOrDefault` and `ForEach`.
 * Avoid switching between monads, i.e. going from `IObservable<T>` to `IEnumerable<T>` and back to `IObservable<T>`.
 * Favor lazy evaluation over eager evaluation.
 * Break large queries up into parts. Key indicators of a large query:		
	 1. nesting
	 2. over 10 lines of query comprehension syntax
	 3. using the into statement
 * Name your queries well, i.e. avoid using the names like `query`, `q`, `xs`, `ys`, `subject` etc.
 * Avoid creating side effects. 
If you must create side effects, be explicit by using the `Do` operator.
 * Avoid the use of the subject types. 
Rx is effectively a functional programming paradigm.
Using subjects means we are now managing state, which is potentially mutating. 
Dealing with both mutating state and asynchronous programming at the same time is very hard to get right.
Furthermore, many of the operators (extension methods) have been carefully written to ensure that correct and consistent lifetime of subscriptions and sequences is maintained;
when you introduce subjects, you can break this. 
Future releases may also see significant performance degradation if you explicitly use subjects.
 * Avoid creating your own implementations of the `IObservable<T>` interface.
Favor using the `Observable.Create` factory method overloads instead.
 * Avoid creating your own implementations of the `IObserver<T>` interface.
Favor using the `Subscribe` extension method overloads instead.
 * The subscriber should define the concurrency model. 
The `SubscribeOn` and `ObserveOn` operators should only ever precede a `Subscribe` method.

---
<div class="webonly">
	<h1 class="ignoreToc">Additional recommended reading</h1>
	<div align="center">
		
		<!--Framework Design Guidelines 2nd Edition (Kindle) Amazon.co.uk-->
		<div style="display:inline-block; vertical-align: top; margin: 10px; width: 140px; font-size: 11px; text-align: center">
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=B0017SWPNO&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
					style="width:120px;height:240px;" 
					scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>
		</div>

		<!--Refactoring (Kindle) Amazon.co.uk-->
		<div style="display:inline-block; vertical-align: top; margin: 10px; width: 140px; font-size: 11px; text-align: center">
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=B007WTFWJ6&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
					style="width:120px;height:240px;" 
					scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>
		</div>
		
		<!--CLR via C# v4 Amazon.co.uk-->
		<div style="display:inline-block; vertical-align: top; margin: 10px; width: 140px; font-size: 11px; text-align: center">
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=B00AA36R4U&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
					style="width:120px;height:240px;margin: 10px" 
					scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>

		</div>

		<!--Domain Driven Design (Kindle) Amazon.co.uk-->
		<div style="display:inline-block; vertical-align: top; margin: 10px; width: 140px; font-size: 11px; text-align: center">
			<iframe src="http://rcm-uk.amazon.co.uk/e/cm?t=int0b-21&amp;o=2&amp;p=8&amp;l=as1&amp;asins=B00794TAUG&amp;ref=qf_sp_asin_til&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;m=amazon&amp;lc1=0000FF&amp;bc1=000000&amp;bg1=FFFFFF&amp;f=ifr" 
					style="width:120px;height:240px;" 
					scrolling="no" marginwidth="0" marginheight="0" frameborder="0"></iframe>
		</div>
	</div></div>
