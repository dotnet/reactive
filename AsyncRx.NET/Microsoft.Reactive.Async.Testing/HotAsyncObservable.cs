using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace Microsoft.Reactive.Async.Testing
{
    internal class HotAsyncObservable<T> : ITestableAsyncObservable<T>
    {
        private readonly TestAsyncScheduler _scheduler;
        private readonly List<IAsyncObserver<T>> _observers;
        private readonly List<Subscription> _subscriptions = new List<Subscription>();

        private HotAsyncObservable(
            TestAsyncScheduler scheduler,
            List<IAsyncObserver<T>> observers,
            params Recorded<Notification<T>>[] messages)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _observers = observers;
            Messages = messages;
        }

        public IReadOnlyList<Subscription> Subscriptions { get => _subscriptions; }

        public IReadOnlyList<Recorded<Notification<T>>> Messages { get; }

        public Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));

            _observers.Add(observer);
            _subscriptions.Add(new Subscription(_scheduler.Clock));
            var index = Subscriptions.Count - 1;

            return Task.FromResult(AsyncDisposable.Create(() =>
            {
                _observers.Remove(observer);
                _subscriptions[index] = new Subscription(Subscriptions[index].Subscribe, _scheduler.Clock);
                return Task.CompletedTask;
            }));
        }

        public static async Task<ITestableAsyncObservable<T>> Create(TestAsyncScheduler scheduler, params Recorded<Notification<T>>[] messages)
        {
            var observers = new List<IAsyncObserver<T>>();

            for (var i = 0; i < messages.Length; ++i)
            {
                var notification = messages[i].Value;
                await scheduler.ScheduleAbsolute(messages[i].Time, async cancel =>
                {
                    var _observers = observers.ToArray();
                    for (var j = 0; j < _observers.Length; ++j)
                    {
                        await notification.AcceptAsync(_observers[j]).ConfigureAwait(false);
                    }
                });
            }

            return new HotAsyncObservable<T>(scheduler, observers, messages);
        }
    }
}
