// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveTests
{
    internal class TestLongRunningScheduler : IScheduler, ISchedulerLongRunning, IServiceProvider
    {
        private readonly Action<ManualResetEvent> _setStart;
        private readonly Action<ManualResetEvent> _setEnd;
        private readonly Action<Exception> _setException;

        public TestLongRunningScheduler(Action<ManualResetEvent> setStart, Action<ManualResetEvent> setEnd)
            : this(setStart, setEnd, null)
        {
        }

        public TestLongRunningScheduler(Action<ManualResetEvent> setStart, Action<ManualResetEvent> setEnd, Action<Exception> setException)
        {
            _setStart = setStart;
            _setEnd = setEnd;
            _setException = setException;
        }

        public DateTimeOffset Now
        {
            get { return DateTimeOffset.Now; }
        }

        public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            throw new NotImplementedException();
        }

        public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            throw new NotImplementedException();
        }

        public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            throw new NotImplementedException();
        }

        public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
        {
            var d = new BooleanDisposable();

            var eb = new ManualResetEvent(false);
            _setStart(eb);

            var ee = new ManualResetEvent(false);
            _setEnd(ee);

            Task.Run(() =>
            {
                eb.Set();
                try
                {
                    action(state, d);
                }
                catch (Exception ex)
                {
                    if (_setException == null)
                    {
                        throw;
                    }

                    _setException(ex);
                }
                finally
                {
                    ee.Set();
                }
            });

            return d;
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == typeof(ISchedulerLongRunning))
            {
                return this;
            }

            return null;
        }
    }
}
