// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

extern alias SystemReactiveNet;

using SystemReactiveNet::System.Reactive.PlatformServices;

using System.Runtime.CompilerServices;

[assembly:TypeForwardedToAttribute(typeof(EnlightenmentProvider))]
[assembly:TypeForwardedToAttribute(typeof(CurrentPlatformEnlightenmentProvider))]
[assembly:TypeForwardedToAttribute(typeof(IExceptionServices))]
[assembly:TypeForwardedToAttribute(typeof(HostLifecycleService))]
[assembly:TypeForwardedToAttribute(typeof(IHostLifecycleNotifications))]
[assembly:TypeForwardedToAttribute(typeof(HostSuspendingEventArgs))]
[assembly:TypeForwardedToAttribute(typeof(HostResumingEventArgs))]
[assembly:TypeForwardedToAttribute(typeof(IPlatformEnlightenmentProvider))]
[assembly:TypeForwardedToAttribute(typeof(PlatformEnlightenmentProvider))]
[assembly:TypeForwardedToAttribute(typeof(SystemClock))]
[assembly:TypeForwardedToAttribute(typeof(ISystemClock))]
[assembly:TypeForwardedToAttribute(typeof(INotifySystemClockChanged))]
[assembly:TypeForwardedToAttribute(typeof(SystemClockChangedEventArgs))]
[assembly:TypeForwardedToAttribute(typeof(DefaultSystemClock))]
[assembly:TypeForwardedToAttribute(typeof(PeriodicTimerSystemClockMonitor))]
