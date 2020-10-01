// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

#if NETCOREAPP2_1 || NET472 || NETCOREAPP3_1 || CSWINRT
using System.Threading;
#endif
#if HAS_DISPATCHER
using System;
using System.Windows.Threading;
#endif

namespace ReactiveTests
{
#if HAS_DISPATCHER
    static class DispatcherHelpers
    {
        public static DispatcherWrapper EnsureDispatcher()
        {
#if DESKTOPCLR
            var dispatcher = new Thread(Dispatcher.Run);
            dispatcher.IsBackground = true;
            dispatcher.Start();

            while (Dispatcher.FromThread(dispatcher) == null)
                Thread.Sleep(10);

            var d = Dispatcher.FromThread(dispatcher);

            while (d.BeginInvoke(new Action(() => { })).Status == DispatcherOperationStatus.Aborted) ;

            return new DispatcherWrapper(d);
#else
            return new DispatcherWrapper(Dispatcher.CurrentDispatcher);
#endif
        }
    }

    class DispatcherWrapper
    {
        private Dispatcher _dispatcher;

        public DispatcherWrapper(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public Dispatcher Dispatcher { get { return _dispatcher; } }

        public void InvokeShutdown()
        {
            _dispatcher.InvokeShutdown();
        }

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
