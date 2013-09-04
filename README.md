Reactive Extensions
======================

A Brief Intro
-------------------

The Reactive Extensions (Rx) is a library for composing asynchronous and event-based programs using observable sequences and LINQ-style query operators. Using Rx, developers *__represent__* asynchronous data streams with [Observables](http://msdn.microsoft.com/library/dd990377.aspx), *__query__* asynchronous data streams using [LINQ operators](http://msdn.microsoft.com/en-us/library/hh242983.aspx), and *__parameterize__* the concurrency in the asynchronous data streams using [Schedulers](http://msdn.microsoft.com/en-us/library/hh242963.aspx). Simply put, Rx = Observables + LINQ + Schedulers.

Whether you are authoring a traditional desktop or web-based application, you have to deal with asynchronous and event-based programming from time to time. Desktop applications have I/O operations and computationally expensive tasks that might take a long time to complete and potentially block other active threads. Furthermore, handling exceptions, cancellation, and synchronization is difficult and error-prone.

Using Rx, you can represent multiple asynchronous data streams (that come from diverse sources, e.g., stock quote, tweets, computer events, web service requests, etc.), and subscribe to the event stream using the `IObserver<T>` interface. The `IObservable<T>` interface notifies the subscribed `IObserver<T>` interface whenever an event occurs.

Because observable sequences are data streams, you can query them using standard LINQ query operators implemented by the Observable extension methods. Thus you can filter, project, aggregate, compose and perform time-based operations on multiple events easily by using these standard LINQ operators. In addition, there are a number of other reactive stream specific operators that allow powerful queries to be written.  Cancellation, exceptions, and synchronization are also handled gracefully by using the extension methods provided by Rx.

Rx complements and interoperates smoothly with both synchronous data streams (`IEnumerable<T>`) and single-value asynchronous computations (`Task<T>`) as the following diagram shows:
  	

<table>
   <th></th><th>Single return value</th><th>Mutiple return values</th>
   <tr>
      <td>Pull/Synchronous/Interactive</td>
      <td>T</td>
      <td>IEnumerable<T></td>
   </tr>
   <tr>
      <td>Push/Asycnhrounous/Reactive</td>
      <td>Task<T></td>
      <td>IObservable<T></td>
   </tr>
</table>

Additional documentation, video, tutorials and HOL are available on MSDN.

Flavors of Rx
---------------

* __Rx.NET__: *(this repository)* The Reactive Extensions (Rx) is a library for composing asynchronous and event-based programs using observable sequences and LINQ-style query operators.
* [RxJS](http://rxjs.codeplex.com): The Reactive Extensions for JavaScript (RxJS) is a library for composing asynchronous and event-based programs using observable sequences and LINQ-style query operators in JavaScript which can target both the browser and Node.js.
* [RxCpp](http://rxcpp.codeplex.com): The Reactive Extensions for Native (RxCpp) is a library for composing asynchronous and event-based programs using observable sequences and LINQ-style query operators in both C and C++.
* [Rx.rb](http://rxrb.codeplex.com): A prototype implementation of Reactive Extensions for Ruby (Rx.rb).
* [RxPy](http://rxpy.codeplex.com): The Reactive Extensions for Python 3 (Rx.Py) is a set of libraries to compose asynchronous and event-based programs using observable collections and LINQ-style query operators in Python 3. 


Interactive Extensions
-----------------------
* __Ix__: *(included in this repository)* The Interactive Extensions (Ix) is a .NET library which extends LINQ to Objects to provide many of the operators available in Rx but targeted for IEnumerable<T>.
* [IxJS](http://rxjs.codeplex.com): An implementation of LINQ to Objects and the Interactive Extensions (Ix) in JavaScript.
* [IxCpp]([IxJS](http://rxjs.codeplex.com): An implantation of LINQ for Native Developers in C++

Applications
-------------
* [Tx] (http://tx.codeplex.com): a set of code samples showing how to use LINQ to events, such as real-time standing queries and queries on past history from trace and log files, which targets ETW, Windows Event Logs and SQL Server Extended Events.
* [LINQ2Charts] (http://linq2charts.codeplex.com): an example for Rx bindings.  Similar to existing APIs like LINQ to XML, it allows developers to use LINQ to create/change/update charts in an easy way and avoid having to deal with XML or other underneath data structures. We would love to see more Rx bindings like this one.

Contributing Code
------------------

Before submitting a feature or substantial code contribution please  discuss it with the team and ensure it follows the product roadmap. Note that all code submissions will be rigorously reviewed and tested by the Rx Team, and only those that meet an extremely high bar for both quality and design/roadmap appropriateness will be merged into the source. 

You will need to submit a  Contributor License Agreement form before submitting your pull request. This needs to only be done once for any Microsoft OSS project. Download the Contributor License Agreement (CLA). Please fill in, sign, scan and email it to <msopentech-cla@microsoft.com>. 