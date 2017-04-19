// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Concat<TSource> : Producer<TSource>, IConcatenatable<TSource>
    {
        private readonly IEnumerable<IObservable<TSource>> _sources;

        public Concat(IEnumerable<IObservable<TSource>> sources)
        {
            _sources = sources;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return sink.Run(_sources);
        }

        public IEnumerable<IObservable<TSource>> GetSources() => _sources;

        private sealed class _ : ConcatSink<TSource>
        {
            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public override void OnNext(TSource value)
            {
                base._observer.OnNext(value);
            }

            public override void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }
        }
    }
}
