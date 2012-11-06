// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace ReactiveTests
{
    class TestLongRunningScheduler : IScheduler, ISchedulerLongRunning, IServiceProvider
    {
        private Action<ManualResetEvent> _setStart;
        private Action<ManualResetEvent> _setEnd;
        private Action<Exception> _setException;

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

            new Thread(() =>
            {
                eb.Set();
                try
                {
                    action(state, d);
                }
                catch (Exception ex)
                {
                    if (_setException == null)
                        throw;

                    _setException(ex);
                }
                finally
                {
                    ee.Set();
                }
            }).Start();

            return d;
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == typeof(ISchedulerLongRunning))
                return this;

            return null;
        }
    }
}
