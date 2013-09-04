// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Reactive.Disposables;

namespace System.Reactive
{
    class AutoDetachObserver<T> : ObserverBase<T>
    {
        private readonly IObserver<T> observer;
        private readonly SingleAssignmentDisposable m = new SingleAssignmentDisposable();

        public AutoDetachObserver(IObserver<T> observer)
        {
            this.observer = observer;
        }

        public IDisposable Disposable
        {
            set { m.Disposable = value; }
        }

        protected override void OnNextCore(T value)
        {
            //
            // Safeguarding of the pipeline against rogue observers is required for proper
            // resource cleanup. Consider the following example:
            //
            //   var xs  = Observable.Interval(TimeSpan.FromSeconds(1));
            //   var ys  = <some random sequence>;
            //   var res = xs.CombineLatest(ys, (x, y) => x + y);
            //
            // The marble diagram of the query above looks as follows:
            //
            //   xs  -----0-----1-----2-----3-----4-----5-----6-----7-----8-----9---...
            //                  |     |     |     |     |     |     |     |     |
            //   ys  --------4--+--5--+-----+--2--+--1--+-----+-----+--0--+-----+---...
            //               |  |  |  |     |  |  |  |  |     |     |  |  |     |
            //               v  v  v  v     v  v  v  v  v     v     v  v  v     v
            //   res --------4--5--6--7-----8--5--6--5--6-----7-----8--7--8-----9---...
            //                                 |
            //                                @#&
            //
            // Notice the free-threaded nature of Rx, where messages on the resulting sequence
            // are produced by either of the two input sequences to CombineLatest.
            //
            // Now assume an exception happens in the OnNext callback for the observer of res,
            // at the indicated point marked with @#& above. The callback runs in the context
            // of ys, so the exception will take down the scheduler thread of ys. This by
            // itself is a problem (that can be mitigated by a Catch operator on IScheduler),
            // but notice how the timer that produces xs is kept alive.
            //
            // The safe-guarding code below ensures the acquired resources are disposed when
            // the user callback throws.
            //
            var __noError = false;
            try
            {
                observer.OnNext(value);
                __noError = true;
            }
            finally
            {
                if (!__noError)
                    Dispose();
            }
        }

        protected override void OnErrorCore(Exception exception)
        {
            try
            {
                observer.OnError(exception);
            }
            finally
            {
                Dispose();
            }
        }

        protected override void OnCompletedCore()
        {
            try
            {
                observer.OnCompleted();
            }
            finally
            {
                Dispose();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                m.Dispose();
        }
    }
}