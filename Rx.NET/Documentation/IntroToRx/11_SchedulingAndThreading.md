# Scheduling and Threading

Rx is primarily a system for working with _data in motion_ asynchronously. If we are dealing with multiple information sources, they may well generate data concurrently. We may want some degree of parallelism when processing data to achieve our scalability targets. We will need control over these aspects of our system.

So far, we have managed to avoid any explicit usage of threading or concurrency. We have seen some methods that must deal with timing to perform their jobs. (For example, `Buffer`, `Delay`, and `Sample` must arrange for work to happen on a particular schedule.) However, we have relied on the default behaviour, and although the defaults often do what we want, we sometimes need to exercise more control. This chapter will look at Rx's scheduling system, which offers an elegant way to manage these concerns.

## Rx, Threads and Concurrency

Rx does not impose constraints on which threads we use. An `IObservable<T>` is free to invoke its subscribers' `OnNext/Completed/Error` methods on any thread, perhaps a different thread for each call. Despite this free-for-all, there is one aspect of Rx that prevents chaos: observable sources must obey the [Fundamental Rules of Rx Sequences](02_KeyTypes.md#the-fundamental-rules-of-rx-sequences) under all circumstances.

When we first explored these rules, we focused on how they determine the ordering of calls into any single observer. There can be any number of calls to `OnNext`, but once either `OnError` or `OnCompleted` have been invoked, there must be no further calls. But now that we're looking at concurrency, a different aspect of these rules becomes more important: for any single subscription, an observable source must not make concurrent calls into that subscription's observer. So if a source calls `OnNext`, it must wait until that call returns before either calling `OnNext` again, or calling `OnError` or `OnComplete`.

The upshot for observers is that as long as your observer is involved in just one subscription, it will only ever be asked to deal with one thing at a time. It doesn't matter if the source to which it is subscribed is a long and complex processing chain involving many different operators. Even if you build that source by combining multiple inputs (e.g., using [`Merge`](09_CombiningSequences.md#merge)), the fundamental rules require that if you called `Subscribe` just once on a single `IObservable<T>`, that source is never allowed to make multiple concurrent calls into your `IObserver<T>` methods.

So although each call might come in on a different thread, the calls are strictly sequential (unless a single observer is involved in multiple subscriptions).

Rx operators that receive incoming notifications as well as producing them will notify their observers on whatever thread the incoming notification happened to arrive on. Suppose you have a sequence of operators like this:

```csharp
source
    .Where(x => x.MessageType == 3)
    .Buffer(10)
    .Take(20)
    .Subscribe(x => Console.WriteLine(x));
```

When that call to `Subscribe` happens, we end up with a chain of observers. The Rx-supplied observer that will invoke our callback was passed to the observable returned by `Take`, which will in turn create an observer that subscribed to the observable returned by `Buffer`, which will in turn create an observer subscribed to the `Where` observable, which will have created yet another observer which is subscribed to `source`.

So when `source` decides to produce an item, it will invoke the `Where` operator's observer's `OnNext`. That will invoke the predicate, and if the `MessageType` is indeed 3, the `Where` observer will call `OnNext` on the `Buffer`'s observer, and it will do this on the same thread. The `Where` observer's `OnNext` isn't going to return until the `Buffer` observer's `OnNext` returns. Now if the `Buffer` observer determines that it has completely filled a buffer (e.g., it just received its 10th item), then it is also not going to return yet—it's going to invoke the `Take` observer's `OnNext`, and as long as `Take` hasn't already received 20 buffers, it's going to call `OnNext` on the Rx-supplied observer that will invoke our callback.

So for the source notifications that make it all the way through to that `Console.WriteLine` in the callback passed to subscribe, we end up with a lot of nested calls on the stack:

```
`source` calls:
  `Where` observer, which calls:
    `Buffer` observer, which calls:
      `Take` observer, which calls:
        `Subscribe` observer, which calls our lambda
```

This is all happening on one thread. Most Rx operators don't have any one particular thread that they call home. They just do their work on whatever thread the call comes in on. This makes Rx pretty efficient. Passing data from one operator to the next merely involves a method call, and those are pretty fast. (In fact, there are typically a few more layers. Rx tends to add a few wrappers to handle errors and early unsubscription. So the call stack will look a bit more complex than what I've just shown. But it's still typically all just method calls.)

You will sometimes hear Rx described as having a _free threaded_ model. All that means is that operators don't generally care what thread they use. As we will see, there are exceptions, but this direct calling by one operator of the next is the norm.

An upshot of this is that it's typically the original source that determines which thread is used. This next example illustrates this by creating a subject, then calling `OnNext` on various threads and reporting the thread id.

```csharp
Console.WriteLine($"Main thread: {Environment.CurrentManagedThreadId}");
var subject = new Subject<string>();

subject.Subscribe(
    m => Console.WriteLine($"Received {m} on thread: {Environment.CurrentManagedThreadId}"));

object sync = new();
ParameterizedThreadStart notify = arg =>
{
    string message = arg?.ToString() ?? "null";
    Console.WriteLine(
        $"OnNext({message}) on thread: {Environment.CurrentManagedThreadId}");
    lock (sync)
    {
        subject.OnNext(message);
    }
};

notify("Main");
new Thread(notify).Start("First worker thread");
new Thread(notify).Start("Second worker thread");
```

Output:

```
Main thread: 1
OnNext(Main) on thread: 1
Received Main on thread: 1
OnNext(First worker thread) on thread: 10
Received First worker thread on thread: 10
OnNext(Second worker thread) on thread: 11
Received Second worker thread on thread: 11
```

In each case, the handler passed to `Subscribe` was called back on the same thread that made the call to `subject.OnNext`. This is straightforward and efficient. However, things are not always this simple.

## Timed invocation

