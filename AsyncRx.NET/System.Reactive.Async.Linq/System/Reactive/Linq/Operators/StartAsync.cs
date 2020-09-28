// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> StartAsync<TSource>(Func<ValueTask<TSource>> functionAsync) => StartAsync(functionAsync, ImmediateAsyncScheduler.Instance);

        public static IAsyncObservable<TSource> StartAsync<TSource>(Func<ValueTask<TSource>> functionAsync, IAsyncScheduler scheduler)
        {
            if (functionAsync == null)
                throw new ArgumentNullException(nameof(functionAsync));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            Task<TSource> task;

            try
            {
                task = functionAsync().AsTask();
            }
            catch (Exception ex)
            {
                return Throw<TSource>(ex);
            }

            return task.ToAsyncObservable(scheduler);
        }

        public static IAsyncObservable<TSource> StartAsync<TSource>(Func<CancellationToken, ValueTask<TSource>> functionAsync) => StartAsync(functionAsync, ImmediateAsyncScheduler.Instance);

        public static IAsyncObservable<TSource> StartAsync<TSource>(Func<CancellationToken, ValueTask<TSource>> functionAsync, IAsyncScheduler scheduler)
        {
            if (functionAsync == null)
                throw new ArgumentNullException(nameof(functionAsync));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var cancel = new CancellationAsyncDisposable();

            var task = default(Task<TSource>);

            try
            {
                task = functionAsync(cancel.Token).AsTask();
            }
            catch (Exception ex)
            {
                return Throw<TSource>(ex);
            }

            return Create<TSource>(async observer =>
            {
                var subscription = await task.ToAsyncObservable(scheduler).SubscribeAsync(observer).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(cancel, subscription);
            });
        }

        public static IAsyncObservable<Unit> StartAsync(Func<Task> actionAsync) => StartAsync(actionAsync, ImmediateAsyncScheduler.Instance);

        public static IAsyncObservable<Unit> StartAsync(Func<Task> actionAsync, IAsyncScheduler scheduler)
        {
            if (actionAsync == null)
                throw new ArgumentNullException(nameof(actionAsync));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            Task task;

            try
            {
                task = actionAsync();
            }
            catch (Exception ex)
            {
                return Throw<Unit>(ex);
            }

            return task.ToAsyncObservable(scheduler);
        }

        public static IAsyncObservable<Unit> StartAsync(Func<CancellationToken, Task> actionAsync) => StartAsync(actionAsync, ImmediateAsyncScheduler.Instance);

        public static IAsyncObservable<Unit> StartAsync(Func<CancellationToken, Task> actionAsync, IAsyncScheduler scheduler)
        {
            if (actionAsync == null)
                throw new ArgumentNullException(nameof(actionAsync));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var cancel = new CancellationAsyncDisposable();

            var task = default(Task);

            try
            {
                task = actionAsync(cancel.Token);
            }
            catch (Exception ex)
            {
                return Throw<Unit>(ex);
            }

            return Create<Unit>(async observer =>
            {
                var subscription = await task.ToAsyncObservable(scheduler).SubscribeAsync(observer).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(cancel, subscription);
            });
        }
    }
}
