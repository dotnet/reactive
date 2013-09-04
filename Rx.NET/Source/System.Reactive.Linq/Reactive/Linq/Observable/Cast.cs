// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;

namespace System.Reactive.Linq.Observαble
{
    class Cast<TSource, TResult> : Producer<TResult> /* Could optimize further by deriving from Select<TResult> and providing Ω<TResult2>. We're not doing this (yet) for debuggability. */
    {
        private readonly IObservable<TSource> _source;

        public Cast(IObservable<TSource> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<TResult>, IObserver<TSource>
        {
            public _(IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public void OnNext(TSource value)
            {
                var result = default(TResult);
                try
                {
                    result = (TResult)(object)value;
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return;
                }

                base._observer.OnNext(result);
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
#endif