Some notifications will not be the immediate result of a source providing an item. For example, Rx offers a [`Delay`](12_Timing.md#delay) operator, which time shifts the delivery of items. This next example is based on the preceding one, with the main difference being that we no longer subscribe directly to the source. We go via `Delay`:

```csharp
Console.WriteLine($"Main thread: {Environment.CurrentManagedThreadId}");
var subject = new Subject<string>();

subject
    .Delay(TimeSpan.FromSeconds(0.25))
    .Subscribe(
    m => Console.WriteLine($"Received {m} on thread: {Environment.CurrentManagedThreadId}"));

object sync = new();
ParameterizedThreadStart notify = arg =>
{
    string message = arg?.ToString() ?? "null";
    Console.WriteLine(
        $"OnNext({message}) on thread: {Environment.CurrentManagedThreadId}");
    lock (sync)
    {
        subject.OnNext(message);
    }
};

notify("Main 1");
Thread.Sleep(TimeSpan.FromSeconds(0.1));
notify("Main 2");
Thread.Sleep(TimeSpan.FromSeconds(0.3));
notify("Main 3");
new Thread(notify).Start("First worker thread");
Thread.Sleep(TimeSpan.FromSeconds(0.1));
new Thread(notify).Start("Second worker thread");

Thread.Sleep(TimeSpan.FromSeconds(2));
```

This also waits for a while between sending source items, so we can see the effect of `Delay`. Here's the output:

```
Main thread: 1
OnNext(Main 1) on thread: 1
OnNext(Main 2) on thread: 1
Received Main 1 on thread: 12
Received Main 2 on thread: 12
OnNext(Main 3) on thread: 1
OnNext(First worker thread) on thread: 13
OnNext(Second worker thread) on thread: 14
Received Main 3 on thread: 12
Received First worker thread on thread: 12
Received Second worker thread on thread: 12
```

Notice that in this case every `Received` message is on thread id 12, which is different from any of the three threads on which the notifications were raised.

This shouldn't be entirely surprising. The only way Rx could have used the original thread here would be for `Delay` to block the thread for the specified time (a quarter of a second here) before forwarding the call. This would be unacceptable for most scenarios, so instead, the `Delay` operator arranges for a callback to occur after a suitable delay. As you can see from the output, these all seems to happen on one particular thread. No matter which thread calls `OnNext`, the delayed notification arrives on thread id 12. But this is not a thread created by the `Delay` operator. This is happening because `Delay` is using a _scheduler_.

## Schedulers

Schedulers do three things:

* determining the context in which to execute work (e.g., a certain thread)
* deciding when to execute work (e.g., immediately, or deferred)
* keeping track of time

Here's a simple example to explore the first two of those:

```csharp
Console.WriteLine($"Main thread: {Environment.CurrentManagedThreadId}");

Observable
    .Range(1, 5)
    .Subscribe(m => 
      Console.WriteLine(
        $"Received {m} on thread: {Environment.CurrentManagedThreadId}"));

Console.WriteLine("Subscribe returned");
Console.ReadLine();
```

It might not be obvious that this has anything to do with scheduling, but in fact, `Range` always uses a scheduler to do its work. We've just let it use its default scheduler. Here's the output:

```
Main thread: 1
Received 1 on thread: 1
Received 2 on thread: 1
Received 3 on thread: 1
Received 4 on thread: 1
Received 5 on thread: 1
Subscribe returned
```

Looking at the first two items in our list of what schedulers do, we can see that the context in which this has executed the work is the thread on which I called `Subscribe`. And as for when it has decided to execute the work, it has decided to do it all before `Subscribe` returns. So you might think that `Range` immediately produces all of the items we've asked for and then returns. However, it's not quite as simple as that. Let's look at what happens if we have multiple `Range` instances running simultaneously. This introduces an extra operator: a `SelectMany` that calls `Range` again:

```csharp
Observable
    .Range(1, 5)
    .SelectMany(i => Observable.Range(i * 10, 5))
    .Subscribe(m => 
      Console.WriteLine(
        $"Received {m} on thread: {Environment.CurrentManagedThreadId}"));
```

The output shows that `Range` doesn't in fact necessarily produce all of its items immediately:

```
Received 10 on thread: 1
Received 11 on thread: 1
Received 20 on thread: 1
Received 12 on thread: 1
Received 21 on thread: 1
Received 30 on thread: 1
Received 13 on thread: 1
Received 22 on thread: 1
Received 31 on thread: 1
Received 40 on thread: 1
Received 14 on thread: 1
Received 23 on thread: 1
Received 32 on thread: 1
Received 41 on thread: 1
Received 50 on thread: 1
Received 24 on thread: 1
Received 33 on thread: 1
Received 42 on thread: 1
Received 51 on thread: 1
Received 34 on thread: 1
Received 43 on thread: 1
Received 52 on thread: 1
Received 44 on thread: 1
Received 53 on thread: 1
Received 54 on thread: 1
Subscribe returned
```

The first nested `Range` produces by the `SelectMany` callback produces a couple of values (10 and 11) but then the second one manages to get its first value out (20) before the first one produces its third (12). You can see there's some interleaving of progress here. So although the context in which work is executed continues to be the thread on which we invoked `Subscribe`, the second choice the scheduler has to make—when to execute the work—is more subtle than it first seems. This tells us that `Range` is not as simple as this naive implementation:

```csharp
public static IObservable<int> NaiveRange(int start, int count)
{
    return System.Reactive.Linq.Observable.Create<int>(obs =>
    {
        for (int i = 0; i < count; i++)
        {
            obs.OnNext(start + i);
        }

        return Disposable.Empty;
    });
}
```

If `Range` worked like that, this code would produce all of the items from the first range returned by the `SelectMany` callback before moving on to the next. In fact, Rx does provide a scheduler that would give us that behaviour if that's what we want. This example passes `ImmediateScheduler.Instance` to the nested `Observable.Range` call:

```csharp
Observable
    .Range(1, 5)
    .SelectMany(i => Observable.Range(i * 10, 5, ImmediateScheduler.Instance))
    .Subscribe(
    m => Console.WriteLine($"Received {m} on thread: {Environment.CurrentManagedThreadId}"));
```

Here's the outcome:

```
Received 10 on thread: 1
Received 11 on thread: 1
Received 12 on thread: 1
Received 13 on thread: 1
Received 14 on thread: 1
Received 20 on thread: 1
Received 21 on thread: 1
Received 22 on thread: 1
Received 23 on thread: 1
Received 24 on thread: 1
Received 30 on thread: 1
Received 31 on thread: 1
Received 32 on thread: 1
Received 33 on thread: 1
Received 34 on thread: 1
Received 40 on thread: 1
Received 41 on thread: 1
Received 42 on thread: 1
Received 43 on thread: 1
Received 44 on thread: 1
Received 50 on thread: 1
Received 51 on thread: 1
Received 52 on thread: 1
Received 53 on thread: 1
Received 54 on thread: 1
Subscribe returned
```

By specifying `ImmediateScheduler.Instance` in the innermost call to `Observable.Range` we've asked for a particular policy: this invokes all work on the caller's thread, and it always does so immediately. There are a couple of reasons this is not `Range`'s default. (Its default is `Scheduler.CurrentThread`, which always returns an instance of `CurrentThreadScheduler`.) First, `ImmediateScheduler.Instance` can end up causing fairly deep call stacks. Most of the other schedulers maintain work queues, so if one operator decides it has new work to do while another is in the middle of doing something (e.g., a nested `Range` operator decides to start emitting its values), instead of starting that work immediately (which will involve invoking the method that will do the work) that work can be put on a queue instead, enabling the work already in progress to finish before starting on the next thing. Using the immediate scheduler everywhere can cause stack overflows when queries become complex. The second reason `Range` does not use the immediate scheduler by default is so that when multiple observables are all active at once, they can all make some progress—`Range` produces all of its items as quickly as it can, so it could end up starving other operators of CPU time if it didn't use a scheduler that enabled operators to take it in turns.

Notice that the `Subscribe returned` message appears last in both examples. So although the `CurrentThreadScheduler` isn't quite as eager as the immediate scheduler, it still won't return to its caller until it has completed all outstanding work. It maintains a work queue, enabling slightly more fairness, and avoiding stack overflows, but as soon as anything asks the `CurrentThreadScheduler` to do something, it won't return until it has drained its queue.

Not all schedulers have this characteristic. Here's a variation on the earlier example in which we have just a single call to `Range`, without any nested observables. This time I'm asking it to use the `TaskPoolScheduler`.

```csharp
Observable
    .Range(1, 5, TaskPoolScheduler.Default)
    .Subscribe(
    m => Console.WriteLine($"Received {m} on thread: {Environment.CurrentManagedThreadId}"));
```

This makes a different decision about the context in which to run work, compared to the immediate and current thread schedulers, as we can see from its output:

```
Main thread: 1
Subscribe returned
Received 1 on thread: 12
Received 2 on thread: 12
Received 3 on thread: 12
Received 4 on thread: 12
Received 5 on thread: 12
```

Notice that the notifications all happened on a different thread (with id 12) than the thread on which we invoked `Subscribe` (id 1). That's because the `TaskPoolScheduler`'s defining feature is that it invokes all work through the Task Parallel Library's (TPL) task pool. That's why we see a different thread id: the task pool doesn't own our application's main thread. In this case, it hasn't seen any need to spin up multiple threads. That's reasonable, there's just a single source here providing item one at a time. It's good that we didn't get more threads in this case—the thread pool is at its most efficient when a single thread processes work items sequentially, because it avoids context switching overheads, and since there's no actual scope for concurrent work here, we would gain nothing if it had created multiple threads in this case.

There's one other very significant difference with this scheduler: notice that the call to `Subscribe` returned before _any_ of the notifications reached our observer. That's because this is the first scheduler we've looked at that will introduce real parallelism. The `ImmediateScheduler` and `CurrentThreadScheduler` will never spin up new threads by themselves, no matter how much the operators executing might want to perform concurrent operations. And although the `TaskPoolScheduler` determined that there's no need for it to create multiple threads, the one thread it did create is a different thread from the application's main thread, meaning that the main thread can continue to run in parallel with this subscription. Since `TaskPoolScheduler` isn't going to do any work on the thread that initiated the work, it can return as soon as it has queued the work up, enabling the `Subscribe` method to return immediately.

What if we use the `TaskPoolScheduler` in the example with nested observables? This uses it just on the inner call to `Range`, so the outer one will still use the default `CurrentThreadScheduler`:

```csharp
Observable
    .Range(1, 5)
    .SelectMany(i => Observable.Range(i * 10, 5, TaskPoolScheduler.Default))
    .Subscribe(
    m => Console.WriteLine($"Received {m} on thread: {Environment.CurrentManagedThreadId}"));
```

Now we can see a few more threads getting involved:

```
Received 10 on thread: 13
Received 11 on thread: 13
Received 12 on thread: 13
Received 13 on thread: 13
Received 40 on thread: 16
Received 41 on thread: 16
Received 42 on thread: 16
Received 43 on thread: 16
Received 44 on thread: 16
Received 50 on thread: 17
Received 51 on thread: 17
Received 52 on thread: 17
Received 53 on thread: 17
Received 54 on thread: 17
Subscribe returned
Received 14 on thread: 13
Received 20 on thread: 14
Received 21 on thread: 14
Received 22 on thread: 14
Received 23 on thread: 14
Received 24 on thread: 14
Received 30 on thread: 15
Received 31 on thread: 15
Received 32 on thread: 15
Received 33 on thread: 15
Received 34 on thread: 15
```

Since we have only a single observer in this example, the rules of Rx require it to be given items one at a time, so in practice there wasn't really any scope for parallelism here, but the more complex structure would have resulted in more work items initially going into the scheduler's queue than in the preceding example, which is probably why the work got picked up by more than one thread this time. In practice most of these threads would have spent most of their time blocked in the code inside `SelectMany` that ensures that it delivers one item at a time to its target observer. It's perhaps a little surprising that the items are not more scrambled. The subranges themselves seem to have emerged in a random order, but it has almost produced the items sequentially within each subrange (with item 14 being the one exception to that). This is a quirk relating to the way in which `Range` interacts with the `TaskPoolScheduler`.

I've not yet talked about the scheduler's third job: keeping track of time. This doesn't arise with `Range` because it attempts to produce all of its items as quickly as it can. But for the `Delay` operator I showed in the [Timed Invocation](#timed-invocation) section, timing is obviously a critical element. In fact this would be a good point to show the API that schedulers offer:

```csharp
public interface IScheduler
{
    DateTimeOffset Now { get; }
    
    IDisposable Schedule<TState>(TState state, 
                                 Func<IScheduler, TState, IDisposable> action);
    
    IDisposable Schedule<TState>(TState state, 
                                 TimeSpan dueTime, 
                                 Func<IScheduler, TState, IDisposable> action);
    
    IDisposable Schedule<TState>(TState state, 
                                 DateTimeOffset dueTime, 
                                 Func<IScheduler, TState, IDisposable> action);
}
```

You can see that all but one of these is concerned with timing. Only the first `Schedule` overload is not, and operators call that when they want to schedule work to run as soon as the scheduler will allow. That's the overload used by `Range`. (Strictly speaking, `Range` interrogates the scheduler to find out whether it supports long-running operations, in which an operator can take temporary control of a thread for an extended period. It prefers to use that when it can because it tends to be more efficient than submitting work to the scheduler for every single item it wishes to produce. The `TaskPoolScheduler` does support long running operations, which explains the slightly surprising output we saw earlier, but the `CurrentThreadScheduler`, `Range`'s default choice, does not. So by default, `Range` will invoke that first `Schedule` overload once for each item it wishes to produce.)

