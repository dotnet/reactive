// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IObservable<TSource> ToObservable<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new ToObservableObservable<TSource>(source);
        }

        private sealed class ToObservableObservable<T> : IObservable<T>
        {
            private readonly IAsyncEnumerable<T> _source;

            public ToObservableObservable(IAsyncEnumerable<T> source)
            {
                _source = source;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                var enumerator = new ToObservableEnumerator(observer);
                enumerator.Source = _source.GetAsyncEnumerator(enumerator.Token);
                enumerator.MoveNext();
                return enumerator;
            }

            private sealed class ToObservableEnumerator : IDisposable
            {
                private readonly IObserver<T> _downstream;

                private readonly CancellationTokenSource _cts;


                internal IAsyncEnumerator<T> Source;

                private int _wip;

                private int _dispose;

                private TaskCompletionSource<bool> _disposeTask;

                internal CancellationToken Token => _cts.Token;

                public ToObservableEnumerator(IObserver<T> downstream)
                {
                    _downstream = downstream;
                    _cts = new CancellationTokenSource();
                }

                internal void MoveNext()
                {
                    if (Interlocked.Increment(ref _wip) == 1)
                    {
                        do
                        {
                            if (Interlocked.Increment(ref _dispose) == 1)
                            {
                                Source.MoveNextAsync()
                                    .AsTask()
                                    .ContinueWith((t, state) => ((ToObservableEnumerator)state).NextHandler(t), this, _cts.Token);
                            }
                            else
                            {
                                break;
                            }
                        }
                        while (Interlocked.Decrement(ref _wip) != 0);
                    }
                }

                private void NextHandler(Task<bool> t)
                {
                    if (t.IsCanceled)
                    {
                        // Nothing to do.
                        TryDispose();
                    }
                    else if (t.IsFaulted)
                    {
                        if (TryDispose() && !_cts.IsCancellationRequested)
                        {
                            _downstream.OnError(t.Exception);
                        }
                    }
                    else if (t.Result)
                    {
                        if (!_cts.IsCancellationRequested)
                        {
                            var value = Source.Current;
                            if (TryDispose())
                            {
                                _downstream.OnNext(value);
                                MoveNext();
                            }
                        }
                    }
                    else
                    {
                        if (TryDispose() && !_cts.IsCancellationRequested)
                        {
                            _downstream.OnCompleted();
                        }
                    }
                }

                private bool TryDispose()
                {
                    if (Interlocked.Decrement(ref _dispose) != 0)
                    {
                        Dispose(Source);
                        return false;
                    }
                    return true;
                }

                private void Dispose(IAsyncDisposable disposable)
                {
                    disposable.DisposeAsync()
                        .AsTask()
                        .ContinueWith((t, state) => ((ToObservableEnumerator)state).HandleDispose(t), this);
                }

                private void HandleDispose(Task t)
                {
                    if (t.IsFaulted)
                    {
                        GetOrCreateDisposeTask().TrySetException(t.Exception);
                    }
                    else if (t.IsCompleted)
                    {
                        GetOrSetTrueDisposeTask();
                    }
                }

                public void Dispose()
                {
                    _cts.Cancel();
                    if (Interlocked.Increment(ref _dispose) == 1)
                    {
                        Source.DisposeAsync().AsTask().Wait();
                    }
                    else
                    {
                        GetOrCreateDisposeTask().Task.Wait();
                    }
                }

                private TaskCompletionSource<bool> GetOrCreateDisposeTask()
                {
                    var tcs = Volatile.Read(ref _disposeTask);
                    if (tcs != null)
                    {
                        return tcs;
                    }
                    var next = new TaskCompletionSource<bool>();
                    var old = Interlocked.CompareExchange(ref _disposeTask, next, null);

                    return old ?? next;
                }

                private void GetOrSetTrueDisposeTask()
                {
                    var tcs = Volatile.Read(ref _disposeTask);
                    if (tcs != null)
                    {
                        tcs.TrySetResult(true);
                    }
                    var next = TaskExt.ResumeTrue;
                    var old = Interlocked.CompareExchange(ref _disposeTask, next, null);

                    old?.TrySetResult(true);
                }
            }
        }
    }
}
