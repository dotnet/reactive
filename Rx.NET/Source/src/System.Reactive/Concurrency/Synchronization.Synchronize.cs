// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Concurrency
{
    internal sealed class Synchronize<TSource> : Producer<TSource, Synchronize<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly object _gate;

        public Synchronize(IObservable<TSource> source, object gate)
        {
            _source = source;
            _gate = gate;
        }

        public Synchronize(IObservable<TSource> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(this, observer, cancel);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Justification = "Visibility restricted to friend assemblies. Those should be correct by inspection.")]
        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Synchronize<TSource> _parent;
            private readonly object _gate;

            public _(Synchronize<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _gate = _parent._gate ?? new object();
            }

            public void OnNext(TSource value)
            {
                lock (_gate)
                {
                    _observer.OnNext(value);
                }
            }

            public void OnError(Exception error)
            {
                lock (_gate)
                {
                    _observer.OnError(error);
                    Dispose();
                }
            }

            public void OnCompleted()
            {
                lock (_gate)
                {
                    _observer.OnCompleted();
                    Dispose();
                }
            }
        }
    }
}