`Delay` uses the second overload. The exact implementation is quite complex (mainly because of how it catches up efficiently when a busy source causes it to fall behind) but in essence, each time a new item arrives into the `Delay` operator, it schedules a work item to run after the configured delay, so that it can supply that item to its subscriber with the expected time shift.

Schedulers have to be responsible for managing time, because .NET has several different timer mechanisms, and the choice of timer is often determined by the context in which you want to handle a timer callback. Since schedulers determine the context in which work runs, that means they must also choose the timer type. For example, UI frameworks typically provide timers that invoke their callbacks in a context suitable for making updates to the user interface. Rx provides some UI-framework-specific schedulers that use these timers, but these would be inappropriate choices for other scenarios. So each scheduler uses a timer suitable for the context in which it is going to run work items.

There's a useful upshot of this: because `IScheduler` provides an abstraction for timing-related details, it is possible to virtualize time. This is very useful for testing. If you look at the extensive test suite in the [Rx repository](https://github.com/dotnet/reactive) you will find that there are many tests that verify timing-related behaviour. If these ran in real-time, the test suite would take far too long to run, and would also be likely to produce the odd spurious failure, because background tasks running on the same machine as the tests will occasionally change the speed of execution in a way that might confuse the test. Instead, these tests use a specialized scheduler that provides complete control over the passage of time. (For more information, see the [ Test Schedulers section later](#test-schedulers) and there's also a whole [testing chapter](16_TestingRx.md) coming up.)

Notice that all three `IScheduler.Schedule` methods require a callback. A scheduler will invoke this at the time and in the context that it chooses. A scheduler callback takes another `IScheduler` as its first argument. This is used in scenarios where repetitive invocation is required, as we'll see later. 

Rx supplies several schedulers. The following sections describe the most widely used ones.

### ImmediateScheduler

`ImmediateScheduler` is the simplest scheduler Rx offers. As you saw in the preceding sections, whenever it is asked to schedule some work, it just runs it immediately. It does this inside its `IScheduler.Schedule` method.

This is a very simple strategy, and it makes `ImmediateScheduler` very efficient. For this reason, many operators default to using `ImmediateScheduler`. However, it can be problematic with operators that instantly produce multiple items, especially when the number of items might be large. For example, Rx defines the [`ToObservable` extension method for `IEnumerable<T>`](03_CreatingObservableSequences.md#from-ienumerablet). When you subscribe to an `IObservable<T>` returned by this, it will start iterating over the collection immediately, and if you were to tell it to use the `ImmediateScheduler`, `Subscribe` would not return until it reached the end of the collection. That would obviously be a problem for an infinite sequence, and it's why operators of this kind do not use `ImmediateScheduler` by default.

The `ImmediateScheduler` also has potentially surprising behaviour when you invoke the `Schedule` overload that takes a `TimeSpan`. This asks the scheduler to run some work after the specified length of time. The way it achieves this is to call `Thread.Sleep`. With most of Rx's schedulers, this overload will arrange for some sort of timer mechanism to run the code later, enabling the current thread to get on with its business, but `ImmediateScheduler` is true to its name here, in that it refuses to engage in such deferred execution. It just blocks the current thread until it is time to do the work. This means that time-based observables like those returned by `Interval` would work if you specified this scheduler, but at the cost of preventing the thread from doing anything else.

The `Schedule` overload that takes a `DateTime` is slightly different. If you specify a time less than 10 seconds into the future, it will block the calling thread like it does when you use `TimeSpan`. But if you pass a `DateTime` that is further into the future, it gives up on immediate execution, and falls back to using a timer.

### CurrentThreadScheduler

The `CurrentThreadScheduler` is very similar to the `ImmediateScheduler`. The difference is how it handles requests to schedule work when an existing work item is already being handled on the current thread. This can happen if you chain together multiple operators that use schedulers to do their work.

To understand what happens, it's helpful to know how sources that produce multiple items in quick succession, such as the [`ToObservable` extension method for `IEnumerable<T>`](03_CreatingObservableSequences.md#from-ienumerablet) or [`Observable.Range`](03_CreatingObservableSequences.md#observablerange), use schedulers. These kinds of operators do not use normal `for` or `foreach` loops. They typically schedule a new work item for each iteration (unless the scheduler happens to make special provisions for long-running work). Whereas the `ImmediateScheduler` will run such work immediately, the `CurrentThreadScheduler` checks to see if it is already processing a work item. We saw that with this example from earlier:

```csharp
Observable
    .Range(1, 5)
    .SelectMany(i => Observable.Range(i * 10, 5))
    .Subscribe(
        m => Console.WriteLine($"Received {m} on thread: {Environment.CurrentManagedThreadId}"));
```

Let's follow exactly what happens here. First, assume that this code is just running normally and not in any unusual context—perhaps inside the `Main` entry point of a program. When this code calls `Subscribe` on the `IObservable<int>` returned by `SelectMany`, that will in turn will call `Subscribe` on the `IObservable<int>` returned by the first `Observable.Range`, which will in turn schedule a work item for the generation of the first value in the range (`1`).

Since we didn't pass a scheduler explicitly to `Range`, it will use its default choice, the `CurrentThreadScheduler`, and that will ask itself "Am I already in the middle of handling some work item on this thread?" In this case the answer will be "no," so it will run the work item immediately (before returning from the `Schedule` call made by the `Range` operator). The `Range` operator will then produce its first value, calling `OnNext` on the `IObserver<int>` that the `SelectMany` operator provided when it subscribed to the range.

The `SelectMany` operator's `OnNext` method will now invoke its lambda, passing in the argument supplied (the value `1` from the `Range` operator). You can see from the example above that this lambda calls `Observable.Range` again, returning a new `IObservable<int>`. `SelectMany` will immediately subscribe to this (before returning from its `OnNext`). This is the second time this code has ended up calling `Subscribe` on an `IObservable<int>` returned by a `Range` (but it's a different instance than the last time), and `Range` will once again default to using the `CurrentThreadScheduler`, and will once again schedule a work item to perform the first iteration.

So once again,the `CurrentThreadScheduler` will ask itself "Am I already in the middle of handling some work item on this thread?" But this time, the answer will be yes. And this is where the behaviour is different than `ImmediateScheduler`. The `CurrentThreadScheduler` maintains a queue of work for each thread that it gets used on, and in this case it just adds the newly scheduled work to the queue, and returns back to the `SelectMany` operators `OnNext`.

`SelectMany` has now completed its handling of this item (the value `1`) from the first `Range`, so its `OnNext` returns. At this point, this outer `Range` operator schedules another work item. Again, the `CurrentThreadScheduler` will detect that it is currently running a work item, so it just adds this to the queue.

Having scheduled the work item that is going to generate its second value (`2`), the `Range` operator returns. Remember, the code in the `Range` operator that was running at this point was the callback for the first scheduled work item, so it's returning to the `CurrentThreadScheduler`—we are back inside its `Schedule` method (which was invoked by the range operator's `Subscribe` method).

