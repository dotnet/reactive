// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive
{
    internal abstract class TailRecursiveSink<TSource> : IdentitySink<TSource>
    {
        public TailRecursiveSink(IObserver<TSource> observer)
            : base(observer)
        {
        }

        bool _isDisposed;

        int trampoline;

        IDisposable currentSubscription;

        Stack<IEnumerator<IObservable<TSource>>> stack;

        public void Run(IEnumerable<IObservable<TSource>> sources)
        {
            if (!TryGetEnumerator(sources, out var current))
                return;

            stack = new Stack<IEnumerator<IObservable<TSource>>>();
            stack.Push(current);

            Drain();

            SetUpstream(new RecursiveSinkDisposable(this));
        }

        sealed class RecursiveSinkDisposable : IDisposable
        {
            readonly TailRecursiveSink<TSource> parent;

            public RecursiveSinkDisposable(TailRecursiveSink<TSource> parent)
            {
                this.parent = parent;
            }

            public void Dispose()
            {
                parent.DisposeAll();
            }
        }

        void Drain()
        {
            if (Interlocked.Increment(ref trampoline) != 1)
            {
                return;
            }

            for (; ; )
            {
                if (Volatile.Read(ref _isDisposed))
                {
                    while (stack.Count != 0)
                    {
                        var enumerator = stack.Pop();
                        enumerator.Dispose();
                    }

                    Disposable.TryDispose(ref currentSubscription);
                }
                else
                {
                    if (stack.Count != 0)
                    {
                        var currentEnumerator = stack.Peek();

                        var currentObservable = default(IObservable<TSource>);
                        var next = default(IObservable<TSource>);

                        try
                        {
                            if (currentEnumerator.MoveNext())
                            {
                                currentObservable = currentEnumerator.Current;
                            }
                        }
                        catch (Exception ex)
                        {
                            currentEnumerator.Dispose();
                            ForwardOnError(ex);
                            Volatile.Write(ref _isDisposed, true);
                            continue;
                        }

                        try
                        {
                            next = Helpers.Unpack(currentObservable);

                        }
                        catch (Exception ex)
                        {
                            next = null;
                            if (!Fail(ex))
                            {
                                Volatile.Write(ref _isDisposed, true);
                            }
                            continue;
                        }

                        if (next != null)
                        {
                            var nextSeq = Extract(next);
                            if (nextSeq != null)
                            {
                                if (TryGetEnumerator(nextSeq, out var nextEnumerator))
                                {
                                    stack.Push(nextEnumerator);
                                    continue;
                                }
                                else
                                {
                                    Volatile.Write(ref _isDisposed, true);
                                    continue;
                                }
                            }
                            else
                            {
                                var sad = new SingleAssignmentDisposable();

                                if (Disposable.TrySetSingle(ref currentSubscription, sad) == TrySetSingleResult.Success)
                                {
                                    sad.Disposable = next.SubscribeSafe(this);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            stack.Pop();
                            currentEnumerator.Dispose();
                            continue;
                        }
                    }
                    else
                    {
                        Volatile.Write(ref _isDisposed, true);
                        Done();
                    }
                }

                if (Interlocked.Decrement(ref trampoline) == 0)
                {
                    break;
                }
            }
        }

        void DisposeAll()
        {
            Volatile.Write(ref _isDisposed, true);
            // the disposing of currentSubscription is deferred to drain due to some ObservableExTest.Iterate_Complete()
            // Interlocked.Exchange(ref currentSubscription, BooleanDisposable.True)?.Dispose();
            Drain();
        }

        protected void Recurse()
        {
            if (Disposable.TrySetSerial(ref currentSubscription, null))
                Drain();
        }

        protected abstract IEnumerable<IObservable<TSource>> Extract(IObservable<TSource> source);

        private bool TryGetEnumerator(IEnumerable<IObservable<TSource>> sources, out IEnumerator<IObservable<TSource>> result)
        {
            try
            {
                result = sources.GetEnumerator();
                return true;
            }
            catch (Exception exception)
            {
                ForwardOnError(exception);

                result = null;
                return false;
            }
        }

        protected virtual void Done()
        {
            ForwardOnCompleted();
        }

        protected virtual bool Fail(Exception error)
        {
            ForwardOnError(error);

            return false;
        }
    }
}
