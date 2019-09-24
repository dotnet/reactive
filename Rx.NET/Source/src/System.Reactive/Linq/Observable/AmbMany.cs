// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class AmbManyArray<T> : BasicProducer<T>
    {
        private readonly IObservable<T>[] _sources;

        public AmbManyArray(IObservable<T>[] sources)
        {
            _sources = sources;
        }

        protected override IDisposable Run(IObserver<T> observer)
        {
            return AmbCoordinator<T>.Create(observer, _sources);
        }
    }

    internal sealed class AmbManyEnumerable<T> : BasicProducer<T>
    {
        private readonly IEnumerable<IObservable<T>> _sources;

        public AmbManyEnumerable(IEnumerable<IObservable<T>> sources)
        {
            _sources = sources;
        }

        protected override IDisposable Run(IObserver<T> observer)
        {
            var sourcesEnumerable = _sources;

            IObservable<T>[] sources;
            try
            {
                sources = sourcesEnumerable.ToArray();
            }
            catch (Exception ex)
            {
                observer.OnError(ex);
                return Disposable.Empty;
            }

            return AmbCoordinator<T>.Create(observer, sources);
        }
    }

    internal sealed class AmbCoordinator<T> : IDisposable
    {
        private readonly IObserver<T> _downstream;
        private readonly InnerObserver[] _observers;
        private int _winner;

        internal AmbCoordinator(IObserver<T> downstream, int n)
        {
            _downstream = downstream;
            var o = new InnerObserver[n];
            for (var i = 0; i < n; i++)
            {
                o[i] = new InnerObserver(this, i);
            }
            _observers = o;
            Volatile.Write(ref _winner, -1);
        }

        internal static IDisposable Create(IObserver<T> observer, IObservable<T>[] sources)
        {
            var n = sources.Length;
            if (n == 0)
            {
                observer.OnCompleted();
                return Disposable.Empty;
            }

            if (n == 1)
            {
                return sources[0].Subscribe(observer);
            }

            var parent = new AmbCoordinator<T>(observer, n);

            parent.Subscribe(sources);

            return parent;
        }

        internal void Subscribe(IObservable<T>[] sources)
        {
            for (var i = 0; i < _observers.Length; i++)
            {
                var inner = Volatile.Read(ref _observers[i]);
                if (inner == null)
                {
                    break;
                }
                inner.Run(sources[i]);
            }
        }

        public void Dispose()
        {
            for (var i = 0; i < _observers.Length; i++)
            {
                Interlocked.Exchange(ref _observers[i], null)?.Dispose();
            }
        }

        private bool TryWin(int index)
        {
            if (Volatile.Read(ref _winner) == -1 && Interlocked.CompareExchange(ref _winner, index, -1) == -1)
            {
                for (var i = 0; i < _observers.Length; i++)
                {
                    if (index != i)
                    {
                        Interlocked.Exchange(ref _observers[i], null)?.Dispose();
                    }
                }
                return true;
            }
            return false;
        }

        internal sealed class InnerObserver : IdentitySink<T>
        {
            private readonly AmbCoordinator<T> _parent;
            private readonly int _index;
            private bool _won;

            public InnerObserver(AmbCoordinator<T> parent, int index) : base(parent._downstream)
            {
                _parent = parent;
                _index = index;
            }

            public override void OnCompleted()
            {
                if (_won)
                {
                    ForwardOnCompleted();
                }
                else if (_parent.TryWin(_index))
                {
                    _won = true;
                    ForwardOnCompleted();
                }
                else
                {
                    Dispose();
                }
            }

            public override void OnError(Exception error)
            {
                if (_won)
                {
                    ForwardOnError(error);
                }
                else if (_parent.TryWin(_index))
                {
                    _won = true;
                    ForwardOnError(error);
                }
                else
                {
                    Dispose();
                }
            }

            public override void OnNext(T value)
            {
                if (_won)
                {
                    ForwardOnNext(value);
                }
                else if (_parent.TryWin(_index))
                {
                    _won = true;
                    ForwardOnNext(value);
                }
                else
                {
                    Dispose();
                }
            }
        }
    }
}
