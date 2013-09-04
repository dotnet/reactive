// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;

#if !NO_TPL
using System.Threading.Tasks;
#endif

namespace System.Reactive.Linq
{
#if !NO_PERF
    using Observαble;
#endif

    internal partial class QueryLanguage
    {
        #region ForEachAsync

#if !NO_TPL
        public virtual Task ForEachAsync<TSource>(IObservable<TSource> source, Action<TSource> onNext)
        {
            return ForEachAsync_(source, onNext, CancellationToken.None);
        }

        public virtual Task ForEachAsync<TSource>(IObservable<TSource> source, Action<TSource> onNext, CancellationToken cancellationToken)
        {
            return ForEachAsync_(source, onNext, cancellationToken);
        }

        public virtual Task ForEachAsync<TSource>(IObservable<TSource> source, Action<TSource, int> onNext)
        {
            var i = 0;
            return ForEachAsync_(source, x => onNext(x, checked(i++)), CancellationToken.None);
        }

        public virtual Task ForEachAsync<TSource>(IObservable<TSource> source, Action<TSource, int> onNext, CancellationToken cancellationToken)
        {
            var i = 0;
            return ForEachAsync_(source, x => onNext(x, checked(i++)), cancellationToken);
        }

        private static Task ForEachAsync_<TSource>(IObservable<TSource> source, Action<TSource> onNext, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>();
            var subscription = new SingleAssignmentDisposable();

            var ctr = default(CancellationTokenRegistration);

            if (cancellationToken.CanBeCanceled)
            {
                ctr = cancellationToken.Register(() =>
                {
                    tcs.TrySetCanceled();
                    subscription.Dispose();
                });
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                // Making sure we always complete, even if disposing throws.
                var dispose = new Action<Action>(action =>
                {
                    try
                    {
                        ctr.Dispose(); // no null-check needed (struct)
                        subscription.Dispose();
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                        return;
                    }

                    action();
                });

                var taskCompletionObserver = new AnonymousObserver<TSource>(
                    x =>
                    {
                        if (!subscription.IsDisposed)
                        {
                            try
                            {
                                onNext(x);
                            }
                            catch (Exception exception)
                            {
                                dispose(() => tcs.TrySetException(exception));
                            }
                        }
                    },
                    exception =>
                    {
                        dispose(() => tcs.TrySetException(exception));
                    },
                    () =>
                    {
                        dispose(() => tcs.TrySetResult(null));
                    }
                );

                //
                // Subtle race condition: if the source completes before we reach the line below, the SingleAssigmentDisposable
                // will already have been disposed. Upon assignment, the disposable resource being set will be disposed on the
                // spot, which may throw an exception. (See TFS 487142)
                //
                try
                {
                    //
                    // [OK] Use of unsafe Subscribe: we're catching the exception here to set the TaskCompletionSource.
                    //
                    // Notice we could use a safe subscription to route errors through OnError, but we still need the
                    // exception handling logic here for the reason explained above. We cannot afford to throw here
                    // and as a result never set the TaskCompletionSource, so we tunnel everything through here.
                    //
                    subscription.Disposable = source.Subscribe/*Unsafe*/(taskCompletionObserver);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }

            return tcs.Task;
        }
#endif

        #endregion

        #region + Case +

        public virtual IObservable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IObservable<TResult>> sources)
        {
            return Case(selector, sources, Empty<TResult>());
        }

        public virtual IObservable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IObservable<TResult>> sources, IScheduler scheduler)
        {
            return Case(selector, sources, Empty<TResult>(scheduler));
        }

        public virtual IObservable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IObservable<TResult>> sources, IObservable<TResult> defaultSource)
        {
#if !NO_PERF
            return new Case<TValue, TResult>(selector, sources, defaultSource);
#else
            return Observable.Defer(() =>
            {
                IObservable<TResult> result;
                if (!sources.TryGetValue(selector(), out result))
                    result = defaultSource;
                return result;
            });
#endif
        }

        #endregion

        #region + DoWhile +

        public virtual IObservable<TSource> DoWhile<TSource>(IObservable<TSource> source, Func<bool> condition)
        {
#if !NO_PERF
            return new DoWhile<TSource>(source, condition);
#else
            return source.Concat(While(condition, source));
#endif
        }

        #endregion

        #region + For +

        public virtual IObservable<TResult> For<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, IObservable<TResult>> resultSelector)
        {
#if !NO_PERF
            return new For<TSource, TResult>(source, resultSelector);
#else
            return ForCore(source, resultSelector).Concat();
#endif
        }

#if NO_PERF
        static IEnumerable<IObservable<TResult>> ForCore<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, IObservable<TResult>> resultSelector)
        {
            foreach (var item in source)
                yield return resultSelector(item);
        }
#endif

        #endregion

        #region + If +

        public virtual IObservable<TResult> If<TResult>(Func<bool> condition, IObservable<TResult> thenSource)
        {
            return If(condition, thenSource, Empty<TResult>());
        }

        public virtual IObservable<TResult> If<TResult>(Func<bool> condition, IObservable<TResult> thenSource, IScheduler scheduler)
        {
            return If(condition, thenSource, Empty<TResult>(scheduler));
        }

        public virtual IObservable<TResult> If<TResult>(Func<bool> condition, IObservable<TResult> thenSource, IObservable<TResult> elseSource)
        {
#if !NO_PERF
            return new If<TResult>(condition, thenSource, elseSource);
#else
            return Observable.Defer(() => condition() ? thenSource : elseSource);
#endif
        }

        #endregion

        #region + While +

        public virtual IObservable<TSource> While<TSource>(Func<bool> condition, IObservable<TSource> source)
        {
#if !NO_PERF
            return new While<TSource>(condition, source);
#else
            return WhileCore(condition, source).Concat();
#endif
        }

#if NO_PERF
        static IEnumerable<IObservable<TSource>> WhileCore<TSource>(Func<bool> condition, IObservable<TSource> source)
        {
            while (condition())
                yield return source;
        }
#endif

        #endregion
    }
}
