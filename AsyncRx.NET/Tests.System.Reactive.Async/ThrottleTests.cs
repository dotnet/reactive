using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reactive.Async.Testing;
using Microsoft.Reactive.Testing;
using Xunit;

namespace Tests.System.Reactive.Async.Linq
{
    public class ThrottleTests : ReactiveTest
    {
        private IEnumerable<Recorded<Notification<T>>> Generate<T, S>(S seed, Func<S, bool> condition, Func<S, S> iterate, Func<S, Recorded<Notification<T>>> selector, Func<S, Recorded<Notification<T>>> final)
        {
            S s;
            for (s = seed; condition(s); s = iterate(s))
            {
                yield return selector(s);
            }

            yield return final(s);
        }

        [Fact]
        public async Task Throttle_TimeSpan_AllPass()
        {
            var scheduler = new TestAsyncScheduler();

            var xs = await scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnNext(300, 4),
                OnCompleted<int>(400)
            );

            var res = await scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(20), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(230, 1),
                OnNext(260, 2),
                OnNext(290, 3),
                OnNext(320, 4),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public async Task DoesThing()
        {
            var scheduler = new TestAsyncScheduler();

            var xs = await scheduler.CreateHotObservable(
                OnNext(100, 0),
                OnCompleted<int>(800)
            );

            var res = await scheduler.Start(() =>
                xs
            );

            res.Messages.AssertEqual(xs.Messages);
        }
    }
}
