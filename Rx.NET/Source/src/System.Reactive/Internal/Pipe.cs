// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive
{
    /// <summary>
    /// Base class for implementation of query operators.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    internal abstract class Pipe<TSource, TResult> : Producer<TResult, Pipe<TSource, TResult>>, IObserver<TSource>, ISink<TResult>, IDisposable
    {
        protected readonly IObservable<TSource> _source;

        private IDisposable _upstream;
        private volatile IObserver<TResult> _downstream;

        public Pipe(IObservable<TSource> source)
        {
            _source = source;
        }

        protected override Pipe<TSource, TResult> CreateSink(IObserver<TResult> observer)
        {
            if (Interlocked.CompareExchange(ref _downstream, observer, null) == null)
                return this;
            else
            {
                var pipe = Clone();
                pipe._downstream = observer;
                return pipe;
            }
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _downstream, NopObserver<TResult>.Instance) != NopObserver<TResult>.Instance)
                Dispose(true);
        }

        protected abstract Pipe<TSource, TResult> Clone();

        protected override void Run(Pipe<TSource, TResult> sink)
        {
            sink.SetUpstream(_source.SubscribeSafe(sink));
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

        public void ForwardOnNext(TResult value)
        {
            _downstream.OnNext(value);
        }

        public void ForwardOnCompleted()
        {
            _downstream.OnCompleted();
            Dispose();
        }

        public void ForwardOnError(Exception error)
        {
            _downstream.OnError(error);
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


        public abstract void OnNext(TSource value);

        public virtual void OnError(Exception error)
        {
            ForwardOnError(error);
        }

        public virtual void OnCompleted()
        {
            ForwardOnCompleted();
        }

        public IObserver<TResult> GetForwarder() => new _(this);

        private sealed class _ : IObserver<TResult>
        {
            private readonly ISink<TResult> _forward;

            public _(ISink<TResult> forward)
            {
                _forward = forward;
            }

            public void OnNext(TResult value)
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

    internal abstract class Pipe<TSource> : Pipe<TSource, TSource>
    {
        internal Pipe(IObservable<TSource> source) : base(source)
        {
        }

        public override void OnNext(TSource value)
        {
            ForwardOnNext(value);
        }
    }
}
