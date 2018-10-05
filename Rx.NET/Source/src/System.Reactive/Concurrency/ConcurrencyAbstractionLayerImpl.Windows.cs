// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if NO_THREAD && WINDOWS
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

            return res.AsDisposable();
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
                throw new ArgumentOutOfRangeException(nameof(period), Strings_PlatformServices.WINRT_NO_SUB1MS_TIMERS);

            var res = global::Windows.System.Threading.ThreadPoolTimer.CreatePeriodicTimer(
                tpt =>
                {
                    action();
                },
                period
            );

            return res.AsDisposable();
        }

        public IDisposable QueueUserWorkItem(Action<object> action, object state)
        {
            var res = global::Windows.System.Threading.ThreadPool.RunAsync(iaa =>
            {
                action(state);
            });

            return res.AsDisposable();
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

        public IStopwatch StartStopwatch() => new StopwatchImpl();

        public bool SupportsLongRunning => false;

        public void StartThread(Action<object> action, object state)
        {
            throw new NotSupportedException();
        }

        private TimeSpan Normalize(TimeSpan dueTime) => dueTime < TimeSpan.Zero ? TimeSpan.Zero : dueTime;
    }
}
#endif
