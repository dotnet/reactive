// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class AppendPrepend<TSource>
    {
        internal interface IAppendPrepend : IObservable<TSource>
        {
            IAppendPrepend Append(TSource value);
            IAppendPrepend Prepend(TSource value);
            IScheduler Scheduler { get; }
        }

        internal abstract class SingleBase<TSink> : Producer<TSource, TSink>, IAppendPrepend
            where TSink : IDisposable
        {
            protected readonly IObservable<TSource> _source;
            protected readonly TSource _value;
            protected readonly bool _append;

            public abstract IScheduler Scheduler { get; }

            public SingleBase(IObservable<TSource> source, TSource value, bool append)
            {
                _source = source;
                _value = value;
                _append = append;
            }

            public IAppendPrepend Append(TSource value)
            {
                var prev = new Node<TSource>(_value);

                Node<TSource> appendNode;
                Node<TSource> prependNode = null;

                if (_append)
                {
                    appendNode = new Node<TSource>(prev, value);
                }
                else
                {
                    prependNode = prev;
                    appendNode = new Node<TSource>(value);
                }

                return CreateAppendPrepend(prependNode, appendNode);
            }

            public IAppendPrepend Prepend(TSource value)
            {
                var prev = new Node<TSource>(_value);

                Node<TSource> appendNode = null;
                Node<TSource> prependNode;

                if (_append)
                {
                    prependNode = new Node<TSource>(value);
                    appendNode = prev;
                }
                else
                {
                    prependNode = new Node<TSource>(prev, value);
                }

                return CreateAppendPrepend(prependNode, appendNode);
            }

            private IAppendPrepend CreateAppendPrepend(Node<TSource> prepend, Node<TSource> append)
            {
                if (Scheduler is ISchedulerLongRunning longRunning)
                {
                    return new LongRunning(_source, prepend, append, Scheduler, longRunning);
                }

                return new Recursive(_source, prepend, append, Scheduler);
            }
        }


        internal sealed class SingleValue : SingleBase<SingleValue._>
        {
            public override IScheduler Scheduler { get; }

            public SingleValue(IObservable<TSource> source, TSource value, IScheduler scheduler, bool append)
                : base (source, value, append)
            {
                Scheduler = scheduler;
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

                public _(SingleValue parent, IObserver<TSource> observer)
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
                        : _scheduler.ScheduleAction(this, PrependValue);

                    SetUpstream(disp);
                }

                private static IDisposable PrependValue(_ sink)
                {
                    sink.ForwardOnNext(sink._value);
                    return sink._source.SubscribeSafe(sink);
                }

                public override void OnCompleted()
                {
                    if (_append)
                    {
                        var disposable = _scheduler.ScheduleAction(this, AppendValue);
                        Disposable.TrySetSingle(ref _schedulerDisposable, disposable);
                    }
                    else
                    {
                        ForwardOnCompleted();
                    }
                }

                private static void AppendValue(_ sink)
                {
                    sink.ForwardOnNext(sink._value);
                    sink.ForwardOnCompleted();
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

        private sealed class Recursive : Producer<TSource, Recursive._>, IAppendPrepend
        {
            private readonly IObservable<TSource> _source;
            private readonly Node<TSource> _appends;
            private readonly Node<TSource> _prepends;

            public IScheduler Scheduler { get; }

            public Recursive(IObservable<TSource> source, Node<TSource> prepends, Node<TSource> appends, IScheduler scheduler)
            {
                _source = source;
                _appends = appends;
                _prepends = prepends;
                Scheduler = scheduler;
            }

            public IAppendPrepend Append(TSource value)
            {
                return new Recursive(_source,
                    _prepends, new Node<TSource>(_appends, value), Scheduler);
            }

            public IAppendPrepend Prepend(TSource value)
            {
                return new Recursive(_source,
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
                private readonly Node<TSource> _appends;
                private readonly IScheduler _scheduler;

                private Node<TSource> _currentPrependNode;
                private TSource[] _appendArray;
                private int _currentAppendIndex;
                private volatile bool _disposed;

                public _(Recursive parent, IObserver<TSource> observer)
                    : base(observer)
                {
                    _source = parent._source;
                    _scheduler = parent.Scheduler;
                    _currentPrependNode = parent._prepends;
                    _appends = parent._appends;
                }

                public void Run()
                {
                    if (_currentPrependNode == null)
                    {
                        SetUpstream(_source.SubscribeSafe(this));
                    }
                    else
                    {
                        //
                        // We never allow the scheduled work to be cancelled. Instead, the _disposed flag
                        // is used to have PrependValues() bail out.
                        //
                        _scheduler.Schedule(this, (innerScheduler, @this) => @this.PrependValues(innerScheduler));
                    }
                }

                public override void OnCompleted()
                {
                    if (_appends == null)
                    {
                        ForwardOnCompleted();
                    }
                    else
                    {
                        _appendArray = _appends.ToReverseArray();
                        //
                        // We never allow the scheduled work to be cancelled. Instead, the _disposed flag
                        // is used to have `AppendValues` bail out.
                        //
                        _scheduler.Schedule(this, (innerScheduler, @this) => @this.AppendValues(innerScheduler));
                    }
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        _disposed = true;
                    }

                    base.Dispose(disposing);
                }

                private IDisposable PrependValues(IScheduler scheduler)
                {
                    if (_disposed)
                    {
                        return Disposable.Empty;
                    }

                    var current = _currentPrependNode.Value;
                    ForwardOnNext(current);

                    _currentPrependNode = _currentPrependNode.Parent;
                    if (_currentPrependNode == null)
                    {
                        SetUpstream(_source.SubscribeSafe(this));
                    }
                    else
                    { 
                        //
                        // We never allow the scheduled work to be cancelled. Instead, the _disposed flag
                        // is used to have PrependValues() bail out.
                        //
                        scheduler.Schedule(this, (innerScheduler, @this) => @this.PrependValues(innerScheduler));
                    }

                    return Disposable.Empty;
                }

                private IDisposable AppendValues(IScheduler scheduler)
                {
                    if (_disposed)
                    {
                        return Disposable.Empty;
                    }

                    var current = _appendArray[_currentAppendIndex];
                    ForwardOnNext(current);

                    _currentAppendIndex++;

                    if (_currentAppendIndex == _appendArray.Length)
                    {
                        ForwardOnCompleted();
                    }
                    else
                    { 
                        //
                        // We never allow the scheduled work to be cancelled. Instead, the _disposed flag
                        // is used to have AppendValues() bail out.
                        //
                        scheduler.Schedule(this, (innerScheduler, @this) => @this.AppendValues(innerScheduler));
                    }

                    return Disposable.Empty;
                }
            }
        }

        private sealed class LongRunning : Producer<TSource, LongRunning._>, IAppendPrepend
        {
            private readonly IObservable<TSource> _source;
            private readonly Node<TSource> _appends;
            private readonly Node<TSource> _prepends;
            private readonly ISchedulerLongRunning _longRunningScheduler;

            public IScheduler Scheduler { get; }

            public LongRunning(IObservable<TSource> source, Node<TSource> prepends, Node<TSource> appends, IScheduler scheduler, ISchedulerLongRunning longRunningScheduler)
            {
                _source = source;
                _appends = appends;
                _prepends = prepends;
                Scheduler = scheduler;
                _longRunningScheduler = longRunningScheduler;
            }

            public IAppendPrepend Append(TSource value)
            {
                return new LongRunning(_source,
                    _prepends, new Node<TSource>(_appends, value), Scheduler, _longRunningScheduler);
            }

            public IAppendPrepend Prepend(TSource value)
            {
                return new LongRunning(_source,
                    new Node<TSource>(_prepends, value), _appends, Scheduler, _longRunningScheduler);
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
                private readonly Node<TSource> _prepends; 
                private readonly Node<TSource> _appends;
                private readonly ISchedulerLongRunning _scheduler;

                private IDisposable _schedulerDisposable;

                public _(LongRunning parent, IObserver<TSource> observer)
                    : base(observer)
                {
                    _source = parent._source;
                    _scheduler = parent._longRunningScheduler;
                    _prepends = parent._prepends;
                    _appends = parent._appends;
                }

                public void Run()
                {
                    if (_prepends == null)
                    {
                        SetUpstream(_source.SubscribeSafe(this));
                    }
                    else
                    {
                        var disposable = _scheduler.ScheduleLongRunning(this, (@this, cancel) => @this.PrependValues(cancel));
                        Disposable.TrySetSingle(ref _schedulerDisposable, disposable);
                    }
                }

                public override void OnCompleted()
                {
                    if (_appends == null)
                    {
                        ForwardOnCompleted();
                    }
                    else
                    {
                        var disposable = _scheduler.ScheduleLongRunning(this, (@this, cancel) => @this.AppendValues(cancel));
                        Disposable.TrySetSerial(ref _schedulerDisposable, disposable);
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

                private void PrependValues(ICancelable cancel)
                {
                    var current = _prepends;

                    while (!cancel.IsDisposed)
                    {
                        ForwardOnNext(current.Value);
                        current = current.Parent;

                        if (current == null)
                        {
                            SetUpstream(_source.SubscribeSafe(this));
                            break;
                        }
                    }
                }

                private void AppendValues(ICancelable cancel)
                {
                    var array = _appends.ToReverseArray();
                    var i = 0;

                    while (!cancel.IsDisposed)
                    {
                        ForwardOnNext(array[i]);
                        i++;

                        if (i == array.Length)
                        {
                            ForwardOnCompleted();
                            break;
                        }
                    }
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
                {
                    Count = 1;
                }
                else
                {
                    if (parent.Count == int.MaxValue)
                    {
                        throw new NotSupportedException($"Consecutive appends or prepends with a count of more than int.MaxValue ({int.MaxValue}) are not supported.");
                    }

                    Count = parent.Count + 1;
                }
            }

            public T[] ToReverseArray()
            {
                var array = new T[Count];
                var current = this;
                for (var i = Count - 1; i >= 0; i--)
                {
                    array[i] = current.Value;
                    current = current.Parent;
                }
                return array;
            }
        }

        internal sealed class SingleImmediate : SingleBase<SingleImmediate._>
        {
            public override IScheduler Scheduler => ImmediateScheduler.Instance;

            public SingleImmediate(IObservable<TSource> source, TSource value, bool append)
                : base(source, value, append)
            {
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(this, observer);

            protected override void Run(_ sink) => sink.Run();

            internal sealed class _ : IdentitySink<TSource>
            {
                private readonly IObservable<TSource> _source;
                private readonly TSource _value;
                private readonly bool _append;

                public _(SingleImmediate parent, IObserver<TSource> observer)
                    : base(observer)
                {
                    _source = parent._source;
                    _value = parent._value;
                    _append = parent._append;
                }

                public void Run()
                {
                    if (!_append)
                    {
                        ForwardOnNext(_value);
                    }
                    Run(_source);
                }

                public override void OnCompleted()
                {
                    if (_append)
                    {
                        ForwardOnNext(_value);
                    }
                    ForwardOnCompleted();
                }
            }
        }
    }
}
