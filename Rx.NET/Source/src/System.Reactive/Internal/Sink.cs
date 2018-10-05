// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive
{
    internal interface ISink<in TTarget>
    {
        void ForwardOnNext(TTarget value);
        void ForwardOnCompleted();
        void ForwardOnError(Exception error);
    }

    internal abstract class Sink<TTarget> : ISink<TTarget>, IDisposable
    {
        private IDisposable _upstream;
        private volatile IObserver<TTarget> _observer;

        protected Sink(IObserver<TTarget> observer)
        {
            _observer = observer;
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _observer, NopObserver<TTarget>.Instance) != NopObserver<TTarget>.Instance)
                Dispose(true);
        }

        /// <summary>
        /// Override this method to dispose additional resources.
        /// The method is guaranteed to be called at most once.
        /// </summary>
        /// <param name="disposing">If true, the method was called from <see cref="Dispose()"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            //Calling base.Dispose(true) is not a proper disposal, so we can omit the assignment here.
            //Sink is internal so this can pretty much be enforced.
            //_observer = NopObserver<TTarget>.Instance;

            Disposable.TryDispose(ref _upstream);
        }

        public void ForwardOnNext(TTarget value)
        {
            _observer.OnNext(value);
        }

        public void ForwardOnCompleted()
        {
            _observer.OnCompleted();
            Dispose();
        }

        public void ForwardOnError(Exception error)
        {
            _observer.OnError(error);
            Dispose();
        }

        protected void SetUpstream(IDisposable upstream)
        {
            Disposable.SetSingle(ref _upstream, upstream);
        }

        protected void DisposeUpstream()
        {
            Disposable.TryDispose(ref _upstream);
        }
    }

    /// <summary>
    /// Base class for implementation of query operators, providing a lightweight sink that can be disposed to mute the outgoing observer.
    /// </summary>
    /// <typeparam name="TTarget">Type of the resulting sequence's elements.</typeparam>
    /// <typeparam name="TSource"></typeparam>
    /// <remarks>Implementations of sinks are responsible to enforce the message grammar on the associated observer. Upon sending a terminal message, a pairing Dispose call should be made to trigger cancellation of related resources and to mute the outgoing observer.</remarks>
    internal abstract class Sink<TSource, TTarget> : Sink<TTarget>, IObserver<TSource>
    {
        protected Sink(IObserver<TTarget> observer) : base(observer)
        {
        }

        public virtual void Run(IObservable<TSource> source)
        {
            SetUpstream(source.SubscribeSafe(this));
        }

        public abstract void OnNext(TSource value);

        public virtual void OnError(Exception error)
        {
            ForwardOnError(error);
        }

        public virtual void OnCompleted()
        {
            ForwardOnCompleted();
        }

        public IObserver<TTarget> GetForwarder() => new _(this);

        private sealed class _ : IObserver<TTarget>
        {
            private readonly Sink<TSource, TTarget> _forward;

            public _(Sink<TSource, TTarget> forward)
            {
                _forward = forward;
            }

            public void OnNext(TTarget value)
            {
                _forward.ForwardOnNext(value);
            }

            public void OnError(Exception error)
            {
                _forward.ForwardOnError(error);
            }

            public void OnCompleted()
            {
                _forward.ForwardOnCompleted();
            }
        }
    }
}
