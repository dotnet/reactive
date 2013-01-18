// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if NO_THREAD && WINDOWS
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
    internal class /*Default*/ConcurrencyAbstractionLayerImpl : IConcurrencyAbstractionLayer
    {
        public IDisposable StartTimer(Action<object> action, object state, TimeSpan dueTime)
        {
            var res = global::Windows.System.Threading.ThreadPoolTimer.CreateTimer(
                tpt =>
                {
                    action(state);
                },
                Normalize(dueTime)
            );

            return Disposable.Create(res.Cancel);
        }

        public IDisposable StartPeriodicTimer(Action action, TimeSpan period)
        {
            //
            // The WinRT thread pool is based on the Win32 thread pool and cannot handle
            // sub-1ms resolution. When passing a lower period, we get single-shot
            // timer behavior instead. See MSDN documentation for CreatePeriodicTimer
            // for more information.
            //
            if (period < TimeSpan.FromMilliseconds(1))
                throw new ArgumentOutOfRangeException("period", Strings_PlatformServices.WINRT_NO_SUB1MS_TIMERS);

            var res = global::Windows.System.Threading.ThreadPoolTimer.CreatePeriodicTimer(
                tpt =>
                {
                    action();
                },
                period
            );

            return Disposable.Create(res.Cancel);
        }

        public IDisposable QueueUserWorkItem(Action<object> action, object state)
        {
            var res = global::Windows.System.Threading.ThreadPool.RunAsync(iaa =>
            {
                action(state);
            });

            return Disposable.Create(res.Cancel);
        }
        
        public void Sleep(TimeSpan timeout)
        {
            var e = new ManualResetEventSlim();

            global::Windows.System.Threading.ThreadPoolTimer.CreateTimer(
                tpt =>
                {
                    e.Set();
                },
                Normalize(timeout)
            );

            e.Wait();
        }

        public IStopwatch StartStopwatch()
        {
#if !NO_STOPWATCH
            return new StopwatchImpl();
#else
            return new DefaultStopwatch();
#endif
        }

        public bool SupportsLongRunning
        {
            get { return false; }
        }

        public void StartThread(Action<object> action, object state)
        {
            throw new NotSupportedException();
        }

        private TimeSpan Normalize(TimeSpan dueTime)
        {
            if (dueTime < TimeSpan.Zero)
                return TimeSpan.Zero;

            return dueTime;
        }
    }
}
#endif