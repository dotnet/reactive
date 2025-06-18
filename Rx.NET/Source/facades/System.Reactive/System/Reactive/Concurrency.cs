// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

extern alias SystemReactiveNet;

using SystemReactiveNet::System.Reactive.Concurrency;

using System.Runtime.CompilerServices;

[assembly:TypeForwardedToAttribute(typeof(AsyncLock))]
[assembly:TypeForwardedToAttribute(typeof(IConcurrencyAbstractionLayer))]
[assembly:TypeForwardedToAttribute(typeof(CurrentThreadScheduler))]
[assembly:TypeForwardedToAttribute(typeof(DefaultScheduler))]
[assembly:TypeForwardedToAttribute(typeof(EventLoopScheduler))]
[assembly:TypeForwardedToAttribute(typeof(HistoricalSchedulerBase))]
[assembly:TypeForwardedToAttribute(typeof(HistoricalScheduler))]
[assembly:TypeForwardedToAttribute(typeof(ImmediateScheduler))]
[assembly:TypeForwardedToAttribute(typeof(IScheduledItem<>))]
[assembly:TypeForwardedToAttribute(typeof(IScheduler))]
[assembly:TypeForwardedToAttribute(typeof(ISchedulerLongRunning))]
[assembly:TypeForwardedToAttribute(typeof(ISchedulerPeriodic))]
[assembly:TypeForwardedToAttribute(typeof(IStopwatch))]
[assembly:TypeForwardedToAttribute(typeof(IStopwatchProvider))]
[assembly:TypeForwardedToAttribute(typeof(LocalScheduler))]
[assembly:TypeForwardedToAttribute(typeof(NewThreadScheduler))]
[assembly:TypeForwardedToAttribute(typeof(ScheduledItem<>))]
[assembly:TypeForwardedToAttribute(typeof(ScheduledItem<,>))]
[assembly:TypeForwardedToAttribute(typeof(Scheduler))]
[assembly:TypeForwardedToAttribute(typeof(SchedulerOperation))]
[assembly:TypeForwardedToAttribute(typeof(SchedulerOperationAwaiter))]
[assembly:TypeForwardedToAttribute(typeof(SchedulerQueue<>))]
[assembly:TypeForwardedToAttribute(typeof(Synchronization))]
[assembly:TypeForwardedToAttribute(typeof(SynchronizationContextScheduler))]
[assembly:TypeForwardedToAttribute(typeof(TaskObservationOptions))]
[assembly:TypeForwardedToAttribute(typeof(TaskPoolScheduler))]

#if !WINDOWS_UWP
// The UWP build of System.Reactive has always included some additional UWP-specific members in its ThreadPoolScheduler,
// so we can't forward the ThreadPoolScheduler type to the one in System.Reactive.Net for UWP builds.
// Instead, we supply a completely separate ThreadPoolScheduler type in this component. The UWP-specific members are
// all marked as obsolete. The intended migration for UWP apps is:
//  1. Replace any use of the UWP-specific ThreadPoolScheduler features with UwpThreadPoolScheduler from the new
//      Uwp-specific component. (If the app was using only standard ThreadPoolScheduler features, this step will
//      not be required.)
//  2. Replace reference to System.Reactive with System.Reactive.Net.
// TODO: does this create a problem for UWP apps that depend indirectly on multiple versions of Rx? They'll have
// both System.Reactive and System.Reactive.Net. Both types will define ThreadPoolScheduler. Pre-compiled components
// will get the one they were build against, but the type name ThreadPoolScheduler will be ambiguous in the app code.
[assembly:TypeForwardedToAttribute(typeof(ThreadPoolScheduler))]
#endif

[assembly:TypeForwardedToAttribute(typeof(VirtualTimeSchedulerBase<,>))]
[assembly:TypeForwardedToAttribute(typeof(VirtualTimeScheduler<,>))]
[assembly:TypeForwardedToAttribute(typeof(VirtualTimeSchedulerExtensions))]

// TODO: currently we've moved these three types into this assembly, with the intention of deprecating them all,
// and preferring the newer types in the platform-specific packages.
// An alternative would be to have the types retain their original names and live in those new packages without
// deprecating them. This would enable us to avoid making working types obsolete which might reduce confusion.
// It does remove the opportunity to fix up some naming issues, but maybe it would be better to live with that
// to minimize disruption.
#if WINDOWS
[assembly:TypeForwardedToAttribute(typeof(System.Reactive.Concurrency.CoreDispatcherScheduler))]
#endif
#if HAS_WINFORMS
[assembly:TypeForwardedToAttribute(typeof(System.Reactive.Concurrency.ControlScheduler))]
#endif
#if HAS_DISPATCHER
[assembly:TypeForwardedToAttribute(typeof(System.Reactive.Concurrency.DispatcherScheduler))]
#endif
