// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#if WINDOWS
[assembly:System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Reactive.Concurrency.CoreDispatcherScheduler))]
[assembly:System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Reactive.Linq.CoreDispatcherObservable))]
#else
[assembly:System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Reactive.Concurrency.DispatcherScheduler))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Reactive.Linq.DispatcherObservable))]
#endif

