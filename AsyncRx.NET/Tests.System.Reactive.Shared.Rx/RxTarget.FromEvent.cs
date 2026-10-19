// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<TEventArgs>> ISeqVisitor.FromEventConversion<TDelegate, TEventArgs>(
            FromEventConversionSeq<TDelegate, TEventArgs> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<TEventArgs>>(FromEventConversionImpl<TDelegate, TEventArgs>,
                    seq.Conversion, seq.AddHandler, seq.RemoveHandler)
            : _bridge.Run<Seq<TEventArgs>>(FromEventConversionOnImpl<TDelegate, TEventArgs>,
                    seq.Conversion, seq.AddHandler, seq.RemoveHandler, Unwrap(seq.Scheduler));

    Realized<Seq<TEventArgs>> ISeqVisitor.FromEventDelegate<TDelegate, TEventArgs>(
            FromEventDelegateSeq<TDelegate, TEventArgs> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<TEventArgs>>(FromEventDelegateImpl<TDelegate, TEventArgs>,
                    seq.AddHandler, seq.RemoveHandler)
            : _bridge.Run<Seq<TEventArgs>>(FromEventDelegateOnImpl<TDelegate, TEventArgs>,
                    seq.AddHandler, seq.RemoveHandler, Unwrap(seq.Scheduler));

    Realized<Seq<TEventArgs>> ISeqVisitor.FromEvent<TEventArgs>(FromEventSeq<TEventArgs> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<TEventArgs>>(FromEventImpl<TEventArgs>, seq.AddHandler,
                    seq.RemoveHandler)
            : _bridge.Run<Seq<TEventArgs>>(FromEventOnImpl<TEventArgs>, seq.AddHandler,
                    seq.RemoveHandler, Unwrap(seq.Scheduler));

    Realized<Seq<Unit>> ISeqVisitor.FromEventUnit(FromEventUnitSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<Unit>>(FromEventUnitImpl, seq.AddHandler, seq.RemoveHandler)
            : _bridge.Run<Seq<Unit>>(FromEventUnitOnImpl, seq.AddHandler, seq.RemoveHandler, Unwrap(
                    seq.Scheduler));

    private static IObservable<TEventArgs> FromEventConversionImpl<TDelegate, TEventArgs>(
        Func<Action<TEventArgs>, TDelegate> conversion,
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler) =>
        Observable.FromEvent<TDelegate, TEventArgs>(conversion, addHandler, removeHandler);

    private static IObservable<TEventArgs> FromEventConversionOnImpl<TDelegate, TEventArgs>(
        Func<Action<TEventArgs>, TDelegate> conversion,
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler,
        IScheduler scheduler) =>
        Observable.FromEvent<TDelegate, TEventArgs>(conversion, addHandler, removeHandler,
                scheduler);

    private static IObservable<TEventArgs> FromEventDelegateImpl<TDelegate, TEventArgs>(
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler) =>
        Observable.FromEvent<TDelegate, TEventArgs>(addHandler, removeHandler);

    private static IObservable<TEventArgs> FromEventDelegateOnImpl<TDelegate, TEventArgs>(
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler,
        IScheduler scheduler) =>
        Observable.FromEvent<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);

    private static IObservable<TEventArgs> FromEventImpl<TEventArgs>(
        Action<Action<TEventArgs>> addHandler,
        Action<Action<TEventArgs>> removeHandler) =>
        Observable.FromEvent<TEventArgs>(addHandler, removeHandler);

    private static IObservable<TEventArgs> FromEventOnImpl<TEventArgs>(
        Action<Action<TEventArgs>> addHandler,
        Action<Action<TEventArgs>> removeHandler,
        IScheduler scheduler) =>
        Observable.FromEvent<TEventArgs>(addHandler, removeHandler, scheduler);

    private static IObservable<Unit> FromEventUnitImpl(
        Action<Action> addHandler,
        Action<Action> removeHandler) =>
        Observable.FromEvent(addHandler, removeHandler);

    private static IObservable<Unit> FromEventUnitOnImpl(
        Action<Action> addHandler,
        Action<Action> removeHandler,
        IScheduler scheduler) =>
        Observable.FromEvent(addHandler, removeHandler, scheduler);
}
