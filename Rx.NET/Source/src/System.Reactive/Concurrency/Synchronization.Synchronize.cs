// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System.Reactive.Concurrency
{
    internal sealed class Synchronize<TSource, TGate> : Producer<TSource, Synchronize<TSource, TGate>._>
        where TGate : notnull, new()
    {
        private readonly IObservable<TSource> _source;
        private readonly TGate? _gate;

        public Synchronize(IObservable<TSource> source, TGate gate)
        {
            _source = source;
            _gate = gate;
        }

        public Synchronize(IObservable<TSource> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new(this, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<TSource>
        {
            private readonly TGate _gate;

            public _(Synchronize<TSource, TGate> parent, IObserver<TSource> observer)
                : base(observer)
            {
                _gate = parent._gate ?? new TGate();
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
}
