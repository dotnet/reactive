// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Create(() =>
            {
                var e = source.GetEnumerator();

                return Create(
                    ct => Task.Factory.StartNew(() =>
                    {
                        var res = default(bool);
                        try
                        {
                            res = e.MoveNext();
                        }
                        finally
                        {
                            if (!res)
                                e.Dispose();
                        }
                        return res;
                    }, ct),
                    () => e.Current,
                    () => e.Dispose()
                );
            });
        }

        public static IEnumerable<TSource> ToEnumerable<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return ToEnumerable_(source);
        }

        private static IEnumerable<TSource> ToEnumerable_<TSource>(IAsyncEnumerable<TSource> source)
        {
            using (var e = source.GetEnumerator())
            {
                while (true)
                {
                    var t = e.MoveNext(CancellationToken.None);
                    t.Wait();
                    if (!t.Result)
                        break;
                    var c = e.Current;
                    yield return c;
                }
            }
        }

        public static IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this Task<TSource> task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            return Create(() =>
            {
                var called = 0;

                return Create(
                    (ct, tcs) =>
                    {
                        if (Interlocked.CompareExchange(ref called, 1, 0) == 0)
                        {
                            task.Then(continuedTask =>
                            {
                                if (continuedTask.IsCanceled)
                                    tcs.SetCanceled();
                                else if (continuedTask.IsFaulted)
                                    tcs.SetException(continuedTask.Exception.InnerException);
                                else
                                    tcs.SetResult(true);
                            });
                        }
                        else
                            tcs.SetResult(false);

                        return tcs.Task;
                    },
                    () => task.Result,
                    () => { });
            });
        }

#if !NO_RXINTERFACES
        public static IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Create(() =>
            {
                var observer = new ToAsyncEnumerableObserver<TSource>();

                var subscription = source.Subscribe(observer);

                return Create(
                    (ct, tcs) =>
                    {
                        var hasValue = false;
                        var hasCompleted = false;
                        var error = default(Exception);

                        lock (observer.SyncRoot)
                        {
                            if (observer.Values.Count > 0)
                            {
                                hasValue = true;
                                observer.Current = observer.Values.Dequeue();
                            }
                            else if (observer.HasCompleted)
                            {
                                hasCompleted = true;
                            }
                            else if (observer.Error != null)
                            {
                                error = observer.Error;
                            }
                            else
                            {
                                observer.TaskCompletionSource = tcs;
                            }
                        }

                        if (hasValue)
                        {
                            tcs.TrySetResult(true);
                        }
                        else if (hasCompleted)
                        {
                            tcs.TrySetResult(false);
                        }
                        else if (error != null)
                        {
                            tcs.TrySetException(error);
                        }

                        return tcs.Task;
                    },
                    () => observer.Current,
                    () =>
                    {
                        subscription.Dispose();
                        // Should we cancel in-flight operations somehow?
                    });
            });
        }

        class ToAsyncEnumerableObserver<T> : IObserver<T>
        {
            public ToAsyncEnumerableObserver()
            {
                Values = new Queue<T>();
            }

            public object SyncRoot
            {
                get { return Values; }
            }

            public readonly Queue<T> Values;
            public Exception Error;
            public bool HasCompleted;

            public T Current;
            public TaskCompletionSource<bool> TaskCompletionSource;

            public void OnCompleted()
            {
                var tcs = default(TaskCompletionSource<bool>);

                lock (SyncRoot)
                {
                    HasCompleted = true;

                    if (TaskCompletionSource != null)
                    {
                        tcs = TaskCompletionSource;
                        TaskCompletionSource = null;
                    }
                }

                if (tcs != null)
                {
                    tcs.SetResult(false);
                }
            }

            public void OnError(Exception error)
            {
                var tcs = default(TaskCompletionSource<bool>);

                lock (SyncRoot)
                {
                    Error = error;

                    if (TaskCompletionSource != null)
                    {
                        tcs = TaskCompletionSource;
                        TaskCompletionSource = null;
                    }
                }

                if (tcs != null)
                {
                    tcs.SetException(error);
                }
            }

            public void OnNext(T value)
            {
                var tcs = default(TaskCompletionSource<bool>);

                lock (SyncRoot)
                {
                    if (TaskCompletionSource == null)
                    {
                        Values.Enqueue(value);
                    }
                    else
                    {
                        Current = value;

                        tcs = TaskCompletionSource;
                        TaskCompletionSource = null;
                    }
                }

                if (tcs != null)
                {
                    tcs.SetResult(true);
                }
            }
        }

        public static IObservable<TSource> ToObservable<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new ToObservableObservable<TSource>(source);
        }

        class ToObservableObservable<T> : IObservable<T>
        {
            private readonly IAsyncEnumerable<T> source;

            public ToObservableObservable(IAsyncEnumerable<T> source)
            {
                this.source = source;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                var ctd = new CancellationTokenDisposable();
                var e = source.GetEnumerator();

                var f = default(Action);
                f = () => e.MoveNext(ctd.Token).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        observer.OnError(t.Exception);
                        e.Dispose();
                    }
                    else if (t.IsCanceled)
                    {
                        e.Dispose();
                    }
                    else if (t.IsCompleted)
                    {
                        if (t.Result)
                        {
                            observer.OnNext(e.Current);
                            f();
                        }
                        else
                        {
                            observer.OnCompleted();
                            e.Dispose();
                        }
                    }
                }, ctd.Token);

                f();

                return Disposable.Create(ctd, e);
            }
        }
#endif
    }
}
