// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class IsEmpty<TSource> : Producer<bool, IsEmpty<TSource>._>
    {
        private readonly IObservable<TSource> _source;

        public IsEmpty(IObservable<TSource> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<bool> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource, bool>
        {
            public _(IObserver<bool> observer)
                : base(observer)
            {
            }

            public override void OnNext(TSource value)
            {
                ForwardOnNext(false);
                ForwardOnCompleted();
            }

            public override void OnCompleted()
            {
                ForwardOnNext(true);
                ForwardOnCompleted();
            }
        }
    }
}
