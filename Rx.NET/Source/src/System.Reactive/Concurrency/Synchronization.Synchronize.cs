// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Threading;

namespace System.Reactive.Concurrency
{
    internal sealed class SynchronizeWithObject<TSource> : Producer<TSource, SynchronizeWithObject<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly object? _gate;

        public SynchronizeWithObject(IObservable<TSource> source, object gate)
        {
            _source = source;
            _gate = gate;
        }

        public SynchronizeWithObject(IObservable<TSource> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new(this, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<TSource>
        {
            private readonly object _gate;

            public _(SynchronizeWithObject<TSource> parent, IObserver<TSource> observer)
                : base(observer)
            {
                _gate = parent._gate ?? new object();
            }

            public override void OnNext(TSource value)
            {
                lock (_gate)
                {
                    ForwardOnNext(value);
                }
            }

            public override void OnError(Exception error)
            {
                lock (_gate)
                {
                    ForwardOnError(error);
                }
            }

            public override void OnCompleted()
            {
                lock (_gate)
                {
                    ForwardOnCompleted();
                }
            }
        }
    }

    #if HAS_SYSTEM_THREADING_LOCK
    internal sealed class SynchronizeWithLock<TSource> : Producer<TSource, SynchronizeWithLock<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly Lock _gate;

        public SynchronizeWithLock(IObservable<TSource> source, Lock gate)
        {
            _source = source;
            _gate = gate;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new(this, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<TSource>
        {
            private readonly Lock _gate;

            public _(SynchronizeWithLock<TSource> parent, IObserver<TSource> observer)
                : base(observer)
            {
                _gate = parent._gate;
            }

            public override void OnNext(TSource value)
            {
                lock (_gate)
                {
                    ForwardOnNext(value);
                }
            }

            public override void OnError(Exception error)
            {
                lock (_gate)
                {
                    ForwardOnError(error);
                }
            }

            public override void OnCompleted()
            {
                lock (_gate)
                {
                    ForwardOnCompleted();
                }
            }
        }
    }
    #endif
}
