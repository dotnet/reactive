# Rx (Reactive Extensions for .NET)

Rx enables event-driven programming with a composable, declarative model.


## Getting started

Run the following at a command line:

```ps1
mkdir TryRx
cd TryRx
dotnet new console
dotnet add package System.Reactive
```

Alternatively, if you have Visual Studio installed, create a new .NET Console project, and then use the NuGet package manager to add a reference to `System.Reactive`.

You can then add this code to your `Program.cs`. This creates an observable source (`ticks`) that produces an event once every second. It also adds a handler to that source that writes a message to the console for each event:

```cs
using System.Reactive.Linq;

IObservable<long> ticks = Observable.Timer(
    dueTime: TimeSpan.Zero,
    period: TimeSpan.FromSeconds(1));

ticks.Subscribe(
    tick => Console.WriteLine($"Tick {tick}"));

Console.ReadLine();
```

## Examples

### Wrapping an existing event source as an Rx `IObservable<T>`

If you have an existing source of events that does not support Rx directly, but which does offer .NET events, you can bring this into the world of Rx using the `Observable.FromEventPattern` method:

```cs
using System.Reactive.Linq;

FileSystemWatcher fsw = new FileSystemWatcher(@"C:\temp");

IObservable<FileSystemEventArgs> changeEvents = Observable
    .FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
        h => fsw.Changed += h,
        h => fsw.Changed -= h)
    .Select(e => e.EventArgs);

fsw.EnableRaisingEvents = true;
```


### Waiting for inactivity

It can sometimes be useful to wait for a period of inactivity before taking action. For example, if you have code that monitors a directory in a filesystem, processing modified or added files, it's common for there to be flurries of activity. For example, if a user is copying multiple files into a folder that you're observing, there will be multiple changes, and it could be more efficient to wait until those stop and then process all the changes in one batch, than to attempt to process everything immediately.

This example defines a custom Rx operator that can be attached to any source. It will wait for that source to start producing events, and then, it will wait for it to stop again for the specified period. Each time that happens, it reports all of the activity that occurred between the last two periods of inactivity:

```cs
static class RxExt
{
    public static IObservable<IList<T>> Quiescent<T>(
        this IObservable<T> src,
        TimeSpan minimumInactivityPeriod,
        IScheduler scheduler)
    {
        IObservable<int> onoffs =
            from _ in src
            from delta in Observable.Return(1, scheduler).Concat(Observable.Return(-1, scheduler).Delay(minimumInactivityPeriod, scheduler))
            select delta;
        IObservable<int> outstanding = onoffs.Scan(0, (total, delta) => total + delta);
        IObservable<int> zeroCrossings = outstanding.Where(total => total == 0);
        return src.Buffer(zeroCrossings);
    }
}
```

(This works by creating a sequence (`onoffs`) that produces a value 1 each time activity occurs, and then a corresponding -1 after the specified time has elapsed. It then uses `Scan` to produce the `outstanding` sequence, which is just a running total of those `onoffs`. This is effectively a count of the number of events that have happened recently (where 'recently' is defined as 'less than `minimumInactivityPeriod` ago). Every new event that occurs raises this running total by 1, but each time the specified timespan has passed for a particular event, it drops by one. So when this drops back to 0, it means that there are no events that have occurred as recently as the `minimumInactivityPeriod`. The `zeroCrossings` sequence picks out just the events in which `outstanding` drops back to zero. This has the effect that `zeroCrossings` raises an event every time there has been some activity followed by `minimumInactivityPeriod` of inactivity. Finally, we plug this into the `Buffer` operator, which slices the input events (`src`) into chunks. By passing it the `zeroCrossings` source, we tell `Buffer` to deliver a new slice every time the source becomes inactive. The effect is that the source returned by `Quiescent` does nothing until there has been some activity followed by the specified period of inactivity, at which point it produces a single event reporting all of the source events that have occurred since the previous period, or in the initial case, all of the source events so far.)

You could use this in conjunction with the adapted `FileSystemWatcher` from the preceding example:

```cs
IObservable<IList<FileSystemEventArgs>> fileActivityStopped = changeEvents
    .Quiescent(TimeSpan.FromSeconds(2), Scheduler.Default);

await fileActivityStopped.ForEachAsync(
    a => Console.WriteLine($"File changes stopped after {a.Count} changes"));
```

(Note: this only uses the `Changed` event. A real application might also need to look at the `FileSystemWatcher`'s `Created`, `Renamed`, and `Deleted` events.)

## Feedback

You can create issues at the https://github.com/dotnet/reactive repository