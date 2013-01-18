// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Linq;

#if !NO_TPL
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
#endif

namespace System.Reactive.Linq
{
#if !NO_PERF
    using Observαble;
#endif

    internal partial class QueryLanguage
    {
        #region - Create -

        public virtual IObservable<TSource> Create<TSource>(Func<IObserver<TSource>, IDisposable> subscribe)
        {
            return new AnonymousObservable<TSource>(subscribe);
        }

        public virtual IObservable<TSource> Create<TSource>(Func<IObserver<TSource>, Action> subscribe)
        {
            return new AnonymousObservable<TSource>(o =>
            {
                var a = subscribe(o);
                return a != null ? Disposable.Create(a) : Disposable.Empty;
            });
        }

        #endregion

        #region - CreateAsync -

#if !NO_TPL
        public virtual IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, CancellationToken, Task> subscribeAsync)
        {
            return new AnonymousObservable<TResult>(observer =>
            {
                var cancellable = new CancellationDisposable();

                var taskObservable = subscribeAsync(observer, cancellable.Token).ToObservable();
                var taskCompletionObserver = new AnonymousObserver<Unit>(Stubs<Unit>.Ignore, observer.OnError, observer.OnCompleted);
                var subscription = taskObservable.Subscribe(taskCompletionObserver);

                return new CompositeDisposable(cancellable, subscription);
            });
        }

        public virtual IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, Task> subscribeAsync)
        {
            return Create<TResult>((observer, token) => subscribeAsync(observer));
        }

        public virtual IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, CancellationToken, Task<IDisposable>> subscribeAsync)
        {
            return new AnonymousObservable<TResult>(observer =>
            {
                var subscription = new SingleAssignmentDisposable();
                var cancellable = new CancellationDisposable();

                var taskObservable = subscribeAsync(observer, cancellable.Token).ToObservable();
                var taskCompletionObserver = new AnonymousObserver<IDisposable>(d => subscription.Disposable = d ?? Disposable.Empty, observer.OnError, Stubs.Nop);

                //
                // We don't cancel the subscription below *ever* and want to make sure the returned resource gets disposed eventually.
                // Notice because we're using the AnonymousObservable<T> type, we get auto-detach behavior for free.
                //
                taskObservable.Subscribe(taskCompletionObserver);

                return new CompositeDisposable(cancellable, subscription);
            });
        }

        public virtual IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, Task<IDisposable>> subscribeAsync)
        {
            return Create<TResult>((observer, token) => subscribeAsync(observer));
        }

        public virtual IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, CancellationToken, Task<Action>> subscribeAsync)
        {
            return new AnonymousObservable<TResult>(observer =>
            {
                var subscription = new SingleAssignmentDisposable();
                var cancellable = new CancellationDisposable();

                var taskObservable = subscribeAsync(observer, cancellable.Token).ToObservable();
                var taskCompletionObserver = new AnonymousObserver<Action>(a => subscription.Disposable = a != null ? Disposable.Create(a) : Disposable.Empty, observer.OnError, Stubs.Nop);

                //
                // We don't cancel the subscription below *ever* and want to make sure the returned resource eventually gets disposed.
                // Notice because we're using the AnonymousObservable<T> type, we get auto-detach behavior for free.
                //
                taskObservable.Subscribe(taskCompletionObserver);

                return new CompositeDisposable(cancellable, subscription);
            });
        }

        public virtual IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, Task<Action>> subscribeAsync)
        {
            return Create<TResult>((observer, token) => subscribeAsync(observer));
        }
#endif

        #endregion

        #region + Defer +

        public virtual IObservable<TValue> Defer<TValue>(Func<IObservable<TValue>> observableFactory)
        {
#if !NO_PERF
            return new Defer<TValue>(observableFactory);
#else
            return new AnonymousObservable<TValue>(observer =>
            {
                IObservable<TValue> result;
                try
                {
                    result = observableFactory();
                }
                catch (Exception exception)
                {
                    return Throw<TValue>(exception).Subscribe(observer);
                }

                return result.Subscribe(observer);
            });
#endif
        }

        #endregion

        #region + DeferAsync +

