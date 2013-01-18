// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if WINDOWSPHONE7

#if DEBUG_NO_AGENT_SUPPORT
using Microsoft.Phone.Shell;
#else
using System.Reactive.PlatformServices.Phone.Shell;
#endif

namespace System.Reactive.PlatformServices
{
    internal class HostLifecycleNotifications : IHostLifecycleNotifications
    {
        private EventHandler<ActivatedEventArgs> _activated;
        private EventHandler<DeactivatedEventArgs> _deactivated;

        public event EventHandler<HostSuspendingEventArgs> Suspending
        {
            add
            {
                _deactivated = (o, e) => value(o, new HostSuspendingEventArgs());

                var current = PhoneApplicationService.Current;
                if (current != null)
                    current.Deactivated += _deactivated;
            }

            remove
            {
                var current = PhoneApplicationService.Current;
                if (current != null)
                    current.Deactivated -= _deactivated;
            }
        }

        public event EventHandler<HostResumingEventArgs> Resuming
        {
            add
            {
                _activated = (o, e) =>
                {
                    if (e.IsApplicationInstancePreserved)
                    {
                        value(o, new HostResumingEventArgs());
                    }
                };

                var current = PhoneApplicationService.Current;
                if (current != null)
                    current.Activated += _activated;
            }

            remove
            {
                var current = PhoneApplicationService.Current;
                if (current != null)
                    current.Activated -= _activated;
            }
        }
    }
}
#endif