// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

extern alias SystemReactiveNet;

using SystemReactiveNet::System.Reactive.Subjects;

using System.Runtime.CompilerServices;

[assembly:TypeForwardedToAttribute(typeof(AsyncSubject<>))]
[assembly:TypeForwardedToAttribute(typeof(BehaviorSubject<>))]
[assembly:TypeForwardedToAttribute(typeof(IConnectableObservable<>))]
[assembly:TypeForwardedToAttribute(typeof(ISubject<>))]
[assembly:TypeForwardedToAttribute(typeof(ISubject<,>))]
[assembly:TypeForwardedToAttribute(typeof(ReplaySubject<>))]
[assembly:TypeForwardedToAttribute(typeof(Subject<>))]
[assembly:TypeForwardedToAttribute(typeof(Subject))]
[assembly:TypeForwardedToAttribute(typeof(SubjectBase<>))]
