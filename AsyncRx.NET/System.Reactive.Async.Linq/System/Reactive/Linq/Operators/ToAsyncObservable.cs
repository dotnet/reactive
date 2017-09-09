// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> ToAsyncObservable<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return ToAsyncObservable(source, TaskPoolAsyncScheduler.Default, TaskPoolAsyncScheduler.Default);
        }

        public static IAsyncObservable<TSource> ToAsyncObservable<TSource>(this IObservable<TSource> source, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return ToAsyncObservable(source, scheduler, scheduler);
        }

        public static IAsyncObservable<TSource> ToAsyncObservable<TSource>(this IObservable<TSource> source, IAsyncScheduler subscribeScheduler, IAsyncScheduler disposeScheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (subscribeScheduler == null)
                throw new ArgumentNullException(nameof(subscribeScheduler));
            if (disposeScheduler == null)
                throw new ArgumentNullException(nameof(disposeScheduler));

            return Create<TSource>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var subscribeTask = await subscribeScheduler.ScheduleAsync(async ct =>
                {
                    ct.ThrowIfCancellationRequested();

                    var disposable = source.Subscribe(AsyncObserver.ToObserver(observer));

                    var disposeTask = AsyncDisposable.Create(() => disposeScheduler.ExecuteAsync(_ =>
                    {
                        disposable.Dispose();
                        return Task.CompletedTask;
                    }));

                    await d.AddAsync(disposeTask).RendezVous(subscribeScheduler);
                }).ConfigureAwait(false);

                await d.AddAsync(subscribeTask).ConfigureAwait(false);

                return d;
            });
        }
    }

    partial class AsyncObserver
    {
        // REVIEW: Add a way to parameterize blocking behavior (e.g. blocking, fire-and-forget, async chaining).

        public static IObserver<TSource> ToObserver<TSource>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return new AsyncToSyncObserver<TSource>(observer);
        }

        private sealed class AsyncToSyncObserver<T> : IObserver<T>
        {
            private readonly IAsyncObserver<T> _observer;

            public AsyncToSyncObserver(IAsyncObserver<T> observer)
            {
                _observer = observer;
            }

            public void OnCompleted() => _observer.OnCompletedAsync().GetAwaiter().GetResult();

            public void OnError(Exception error) => _observer.OnErrorAsync(error).GetAwaiter().GetResult();

            public void OnNext(T value) => _observer.OnNextAsync(value).GetAwaiter().GetResult();
        }
    }
}
