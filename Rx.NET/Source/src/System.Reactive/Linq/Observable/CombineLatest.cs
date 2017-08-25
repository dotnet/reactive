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

        protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(_resultSelector, observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(_first, _second);

        internal sealed class _ : Sink<TResult>
        {
            private readonly Func<TFirst, TSecond, TResult> _resultSelector;

            public _(Func<TFirst, TSecond, TResult> resultSelector, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _resultSelector = resultSelector;
            }

            private object _gate;

            public IDisposable Run(IObservable<TFirst> first, IObservable<TSecond> second)
            {
                _gate = new object();

                var fstSubscription = new SingleAssignmentDisposable();
                var sndSubscription = new SingleAssignmentDisposable();

                var fstO = new FirstObserver(this, fstSubscription);
                var sndO = new SecondObserver(this, sndSubscription);

                fstO.Other = sndO;
                sndO.Other = fstO;

                fstSubscription.Disposable = first.SubscribeSafe(fstO);
                sndSubscription.Disposable = second.SubscribeSafe(sndO);

                return StableCompositeDisposable.Create(fstSubscription, sndSubscription);
            }

            private sealed class FirstObserver : IObserver<TFirst>
            {
                private readonly _ _parent;
                private readonly IDisposable _self;
                private SecondObserver _other;

                public FirstObserver(_ parent, IDisposable self)
                {
                    _parent = parent;
                    _self = self;
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
                            var res = default(TResult);
                            try
                            {
                                res = _parent._resultSelector(value, _other.Value);
                            }
                            catch (Exception ex)
                            {
                                _parent._observer.OnError(ex);
                                _parent.Dispose();
                                return;
                            }

                            _parent._observer.OnNext(res);
                        }
                        else if (_other.Done)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                            return;
                        }
                    }
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
                    lock (_parent._gate)
                    {
                        Done = true;

                        if (_other.Done)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                            return;
                        }
                        else
                        {
                            _self.Dispose();
                        }
                    }
                }
            }

            private sealed class SecondObserver : IObserver<TSecond>
            {
                private readonly _ _parent;
                private readonly IDisposable _self;
                private FirstObserver _other;

                public SecondObserver(_ parent, IDisposable self)
                {
                    _parent = parent;
                    _self = self;
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
                            var res = default(TResult);
                            try
                            {
                                res = _parent._resultSelector(_other.Value, value);
                            }
                            catch (Exception ex)
                            {
                                _parent._observer.OnError(ex);
                                _parent.Dispose();
                                return;
                            }

                            _parent._observer.OnNext(res);
                        }
                        else if (_other.Done)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                            return;
                        }
                    }
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
                    lock (_parent._gate)
                    {
                        Done = true;

                        if (_other.Done)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                            return;
                        }
                        else
                        {
                            _self.Dispose();
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

    internal abstract class CombineLatestSink<TResult> : Sink<TResult>, ICombineLatest
    {
        protected readonly object _gate;

        private bool _hasValueAll;
        private readonly bool[] _hasValue;
        private readonly bool[] _isDone;

        public CombineLatestSink(int arity, IObserver<TResult> observer, IDisposable cancel)
            : base(observer, cancel)
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
                var res = default(TResult);
                try
                {
                    res = GetResult();
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
                    base.Dispose();
                    return;
                }

                base._observer.OnNext(res);
            }
            else
            {
                var allOthersDone = true;
                for (int i = 0; i < _isDone.Length; i++)
                {
                    if (i != index && !_isDone[i])
                    {
                        allOthersDone = false;
                        break;
                    }
                }

                if (allOthersDone)
                {
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }

        protected abstract TResult GetResult();

        public void Fail(Exception error)
        {
            base._observer.OnError(error);
            base.Dispose();
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
                base._observer.OnCompleted();
                base.Dispose();
                return;
            }
        }
    }

    internal sealed class CombineLatestObserver<T> : IObserver<T>
    {
        private readonly object _gate;
        private readonly ICombineLatest _parent;
        private readonly int _index;
        private readonly IDisposable _self;
        private T _value;

        public CombineLatestObserver(object gate, ICombineLatest parent, int index, IDisposable self)
        {
            _gate = gate;
            _parent = parent;
            _index = index;
            _self = self;
        }

        public T Value => _value;

        public void OnNext(T value)
        {
            lock (_gate)
            {
                _value = value;
                _parent.Next(_index);
            }
        }

        public void OnError(Exception error)
        {
            _self.Dispose();

            lock (_gate)
            {
                _parent.Fail(error);
            }
        }

        public void OnCompleted()
        {
            _self.Dispose();

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

        protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(_resultSelector, observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(_sources);

        internal sealed class _ : Sink<TResult>
        {
            private readonly Func<IList<TSource>, TResult> _resultSelector;

            public _(Func<IList<TSource>, TResult> resultSelector, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _resultSelector = resultSelector;
            }

            private object _gate;
            private bool[] _hasValue;
            private bool _hasValueAll;
            private List<TSource> _values;
            private bool[] _isDone;
            private IDisposable[] _subscriptions;

            public IDisposable Run(IEnumerable<IObservable<TSource>> sources)
            {
                var srcs = sources.ToArray();

                var N = srcs.Length;

                _hasValue = new bool[N];
                _hasValueAll = false;

                _values = new List<TSource>(N);
                for (int i = 0; i < N; i++)
                    _values.Add(default(TSource));

                _isDone = new bool[N];

                _subscriptions = new IDisposable[N];

                _gate = new object();

                for (int i = 0; i < N; i++)
                {
                    var j = i;

                    var d = new SingleAssignmentDisposable();
                    _subscriptions[j] = d;

                    var o = new SourceObserver(this, j);
                    d.Disposable = srcs[j].SubscribeSafe(o);
                }

                return StableCompositeDisposable.Create(_subscriptions);
            }

            private void OnNext(int index, TSource value)
            {
                lock (_gate)
                {
                    _values[index] = value;

                    _hasValue[index] = true;

                    if (_hasValueAll || (_hasValueAll = _hasValue.All()))
                    {
                        var res = default(TResult);
                        try
                        {
                            res = _resultSelector(new ReadOnlyCollection<TSource>(_values));
                        }
                        catch (Exception ex)
                        {
                            base._observer.OnError(ex);
                            base.Dispose();
                            return;
                        }

                        _observer.OnNext(res);
                    }
                    else if (_isDone.AllExcept(index))
                    {
                        base._observer.OnCompleted();
                        base.Dispose();
                        return;
                    }
                }
            }

            private void OnError(Exception error)
            {
                lock (_gate)
                {
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            private void OnCompleted(int index)
            {
                lock (_gate)
                {
                    _isDone[index] = true;

                    if (_isDone.All())
                    {
                        base._observer.OnCompleted();
                        base.Dispose();
                        return;
                    }
                    else
                    {
                        _subscriptions[index].Dispose();
                    }
                }
            }

            private static bool All(bool[] values)
            {
                foreach (var value in values)
                {
                    if (!value)
                    {
                        return false;
                    }
                }

                return true;
            }

            private static bool AllExcept(bool[] values, int index)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    if (i != index)
                    {
                        if (!values[i])
                        {
                            return false;
                        }
                    }
                }

                return true;
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
