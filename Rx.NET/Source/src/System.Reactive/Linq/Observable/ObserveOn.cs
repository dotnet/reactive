// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class ObserveOn<TSource>
    {
        internal sealed class Scheduler : Producer<TSource>
        {
            private readonly IObservable<TSource> _source;
            private readonly IScheduler _scheduler;

            public Scheduler(IObservable<TSource> source, IScheduler scheduler)
            {
                _source = source;
                _scheduler = scheduler;
            }

            protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new ObserveOnObserver<TSource>(_scheduler, observer, cancel);
                setSink(sink);
                return _source.Subscribe(sink);
            }
        }

        internal sealed class Context : Producer<TSource>
        {
            private readonly IObservable<TSource> _source;
            private readonly SynchronizationContext _context;

            public Context(IObservable<TSource> source, SynchronizationContext context)
            {
                _source = source;
                _context = context;
            }

            protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(_context, observer, cancel);
                setSink(sink);
                return _source.Subscribe(sink);
            }

            private sealed class _ : Sink<TSource>, IObserver<TSource>
            {
                private readonly SynchronizationContext _context;

                public _(SynchronizationContext context, IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _context = context;
                }

                public void OnNext(TSource value)
                {
                    _context.PostWithStartComplete(() =>
                    {
                        base._observer.OnNext(value);
                    });
                }

                public void OnError(Exception error)
                {
                    _context.PostWithStartComplete(() =>
                    {
                        base._observer.OnError(error);
                        base.Dispose();
                    });
                }

                public void OnCompleted()
                {
                    _context.PostWithStartComplete(() =>
                    {
                        base._observer.OnCompleted();
                        base.Dispose();
                    });
                }
            }
        }
    }
}
