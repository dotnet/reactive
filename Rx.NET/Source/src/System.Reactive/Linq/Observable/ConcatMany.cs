// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class ConcatMany<T> : IObservable<T>
    {
        private readonly IObservable<IObservable<T>> _sources;

        internal ConcatMany(IObservable<IObservable<T>> sources)
        {
            _sources = sources;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }
            var parent = new ConcatManyOuterObserver(observer);
            var d = _sources.SubscribeSafe(parent);
            parent.OnSubscribe(d);
            return parent;
        }

        internal sealed class ConcatManyOuterObserver : IObserver<IObservable<T>>, IDisposable
        {
            private readonly IObserver<T> _downstream;
            private readonly ConcurrentQueue<IObservable<T>> _queue;
            private readonly InnerObserver _innerObserver;
            private IDisposable _upstream;
            private int _trampoline;
            private Exception _error;
            private bool _done;
            private int _active;

            internal ConcatManyOuterObserver(IObserver<T> downstream)
            {
                _downstream = downstream;
                _queue = new ConcurrentQueue<IObservable<T>>();
                _innerObserver = new InnerObserver(this);
            }

            internal void OnSubscribe(IDisposable d)
            {
                Disposable.SetSingle(ref _upstream, d);
            }

            public void Dispose()
            {
                _innerObserver.Dispose();
                DisposeMain();
            }

            private void DisposeMain()
            {
                Disposable.TryDispose(ref _upstream);
            }

            private bool IsDisposed()
            {
                return Disposable.GetIsDisposed(ref _upstream);
            }

            public void OnCompleted()
            {
                Volatile.Write(ref _done, true);
                Drain();
            }

            public void OnError(Exception error)
            {
                if (Interlocked.CompareExchange(ref _error, error, null) == null)
                {
                    Volatile.Write(ref _done, true);
                    Drain();
                }
            }

            public void OnNext(IObservable<T> value)
            {
                _queue.Enqueue(value);
                Drain();
            }

            private void InnerNext(T item)
            {
                _downstream.OnNext(item);
            }

            private void InnerError(Exception error)
            {
                if (_innerObserver.Finish())
                {
                    if (Interlocked.CompareExchange(ref _error, error, null) == null)
                    {
                        Volatile.Write(ref _done, true);
                        Volatile.Write(ref _active, 0);
                        Drain();
                    }
                }
            }

            private void InnerComplete()
            {
                if (_innerObserver.Finish())
                {
                    Volatile.Write(ref _active, 0);
                    Drain();
                }
            }

            private void Drain()
            {
                if (Interlocked.Increment(ref _trampoline) != 1)
                {
                    return;
                }

                do
                {
                    if (IsDisposed())
                    {
                        while (_queue.TryDequeue(out _))
                        {
                        }
                    }
                    else
                    {
                        if (Volatile.Read(ref _active) == 0)
                        {
                            var isDone = Volatile.Read(ref _done);

                            if (isDone)
                            {
                                var ex = Volatile.Read(ref _error);
                                if (ex != null)
                                {
                                    _downstream.OnError(ex);
                                    DisposeMain();
                                    continue;
                                }
                            }

                            if (_queue.TryDequeue(out var source))
                            {
                                var sad = new SingleAssignmentDisposable();
                                if (_innerObserver.SetDisposable(sad))
                                {
                                    Interlocked.Exchange(ref _active, 1);
                                    sad.Disposable = source.SubscribeSafe(_innerObserver);
                                }
                            }
                            else
                            {
                                if (isDone)
                                {
                                    _downstream.OnCompleted();
                                    DisposeMain();
                                }
                            }
                        }
                    }
                } while (Interlocked.Decrement(ref _trampoline) != 0);
            }

            internal sealed class InnerObserver : IObserver<T>, IDisposable
            {
                private readonly ConcatManyOuterObserver _parent;

                internal IDisposable Upstream;

                internal InnerObserver(ConcatManyOuterObserver parent)
                {
                    _parent = parent;
                }

                internal bool SetDisposable(SingleAssignmentDisposable sad)
                {
                    return Disposable.TrySetSingle(ref Upstream, sad) == TrySetSingleResult.Success;
                }

                internal bool Finish()
                {
                    var sad = Volatile.Read(ref Upstream);
                    if (sad != BooleanDisposable.True)
                    {
                        if (Interlocked.CompareExchange(ref Upstream, null, sad) == sad)
                        {
                            sad.Dispose();
                            return true;
                        }
                    }
                    return false;
                }

                public void Dispose()
                {
                    Disposable.TryDispose(ref Upstream);
                }

                public void OnCompleted()
                {
                    _parent.InnerComplete();
                }

                public void OnError(Exception error)
                {
                    _parent.InnerError(error);
                }

                public void OnNext(T value)
                {
                    _parent.InnerNext(value);
                }
            }
        }
    }
}
