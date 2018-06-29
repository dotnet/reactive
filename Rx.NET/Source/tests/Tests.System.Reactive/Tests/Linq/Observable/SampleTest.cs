// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class SampleTest : ReactiveTest
    {

        [Fact]
        public void Sample_ArgumentChecking()
        {
            var scheduler = new TestScheduler();
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sample(default(IObservable<int>), TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sample(default(IObservable<int>), TimeSpan.Zero, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sample(someObservable, TimeSpan.Zero, null));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Sample(someObservable, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Sample(someObservable, TimeSpan.FromSeconds(-1), scheduler));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sample(default(IObservable<int>), someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sample(someObservable, default(IObservable<int>)));
        }

        [Fact]
        public void Sample_Regular()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(260, 4),
                OnNext(300, 5),
                OnNext(350, 6),
                OnNext(380, 7),
                OnCompleted<int>(390)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnNext(300, 5), /* CHECK: boundary of sampling */
                OnNext(350, 6),
                OnNext(400, 7), /* Sample in last bucket */
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );
        }

        [Fact]
        public void Sample_Periodic_Regular()
        {
            var scheduler = new PeriodicTestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(260, 4),
                OnNext(300, 5),
                OnNext(350, 6),
                OnNext(380, 7),
                OnCompleted<int>(390)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnNext(300, 5), /* CHECK: boundary of sampling */
                OnNext(350, 6),
                OnNext(400, 7), /* Sample in last bucket */
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );

#if !WINDOWS
            scheduler.Timers.AssertEqual(
                new TimerRun(200, 400) { 250, 300, 350, 400 }
            );
#endif
        }

        [Fact]
        public void Sample_ErrorInFlight()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(260, 4),
                OnNext(300, 5),
                OnNext(310, 6),
                OnError<int>(330, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnNext(300, 5), /* CHECK: boundary of sampling */
                OnError<int>(330, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 330)
            );
        }

        [Fact]
        public void Sample_Periodic_ErrorInFlight()
        {
            var scheduler = new PeriodicTestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(260, 4),
                OnNext(300, 5),
                OnNext(310, 6),
                OnError<int>(330, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnNext(300, 5), /* CHECK: boundary of sampling */
                OnError<int>(330, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 330)
            );

#if !WINDOWS
            scheduler.Timers.AssertEqual(
                new TimerRun(200, 330) { 250, 300 }
            );
#endif
        }

        [Fact]
        public void Sample_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void Sample_Periodic_Empty()
        {
            var scheduler = new PeriodicTestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

#if !WINDOWS
            scheduler.Timers.AssertEqual(
                new TimerRun(200, 300) { 250, 300 }
            );
#endif
        }

        [Fact]
        public void Sample_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void Sample_Periodic_Error()
        {
            var scheduler = new PeriodicTestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

#if !WINDOWS
            scheduler.Timers.AssertEqual(
                new TimerRun(200, 300) { 250 }
            );
#endif
        }

        [Fact]
        public void Sample_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Sample_Periodic_Never()
        {
            var scheduler = new PeriodicTestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

#if !WINDOWS
            scheduler.Timers.AssertEqual(
                new TimerRun(200, 1000) { 250, 300, 350, 400, 450, 500, 550, 600, 650, 700, 750, 800, 850, 900, 950 }
            );
#endif
        }

        [Fact]
        public void Sample_DefaultScheduler_Periodic()
        {
            var res = Observable.Return(42).Sample(TimeSpan.FromMilliseconds(1)).ToEnumerable().Single();
            Assert.Equal(42, res);
        }

        [Fact]
        public void Sample_DefaultScheduler_PeriodicDisabled()
        {
            var res = Observable.Return(42).Sample(TimeSpan.FromMilliseconds(1), Scheduler.Default.DisableOptimizations()).ToEnumerable().Single();
            Assert.Equal(42, res);
        }

        [Fact]
        public void Sample_Sampler_Simple1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(300, 5),
                OnNext(310, 6),
                OnCompleted<int>(400)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(150, ""),
                OnNext(210, "bar"),
                OnNext(250, "foo"),
                OnNext(260, "qux"),
                OnNext(320, "baz"),
                OnCompleted<string>(500)
            );

            var res = scheduler.Start(() =>
                xs.Sample(ys)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnNext(320, 6),
                OnCompleted<int>(500 /* on sampling boundaries only */)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [Fact]
        public void Sample_Sampler_Simple2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(300, 5),
                OnNext(310, 6),
                OnNext(360, 7),
                OnCompleted<int>(400)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(150, ""),
                OnNext(210, "bar"),
                OnNext(250, "foo"),
                OnNext(260, "qux"),
                OnNext(320, "baz"),
                OnCompleted<string>(500)
            );

            var res = scheduler.Start(() =>
                xs.Sample(ys)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnNext(320, 6),
                OnNext(500, 7),
                OnCompleted<int>(500 /* on sampling boundaries only */)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [Fact]
        public void Sample_Sampler_Simple3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(150, ""),
                OnNext(210, "bar"),
                OnNext(250, "foo"),
                OnNext(260, "qux"),
                OnNext(320, "baz"),
                OnCompleted<string>(500)
            );

            var res = scheduler.Start(() =>
                xs.Sample(ys)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnNext(320, 4),
                OnCompleted<int>(320 /* on sampling boundaries only */)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 320)
            );
        }

        [Fact]
        public void Sample_Sampler_completes_first()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnCompleted<int>(600)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(150, ""),
                OnNext(210, "bar"),
                OnNext(250, "foo"),
                OnNext(260, "qux"),
                OnNext(320, "baz"),
                OnCompleted<string>(500)
            );

            var res = scheduler.Start(() =>
                xs.Sample(ys)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnNext(320, 4),
                OnCompleted<int>(600 /* on sampling boundaries only */)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [Fact]
        public void Sample_Sampler_SourceThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(300, 5),
                OnNext(310, 6),
                OnError<int>(320, ex)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(150, ""),
                OnNext(210, "bar"),
                OnNext(250, "foo"),
                OnNext(260, "qux"),
                OnNext(330, "baz"),
                OnCompleted<string>(400)
            );

            var res = scheduler.Start(() =>
                xs.Sample(ys)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnError<int>(320, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 320)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 320)
            );
        }

#if !NO_PERF // BREAKING CHANGE v2 > v1.x - behavior when sampler throws
        [Fact]
        public void Sample_Sampler_SamplerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(300, 5),
                OnNext(310, 6),
                OnCompleted<int>(400)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(150, ""),
                OnNext(210, "bar"),
                OnNext(250, "foo"),
                OnNext(260, "qux"),
                OnError<string>(320, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sample(ys)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnError<int>(320, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 320)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 320)
            );
        }
#endif

    }
}
