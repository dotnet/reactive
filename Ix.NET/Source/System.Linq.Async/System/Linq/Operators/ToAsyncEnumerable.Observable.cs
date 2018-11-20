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
                throw Error.ArgumentNull(nameof(source));

            return CreateEnumerable(
                ct =>
                {
                    var observer = new ToAsyncEnumerableObserver<TSource>();

                    var subscription = source.Subscribe(observer);

                    // REVIEW: Review possible concurrency issues with Dispose calls.

                    var ctr = ct.Register(subscription.Dispose);

                    return AsyncEnumerator.Create(
                        tcs =>
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

                            return new ValueTask<bool>(tcs.Task);
                        },
                        () => observer.Current,
                        () =>
                        {
                            ctr.Dispose();
                            subscription.Dispose();
                            // Should we cancel in-flight operations somehow?
                            return default;
                        });
                });
        }

        private sealed class ToAsyncEnumerableObserver<T> : IObserver<T>
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

                tcs?.TrySetResult(false);
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

                tcs?.TrySetException(error);
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

                tcs?.TrySetResult(true);
            }
        }
    }
}
