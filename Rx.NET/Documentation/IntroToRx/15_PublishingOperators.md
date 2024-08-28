# Publishing operators

Hot sources need to be able to deliver events to multiple subscribers. While we can implement the subscriber tracking ourselves, it can be easier to write an oversimplified source that works only for a single subscriber. And although that won't be a full implementation of `IObservable<T>`, that won't matter if we then use one of Rx's _multicast_ operators to publish it as a multi-subscriber hot source. The example in ["Representing Filesystem Events in Rx"](03_CreatingObservableSequences.md#representing-filesystem-events-in-rx) used this trick, but as you'll see in this chapter there are a few variations on the theme.

## Multicast

Rx offers three operators enabling us to support multiple subscribers using just a single subscription to some underlying source: [`Publish`](#publish), [`PublishLast`](#publishlast), and [`Replay`](#replay). All three of these are wrappers around Rx's `Multicast` operator, which provides the common mechanism at the heart of all of them.

`Multicast` turns any `IObservable<T>` into an `IConnectableObservable<T>` which, as you can see, just adds a `Connect` method:

```csharp
public interface IConnectableObservable<out T> : IObservable<T>
{
    IDisposable Connect();
}
```

Since it derives from `IObservable<T>`, you can call `Subscribe` on an `IConnectableObservable<T>`, but the implementation returned by `Multicast` won't call `Subscribe` on the underlying source when you do that. It only calls `Subscribe` on the underlying source when you call `Connect`. So that we can see this in action, let's define a source that prints out a message each time `Subscribe` is called:

```csharp
IObservable<int> src = Observable.Create<int>(obs =>
{
    Console.WriteLine("Create callback called");
    obs.OnNext(1);
    obs.OnNext(2);
    obs.OnCompleted();
    return Disposable.Empty;
});
```

Since this is only going to be invoked once no matter how many observers subscribe, `Multicast` can't pass on the `IObserver<T>`s handed to its own `Subscribe` method, because there could be any number of them. It uses a [Subject](03_CreatingObservableSequences.md#subjectt) as the single `IObserver<T>` that is passes to the underlying source, and this subject is also responsible for keeping track of all subscribers. If we call `Multicast` directly, we are required to pass in the subject we want to use:

```csharp
IConnectableObservable<int> m = src.Multicast(new Subject<int>());
```

We can now subscribe to this a few times over:

```csharp
m.Subscribe(x => Console.WriteLine($"Sub1: {x}"));
m.Subscribe(x => Console.WriteLine($"Sub2: {x}"));
m.Subscribe(x => Console.WriteLine($"Sub3: {x}"));
```

None of these subscribers will receive anything unless we call `Connect`:

```csharp
m.Connect();
```

**Note**: `Connect` returns an `IDisposable`. Calling `Dispose` on that unsubscribes from the underlying source.

This call to `Connect` causes the following output:

```csharp
Create callback called
Sub1: 1
Sub2: 1
Sub3: 1
Sub1: 2
Sub2: 2
Sub3: 2
```

As you can see, the method we passed to `Create` runs only once, confirming that `Multicast` did only subscribe once, despite us calling `Subscribe` three times over. But each item went to all three subscriptions.

The way `Multicast` works is fairly straightforward: it gets the subject do most of the work. Whenever you call `Subscribe` on an observable returned by `Multicast`, it just calls `Subscribe` on the subject. And when you call `Connect`, it just passes the subject into the underlying source's `Subscribe`. So this code would have had the same effect:

```csharp
var s = new Subject<int>();

s.Subscribe(x => Console.WriteLine($"Sub1: {x}"));
s.Subscribe(x => Console.WriteLine($"Sub2: {x}"));
s.Subscribe(x => Console.WriteLine($"Sub3: {x}"));

src.Subscribe(s);
```

However, an advantage of `Multicast` is that it returns `IConnectableObservable<T>`, and as we'll see later, some other parts of Rx know how to work with this interface.

`Multicast` offers an overload that works in a quite different way: it is intended for scenarios where you want to write a query that uses its source observable twice. For example, we might want to get adjacent pairs of items using `Zip`:

```csharp
IObservable<(int, int)> ps = src.Zip(src.Skip(1));
ps.Subscribe(ps => Console.WriteLine(ps));
```

(Although [`Buffer`](08_Partitioning.md#buffer) might seem like a more obvious way to do this, one advantage of this `Zip` approach is that it will never give us half of a pair. When we ask `Buffer` for pairs, it will give us a single-item buffer when we reach the end, which can require extra code to work around.)

The problem with this approach is that the source will see two subscriptions: one directly from `Zip`, and then a second one through `Skip`. If we were to run the code above, we'd see this output:

```
Create callback called
Create callback called
(1, 2)
```

Our `Create` callback ran twice. The second `Multicast` overload lets us avoid that:

```csharp
IObservable<(int, int)> ps = src.Multicast(() => new Subject<int>(), s => s.Zip(s.Skip(1)));
ps.Subscribe(ps => Console.WriteLine(ps));
```

As the output shows, this avoids the multiple subscriptions:

```csharp
Create callback called
(1, 2)
```

This overload of `Multicast` returns a normal `IObservable<T>`. This means we don't need to call `Connect`. But it also means that each subscription to the resulting `IObservable<T>` causes a subscription to the underlying source. But for the scenario it is designed for this is fine: we're just trying to avoid getting twice as many subscriptions to the underlying source.

The remaining operators defined in this section, `Publish`, `PublishLast`, and `Replay`, are all wrappers around `Multicast`, each supplying a specific type of subject for you.

### Publish

The `Publish` operator calls `Multicast` with a [`Subject<T>`](03_CreatingObservableSequences.md#subjectt). The effect of this is that once you have called `Connect` on the result, any items produced by the source will be delivered to all subscribers. This enables me to replace this earlier example:

```csharp
IConnectableObservable<int> m = src.Multicast(new Subject<int>());
```

with this:

```csharp
IConnectableObservable<int> m = src.Publish();
```

These are exactly equivalent.

Because `Subject<T>` forwards all incoming `OnNext` calls to each of its subscribers immediately, and because it doesn't store any previously made calls, the result is a hot source. If you attach some subscribers before calling `Connect`, and then you attached more subscribers after calling `Connect`, those later subscribers will only receive events that occurred after they subscribed. This example demonstrates that:

```csharp
IConnectableObservable<long> publishedTicks = Observable
    .Interval(TimeSpan.FromSeconds(1))
    .Take(4)
    .Publish();

publishedTicks.Subscribe(x => Console.WriteLine($"Sub1: {x} ({DateTime.Now})"));
publishedTicks.Subscribe(x => Console.WriteLine($"Sub2: {x} ({DateTime.Now})"));

publishedTicks.Connect();
Thread.Sleep(2500);
Console.WriteLine();
Console.WriteLine("Adding more subscribers");
Console.WriteLine();

publishedTicks.Subscribe(x => Console.WriteLine($"Sub3: {x} ({DateTime.Now})"));
publishedTicks.Subscribe(x => Console.WriteLine($"Sub4: {x} ({DateTime.Now})"));
```

The following output shows that we only see output for the Sub3 and Sub4 subscriptions for the final 2 events:

```
Sub1: 0 (10/08/2023 16:04:02)
Sub2: 0 (10/08/2023 16:04:02)
Sub1: 1 (10/08/2023 16:04:03)
Sub2: 1 (10/08/2023 16:04:03)

Adding more subscribers

Sub1: 2 (10/08/2023 16:04:04)
Sub2: 2 (10/08/2023 16:04:04)
Sub3: 2 (10/08/2023 16:04:04)
Sub4: 2 (10/08/2023 16:04:04)
Sub1: 3 (10/08/2023 16:04:05)
Sub2: 3 (10/08/2023 16:04:05)
Sub3: 3 (10/08/2023 16:04:05)
Sub4: 3 (10/08/2023 16:04:05)
```

As with [`Multicast`](#multicast), `Publish` offers an overload that provides per-top-level-subscription multicast. This lets us simplify the example from the end of that section from this:

```csharp
IObservable<(int, int)> ps = src.Multicast(() => new Subject<int>(), s => s.Zip(s.Skip(1)));
ps.Subscribe(ps => Console.WriteLine(ps));
```

to this:

```csharp
IObservable<(int, int)> ps = src.Publish(s => s.Zip(s.Skip(1)));
ps.Subscribe(ps => Console.WriteLine(ps));
```

`Publish` offers overloads that let you specify an initial value. These use [`BehaviorSubject<T>`](03_CreatingObservableSequences.md#behaviorsubjectt) instead of `Subject<T>`. The difference here is that all subscribers will immediately receive a value as soon as they subscribe. If the underlying source hasn't yet produced an item (or if `Connect` hasn't been called, meaning we've not even subscribed to the source yet) they will receive the initial value. And if at least one item has been received from the source, any new subscribers will instantly receive the latest value the source produced, and will then go on to receive any further new values.

### PublishLast

The `PublishLast` operator calls `Multicast` with an [`AsyncSubject<T>`](03_CreatingObservableSequences.md#asyncsubjectt). The effect of this is that the final item produced by the source will be delivered to all subscribers. You still need to call `Connect`. This determines when subscription to the underlying source occurs. But all subscribers will receive the final event regardless of when they subscribe, because `AsyncSubject<T>` remembers the final result. We can see this in action with the following example:

```csharp
IConnectableObservable<long> pticks = Observable
    .Interval(TimeSpan.FromSeconds(0.1))
    .Take(4)
    .PublishLast();

pticks.Subscribe(x => Console.WriteLine($"Sub1: {x} ({DateTime.Now})"));
pticks.Subscribe(x => Console.WriteLine($"Sub2: {x} ({DateTime.Now})"));

pticks.Connect();
Thread.Sleep(3000);
Console.WriteLine();
Console.WriteLine("Adding more subscribers");
Console.WriteLine();

pticks.Subscribe(x => Console.WriteLine($"Sub3: {x} ({DateTime.Now})"));
pticks.Subscribe(x => Console.WriteLine($"Sub4: {x} ({DateTime.Now})"));
```

This creates a source that produces 4 values in the space of 0.4 seconds. It attaches a couple of subscribers to the `IConnectableObservable<T>` returned by `PublishLast` and then immediately calls `Connect`. Then it sleeps for 1 second, which gives the source time to complete. This means that those first two subscribers will receive the one and only value they will ever receive (the last value in the sequence) before that call to `Thread.Sleep` returns. But we then go on to attach two more subscribers. As the output shows, these also receive that same final event:

```
Sub1: 3 (11/14/2023 9:15:46 AM)
Sub2: 3 (11/14/2023 9:15:46 AM)

Adding more subscribers

Sub3: 3 (11/14/2023 9:15:49 AM)
Sub4: 3 (11/14/2023 9:15:49 AM)
```

These last two subscribers receive the value later because they subscribed later, but the `AsyncSubject<T>` created by `PublishLast` is just replaying the final value it received to these late subscribers.

### Replay

The `Replay` operator calls `Multicast` with a [`ReplaySubject<T>`](03_CreatingObservableSequences.md#replaysubjectt). The effect of this is that any subscribers attached before calling `Connect` just receive all events as the underlying source produces them, but any subscribers attached later effectively get to 'catch up', because the `ReplaySubject<T>` remembers events it has already seen, and replays them to new subscribers.

This example is very similar to the one used for `Publish`:

```csharp
IConnectableObservable<long> pticks = Observable
    .Interval(TimeSpan.FromSeconds(1))
    .Take(4)
    .Replay();

pticks.Subscribe(x => Console.WriteLine($"Sub1: {x} ({DateTime.Now})"));
pticks.Subscribe(x => Console.WriteLine($"Sub2: {x} ({DateTime.Now})"));

pticks.Connect();
Thread.Sleep(2500);
Console.WriteLine();
Console.WriteLine("Adding more subscribers");
Console.WriteLine();

pticks.Subscribe(x => Console.WriteLine($"Sub3: {x} ({DateTime.Now})"));
pticks.Subscribe(x => Console.WriteLine($"Sub4: {x} ({DateTime.Now})"));
```

This creates a source that will produce items regularly for 4 seconds. It attaches two subscribers before calling `Connect`. It then waits long enough for the first two events to emerge before attaching two more subscribers. But unlike with `Publish`, those late subscribers will see the events that happened before they subscribed:

```
Sub1: 0 (10/08/2023 16:18:22)
Sub2: 0 (10/08/2023 16:18:22)
Sub1: 1 (10/08/2023 16:18:23)
Sub2: 1 (10/08/2023 16:18:23)

Adding more subscribers

Sub3: 0 (10/08/2023 16:18:24)
Sub3: 1 (10/08/2023 16:18:24)
Sub4: 0 (10/08/2023 16:18:24)
Sub4: 1 (10/08/2023 16:18:24)
Sub1: 2 (10/08/2023 16:18:24)
Sub2: 2 (10/08/2023 16:18:24)
Sub3: 2 (10/08/2023 16:18:24)
Sub4: 2 (10/08/2023 16:18:24)
Sub1: 3 (10/08/2023 16:18:25)
Sub2: 3 (10/08/2023 16:18:25)
Sub3: 3 (10/08/2023 16:18:25)
Sub4: 3 (10/08/2023 16:18:25)
```

They receive them late of course, because they subscribed late. So we see a quick flurry of events reported as `Sub3` and `Sub4` catch up, but once they have caught up, they then receive all further events immediately.

The `ReplaySubject<T>` that enables this behaviour will consume memory to store events. As you may recall, this subject type can be configured to store only a limited number of events, or not to hold onto events older than some specified time limit. The `Replay` operator provides overloads that enable you to configure these kinds of limits.

`Replay` also supports the per-subscription-multicast model I showed for the other `Multicast`-based operators in this section.

## RefCount

We saw in the preceding section that `Multicast` (and also its various wrappers) supports two usage models:

* returning an `IConnectableObservable<T>` to allow top-level control of when subscription to the underlying source occurs
* returning an ordinary `IObservable<T>`, enabling us to avoid unnecessary multiple subscriptions to the source when using a query that uses the source in multiple places (e.g., `s.Zip(s.Take(1))`), but still making one `Subscribe` call to the underlying source for each top-level `Subscribe`

`RefCount` offers a slightly different model. It enables subscription to the underlying source to be triggered by an ordinary `Subscribe`, but can still make just a single call to the underlying source. This might be useful in the AIS example used throughout this book. You might want to attach multiple subscribers to an observable source that reports the location messages broadcast by ships and other vessels, but you would normally want a library presenting an Rx-based API for this to connect only once to any underlying service providing those messages. And you would most likely want it to connect only when at least one subscriber is listening. `RefCount` would be ideal for this because it enables a single source to support multiple subscribers, and for the underlying source to know when we move between the "no subscribers" and "at least one subscriber" states.

To be able to observe how `RefCount` operators, I'm going to use a modified version of the source that reports when subscription occurs:

```csharp
IObservable<int> src = Observable.Create<int>(async obs =>
{
    Console.WriteLine("Create callback called");
    obs.OnNext(1);
    await Task.Delay(250).ConfigureAwait(false);
    obs.OnNext(2);
    await Task.Delay(250).ConfigureAwait(false);
    obs.OnNext(3);
    await Task.Delay(250).ConfigureAwait(false);
    obs.OnNext(4);
    await Task.Delay(100).ConfigureAwait(false);
    obs.OnCompleted();
});
```

Unlike the earlier example, this uses `async` and delays between each `OnNext` to ensure that the main thread has time to set up multiple subscriptions before all the items are produced. We can then wrap this with `RefCount`:

```csharp
IObservable<int> rc = src
    .Publish()
    .RefCount();
```

Notice that I have to call `Publish` first. This is because `RefCount` expects an `IConnectableObservable<T>`. It wants to start the source only when something first subscribes. It will call `Connect` as soon as there's at least one subscriber. Let's try it:

```csharp
rc.Subscribe(x => Console.WriteLine($"Sub1: {x} ({DateTime.Now})"));
rc.Subscribe(x => Console.WriteLine($"Sub2: {x} ({DateTime.Now})"));
Thread.Sleep(600);
Console.WriteLine();
Console.WriteLine("Adding more subscribers");
Console.WriteLine();
rc.Subscribe(x => Console.WriteLine($"Sub3: {x} ({DateTime.Now})"));
rc.Subscribe(x => Console.WriteLine($"Sub4: {x} ({DateTime.Now})"));
```

Here's the output:

```
Create callback called
Sub1: 1 (10/08/2023 16:36:44)
Sub1: 2 (10/08/2023 16:36:45)
Sub2: 2 (10/08/2023 16:36:45)
Sub1: 3 (10/08/2023 16:36:45)
Sub2: 3 (10/08/2023 16:36:45)

Adding more subscribers

Sub1: 4 (10/08/2023 16:36:45)
Sub2: 4 (10/08/2023 16:36:45)
Sub3: 4 (10/08/2023 16:36:45)
Sub4: 4 (10/08/2023 16:36:45)
```

Notice that only `Sub1` receives the very first event. That's because the callback passed to `Create` produces that immediately. Only when it invokes its first `await` does it return to the caller, enabling us to attach the second subscriber. That has already missed the first event, but as you can see it receives the 2nd and 3rd. The code waits long enough for the first three events to occur before attaching two more subscribers, and you can see that all four subscribers receive the final event.

As the name suggests `RefCount` counts the number of active subscribers. If this ever drops to 0, it will call `Dispose` on the object that `Connect` returned, shutting down the subscription. If further subscribers attach, it will restart. This example shows that:

```csharp
IDisposable s1 = rc.Subscribe(x => Console.WriteLine($"Sub1: {x} ({DateTime.Now})"));
IDisposable s2 = rc.Subscribe(x => Console.WriteLine($"Sub2: {x} ({DateTime.Now})"));
Thread.Sleep(600);

Console.WriteLine();
Console.WriteLine("Removing subscribers");
s1.Dispose();
s2.Dispose();
Thread.Sleep(600);
Console.WriteLine();

Console.WriteLine();
Console.WriteLine("Adding more subscribers");
Console.WriteLine();
rc.Subscribe(x => Console.WriteLine($"Sub3: {x} ({DateTime.Now})"));
rc.Subscribe(x => Console.WriteLine($"Sub4: {x} ({DateTime.Now})"));
```

We get this output:

```
Create callback called
Sub1: 1 (10/08/2023 16:40:39)
Sub1: 2 (10/08/2023 16:40:39)
Sub2: 2 (10/08/2023 16:40:39)
Sub1: 3 (10/08/2023 16:40:39)
Sub2: 3 (10/08/2023 16:40:39)

Removing subscribers


Adding more subscribers

Create callback called
Sub3: 1 (10/08/2023 16:40:40)
Sub3: 2 (10/08/2023 16:40:40)
Sub4: 2 (10/08/2023 16:40:40)
Sub3: 3 (10/08/2023 16:40:41)
Sub4: 3 (10/08/2023 16:40:41)
Sub3: 4 (10/08/2023 16:40:41)
Sub4: 4 (10/08/2023 16:40:41)
```

This time, the `Create` callback ran twice. That's because the number of active subscribers dropped to 0, so `RefCount` called `Dispose` to shut things down. When new subscribers came along, it called `Connect` again to start things back up. There are some overloads enabling you to specify a `disconnectDelay`. This tells it to wait for the specified time after the number of subscribers drops to zero before disconnecting, to see if any new subscribers come along. But it will still disconnect if the specified time elapses. If that's not what you want, the next operator might be for you.

## AutoConnect

The `AutoConnect` operator behaves in much the same way as `RefCount`, in that it calls `Connect` on its underlying `IConnectableObservable<T>` when the first subscriber subscribers. The difference is that it doesn't attempt to detect when the number of active subscribers has dropped to zero: once it connects, it remains connected indefinitely, even if it has no subscribers.

Although `AutoConnect` can be convenient, you need to be a little careful, because it can cause leaks: it will never disconnect automatically. It is still possible to tear down the connection it creates: `AutoConnect` accepts an optional argument of type `Action<IDisposable>`. It invokes this when it first connects to the source, passing you the `IDisposable` returned by the source's `Connect` method. You can shut it down by calling `Dispose`.

The operators in this chapter can be useful whenever you have a source that is not well suited do dealing with multiple subscribers. It provides various ways to attach multiple subscribers while only triggering a single `Subscribe` to the underlying source.