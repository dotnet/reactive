// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    #region Binary

    internal sealed class CombineLatest<TFirst, TSecond, TResult> : Producer<TResult, CombineLatest<TFirst, TSecond, TResult>._>
    {
        private readonly IObservable<TFirst> _first;
        private readonly IObservable<TSecond> _second;
        private readonly Func<TFirst, TSecond, TResult> _resultSelector;

        public CombineLatest(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
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

            public _(Func<TFirst, TSecond, TResult> resultSelector, IObserver<TResult> observer)
                : base(observer)
            {
                _resultSelector = resultSelector;
            }

            private object _gate;

            private IDisposable _firstDisposable;
            private IDisposable _secondDisposable;

            public void Run(IObservable<TFirst> first, IObservable<TSecond> second)
            {
                _gate = new object();

                var fstO = new FirstObserver(this);
                var sndO = new SecondObserver(this);

                fstO.Other = sndO;
                sndO.Other = fstO;

                Disposable.SetSingle(ref _firstDisposable, first.SubscribeSafe(fstO));
                Disposable.SetSingle(ref _secondDisposable, second.SubscribeSafe(sndO));
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Disposable.TryDispose(ref _firstDisposable);
                    Disposable.TryDispose(ref _secondDisposable);
                }
                base.Dispose(disposing);
            }

            private sealed class FirstObserver : IObserver<TFirst>
            {
                private readonly _ _parent;
                private SecondObserver _other;

                public FirstObserver(_ parent)
                {
                    _parent = parent;
                }

                public SecondObserver Other { set { _other = value; } }

                public bool HasValue { get; private set; }
                public TFirst Value { get; private set; }
                public bool Done { get; private set; }

                public void OnNext(TFirst value)
                {
                    lock (_parent._gate)
                    {
                        HasValue = true;
                        Value = value;

                        if (_other.HasValue)
                        {
                            TResult res;
                            try
                            {
                                res = _parent._resultSelector(value, _other.Value);
                            }
                            catch (Exception ex)
                            {
                                _parent.ForwardOnError(ex);
                                return;
                            }

                            _parent.ForwardOnNext(res);
                        }
                        else if (_other.Done)
                        {
                            _parent.ForwardOnCompleted();
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
            }

            private sealed class SecondObserver : IObserver<TSecond>
            {
                private readonly _ _parent;
                private FirstObserver _other;

                public SecondObserver(_ parent)
                {
                    _parent = parent;
                }

                public FirstObserver Other { set { _other = value; } }

                public bool HasValue { get; private set; }
                public TSecond Value { get; private set; }
                public bool Done { get; private set; }

                public void OnNext(TSecond value)
                {
                    lock (_parent._gate)
                    {
                        HasValue = true;
                        Value = value;

                        if (_other.HasValue)
                        {
                            TResult res;
                            try
                            {
                                res = _parent._resultSelector(_other.Value, value);
                            }
                            catch (Exception ex)
                            {
                                _parent.ForwardOnError(ex);
                                return;
                            }

                            _parent.ForwardOnNext(res);
                        }
                        else if (_other.Done)
                        {
                            _parent.ForwardOnCompleted();
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
            }
        }
    }

    #endregion

    #region [3,16]-ary

    #region Helpers for n-ary overloads

    internal interface ICombineLatest
    {
        void Next(int index);
        void Fail(Exception error);
        void Done(int index);
    }

    internal abstract class CombineLatestSink<TResult> : IdentitySink<TResult>, ICombineLatest
    {
        protected readonly object _gate;

        private bool _hasValueAll;
        private readonly bool[] _hasValue;
        private readonly bool[] _isDone;

        protected CombineLatestSink(int arity, IObserver<TResult> observer)
            : base(observer)
        {
            _gate = new object();

            _hasValue = new bool[arity];
            _isDone = new bool[arity];
        }

        public void Next(int index)
        {
            if (!_hasValueAll)
            {
                _hasValue[index] = true;

                var hasValueAll = true;
                foreach (var hasValue in _hasValue)
                {
                    if (!hasValue)
                    {
                        hasValueAll = false;
                        break;
                    }
                }

                _hasValueAll = hasValueAll;
            }

            if (_hasValueAll)
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

    internal sealed class CombineLatestObserver<T> : SafeObserver<T>
    {
        private readonly object _gate;
        private readonly ICombineLatest _parent;
        private readonly int _index;
        private T _value;

        public CombineLatestObserver(object gate, ICombineLatest parent, int index)
        {
            _gate = gate;
            _parent = parent;
            _index = index;
        }

        public T Value => _value;

        public override void OnNext(T value)
        {
            lock (_gate)
            {
                _value = value;
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
            Dispose();
            lock (_gate)
            {
                _parent.Done(_index);
            }
        }
    }

    #endregion

    #endregion

    #region N-ary

    internal sealed class CombineLatest<TSource, TResult> : Producer<TResult, CombineLatest<TSource, TResult>._>
    {
        private readonly IEnumerable<IObservable<TSource>> _sources;
        private readonly Func<IList<TSource>, TResult> _resultSelector;

        public CombineLatest(IEnumerable<IObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector)
        {
            _sources = sources;
            _resultSelector = resultSelector;
        }

        protected override _ CreateSink(IObserver<TResult> observer) => new _(_resultSelector, observer);

        protected override void Run(_ sink) => sink.Run(_sources);

        internal sealed class _ : IdentitySink<TResult>
        {
            private readonly Func<IList<TSource>, TResult> _resultSelector;

            public _(Func<IList<TSource>, TResult> resultSelector, IObserver<TResult> observer)
                : base(observer)
            {
                _resultSelector = resultSelector;
            }

            private object _gate;
            private bool[] _hasValue;
            private bool _hasValueAll;
            private List<TSource> _values;
            private bool[] _isDone;
            private IDisposable[] _subscriptions;

            public void Run(IEnumerable<IObservable<TSource>> sources)
            {
                var srcs = sources.ToArray();

                var N = srcs.Length;

                _hasValue = new bool[N];
                _hasValueAll = false;

                _values = new List<TSource>(N);
                for (var i = 0; i < N; i++)
                {
                    _values.Add(default);
                }

                _isDone = new bool[N];

                _subscriptions = new IDisposable[N];

                _gate = new object();

                for (var i = 0; i < N; i++)
                {
                    var j = i;

                    var o = new SourceObserver(this, j);
                    _subscriptions[j] = o;

                    o.SetResource(srcs[j].SubscribeSafe(o));
                }

                SetUpstream(StableCompositeDisposable.CreateTrusted(_subscriptions));
            }

            private void OnNext(int index, TSource value)
            {
                lock (_gate)
                {
                    _values[index] = value;

                    _hasValue[index] = true;

                    if (_hasValueAll || (_hasValueAll = _hasValue.All()))
                    {
                        TResult res;
                        try
                        {
                            res = _resultSelector(new ReadOnlyCollection<TSource>(_values));
                        }
                        catch (Exception ex)
                        {
                            ForwardOnError(ex);
                            return;
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
                        _subscriptions[index].Dispose();
                    }
                }
            }

            private sealed class SourceObserver : SafeObserver<TSource>
            {
                private readonly _ _parent;
                private readonly int _index;

                public SourceObserver(_ parent, int index)
                {
                    _parent = parent;
                    _index = index;
                }

                public override void OnNext(TSource value)
                {
                    _parent.OnNext(_index, value);
                }

                public override void OnError(Exception error)
                {
                    _parent.OnError(error);
                }

                public override void OnCompleted()
                {
                    _parent.OnCompleted(_index);
                }
            }
        }
    }

    #endregion
}