At this point, the `CurrentThreadScheduler` does not return from `Schedule` because it checks its work queue, and will see that there are now two items in the queue. (There's the work item that the nested `Range` observable scheduled to generate its first value, and there's also the work item that the top-level `Range` observable just scheduled to generate its second value.) The `CurrentThreadScheduler` will now execute the first of these: the nested `Range` operator now gets to generate its first value (which will be `10`), so it calls `OnNext` on the observer supplied by `SelectMany`, which will then call its observer, which was supplied thanks to the top-level call to `Subscribe` in the example. And that observer will just call the lambda we passed to `Subscribe`, causing our `Console.WriteLine` to run. After that returns, the nested `Range` operator will schedule another work item to generate its second item. Again, the `CurrentThreadScheduler` will realise that it's already in the middle of handling a work item on this thread, so it just puts it in the queue and then returns immediately from `Schedule`. The nested `Range` operator is now done for this iteration so it returns back to the scheduler. The scheduler will now pick up the next item in the queue, which in this case is the work item added by the top-level `Range` to produce the second item.

And so it continues. This queuing of work items when work is already in progress is what enables multiple observable sources to make progress in parallel.

By contrast, the `ImmediateScheduler` runs new work items immediately, which is why we don't see this parallel progress.

(To be strictly accurate, there are certain scenarios in which `ImmediateScheduler` can't run work immediately. In these iterative scenarios, it actually supplies a slightly different scheduler that the operators use to schedule all work after the first item, and this checks whether it's being asked to process multiple work items simultaneously. If it is, it falls back to a queuing strategy similar to `CurrentThreadScheduler`, except it's a queue local to the initial work item, instead of a per-thread queue. This prevents problems due to multithreading, and it also avoids stack overflows that would otherwise occur when an iterative operator schedules a new work item inside the handler for the current work item. Since the queue is not shared across all work in the thread, this still has the effect of ensuring that any nested work queued up by a work item completes before the call to `Schedule` returns. So even when this queueing kicks in, we typically don't see interleaving of work from separate sources like we do with `CurrentThreadScheduler`. For example, if we told the nested `Range` to use `ImmediateScheduler`, this queueing behaviour would kick in as `Range` starts to iterate, but because the queue is local to initial work item executed by that nested `Range`, it will end up producing all of the nested `Range` items before returning.)

### DefaultScheduler

The `DefaultScheduler` is intended for work that may need to be spread out over time, or where you are likely to want concurrent execution. These features mean that this can't guarantee to run work on any particular thread, and in practice it schedules work via the CLR's thread pool. This is the default scheduler for all of Rx's time-based operators, and also for the `Observable.ToAsync` operator that can wrap a .NET method as an `IObservable<T>`.

Although this scheduler is useful if you would prefer work not to happen on your current thread—perhaps you're writing an application with a user interface and you prefer to avoid doing too much work on the thread responsible for updating the UI and responding to user input—the fact that it can end up running work on any thread may make like complicated. What if you want all the work to happen on one thread, just not the thread you're on now? There's another scheduler for that.

### EventLoopScheduler

The `EventLoopScheduler` provides one-at-a-time scheduling, queuing up newly scheduled work items. This is similar to how the `CurrentThreadScheduler` operates if you use it from just one thread. The difference is that `EventLoopScheduler` creates a dedicated thread for this work instead of using whatever thread you happen to schedule the work from.

Unlike the schedulers we've examined so far, there is no static property for obtaining an `EventLoopScheduler`. That's because each one has its own thread, so you need to create one explicitly. It offers two constructors:

```csharp
public EventLoopScheduler()
public EventLoopScheduler(Func<ThreadStart, Thread> threadFactory)
```

The first creates a thread for you. The second lets you control the thread creation process. It invokes the callback you supply, and it will pass this its own callback that you are required to run on the newly created thread.

The `EventLoopScheduler` implements `IDisposable`, and calling Dispose will allow the thread to terminate. This can work nicely with the `Observable.Using` method. The following example shows how to use an `EventLoopScheduler` to iterate over all contents of an `IEnumerable<T>` on a dedicated thread, ensuring that the thread exits once we have finished:

```csharp
IEnumerable<int> xs = GetNumbers();
Observable
    .Using(
        () => new EventLoopScheduler(),
        scheduler => xs.ToObservable(scheduler))
    .Subscribe(...);
```

### NewThreadScheduler

The `NewThreadScheduler` creates a new thread to execute every work item it is given. This is unlikely to make sense in most scenarios. However, it might be useful in cases where you want to execute some long running work, and represent its completion through an `IObservable<T>`. The `Observable.ToAsync` does exactly this, and will normally use the `DefaultScheduler`, meaning it will run the work on a thread pool thread. But if the work is likely to take more than second or two, the thread pool may not be a good choice, because it is optimized for short execution times, and its heuristics for managing the size of the thread pool are not designed with long-running operations in mind. The `NewThreadScheduler` may be a better choice in this case.

Although each call to `Schedule` creates a new thread, the `NewThreadScheduler` passes a different scheduler into work item callbacks, meaning that anything that attempts to perform iterative work will not create a new thread for every iteration. For example, if you use `NewThreadScheduler` with `Observable.Range`, you will get a new thread each time you subscribe to the resulting `IObservable<int>`, but you won't get a new thread for each item, even though `Range` does schedule a new work item for each value it produces. It schedules these per-value work items through the nested scheduler supplied to the work item callback, and the nested scheduler that `NewThreadScheduler` supplies in these cases invokes all such nested work items on the same thread.

### SynchronizationContextScheduler

This invokes all work through a [`SynchronizationContext`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.synchronizationcontext). This is useful in user interface scenarios. Most .NET client-side user interface frameworks make a `SynchronizationContext` available that can be used to invoke callbacks in a context suitable for making changes to the UI. (Typically this involves invoking them on the correct thread, but individual implementations can decide what constitutes the appropriate context.)

### TaskPoolScheduler

Invokes all work via the thread pool using [TPL tasks](https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-parallel-library-tpl). The TPL was introduced many years after the CLR thread pool, and is now the recommended way to launch work via the thread pool. At the time the TPL was added, the thread pool would use a slightly different algorithm when you scheduled work through tasks than it would use if you relied on the older thread pool APIs. This newer algorithm enabled it to be more efficient in some scenarios. The documentation is now rather vague about this, so it's not clear whether these differences still exist on modern .NET, but tasks continue to be the recommended mechanism for using the thread pool. Rx's DefaultScheduler uses the older CLR thread pool APIs for backwards compatibility reasons. In performance critical code you could try using the `TaskPoolScheduler` instead in cases where a lot of work is being run on thread pool threads to see if it offers any performance benefits for your workload.

### ThreadPoolScheduler

