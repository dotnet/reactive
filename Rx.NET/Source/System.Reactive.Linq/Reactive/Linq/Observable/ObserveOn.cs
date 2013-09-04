// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;
using System.Threading;

namespace System.Reactive.Linq.Observαble
{
    class ObserveOn<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly IScheduler _scheduler;

        public ObserveOn(IObservable<TSource> source, IScheduler scheduler)
        {
            _source = source;
            _scheduler = scheduler;
        }

#if !NO_SYNCCTX
        private readonly SynchronizationContext _context;

        public ObserveOn(IObservable<TSource> source, SynchronizationContext context)
        {
            _source = source;
            _context = context;
        }
#endif

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
#if !NO_SYNCCTX
            if (_context != null)
            {
                var sink = new ς(this, observer, cancel);
                setSink(sink);
                return _source.Subscribe(sink);
            }
            else
#endif
            {
                var sink = new ObserveOnObserver<TSource>(_scheduler, observer, cancel);
                setSink(sink);
                return _source.Subscribe(sink);
            }
        }

#if !NO_SYNCCTX
        class ς : Sink<TSource>, IObserver<TSource>
        {
            private readonly ObserveOn<TSource> _parent;

            public ς(ObserveOn<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public void OnNext(TSource value)
            {
                _parent._context.PostWithStartComplete(() =>
                {
                    base._observer.OnNext(value);
                });
            }

            public void OnError(Exception error)
            {
                _parent._context.PostWithStartComplete(() =>
                {
                    base._observer.OnError(error);
                    base.Dispose();
                });
            }

            public void OnCompleted()
            {
                _parent._context.PostWithStartComplete(() =>
                {
                    base._observer.OnCompleted();
                    base.Dispose();
                });
            }
        }
#endif
    }
}
#endif