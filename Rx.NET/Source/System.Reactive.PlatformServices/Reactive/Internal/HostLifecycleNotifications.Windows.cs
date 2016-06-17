// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if WINDOWS
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;

namespace System.Reactive.PlatformServices
{
    internal class HostLifecycleNotifications : IHostLifecycleNotifications
    {
        private EventHandler<SuspendingEventArgs> _suspending;
        private EventHandler<object> _resuming;

        public event EventHandler<HostSuspendingEventArgs> Suspending
        {
            add
            {
                _suspending = (o, e) => value(o, new HostSuspendingEventArgs());
                CoreApplication.Suspending += _suspending;
            }

            remove
            {
                CoreApplication.Suspending -= _suspending;
            }
        }

        public event EventHandler<HostResumingEventArgs> Resuming
        {
            add
            {
                _resuming = (o, e) => value(o, new HostResumingEventArgs());
                CoreApplication.Resuming += _resuming;
            }

            remove
            {
                CoreApplication.Resuming -= _resuming;
            }
        }
    }
}
#endif