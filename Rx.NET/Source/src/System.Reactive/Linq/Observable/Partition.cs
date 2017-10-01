// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Partition<TSource>
    {
        private readonly PartitionProducer[] _producers = new PartitionProducer[2];
        private readonly InnerSink _innerSink;

        public Partition(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            _producers[0] = new PartitionProducer(this, 0);
            _producers[1] = new PartitionProducer(this, 1);
            _innerSink = new InnerSink(source, predicate);
        }

        public Tuple<IObservable<TSource>, IObservable<TSource>> AsTuple()
        {
            return Tuple.Create<IObservable<TSource>, IObservable<TSource>>(_producers[0], _producers[1]);
        }

        // An outer sink is created for each subscription on either of the subscriptions
        internal sealed class OuterSink : Sink<TSource>, IObserver<TSource>
        {
            private readonly Partition<TSource> _parent;
            private readonly int _index;

            public OuterSink(Partition<TSource> parent, int index, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _index = index;
            }

            public IDisposable Run()
            {
                return _parent._innerSink.Run(this, _index);
            }

            public void OnNext(TSource value)
            {
                base._observer.OnNext(value);
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

        // One inner sink is created for the partition operation and is shared amongst all the outer sinks
        // The inner sink is responsible for subscribing to the source sequence
        internal sealed class InnerSink : IObserver<TSource>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, bool> _predicate;
            private readonly List<OuterSink>[] _outerSinks = new List<OuterSink>[2];
            private IDisposable _sourceSubscription;

            public InnerSink(IObservable<TSource> source, Func<TSource, bool> predicate)
            {
                _source = source;
                _predicate = predicate;
                _outerSinks[0] = new List<OuterSink>();
                _outerSinks[1] = new List<OuterSink>();
            }

            public IDisposable Run(OuterSink outerSink, int index)
            {
                // Save a reference to the outer sink
                _outerSinks[index].Add(outerSink);
                var subscription = Disposable.Create(() =>
                {
                    for (int i = 0; i < _outerSinks[index].Count; i++)
                    {
                        if (_outerSinks[index][i] == outerSink)
                        {
                            _outerSinks[index].RemoveAt(i);
                            break;
                        }
                    }

                    // Dispose the source subscription if nothing else is listening
                    if (_sourceSubscription != null && _outerSinks[0].Count == 0 && _outerSinks[1].Count == 0)
                    {
                        _sourceSubscription.Dispose();
                        _sourceSubscription = null;
                    }
                });

                // Subscribe to the source once both partitions have subscriptions
                if (_sourceSubscription == null && _outerSinks[0].Count > 0 && _outerSinks[1].Count > 0)
                {
                    _sourceSubscription = _source.SubscribeSafe(this);
                }

                return subscription;
            }

            public void OnNext(TSource value)
            {
                bool result;
                try
                {
                    result = _predicate(value);
                }
                catch (Exception error)
                {
                    OnError(error);
                    return;
                }

                if (result)
                {
                    foreach (var sink in _outerSinks[0])
                    {
                        sink.OnNext(value);
                    }
                }
                else
                {
                    foreach (var sink in _outerSinks[1])
                    {
                        sink.OnNext(value);
                    }
                }
            }

            public void OnError(Exception error)
            {
                // The sinks should remove themselves from the lists as they are disposed, so iterate backwards to make this safe
                for (int i = _outerSinks[0].Count - 1; i >= 0; i--)
                {
                    _outerSinks[0][i].OnError(error);
                }
                for (int i = _outerSinks[1].Count - 1; i >= 0; i--)
                {
                    _outerSinks[1][i].OnError(error);
                }
            }

            public void OnCompleted()
            {
                // The sinks should remove themselves from the lists as they are disposed, so iterate backwards to make this safe
                for (int i = _outerSinks[0].Count - 1; i >= 0; i--)
                {
                    _outerSinks[0][i].OnCompleted();
                }
                for (int i = _outerSinks[1].Count - 1; i >= 0; i--)
                {
                    _outerSinks[1][i].OnCompleted();
                }
            }
        }

        internal sealed class PartitionProducer : Producer<TSource, OuterSink>
        {
            private readonly Partition<TSource> _parent;
            private readonly int _index;

            public PartitionProducer(Partition<TSource> parent, int index)
            {
                _parent = parent;
                _index = index;
            }

            protected override OuterSink CreateSink(IObserver<TSource> observer, IDisposable cancel) => new OuterSink(_parent, _index, observer, cancel);

            protected override IDisposable Run(OuterSink sink) => sink.Run();
        }
    }
}
