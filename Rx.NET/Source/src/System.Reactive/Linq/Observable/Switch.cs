// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Switch<TSource> : Producer<TSource, Switch<TSource>._>
    {
        private readonly IObservable<IObservable<TSource>> _sources;

        public Switch(IObservable<IObservable<TSource>> sources)
        {
            _sources = sources;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new(observer);

        protected override void Run(_ sink) => sink.Run(_sources);

        internal sealed class _ : Sink<IObservable<TSource>, TSource>
        {
            private readonly object _gate = new();

            public _(IObserver<TSource> observer)
                : base(observer)
            {
            }

            private SerialDisposableValue _innerSerialDisposable;
            private bool _isStopped;
            private ulong _latest;
            private bool _hasLatest;

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _innerSerialDisposable.Dispose();
                }

                base.Dispose(disposing);
            }

            public override void OnNext(IObservable<TSource> value)
            {
                ulong id;
                
                lock (_gate)
                {
                    id = unchecked(++_latest);
                    _hasLatest = true;
                }

                var innerObserver = new InnerObserver(this, id);

                _innerSerialDisposable.Disposable = innerObserver;
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
                lock (_gate)
                {
                    DisposeUpstream();

                    _isStopped = true;
                    if (!_hasLatest)
                    {
                        ForwardOnCompleted();
                    }
                }
            }

            private sealed class InnerObserver : SafeObserver<TSource>
            {
                private readonly _ _parent;
                private readonly ulong _id;

                public InnerObserver(_ parent, ulong id)
                {
                    _parent = parent;
                    _id = id;
                }

                public override void OnNext(TSource value)
                {
                    lock (_parent._gate)
                    {
                        if (_parent._latest == _id)
                        {
                            _parent.ForwardOnNext(value);
                        }
                    }
                }

                public override void OnError(Exception error)
                {
                    lock (_parent._gate)
                    {
                        Dispose();

                        if (_parent._latest == _id)
                        {
                            _parent.ForwardOnError(error);
                        }
                    }
                }

                public override void OnCompleted()
                {
                    lock (_parent._gate)
                    {
                        Dispose();

                        if (_parent._latest == _id)
                        {
                            _parent._hasLatest = false;

                            if (_parent._isStopped)
                            {
                                _parent.ForwardOnCompleted();
                            }
                        }
                    }
                }
            }
        }
    }
}
