// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class While<TSource> : Producer<TSource, While<TSource>._>, IConcatenatable<TSource>
    {
        private readonly Func<bool> _condition;
        private readonly IObservable<TSource> _source;

        public While(Func<bool> condition, IObservable<TSource> source)
        {
            _condition = condition;
            _source = source;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(GetSources());

        public IEnumerable<IObservable<TSource>> GetSources()
        {
            while (_condition())
                yield return _source;
        }

        internal sealed class _ : ConcatSink<TSource>
        {
            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }
        }
    }
}
