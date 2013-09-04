// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Windows.Threading;

namespace ReactiveTests
{
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
            return new DispatcherWrapper(System.Windows.Deployment.Current.Dispatcher);
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
#if !USE_SL_DISPATCHER
            _dispatcher.InvokeShutdown();
#endif
        }

        public static implicit operator Dispatcher(DispatcherWrapper wrapper)
        {
            return wrapper._dispatcher;
        }

#if !USE_SL_DISPATCHER
        public event DispatcherUnhandledExceptionEventHandler UnhandledException
        {
            add { _dispatcher.UnhandledException += value; }
            remove { _dispatcher.UnhandledException -= value; }
        }
#endif

        public void BeginInvoke(Action action)
        {
            _dispatcher.BeginInvoke(action);
        }
    }
}
