// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class ToObservableRecursive<TSource> : Producer<TSource, ToObservableRecursive<TSource>._>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly IScheduler _scheduler;

        public ToObservableRecursive(IEnumerable<TSource> source, IScheduler scheduler)
        {
            _source = source;
            _scheduler = scheduler;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_source, _scheduler);

        internal sealed class _ : IdentitySink<TSource>
        {
            private IEnumerator<TSource> _enumerator;

            private volatile bool _disposed;

            public _(IObserver<TSource> observer)
                : base(observer)
            {
            }

            public void Run(IEnumerable<TSource> source, IScheduler scheduler)
            {
                try
                {
                    _enumerator = source.GetEnumerator();
                }
                catch (Exception exception)
                {
                    ForwardOnError(exception);

                    return;
                }

                //
                // We never allow the scheduled work to be cancelled. Instead, the _disposed flag
                // is used to have LoopRec bail out and perform proper clean-up of the
                // enumerator.
                //
                scheduler.Schedule(this, (innerScheduler, @this) => @this.LoopRec(innerScheduler));
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                if (disposing)
                {
                    _disposed = true;
                }
            }

            private IDisposable LoopRec(IScheduler scheduler)
            {
                var hasNext = false;
                var ex = default(Exception);
                var current = default(TSource);

                var enumerator = _enumerator;

                if (_disposed)
                {
                    _enumerator.Dispose();
                    _enumerator = null;

                    return Disposable.Empty;
                }

                try
                {
                    hasNext = enumerator.MoveNext();
                    if (hasNext)
                    {
                        current = enumerator.Current;
                    }
                }
                catch (Exception exception)
                {
                    ex = exception;
                }

                if (ex != null)
                {
                    enumerator.Dispose();
                    _enumerator = null;

                    ForwardOnError(ex);
                    return Disposable.Empty;
                }

                if (!hasNext)
                {
                    enumerator.Dispose();
                    _enumerator = null;

                    ForwardOnCompleted();
                    return Disposable.Empty;
                }

                ForwardOnNext(current);

                //
                // We never allow the scheduled work to be cancelled. Instead, the _disposed flag
                // is used to have LoopRec bail out and perform proper clean-up of the
                // enumerator.
                //
                scheduler.Schedule(this, (innerScheduler, @this) => @this.LoopRec(innerScheduler));

                return Disposable.Empty;
            }
        }
    }

    internal sealed class ToObservableLongRunning<TSource> : Producer<TSource, ToObservableLongRunning<TSource>._>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly ISchedulerLongRunning _scheduler;

        public ToObservableLongRunning(IEnumerable<TSource> source, ISchedulerLongRunning scheduler)
        {
            _source = source;
            _scheduler = scheduler;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_source, _scheduler);

        internal sealed class _ : IdentitySink<TSource>
        {
            public _(IObserver<TSource> observer)
                : base(observer)
            {
            }

            public void Run(IEnumerable<TSource> source, ISchedulerLongRunning scheduler)
            {
                var e = default(IEnumerator<TSource>);
                try
                {
                    e = source.GetEnumerator();
                }
                catch (Exception exception)
                {
                    ForwardOnError(exception);

                    return;
                }

                SetUpstream(scheduler.ScheduleLongRunning((@this: this, e), (tuple, cancelable) => tuple.@this.Loop(tuple.e, cancelable)));
            }

            private void Loop(IEnumerator<TSource> enumerator, ICancelable cancel)
            {
                while (!cancel.IsDisposed)
                {
                    var hasNext = false;
                    var ex = default(Exception);
                    var current = default(TSource);

                    try
                    {
                        hasNext = enumerator.MoveNext();
                        if (hasNext)
                        {
                            current = enumerator.Current;
                        }
                    }
                    catch (Exception exception)
                    {
                        ex = exception;
                    }

                    if (ex != null)
                    {
                        ForwardOnError(ex);
                        break;
                    }

                    if (!hasNext)
                    {
                        ForwardOnCompleted();
                        break;
                    }

                    ForwardOnNext(current);
                }

                enumerator.Dispose();
                Dispose();
            }
        }
    }
}
