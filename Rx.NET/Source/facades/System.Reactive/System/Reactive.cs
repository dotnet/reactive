// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

extern alias SystemReactiveNet;

using SystemReactiveNet::System.Reactive;

using System.Runtime.CompilerServices;

[assembly:TypeForwardedToAttribute(typeof(AnonymousObservable<>))]
[assembly:TypeForwardedToAttribute(typeof(AnonymousObserver<>))]
[assembly:TypeForwardedToAttribute(typeof(EventPattern<>))]
[assembly:TypeForwardedToAttribute(typeof(EventPattern<,>))]
[assembly:TypeForwardedToAttribute(typeof(EventPatternSourceBase<,>))]
[assembly:TypeForwardedToAttribute(typeof(ExperimentalAttribute))]
[assembly:TypeForwardedToAttribute(typeof(IEventPattern<,>))]
[assembly:TypeForwardedToAttribute(typeof(IEventPatternSource<>))]
[assembly:TypeForwardedToAttribute(typeof(IEventSource<>))]
[assembly:TypeForwardedToAttribute(typeof(IObserver<,>))]
[assembly:TypeForwardedToAttribute(typeof(ListObservable<>))]
[assembly:TypeForwardedToAttribute(typeof(NotificationKind))]
[assembly:TypeForwardedToAttribute(typeof(Notification<>))]
[assembly:TypeForwardedToAttribute(typeof(Notification))]
[assembly:TypeForwardedToAttribute(typeof(ObservableBase<>))]
[assembly:TypeForwardedToAttribute(typeof(Observer))]
[assembly:TypeForwardedToAttribute(typeof(ObserverBase<>))]
[assembly:TypeForwardedToAttribute(typeof(ITaskObservable<>))]
[assembly:TypeForwardedToAttribute(typeof(ITaskObservableAwaiter<>))]
[assembly:TypeForwardedToAttribute(typeof(TimeInterval<>))]
[assembly:TypeForwardedToAttribute(typeof(Timestamped<>))]
[assembly:TypeForwardedToAttribute(typeof(Timestamped))]
[assembly:TypeForwardedToAttribute(typeof(Unit))]

#if WINDOWS
[assembly: TypeForwardedToAttribute(typeof(System.Reactive.IEventPatternSource<,>))]
#endif
