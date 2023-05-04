// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

#if NET472 || CSWINRT
using System.Threading;
#endif
#if HAS_DISPATCHER
using System;
using System.Windows.Threading;
#endif

namespace ReactiveTests
{
#if HAS_DISPATCHER
    internal static class DispatcherHelpers
    {
        private static readonly Semaphore s_oneDispatcher = new(1, 1);

        public static IDisposable RunTest(out DispatcherWrapper wrapper)
        {
            s_oneDispatcher.WaitOne();

#if DESKTOPCLR
            var dispatcher = new Thread(Dispatcher.Run);
            dispatcher.IsBackground = true;
            dispatcher.Start();

            while (Dispatcher.FromThread(dispatcher) == null)
                Thread.Sleep(10);

            var d = Dispatcher.FromThread(dispatcher);

            while (d.BeginInvoke(new Action(() => { })).Status == DispatcherOperationStatus.Aborted) ;

            wrapper = new DispatcherWrapper(d);

            return new DispatcherTest(dispatcher);
#else
            wrapper = new DispatcherWrapper(Dispatcher.CurrentDispatcher);

            return Disposable.Empty; // REVIEW: Anything to shut down?
#endif
        }

#if DESKTOPCLR
        private sealed class DispatcherTest : IDisposable
        {
            private readonly Thread _t;

            public DispatcherTest(Thread t)
            {
                _t = t;
            }

            public void Dispose()
            {
                var d = Dispatcher.FromThread(_t);

                d.BeginInvoke(new Action(() => d.InvokeShutdown()));

                _t.Join();

                s_oneDispatcher.Release();
            }
        }
#endif
    }

    internal class DispatcherWrapper
    {
        private readonly Dispatcher _dispatcher;

        public DispatcherWrapper(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public Dispatcher Dispatcher => _dispatcher;

        public static implicit operator Dispatcher(DispatcherWrapper wrapper)
        {
            return wrapper._dispatcher;
        }

        public event DispatcherUnhandledExceptionEventHandler UnhandledException
        {
            add { _dispatcher.UnhandledException += value; }
            remove { _dispatcher.UnhandledException -= value; }
        }

        public void BeginInvoke(Action action)
        {
            _dispatcher.BeginInvoke(action);
        }
    }
#endif
}
