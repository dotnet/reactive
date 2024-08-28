# Testing Rx

Modern quality assurance standards demand comprehensive automated testing that can help evaluate and prevent bugs. It is good practice to have a suite of tests that verify correct behaviour and to run this as part of the build process to detect regressions early.

The `System.Reactive` source code includes a comprehensive tests suite. Testing Rx-based code presents some challenges, especially when time-sensitive operators are involved. Rx.NET's test suite includes many tests designed to exercise awkward edge cases to ensure predictable behaviour under load. This is only possible because Rx.NET was designed to be testable.

In this chapter, we'll show how you can take advantage of Rx's testability in your own code.

## Virtual Time

It's common to deal with timing in Rx. As you've seen, it offers several operators that take time into account, and this presents a challenge. We don't want to introduce slow tests, because that can make test suites take too long to execute, but how might we test an application that waits for the user to stop typing for half a second before submitting a query? Non-deterministic tests can also be a problem: when there are race conditions it can be very hard to recreate these reliably.

The [Scheduling and Threading](11_SchedulingAndThreading.md) chapter described how schedulers use a virtualized representation of time. This is critical for enabling tests to validate time-related behaviour. It lets us control Rx's perception of the progression of time, enabling us to write tests that logically take seconds, but which execute in microseconds.

Consider this example, where we create a sequence that publishes values every second for five seconds.

```csharp
IObservable<long> interval = Observable
    .Interval(TimeSpan.FromSeconds(1))
    .Take(5);
```

A naive a test to ensure that this produces five values at one second intervals would take five seconds to run. That would be no good; we want hundreds if not thousands of tests to run in five seconds. Another very common requirement is to test a timeout. Here, we try to test a timeout of one minute.

```csharp
var never = Observable.Never<int>();
var exceptionThrown = false;

never.Timeout(TimeSpan.FromMinutes(1))
     .Subscribe(
        i => Console.WriteLine("This will never run."),
        ex => exceptionThrown = true);

Assert.IsTrue(exceptionThrown);
```

It looks like we would have no choice but to make our test wait for a minute before running that assert. In practice, we'd want to wait a little over a minute, because if the computer running the test is busy, it might trigger the timeout bit later than we've asked. This kind of scenario is notorious for causing tests to fail occasionally even when there's no real problem in the code being tested.

Nobody wants slow, inconsistent tests. So let's look at how Rx helps us to avoid these problems.

## TestScheduler

The [Scheduling and Threading](11_SchedulingAndThreading.md) chapter explained that schedulers determine when and how to execute code, and that they keep track of time. Most of the schedulers we looked at in that chapter addressed various threading concerns, and when it came to timing, they all attempted to run work at the time requested. But Rx provides `TestScheduler`, which handles time completely differently. It takes advantage of the fact that schedulers control all time-related behaviour to allow us to emulate and control time.

**Note:** `TestScheduler` is not in the main `System.Reactive` package. You will need to add a reference to `Microsoft.Reactive.Testing` to use it.

Any scheduler maintains a queue of actions to be executed. Each action is assigned a point in time when it should be executed. (Sometimes that time is "as soon as possible" but time-based operators will often schedule work to run at some specific time in the future.) If we use the `TestScheduler` it will effectively act as though time stands still until we tell it we want time to move on.

In this example, we schedule a task to be run immediately by using the simplest `Schedule` overload. Even though this effectively asks for the work to be run as soon as possible, the `TestScheduler` always waits for us to tell it we're ready before processing newly queued work. We advance the virtual clock forward by one tick, at which point it will execute that queued work. (It runs all newly-queued "as soon as possible" work any time we advance the virtual time. If we advance the time far enough to mean that work that was previously logically in the future is now runnable, it runs that too.)

```csharp
var scheduler = new TestScheduler();
var wasExecuted = false;
scheduler.Schedule(() => wasExecuted = true);
Assert.IsFalse(wasExecuted);
scheduler.AdvanceBy(1); // execute 1 tick of queued actions
Assert.IsTrue(wasExecuted);
```

The `TestScheduler` implements the `IScheduler` interface and also defines methods allowing us to control and monitor virtual time. This shows these additional methods:

```csharp
public class TestScheduler : // ...
{
    public bool IsEnabled { get; private set; }
    public TAbsolute Clock { get; protected set; }
    public void Start()
    public void Stop()
    public void AdvanceTo(long time)
    public void AdvanceBy(long time)
    
    ...
}
```

