// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Amb<TSource> : Producer<TSource, Amb<TSource>.AmbCoordinator>
    {
        private readonly IObservable<TSource> _left;
        private readonly IObservable<TSource> _right;

        public Amb(IObservable<TSource> left, IObservable<TSource> right)
        {
            _left = left;
            _right = right;
        }

        protected override AmbCoordinator CreateSink(IObserver<TSource> observer) => new AmbCoordinator(observer);

        protected override void Run(AmbCoordinator sink) => sink.Run(_left, _right);

        internal sealed class AmbCoordinator : IDisposable
        {
            private readonly AmbObserver leftObserver;
            private readonly AmbObserver rightObserver;
            private int winner;

            public AmbCoordinator(IObserver<TSource> observer)
            {
                leftObserver = new AmbObserver(observer, this, true);
                rightObserver = new AmbObserver(observer, this, false);
            }

            public void Run(IObservable<TSource> left, IObservable<TSource> right)
            {
                leftObserver.OnSubscribe(left.Subscribe(leftObserver));
                rightObserver.OnSubscribe(right.Subscribe(rightObserver));
            }

            public void Dispose()
            {
                leftObserver.Dispose();
                rightObserver.Dispose();
            }

            /// <summary>
            /// Try winning the race for the right of emission.
            /// </summary>
            /// <param name="isLeft">If true, the contender is the left source.</param>
            /// <returns>True if the contender has won the race.</returns>
            public bool TryWin(bool isLeft)
            {
                var index = isLeft ? 1 : 2;

                if (Volatile.Read(ref winner) == index)
                {
                    return true;
                }
                if (Interlocked.CompareExchange(ref winner, index, 0) == 0)
                {
                    (isLeft ? rightObserver : leftObserver).Dispose();
                    return true;
                }
                return false;
            }

            private sealed class AmbObserver : IObserver<TSource>, IDisposable
            {
                private readonly IObserver<TSource> downstream;
                private readonly AmbCoordinator parent;
                private readonly bool isLeft;
                private IDisposable upstream;

                /// <summary>
                /// If true, this observer won the race and now can emit
                /// on a fast path.
                /// </summary>
                private bool iwon;

                public AmbObserver(IObserver<TSource> downstream, AmbCoordinator parent, bool isLeft)
                {
                    this.downstream = downstream;
                    this.parent = parent;
                    this.isLeft = isLeft;
                }

                internal void OnSubscribe(IDisposable d)
                {
                    Disposable.SetSingle(ref upstream, d);
                }

                public void Dispose()
                {
                    Disposable.TryDispose(ref upstream);
                }

                public void OnCompleted()
                {
                    if (iwon)
                    {
                        downstream.OnCompleted();
                    }
                    else
                    if (parent.TryWin(isLeft))
                    {
                        iwon = true;
                        downstream.OnCompleted();
                    }
                    Dispose();
                }

                public void OnError(Exception error)
                {
                    if (iwon)
                    {
                        downstream.OnError(error);
                    }
                    else
                    if (parent.TryWin(isLeft))
                    {
                        iwon = true;
                        downstream.OnError(error);
                    }
                    Dispose();
                }

                public void OnNext(TSource value)
                {
                    if (iwon)
                    {
                        downstream.OnNext(value);
                    }
                    else
                    if (parent.TryWin(isLeft))
                    {
                        iwon = true;
                        downstream.OnNext(value);
                    }
                }
            }
        }
    }
}
