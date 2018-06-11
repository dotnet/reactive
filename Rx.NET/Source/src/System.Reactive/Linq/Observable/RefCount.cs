// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class RefCount<TSource> : Producer<TSource, RefCount<TSource>._>
    {
        private readonly IConnectableObservable<TSource> _source;

        private readonly object _gate;
        private int _count;
        private IDisposable _connectableSubscription;

        public RefCount(IConnectableObservable<TSource> source)
        {
            _source = source;
            _gate = new object();
            _count = 0;
            _connectableSubscription = default(IDisposable);
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new _(observer, this);

        protected override void Run(_ sink) => sink.Run();

        internal sealed class _ : IdentitySink<TSource>
        {
            readonly RefCount<TSource> _parent;

            public _(IObserver<TSource> observer, RefCount<TSource> parent)
                : base(observer)
            {
                this._parent = parent;
            }

            public void Run()
            {
                base.Run(_parent._source);

                lock (_parent._gate)
                {
                    if (++_parent._count == 1)
                    {
                        // We need to set _connectableSubscription to something
                        // before Connect because if Connect terminates synchronously,
                        // Dispose(bool) gets executed and will try to dispose
                        // _connectableSubscription of null.
                        // ?.Dispose() is no good because the dispose action has to be
                        // executed anyway.
                        // We can't inline SAD either because the IDisposable of Connect
                        // may belong to the wrong connection.
                        var sad = new SingleAssignmentDisposable();
                        _parent._connectableSubscription = sad;

                        sad.Disposable = _parent._source.Connect();
                    }
                }
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (disposing)
                {
                    lock (_parent._gate)
                    {
                        if (--_parent._count == 0)
                        {
                            _parent._connectableSubscription.Dispose();
                        }
                    }
                }
            }
        }
    }
}
