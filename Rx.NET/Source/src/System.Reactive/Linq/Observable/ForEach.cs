// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class ForEach<TSource>
    {
        public sealed class Observer : IObserver<TSource>
        {
            private readonly Action<TSource> _onNext;
            private readonly Action _done;

            private Exception _exception;
            private int _stopped;

            public Observer(Action<TSource> onNext, Action done)
            {
                _onNext = onNext;
                _done = done;

                _stopped = 0;
            }

            public Exception Error => _exception;

            public void OnNext(TSource value)
            {
                if (_stopped == 0)
                {
                    try
                    {
                        _onNext(value);
                    }
                    catch (Exception ex)
                    {
                        OnError(ex);
                    }
                }
            }

            public void OnError(Exception error)
            {
                if (Interlocked.Exchange(ref _stopped, 1) == 0)
                {
                    _exception = error;
                    _done();
                }
            }

            public void OnCompleted()
            {
                if (Interlocked.Exchange(ref _stopped, 1) == 0)
                {
                    _done();
                }
            }
        }

        public sealed class ObserverIndexed : IObserver<TSource>
        {
            private readonly Action<TSource, int> _onNext;
            private readonly Action _done;

            private int _index;
            private Exception _exception;
            private int _stopped;

            public ObserverIndexed(Action<TSource, int> onNext, Action done)
            {
                _onNext = onNext;
                _done = done;

                _index = 0;
                _stopped = 0;
            }

            public Exception Error => _exception;

            public void OnNext(TSource value)
            {
                if (_stopped == 0)
                {
                    try
                    {
                        _onNext(value, checked(_index++));
                    }
                    catch (Exception ex)
                    {
                        OnError(ex);
                    }
                }
            }

            public void OnError(Exception error)
            {
                if (Interlocked.Exchange(ref _stopped, 1) == 0)
                {
                    _exception = error;
                    _done();
                }
            }

            public void OnCompleted()
            {
                if (Interlocked.Exchange(ref _stopped, 1) == 0)
                {
                    _done();
                }
            }
        }
    }
}
