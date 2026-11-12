// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
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

    private static IAsyncObservable<TEventArgs> FromEventConversionImpl<TDelegate, TEventArgs>(
        Func<Action<TEventArgs>, TDelegate> conversion,
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler) =>
        AsyncObservable.FromEvent<TDelegate, TEventArgs>(conversion, addHandler, removeHandler);

    private static IAsyncObservable<TEventArgs> FromEventConversionOnImpl<TDelegate, TEventArgs>(
        Func<Action<TEventArgs>, TDelegate> conversion,
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler,
        IAsyncScheduler scheduler) =>
        AsyncObservable.FromEvent<TDelegate, TEventArgs>(conversion, addHandler, removeHandler,
                scheduler);

    private static IAsyncObservable<TEventArgs> FromEventDelegateImpl<TDelegate, TEventArgs>(
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler) =>
        AsyncObservable.FromEvent<TDelegate, TEventArgs>(addHandler, removeHandler);

    private static IAsyncObservable<TEventArgs> FromEventDelegateOnImpl<TDelegate, TEventArgs>(
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler,
        IAsyncScheduler scheduler) =>
        AsyncObservable.FromEvent<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);

    private static IAsyncObservable<TEventArgs> FromEventImpl<TEventArgs>(
        Action<Action<TEventArgs>> addHandler,
        Action<Action<TEventArgs>> removeHandler) =>
        AsyncObservable.FromEvent<TEventArgs>(addHandler, removeHandler);

    private static IAsyncObservable<TEventArgs> FromEventOnImpl<TEventArgs>(
        Action<Action<TEventArgs>> addHandler,
        Action<Action<TEventArgs>> removeHandler,
        IAsyncScheduler scheduler) =>
        AsyncObservable.FromEvent<TEventArgs>(addHandler, removeHandler, scheduler);

    private static IAsyncObservable<Unit> FromEventUnitImpl(
        Action<Action> addHandler,
        Action<Action> removeHandler) =>
        AsyncObservable.FromEvent(addHandler, removeHandler);

    private static IAsyncObservable<Unit> FromEventUnitOnImpl(
        Action<Action> addHandler,
        Action<Action> removeHandler,
        IAsyncScheduler scheduler) =>
        AsyncObservable.FromEvent(addHandler, removeHandler, scheduler);
}
