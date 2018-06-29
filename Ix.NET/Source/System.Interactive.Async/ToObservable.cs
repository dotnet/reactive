// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return CreateEnumerable(
                () =>
                {
                    var observer = new ToAsyncEnumerableObserver<TSource>();

                    var subscription = source.Subscribe(observer);

                    return CreateEnumerator(
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

        public static IObservable<TSource> ToObservable<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new ToObservableObservable<TSource>(source);
        }

        private class ToAsyncEnumerableObserver<T> : IObserver<T>
        {
            public readonly Queue<T> Values;

            public T Current;
            public Exception Error;
            public bool HasCompleted;
            public TaskCompletionSource<bool> TaskCompletionSource;

            public ToAsyncEnumerableObserver()
            {
                Values = new Queue<T>();
            }

            public object SyncRoot
            {
                get { return Values; }
            }

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
                    tcs.TrySetResult(false);
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
                    tcs.TrySetException(error);
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
                    tcs.TrySetResult(true);
                }
            }
        }

        private class ToObservableObservable<T> : IObservable<T>
        {
            private readonly IAsyncEnumerable<T> _source;

            public ToObservableObservable(IAsyncEnumerable<T> source)
            {
                _source = source;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                var ctd = new CancellationTokenDisposable();
                var e = _source.GetEnumerator();

                void f() => e.MoveNext(ctd.Token)
                           .ContinueWith(t =>
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

                                                     if (!ctd.Token.IsCancellationRequested)
                                                     {
                                                         f();
                                                     }

                                                     //In case cancellation is requested, this could only have happened
                                                     //by disposing the returned composite disposable (see below).
                                                     //In that case, e will be disposed too, so there is no need to dispose e here.
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
    }
}
