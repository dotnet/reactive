// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
    internal static class ObserveOn<TSource>
    {
        internal sealed class Scheduler : Producer<TSource, ObserveOnObserver<TSource>>
        {
            private readonly IObservable<TSource> _source;
            private readonly IScheduler _scheduler;

            public Scheduler(IObservable<TSource> source, IScheduler scheduler)
            {
                _source = source;
                _scheduler = scheduler;
            }

            protected override ObserveOnObserver<TSource> CreateSink(IObserver<TSource> observer, IDisposable cancel) => new ObserveOnObserver<TSource>(_scheduler, observer, cancel);

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Justification = "Visibility restricted to friend assemblies. Those should be correct by inspection.")]
            protected override IDisposable Run(ObserveOnObserver<TSource> sink) => _source.SubscribeSafe(sink);
        }

        internal sealed class Context : Producer<TSource, Context._>
        {
            private readonly IObservable<TSource> _source;
            private readonly SynchronizationContext _context;

            public Context(IObservable<TSource> source, SynchronizationContext context)
            {
                _source = source;
                _context = context;
            }

            protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(_context, observer, cancel);

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Justification = "Visibility restricted to friend assemblies. Those should be correct by inspection.")]
            protected override IDisposable Run(_ sink) => sink.Run(_source);

            internal sealed class _ : Sink<TSource>, IObserver<TSource>
            {
                private readonly SynchronizationContext _context;

                public _(SynchronizationContext context, IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _context = context;
                }

                public IDisposable Run(IObservable<TSource> source)
                {
                    //
                    // The interactions with OperationStarted/OperationCompleted below allow
                    // for test frameworks to wait until a whole sequence is observed, running
                    // asserts on a per-message level. Also, for ASP.NET pages, the use of the
                    // built-in synchronization context would allow processing to finished in
                    // its entirety before moving on with the page lifecycle.
                    //
                    _context.OperationStarted();

                    var d = source.SubscribeSafe(this);
                    var c = Disposable.Create(_context.OperationCompleted);

                    return StableCompositeDisposable.Create(d, c);
                }

                public void OnNext(TSource value)
                {
                    _context.Post(OnNextPosted, value);
                }

                public void OnError(Exception error)
                {
                    _context.Post(OnErrorPosted, error);
                }

                public void OnCompleted()
                {
                    _context.Post(OnCompletedPosted, state: null);
                }

                private void OnNextPosted(object value)
                {
                    _observer.OnNext((TSource)value);
                }

                private void OnErrorPosted(object error)
                {
                    _observer.OnError((Exception)error);
                    Dispose();
                }

                private void OnCompletedPosted(object ignored)
                {
                    _observer.OnCompleted();
                    Dispose();
                }
            }
        }
    }
}
