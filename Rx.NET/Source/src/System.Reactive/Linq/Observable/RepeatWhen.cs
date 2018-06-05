// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class RepeatWhen<T, U> : IObservable<T>
    {
        readonly IObservable<T> source;

        readonly Func<IObservable<object>, IObservable<U>> handler;

        internal RepeatWhen(IObservable<T> source, Func<IObservable<object>, IObservable<U>> handler)
        {
            this.source = source;
            this.handler = handler;
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
                redo = handler(completeSignals);
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

            var parent = new MainObserver(observer, source, new RedoSerializedObserver<object>(completeSignals));

            var d = redo.SubscribeSafe(parent.handlerObserver);
            Disposable.SetSingle(ref parent.handlerUpstream, d);

            parent.HandlerNext();

            return parent;
        }

        sealed class MainObserver : Sink<T>, IObserver<T>
        {
            readonly IObserver<Exception> errorSignal;

            internal readonly HandlerObserver handlerObserver;

            readonly IObservable<T> source;

            IDisposable upstream;

            internal IDisposable handlerUpstream;

            int trampoline;

            int halfSerializer;

            Exception error;

            internal MainObserver(IObserver<T> downstream, IObservable<T> source, IObserver<Exception> errorSignal) : base(downstream, null)
            {
                this.source = source;
                this.errorSignal = errorSignal;
                this.handlerObserver = new HandlerObserver(this);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Disposable.TryDispose(ref upstream);
                    Disposable.TryDispose(ref handlerUpstream);
                }
                base.Dispose(disposing);
            }

            public void OnCompleted()
            {
                if (Disposable.TrySetSerial(ref upstream, null))
                {
                    errorSignal.OnNext(null);
                }

            }

            public void OnError(Exception error)
            {
                HalfSerializer.ForwardOnError(this, error, ref halfSerializer, ref this.error);
            }

            public void OnNext(T value)
            {
                HalfSerializer.ForwardOnNext(this, value, ref halfSerializer, ref this.error);
            }

            internal void HandlerError(Exception error)
            {
                HalfSerializer.ForwardOnError(this, error, ref halfSerializer, ref this.error);
            }

            internal void HandlerComplete()
            {
                HalfSerializer.ForwardOnCompleted(this, ref halfSerializer, ref this.error);
            }

            internal void HandlerNext()
            {
                if (Interlocked.Increment(ref trampoline) == 1)
                {
                    do
                    {
                        var sad = new SingleAssignmentDisposable();
                        if (Interlocked.CompareExchange(ref upstream, sad, null) != null)
                        {
                            return;
                        }

                        sad.Disposable = source.SubscribeSafe(this);
                    }
                    while (Interlocked.Decrement(ref trampoline) != 0);
                }
            }

            internal sealed class HandlerObserver : IObserver<U>
            {
                readonly MainObserver main;

                internal HandlerObserver(MainObserver main)
                {
                    this.main = main;
                }

                public void OnCompleted()
                {
                    main.HandlerComplete();
                }

                public void OnError(Exception error)
                {
                    main.HandlerError(error);
                }

                public void OnNext(U value)
                {
                    main.HandlerNext();
                }
            }
        }

    }
}