Invokes all work through the thread pool using the old pre-[TPL](https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-parallel-library-tpl) API. This type is a historical artifact, dating back to when not all platforms offered the same kind of thread pool. In almost all cases, if you want the behaviour for which this type was designed, you should use the `DefaultScheduler` (although [`TaskPoolScheduler`](#taskpoolscheduler) offers a different behaviour that might be). The only scenario in which using `ThreadPoolScheduler` makes any difference is when writing UWP applications. The UWP target of `System.Reactive` v6.0 provides a different implementation of this class than you get for all other targets. It uses `Windows.System.Threading.ThreadPool` whereas all other targets use `System.Threading.ThreadPool`. The UWP version provides properties letting you configure some features specific to the UWP thread pool.

In practice it's best to avoid this class in new code. The only reason the UWP target had a different implementation was that UWP used not to provide `System.Threading.ThreadPool`. But that changed when UWP added support for .NET Standard 2.0 in Windows version 10.0.19041. There is no longer any good reason for there to be a UWP-specific `ThreadPoolScheduler`, and it's a source of confusion that this type is quite different in the UWP target but it has to remain for backwards compatibility purposes. (It may well be deprecated because Rx 7 will be addressing some problems arising from the fact that the `System.Reactive` component currently has direct dependencies on UI frameworks.) If you use the `DefaultScheduler` you will be using the `System.Threading.ThreadPool` no matter which platform you are running on.

### UI Framework Schedulers: ControlScheduler, DispatcherScheduler and CoreDispatcherScheduler

Although the `SynchronizationContextScheduler` will work for all widely used client-side UI frameworks in .NET, Rx offers more specialized schedulers. `ControlScheduler` is for Windows Forms applications, `DispatcherScheduler` for WPF, and `CoreDispatcherScheduler` for UWP.

These more specialized types offer two benefits. First, you don't necessarily have to be on the target UI thread to obtain an instance of these schedulers. Whereas with `SynchronizationContextScheduler` the only way you can generally obtain the `SynchronizationContext` this requires is by retrieving `SynchronizationContext.Current` while running on the UI thread. But these other UI-framework-specific schedulers can be passed a suitable `Control`, `Dispatcher` or `CoreDispatcher`, which it's possible to obtain from a non-UI thread. Second, `DispatcherScheduler` and `CoreDispatcherScheduler` provide a way to use the prioritisation mechanism supported by the `Dispatcher` and `CoreDispatcher` types.

### Test Schedulers

The Rx libraries define several schedulers that virtualize time, including `HistoricalScheduler`, `TestScheduler`, `VirtualTimeScheduler`, and `VirtualTimeSchedulerBase`. We will look at this sort of scheduler in the [Testing chapter](16_TestingRx.md).

## SubscribeOn and ObserveOn

So far, I've talked about why some Rx sources need access to schedulers. This is necessary for timing-related behaviour, and also for sources that produce items as quickly as possible. But remember, schedulers control three things:

- determining the context in which to execute work (e.g., a certain thread)
- deciding when to execute work (e.g., immediately, or deferred)
- keeping track of time

The discussion so far as mostly focused on the 2nd and 3rd features. When it comes to our own application code, we are most likely to use schedulers to control that first aspect. Rx defines two extension methods to `IObservable<T>` for this: `SubscribeOn` and `ObserveOn`. Both methods take an `IScheduler` and return an `IObservable<T>` so you can chain more operators downstream of these.

These methods do what their names suggest. If you use `SubscribeOn`, then when you call `Subscribe` on the resulting `IObservable<T>` it arranges to call the original `IObservable<T>`'s `Subscribe` method via the specified scheduler. Here's an example:

```csharp
Console.WriteLine($"[T:{Environment.CurrentManagedThreadId}] Main thread");

Observable
    .Interval(TimeSpan.FromSeconds(1))
    .SubscribeOn(new EventLoopScheduler((start) =>
    {
        Thread t = new(start) { IsBackground = false };
        Console.WriteLine($"[T:{t.ManagedThreadId}] Created thread for EventLoopScheduler");
        return t;
    }))
    .Subscribe(tick => 
          Console.WriteLine(
            $"[T:{Environment.CurrentManagedThreadId}] {DateTime.Now}: Tick {tick}"));

Console.WriteLine($"[T:{Environment.CurrentManagedThreadId}] {DateTime.Now}: Main thread exiting");
```

This calls `Observable.Interval` (which uses `DefaultScheduler` by default), but instead of subscribing directly to this, it first takes the `IObservable<T>` returned by `Interval` and invokes `SubscribeOn`. I've used an `EventLoopScheduler`, and I've passed it a factory callback for the thread that it will use to ensure that it is a non-background thread. (By default `EventLoopScheduler` creates itself a background thread, meaning that the thread won't force the process to stay alive. Normally that's what you'd want but I'm changing that in this example to show what's happening.)

When I call `Subscribe` on the `IObservable<long>` returned by `SubscribeOn`, it calls `Schedule` on the `EventLoopScheduler` that I supplied, and in the callback for that work item, it then calls `Subscribe` on the original `Interval` source. So the effect is that the subscription to the underlying source doesn't happen on my main thread, it happens on the thread created for my `EventLoopScheduler`. Running the program produces this output:

```
[T:1] Main thread
[T:12] Created thread for EventLoopScheduler
[T:1] 21/07/2023 14:57:21: Main thread exiting
[T:6] 21/07/2023 14:57:22: Tick 0
[T:6] 21/07/2023 14:57:23: Tick 1
[T:6] 21/07/2023 14:57:24: Tick 2
...
```

Notice that my application's main thread exits before the source begins producing notifications. But also notice that the thread id for the newly created thread is 12, and yet my notifications are coming through on a different thread, with id 6! What's happening?

This often catches people out. The scheduler on which you subscribe to an observable source doesn't necessarily have any impact on how the source behaves once it is up and running. Remember earlier that I said `Observable.Interval` uses `DefaultScheduler` by default? Well we've not specified a scheduler for the `Interval` here, so it will be using that default. It doesn't care what context we invoke its `Subscribe` method from. So really, the only effect of introducing the `EventLoopScheduler` here has been to keep the process alive even after its main thread exits. That scheduler thread never actually gets used again after it makes its initial `Subscribe` call into the `IObservable<long>` returned by `Observable.Interval`. It just sits patiently waiting for further calls to `Schedule` that never come.

Not all sources are completely unaffected by the context in which their `Subscribe` is invoked, though. If I were to replace this line:

```csharp
    .Interval(TimeSpan.FromSeconds(1))
```

with this:

```csharp
    .Range(1, 5)
```

then we get this output:

```
[T:1] Main thread
[T:12] Created thread for EventLoopScheduler
[T:12] 21/07/2023 15:02:09: Tick 1
[T:1] 21/07/2023 15:02:09: Main thread exiting
[T:12] 21/07/2023 15:02:09: Tick 2
[T:12] 21/07/2023 15:02:09: Tick 3
[T:12] 21/07/2023 15:02:09: Tick 4
[T:12] 21/07/2023 15:02:09: Tick 5
```

Now all the notifications are coming in on thread 12, the thread created for the `EventLoopScheduler`. Note that even here, `Range` isn't using that scheduler. The difference is that `Range` defaults to `CurrentThreadScheduler`, so it will generate its outputs from whatever thread you happen to call it from. So even though it's not actually using the `EventLoopScheduler`, it does end up using that scheduler's thread, because we used that scheduler to subscribe to the `Range`.

So this illustrates that `SubscribeOn` is doing what it promises: it does determine the context from which `Subscribe` is invoked. It's just that it doesn't always matter what context that is. If `Subscribe` does non-trivial work, it can matter. For example, if you use [`Observable.Create`](03_CreatingObservableSequences.md#observablecreate) to create a custom sequence, `SubscribeOn` determines the context in which the callback you passed to `Create` is invoked. But Rx doesn't have a concept of a 'current' scheduler—there's no way to ask "which scheduler was I invoked from?"—so Rx operators don't just inherit their scheduler from the context on which they were subscribed.

When it comes to emitting items, most of the sources Rx supplies fall into one of three categories. First, operators that produce outputs in response to inputs from an upstream source (e.g., `Where`, `Select`, or `GroupBy`) generally call their observers methods from inside their own `OnNext`. So whatever context their source observable was running in when it called `OnNext`, that's the context the operator will use when calling its observer. Second, operators that produce items either iteratively, or based on timing will use a scheduler (either explicitly supplied, or a default type when none is specified). Third, some sources just produce items from whatever context they like. For example, if an `async` method uses `await` and specifies `ConfigureAwait(false)` then it could be on more or less any thread and in any context after the `await` completes, and it might then go on to invoke `OnNext` on an observer.

As long as a source follows [the fundamental rules of Rx sequences](02_KeyTypes.md#the-fundamental-rules-of-rx-sequences), it's allowed to invoke its observer's methods from any context it likes. It can choose to accept a scheduler as input and to use that, but it's under no obligation to. And if you have an unruly source of this kind that you'd like to tame, that's where the `ObserveOn` extension method comes in. Consider the following rather daft example:

```csharp
Observable
    .Interval(TimeSpan.FromSeconds(1))
    .SelectMany(tick => Observable.Return(tick, NewThreadScheduler.Default))
    .Subscribe(tick => 
      Console.WriteLine($"{DateTime.Now}-{Environment.CurrentManagedThreadId}: Tick {tick}"));
```

This deliberately causes every notification to arrive on a different thread, as this output shows:

```
Main thread: 1
21/07/2023 15:19:56-12: Tick 0
21/07/2023 15:19:57-13: Tick 1
21/07/2023 15:19:58-14: Tick 2
21/07/2023 15:19:59-15: Tick 3
...
```

(It's achieving this by calling `Observable.Return` for every single tick that emerges from `Interval`, and telling `Return` to use the `NewThreadScheduler`. Each such call to `Return` will create a new thread. This is a terrible idea, but it is an easy way to get a source that calls from a different context every time.) If I want to impose some order, I can add a call to `ObserveOn`:

```csharp
Observable
    .Interval(TimeSpan.FromSeconds(1))
    .SelectMany(tick => Observable.Return(tick, NewThreadScheduler.Default))
    .ObserveOn(new EventLoopScheduler())
    .Subscribe(tick => 
      Console.WriteLine($"{DateTime.Now}-{Environment.CurrentManagedThreadId}: Tick {tick}"));
```

I've created an `EventLoopScheduler` here because it creates a single thread, and runs every scheduled work item on that thread. The output now shows the same thread id (13) every time:

```
Main thread: 1
21/07/2023 15:24:23-13: Tick 0
21/07/2023 15:24:24-13: Tick 1
21/07/2023 15:24:25-13: Tick 2
21/07/2023 15:24:26-13: Tick 3
...
```

So although each new observable created by `Observable.Return` creates a brand new thread, `ObserveOn` ensures that my observer's `OnNext` (and `OnCompleted` or `OnError` in cases where those are called) is invoked via the specified scheduler.

### SubscribeOn and ObserveOn in UI applications

If you're using Rx in a user interface, `ObserveOn` is useful when you are dealing with information sources that don't provide notifications on the UI thread. You can wrap any `IObservable<T>` with `ObserveOn`, passing a `SynchronizationContextScheduler` (or a framework-specific type such as `DispatcherScheduler`), to ensure that your observer receives notifications on the UI thread, making it safe to update the UI.

`SubscribeOn` can also be useful in user interfaces as a way to ensure that any initialization work that an observable source does to get started does not happen on the UI thread.

Most UI frameworks designate one particular thread for receiving notifications from the user and also for updating the UI, for any one window. It is critical to avoid blocking this UI thread, as doing so leads to a poor user experience—if you are doing work on the UI thread, it will be unavailable for responding to user input until that work is done. As a general rule, if you cause a user interface to become unresponsive for longer than 100ms, users will become irritated, so you should not be perform any work that will take longer than this on the UI thread. When Microsoft first introduced its application store (which came in with Windows 8) they specified an even more stringent limit: if your application blocked the UI thread for longer than 50ms, it might not be allowed into the store. With the processing power offered by modern processors, you can achieve a lot of processing 50ms. Even on the relatively low-powered processors in mobile devices that's long enough to execute millions of instructions. However, anything involving I/O (reading or writing files, or waiting for a response from any kind of network service) should not be done on the UI thread. The general pattern for creating responsive UI applications is:

- receive a notification about some sort of user action
- if slow work is required, do this on a background thread
- pass the result back to the UI thread
- update the UI

This is a great fit for Rx: responding to events, potentially composing multiple events, passing data to chained method calls. With the inclusion of scheduling, we even have the power to get off and back onto the UI thread for that responsive application feel that users demand.

Consider a WPF application that used Rx to populate an `ObservableCollection<T>`. You could use `SubscribeOn` to ensure that the main work was not done on the UI thread, followed by `ObserveOn` to ensure you were notified back on the correct thread. If you failed to use the `ObserveOn` method, then your `OnNext` handlers would be invoked on the same thread that raised the notification. In most UI frameworks, this would cause some sort of not-supported/cross-threading exception. In this example, we subscribe to a sequence of `Customers`. I'm using `Defer` so that if `GetCustomers` does any slow initial work before returning its `IObservable<Customer>`, that won't happen until we subscribe. We then use `SubscribeOn` to call that method and perform the subscription on a task pool thread. Then we ensure that as we receive `Customer` notifications, we add them to the `Customers` collection on the `Dispatcher`.

```csharp
Observable
    .Defer(() => _customerService.GetCustomers())
    .SubscribeOn(TaskPoolScheduler.Default)
    .ObserveOn(DispatcherScheduler.Instance) 
    .Subscribe(Customers.Add);
```

Rx also offers `SubscribeOnDispatcher()` and `ObserveOnDispatcher()` extension methods to `IObservable<T>`, that automatically use the current thread's `Dispatcher` (and equivalents for `CoreDispatcher`). While these might be slightly more convenient they can make it harder to test your code. We explain why in the [Testing Rx](16_TestingRx.md) chapter.

## Concurrency pitfalls

Introducing concurrency to your application will increase its complexity. If your application is not noticeably improved by adding a layer of concurrency, then you should avoid doing so. Concurrent applications can exhibit maintenance problems with symptoms surfacing in the areas of debugging, testing and refactoring.

The common problem that concurrency introduces is unpredictable timing. Unpredictable timing can be caused by variable load on a system, as well as variations in system configurations (e.g. varying core clock speed and availability of processors). These can ultimately can result in [deadlocks](http://en.wikipedia.org/wiki/Deadlock), [livelocks](http://en.wikipedia.org/wiki/Deadlock#Livelock) and corrupted state.

A particularly significant danger of introducing concurrency to an application is that you can silently introduce bugs. Bugs arising from unpredictable timing are notoriously difficult to detect, making it easy for these kinds of defects to slip past Development, QA and UAT and only manifest themselves in Production environments. Rx, however, does such a good job of simplifying the concurrent processing of observable sequences that many of these concerns can be mitigated. You can still create problems, but if you follow the guidelines then you can feel a lot safer in the knowledge that you have heavily reduced the capacity for unwanted race conditions.

In a later chapter, [Testing Rx](16_TestingRx.md), we will look at how Rx improves your ability to test concurrent workflows.

### Lock-ups

Rx can simplify handling of concurrency, but it is not immune deadlock. Some calls (like `First`, `Last`, `Single` and `ForEach`) are blocking—they do not return until something that they are waiting for occurs. The following example shows that this makes it very easy for deadlock to occur:

```csharp
var sequence = new Subject<int>();

Console.WriteLine("Next line should lock the system.");

IEnumerable<int> value = sequence.First();
sequence.OnNext(1);

Console.WriteLine("I can never execute....");
```

The `First` method will not return until its source emits a sequence. But the code that causes this source to emit sequence is on the line _after_ the call to `First`. So the source can't emit a sequence until `First` returns. This style of deadlock, with two parties, each unable to proceed until the other proceeds, is often known as a _deadly embrace_. As this code shows, it's entirely possible for a deadly embrace to occur even in single threaded code. In fact, the single threaded nature of this code is what enables deadlock: we have two operations (waiting for the first notification, and sending the first notification) and only a single thread. That doesn't have to be a problem. If we'd used `FirstAsync` and attached an observer to that, `FirstAsync` would have executed its logic when the source `Subject<int>` invoked its `OnNext`. But that is more complex than just calling `First` and assigning the result into a variable.

This is an oversimplified example to illustrate the behaviour, and we would never write such code in production. (And even if we did, it fails so quickly and consistently that we would immediately become aware of a problem.) But in real application code, these kinds of problems can be harder to spot. Race conditions often slip into the system at integration points, so the problem isn't necessarily evidence in any one piece of code: timing problems can emerge as a result of how we plug multiple pieces of code together.

The next example may be a little harder to detect, but is only small step away from our first, unrealistic example. The basic idea is that we've got a subject that represents button clicks in a user interface. Event handlers representing user input are invoked by the UI framework. We just provide the framework with event handler methods, and it calls them for us whenever the event of interest, such as a button being clicked, occurs. This code calls `First` on the subject representing clicks, but it's less obvious that this might cause a problem here than it was in the preceding example:

```csharp
public Window1()
{
    InitializeComponent();
    DataContext = this;
    Value = "Default value";
    
    // Deadlock! We need the dispatcher to continue to allow me to click the button to produce a value
    Value = _subject.First();
    
    // This will have the intended effect, but because it does not block,
    // we can call this on the UI thread without deadlocking.
    //_subject.FirstAsync(1).Subscribe(value => Value = value);
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
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
    }
}
```

The earlier example called the subject's `OnNext` after `First` returned, making it relatively straightforward to see that if `First` didn't return, then the subject wouldn't emit a notification. But that's not as obvious here. The `MyButton_Click` event handler will be set up inside the call to `InitializeComponent` (as is normal in WPF code), so apparently we've done the necessary setup to enable events to flow. By the time we reach this call to `First`, the UI framework already knows that if the user clicks `MyButton`, it should call `MyButton_Click`, and that method is going to cause the subject to emit a value.

There's nothing intrinsically wrong with that use of `First`. (Risky, yes, but there are scenarios in which that exact code would be absolutely fine.) The problem is the context in which we've used it. This code is in the constructor of a UI element, and these always run on a particular thread associated with that window's UI elements. (This happens to be a WPF example, but other UI frameworks work the same way.) And that's the same thread that the UI framework will use to deliver notifications about user input. If we block this UI thread, we prevent the UI framework from invoking our button click event handler. So this blocking call is waiting for an event that can only be raised from the very thread that it is blocking, thus creating a deadlock.

You might be starting to get the impression that we should try to avoid blocking calls in Rx. This is a good rule of thumb. We can fix the code above by commenting out the line that uses `First`, and uncommenting the one below it containing this code:

```csharp
_subject.FirstAsync(1).Subscribe(value => Value = value);
```

This uses `FirstAsync` which does the same job, but with a different approach. It implements the same logic but it returns an `IObservable<T>` to which we must subscribe if we want to receive the first value whenever it does eventually appear. It is more complex than the just assigning the result of `First` into the `Value` property, but it is better adapted to the fact that we can't know when that source will produce a value.

If you do a lot of UI development, that last example might have seemed obviously wrong to you: we had code in the constructor for a window that wouldn't allow the constructor to complete until the user clicked a button in that window. The window isn't even going to appear until construction is complete so it makes no sense to wait for the user to click a button. That button's not even going to be visible on screen until after our constructor completes. Moreover, seasoned UI developers know that you don't just stop the world and wait for a specific action from the user. (Even modal dialogs, which effectively do demand a response before continuing, don't block the UI thread.) But as the next example shows, it's easy for problems to be harder to see. In this example, a button's click handler will try to get the first value from an observable sequence exposed via an interface.

```csharp
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
            if (handler != null) handler(this, new PropertyChangedEventArgs(nameof(Value2)));
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
```

Unlike the earlier example, this does not attempt to block progress in the constructor. The blocking call to `First` occurs here in a button click handler (the `MyButton2_Click` method near the end). This example is more interesting because this sort of thing isn't necessarily wrong. Applications often perform blocking operations in click handlers: when we click a button to save a copy of a document, we expect the application to perform all necessary IO work to write our data out to storage. With modern solid state storage devices, this often happens so quickly as to appear instantaneous, but back in the days when mechanical hard drives were the norm, it was not unusual for an application to become briefly unresponsive while it saved our document. This can happen even today if your storage is remote, and networking issues are causing delays.

So even if we've learned to be suspicious of blocking operations such as `First`, it's possible that it's OK in this example. It's not possible to tell for certain by looking at this code alone. It all depends on what sort of an observable `GetTemperature` returns, and the manner in which it produces its items. That call to `First` will block on the UI thread until a first item becomes available, so this will produce a deadlock if the production of that first item requires access to the UI thread. Here's a slightly contrived way to create that problem:

```csharp
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
```

This fakes up behaviour intended to simulate an actual temperature sensor by making a series of calls to `OnNext`. But it does some odd explicit scheduling: it calls `SubscribeOnDispatcher`. That's an extension method that effectively calls `SubscribeOn(DispatcherScheduler.Current.Dispatcher)`. This effectively tells Rx that when something tries to subscribe to the `IObservable<int>` that `GetTemperature` returns, that subscription call should be done through a WPF-specific scheduler that runs its work items on the UI thread. (Strictly, speaking, WPF does allow multiple UI threads, so to more precise, this code only works if you call it on a UI thread, and if you do so, the scheduler will ensure that work items are scheduled onto the same UI thread.)

The effect is that when our click handler calls `First`, that will in turn subscribe to the `IObservable<int>` returned by `GetTemperature`, and because that used `SubscribeOnDispatcher`, this does not invoke the callback passed to `Observable.Create` immediately. Instead, it schedules a work item that will do that when the UI thread (i.e., the thread we're running on) becomes free. It's not considered to be free right now, because it's in the middle of handling the button click. Having handed this work item to the scheduler, the `Subscribe` call returns back to the `First` method. And the `First` method now sits and waits for the first item to emerge. Since it won't return until that happens, the UI thread will not be considered to be available until that happens, meaning that the scheduled work item that was supposed to produce that first item can never run, and we have deadlock.

This boils down to the same basic problem as the first of these `First`-related deadlock examples. We have two processes: the generation of items, and waiting for an item to occur. These need to be in progress concurrently—we need the "wait for first item" logic to be up and running at the point when the source emits its first item. These examples all use just a single thread, which makes it a bad idea to use a single blocking call (`First`) both to set up the process of watching for the first item, and also to wait for that to happen. But even though it was the same basic problem in all three cases, it became harder to see as the code became more complex. With real application code, it's often a lot harder than this to see the root causes of deadlocks.

So far, this chapter may seem to say that concurrency is all doom and gloom by focusing on the problems you could face, and the fact that they are often hard to spot in practice; this is not the intent though. 
Although adopting Rx can't magically avoid classic concurrency problems, Rx can make it easier to get it right, provided you follow these two rules.

- Only the top-level subscriber should make scheduling decisions
- Avoid using blocking calls: e.g. `First`, `Last` and `Single`

The last example came unstuck with one simple problem; the `GetTemperature` service was dictating the scheduling model when, really, it had no business doing so. Code representing a temperature sensor shouldn't need to know that I'm using a particular UI framework, and certainly shouldn't be unilaterally deciding that it is going to run certain work on a WPF user interface thread.

When getting started with Rx, it can be easy to convince yourself that baking scheduling decisions into lower layers is somehow being 'helpful'. "Look!" you might say. "Not only have I provided temperature readings, I've also made this automatically notify you on the UI thread, so you won't have to bother with `ObserveOn`." The intentions may be good, but it's all too easy to create a threading nightmare.

Only the code that sets up a subscription and consumes its results can have a complete overview of the concurrency requirements, so that is the right level at which to choose which schedulers to use. Lower levels of code should not try to get involved; they should just do what they are told. (Rx arguably breaks this rule slightly itself by choosing default schedulers where they are needed. But it makes very conservative choices designed to minimize the chances of deadlock, and always allows applications to take control by specifying the scheduler.)

Note that following either one of the two rules above would have been sufficient to prevent deadlock in this example. But it is best to follow both rules.

This does leave one question unanswered: _how_ should the top-level subscriber make scheduling decisions? I've identified the area of the code that needs to make the decision, but what should the decision be? It will depend on the kind of application you are writing. For UI code, this pattern generally works well: "Subscribe on a background thread; Observe on the UI thread". With UI code, the risk of deadlock arises in because the UI thread is effectively a shared resource, and contention for that resource can produce deadlock. So the strategy is to avoid requiring that resource as much as possible: work that doesn't need to be on the thread should not be on that thread, which is why performing subscription on a worker thread (e.g., by using the `TaskPoolScheduler`) reduces the risk of deadlock.

It follows that if you have observable sources that decide when to produce events (e.g., timers, or sources representing inputs from external information feeds or devices) you would also want those to schedule work on worker threads. It is only when we need to update the user interface that we need our code to run on the UI thread, and so we defer that until the last possible moment by using `ObserveOn` in conjunction with a suitable UI-aware scheduler (such as the WPF `DispatcherScheduler`). If we have a complex Rx query made up out of multiple operators, this `ObserveOn` should come right at the end, just before we call `Subscribe` to attach the handler that will update the UI. This way, only the final step, the updating of the UI, will need access to the UI thread. By the time this runs, all complex processing will be complete, and so this should be able to run very quickly, relinquishing control of the UI thread almost immediately, improving application responsiveness, and lowering the risk of deadlock.

Other scenarios will require other strategies, but the general principle with deadlocks is always the same: understand which shared resources require exclusive access. For example, if you have a sensor library, it might create a dedicated thread to monitor devices and report new measurements, and if it were to stipulate that certain work had to be done on that thread, this would be very similar to the UI scenario: there is a particular thread that you will need to avoid blocking. The same approach would likely apply here. But this is not the only kind of scenario.

You could imagine a data processing application in which certain data structures are shared. It's quite common in these cases to be allowed to access such data structures from any thread, but to be required to do so one thread at a time. Typically we would use thread synchronization primitives to protect against concurrent use of these critical data structures. In these cases, the risks of deadlock do not arise from the use of particular threads. Instead, they arise from the possibility that one thread can't progress because some other thread is using a shared data structure, but that other thread is waiting for the first thread to do something, and won't relinquish its lock on that data structure until that happens. The simplest way to avoid problems here is to avoid blocking wherever possible. Avoid methods like `First`, preferring their non-blocking equivalents such as `FirstAsync`. (If there are cases where you can't avoid blocking, try to avoid doing so while in possession of locks that guard access to shared data. And if you really can't avoid that either, then there are no simple answers. You'll now have to start thinking about lock hierarchies to systematically avoid deadlock, just as you would if you weren't using Rx.) The non-blocking style is the natural way to do things with Rx, and that's the main way Rx can help you avoid concurrency related problems in these cases.

## Advanced features of schedulers

Schedulers provide some features that are mainly of interest when writing observable sources that need to interact with a scheduler. The most common way to use schedulers is when setting up a subscription, either supplying them as arguments when creating observable sources, or passing them to `SubscribeOn` and `ObserveOn`. But if you need to write an observable source that produces items on some schedule of its own choosing (e.g., suppose you are writing a library that represents some external data source and you want to present that as an `IObservable<T>`), you might need to use some of these more advanced features.

### Passing state

All of the methods defined by `IScheduler` take a `state` argument. Here's the interface definition again:

```csharp
public interface IScheduler
{
    DateTimeOffset Now { get; }

    IDisposable Schedule<TState>(TState state, 
                                 Func<IScheduler, TState, IDisposable> action);
    
    IDisposable Schedule<TState>(TState state, 
                                 TimeSpan dueTime, 
                                 Func<IScheduler, TState, IDisposable> action);
    
    IDisposable Schedule<TState>(TState state, 
                                 DateTimeOffset dueTime, 
                                 Func<IScheduler, TState, IDisposable> action);
}
```

The scheduler does not care what is in this `state` argument. It just passes it unmodified into your callback when it executes your work item. This provides one way to provide context for that callback. It's not strictly necessary: the delegate we pass as the `action` can incorporate whatever state we need. The easiest way to do that is to capture variables in a lambda. However, if you look at the [Rx source code](https://github.com/dotnet/reactive/) you will find that it typically doesn't do that. For example, the heart of the `Range` operator is a method called `LoopRec` and if you look at [the source for `LoopRec`](https://github.com/dotnet/reactive/blob/95d9ea9d2786f6ec49a051c5cff47dc42591e54f/Rx.NET/Source/src/System.Reactive/Linq/Observable/Range.cs#L55-L73) you'll see that it includes this line:

```csharp
var next = scheduler.Schedule(this, static (innerScheduler, @this) => @this.LoopRec(innerScheduler));
```

Logically, `Range` is just a loop that executes once for each item it produces. But to enable concurrent execution and to avoid stack overflows, it implements this by scheduling each iteration of the loop as an individual work item. (The method is called `LoopRec` because it is logically a recursive loop: it is kicked off by calling `Schedule`, and each time the scheduler calls this method, it calls `Schedule` again to ask for the next item to run. This doesn't actually cause recursion with any of Rx's built-in schedulers, even the `ImmediateScheduler`, because they all detect this and arrange to run the next item after the current one returns. But if you wrote the most naive scheduler possible, this would actually end up recursing at runtime, likely leading to stack overflows if you tried to create a large sequence.)

Notice that the lambda passed to `Schedule` has been annotated with `static`. This tells the C# compiler that it is our intention _not_ to capture any variables, and that any attempt to do so should cause a compiler error. The advantage of this is that the compiler is able to generate code that reuses the same delegate instance for every call. The first time this runs, it will create a delegate and store it in a hidden field. On every subsequent execution of this (either in future iterations of the same range, or for completely new range instances) it can just use that same delegate again and again and again. This is possible because the delegate captures no state. This avoids allocating a new object each time round the loop.

Couldn't the Rx library have used a more straightforward approach? We could choose not to use the state, passing a `null` state to scheduler, and then discarding the state argument passed to our callback:

```csharp
// Less weird, but less efficient:
var next = scheduler.Schedule<object?>(null, (innerScheduler, _) => LoopRec(innerScheduler));
```

This avoids the previous example's weirdness of passing our own `this` argument: now we're just invoking the `LoopRec` instance member in the ordinary way: we're implicitly using the `this` reference that is in scope. So this will create a delegate the captures that implicit `this` reference. This works, but it's inefficient: it will force the compiler to generate code that allocates a couple of objects. It creates one object that has a field holding onto the captured `this`, and then it needs to create a distinct delegate instance that has a reference to that capture object.

The more complex code that is actually in the `Range` implementation avoids this. It disables capture by annotating the lambda with `static`. That prevents code from relying on the implicit `this` reference. So it has had to arrange for the `this` reference to be available to the callback. And that's exactly the sort of thing the `state` argument is there for. It provides a way to pass in some per-work-item state so that you can avoid the overhead of capturing variables on each iteration.

### Future scheduling

I talked earlier about time-based operators, and also about the two time-based members of `ISchedule` that enable this, but I've not yet shown how to use it. These enable you to schedule an action to be executed in the future. (This relies on the process continuing to run for as long as necessary. As mentioned in earlier chapters, `System.Reactive` doesn't support persistent, durable subscriptions. So if you want to schedule something for days into the future, you might want to look at [Reaqtor](https://reaqtive.net/).) You can do so by specifying the exact point in time an action should be invoked by calling the overload of `Schedule` that takes a `DateTimeOffset`, or you can specify the period of time to wait until the action is invoked with the `TimeSpan`-based overload.

You can use the `TimeSpan` overload like this:

```csharp
var delay = TimeSpan.FromSeconds(1);
Console.WriteLine("Before schedule at {0:o}", DateTime.Now);

scheduler.Schedule(delay, () => Console.WriteLine("Inside schedule at {0:o}", DateTime.Now));
Console.WriteLine("After schedule at  {0:o}", DateTime.Now);
```

Output:

```
Before schedule at 2012-01-01T12:00:00.000000+00:00
After schedule at 2012-01-01T12:00:00.058000+00:00
Inside schedule at 2012-01-01T12:00:01.044000+00:00
```

This illustrates that scheduling was non-blocking here, because the 'before' and 'after' calls are very close together in time. (It will be this way for most schedulers, but as discussed earlier, `ImmediateScheduler` works differently. In this case, you would see the After message after the Inside one. that's why none of the timed operators will use `ImmediateScheduler` by default.) You can also see that approximately one second after the action was scheduled, it was invoked.

You can specify a specific point in time to schedule the task with the `DateTimeOffset` overload. If, for some reason, the point in time you specify is in the past, then the action is scheduled as soon as possible. Be aware that changes in the system clock complicate matters. Rx's schedulers do make some accommodations to deal with clock drift, but sudden large changes to the system clock can cause some short term chaos.

### Cancellation

Each of the overloads to `Schedule` returns an `IDisposable`, and calling `Dispose` on this will cancel the scheduled work. In the previous example, we scheduled work to be invoked in one second. We could cancel that work by disposing of the return value.

```csharp
var delay = TimeSpan.FromSeconds(1);
Console.WriteLine("Before schedule at {0:o}", DateTime.Now);

var workItem = scheduler.Schedule(delay, 
   () => Console.WriteLine("Inside schedule at {0:o}", DateTime.Now));

Console.WriteLine("After schedule at  {0:o}", DateTime.Now);

workItem.Dispose();
```

Output:

```
Before schedule at 2012-01-01T12:00:00.000000+00:00
After schedule at 2012-01-01T12:00:00.058000+00:00
```

Note that the scheduled action never occurred, because we cancelled it almost immediately.

When the user cancels the scheduled action method before the scheduler is able to invoke it, that action is just removed from the queue of work. This is what we see in example above. It's possible to cancel scheduled work that is already running, and this is why the work item callback is required to return `IDisposable`: if work has already begun when you try to cancel the work item, Rx calls `Dispose` on the `IDisposable` that your work item callback returned. This gives a way for users to cancel out of a job that may already be running. This job could be some sort of I/O, heavy computations or perhaps usage of `Task` to perform some work.

You may be wondering how this mechanism can be any use: the work item callback needs to have returned already for Rx to be able to invoke the `IDisposable` that it returns. This mechanism can only be used in practice if work continues after returning to the scheduler. You could fire up another thread so the work happens concurrently, although we generally try to avoid creating threads in Rx. Another possibility would be if the scheduled work item invoked some asynchronous API and returned without waiting for it to complete. If that API offered cancellation, you could return an `IDisposable` that cancelled it.

To illustrate cancellation in operation, this slightly unrealistic example runs some work as a `Task` to enable it to continue after our callback returns. It just fakes some work by performing a spin wait and adding values to the `list` argument. The key here is that we create a `CancellationToken` to be able to tell the task we want it to stop, and we return an `IDisposable` that puts this token in to a cancelled state.

```csharp
public IDisposable Work(IScheduler scheduler, List<int> list)
{
    CancellationTokenSource tokenSource = new();
    CancellationToken cancelToken = tokenSource.Token;
    Task task = new(() =>
    {
        Console.WriteLine();
   
        for (int i = 0; i < 1000; i++)
        {
            SpinWait sw = new();
   
            for (int j = 0; j < 3000; j++) sw.SpinOnce();
   
            Console.Write(".");
   
            list.Add(i);
   
            if (cancelToken.IsCancellationRequested)
            {
                Console.WriteLine("Cancellation requested");
                
                // cancelToken.ThrowIfCancellationRequested();
                
                return;
            }
        }
    }, cancelToken);
   
    task.Start();
   
    return Disposable.Create(tokenSource.Cancel);
}
```

This code schedules the above code and allows the user to cancel the processing work by pressing Enter

```csharp
List<int> list = new();
Console.WriteLine("Enter to quit:");

IDisposable token = scheduler.Schedule(list, Work);
Console.ReadLine();

Console.WriteLine("Cancelling...");

token.Dispose();

Console.WriteLine("Cancelled");
```

Output:

```
Enter to quit:
........
Cancelling...
Cancelled
Cancellation requested
```

The problem here is that we have introduced explicit use of `Task` so we are increasing concurrency in a way that is outside of the control of the scheduler. The Rx library generally allows control over the way in which concurrency is introduced by accepting a scheduler parameter. If the goal is to enable long-running iterative work, we can avoid having to spin up new threads or tasks but using Rx recursive scheduler features instead. I already talked a bit about this in the [Passing state](#passing-state) section, but there are a few ways to go about it.

### Recursion

In addition to the `IScheduler` methods, Rx defines various overloads of `Schedule` in the form of extension methods. Some of these take some strange looking delegates as parameters. Take special note of the final parameter in each of these overloads of the `Schedule` extension method.

```csharp
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
```

Each of these overloads take a delegate "action" that allows you to call "action" recursively. This may seem a very odd signature, but it allows us to achieve a similar logically recursive iterative approach as you saw in [Passing state](#passing-state) section, but in a potentially simpler way.

This example uses the simplest recursive overload. We have an `Action` that can be called recursively.

```csharp
Action<Action> work = (Action self) =>
{
    Console.WriteLine("Running");
    self();
};

var token = s.Schedule(work);
    
Console.ReadLine();
Console.WriteLine("Cancelling");

token.Dispose();

Console.WriteLine("Cancelled");
```

Output:

```
Enter to quit:
Running
Running
Running
Running
Cancelling
Cancelled
Running
```

Note that we didn't have to write any cancellation code in our delegate. Rx handled the looping and checked for cancellation on our behalf. Since each individual iteration was scheduled as a separate work item, there are no long-running jobs, so it's enough to let the scheduler deal entirely with cancellation.

The main difference between these overloads, and using the `IScheduler` methods directly, is that you don't need to pass another callback directly into the scheduler. You just invoke the supplied `Action` and it schedules another call to your method. They also enable you not to pass a state argument if you don't have any use for one.

As mentioned in the earlier section, although this logically represents recursion, Rx protects us from stack overflows. The schedulers implement this style of recursion by waiting for the method to return before performing the recursive call.
    
This concludes our tour of scheduling and threading. Next, we will look at the related topic of timing.
