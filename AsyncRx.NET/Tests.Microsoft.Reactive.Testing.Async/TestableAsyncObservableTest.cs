// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;

using Microsoft.Reactive.Testing.Async;

namespace Tests.Microsoft.Reactive.Testing.Async;

/// <summary>
/// Tests for the testable hot/cold observables, the recording observer, and the Start
/// harness — the "trust in the harness" layer.
/// </summary>
[TestClass]
public class TestableAsyncObservableTest : AsyncReactiveTest
{
    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void Hot_observable_delivers_messages_at_absolute_times_to_active_subscribers(ExecutionShape shape)
    {
        var scheduler = new TestAsyncScheduler(shape);

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(500, 3),
            OnCompleted<int>(600));

        var res = scheduler.Start(() => xs);

        // The message at 150 precedes the subscription at 200 and is not observed.
        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(500, 3),
            OnCompleted<int>(600));

        xs.Subscriptions.AssertEqual(Subscribe(200, 1000));
    }

    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void Cold_observable_delivers_messages_relative_to_subscription(ExecutionShape shape)
    {
        var scheduler = new TestAsyncScheduler(shape);

        var xs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnCompleted<int>(200));

        var res = scheduler.Start(() => xs);

        res.Messages.AssertEqual(
            OnNext(300, 1),
            OnNext(350, 2),
            OnCompleted<int>(400));

        xs.Subscriptions.AssertEqual(Subscribe(200, 1000));
    }

    [TestMethod]
    public void Cold_observable_stops_delivering_after_disposal()
    {
        var scheduler = new TestAsyncScheduler();

        var xs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(300, 2));

        var res = scheduler.Start(() => xs, disposed: 450);

        res.Messages.AssertEqual(
            OnNext(300, 1));

        xs.Subscriptions.AssertEqual(Subscribe(200, 450));
    }

    [TestMethod]
    public void A_subscription_that_is_never_disposed_records_infinite_dispose_times()
    {
        var scheduler = new TestAsyncScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1));

        var observer = scheduler.CreateObserver<int>();
        scheduler.ScheduleAbsolute(200, async _ =>
        {
            await xs.SubscribeAsync(observer);
        });

        scheduler.Start();

        xs.Subscriptions.AssertEqual(Subscribe(200));
        observer.Messages.AssertEqual(OnNext(210, 1));
    }

    [TestMethod]
    public void Hot_observable_serializes_deliveries_when_the_consumer_prolongs_completion()
    {
        // A hot source must not begin a delivery while a previous one is in flight, so each
        // delivery starts at max(scheduled tick, completion of the previous one). With the
        // consumer prolonging each completion by 40 ticks, the messages scheduled at
        // 210/220/230 are delivered back to back instead.
        var scheduler = new TestAsyncScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3));

        var observer = scheduler.CreateObserver<int>(_ => scheduler.Delay(TimeSpan.FromTicks(40)));

        scheduler.ScheduleAbsolute(200, async _ =>
        {
            await xs.SubscribeAsync(observer);
        });

        scheduler.Start();

        observer.Messages.AssertEqual(
            OnNext((210, 250), 1),
            OnNext((250, 290), 2),
            OnNext((290, 330), 3));
    }

    [TestMethod]
    public void Hot_observable_catches_up_when_the_consumer_stops_prolonging_completion()
    {
        // Only the first delivery is prolonged (40 ticks). The second message, scheduled at
        // 220, is held back to 250 by the in-flight first delivery, but completes instantly.
        // With the backlog drained, the third message (scheduled at 300) is delivered right
        // on schedule again: delivery starts at max(scheduled tick, previous completion),
        // not at "previous completion" unconditionally.
        var scheduler = new TestAsyncScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(300, 3));

        var observer = scheduler.CreateObserver<int>(
            n => n.Value == 1 ? scheduler.Delay(TimeSpan.FromTicks(40)) : default);

        scheduler.ScheduleAbsolute(200, async _ =>
        {
            await xs.SubscribeAsync(observer);
        });

        scheduler.Start();

        observer.Messages.AssertEqual(
            OnNext((210, 250), 1),
            OnNext((250, 250), 2),
            OnNext((300, 300), 3));
    }

    [TestMethod]
    public void Hot_observable_catches_up_when_scheduled_messages_stop_outrunning_the_consumer()
    {
        // The consumer prolongs every delivery by 40 ticks, but the source's own schedule
        // slows down: the first two messages (210, 220) outrun the consumer and back up,
        // while the third is scheduled far enough out (320) that the backlog has drained by
        // then and it is delivered at its scheduled tick.
        var scheduler = new TestAsyncScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(320, 3));

        var observer = scheduler.CreateObserver<int>(_ => scheduler.Delay(TimeSpan.FromTicks(40)));

        scheduler.ScheduleAbsolute(200, async _ =>
        {
            await xs.SubscribeAsync(observer);
        });

        scheduler.Start();

        observer.Messages.AssertEqual(
            OnNext((210, 250), 1),
            OnNext((250, 290), 2),
            OnNext((320, 360), 3));
    }

    [TestMethod]
    public void Execution_shapes_produce_identical_traces_for_full_scenarios()
    {
        var (messages, subscriptions) = RunScenario(ExecutionShape.ForcedYield);
        var (expectedMessages, expectedSubscriptions) = RunScenario(ExecutionShape.SynchronousCompletion);

        CollectionAssert.AreEqual(expectedMessages.ToList(), messages.ToList());
        CollectionAssert.AreEqual(expectedSubscriptions.ToList(), subscriptions.ToList());
    }

    private static (IReadOnlyList<AsyncRecorded<Notification<int>>> Messages, IReadOnlyList<AsyncSubscription> Subscriptions) RunScenario(ExecutionShape shape)
    {
        var scheduler = new TestAsyncScheduler(shape);

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 3),
            OnCompleted<int>(500));

        var res = scheduler.Start(() => xs, disposed: 600);

        return (res.Messages, xs.Subscriptions);
    }

    [TestMethod]
    public void Start_fails_informatively_when_subscribe_has_not_completed_by_the_dispose_tick()
    {
        var scheduler = new TestAsyncScheduler();

        var thrown = Assert.ThrowsExactly<TestAsyncSchedulerException>(() => scheduler.Start<int>(() => new NeverCompletingSubscribe()));

        Assert.Contains("the subscription is not complete", thrown.Message);
        Assert.Contains("called at tick 200", thrown.Message);
    }

    [TestMethod]
    public void Start_honors_custom_created_subscribed_and_disposed_times()
    {
        var scheduler = new TestAsyncScheduler();

        var xs = scheduler.CreateColdObservable(
            OnNext(50, 1),
            OnNext(200, 2));

        var res = scheduler.Start(() => xs, created: 50, subscribed: 300, disposed: 400);

        res.Messages.AssertEqual(
            OnNext(350, 1));

        xs.Subscriptions.AssertEqual(Subscribe(300, 400));
    }

    private sealed class NeverCompletingSubscribe : IAsyncObservable<int>
    {
        public async ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<int> observer)
        {
            await new TaskCompletionSource().Task;
            throw new InvalidOperationException("unreachable");
        }
    }
}
