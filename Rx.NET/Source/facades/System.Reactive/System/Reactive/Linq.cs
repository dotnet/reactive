// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

extern alias SystemReactiveNet;

using SystemReactiveNet::System.Reactive.Linq;

using System.Runtime.CompilerServices;

[assembly:TypeForwardedToAttribute(typeof(IGroupedObservable<,>))]
[assembly:TypeForwardedToAttribute(typeof(IQbservable<>))]
[assembly:TypeForwardedToAttribute(typeof(IQbservable))]
[assembly:TypeForwardedToAttribute(typeof(IQbservableProvider))]
[assembly:TypeForwardedToAttribute(typeof(LocalQueryMethodImplementationTypeAttribute))]
[assembly:TypeForwardedToAttribute(typeof(Observable))]
[assembly:TypeForwardedToAttribute(typeof(ObservableEx))]
[assembly:TypeForwardedToAttribute(typeof(Qbservable))]
[assembly:TypeForwardedToAttribute(typeof(QbservableEx))]
[assembly:TypeForwardedToAttribute(typeof(QueryDebugger))]
//#if WINDOWS
//[assembly:TypeForwardedToAttribute(typeof(AsyncInfoObservable))]
//[assembly:TypeForwardedToAttribute(typeof(CoreDispatcherObservable))]
//[assembly:TypeForwardedToAttribute(typeof(WindowsObservable))]
//#endif
//#if HAS_WINFORMS
//[assembly:TypeForwardedToAttribute(typeof(ControlObservable))]
//#endif
//#if HAS_DISPATCHER
//[assembly:TypeForwardedToAttribute(typeof(DispatcherObservable))]
//#endif
//#if HAS_REMOTING
//[assembly: TypeForwardedTo(typeof(RemotingObservable))]
//#endif