`TestScheduler` works in the same units as [`TimeSpan.Ticks`](https://learn.microsoft.com/en-us/dotnet/api/system.timespan.ticks). If you want to move time forward by 1 second, you can call `scheduler.AdvanceBy(TimeSpan.FromSeconds(1).Ticks)`. One tick corresponds to 100ns, so 1 second is 10,000,000 ticks.

### AdvanceTo

The `AdvanceTo(long)` method sets the virtual time to the specified number of ticks. This will execute all the actions that have been scheduled up to that absolute time specified. The `TestScheduler` uses ticks as its measurement of time. In this example, we schedule actions to be invoked now, in 10 ticks, and in 20 ticks (1 and 2 microseconds respectively).

```csharp
var scheduler = new TestScheduler();
scheduler.Schedule(() => Console.WriteLine("A")); // Schedule immediately
scheduler.Schedule(TimeSpan.FromTicks(10), () => Console.WriteLine("B"));
scheduler.Schedule(TimeSpan.FromTicks(20), () => Console.WriteLine("C"));

Console.WriteLine("scheduler.AdvanceTo(1);");
scheduler.AdvanceTo(1);

Console.WriteLine("scheduler.AdvanceTo(10);");
scheduler.AdvanceTo(10);

Console.WriteLine("scheduler.AdvanceTo(15);");
scheduler.AdvanceTo(15);

Console.WriteLine("scheduler.AdvanceTo(20);");
scheduler.AdvanceTo(20);
```

Output:

```
scheduler.AdvanceTo(1);
A
scheduler.AdvanceTo(10);
B
scheduler.AdvanceTo(15);
scheduler.AdvanceTo(20);
C
```

Note that nothing happened when we advanced to 15 ticks. All work scheduled before 15 ticks had been performed and we had not advanced far enough yet to get to the next scheduled action.

### AdvanceBy
    
The `AdvanceBy(long)` method allows us to move the clock forward by some amount of time. Unlike `AdvanceTo`, the argument here is relative to the current virtual time. Again, the measurements are in ticks. We can take the last example and modify it to use `AdvanceBy(long)`.

```csharp
var scheduler = new TestScheduler();
scheduler.Schedule(() => Console.WriteLine("A")); // Schedule immediately
scheduler.Schedule(TimeSpan.FromTicks(10), () => Console.WriteLine("B"));
scheduler.Schedule(TimeSpan.FromTicks(20), () => Console.WriteLine("C"));

Console.WriteLine("scheduler.AdvanceBy(1);");
scheduler.AdvanceBy(1);

Console.WriteLine("scheduler.AdvanceBy(9);");
scheduler.AdvanceBy(9);

Console.WriteLine("scheduler.AdvanceBy(5);");
scheduler.AdvanceBy(5);

Console.WriteLine("scheduler.AdvanceBy(5);");
scheduler.AdvanceBy(5);
```

Output:

```
scheduler.AdvanceBy(1);
A
scheduler.AdvanceBy(9);
B
scheduler.AdvanceBy(5);
scheduler.AdvanceBy(5);
C
```

### Start
    
The `TestScheduler`'s `Start()` method runs everything that has been scheduled, advancing virtual time as necessary for work items that were queued for a specific time. We take the same example again and swap out the `AdvanceBy(long)` calls for a single `Start()` call.

```csharp
var scheduler = new TestScheduler();
scheduler.Schedule(() => Console.WriteLine("A")); // Schedule immediately
scheduler.Schedule(TimeSpan.FromTicks(10), () => Console.WriteLine("B"));
scheduler.Schedule(TimeSpan.FromTicks(20), () => Console.WriteLine("C"));

Console.WriteLine("scheduler.Start();");
scheduler.Start();

Console.WriteLine("scheduler.Clock:{0}", scheduler.Clock);
```

Output:

```
scheduler.Start();
A
B
C
scheduler.Clock:20
```

Note that once all of the scheduled actions have been executed, the virtual clock matches our last scheduled item (20 ticks).

We further extend our example by scheduling a new action to happen after `Start()` has already been called.

```csharp
var scheduler = new TestScheduler();
scheduler.Schedule(() => Console.WriteLine("A"));
scheduler.Schedule(TimeSpan.FromTicks(10), () => Console.WriteLine("B"));
scheduler.Schedule(TimeSpan.FromTicks(20), () => Console.WriteLine("C"));

Console.WriteLine("scheduler.Start();");
scheduler.Start();

Console.WriteLine("scheduler.Clock:{0}", scheduler.Clock);

scheduler.Schedule(() => Console.WriteLine("D"));
```

Output:

```
scheduler.Start();
A
B
C
scheduler.Clock:20
```

Note that the output is exactly the same; If we want our fourth action to be executed, we will have to call `Start()` (or `AdvanceTo` or `AdvanceBy`) again.

### Stop

There is a `Stop()` method whose name seems to imply some symmetry with `Start()`. This sets the scheduler's `IsEnabled` property to false, and if `Start` is currently running, this means that it will stop inspecting the queue for further work, and will return as soon as the work item currently being processed completes.

In this example, we show how you could use `Stop()` to pause processing of scheduled actions.

```csharp
var scheduler = new TestScheduler();
scheduler.Schedule(() => Console.WriteLine("A"));
scheduler.Schedule(TimeSpan.FromTicks(10), () => Console.WriteLine("B"));
scheduler.Schedule(TimeSpan.FromTicks(15), scheduler.Stop);
scheduler.Schedule(TimeSpan.FromTicks(20), () => Console.WriteLine("C"));

Console.WriteLine("scheduler.Start();");
scheduler.Start();
Console.WriteLine("scheduler.Clock:{0}", scheduler.Clock);
```

Output:

```
scheduler.Start();
A
B
scheduler.Clock:15
```

Note that "C" never gets printed as we stop the clock at 15 ticks.

Since `Start` automatically stops when it has drained the work queue, you're under no obligation to call `Stop`. It's there only if you want to call `Start` but then pause processing part way through the test.

### Schedule collision

When scheduling actions, it is possible and even likely that many actions will be scheduled for the same point in time. This most commonly would occur when scheduling multiple actions for _now_. It could also happen that there are multiple actions scheduled for the same point in the future. The `TestScheduler` has a simple way to deal with this. When actions are scheduled, they are marked with the clock time they are scheduled for. If multiple items are scheduled for the same point in time, they are queued in order that they were scheduled; when the clock advances, all items for that point in time are executed in the order that they were scheduled.

```csharp
var scheduler = new TestScheduler();
scheduler.Schedule(TimeSpan.FromTicks(10), () => Console.WriteLine("A"));
scheduler.Schedule(TimeSpan.FromTicks(10), () => Console.WriteLine("B"));
scheduler.Schedule(TimeSpan.FromTicks(10), () => Console.WriteLine("C"));

Console.WriteLine("scheduler.Start();");
scheduler.Start();
Console.WriteLine("scheduler.Clock:{0}", scheduler.Clock);
```

Output:

```
scheduler.AdvanceTo(10);
A
B
C
scheduler.Clock:10
```

Note that the virtual clock is at 10 ticks, the time we advanced to.

## Testing Rx code

Now that we have learnt a little bit about the `TestScheduler`, let's look at how we could use it to test our two initial code snippets that use `Interval` and `Timeout`. We want to execute tests as fast as possible but still maintain the semantics of time. In this example we generate our five values one second apart but pass in our `TestScheduler` to the `Interval` method to use instead of the default scheduler.

```csharp
[TestMethod]
public void Testing_with_test_scheduler()
{
    var expectedValues = new long[] {0, 1, 2, 3, 4};
    var actualValues = new List<long>();
    var scheduler = new TestScheduler();

    var interval = Observable.Interval(TimeSpan.FromSeconds(1), scheduler).Take(5);
    
    interval.Subscribe(actualValues.Add);

    scheduler.Start();
    CollectionAssert.AreEqual(expectedValues, actualValues);
    // Executes in less than 0.01s "on my machine"
}
```

While this is mildly interesting, what I think is more important is how we would test a real piece of code. Imagine, if you will, a ViewModel that subscribes to a stream of prices. As prices are published, it adds them to a collection. Assuming this is a WPF implementation, we take the liberty of enforcing that the subscription be done on the `ThreadPool` and the observing is executed on the `Dispatcher`.

```csharp
public class MyViewModel : IMyViewModel
{
    private readonly IMyModel _myModel;
    private readonly ObservableCollection<decimal> _prices;

    public MyViewModel(IMyModel myModel)
    {
        _myModel = myModel;
        _prices = new ObservableCollection<decimal>();
    }

    public void Show(string symbol)
    {
        // TODO: resource mgt, exception handling etc...
        _myModel.PriceStream(symbol)
                .SubscribeOn(Scheduler.ThreadPool)
                .ObserveOn(Scheduler.Dispatcher)
                .Timeout(TimeSpan.FromSeconds(10), Scheduler.ThreadPool)
                .Subscribe(
                    Prices.Add,
                    ex=>
                        {
                            if(ex is TimeoutException)
                                IsConnected = false;
                        });
        IsConnected = true;
    }

    public ObservableCollection<decimal> Prices
    {
        get { return _prices; }
    }

    public bool IsConnected { get; private set; }
}
```

### Injecting scheduler dependencies

While the snippet of code above may do what we want it to, it will be hard to test as it is accessing the schedulers via static properties. You will need some way of enabling tests to supply different schedulers during testing. In this example, we're going to define an interface for this purpose:

```csharp
public interface ISchedulerProvider
{
    IScheduler CurrentThread { get; }
    IScheduler Dispatcher { get; }
    IScheduler Immediate { get; }
    IScheduler NewThread { get; }
    IScheduler ThreadPool { get; }
    IScheduler TaskPool { get; } 
}
```

The default implementation that we would run in production is implemented as follows:

```csharp
public sealed class SchedulerProvider : ISchedulerProvider
{
    public IScheduler CurrentThread => Scheduler.CurrentThread;
    public IScheduler Dispatcher => DispatcherScheduler.Instance;
    public IScheduler Immediate => Scheduler.Immediate;
    public IScheduler NewThread => Scheduler.NewThread;
    public IScheduler ThreadPool => Scheduler.ThreadPool;
    public IScheduler TaskPool => Scheduler.TaskPool;
}
```

We can substitute implementations of `ISchedulerProvider` to help with testing. For example:

```csharp
public sealed class TestSchedulers : ISchedulerProvider
{
    // Schedulers available as TestScheduler type
    public TestScheduler CurrentThread { get; }  = new TestScheduler();
    public TestScheduler Dispatcher { get; }  = new TestScheduler();
    public TestScheduler Immediate { get; }  = new TestScheduler();
    public TestScheduler NewThread { get; }  = new TestScheduler();
    public TestScheduler ThreadPool { get; }  = new TestScheduler();
    
    // ISchedulerService needs us to return IScheduler, but we want the properties
    // to return TestScheduler for the convenience of test code, so we provide
    // explicit implementations of all the properties to match ISchedulerService.
    IScheduler ISchedulerProvider.CurrentThread => CurrentThread;
    IScheduler ISchedulerProvider.Dispatcher => Dispatcher;
    IScheduler ISchedulerProvider.Immediate => Immediate;
    IScheduler ISchedulerProvider.NewThread => NewThread;
    IScheduler ISchedulerProvider.ThreadPool => ThreadPool;
}
```

Note that `ISchedulerProvider` is implemented explicitly because that interface requires each property to return an `IScheduler`, but our tests will need to access the `TestScheduler` instances directly. I can now write some tests for my ViewModel. Below, we test a modified version of the `MyViewModel` class that takes an `ISchedulerProvider` and uses that instead of the static schedulers from the `Scheduler` class. We also use the popular [Moq](https://github.com/Moq) framework to provide a suitable fake implementation of our model.

```csharp
[TestInitialize]
public void SetUp()
{
    _myModelMock = new Mock<IMyModel>();
    _schedulerProvider = new TestSchedulers();
    _viewModel = new MyViewModel(_myModelMock.Object, _schedulerProvider);
}

[TestMethod]
public void Should_add_to_Prices_when_Model_publishes_price()
{
    decimal expected = 1.23m;
    var priceStream = new Subject<decimal>();
    _myModelMock.Setup(svc => svc.PriceStream(It.IsAny<string>())).Returns(priceStream);

    _viewModel.Show("SomeSymbol");
    
    // Schedule the OnNext
    _schedulerProvider.ThreadPool.Schedule(() => priceStream.OnNext(expected));  

    Assert.AreEqual(0, _viewModel.Prices.Count);

    // Execute the OnNext action
    _schedulerProvider.ThreadPool.AdvanceBy(1);  
    Assert.AreEqual(0, _viewModel.Prices.Count);
    
    // Execute the OnNext handler
    _schedulerProvider.Dispatcher.AdvanceBy(1);  
    Assert.AreEqual(1, _viewModel.Prices.Count);
    Assert.AreEqual(expected, _viewModel.Prices.First());
}

[TestMethod]
public void Should_disconnect_if_no_prices_for_10_seconds()
{
    var timeoutPeriod = TimeSpan.FromSeconds(10);
    var priceStream = Observable.Never<decimal>();
    _myModelMock.Setup(svc => svc.PriceStream(It.IsAny<string>())).Returns(priceStream);

    _viewModel.Show("SomeSymbol");

    _schedulerProvider.ThreadPool.AdvanceBy(timeoutPeriod.Ticks - 1);
    Assert.IsTrue(_viewModel.IsConnected);
    _schedulerProvider.ThreadPool.AdvanceBy(timeoutPeriod.Ticks);
    Assert.IsFalse(_viewModel.IsConnected);
}
```

Output:

```
2 passed, 0 failed, 0 skipped, took 0.41 seconds (MSTest 10.0).
```

These two tests ensure five things:

* That the `Price` property has prices added to it as the model produces them
* That the sequence is subscribed to on the ThreadPool
* That the `Price` property is updated on the Dispatcher i.e. the sequence is observed on the Dispatcher
* That a timeout of 10 seconds between prices will set the ViewModel to disconnected
* The tests run fast.
  
While the time to run the tests is not that impressive, most of that time seems to be spent warming up my test harness. Moreover, increasing the test count to 10 only adds 0.03seconds. In general, a modern CPU should be able to execute thousands of unit tests per second.

In the first test, we can see that only once both the `ThreadPool` and the `Dispatcher` schedulers have been run will we get a result. In the second test, it helps to verify that the timeout is not less than 10 seconds.

In some scenarios, you are not interested in the scheduler and you want to be focusing your tests on other functionality. If this is the case, then you may want to create another test implementation of the `ISchedulerProvider` that returns the `ImmediateScheduler` for all of its members. That can help reduce the noise in your tests.

```csharp
public sealed class ImmediateSchedulers : ISchedulerService
{
    public IScheduler CurrentThread => Scheduler.Immediate;
    public IScheduler Dispatcher => Scheduler.Immediate;
    public IScheduler Immediate => Scheduler.Immediate;
    public IScheduler NewThread => Scheduler.Immediate;
    public IScheduler ThreadPool => Scheduler.Immediate;
}
```

## Advanced features - ITestableObserver

The `TestScheduler` provides further advanced features. These can be useful when parts of your test setup need to run at particular virtual times.

### `Start(Func<IObservable<T>>)`

There are three overloads to `Start`, which are used to start an observable sequence at a given time, record the notifications it makes and dispose of the subscription at a given time. This can be confusing at first, as the parameterless overload of `Start` is quite unrelated. These three overloads return an `ITestableObserver<T>` which allows you to record the notifications from an observable sequence, much like the `Materialize` method we saw in the [Transformation chapter](06_Transformation.md#materialize-and-dematerialize).

```csharp
public interface ITestableObserver<T> : IObserver<T>
{
    // Gets recorded notifications received by the observer.
    IList<Recorded<Notification<T>>> Messages { get; }
}
```

While there are three overloads, we will look at the most specific one first. This overload takes four parameters:

* an observable sequence factory delegate
* the point in time to invoke the factory
* the point in time to subscribe to the observable sequence returned from the factory
* the point in time to dispose of the subscription

The _time_ for the last three parameters is measured in ticks, as per the rest of the `TestScheduler` members.

```csharp
public ITestableObserver<T> Start<T>(
    Func<IObservable<T>> create, 
    long created, 
    long subscribed, 
    long disposed)
{...}
```

We could use this method to test the `Observable.Interval` factory method. Here, we create an observable sequence that spawns a value every second for 4 seconds. We use the `TestScheduler.Start` method to create and subscribe to it immediately (by passing 0 for the second and third parameters). We dispose of our subscription after 5 seconds. Once the `Start` method has run, we output what we have recorded.

```csharp
var scheduler = new TestScheduler();
var source = Observable.Interval(TimeSpan.FromSeconds(1), scheduler)
    .Take(4);

var testObserver = scheduler.Start(
    () => source, 
    0, 
    0, 
    TimeSpan.FromSeconds(5).Ticks);

Console.WriteLine("Time is {0} ticks", scheduler.Clock);
Console.WriteLine("Received {0} notifications", testObserver.Messages.Count);

foreach (Recorded<Notification<long>> message in testObserver.Messages)
{
    Console.WriteLine("{0} @ {1}", message.Value, message.Time);
}
```

Output:

```
Time is 50000000 ticks
Received 5 notifications
OnNext(0) @ 10000001
OnNext(1) @ 20000001
OnNext(2) @ 30000001
OnNext(3) @ 40000001
OnCompleted() @ 40000001
```

Note that the `ITestObserver<T>` records `OnNext` and `OnCompleted` notifications. If the sequence was to terminate in error, the `ITestObserver<T>` would record the `OnError` notification instead.

We can play with the input variables to see the impact it makes. We know that the `Observable.Interval` method is a Cold Observable, so the virtual time of the creation is not relevant. Changing the virtual time of the subscription can change our results. If we change it to 2 seconds, we will notice that if we leave the disposal time at 5 seconds, we will miss some messages.

```csharp
var testObserver = scheduler.Start(
    () => Observable.Interval(TimeSpan.FromSeconds(1), scheduler).Take(4), 
    0,
    TimeSpan.FromSeconds(2).Ticks,
    TimeSpan.FromSeconds(5).Ticks);
```

Output:

```
Time is 50000000 ticks
Received 2 notifications
OnNext(0) @ 30000000
OnNext(1) @ 40000000
```

We start the subscription at 2 seconds; the `Interval` produces values after each second (i.e. second 3 and 4), and we dispose on second 5. So we miss the other two `OnNext` messages as well as the `OnCompleted` message.

There are two other overloads to this `TestScheduler.Start` method.

```csharp
public ITestableObserver<T> Start<T>(Func<IObservable<T>> create, long disposed)
{
    if (create == null)
    {
        throw new ArgumentNullException("create");
    }
    else
    {
        return this.Start<T>(create, 100L, 200L, disposed);
    }
}

public ITestableObserver<T> Start<T>(Func<IObservable<T>> create)
{
    if (create == null)
    {
        throw new ArgumentNullException("create");
    }
    else
    {
        return this.Start<T>(create, 100L, 200L, 1000L);
    }
}
```

As you can see, these overloads just call through to the variant we have been looking at, but passing some default values. These default values provide short gaps before creation and between creation and subscription, giving enough space to configure other things to happen between them. And then the disposal happens a bit later, allowing a little longer for the thing to run. There's nothing particularly magical about these default values, but if you value a lack of clutter over it being completely obvious what happens when, and are happy to rely on the invisible effects of convention, then you might prefer this. The Rx source code itself contains thousands of tests, and a very large number of them use the simplest `Start` overload, and developers working in the code base day in, day out soon get used to the idea that creation occurs at time 100, and subscription at time 200, and that you test everything you need to before 1000.

### CreateColdObservable

Just as we can record an observable sequence, we can also use `CreateColdObservable` to play back a set of `Recorded<Notification<int>>`. The signature for `CreateColdObservable` simply takes a `params` array of recorded notifications.

```csharp
// Creates a cold observable from an array of notifications.
// Returns a cold observable exhibiting the specified message behavior.
public ITestableObservable<T> CreateColdObservable<T>(
    params Recorded<Notification<T>>[] messages)
{...}
```

The `CreateColdObservable` returns an `ITestableObservable<T>`. This interface extends `IObservable<T>` by exposing the list of "subscriptions" and the list of messages it will produce.

```csharp
public interface ITestableObservable<T> : IObservable<T>
{
    // Gets the subscriptions to the observable.
    IList<Subscription> Subscriptions { get; }

    // Gets the recorded notifications sent by the observable.
    IList<Recorded<Notification<T>>> Messages { get; }
}
```

Using `CreateColdObservable`, we can emulate the `Observable.Interval` test we had earlier.

```csharp
var scheduler = new TestScheduler();
var source = scheduler.CreateColdObservable(
    new Recorded<Notification<long>>(10000000, Notification.CreateOnNext(0L)),
    new Recorded<Notification<long>>(20000000, Notification.CreateOnNext(1L)),
    new Recorded<Notification<long>>(30000000, Notification.CreateOnNext(2L)),
    new Recorded<Notification<long>>(40000000, Notification.CreateOnNext(3L)),
    new Recorded<Notification<long>>(40000000, Notification.CreateOnCompleted<long>())
    );

var testObserver = scheduler.Start(
    () => source,
    0,
    0,
    TimeSpan.FromSeconds(5).Ticks);

Console.WriteLine("Time is {0} ticks", scheduler.Clock);
Console.WriteLine("Received {0} notifications", testObserver.Messages.Count);

foreach (Recorded<Notification<long>> message in testObserver.Messages)
{
    Console.WriteLine("  {0} @ {1}", message.Value, message.Time);
}
```

Output:

```
Time is 50000000 ticks
Received 5 notifications
OnNext(0) @ 10000001
OnNext(1) @ 20000001
OnNext(2) @ 30000001
OnNext(3) @ 40000001
OnCompleted() @ 40000001
```

Note that our output is exactly the same as the previous example with `Observable.Interval`.

### CreateHotObservable

We can also create hot test observable sequences using the `CreateHotObservable` method. It has the same parameters and return value as `CreateColdObservable`; the difference is that the virtual time specified for each message is now relative to when the observable was created, not when it is subscribed to as per the `CreateColdObservable` method.

This example is just that last "cold" sample, but creating a Hot observable instead.

```csharp
var scheduler = new TestScheduler();
var source = scheduler.CreateHotObservable(
    new Recorded<Notification<long>>(10000000, Notification.CreateOnNext(0L)),
// ...    
```

Output:

```
Time is 50000000 ticks
Received 5 notifications
OnNext(0) @ 10000000
OnNext(1) @ 20000000
OnNext(2) @ 30000000
OnNext(3) @ 40000000
OnCompleted() @ 40000000
```

Note that the output is almost the same. Scheduling of the creation and subscription do not affect the Hot Observable, therefore the notifications happen 1 tick earlier than their Cold counterparts.

We can see the major difference a Hot Observable bears by changing the virtual create time and virtual subscribe time to be different values. With a Cold Observable, the virtual create time has no real impact, as subscription is what initiates any action. This means we cannot miss any early message on a Cold Observable. For Hot Observables, we can miss messages if we subscribe too late. Here, we create the Hot Observable immediately, but only subscribe to it after 1 second (thus missing the first message).

```csharp
var scheduler = new TestScheduler();
var source = scheduler.CreateHotObservable(
    new Recorded<Notification<long>>(10000000, Notification.CreateOnNext(0L)),
    new Recorded<Notification<long>>(20000000, Notification.CreateOnNext(1L)),
    new Recorded<Notification<long>>(30000000, Notification.CreateOnNext(2L)),
    new Recorded<Notification<long>>(40000000, Notification.CreateOnNext(3L)),
    new Recorded<Notification<long>>(40000000, Notification.CreateOnCompleted<long>())
    );

var testObserver = scheduler.Start(
    () => source,
    0,
    TimeSpan.FromSeconds(1).Ticks,
    TimeSpan.FromSeconds(5).Ticks);

Console.WriteLine("Time is {0} ticks", scheduler.Clock);
Console.WriteLine("Received {0} notifications", testObserver.Messages.Count);

foreach (Recorded<Notification<long>> message in testObserver.Messages)
{
    Console.WriteLine("  {0} @ {1}", message.Value, message.Time);
}
```

Output:

```
Time is 50000000 ticks
Received 4 notifications
OnNext(1) @ 20000000
OnNext(2) @ 30000000
OnNext(3) @ 40000000
OnCompleted() @ 40000000
```

### CreateObserver 

Finally, if you do not want to use the `TestScheduler.Start` methods, and you need more fine-grained control over your observer, you can use `TestScheduler.CreateObserver()`. This will return an `ITestObserver` that you can use to manage the subscriptions to your observable sequences with. Furthermore, you will still be exposed to the recorded messages and any subscribers.

Current industry standards demand broad coverage of automated unit tests to meet quality assurance standards. Concurrent programming, however, is often a difficult area to test well. Rx delivers a well-designed implementation of testing features, allowing deterministic and high-throughput testing. The `TestScheduler` provides methods to control virtual time and produce observable sequences for testing. This ability to easily and reliably test concurrent systems sets Rx apart from many other libraries.