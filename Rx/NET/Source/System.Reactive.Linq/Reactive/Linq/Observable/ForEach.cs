// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Threading;

namespace System.Reactive.Linq.Observαble
{
    class ForEach<TSource>
    {
        public class _ : IObserver<TSource>
        {
            private readonly Action<TSource> _onNext;
            private readonly Action _done;

            private Exception _exception;
            private int _stopped;

            public _(Action<TSource> onNext, Action done)
            {
                _onNext = onNext;
                _done = done;

                _stopped = 0;
            }

            public Exception Error
            {
                get { return _exception; }
            }

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

        public class τ : IObserver<TSource>
        {
            private readonly Action<TSource, int> _onNext;
            private readonly Action _done;

            private int _index;
            private Exception _exception;
            private int _stopped;

            public τ(Action<TSource, int> onNext, Action done)
            {
                _onNext = onNext;
                _done = done;
                
                _index = 0;
                _stopped = 0;
            }

            public Exception Error
            {
                get { return _exception; }
            }

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
#endif