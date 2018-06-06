#if WINDOWS
[assembly:System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Reactive.Concurrency.CoreDispatcherScheduler))]
#else
[assembly:System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Reactive.Concurrency.DispatcherScheduler))]
#endif
[assembly:System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Reactive.Linq.DispatcherObservable))]
