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
        private readonly IObservable<T>[] sources;

        public AmbManyArray(IObservable<T>[] sources)
        {
            this.sources = sources;
        }

        protected override IDisposable Run(IObserver<T> observer)
        {
            return AmbCoordinator<T>.Create(observer, sources);
        }
    }

    internal sealed class AmbManyEnumerable<T> : BasicProducer<T>
    {
        private readonly IEnumerable<IObservable<T>> sources;

        public AmbManyEnumerable(IEnumerable<IObservable<T>> sources)
        {
            this.sources = sources;
        }

        protected override IDisposable Run(IObserver<T> observer)
        {
            var sourcesEnumerable = this.sources;
            var sources = default(IObservable<T>[]);

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
        private readonly IObserver<T> downstream;
        private readonly InnerObserver[] observers;
        private int winner;

        internal AmbCoordinator(IObserver<T> downstream, int n)
        {
            this.downstream = downstream;
            var o = new InnerObserver[n];
            for (var i = 0; i < n; i++)
            {
                o[i] = new InnerObserver(this, i);
            }
            observers = o;
            Volatile.Write(ref winner, -1);
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
            for (var i = 0; i < observers.Length; i++)
            {
                var inner = Volatile.Read(ref observers[i]);
                if (inner == null)
                {
                    break;
                }
                inner.OnSubscribe(sources[i].Subscribe(inner));
            }
        }

        public void Dispose()
        {
            for (var i = 0; i < observers.Length; i++)
            {
                Interlocked.Exchange(ref observers[i], null)?.Dispose();
            }
        }

        private bool TryWin(int index)
        {
            if (Volatile.Read(ref winner) == -1 && Interlocked.CompareExchange(ref winner, index, -1) == -1)
            {
                for (var i = 0; i < observers.Length; i++)
                {
                    if (index != i)
                    {
                        Interlocked.Exchange(ref observers[i], null)?.Dispose();
                    }
                }
                return true;
            }
            return false;
        }

        internal sealed class InnerObserver : IObserver<T>, IDisposable
        {
            private readonly IObserver<T> downstream;
            private readonly AmbCoordinator<T> parent;
            private readonly int index;
            private IDisposable upstream;
            private bool won;

            public InnerObserver(AmbCoordinator<T> parent, int index)
            {
                downstream = parent.downstream;
                this.parent = parent;
                this.index = index;
            }

            public void Dispose()
            {
                Disposable.TryDispose(ref upstream);
            }

            public void OnCompleted()
            {
                if (won)
                {
                    downstream.OnCompleted();
                }
                else
                if (parent.TryWin(index))
                {
                    won = true;
                    downstream.OnCompleted();
                }
                Dispose();
            }

            public void OnError(Exception error)
            {
                if (won)
                {
                    downstream.OnError(error);
                }
                else
                if (parent.TryWin(index))
                {
                    won = true;
                    downstream.OnError(error);
                }
                Dispose();
            }

            public void OnNext(T value)
            {
                if (won)
                {
                    downstream.OnNext(value);
                }
                else
                if (parent.TryWin(index))
                {
                    won = true;
                    downstream.OnNext(value);
                }
                else
                {
                    Dispose();
                }
            }

            internal void OnSubscribe(IDisposable d)
            {
                Disposable.SetSingle(ref upstream, d);
            }
        }

    }
}
