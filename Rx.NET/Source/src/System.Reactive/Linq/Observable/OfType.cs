// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class OfType<TSource, TResult> : Producer<TResult, OfType<TSource, TResult>._>
    {
        private readonly IObservable<TSource> _source;

        public OfType(IObservable<TSource> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<TSource, TResult> 
        {
            public _(IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public override void OnNext(TSource value)
            {
                if (value is TResult)
                {
                    ForwardOnNext((TResult)(object)value);
                }
            }
        }
    }
}
