// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

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
            private readonly AmbObserver _leftObserver;
            private readonly AmbObserver _rightObserver;
            private int _winner;

            public AmbCoordinator(IObserver<TSource> observer)
            {
                _leftObserver = new AmbObserver(observer, this, true);
                _rightObserver = new AmbObserver(observer, this, false);
            }

            public void Run(IObservable<TSource> left, IObservable<TSource> right)
            {
                _leftObserver.Run(left);
                _rightObserver.Run(right);
            }

            public void Dispose()
            {
                _leftObserver.Dispose();
                _rightObserver.Dispose();
            }

            /// <summary>
            /// Try winning the race for the right of emission.
            /// </summary>
            /// <param name="isLeft">If true, the contender is the left source.</param>
            /// <returns>True if the contender has won the race.</returns>
            public bool TryWin(bool isLeft)
            {
                var index = isLeft ? 1 : 2;

                if (Volatile.Read(ref _winner) == index)
                {
                    return true;
                }
                if (Interlocked.CompareExchange(ref _winner, index, 0) == 0)
                {
                    (isLeft ? _rightObserver : _leftObserver).Dispose();
                    return true;
                }
                return false;
            }

            private sealed class AmbObserver : IdentitySink<TSource>
            {
                private readonly AmbCoordinator _parent;
                private readonly bool _isLeft;

                /// <summary>
                /// If true, this observer won the race and now can emit
                /// on a fast path.
                /// </summary>
                private bool _iwon;

                public AmbObserver(IObserver<TSource> downstream, AmbCoordinator parent, bool isLeft) : base(downstream)
                {
                    _parent = parent;
                    _isLeft = isLeft;
                }

                public override void OnCompleted()
                {
                    if (_iwon)
                    {
                        ForwardOnCompleted();
                    }
                    else if (_parent.TryWin(_isLeft))
                    {
                        _iwon = true;
                        ForwardOnCompleted();
                    }
                    else
                    {
                        Dispose();
                    }
                }

                public override void OnError(Exception error)
                {
                    if (_iwon)
                    {
                        ForwardOnError(error);
                    }
                    else if (_parent.TryWin(_isLeft))
                    {
                        _iwon = true;
                        ForwardOnError(error);
                    }
                    else
                    {
                        Dispose();
                    }
                }

                public override void OnNext(TSource value)
                {
                    if (_iwon)
                    {
                        ForwardOnNext(value);
                    }
                    else
                    if (_parent.TryWin(_isLeft))
                    {
                        _iwon = true;
                        ForwardOnNext(value);
                    }
                }
            }
        }
    }
}
