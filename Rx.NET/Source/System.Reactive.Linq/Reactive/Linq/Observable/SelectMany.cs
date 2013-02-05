// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System.Collections.Generic;
using System.Reactive.Disposables;

#if !NO_TPL
using System.Threading;
using System.Threading.Tasks;
#endif

namespace System.Reactive.Linq.Observαble
{
    class SelectMany<TSource, TCollection, TResult> : Producer<TResult>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, IObservable<TCollection>> _collectionSelector;
        private readonly Func<TSource, int, IObservable<TCollection>> _collectionSelectorI;
        private readonly Func<TSource, IEnumerable<TCollection>> _collectionSelectorE;
        private readonly Func<TSource, int, IEnumerable<TCollection>> _collectionSelectorEI;
        private readonly Func<TSource, TCollection, TResult> _resultSelector;
        private readonly Func<TSource, int, TCollection, int, TResult> _resultSelectorI;

        public SelectMany(IObservable<TSource> source, Func<TSource, IObservable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            _source = source;
            _collectionSelector = collectionSelector;
            _resultSelector = resultSelector;
        }

        public SelectMany(IObservable<TSource> source, Func<TSource, int, IObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            _source = source;
            _collectionSelectorI = collectionSelector;
            _resultSelectorI = resultSelector;
        }

        public SelectMany(IObservable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            _source = source;
            _collectionSelectorE = collectionSelector;
            _resultSelector = resultSelector;
        }

        public SelectMany(IObservable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            _source = source;
            _collectionSelectorEI = collectionSelector;
            _resultSelectorI = resultSelector;
        }

#if !NO_TPL
        private readonly Func<TSource, CancellationToken, Task<TCollection>> _collectionSelectorT;
        private readonly Func<TSource, int, CancellationToken, Task<TCollection>> _collectionSelectorTI;
        private readonly Func<TSource, int, TCollection, TResult> _resultSelectorTI;

        public SelectMany(IObservable<TSource> source, Func<TSource, CancellationToken, Task<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            _source = source;
            _collectionSelectorT = collectionSelector;
            _resultSelector = resultSelector;
        }

