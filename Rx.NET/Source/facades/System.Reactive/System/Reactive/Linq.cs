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

// These UI-framework-specific types have been moved to the platform-specific packages. To maintain binary
// compatibility we forward the types to the new packages. However, when building reference assemblies for
// the netX.0-windows10.0.X target, we do not include these types in the reference assembly. We do this to
// enable applications that target a Windows-specific .NET TFM to use Rx without needing to reference the
// Microsoft.WindowsDesktop.App framework. If we don't hide the UI-framework-specific types, they become
// visible to the compiler, which can then complain that it can't find the Windows Forms or WPF types
// that these types use. (In particular, this becomes a problem for the types that add UI-framework-specific
// extension methods such as ObserveOn(Control). Code that is trying to use the non-UI-framework-specific
// overloads fails to compile because the compiler can't find the Control type, and so it doesn't have
// the information it requires to be determine that the Control overloads is not applicable.) So we
// exclude UI-framework-specific types from the .NET '-windows' reference assembly, but we keep the
// forwarders in the runtime assembly.

#if !(BUILDING_REFERENCE_ASSEMBLY && NET8_0_OR_GREATER)
#if WINDOWS
[assembly:TypeForwardedToAttribute(typeof(System.Reactive.Linq.AsyncInfoObservable))]
[assembly:TypeForwardedToAttribute(typeof(System.Reactive.Linq.CoreDispatcherObservable))]
[assembly:TypeForwardedToAttribute(typeof(System.Reactive.Linq.WindowsObservable))]
#endif
#if HAS_WINFORMS
[assembly:TypeForwardedToAttribute(typeof(System.Reactive.Linq.ControlObservable))]
#endif
#if HAS_DISPATCHER
[assembly: TypeForwardedToAttribute(typeof(System.Reactive.Linq.DispatcherObservable))]
#endif
#endif
