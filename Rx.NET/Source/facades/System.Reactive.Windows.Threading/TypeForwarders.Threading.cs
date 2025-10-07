// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#if WINDOWS
extern alias SystemReactiveForWindowsRuntime;
[assembly:System.Runtime.CompilerServices.TypeForwardedTo(typeof(SystemReactiveForWindowsRuntime::System.Reactive.Concurrency.CoreDispatcherScheduler))]
[assembly:System.Runtime.CompilerServices.TypeForwardedTo(typeof(SystemReactiveForWindowsRuntime::System.Reactive.Linq.CoreDispatcherObservable))]
#else
extern alias SystemReactiveForWpf;
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(SystemReactiveForWpf::System.Reactive.Concurrency.DispatcherScheduler))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(SystemReactiveForWpf::System.Reactive.Linq.DispatcherObservable))]
#endif
