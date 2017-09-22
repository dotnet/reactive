// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return Create<TResult>(observer => AsyncObserver.Generate(observer, initialState, condition, iterate, resultSelector));
        }

        public static IAsyncObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, IAsyncScheduler scheduler)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TResult>(observer => AsyncObserver.Generate(observer, initialState, condition, iterate, resultSelector, scheduler));
        }

        public static IAsyncObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, Task<bool>> condition, Func<TState, Task<TState>> iterate, Func<TState, Task<TResult>> resultSelector)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return Create<TResult>(observer => AsyncObserver.Generate(observer, initialState, condition, iterate, resultSelector));
        }

        public static IAsyncObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, Task<bool>> condition, Func<TState, Task<TState>> iterate, Func<TState, Task<TResult>> resultSelector, IAsyncScheduler scheduler)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TResult>(observer => AsyncObserver.Generate(observer, initialState, condition, iterate, resultSelector, scheduler));
        }

        public static IAsyncObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, TimeSpan> timeSelector)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));

            return Create<TResult>(observer => AsyncObserver.Generate(observer, initialState, condition, iterate, resultSelector, timeSelector));
        }

        public static IAsyncObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, TimeSpan> timeSelector, IAsyncScheduler scheduler)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TResult>(observer => AsyncObserver.Generate(observer, initialState, condition, iterate, resultSelector, timeSelector, scheduler));
        }

        public static IAsyncObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, Task<bool>> condition, Func<TState, Task<TState>> iterate, Func<TState, Task<TResult>> resultSelector, Func<TState, Task<TimeSpan>> timeSelector)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));

            return Create<TResult>(observer => AsyncObserver.Generate(observer, initialState, condition, iterate, resultSelector, timeSelector));
        }

        public static IAsyncObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, Task<bool>> condition, Func<TState, Task<TState>> iterate, Func<TState, Task<TResult>> resultSelector, Func<TState, Task<TimeSpan>> timeSelector, IAsyncScheduler scheduler)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TResult>(observer => AsyncObserver.Generate(observer, initialState, condition, iterate, resultSelector, timeSelector, scheduler));
        }

        public static IAsyncObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, DateTimeOffset> timeSelector)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));

            return Create<TResult>(observer => AsyncObserver.Generate(observer, initialState, condition, iterate, resultSelector, timeSelector));
        }

        public static IAsyncObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, DateTimeOffset> timeSelector, IAsyncScheduler scheduler)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TResult>(observer => AsyncObserver.Generate(observer, initialState, condition, iterate, resultSelector, timeSelector, scheduler));
        }

        public static IAsyncObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, Task<bool>> condition, Func<TState, Task<TState>> iterate, Func<TState, Task<TResult>> resultSelector, Func<TState, Task<DateTimeOffset>> timeSelector)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));

            return Create<TResult>(observer => AsyncObserver.Generate(observer, initialState, condition, iterate, resultSelector, timeSelector));
        }

        public static IAsyncObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, Task<bool>> condition, Func<TState, Task<TState>> iterate, Func<TState, Task<TResult>> resultSelector, Func<TState, Task<DateTimeOffset>> timeSelector, IAsyncScheduler scheduler)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TResult>(observer => AsyncObserver.Generate(observer, initialState, condition, iterate, resultSelector, timeSelector, scheduler));
        }
    }

    partial class AsyncObserver
    {
        public static Task<IAsyncDisposable> Generate<TState, TResult>(IAsyncObserver<TResult> observer, TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return Generate(observer, initialState, s => Task.FromResult(condition(s)), s => Task.FromResult(iterate(s)), s => Task.FromResult(resultSelector(s)), TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> Generate<TState, TResult>(IAsyncObserver<TResult> observer, TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Generate(observer, initialState, s => Task.FromResult(condition(s)), s => Task.FromResult(iterate(s)), s => Task.FromResult(resultSelector(s)), scheduler);
        }

        public static Task<IAsyncDisposable> Generate<TState, TResult>(IAsyncObserver<TResult> observer, TState initialState, Func<TState, Task<bool>> condition, Func<TState, Task<TState>> iterate, Func<TState, Task<TResult>> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return Generate(observer, initialState, condition, iterate, resultSelector, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> Generate<TState, TResult>(IAsyncObserver<TResult> observer, TState initialState, Func<TState, Task<bool>> condition, Func<TState, Task<TState>> iterate, Func<TState, Task<TResult>> resultSelector, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                var first = true;
                var state = initialState;

                while (!ct.IsCancellationRequested)
                {
                    var hasResult = false;
                    var result = default(TResult);

                    try
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            state = await iterate(state).RendezVous(scheduler, ct);
                        }

                        hasResult = await condition(state).RendezVous(scheduler, ct);

                        if (hasResult)
                        {
                            result = await resultSelector(state).RendezVous(scheduler, ct);
                        }
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                        return;
                    }

                    if (hasResult)
                    {
                        await observer.OnNextAsync(result).RendezVous(scheduler, ct);
                    }
                    else
                    {
                        break;
                    }
                }

                if (!ct.IsCancellationRequested)
                {
                    await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                }
            });
        }

        public static Task<IAsyncDisposable> Generate<TState, TResult>(IAsyncObserver<TResult> observer, TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, TimeSpan> timeSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));

            return Generate(observer, initialState, s => Task.FromResult(condition(s)), s => Task.FromResult(iterate(s)), s => Task.FromResult(resultSelector(s)), s => Task.FromResult(timeSelector(s)), TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> Generate<TState, TResult>(IAsyncObserver<TResult> observer, TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, TimeSpan> timeSelector, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Generate(observer, initialState, s => Task.FromResult(condition(s)), s => Task.FromResult(iterate(s)), s => Task.FromResult(resultSelector(s)), s => Task.FromResult(timeSelector(s)), scheduler);
        }

        public static Task<IAsyncDisposable> Generate<TState, TResult>(IAsyncObserver<TResult> observer, TState initialState, Func<TState, Task<bool>> condition, Func<TState, Task<TState>> iterate, Func<TState, Task<TResult>> resultSelector, Func<TState, Task<TimeSpan>> timeSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));

            return Generate(observer, initialState, condition, iterate, resultSelector, timeSelector, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> Generate<TState, TResult>(IAsyncObserver<TResult> observer, TState initialState, Func<TState, Task<bool>> condition, Func<TState, Task<TState>> iterate, Func<TState, Task<TResult>> resultSelector, Func<TState, Task<TimeSpan>> timeSelector, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                var first = true;
                var state = initialState;

                while (!ct.IsCancellationRequested)
                {
                    var hasResult = false;
                    var result = default(TResult);
                    var nextDue = default(TimeSpan);

                    try
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            state = await iterate(state).RendezVous(scheduler, ct);
                        }

                        hasResult = await condition(state).RendezVous(scheduler, ct);

                        if (hasResult)
                        {
                            result = await resultSelector(state).RendezVous(scheduler, ct);
                            nextDue = await timeSelector(state).RendezVous(scheduler, ct);
                        }
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                        return;
                    }

                    if (hasResult)
                    {
                        await observer.OnNextAsync(result).RendezVous(scheduler, ct);
                    }
                    else
                    {
                        break;
                    }

                    await scheduler.Delay(nextDue).RendezVous(scheduler, ct);
                }

                if (!ct.IsCancellationRequested)
                {
                    await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                }
            });
        }

        public static Task<IAsyncDisposable> Generate<TState, TResult>(IAsyncObserver<TResult> observer, TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, DateTimeOffset> timeSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));

            return Generate(observer, initialState, s => Task.FromResult(condition(s)), s => Task.FromResult(iterate(s)), s => Task.FromResult(resultSelector(s)), s => Task.FromResult(timeSelector(s)), TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> Generate<TState, TResult>(IAsyncObserver<TResult> observer, TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, DateTimeOffset> timeSelector, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Generate(observer, initialState, s => Task.FromResult(condition(s)), s => Task.FromResult(iterate(s)), s => Task.FromResult(resultSelector(s)), s => Task.FromResult(timeSelector(s)), scheduler);
        }

        public static Task<IAsyncDisposable> Generate<TState, TResult>(IAsyncObserver<TResult> observer, TState initialState, Func<TState, Task<bool>> condition, Func<TState, Task<TState>> iterate, Func<TState, Task<TResult>> resultSelector, Func<TState, Task<DateTimeOffset>> timeSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));

            return Generate(observer, initialState, condition, iterate, resultSelector, timeSelector, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> Generate<TState, TResult>(IAsyncObserver<TResult> observer, TState initialState, Func<TState, Task<bool>> condition, Func<TState, Task<TState>> iterate, Func<TState, Task<TResult>> resultSelector, Func<TState, Task<DateTimeOffset>> timeSelector, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                var first = true;
                var state = initialState;

                while (!ct.IsCancellationRequested)
                {
                    var hasResult = false;
                    var result = default(TResult);
                    var nextDue = default(DateTimeOffset);

                    try
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            state = await iterate(state).RendezVous(scheduler, ct);
                        }

                        hasResult = await condition(state).RendezVous(scheduler, ct);

                        if (hasResult)
                        {
                            result = await resultSelector(state).RendezVous(scheduler, ct);
                            nextDue = await timeSelector(state).RendezVous(scheduler, ct);
                        }
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                        return;
                    }

                    if (hasResult)
                    {
                        await observer.OnNextAsync(result).RendezVous(scheduler, ct);
                    }
                    else
                    {
                        break;
                    }

                    await scheduler.Delay(nextDue).RendezVous(scheduler, ct);
                }

                if (!ct.IsCancellationRequested)
                {
                    await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                }
            });
        }
    }
}
