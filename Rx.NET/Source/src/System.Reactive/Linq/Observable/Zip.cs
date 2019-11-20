// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    #region Binary

    internal static class Zip<TFirst, TSecond, TResult>
    {
        internal sealed class Observable : Producer<TResult, Observable._>
        {
            private readonly IObservable<TFirst> _first;
            private readonly IObservable<TSecond> _second;
            private readonly Func<TFirst, TSecond, TResult> _resultSelector;

            public Observable(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
            {
                _first = first;
                _second = second;
                _resultSelector = resultSelector;
            }

            protected override _ CreateSink(IObserver<TResult> observer) => new _(_resultSelector, observer);

            protected override void Run(_ sink) => sink.Run(_first, _second);

            internal sealed class _ : IdentitySink<TResult>
            {
                private readonly Func<TFirst, TSecond, TResult> _resultSelector;

                private readonly object _gate;

                private readonly FirstObserver _firstObserver;
                private IDisposable _firstDisposable;

                private readonly SecondObserver _secondObserver;
                private IDisposable _secondDisposable;

                public _(Func<TFirst, TSecond, TResult> resultSelector, IObserver<TResult> observer)
                    : base(observer)
                {
                    _gate = new object();

                    _firstObserver = new FirstObserver(this);
                    _secondObserver = new SecondObserver(this);

                    _firstObserver.Other = _secondObserver;
                    _secondObserver.Other = _firstObserver;

                    _resultSelector = resultSelector;
                }

                public void Run(IObservable<TFirst> first, IObservable<TSecond> second)
                {
                    Disposable.SetSingle(ref _firstDisposable, first.SubscribeSafe(_firstObserver));
                    Disposable.SetSingle(ref _secondDisposable, second.SubscribeSafe(_secondObserver));
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        Disposable.TryDispose(ref _firstDisposable);
                        Disposable.TryDispose(ref _secondDisposable);

                        // clearing the queue should happen under the lock
                        // as they are plain Queue<T>s, not concurrent queues.
                        lock (_gate)
                        {
                            _firstObserver.Dispose();
                            _secondObserver.Dispose();
                        }
                    }
                    base.Dispose(disposing);
                }

                private sealed class FirstObserver : IObserver<TFirst>, IDisposable
                {
                    private readonly _ _parent;
                    private SecondObserver _other;
                    private readonly Queue<TFirst> _queue;

                    public FirstObserver(_ parent)
                    {
                        _parent = parent;
                        _queue = new Queue<TFirst>();
                    }

                    public SecondObserver Other { set { _other = value; } }

                    public Queue<TFirst> Queue => _queue;
                    public bool Done { get; private set; }

                    public void OnNext(TFirst value)
                    {
                        lock (_parent._gate)
                        {
                            if (_other.Queue.Count > 0)
                            {
                                var r = _other.Queue.Dequeue();

                                TResult res;
                                try
                                {
                                    res = _parent._resultSelector(value, r);
                                }
                                catch (Exception ex)
                                {
                                    _parent.ForwardOnError(ex);
                                    return;
                                }

                                _parent.ForwardOnNext(res);
                            }
                            else
                            {
                                if (_other.Done)
                                {
                                    _parent.ForwardOnCompleted();
                                    return;
                                }

                                _queue.Enqueue(value);
                            }
                        }
                    }

                    public void OnError(Exception error)
                    {
                        lock (_parent._gate)
                        {
                            _parent.ForwardOnError(error);
                        }
                    }

                    public void OnCompleted()
                    {
                        lock (_parent._gate)
                        {
                            Done = true;

                            if (_other.Done)
                            {
                                _parent.ForwardOnCompleted();
                            }
                            else
                            {
                                Disposable.TryDispose(ref _parent._firstDisposable);
                            }
                        }
                    }

                    public void Dispose()
                    {
                        _queue.Clear();
                    }
                }

                private sealed class SecondObserver : IObserver<TSecond>, IDisposable
                {
                    private readonly _ _parent;
                    private FirstObserver _other;
                    private readonly Queue<TSecond> _queue;

                    public SecondObserver(_ parent)
                    {
                        _parent = parent;
                        _queue = new Queue<TSecond>();
                    }

                    public FirstObserver Other { set { _other = value; } }

                    public Queue<TSecond> Queue => _queue;
                    public bool Done { get; private set; }

                    public void OnNext(TSecond value)
                    {
                        lock (_parent._gate)
                        {
                            if (_other.Queue.Count > 0)
                            {
                                var l = _other.Queue.Dequeue();

                                TResult res;
                                try
                                {
                                    res = _parent._resultSelector(l, value);
                                }
                                catch (Exception ex)
                                {
                                    _parent.ForwardOnError(ex);
                                    return;
                                }

                                _parent.ForwardOnNext(res);
                            }
                            else
                            {
                                if (_other.Done)
                                {
                                    _parent.ForwardOnCompleted();
                                    return;
                                }

                                _queue.Enqueue(value);
                            }
                        }
                    }

                    public void OnError(Exception error)
                    {
                        lock (_parent._gate)
                        {
                            _parent.ForwardOnError(error);
                        }
                    }

                    public void OnCompleted()
                    {
                        lock (_parent._gate)
                        {
                            Done = true;

                            if (_other.Done)
                            {
                                _parent.ForwardOnCompleted();
                            }
                            else
                            {
                                Disposable.TryDispose(ref _parent._secondDisposable);
                            }
                        }
                    }

                    public void Dispose()
                    {
                        _queue.Clear();
                    }
                }
            }
        }

        internal sealed class Enumerable : Producer<TResult, Enumerable._>
        {
            private readonly IObservable<TFirst> _first;
            private readonly IEnumerable<TSecond> _second;
            private readonly Func<TFirst, TSecond, TResult> _resultSelector;

            public Enumerable(IObservable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
            {
                _first = first;
                _second = second;
                _resultSelector = resultSelector;
            }

            protected override _ CreateSink(IObserver<TResult> observer) => new _(_resultSelector, observer);

            protected override void Run(_ sink) => sink.Run(_first, _second);

            internal sealed class _ : Sink<TFirst, TResult>
            {
                private readonly Func<TFirst, TSecond, TResult> _resultSelector;

                public _(Func<TFirst, TSecond, TResult> resultSelector, IObserver<TResult> observer)
                    : base(observer)
                {
                    _resultSelector = resultSelector;
                }

                int _enumerationInProgress;

                private IEnumerator<TSecond> _rightEnumerator;

                private static readonly IEnumerator<TSecond> DisposedEnumerator = MakeDisposedEnumerator();

                private static IEnumerator<TSecond> MakeDisposedEnumerator()
                {
                    yield break;
                }

                public void Run(IObservable<TFirst> first, IEnumerable<TSecond> second)
                {
                    //
                    // Notice the evaluation order of obtaining the enumerator and subscribing to the
                    // observable sequence is reversed compared to the operator's signature. This is
                    // required to make sure the enumerator is available as soon as the observer can
                    // be called. Otherwise, we end up having a race for the initialization and use
                    // of the _rightEnumerator field.
                    //
                    try
                    {
                        var enumerator = second.GetEnumerator();
                        if (Interlocked.CompareExchange(ref _rightEnumerator, enumerator, null) != null)
                        {
                            enumerator.Dispose();
                            return;
                        }
                    }
                    catch (Exception exception)
                    {
                        ForwardOnError(exception);

                        return;
                    }

                    Run(first);
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        if (Interlocked.Increment(ref _enumerationInProgress) == 1)
                        {
                            Interlocked.Exchange(ref _rightEnumerator, DisposedEnumerator)?.Dispose();
                        }
                    }
                    base.Dispose(disposing);
                }

                public override void OnNext(TFirst value)
                {
                    var currentEnumerator = Volatile.Read(ref _rightEnumerator);
                    if (currentEnumerator == DisposedEnumerator)
                    {
                        return;
                    }
                    if (Interlocked.Increment(ref _enumerationInProgress) != 1)
                    {
                        return;
                    }
                    bool hasNext;
                    TSecond right = default;
                    var wasDisposed = false;
                    try
                    {
                        try
                        {
                            hasNext = currentEnumerator.MoveNext();
                            if (hasNext)
                            {
                                right = currentEnumerator.Current;
                            }
                        }
                        finally
                        {
                            if (Interlocked.Decrement(ref _enumerationInProgress) != 0)
                            {
                                Interlocked.Exchange(ref _rightEnumerator, DisposedEnumerator)?.Dispose();
                                wasDisposed = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ForwardOnError(ex);
                        return;
                    }

                    if (wasDisposed)
                    {
                        return;
                    }

                    if (hasNext)
                    {
                        TResult result;
                        try
                        {
                            result = _resultSelector(value, right);
                        }
                        catch (Exception ex)
                        {
                            ForwardOnError(ex);
                            return;
                        }

                        ForwardOnNext(result);
                    }
                    else
                    {
                        ForwardOnCompleted();
                    }
                }
            }
        }
    }

    #endregion

    #region [3,16]-ary

    #region Helpers for n-ary overloads

    internal interface IZip
    {
        void Next(int index);
        void Fail(Exception error);
        void Done(int index);
    }

    internal abstract class ZipSink<TResult> : IdentitySink<TResult>, IZip
    {
        protected readonly object _gate;

        private readonly ICollection[] _queues;
        private readonly bool[] _isDone;

        protected ZipSink(int arity, IObserver<TResult> observer)
            : base(observer)
        {
            _gate = new object();

            _isDone = new bool[arity];
            _queues = new ICollection[arity];
        }

        public ICollection[] Queues => _queues;

        public void Next(int index)
        {
            var hasValueAll = true;
            foreach (var queue in _queues)
            {
                if (queue.Count == 0)
                {
                    hasValueAll = false;
                    break;
                }
            }

            if (hasValueAll)
            {
                TResult res;
                try
                {
                    res = GetResult();
                }
                catch (Exception ex)
                {
                    ForwardOnError(ex);
                    return;
                }

                ForwardOnNext(res);
            }
            else
            {
                var allOthersDone = true;
                for (var i = 0; i < _isDone.Length; i++)
                {
                    if (i != index && !_isDone[i])
                    {
                        allOthersDone = false;
                        break;
                    }
                }

                if (allOthersDone)
                {
                    ForwardOnCompleted();
                }
            }
        }

        protected abstract TResult GetResult();

        public void Fail(Exception error)
        {
            ForwardOnError(error);
        }

        public void Done(int index)
        {
            _isDone[index] = true;

            var allDone = true;
            foreach (var isDone in _isDone)
            {
                if (!isDone)
                {
                    allDone = false;
                    break;
                }
            }

            if (allDone)
            {
                ForwardOnCompleted();
            }
        }
    }

    internal sealed class ZipObserver<T> : SafeObserver<T>
    {
        private readonly object _gate;
        private readonly IZip _parent;
        private readonly int _index;
        private readonly Queue<T> _values;

        public ZipObserver(object gate, IZip parent, int index)
        {
            _gate = gate;
            _parent = parent;
            _index = index;
            _values = new Queue<T>();
        }

        public Queue<T> Values => _values;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                lock (_gate)
                {
                    _values.Clear();
                }
            }
        }

        public override void OnNext(T value)
        {
            lock (_gate)
            {
                _values.Enqueue(value);
                _parent.Next(_index);
            }
        }

        public override void OnError(Exception error)
        {
            Dispose();

            lock (_gate)
            {
                _parent.Fail(error);
            }
        }

        public override void OnCompleted()
        {
            // Calling Dispose() here would clear the queue prematurely.
            // We only need to dispose the IDisposable of the upstream,
            // which is done by SafeObserver.Dispose(bool).
            base.Dispose(true);

            lock (_gate)
            {
                _parent.Done(_index);
            }
        }
    }

    #endregion

    #endregion

    #region N-ary

    internal sealed class Zip<TSource> : Producer<IList<TSource>, Zip<TSource>._>
    {
        private readonly IEnumerable<IObservable<TSource>> _sources;

        public Zip(IEnumerable<IObservable<TSource>> sources)
        {
            _sources = sources;
        }

        protected override _ CreateSink(IObserver<IList<TSource>> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_sources);

        internal sealed class _ : IdentitySink<IList<TSource>>
        {
            private readonly object _gate;

            public _(IObserver<IList<TSource>> observer)
                : base(observer)
            {
                _gate = new object();
            }

            private Queue<TSource>[] _queues;
            private bool[] _isDone;
            private IDisposable[] _subscriptions;

            public void Run(IEnumerable<IObservable<TSource>> sources)
            {
                var srcs = sources.ToArray();

                var N = srcs.Length;

                _queues = new Queue<TSource>[N];
                for (var i = 0; i < N; i++)
                {
                    _queues[i] = new Queue<TSource>();
                }

                _isDone = new bool[N];

                var subscriptions = new IDisposable[N];

                if (Interlocked.CompareExchange(ref _subscriptions, subscriptions, null) == null)
                {
                    for (var i = 0; i < N; i++)
                    {
                        var o = new SourceObserver(this, i);
                        Disposable.SetSingle(ref subscriptions[i], srcs[i].SubscribeSafe(o));
                    }
                }
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    var subscriptions = Interlocked.Exchange(ref _subscriptions, Array.Empty<IDisposable>());
                    if (subscriptions != null && subscriptions != Array.Empty<IDisposable>())
                    {
                        for (var i = 0; i < subscriptions.Length; i++)
                        {
                            Disposable.TryDispose(ref subscriptions[i]);
                        }

                        lock (_gate)
                        {
                            foreach (var q in _queues)
                            {
                                q.Clear();
                            }
                        }
                    }
                }
                base.Dispose(disposing);
            }

            private void OnNext(int index, TSource value)
            {
                lock (_gate)
                {
                    _queues[index].Enqueue(value);

                    if (_queues.All(q => q.Count > 0))
                    {
                        var n = _queues.Length;

                        var res = new List<TSource>(n);
                        for (var i = 0; i < n; i++)
                        {
                            res.Add(_queues[i].Dequeue());
                        }

                        ForwardOnNext(res);
                    }
                    else if (_isDone.AllExcept(index))
                    {
                        ForwardOnCompleted();
                    }
                }
            }

            private new void OnError(Exception error)
            {
                lock (_gate)
                {
                    ForwardOnError(error);
                }
            }

            private void OnCompleted(int index)
            {
                lock (_gate)
                {
                    _isDone[index] = true;

                    if (_isDone.All())
                    {
                        ForwardOnCompleted();
                    }
                    else
                    {
                        var subscriptions = Volatile.Read(ref _subscriptions);
                        if (subscriptions != null && subscriptions != Array.Empty<IDisposable>())
                        {
                            Disposable.TryDispose(ref subscriptions[index]);
                        }
                    }
                }
            }

            private sealed class SourceObserver : IObserver<TSource>
            {
                private readonly _ _parent;
                private readonly int _index;

                public SourceObserver(_ parent, int index)
                {
                    _parent = parent;
                    _index = index;
                }

                public void OnNext(TSource value)
                {
                    _parent.OnNext(_index, value);
                }

                public void OnError(Exception error)
                {
                    _parent.OnError(error);
                }

                public void OnCompleted()
                {
                    _parent.OnCompleted(_index);
                }
            }
        }
    }

    #endregion
}
