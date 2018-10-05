// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class RepeatWhen<T, U> : IObservable<T>
    {
        private readonly IObservable<T> _source;
        private readonly Func<IObservable<object>, IObservable<U>> _handler;

        internal RepeatWhen(IObservable<T> source, Func<IObservable<object>, IObservable<U>> handler)
        {
            _source = source;
            _handler = handler;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            var completeSignals = new Subject<object>();
            var redo = default(IObservable<U>);

            try
            {
                redo = _handler(completeSignals);
                if (redo == null)
                {
                    throw new NullReferenceException("The handler returned a null IObservable");
                }
            }
            catch (Exception ex)
            {
                observer.OnError(ex);
                return Disposable.Empty;
            }

            var parent = new MainObserver(observer, _source, new RedoSerializedObserver<object>(completeSignals));

            var d = redo.SubscribeSafe(parent.HandlerConsumer);
            Disposable.SetSingle(ref parent.HandlerUpstream, d);

            parent.HandlerNext();

            return parent;
        }

        private sealed class MainObserver : Sink<T>, IObserver<T>
        {
            private readonly IObserver<Exception> _errorSignal;

            internal readonly HandlerObserver HandlerConsumer;
            private readonly IObservable<T> _source;
            private IDisposable _upstream;

            internal IDisposable HandlerUpstream;
            private int _trampoline;
            private int _halfSerializer;
            private Exception _error;

            internal MainObserver(IObserver<T> downstream, IObservable<T> source, IObserver<Exception> errorSignal) : base(downstream)
            {
                _source = source;
                _errorSignal = errorSignal;
                HandlerConsumer = new HandlerObserver(this);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Disposable.TryDispose(ref _upstream);
                    Disposable.TryDispose(ref HandlerUpstream);
                }
                base.Dispose(disposing);
            }

            public void OnCompleted()
            {
                if (Disposable.TrySetSerial(ref _upstream, null))
                {
                    _errorSignal.OnNext(null);
                }

            }

            public void OnError(Exception error)
            {
                HalfSerializer.ForwardOnError(this, error, ref _halfSerializer, ref _error);
            }

            public void OnNext(T value)
            {
                HalfSerializer.ForwardOnNext(this, value, ref _halfSerializer, ref _error);
            }

            private void HandlerError(Exception error)
            {
                HalfSerializer.ForwardOnError(this, error, ref _halfSerializer, ref _error);
            }

            private void HandlerComplete()
            {
                HalfSerializer.ForwardOnCompleted(this, ref _halfSerializer, ref _error);
            }

            internal void HandlerNext()
            {
                if (Interlocked.Increment(ref _trampoline) == 1)
                {
                    do
                    {
                        var sad = new SingleAssignmentDisposable();
                        if (Interlocked.CompareExchange(ref _upstream, sad, null) != null)
                        {
                            return;
                        }

                        sad.Disposable = _source.SubscribeSafe(this);
                    }
                    while (Interlocked.Decrement(ref _trampoline) != 0);
                }
            }

            internal sealed class HandlerObserver : IObserver<U>
            {
                private readonly MainObserver _main;

                internal HandlerObserver(MainObserver main)
                {
                    _main = main;
                }

                public void OnCompleted()
                {
                    _main.HandlerComplete();
                }

                public void OnError(Exception error)
                {
                    _main.HandlerError(error);
                }

                public void OnNext(U value)
                {
                    _main.HandlerNext();
                }
            }
        }

    }
}
