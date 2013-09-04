// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

#if !NO_TPL
using System.Threading;
using System.Threading.Tasks;
#endif

namespace System.Reactive.Linq.Observαble
{
    class Merge<TSource> : Producer<TSource>
    {
        private readonly IObservable<IObservable<TSource>> _sources;
        private readonly int _maxConcurrent;

        public Merge(IObservable<IObservable<TSource>> sources)
        {
            _sources = sources;
        }

        public Merge(IObservable<IObservable<TSource>> sources, int maxConcurrent)
        {
            _sources = sources;
            _maxConcurrent = maxConcurrent;
        }

#if !NO_TPL
        private readonly IObservable<Task<TSource>> _sourcesT;

        public Merge(IObservable<Task<TSource>> sources)
        {
            _sourcesT = sources;
        }
#endif

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_maxConcurrent > 0)
            {
                var sink = new μ(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
#if !NO_TPL
            else if (_sourcesT != null)
            {
                var sink = new τ(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
#endif
            else
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
        }

        class _ : Sink<TSource>, IObserver<IObservable<TSource>>
        {
            private readonly Merge<TSource> _parent;

            public _(Merge<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private bool _isStopped;
            private CompositeDisposable _group;
            private SingleAssignmentDisposable _sourceSubscription;

            public IDisposable Run()
            {
                _gate = new object();
                _isStopped = false;
                _group = new CompositeDisposable();

                _sourceSubscription = new SingleAssignmentDisposable();
                _group.Add(_sourceSubscription);
                _sourceSubscription.Disposable = _parent._sources.SubscribeSafe(this);

                return _group;
            }

            public void OnNext(IObservable<TSource> value)
            {
                var innerSubscription = new SingleAssignmentDisposable();
                _group.Add(innerSubscription);
                innerSubscription.Disposable = value.SubscribeSafe(new ι(this, innerSubscription));
            }

            public void OnError(Exception error)
            {
                lock (_gate)
                {
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public void OnCompleted()
            {
                _isStopped = true;
                if (_group.Count == 1)
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
                        base._observer.OnCompleted();
                        base.Dispose();
                    }
                }
                else
                {
                    _sourceSubscription.Dispose();
                }
            }

            class ι : IObserver<TSource>
            {
                private readonly _ _parent;
                private readonly IDisposable _self;

                public ι(_ parent, IDisposable self)
                {
                    _parent = parent;
                    _self = self;
                }

                public void OnNext(TSource value)
                {
                    lock (_parent._gate)
                        _parent._observer.OnNext(value);
                }

                public void OnError(Exception error)
                {
                    lock (_parent._gate)
                    {
                        _parent._observer.OnError(error);
                        _parent.Dispose();
                    }
                }

                public void OnCompleted()
                {
                    _parent._group.Remove(_self);
                    if (_parent._isStopped && _parent._group.Count == 1)
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
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                        }
                    }
                }
            }
        }

        class μ : Sink<TSource>, IObserver<IObservable<TSource>>
        {
            private readonly Merge<TSource> _parent;

            public μ(Merge<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private Queue<IObservable<TSource>> _q;
            private bool _isStopped;
            private SingleAssignmentDisposable _sourceSubscription;
            private CompositeDisposable _group;
            private int _activeCount = 0;

            public IDisposable Run()
            {
                _gate = new object();
                _q = new Queue<IObservable<TSource>>();
                _isStopped = false;
                _activeCount = 0;

                _group = new CompositeDisposable();
                _sourceSubscription = new SingleAssignmentDisposable();
                _sourceSubscription.Disposable = _parent._sources.SubscribeSafe(this);
                _group.Add(_sourceSubscription);

                return _group;
            }

            public void OnNext(IObservable<TSource> value)
            {
                lock (_gate)
                {
                    if (_activeCount < _parent._maxConcurrent)
                    {
                        _activeCount++;
                        Subscribe(value);
                    }
                    else
                        _q.Enqueue(value);
                }
            }

            public void OnError(Exception error)
            {
                lock (_gate)
                {
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public void OnCompleted()
            {
                lock (_gate)
                {
                    _isStopped = true;
                    if (_activeCount == 0)
                    {
                        base._observer.OnCompleted();
                        base.Dispose();
                    }
                    else
                    {
                        _sourceSubscription.Dispose();
                    }
                }
            }

            private void Subscribe(IObservable<TSource> innerSource)
            {
                var subscription = new SingleAssignmentDisposable();
                _group.Add(subscription);
                subscription.Disposable = innerSource.SubscribeSafe(new ι(this, subscription));
            }

            class ι : IObserver<TSource>
            {
                private readonly μ _parent;
                private readonly IDisposable _self;

                public ι(μ parent, IDisposable self)
                {
                    _parent = parent;
                    _self = self;
                }

                public void OnNext(TSource value)
                {
                    lock (_parent._gate)
                        _parent._observer.OnNext(value);
                }

                public void OnError(Exception error)
                {
                    lock (_parent._gate)
                    {
                        _parent._observer.OnError(error);
                        _parent.Dispose();
                    }
                }

                public void OnCompleted()
                {
                    _parent._group.Remove(_self);
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
                                _parent._observer.OnCompleted();
                                _parent.Dispose();
                            }
                        }
                    }
                }
            }
        }

#if !NO_TPL
#pragma warning disable 0420
        class τ : Sink<TSource>, IObserver<Task<TSource>>
        {
            private readonly Merge<TSource> _parent;

            public τ(Merge<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private volatile int _count;

            public IDisposable Run()
            {
                _gate = new object();
                _count = 1;

                return _parent._sourcesT.SubscribeSafe(this);
            }

            public void OnNext(Task<TSource> value)
            {
                Interlocked.Increment(ref _count);
                if (value.IsCompleted)
                {
                    OnCompletedTask(value);
                }
                else
                {
                    value.ContinueWith(OnCompletedTask);
                }
            }

            private void OnCompletedTask(Task<TSource> task)
            {
                switch (task.Status)
                {
                    case TaskStatus.RanToCompletion:
                        {
                            lock (_gate)
                                base._observer.OnNext(task.Result);

                            OnCompleted();
                        }
                        break;
                    case TaskStatus.Faulted:
                        {
                            lock (_gate)
                            {
                                base._observer.OnError(task.Exception.InnerException);
                                base.Dispose();
                            }
                        }
                        break;
                    case TaskStatus.Canceled:
                        {
                            lock (_gate)
                            {
                                base._observer.OnError(new TaskCanceledException(task));
                                base.Dispose();
                            }
                        }
                        break;
                }
            }

            public void OnError(Exception error)
            {
                lock (_gate)
                {
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public void OnCompleted()
            {
                if (Interlocked.Decrement(ref _count) == 0)
                {
                    lock (_gate)
                    {
                        base._observer.OnCompleted();
                        base.Dispose();
                    }
                }
            }
        }
#pragma warning restore 0420
#endif
    }
}
#endif