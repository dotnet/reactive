// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Concat<TSource> : Producer<TSource, Concat<TSource>._>, IConcatenatable<TSource>
    {
        private readonly IEnumerable<IObservable<TSource>> _sources;

        public Concat(IEnumerable<IObservable<TSource>> sources)
        {
            _sources = sources;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(_sources);

        public IEnumerable<IObservable<TSource>> GetSources() => _sources;

        internal sealed class _ : ConcatSink<TSource>
        {
            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }
        }
    }
}
