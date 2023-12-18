# Introduction to Rx
By Ian Griffiths and Lee Campbell
   
---

Reactive programming is not a new concept. Any kind of user interface development necessarily involves code that responds to events. Languages like [Smalltalk](https://en.wikipedia.org/wiki/Smalltalk), [Delphi](https://en.wikipedia.org/wiki/Delphi_(software)) and the .NET languages have popularized reactive or event-driven programming paradigms. Architectural patterns such as [CEP (Complex Event Processing)](https://en.wikipedia.org/wiki/Complex_event_processing), and [CQRS (Command Query Responsibility Segregation)](https://en.wikipedia.org/wiki/Command_Query_Responsibility_Segregation) have events as a fundamental part of their makeup. Reactive programming is a useful concept in any program that has to deal with things happening.

> Reactive programming is a useful concept in any program that has to deal with things happening.

The event driven paradigm allows for code to be invoked without the need for breaking encapsulation or applying expensive polling techniques. There are many common ways to implement this, including the [Observer pattern](https://en.wikipedia.org/wiki/Observer_pattern), [events exposed directly in the language (e.g. C#)](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/events/) or other forms of callback via delegate registration. The Reactive Extensions extend the callback metaphor with LINQ to enable querying sequences of events and managing concurrency.

The .NET runtime libraries have included the `IObservable<T>` and `IObserver<T>` interfaces that represent the core concept of reactive programming for well over a decade now. The Reactive Extensions for .NET (Rx.NET) are effectively a library of implementations of these interfaces. Rx.NET first appeared back in 2010 but since then, Rx libraries have become available for other languages, and this way of programming has become especially popular in JavaScript.

This book will introduce Rx via C#. The concepts are universal, so users of other .NET languages such as VB.NET and F#, will be able to extract the concepts and translate them to their particular language.

Rx.NET is just a library, originally created by Microsoft, but now an open source project supported entirely through community effort. (Rx's current lead maintainer, [Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/), is also the author of the latest revision of this book, and indeed the author of this very sentence.)

If you have never used Rx before, it _will_ change the way you design and build software. It provides a well thought out abstraction for a fundamentally important idea in computing: sequences of events. These are as important as lists or arrays, but before Rx there was little direct support in libraries or languages, and what support there was tended to be rather ad hoc, and built on weak theoretical underpinnings. Rx changes that. The extent to which this Microsoft invention has been wholehearted adopted by some developer communities traditionally not especially Microsoft-friendly is a testament to the quality of its fundamental design.

This book aims to teach you:

  * about the types that Rx defines
  * about the extension methods Rx provides, and how to use them
  * how to manage subscriptions to event sources
  * how to visualize "sequences" of data and sketch your solution before coding it
  * how to deal with concurrency to your advantage and avoid common pitfalls
  * how to compose, aggregate and transform streams
  * how to test your Rx code
  * some common best practices when using Rx
    
The best way to learn Rx is to use it. Reading the theory from this book will only help you be familiar with Rx, but to fully understand it you should build things with it. So we warmly encourage you to build based on the examples in this book.

# Acknowledgements

Firstly, I ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/)) should make it clear that this revised edition builds on the excellent work of the original author [Lee Campbell](https://github.com/LeeCampbell). I am grateful that he generously allowed the Rx.NET project to make use of his content, enabling this new edition to come into existence.

I would also like to recognize the people that made this book possible.

Thanks to everyone at [endjin](https://endjin.com) and especially [Howard van Rooijen](https://endjin.com/who-we-are/our-people/howard-van-rooijen/) and [Matthew Adams](https://endjin.com/who-we-are/our-people/matthew-adams/)
for funding not only the updates to this book, but also the ongoing development of Rx.NET itself. (And thanks for employing me too!). Thanks also to [Felix Corke](https://www.blackspike.com/) for his work on the design elements of the [web edition of the book](https://introtorx.com).

Crucial to the first edition of the book, in addition to the author, [Lee Campbell](https://leecampbell.com/), were: James Miles, Matt Barrett, [John Marks](https://johnhmarks.wordpress.com/), Duncan Mole, Cathal Golden, Keith Woods, Ray Booysen, Olivier DeHeurles, [Matt Davey](https://mdavey.wordpress.com), [Joe Albahari](https://www.albahari.com/) and Gregory Andrien. 

Extra special thanks to the team at Microsoft that did the hard work and brought us Rx; [Jeffrey Van Gogh](https://www.linkedin.com/in/jeffrey-van-gogh-145673/), [Wes Dyer](https://www.linkedin.com/in/wesdyer/), [Erik Meijer](https://en.wikipedia.org/wiki/Erik_Meijer_%28computer_scientist%29) & [Bart De Smet](https://www.linkedin.com/in/bartdesmet/).

Thanks also to those who continued to work on Rx.NET after it ceased to be directly supported by Microsoft, and became a community-based open source project. Many people were involved, and it's not practical to list every contributor here, but I'd like to say a particular thank you to [Bart De Smet](https://github.com/bartdesmet) (again, because he continued to work on the open source Rx long after moving onto other things internally at Microsoft) and also to [Claire Novotny](https://github.com/clairernovotny), [Daniel Weber](https://github.com/danielcweber), [David Karnok](https://github.com/akarnokd), [Brendan Forster](https://github.com/shiftkey), [Ani Betts](https://github.com/anaisbetts) and [Chris Pulman](https://www.linkedin.com/in/chrispulman/). We are also grateful to [Richard Lander](https://www.linkedin.com/in/richardlander/) and the [.NET Foundation](https://dotnetfoundation.org/) for helping us at [endjin](https://endjin.com) become the new stewards of the [Rx.NET project](https://github.com/dotnet/reactive), enabling it to continue to thrive.

If you are interested in more information about the origins of Rx, you might find the [A Little History of Reaqtor](https://reaqtive.net/) ebook illuminating.

The version that this book has been written against is `System.Reactive` version 6.0. The source for this book can be found at [https://github.com/dotnet/reactive/tree/main/Rx.NET/Documentation/IntroToRx](https://github.com/dotnet/reactive/tree/main/Rx.NET/Documentation/IntroToRx). If you find any bugs or other issues in this book, please [create an issue](https://github.com/dotnet/reactive/issues) at https://github.com/dotnet/reactive/. You might find the [Reactive X slack](reactivex.slack.com) to be a useful resource if you start using Rx.NET in earnest.

So, fire up Visual Studio and let's get started.

---