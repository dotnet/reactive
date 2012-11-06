// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
    class For<TSource, TResult> : Producer<TResult>, IConcatenatable<TResult>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly Func<TSource, IObservable<TResult>> _resultSelector;

        public For(IEnumerable<TSource> source, Func<TSource, IObservable<TResult>> resultSelector)
        {
            _source = source;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return sink.Run(GetSources());
        }

        public IEnumerable<IObservable<TResult>> GetSources()
        {
            foreach (var item in _source)
                yield return _resultSelector(item);
        }

        class _ : ConcatSink<TResult>
        {
            public _(IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public override void OnNext(TResult value)
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
#endif
