// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class OnErrorResumeNext<TSource> : Producer<TSource, OnErrorResumeNext<TSource>._>
    {
        private readonly IEnumerable<IObservable<TSource>> _sources;

        public OnErrorResumeNext(IEnumerable<IObservable<TSource>> sources)
        {
            _sources = sources;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(_sources);

        internal sealed class _ : TailRecursiveSink<TSource>
        {
            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            protected override IEnumerable<IObservable<TSource>> Extract(IObservable<TSource> source)
            {
                if (source is OnErrorResumeNext<TSource> oern)
                    return oern._sources;

                return null;
            }

            public override void OnNext(TSource value)
            {
                base._observer.OnNext(value);
            }

            public override void OnError(Exception error)
            {
                _recurse();
            }

            public override void OnCompleted()
            {
                _recurse();
            }

            protected override bool Fail(Exception error)
            {
                //
                // Note that the invocation of _recurse in OnError will
                // cause the next MoveNext operation to be enqueued, so
                // we will still return to the caller immediately.
                //
                OnError(error);
                return true;
            }
        }
    }
}
