// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Justification = "Visibility restricted to friend assemblies. Those should be correct by inspection.")]
        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
#if !NO_SYNCCTX
            if (_context != null)
            {
                var sink = new ς(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
            else
#endif
            {
                var sink = new ObserveOnObserver<TSource>(_scheduler, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
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

            public IDisposable Run()
            {
                //
                // The interactions with OperationStarted/OperationCompleted below allow
                // for test frameworks to wait until a whole sequence is observed, running
                // asserts on a per-message level. Also, for ASP.NET pages, the use of the
                // built-in synchronization context would allow processing to finished in
                // its entirety before moving on with the page lifecycle.
                //
                _parent._context.OperationStarted();

                var d = _parent._source.SubscribeSafe(this);
                var c = Disposable.Create(() =>
                {
                    _parent._context.OperationCompleted();
                });

                return new CompositeDisposable(d, c);
            }

            public void OnNext(TSource value)
            {
                _parent._context.Post(OnNextPosted, value);
            }

            public void OnError(Exception error)
            {
                _parent._context.Post(OnErrorPosted, error);
            }

            public void OnCompleted()
            {
                _parent._context.Post(OnCompletedPosted, null);
            }

            private void OnNextPosted(object value)
            {
                base._observer.OnNext((TSource)value);
            }

            private void OnErrorPosted(object error)
            {
                base._observer.OnError((Exception)error);
                base.Dispose();
            }

            private void OnCompletedPosted(object ignored)
            {
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
#endif
    }
}
#endif