using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class RetryWhen<T, U> : IObservable<T>
    {
        readonly IObservable<T> source;

        readonly Func<IObservable<Exception>, IObservable<U>> handler;

        internal RetryWhen(IObservable<T> source, Func<IObservable<Exception>, IObservable<U>> handler)
        {
            this.source = source;
            this.handler = handler;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            var errorSignals = new Subject<Exception>();
            var redo = default(IObservable<U>);

            try
            {
                redo = handler(errorSignals);
                if (redo == null)
                {
                    throw new NullReferenceException("The handler returned a null IObservable");
                }
            }
            catch (Exception ex)
            {
                observer.OnError(ex);
                return Disposable.Empty;
            }

            var parent = new MainObserver(observer, source, new SerializedObserver(errorSignals));

            var d = redo.SubscribeSafe(parent.handlerObserver);
            parent.handlerObserver.OnSubscribe(d);

            parent.HandlerNext();

            return parent;
        }

        sealed class MainObserver : IObserver<T>, IDisposable
        {
            readonly IObserver<T> downstream;

            readonly IObserver<Exception> errorSignal;

            internal readonly HandlerObserver handlerObserver;

            readonly IObservable<T> source;

            SingleAssignmentDisposable upstream;

            int trampoline;

            int halfSerializer;

            Exception error;

            static readonly SingleAssignmentDisposable DISPOSED;

            static MainObserver()
            {
                DISPOSED = new SingleAssignmentDisposable();
                DISPOSED.Dispose();
            }

            internal MainObserver(IObserver<T> downstream, IObservable<T> source, IObserver<Exception> errorSignal)
            {
                this.downstream = downstream;
                this.source = source;
                this.errorSignal = errorSignal;
                this.handlerObserver = new HandlerObserver(this);
            }

            public void Dispose()
            {
                Interlocked.Exchange(ref upstream, DISPOSED)?.Dispose();
                handlerObserver.Dispose();
            }

            public void OnCompleted()
            {
                if (Interlocked.Increment(ref halfSerializer) == 1)
                {
                    downstream.OnCompleted();
                    Dispose();
                }
            }

            public void OnError(Exception error)
            {
                for (; ; )
                {
                    var d = Volatile.Read(ref upstream);
                    if (d == DISPOSED)
                    {
                        break;
                    }
                    if (Interlocked.CompareExchange(ref upstream, null, d) == d)
                    {
                        errorSignal.OnNext(error);
                        d.Dispose();
                        break;
                    }
                }
            }

            public void OnNext(T value)
            {
                if (Interlocked.CompareExchange(ref halfSerializer, 1, 0) == 0)
                {
                    downstream.OnNext(value);
                    if (Interlocked.Decrement(ref halfSerializer) != 0)
                    {
                        var ex = error;
                        if (ex == null)
                        {
                            downstream.OnCompleted();
                        }
                        else
                        {
                            downstream.OnError(ex);
                        }
                        Dispose();
                    }
                }
            }

            internal void HandlerError(Exception error)
            {
                this.error = error;
                if (Interlocked.Increment(ref halfSerializer) == 1)
                {
                    downstream.OnError(error);
                    Dispose();
                }
            }

            internal void HandlerComplete()
            {
                if (Interlocked.Increment(ref halfSerializer) == 1)
                {
                    downstream.OnCompleted();
                    Dispose();
                }
            }

            internal void HandlerNext()
            {
                if (Interlocked.Increment(ref trampoline) == 1)
                {
                    do
                    {
                        var sad = new SingleAssignmentDisposable();
                        if (Interlocked.CompareExchange(ref upstream, sad, null) != null)
                        {
                            return;
                        }

                        sad.Disposable = source.SubscribeSafe(this);
                    }
                    while (Interlocked.Decrement(ref trampoline) != 0);
                }
            }

            internal sealed class HandlerObserver : IObserver<U>, IDisposable
            {
                readonly MainObserver main;

                IDisposable upstream;

                internal HandlerObserver(MainObserver main)
                {
                    this.main = main;
                }

                internal void OnSubscribe(IDisposable d)
                {
                    if (Interlocked.CompareExchange(ref upstream, d, null) != null)
                    {
                        d?.Dispose();
                    }
                }

                public void Dispose()
                {
                    Disposable.TryDispose(ref upstream);
                }

                public void OnCompleted()
                {
                    main.HandlerComplete();
                    Dispose();
                }

                public void OnError(Exception error)
                {
                    main.HandlerError(error);
                    Dispose();
                }

                public void OnNext(U value)
                {
                    main.HandlerNext();
                }
            }
        }

        sealed class SerializedObserver : IObserver<Exception>
        {
            readonly IObserver<Exception> downstream;

            int wip;

            Exception terminalException;

            static readonly Exception DONE = new Exception();

            static readonly Exception SIGNALED = new Exception();

            readonly ConcurrentQueue<Exception> queue;

            internal SerializedObserver(IObserver<Exception> downstream)
            {
                this.downstream = downstream;
                this.queue = new ConcurrentQueue<Exception>();
            }

            public void OnCompleted()
            {
                if (Interlocked.CompareExchange(ref terminalException, DONE, null) == null)
                {
                    Drain();
                }
            }

            public void OnError(Exception error)
            {
                if (Interlocked.CompareExchange(ref terminalException, error, null) == null)
                {
                    Drain();
                }
            }

            public void OnNext(Exception value)
            {
                queue.Enqueue(value);
                Drain();
            }

            void Clear()
            {
                while (queue.TryDequeue(out var _)) ;
            }

            void Drain()
            {
                if (Interlocked.Increment(ref wip) != 1)
                {
                    return;
                }

                int missed = 1;

                for (; ; )
                {
                    var ex = Volatile.Read(ref terminalException);
                    if (ex != null)
                    {
                        if (ex != SIGNALED)
                        {
                            Interlocked.Exchange(ref terminalException, SIGNALED);
                            if (ex != DONE)
                            {
                                downstream.OnError(ex);
                            }
                            else
                            {
                                downstream.OnCompleted();
                            }
                        }
                        Clear();
                    }
                    else
                    {
                        while (queue.TryDequeue(out var item))
                        {
                            downstream.OnNext(item);
                        }
                    }
                        

                    missed = Interlocked.Add(ref wip, -missed);
                    if (missed == 0)
                    {
                        break;
                    }
                }
            }
        }
    }
}
