// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Merge<TSource>
    {
        internal sealed class ObservablesMaxConcurrency : Producer<TSource, ObservablesMaxConcurrency._>
        {
            private readonly IObservable<IObservable<TSource>> _sources;
            private readonly int _maxConcurrent;

            public ObservablesMaxConcurrency(IObservable<IObservable<TSource>> sources, int maxConcurrent)
            {
                _sources = sources;
                _maxConcurrent = maxConcurrent;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(_maxConcurrent, observer);

            protected override void Run(_ sink) => sink.Run(_sources);

            internal sealed class _ : Sink<IObservable<TSource>, TSource>
            {
                private readonly int _maxConcurrent;

                public _(int maxConcurrent, IObserver<TSource> observer)
                    : base(observer)
                {
                    _maxConcurrent = maxConcurrent;
                }

                private readonly object _gate = new object();
                private readonly Queue<IObservable<TSource>> _q = new Queue<IObservable<TSource>>();
                private volatile bool _isStopped;
                private readonly CompositeDisposable _group = new CompositeDisposable();
                private int _activeCount;

                public override void OnNext(IObservable<TSource> value)
                {
                    lock (_gate)
                    {
                        if (_activeCount < _maxConcurrent)
                        {
                            _activeCount++;
                            Subscribe(value);
                        }
                        else
                        {
                            _q.Enqueue(value);
                        }
                    }
                }

                public override void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    lock (_gate)
                    {
                        _isStopped = true;
                        if (_activeCount == 0)
                        {
                            ForwardOnCompleted();
                        }
                        else
                        {
                            DisposeUpstream();
                        }
                    }
                }

                protected override void Dispose(bool disposing)
                {
                    base.Dispose(disposing);

                    if (disposing)
                    {
                        _group.Dispose();
                    }
                }

                private void Subscribe(IObservable<TSource> innerSource)
                {
                    var innerObserver = new InnerObserver(this);
                    _group.Add(innerObserver);
                    innerObserver.SetResource(innerSource.SubscribeSafe(innerObserver));
                }

                private sealed class InnerObserver : SafeObserver<TSource>
                {
                    private readonly _ _parent;

                    public InnerObserver(_ parent)
                    {
                        _parent = parent;
                    }

                    public override void OnNext(TSource value)
                    {
                        lock (_parent._gate)
                        {
                            _parent.ForwardOnNext(value);
                        }
                    }

                    public override void OnError(Exception error)
                    {
                        lock (_parent._gate)
                        {
                            _parent.ForwardOnError(error);
                        }
                    }

                    public override void OnCompleted()
                    {
                        _parent._group.Remove(this);
                        lock (_parent._gate)
                        {
                            if (_parent._q.Count > 0)
                            {
                                var s = _parent._q.Dequeue();
                                _parent.Subscribe(s);
                            }
                            else
                            {
                                _parent._activeCount--;
                                if (_parent._isStopped && _parent._activeCount == 0)
                                {
                                    _parent.ForwardOnCompleted();
                                }
                            }
                        }
                    }
                }
            }
        }

        internal sealed class Observables : Producer<TSource, Observables._>
        {
            private readonly IObservable<IObservable<TSource>> _sources;

            public Observables(IObservable<IObservable<TSource>> sources)
            {
                _sources = sources;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(observer);

            protected override void Run(_ sink) => sink.Run(_sources);

            internal sealed class _ : Sink<IObservable<TSource>, TSource>
            {
                public _(IObserver<TSource> observer)
                    : base(observer)
                {
                }

                private readonly object _gate = new object();
                private volatile bool _isStopped;
                private readonly CompositeDisposable _group = new CompositeDisposable();

                public override void OnNext(IObservable<TSource> value)
                {
                    var innerObserver = new InnerObserver(this);
                    _group.Add(innerObserver);
                    innerObserver.SetResource(value.SubscribeSafe(innerObserver));
                }

                public override void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    _isStopped = true;
                    if (_group.Count == 0)
                    {
                        //
                        // Notice there can be a race between OnCompleted of the source and any
                        // of the inner sequences, where both see _group.Count == 1, and one is
                        // waiting for the lock. There won't be a double OnCompleted observation
                        // though, because the call to Dispose silences the observer by swapping
                        // in a NopObserver<T>.
                        //
                        lock (_gate)
                        {
                            ForwardOnCompleted();
                        }
                    }
                    else
                    {
                        DisposeUpstream();
                    }
                }

                protected override void Dispose(bool disposing)
                {
                    base.Dispose(disposing);

                    if (disposing)
                    {
                        _group.Dispose();
                    }
                }

                private sealed class InnerObserver : SafeObserver<TSource>
                {
                    private readonly _ _parent;

                    public InnerObserver(_ parent)
                    {
                        _parent = parent;
                    }

                    public override void OnNext(TSource value)
                    {
                        lock (_parent._gate)
                        {
                            _parent.ForwardOnNext(value);
                        }
                    }

                    public override void OnError(Exception error)
                    {
                        lock (_parent._gate)
                        {
                            _parent.ForwardOnError(error);
                        }
                    }

                    public override void OnCompleted()
                    {
                        _parent._group.Remove(this);
                        if (_parent._isStopped && _parent._group.Count == 0)
                        {
                            //
                            // Notice there can be a race between OnCompleted of the source and any
                            // of the inner sequences, where both see _group.Count == 1, and one is
                            // waiting for the lock. There won't be a double OnCompleted observation
                            // though, because the call to Dispose silences the observer by swapping
                            // in a NopObserver<T>.
                            //
                            lock (_parent._gate)
                            {
                                _parent.ForwardOnCompleted();
                            }
                        }
                    }
                }
            }
        }

        internal sealed class Tasks : Producer<TSource, Tasks._>
        {
            private readonly IObservable<Task<TSource>> _sources;

            public Tasks(IObservable<Task<TSource>> sources)
            {
                _sources = sources;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(observer);

            protected override void Run(_ sink) => sink.Run(_sources);

            internal sealed class _ : Sink<Task<TSource>, TSource>
            {
                public _(IObserver<TSource> observer)
                    : base(observer)
                {
                }

                private readonly object _gate = new object();
                private readonly CancellationTokenSource _cts = new CancellationTokenSource();
                private volatile int _count = 1;

                public override void OnNext(Task<TSource> value)
                {
                    Interlocked.Increment(ref _count);
                    if (value.IsCompleted)
                    {
                        OnCompletedTask(value);
                    }
                    else
                    {
                        value.ContinueWith((t, thisObject) => ((_)thisObject).OnCompletedTask(t), this, _cts.Token);
                    }
                }

                private void OnCompletedTask(Task<TSource> task)
                {
                    switch (task.Status)
                    {
                        case TaskStatus.RanToCompletion:
                        {
                            lock (_gate)
                            {
                                ForwardOnNext(task.Result);
                            }

                            OnCompleted();
                        }
                        break;
                        case TaskStatus.Faulted:
                        {
                            lock (_gate)
                            {
                                ForwardOnError(task.Exception.InnerException);
                            }
                        }
                        break;
                        case TaskStatus.Canceled:
                        {
                            lock (_gate)
                            {
                                ForwardOnError(new TaskCanceledException(task));
                            }
                        }
                        break;
                    }
                }

                public override void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    if (Interlocked.Decrement(ref _count) == 0)
                    {
                        lock (_gate)
                        {
                            ForwardOnCompleted();
                        }
                    }
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                        _cts.Cancel();

                    base.Dispose(disposing);
                }
            }
        }
    }
}