#if !NO_TPL
        public virtual IObservable<TValue> Defer<TValue>(Func<Task<IObservable<TValue>>> observableFactoryAsync)
        {
            return Defer(() => StartAsync(observableFactoryAsync).Merge());
        }

        public virtual IObservable<TValue> Defer<TValue>(Func<CancellationToken, Task<IObservable<TValue>>> observableFactoryAsync)
        {
            return Defer(() => StartAsync(observableFactoryAsync).Merge());
        }
#endif

        #endregion

        #region + Empty +

        public virtual IObservable<TResult> Empty<TResult>()
        {
#if !NO_PERF
            return new Empty<TResult>(SchedulerDefaults.ConstantTimeOperations);
#else
            return Empty_<TResult>(SchedulerDefaults.ConstantTimeOperations);
#endif
        }

        public virtual IObservable<TResult> Empty<TResult>(IScheduler scheduler)
        {
#if !NO_PERF
            return new Empty<TResult>(scheduler);
#else
            return Empty_<TResult>(scheduler);
#endif
        }

#if NO_PERF
        private static IObservable<TResult> Empty_<TResult>(IScheduler scheduler)
        {
            return new AnonymousObservable<TResult>(observer => scheduler.Schedule(observer.OnCompleted));
        }
#endif

        #endregion

        #region + Generate +

        public virtual IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
#if !NO_PERF
            return new Generate<TState, TResult>(initialState, condition, iterate, resultSelector, SchedulerDefaults.Iteration);
#else
            return Generate_<TState, TResult>(initialState, condition, iterate, resultSelector, SchedulerDefaults.Iteration);
#endif
        }

        public virtual IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, IScheduler scheduler)
        {
#if !NO_PERF
            return new Generate<TState, TResult>(initialState, condition, iterate, resultSelector, scheduler);
#else
            return Generate_<TState, TResult>(initialState, condition, iterate, resultSelector, scheduler);
#endif
        }

#if NO_PERF
        private static IObservable<TResult> Generate_<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, IScheduler scheduler)
        {
            return new AnonymousObservable<TResult>(observer =>
            {
                var state = initialState;
                var first = true;
                return scheduler.Schedule(self =>
                {
                    var hasResult = false;
                    var result = default(TResult);
                    try
                    {
                        if (first)
                            first = false;
                        else
                            state = iterate(state);
                        hasResult = condition(state);
                        if (hasResult)
                            result = resultSelector(state);
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }

                    if (hasResult)
                    {
                        observer.OnNext(result);
                        self();
                    }
                    else
                        observer.OnCompleted();
                });
            });
        }
#endif

        #endregion

        #region + Never +

        public virtual IObservable<TResult> Never<TResult>()
        {
#if !NO_PERF
            return new Never<TResult>();
#else
            return new AnonymousObservable<TResult>(observer => Disposable.Empty);
#endif
        }

        #endregion

        #region + Range +

        public virtual IObservable<int> Range(int start, int count)
        {
            return Range_(start, count, SchedulerDefaults.Iteration);
        }

        public virtual IObservable<int> Range(int start, int count, IScheduler scheduler)
        {
            return Range_(start, count, scheduler);
        }

        private static IObservable<int> Range_(int start, int count, IScheduler scheduler)
        {
#if !NO_PERF
            return new Range(start, count, scheduler);
#else
            return new AnonymousObservable<int>(observer =>
            {
                return scheduler.Schedule(0, (i, self) =>
                {
                    if (i < count)
                    {
                        observer.OnNext(start + i);
                        self(i + 1);
                    }
                    else
                        observer.OnCompleted();
                });
            });
#endif
        }

        #endregion

        #region + Repeat +

        public virtual IObservable<TResult> Repeat<TResult>(TResult value)
        {
#if !NO_PERF
            return new Repeat<TResult>(value, null, SchedulerDefaults.Iteration);
#else
            return Repeat_(value, SchedulerDefaults.Iteration);
#endif
        }

        public virtual IObservable<TResult> Repeat<TResult>(TResult value, IScheduler scheduler)
        {
#if !NO_PERF
            return new Repeat<TResult>(value, null, scheduler);
#else
            return Repeat_<TResult>(value, scheduler);
#endif
        }

#if NO_PERF
        private IObservable<TResult> Repeat_<TResult>(TResult value, IScheduler scheduler)
        {
            return Return(value, scheduler).Repeat();
        }