        public SelectMany(IObservable<TSource> source, Func<TSource, int, CancellationToken, Task<TCollection>> collectionSelector, Func<TSource, int, TCollection, TResult> resultSelector)
        {
            _source = source;
            _collectionSelectorTI = collectionSelector;
            _resultSelectorTI = resultSelector;
        }
#endif

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_collectionSelector != null)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
            else if (_collectionSelectorI != null)
            {
                var sink = new π(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
#if !NO_TPL
            else if (_collectionSelectorT != null)
            {
                var sink = new τ(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
            else if (_collectionSelectorTI != null)
            {
                var sink = new σ(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
#endif
            else if (_collectionSelectorE != null)
            {
                var sink = new ε(this, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }
            else
            {
                var sink = new ω(this, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }
        }

        class _ : Sink<TResult>, IObserver<TSource>
        {
            private readonly SelectMany<TSource, TCollection, TResult> _parent;

            public _(SelectMany<TSource, TCollection, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
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
                _sourceSubscription.Disposable = _parent._source.SubscribeSafe(this);

                return _group;
            }

            public void OnNext(TSource value)
            {
                var collection = default(IObservable<TCollection>);

                try
                {
                    collection = _parent._collectionSelector(value);
                }
                catch (Exception ex)
                {
                    lock (_gate)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                    }
                    return;
                }

                var innerSubscription = new SingleAssignmentDisposable();
                _group.Add(innerSubscription);
                innerSubscription.Disposable = collection.SubscribeSafe(new ι(this, value, innerSubscription));
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

            class ι : IObserver<TCollection>
            {
                private readonly _ _parent;
                private readonly TSource _value;
                private readonly IDisposable _self;

                public ι(_ parent, TSource value, IDisposable self)
                {
                    _parent = parent;
                    _value = value;
                    _self = self;
                }

                public void OnNext(TCollection value)
                {
                    var res = default(TResult);

                    try
                    {
                        res = _parent._parent._resultSelector(_value, value);
                    }
                    catch (Exception ex)
                    {
                        lock (_parent._gate)
                        {
                            _parent._observer.OnError(ex);
                            _parent.Dispose();
                        }
                        return;
                    }

                    lock (_parent._gate)
                        _parent._observer.OnNext(res);
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

        class π : Sink<TResult>, IObserver<TSource>
        {
            private readonly SelectMany<TSource, TCollection, TResult> _parent;

            public π(SelectMany<TSource, TCollection, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private bool _isStopped;
            private CompositeDisposable _group;
            private SingleAssignmentDisposable _sourceSubscription;
            private int _index;

            public IDisposable Run()
            {
                _gate = new object();
                _isStopped = false;
                _group = new CompositeDisposable();

                _sourceSubscription = new SingleAssignmentDisposable();
                _group.Add(_sourceSubscription);
                _sourceSubscription.Disposable = _parent._source.SubscribeSafe(this);

                return _group;
            }

            public void OnNext(TSource value)
            {
                var index = checked(_index++);
                var collection = default(IObservable<TCollection>);

                try
                {
                    collection = _parent._collectionSelectorI(value, index);
                }
                catch (Exception ex)
                {
                    lock (_gate)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                    }
                    return;
                }

                var innerSubscription = new SingleAssignmentDisposable();
                _group.Add(innerSubscription);
                innerSubscription.Disposable = collection.SubscribeSafe(new ι(this, value, index, innerSubscription));
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

            class ι : IObserver<TCollection>
            {
                private readonly π _parent;
                private readonly TSource _value;
                private readonly int _valueIndex;
                private readonly IDisposable _self;

                public ι(π parent, TSource value, int index, IDisposable self)
                {
                    _parent = parent;
                    _value = value;
                    _valueIndex = index;
                    _self = self;
                }

                private int _index;

                public void OnNext(TCollection value)
                {
                    var res = default(TResult);

                    try
                    {
                        res = _parent._parent._resultSelectorI(_value, _valueIndex, value, checked(_index++));
                    }
                    catch (Exception ex)
                    {
                        lock (_parent._gate)
                        {
                            _parent._observer.OnError(ex);
                            _parent.Dispose();
                        }
                        return;
                    }

                    lock (_parent._gate)
                        _parent._observer.OnNext(res);
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

        class ε : Sink<TResult>, IObserver<TSource>
        {
            private readonly SelectMany<TSource, TCollection, TResult> _parent;

            public ε(SelectMany<TSource, TCollection, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public void OnNext(TSource value)
            {
                var xs = default(IEnumerable<TCollection>);
                try
                {
                    xs = _parent._collectionSelectorE(value);
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return;
                }

                var e = default(IEnumerator<TCollection>);
                try
                {
                    e = xs.GetEnumerator();
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return;
                }

                try
                {
                    var hasNext = true;
                    while (hasNext)
                    {
                        hasNext = false;
                        var current = default(TResult);

                        try
                        {
                            hasNext = e.MoveNext();
                            if (hasNext)
                                current = _parent._resultSelector(value, e.Current);
                        }
                        catch (Exception exception)
                        {
                            base._observer.OnError(exception);
                            base.Dispose();
                            return;
                        }

                        if (hasNext)
                            base._observer.OnNext(current);
                    }
                }
                finally
                {
                    if (e != null)
                        e.Dispose();
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnCompleted();
                base.Dispose();
            }
        }

        class ω : Sink<TResult>, IObserver<TSource>
        {
            private readonly SelectMany<TSource, TCollection, TResult> _parent;

            public ω(SelectMany<TSource, TCollection, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private int _index;

            public void OnNext(TSource value)
            {
                var index = checked(_index++);

                var xs = default(IEnumerable<TCollection>);
                try
                {
                    xs = _parent._collectionSelectorEI(value, index);
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return;
                }

                var e = default(IEnumerator<TCollection>);
                try
                {
                    e = xs.GetEnumerator();
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return;
                }

                try
                {
                    var eIndex = 0;
                    var hasNext = true;
                    while (hasNext)
                    {
                        hasNext = false;
                        var current = default(TResult);

                        try
                        {
                            hasNext = e.MoveNext();
                            if (hasNext)
                                current = _parent._resultSelectorI(value, index, e.Current, checked(eIndex++));
                        }
                        catch (Exception exception)
                        {
                            base._observer.OnError(exception);
                            base.Dispose();
                            return;
                        }

                        if (hasNext)
                            base._observer.OnNext(current);
                    }
                }
                finally
                {
                    if (e != null)
                        e.Dispose();
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnCompleted();
                base.Dispose();
            }
        }

#if !NO_TPL
#pragma warning disable 0420
        class τ : Sink<TResult>, IObserver<TSource>
        {
            private readonly SelectMany<TSource, TCollection, TResult> _parent;

            public τ(SelectMany<TSource, TCollection, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private CancellationDisposable _cancel;
            private volatile int _count;

            public IDisposable Run()
            {
                _gate = new object();
                _cancel = new CancellationDisposable();
                _count = 1;

                return new CompositeDisposable(_parent._source.SubscribeSafe(this), _cancel);
            }

            public void OnNext(TSource value)
            {
                var task = default(Task<TCollection>);
                try
                {
                    Interlocked.Increment(ref _count);
                    task = _parent._collectionSelectorT(value, _cancel.Token);
                }
                catch (Exception ex)
                {
                    lock (_gate)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                    }

                    return;
                }

                if (task.IsCompleted)
                {
                    OnCompletedTask(value, task);
                }
                else
                {
                    AttachContinuation(value, task);
                }
            }

            private void AttachContinuation(TSource value, Task<TCollection> task)
            {
                //
                // Separate method to avoid closure in synchronous completion case.
                //
                task.ContinueWith(t => OnCompletedTask(value, t));
            }

            private void OnCompletedTask(TSource value, Task<TCollection> task)
            {
                switch (task.Status)
                {
                    case TaskStatus.RanToCompletion:
                        {
                            var res = default(TResult);
                            try
                            {
                                res = _parent._resultSelector(value, task.Result);
                            }
                            catch (Exception ex)
                            {
                                lock (_gate)
                                {
                                    base._observer.OnError(ex);
                                    base.Dispose();
                                }

                                return;
                            }

                            lock (_gate)
                                base._observer.OnNext(res);

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
                            if (!_cancel.IsDisposed)
                            {
                                lock (_gate)
                                {
                                    base._observer.OnError(new TaskCanceledException(task));
                                    base.Dispose();
                                }
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

        class σ : Sink<TResult>, IObserver<TSource>
        {
            private readonly SelectMany<TSource, TCollection, TResult> _parent;

            public σ(SelectMany<TSource, TCollection, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private CancellationDisposable _cancel;
            private volatile int _count;
            private int _index;

            public IDisposable Run()
            {
                _gate = new object();
                _cancel = new CancellationDisposable();
                _count = 1;

                return new CompositeDisposable(_parent._source.SubscribeSafe(this), _cancel);
            }

            public void OnNext(TSource value)
            {
                var index = checked(_index++);

                var task = default(Task<TCollection>);
                try
                {
                    Interlocked.Increment(ref _count);
                    task = _parent._collectionSelectorTI(value, index, _cancel.Token);
                }
                catch (Exception ex)
                {
                    lock (_gate)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                    }

                    return;
                }

                if (task.IsCompleted)
                {
                    OnCompletedTask(value, index, task);
                }
                else
                {
                    AttachContinuation(value, index, task);
                }
            }

            private void AttachContinuation(TSource value, int index, Task<TCollection> task)
            {
                //
                // Separate method to avoid closure in synchronous completion case.
                //
                task.ContinueWith(t => OnCompletedTask(value, index, t));
            }

            private void OnCompletedTask(TSource value, int index, Task<TCollection> task)
            {
                switch (task.Status)
                {
                    case TaskStatus.RanToCompletion:
                        {
                            var res = default(TResult);
                            try
                            {
                                res = _parent._resultSelectorTI(value, index, task.Result);
                            }
                            catch (Exception ex)
                            {
                                lock (_gate)
                                {
                                    base._observer.OnError(ex);
                                    base.Dispose();
                                }

                                return;
                            }

                            lock (_gate)
                                base._observer.OnNext(res);

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
                            if (!_cancel.IsDisposed)
                            {
                                lock (_gate)
                                {
                                    base._observer.OnError(new TaskCanceledException(task));
                                    base.Dispose();
                                }
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

    class SelectMany<TSource, TResult> : Producer<TResult>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, IObservable<TResult>> _selector;
        private readonly Func<TSource, int, IObservable<TResult>> _selectorI;
        private readonly Func<Exception, IObservable<TResult>> _selectorOnError;
        private readonly Func<IObservable<TResult>> _selectorOnCompleted;
        private readonly Func<TSource, IEnumerable<TResult>> _selectorE;
        private readonly Func<TSource, int, IEnumerable<TResult>> _selectorEI;

        public SelectMany(IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector)
        {
            _source = source;
            _selector = selector;
        }

        public SelectMany(IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector)
        {
            _source = source;
            _selectorI = selector;
        }

        public SelectMany(IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector, Func<Exception, IObservable<TResult>> selectorOnError, Func<IObservable<TResult>> selectorOnCompleted)
        {
            _source = source;
            _selector = selector;
            _selectorOnError = selectorOnError;
            _selectorOnCompleted = selectorOnCompleted;
        }

        public SelectMany(IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector, Func<Exception, IObservable<TResult>> selectorOnError, Func<IObservable<TResult>> selectorOnCompleted)
        {
            _source = source;
            _selectorI = selector;
            _selectorOnError = selectorOnError;
            _selectorOnCompleted = selectorOnCompleted;
        }

        public SelectMany(IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            _source = source;
            _selectorE = selector;
        }

        public SelectMany(IObservable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
        {
            _source = source;
            _selectorEI = selector;
        }

#if !NO_TPL
        private readonly Func<TSource, CancellationToken, Task<TResult>> _selectorT;
        private readonly Func<TSource, int, CancellationToken, Task<TResult>> _selectorTI;

        public SelectMany(IObservable<TSource> source, Func<TSource, CancellationToken, Task<TResult>> selector)
        {
            _source = source;
            _selectorT = selector;
        }
        
        public SelectMany(IObservable<TSource> source, Func<TSource, int, CancellationToken, Task<TResult>> selector)
        {
            _source = source;
            _selectorTI = selector;
        }
#endif

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_selector != null)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
            else if (_selectorI != null)
            {
                var sink = new π(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
#if !NO_TPL
            else if (_selectorT != null)
            {
                var sink = new τ(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
            else if (_selectorTI != null)
            {
                var sink = new σ(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
#endif
            else if (_selectorE != null)
            {
                var sink = new ε(this, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }
            else
            {
                var sink = new ω(this, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }
        }

        class _ : Sink<TResult>, IObserver<TSource>
        {
            private readonly SelectMany<TSource, TResult> _parent;

            public _(SelectMany<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
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
                _sourceSubscription.Disposable = _parent._source.SubscribeSafe(this);

                return _group;
            }

            public void OnNext(TSource value)
            {
                var inner = default(IObservable<TResult>);

                try
                {
                    inner = _parent._selector(value);
                }
                catch (Exception ex)
                {
                    lock (_gate)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                    }
                    return;
                }

                SubscribeInner(inner);
            }

            public void OnError(Exception error)
            {
                if (_parent._selectorOnError != null)
                {
                    var inner = default(IObservable<TResult>);

                    try
                    {
                        inner = _parent._selectorOnError(error);
                    }
                    catch (Exception ex)
                    {
                        lock (_gate)
                        {
                            base._observer.OnError(ex);
                            base.Dispose();
                        }
                        return;
                    }

                    SubscribeInner(inner);

                    Final();
                }
                else
                {
                    lock (_gate)
                    {
                        base._observer.OnError(error);
                        base.Dispose();
                    }
                }
            }

            public void OnCompleted()
            {
                if (_parent._selectorOnCompleted != null)
                {
                    var inner = default(IObservable<TResult>);

                    try
                    {
                        inner = _parent._selectorOnCompleted();
                    }
                    catch (Exception ex)
                    {
                        lock (_gate)
                        {
                            base._observer.OnError(ex);
                            base.Dispose();
                        }
                        return;
                    }

                    SubscribeInner(inner);
                }

                Final();
            }

            private void Final()
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

            private void SubscribeInner(IObservable<TResult> inner)
            {
                var innerSubscription = new SingleAssignmentDisposable();
                _group.Add(innerSubscription);
                innerSubscription.Disposable = inner.SubscribeSafe(new ι(this, innerSubscription));
            }

            class ι : IObserver<TResult>
            {
                private readonly _ _parent;
                private readonly IDisposable _self;

                public ι(_ parent, IDisposable self)
                {
                    _parent = parent;
                    _self = self;
                }

                public void OnNext(TResult value)
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

        class π : Sink<TResult>, IObserver<TSource>
        {
            private readonly SelectMany<TSource, TResult> _parent;

            public π(SelectMany<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private bool _isStopped;
            private CompositeDisposable _group;
            private SingleAssignmentDisposable _sourceSubscription;
            private int _index;

            public IDisposable Run()
            {
                _gate = new object();
                _isStopped = false;
                _group = new CompositeDisposable();

                _sourceSubscription = new SingleAssignmentDisposable();
                _group.Add(_sourceSubscription);
                _sourceSubscription.Disposable = _parent._source.SubscribeSafe(this);

                return _group;
            }

            public void OnNext(TSource value)
            {
                var inner = default(IObservable<TResult>);

                try
                {
                    inner = _parent._selectorI(value, checked(_index++));
                }
                catch (Exception ex)
                {
                    lock (_gate)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                    }
                    return;
                }

                SubscribeInner(inner);
            }

            public void OnError(Exception error)
            {
                if (_parent._selectorOnError != null)
                {
                    var inner = default(IObservable<TResult>);

                    try
                    {
                        inner = _parent._selectorOnError(error);
                    }
                    catch (Exception ex)
                    {
                        lock (_gate)
                        {
                            base._observer.OnError(ex);
                            base.Dispose();
                        }
                        return;
                    }

                    SubscribeInner(inner);

                    Final();
                }
                else
                {
                    lock (_gate)
                    {
                        base._observer.OnError(error);
                        base.Dispose();
                    }
                }
            }

            public void OnCompleted()
            {
                if (_parent._selectorOnCompleted != null)
                {
                    var inner = default(IObservable<TResult>);

                    try
                    {
                        inner = _parent._selectorOnCompleted();
                    }
                    catch (Exception ex)
                    {
                        lock (_gate)
                        {
                            base._observer.OnError(ex);
                            base.Dispose();
                        }
                        return;
                    }

                    SubscribeInner(inner);
                }

                Final();
            }

            private void Final()
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

            private void SubscribeInner(IObservable<TResult> inner)
            {
                var innerSubscription = new SingleAssignmentDisposable();
                _group.Add(innerSubscription);
                innerSubscription.Disposable = inner.SubscribeSafe(new ι(this, innerSubscription));
            }

            class ι : IObserver<TResult>
            {
                private readonly π _parent;
                private readonly IDisposable _self;

                public ι(π parent, IDisposable self)
                {
                    _parent = parent;
                    _self = self;
                }

                public void OnNext(TResult value)
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

        class ε : Sink<TResult>, IObserver<TSource>
        {
            private readonly SelectMany<TSource, TResult> _parent;

            public ε(SelectMany<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public void OnNext(TSource value)
            {
                var xs = default(IEnumerable<TResult>);
                try
                {
                    xs = _parent._selectorE(value);
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return;
                }

                var e = default(IEnumerator<TResult>);
                try
                {
                    e = xs.GetEnumerator();
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return;
                }

                try
                {
                    var hasNext = true;
                    while (hasNext)
                    {
                        hasNext = false;
                        var current = default(TResult);

                        try
                        {
                            hasNext = e.MoveNext();
                            if (hasNext)
                                current = e.Current;
                        }
                        catch (Exception exception)
                        {
                            base._observer.OnError(exception);
                            base.Dispose();
                            return;
                        }

                        if (hasNext)
                            base._observer.OnNext(current);
                    }
                }
                finally
                {
                    if (e != null)
                        e.Dispose();
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnCompleted();
                base.Dispose();
            }
        }

        class ω : Sink<TResult>, IObserver<TSource>
        {
            private readonly SelectMany<TSource, TResult> _parent;

            public ω(SelectMany<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }
            
            private int _index;

            public void OnNext(TSource value)
            {
                var xs = default(IEnumerable<TResult>);
                try
                {
                    xs = _parent._selectorEI(value, checked(_index++));
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return;
                }

                var e = default(IEnumerator<TResult>);
                try
                {
                    e = xs.GetEnumerator();
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return;
                }

                try
                {
                    var hasNext = true;
                    while (hasNext)
                    {
                        hasNext = false;
                        var current = default(TResult);

                        try
                        {
                            hasNext = e.MoveNext();
                            if (hasNext)
                                current = e.Current;
                        }
                        catch (Exception exception)
                        {
                            base._observer.OnError(exception);
                            base.Dispose();
                            return;
                        }

                        if (hasNext)
                            base._observer.OnNext(current);
                    }
                }
                finally
                {
                    if (e != null)
                        e.Dispose();
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnCompleted();
                base.Dispose();
            }
        }

#if !NO_TPL
#pragma warning disable 0420
        class τ : Sink<TResult>, IObserver<TSource>
        {
            private readonly SelectMany<TSource, TResult> _parent;

            public τ(SelectMany<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private CancellationDisposable _cancel;
            private volatile int _count;

            public IDisposable Run()
            {
                _gate = new object();
                _cancel = new CancellationDisposable();
                _count = 1;

                return new CompositeDisposable(_parent._source.SubscribeSafe(this), _cancel);
            }

            public void OnNext(TSource value)
            {
                var task = default(Task<TResult>);
                try
                {
                    Interlocked.Increment(ref _count);
                    task = _parent._selectorT(value, _cancel.Token);
                }
                catch (Exception ex)
                {
                    lock (_gate)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                    }

                    return;
                }

                if (task.IsCompleted)
                {
                    OnCompletedTask(task);
                }
                else
                {
                    task.ContinueWith(OnCompletedTask);
                }
            }

            private void OnCompletedTask(Task<TResult> task)
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
                            if (!_cancel.IsDisposed)
                            {
                                lock (_gate)
                                {
                                    base._observer.OnError(new TaskCanceledException(task));
                                    base.Dispose();
                               }
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

        class σ : Sink<TResult>, IObserver<TSource>
        {
            private readonly SelectMany<TSource, TResult> _parent;

            public σ(SelectMany<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private CancellationDisposable _cancel;
            private volatile int _count;
            private int _index;

            public IDisposable Run()
            {
                _gate = new object();
                _cancel = new CancellationDisposable();
                _count = 1;

                return new CompositeDisposable(_parent._source.SubscribeSafe(this), _cancel);
            }

            public void OnNext(TSource value)
            {
                var task = default(Task<TResult>);
                try
                {
                    Interlocked.Increment(ref _count);
                    task = _parent._selectorTI(value, checked(_index++), _cancel.Token);
                }
                catch (Exception ex)
                {
                    lock (_gate)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                    }

                    return;
                }

                if (task.IsCompleted)
                {
                    OnCompletedTask(task);
                }
                else
                {
                    task.ContinueWith(OnCompletedTask);
                }
            }

            private void OnCompletedTask(Task<TResult> task)
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
                            if (!_cancel.IsDisposed)
                            {
                                lock (_gate)
                                {
                                    base._observer.OnError(new TaskCanceledException(task));
                                    base.Dispose();
                                }
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