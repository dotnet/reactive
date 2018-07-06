// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class RefCount<TSource>
    {
        internal sealed class Eager : Producer<TSource, Eager._>
        {
            private readonly IConnectableObservable<TSource> _source;

            private readonly object _gate;
            private int _count;
            private IDisposable _connectableSubscription;

            public Eager(IConnectableObservable<TSource> source)
            {
                _source = source;
                _gate = new object();
                _count = 0;
                _connectableSubscription = default(IDisposable);
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(observer);

            protected override void Run(_ sink) => sink.Run(this);

            internal sealed class _ : IdentitySink<TSource>
            {
                public _(IObserver<TSource> observer)
                    : base(observer)
                {
                }

                public void Run(Eager parent)
                {
                    var subscription = parent._source.SubscribeSafe(this);

                    lock (parent._gate)
                    {
                        if (++parent._count == 1)
                        {
                            parent._connectableSubscription = parent._source.Connect();
                        }
                    }

                    SetUpstream(Disposable.Create(() =>
                    {
                        subscription.Dispose();

                        lock (parent._gate)
                        {
                            if (--parent._count == 0)
                            {
                                parent._connectableSubscription.Dispose();
                            }
                        }
                    }));
                }
            }
        }

        internal sealed class Lazy : Producer<TSource, Lazy._>
        {
            private readonly object _gate;
            private readonly IScheduler _scheduler;
            private readonly TimeSpan _disconnectTime;
            private readonly IConnectableObservable<TSource> _source;
            private IDisposable _serial;
            private int _count;
            private IDisposable _connectableSubscription;

            public Lazy(IConnectableObservable<TSource> source, TimeSpan disconnectTime, IScheduler scheduler)
            {
                _source = source;
                _gate = new object();
                _disconnectTime = disconnectTime;
                _scheduler = scheduler;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(observer);

            protected override void Run(_ sink) => sink.Run(this);

            internal sealed class _ : IdentitySink<TSource>
            {
                public _(IObserver<TSource> observer)
                    : base(observer)
                {
                }

                public void Run(Lazy parent)
                {
                    var subscription = parent._source.SubscribeSafe(this);

                    lock (parent._gate)
                    {
                        if (++parent._count == 1)
                        {
                            if (parent._connectableSubscription == null)
                            {
                                parent._connectableSubscription = parent._source.Connect();
                            }

                            Disposable.TrySetSerial(ref parent._serial, new SingleAssignmentDisposable());
                        }
                    }

                    SetUpstream(Disposable.Create(
                        (parent, subscription),
                        tuple =>
                        {
                            var (closureParent, closureSubscription) = tuple;

                            closureSubscription.Dispose();

                            lock (closureParent._gate)
                            {
                                if (--closureParent._count == 0)
                                {
                                    var cancelable = (SingleAssignmentDisposable)Volatile.Read(ref closureParent._serial);

                                    cancelable.Disposable = closureParent._scheduler.ScheduleAction((cancelable, closureParent), closureParent._disconnectTime, tuple2 =>
                                    {
                                        lock (tuple2.closureParent._gate)
                                        {
                                            if (ReferenceEquals(Volatile.Read(ref tuple2.closureParent._serial), tuple2.cancelable))
                                            {
                                                tuple2.closureParent._connectableSubscription.Dispose();
                                                tuple2.closureParent._connectableSubscription = null;
                                            }
                                        }
                                    });
                                }
                            }
                        }));
                }
            }
        }
    }
}
