// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class OnErrorResumeNext<TSource> : Producer<TSource>
    {
        private readonly IEnumerable<IObservable<TSource>> _sources;

        public OnErrorResumeNext(IEnumerable<IObservable<TSource>> sources)
        {
            _sources = sources;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return sink.Run(_sources);
        }

        class _ : TailRecursiveSink<TSource>
        {
            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            protected override IEnumerable<IObservable<TSource>> Extract(IObservable<TSource> source)
            {
                var oern = source as OnErrorResumeNext<TSource>;
                if (oern != null)
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
        }
    }
}
#endif