#endif

        public virtual IObservable<TResult> Repeat<TResult>(TResult value, int repeatCount)
        {
#if !NO_PERF
            return new Repeat<TResult>(value, repeatCount, SchedulerDefaults.Iteration);
#else
            return Repeat_(value, repeatCount, SchedulerDefaults.Iteration);
#endif
        }

        public virtual IObservable<TResult> Repeat<TResult>(TResult value, int repeatCount, IScheduler scheduler)
        {
#if !NO_PERF
            return new Repeat<TResult>(value, repeatCount, scheduler);
#else
            return Repeat_(value, repeatCount, scheduler);
#endif
        }

#if NO_PERF
        private IObservable<TResult> Repeat_<TResult>(TResult value, int repeatCount, IScheduler scheduler)
        {
            return Return(value, scheduler).Repeat(repeatCount);
        }
#endif

        #endregion

        #region + Return +

        public virtual IObservable<TResult> Return<TResult>(TResult value)
        {
#if !NO_PERF
            return new Return<TResult>(value, SchedulerDefaults.ConstantTimeOperations);
#else
            return Return_<TResult>(value, SchedulerDefaults.ConstantTimeOperations);
#endif
        }

        public virtual IObservable<TResult> Return<TResult>(TResult value, IScheduler scheduler)
        {
#if !NO_PERF
            return new Return<TResult>(value, scheduler);
#else
            return Return_<TResult>(value, scheduler);
#endif
        }

#if NO_PERF
        private static IObservable<TResult> Return_<TResult>(TResult value, IScheduler scheduler)
        {
            return new AnonymousObservable<TResult>(observer => 
                scheduler.Schedule(() =>
                {
                    observer.OnNext(value);
                    observer.OnCompleted();
                })
            );
        }
#endif

        #endregion

        #region + Throw +

        public virtual IObservable<TResult> Throw<TResult>(Exception exception)
        {
#if !NO_PERF
            return new Throw<TResult>(exception, SchedulerDefaults.ConstantTimeOperations);
#else
            return Throw_<TResult>(exception, SchedulerDefaults.ConstantTimeOperations);
#endif
        }

        public virtual IObservable<TResult> Throw<TResult>(Exception exception, IScheduler scheduler)
        {
#if !NO_PERF
            return new Throw<TResult>(exception, scheduler);
#else
            return Throw_<TResult>(exception, scheduler);
#endif
        }

#if NO_PERF
        private static IObservable<TResult> Throw_<TResult>(Exception exception, IScheduler scheduler)
        {
            return new AnonymousObservable<TResult>(observer => scheduler.Schedule(() => observer.OnError(exception)));
        }
#endif

        #endregion

        #region + Using +

        public virtual IObservable<TSource> Using<TSource, TResource>(Func<TResource> resourceFactory, Func<TResource, IObservable<TSource>> observableFactory) where TResource : IDisposable
        {
#if !NO_PERF
            return new Using<TSource, TResource>(resourceFactory, observableFactory);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var source = default(IObservable<TSource>);
                var disposable = Disposable.Empty;
                try
                {
                    var resource = resourceFactory();
                    if (resource != null)
                        disposable = resource;
                    source = observableFactory(resource);
                }
                catch (Exception exception)
                {
                    return new CompositeDisposable(Throw<TSource>(exception).Subscribe(observer), disposable);
                }

                return new CompositeDisposable(source.Subscribe(observer), disposable);
            });
#endif
        }

        #endregion

        #region - UsingAsync -

#if !NO_TPL

        public virtual IObservable<TSource> Using<TSource, TResource>(Func<CancellationToken, Task<TResource>> resourceFactoryAsync, Func<TResource, CancellationToken, Task<IObservable<TSource>>> observableFactoryAsync) where TResource : IDisposable
        {
            return Observable.FromAsync<TResource>(resourceFactoryAsync)
                .SelectMany(resource =>
                    Observable.Using<TSource, TResource>(
                        () => resource,
                        resource_ => Observable.FromAsync<IObservable<TSource>>(ct => observableFactoryAsync(resource_, ct)).Merge()
                    )
                );
        }

#endif

        #endregion
    }
}
