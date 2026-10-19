// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    public IEventSource<T> ToEvent<T>(Seq<T> source) =>
        _bridge.Call<IEventSource<T>>(ToEventImpl<T>, source);

    public IEventPatternSource<TEventArgs> ToEventPattern<TEventArgs>(
        Seq<EventPattern<TEventArgs>> source) =>
        _bridge.Call<IEventPatternSource<TEventArgs>>(ToEventPatternImpl<TEventArgs>, source);

    private static IEventSource<T> ToEventImpl<T>(IObservable<T> source) => source.ToEvent();

    private static IEventPatternSource<TEventArgs> ToEventPatternImpl<TEventArgs>(
        IObservable<EventPattern<TEventArgs>> source) =>
        source.ToEventPattern();
}
