// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    static internal class AppendPrepend
    {
        internal interface IAppendPrepend<TSource> : IObservable<TSource>
        {
            IAppendPrepend<TSource> Append(TSource value);
            IAppendPrepend<TSource> Prepend(TSource value);
            IScheduler Scheduler { get; }
        }

        internal sealed class AppendPrependSingle<TSource> : Producer<TSource, AppendPrependSingle<TSource>._>, IAppendPrepend<TSource>
        {
            private readonly IObservable<TSource> _source;
            private readonly TSource _value;
            private readonly bool _append;

            public IScheduler Scheduler { get; }

            public AppendPrependSingle(IObservable<TSource> source, TSource value, IScheduler scheduler, bool append)
            {
                _source = source;
                _value = value;
                _append = append;
                Scheduler = scheduler;
            }

            public IAppendPrepend<TSource> Append(TSource value)
            {
                var prev = new Node<TSource>(_value);

                if (_append)
                    return new AppendPrependMultiple<TSource>(_source,
                        null, new Node<TSource>(prev, value), Scheduler);
                else
                    return new AppendPrependMultiple<TSource>(_source,
                        prev, new Node<TSource>(value), Scheduler);
            }

            public IAppendPrepend<TSource> Prepend(TSource value)
            {
                var prev = new Node<TSource>(_value);

                if (_append)
                    return new AppendPrependMultiple<TSource>(_source,
                        new Node<TSource>(value), prev, Scheduler);
                else
                    return new AppendPrependMultiple<TSource>(_source,
                        new Node<TSource>(prev, value), null, Scheduler);
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(this, observer);

            protected override void Run(_ sink) => sink.Run();

            internal sealed class _ : IdentitySink<TSource>
            {
                private readonly IObservable<TSource> _source;
                private readonly TSource _value;
                private readonly IScheduler _scheduler;
                private readonly bool _append;
                private IDisposable _schedulerDisposable;

                public _(AppendPrependSingle<TSource> parent, IObserver<TSource> observer)
                    : base(observer)
                {
                    _source = parent._source;
                    _value = parent._value;
                    _scheduler = parent.Scheduler;
                    _append = parent._append;
                }

                public void Run()
                {
                    var disp = _append
                        ? _source.SubscribeSafe(this)
                        : _scheduler.Schedule(this, PrependValue);

                    SetUpstream(disp);
                }

                private static IDisposable PrependValue(IScheduler scheduler, _ sink)
                {
                    sink.ForwardOnNext(sink._value);
                    return sink._source.SubscribeSafe(sink);
                }

                public override void OnCompleted()
                {
                    if (_append)
                    {
                        var disposable = _scheduler.Schedule(this, AppendValue);
                        Disposable.TrySetSingle(ref _schedulerDisposable, disposable);
                    }
                    else
                    {
                        ForwardOnCompleted();
                    }
                }

                private static IDisposable AppendValue(IScheduler scheduler, _ sink)
                {
                    sink.ForwardOnNext(sink._value);
                    sink.ForwardOnCompleted();
                    return Disposable.Empty;
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        Disposable.TryDispose(ref _schedulerDisposable);
                    }
                    base.Dispose(disposing);
                }
            }
        }

        private sealed class AppendPrependMultiple<TSource> : Producer<TSource, AppendPrependMultiple<TSource>._>, IAppendPrepend<TSource>
        {
            private readonly IObservable<TSource> _source;
            private readonly Node<TSource> _appends;
            private readonly Node<TSource> _prepends;

            public IScheduler Scheduler { get; }

            public AppendPrependMultiple(IObservable<TSource> source, Node<TSource> prepends, Node<TSource> appends, IScheduler scheduler)
            {
                _source = source;
                _appends = appends;
                _prepends = prepends;
                Scheduler = scheduler;
            }

            public IAppendPrepend<TSource> Append(TSource value)
            {
                return new AppendPrependMultiple<TSource>(_source,
                    _prepends, new Node<TSource>(_appends, value), Scheduler);
            }

            public IAppendPrepend<TSource> Prepend(TSource value)
            {
                return new AppendPrependMultiple<TSource>(_source,
                    new Node<TSource>(_prepends, value), _appends, Scheduler);
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(this, observer);

            protected override void Run(_ sink) => sink.Run();

            // The sink is based on the sink of the ToObervalbe class and does basically
            // the same twice, once for the append list and once for the prepend list.
            // Inbetween it forwards the values of the source class.
            //
            internal sealed class _ : IdentitySink<TSource>
            {
                private readonly IObservable<TSource> _source;
                private readonly TSource[] _prepends;
                private readonly TSource[] _appends;
                private readonly IScheduler _scheduler;
                private IDisposable _schedulerDisposable;

                public _(AppendPrependMultiple<TSource> parent, IObserver<TSource> observer)
                    : base(observer)
                {
                    _source = parent._source;
                    _scheduler = parent.Scheduler;

                    if (parent._prepends != null)
                        _prepends = parent._prepends.ToArray();

                    if (parent._appends != null)
                        _appends = parent._appends.ToReverseArray();
                }

                public void Run()
                {
                    if (_prepends != null)
                    {
                        var disposable = Schedule(_prepends, s => s.SetUpstream(s._source.SubscribeSafe(s)));
                        Disposable.TrySetSerial(ref _schedulerDisposable, disposable);
                    }
                    else
                    {
                        SetUpstream(_source.SubscribeSafe(this));
                    }
                }

                public override void OnCompleted()
                {
                    if (_appends != null)
                    {
                        var disposable = Schedule(_appends, s => s.ForwardOnCompleted());
                        Disposable.TrySetSerial(ref _schedulerDisposable, disposable);
                    }
                    else
                    {
                        ForwardOnCompleted();
                    }
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        Disposable.TryDispose(ref _schedulerDisposable);
                    }
                    base.Dispose(disposing);
                }

                private IDisposable Schedule(TSource[] array, Action<_> continueWith)
                {
                    var longRunning = _scheduler.AsLongRunning();
                    if (longRunning != null)
                    {
                        //
                        // Long-running schedulers have the contract they should *never* prevent
                        // the work from starting, such that the scheduled work has the chance
                        // to observe the cancellation and perform proper clean-up. In this case,
                        // we're sure Loop will be entered, allowing us to dispose the enumerator.
                        //
                        return longRunning.ScheduleLongRunning(new State(null, this, array, continueWith), Loop);
                    }
                    else
                    {
                        //
                        // We never allow the scheduled work to be cancelled. Instead, the flag
                        // is used to have LoopRec bail out and perform proper clean-up of the
                        // enumerator.
                        //
                        var flag = new BooleanDisposable();
                        _scheduler.Schedule(new State(flag, this, array, continueWith), LoopRec);
                        return flag;
                    }
                }

                private sealed class State
                {
                    public readonly _ _sink;
                    public readonly ICancelable _flag;
                    public readonly TSource[] _array;
                    public readonly Action<_> _continue;
                    public int _current;

                    public State(ICancelable flag, _ sink, TSource[] array, Action<_> c)
                    {
                        _sink = sink;
                        _flag = flag;
                        _continue = c;
                        _array = array;
                    }
                }

                private void LoopRec(State state, Action<State> recurse)
                {
                    if (state._flag.IsDisposed)
                        return;

                    var current = state._array[state._current];
                    ForwardOnNext(current);

                    state._current++;

                    if (state._current == state._array.Length)
                    {
                        state._continue(state._sink);
                        return;
                    }

                    recurse(state);
                }

                private void Loop(State state, ICancelable cancel)
                {
                    var array = state._array;
                    int i = 0;

                    while (!cancel.IsDisposed)
                    {
                        ForwardOnNext(array[i]);
                        i++;

                        if (i == array.Length)
                        {
                            state._continue(state._sink);
                            break;
                        }
                    }

                    base.Dispose();
                }
            }
        }

        private sealed class Node<T>
        {
            public readonly Node<T> Parent;
            public readonly T Value;
            public readonly int Count;

            public Node(T value) 
                : this(null, value)
            {
            }

            public Node(Node<T> parent, T value)
            {
                Parent = parent;
                Value = value; 

                if (parent == null)
                    Count = 1;
                else
                {
                    if (parent.Count == int.MaxValue)
                        throw new NotSupportedException($"Consecutive appends or prepends with a count of more than int.MaxValue ({int.MaxValue}) are not supported.");

                    Count = parent.Count + 1;
                }
            }

            public T[] ToArray()
            {
                var array = new T[Count];
                var current = this;
                for (int i = 0; i < Count; i++)
                {
                    array[i] = current.Value;
                    current = current.Parent;
                }
                return array;
            }

            public T[] ToReverseArray()
            {
                var array = new T[Count];
                var current = this;
                for (int i = Count - 1; i >= 0; i--)
                {
                    array[i] = current.Value;
                    current = current.Parent;
                }
                return array;
            }
        }
    }
}
