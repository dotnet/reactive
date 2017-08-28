// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    using ObservableImpl;

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

        public virtual IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, CancellationToken, Task> subscribeAsync)
        {
            return new AnonymousObservable<TResult>(observer =>
            {
                var cancellable = new CancellationDisposable();

                var taskObservable = subscribeAsync(observer, cancellable.Token).ToObservable();
                var taskCompletionObserver = new AnonymousObserver<Unit>(Stubs<Unit>.Ignore, observer.OnError, observer.OnCompleted);
                var subscription = taskObservable.Subscribe(taskCompletionObserver);

                return StableCompositeDisposable.Create(cancellable, subscription);
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

                return StableCompositeDisposable.Create(cancellable, subscription);
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

                return StableCompositeDisposable.Create(cancellable, subscription);
            });
        }

        public virtual IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, Task<Action>> subscribeAsync)
        {
            return Create<TResult>((observer, token) => subscribeAsync(observer));
        }

        #endregion

        #region + Defer +

        public virtual IObservable<TValue> Defer<TValue>(Func<IObservable<TValue>> observableFactory)
        {
            return new Defer<TValue>(observableFactory);
        }

        #endregion

        #region + DeferAsync +

        public virtual IObservable<TValue> Defer<TValue>(Func<Task<IObservable<TValue>>> observableFactoryAsync)
        {
            return Defer(() => StartAsync(observableFactoryAsync).Merge());
        }

        public virtual IObservable<TValue> Defer<TValue>(Func<CancellationToken, Task<IObservable<TValue>>> observableFactoryAsync)
        {
            return Defer(() => StartAsync(observableFactoryAsync).Merge());
        }

        #endregion

        #region + Empty +

        public virtual IObservable<TResult> Empty<TResult>()
        {
            return new Empty<TResult>(SchedulerDefaults.ConstantTimeOperations);
        }

        public virtual IObservable<TResult> Empty<TResult>(IScheduler scheduler)
        {
            return new Empty<TResult>(scheduler);
        }

        #endregion

        #region + Generate +

        public virtual IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
            return new Generate<TState, TResult>.NoTime(initialState, condition, iterate, resultSelector, SchedulerDefaults.Iteration);
        }

        public virtual IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, IScheduler scheduler)
        {
            return new Generate<TState, TResult>.NoTime(initialState, condition, iterate, resultSelector, scheduler);
        }

        #endregion

        #region + Never +

        public virtual IObservable<TResult> Never<TResult>()
        {
            return new Never<TResult>();
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
            return new Range(start, count, scheduler);
        }

        #endregion

        #region + Repeat +

        public virtual IObservable<TResult> Repeat<TResult>(TResult value)
        {
            return new Repeat<TResult>.Forever(value, SchedulerDefaults.Iteration);
        }

        public virtual IObservable<TResult> Repeat<TResult>(TResult value, IScheduler scheduler)
        {
            return new Repeat<TResult>.Forever(value, scheduler);
        }

        public virtual IObservable<TResult> Repeat<TResult>(TResult value, int repeatCount)
        {
            return new Repeat<TResult>.Count(value, repeatCount, SchedulerDefaults.Iteration);
        }

        public virtual IObservable<TResult> Repeat<TResult>(TResult value, int repeatCount, IScheduler scheduler)
        {
            return new Repeat<TResult>.Count(value, repeatCount, scheduler);
        }

        #endregion

        #region + Return +

        public virtual IObservable<TResult> Return<TResult>(TResult value)
        {
            return new Return<TResult>(value, SchedulerDefaults.ConstantTimeOperations);
        }

        public virtual IObservable<TResult> Return<TResult>(TResult value, IScheduler scheduler)
        {
            return new Return<TResult>(value, scheduler);
        }

        #endregion

        #region + Throw +

        public virtual IObservable<TResult> Throw<TResult>(Exception exception)
        {
            return new Throw<TResult>(exception, SchedulerDefaults.ConstantTimeOperations);
        }

        public virtual IObservable<TResult> Throw<TResult>(Exception exception, IScheduler scheduler)
        {
            return new Throw<TResult>(exception, scheduler);
        }

        #endregion

        #region + Using +

        public virtual IObservable<TSource> Using<TSource, TResource>(Func<TResource> resourceFactory, Func<TResource, IObservable<TSource>> observableFactory) where TResource : IDisposable
        {
            return new Using<TSource, TResource>(resourceFactory, observableFactory);
        }

        #endregion

        #region - UsingAsync -

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

        #endregion
    }
}
