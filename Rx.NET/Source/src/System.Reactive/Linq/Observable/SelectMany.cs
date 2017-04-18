// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class SelectMany<TSource, TCollection, TResult>
    {
        internal sealed class ObservableSelector : Producer<TResult>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, IObservable<TCollection>> _collectionSelector;
            private readonly Func<TSource, TCollection, TResult> _resultSelector;

            public ObservableSelector(IObservable<TSource> source, Func<TSource, IObservable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
            {
                _source = source;
                _collectionSelector = collectionSelector;
                _resultSelector = resultSelector;
            }

            protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }

            private sealed class _ : Sink<TResult>, IObserver<TSource>
            {
                private readonly ObservableSelector _parent;

                public _(ObservableSelector parent, IObserver<TResult> observer, IDisposable cancel)
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
                    innerSubscription.Disposable = collection.SubscribeSafe(new InnerObserver(this, value, innerSubscription));
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

                private sealed class InnerObserver : IObserver<TCollection>
                {
                    private readonly _ _parent;
                    private readonly TSource _value;
                    private readonly IDisposable _self;

                    public InnerObserver(_ parent, TSource value, IDisposable self)
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
        }

        internal sealed class ObservableSelectorIndexed : Producer<TResult>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, int, IObservable<TCollection>> _collectionSelector;
            private readonly Func<TSource, int, TCollection, int, TResult> _resultSelector;

            public ObservableSelectorIndexed(IObservable<TSource> source, Func<TSource, int, IObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
            {
                _source = source;
                _collectionSelector = collectionSelector;
                _resultSelector = resultSelector;
            }

            protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }

            private sealed class _ : Sink<TResult>, IObserver<TSource>
            {
                private readonly ObservableSelectorIndexed _parent;

                public _(ObservableSelectorIndexed parent, IObserver<TResult> observer, IDisposable cancel)
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
                        collection = _parent._collectionSelector(value, index);
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
                    innerSubscription.Disposable = collection.SubscribeSafe(new InnerObserver(this, value, index, innerSubscription));
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

                private sealed class InnerObserver : IObserver<TCollection>
                {
                    private readonly _ _parent;
                    private readonly TSource _value;
                    private readonly int _valueIndex;
                    private readonly IDisposable _self;

                    public InnerObserver(_ parent, TSource value, int index, IDisposable self)
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
                            res = _parent._parent._resultSelector(_value, _valueIndex, value, checked(_index++));
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
        }

        internal sealed class EnumerableSelector : Producer<TResult>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, IEnumerable<TCollection>> _collectionSelector;
            private readonly Func<TSource, TCollection, TResult> _resultSelector;

            public EnumerableSelector(IObservable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
            {
                _source = source;
                _collectionSelector = collectionSelector;
                _resultSelector = resultSelector;
            }

            protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }

            private sealed class _ : Sink<TResult>, IObserver<TSource>
            {
                private readonly EnumerableSelector _parent;

                public _(EnumerableSelector parent, IObserver<TResult> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _parent = parent;
                }

                public void OnNext(TSource value)
                {
                    var xs = default(IEnumerable<TCollection>);
                    try
                    {
                        xs = _parent._collectionSelector(value);
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
        }

        internal sealed class EnumerableSelectorIndexed : Producer<TResult>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, int, IEnumerable<TCollection>> _collectionSelector;
            private readonly Func<TSource, int, TCollection, int, TResult> _resultSelector;

            public EnumerableSelectorIndexed(IObservable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
            {
                _source = source;
                _collectionSelector = collectionSelector;
                _resultSelector = resultSelector;
            }

            protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }

            private sealed class _ : Sink<TResult>, IObserver<TSource>
            {
                private readonly EnumerableSelectorIndexed _parent;

                public _(EnumerableSelectorIndexed parent, IObserver<TResult> observer, IDisposable cancel)
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
                        xs = _parent._collectionSelector(value, index);
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
                                    current = _parent._resultSelector(value, index, e.Current, checked(eIndex++));
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
        }

        internal sealed class TaskSelector : Producer<TResult>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, CancellationToken, Task<TCollection>> _collectionSelector;
            private readonly Func<TSource, TCollection, TResult> _resultSelector;

            public TaskSelector(IObservable<TSource> source, Func<TSource, CancellationToken, Task<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
            {
                _source = source;
                _collectionSelector = collectionSelector;
                _resultSelector = resultSelector;
            }

            protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }

            private sealed class _ : Sink<TResult>, IObserver<TSource>
            {
                private readonly TaskSelector _parent;

                public _(TaskSelector parent, IObserver<TResult> observer, IDisposable cancel)
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

                    return StableCompositeDisposable.Create(_parent._source.SubscribeSafe(this), _cancel);
                }

                public void OnNext(TSource value)
                {
                    var task = default(Task<TCollection>);
                    try
                    {
                        Interlocked.Increment(ref _count);
                        task = _parent._collectionSelector(value, _cancel.Token);
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
        }

        internal sealed class TaskSelectorIndexed : Producer<TResult>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, int, CancellationToken, Task<TCollection>> _collectionSelector;
            private readonly Func<TSource, int, TCollection, TResult> _resultSelector;

            public TaskSelectorIndexed(IObservable<TSource> source, Func<TSource, int, CancellationToken, Task<TCollection>> collectionSelector, Func<TSource, int, TCollection, TResult> resultSelector)
            {
                _source = source;
                _collectionSelector = collectionSelector;
                _resultSelector = resultSelector;
            }

            protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }

            private sealed class _ : Sink<TResult>, IObserver<TSource>
            {
                private readonly TaskSelectorIndexed _parent;

                public _(TaskSelectorIndexed parent, IObserver<TResult> observer, IDisposable cancel)
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

                    return StableCompositeDisposable.Create(_parent._source.SubscribeSafe(this), _cancel);
                }

                public void OnNext(TSource value)
                {
                    var index = checked(_index++);

                    var task = default(Task<TCollection>);
                    try
                    {
                        Interlocked.Increment(ref _count);
                        task = _parent._collectionSelector(value, index, _cancel.Token);
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
                                    res = _parent._resultSelector(value, index, task.Result);
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
        }
    }

    internal static class SelectMany<TSource, TResult>
    {
        internal sealed class ObservableSelector : Producer<TResult>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, IObservable<TResult>> _selector;
            private readonly Func<Exception, IObservable<TResult>> _selectorOnError;
            private readonly Func<IObservable<TResult>> _selectorOnCompleted;

            public ObservableSelector(IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector)
            {
                _source = source;
                _selector = selector;
            }

            public ObservableSelector(IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector, Func<Exception, IObservable<TResult>> selectorOnError, Func<IObservable<TResult>> selectorOnCompleted)
            {
                _source = source;
                _selector = selector;
                _selectorOnError = selectorOnError;
                _selectorOnCompleted = selectorOnCompleted;
            }

            protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }

            private sealed class _ : Sink<TResult>, IObserver<TSource>
            {
                private readonly ObservableSelector _parent;

                public _(ObservableSelector parent, IObserver<TResult> observer, IDisposable cancel)
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
                    innerSubscription.Disposable = inner.SubscribeSafe(new InnerObserver(this, innerSubscription));
                }

                private sealed class InnerObserver : IObserver<TResult>
                {
                    private readonly _ _parent;
                    private readonly IDisposable _self;

                    public InnerObserver(_ parent, IDisposable self)
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
        }

        internal sealed class ObservableSelectorIndexed : Producer<TResult>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, int, IObservable<TResult>> _selector;
            private readonly Func<Exception, IObservable<TResult>> _selectorOnError;
            private readonly Func<IObservable<TResult>> _selectorOnCompleted;

            public ObservableSelectorIndexed(IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector)
            {
                _source = source;
                _selector = selector;
            }

            public ObservableSelectorIndexed(IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector, Func<Exception, IObservable<TResult>> selectorOnError, Func<IObservable<TResult>> selectorOnCompleted)
            {
                _source = source;
                _selector = selector;
                _selectorOnError = selectorOnError;
                _selectorOnCompleted = selectorOnCompleted;
            }

            protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }

            private sealed class _ : Sink<TResult>, IObserver<TSource>
            {
                private readonly ObservableSelectorIndexed _parent;

                public _(ObservableSelectorIndexed parent, IObserver<TResult> observer, IDisposable cancel)
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
                        inner = _parent._selector(value, checked(_index++));
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
                    innerSubscription.Disposable = inner.SubscribeSafe(new InnerObserver(this, innerSubscription));
                }

                private sealed class InnerObserver : IObserver<TResult>
                {
                    private readonly _ _parent;
                    private readonly IDisposable _self;

                    public InnerObserver(_ parent, IDisposable self)
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
        }

        internal sealed class EnumerableSelector : Producer<TResult>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, IEnumerable<TResult>> _selector;

            public EnumerableSelector(IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
            {
                _source = source;
                _selector = selector;
            }

            protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }

            private sealed class _ : Sink<TResult>, IObserver<TSource>
            {
                private readonly EnumerableSelector _parent;

                public _(EnumerableSelector parent, IObserver<TResult> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _parent = parent;
                }

                public void OnNext(TSource value)
                {
                    var xs = default(IEnumerable<TResult>);
                    try
                    {
                        xs = _parent._selector(value);
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
        }

        internal sealed class EnumerableSelectorIndexed : Producer<TResult>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, int, IEnumerable<TResult>> _selector;

            public EnumerableSelectorIndexed(IObservable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
            {
                _source = source;
                _selector = selector;
            }

            protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }

            private sealed class _ : Sink<TResult>, IObserver<TSource>
            {
                private readonly EnumerableSelectorIndexed _parent;

                public _(EnumerableSelectorIndexed parent, IObserver<TResult> observer, IDisposable cancel)
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
                        xs = _parent._selector(value, checked(_index++));
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
        }

        internal sealed class TaskSelector : Producer<TResult>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, CancellationToken, Task<TResult>> _selector;

            public TaskSelector(IObservable<TSource> source, Func<TSource, CancellationToken, Task<TResult>> selector)
            {
                _source = source;
                _selector = selector;
            }

            protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }

            private sealed class _ : Sink<TResult>, IObserver<TSource>
            {
                private readonly TaskSelector _parent;

                public _(TaskSelector parent, IObserver<TResult> observer, IDisposable cancel)
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

                    return StableCompositeDisposable.Create(_parent._source.SubscribeSafe(this), _cancel);
                }

                public void OnNext(TSource value)
                {
                    var task = default(Task<TResult>);
                    try
                    {
                        Interlocked.Increment(ref _count);
                        task = _parent._selector(value, _cancel.Token);
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
        }

        internal sealed class TaskSelectorIndexed : Producer<TResult>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, int, CancellationToken, Task<TResult>> _selector;

            public TaskSelectorIndexed(IObservable<TSource> source, Func<TSource, int, CancellationToken, Task<TResult>> selector)
            {
                _source = source;
                _selector = selector;
            }

            protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }

            private sealed class _ : Sink<TResult>, IObserver<TSource>
            {
                private readonly TaskSelectorIndexed _parent;

                public _(TaskSelectorIndexed parent, IObserver<TResult> observer, IDisposable cancel)
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

                    return StableCompositeDisposable.Create(_parent._source.SubscribeSafe(this), _cancel);
                }

                public void OnNext(TSource value)
                {
                    var task = default(Task<TResult>);
                    try
                    {
                        Interlocked.Increment(ref _count);
                        task = _parent._selector(value, checked(_index++), _cancel.Token);
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
        }
    }
}
