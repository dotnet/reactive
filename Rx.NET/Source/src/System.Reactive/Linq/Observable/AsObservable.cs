// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class AsObservable<TSource> : Producer<TSource, AsObservable<TSource>._>, IEvaluatableObservable<TSource>
    {
        private readonly IObservable<TSource> _source;

        public AsObservable(IObservable<TSource> source)
        {
            _source = source;
        }

        public IObservable<TSource> Eval() => _source;

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : IdentitySink<TSource>
        {
            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }
        }
    }
}
