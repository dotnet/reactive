// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<EventPattern<object>>> ISeqVisitor.FromEventPatternHandler(
            FromEventPatternHandlerSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<EventPattern<object>>>(FromEventPatternHandlerImpl, seq.AddHandler,
                    seq.RemoveHandler)
            : _bridge.Run<Seq<EventPattern<object>>>(FromEventPatternHandlerOnImpl, seq.AddHandler,
                    seq.RemoveHandler, Unwrap(seq.Scheduler));

    Realized<Seq<EventPattern<TEventArgs>>> ISeqVisitor.FromEventPatternDelegate<TDelegate,
            TEventArgs>(FromEventPatternDelegateSeq<TDelegate, TEventArgs> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<EventPattern<TEventArgs>>>(FromEventPatternDelegateImpl<TDelegate,
                    TEventArgs>, seq.AddHandler, seq.RemoveHandler)
            : _bridge.Run<Seq<EventPattern<TEventArgs>>>(FromEventPatternDelegateOnImpl<TDelegate,
                    TEventArgs>, seq.AddHandler, seq.RemoveHandler, Unwrap(seq.Scheduler));

    Realized<Seq<EventPattern<TEventArgs>>> ISeqVisitor.FromEventPatternConversion<TDelegate,
            TEventArgs>(FromEventPatternConversionSeq<TDelegate, TEventArgs> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<EventPattern<TEventArgs>>>(FromEventPatternConversionImpl<TDelegate,
                    TEventArgs>, seq.Conversion, seq.AddHandler, seq.RemoveHandler)
            : _bridge.Run<Seq<EventPattern<TEventArgs>>>(FromEventPatternConversionOnImpl<TDelegate,
                    TEventArgs>, seq.Conversion, seq.AddHandler, seq.RemoveHandler, Unwrap(
                    seq.Scheduler));

    Realized<Seq<EventPattern<TSender, TEventArgs>>> ISeqVisitor.FromEventPatternSender<TDelegate,
            TSender, TEventArgs>(FromEventPatternSenderSeq<TDelegate, TSender, TEventArgs> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<EventPattern<TSender, TEventArgs>>>(FromEventPatternSenderImpl<
                    TDelegate, TSender, TEventArgs>, seq.AddHandler, seq.RemoveHandler)
            : _bridge.Run<Seq<EventPattern<TSender, TEventArgs>>>(FromEventPatternSenderOnImpl<
                    TDelegate, TSender, TEventArgs>, seq.AddHandler, seq.RemoveHandler, Unwrap(
                    seq.Scheduler));

    Realized<Seq<EventPattern<TEventArgs>>> ISeqVisitor.FromEventPattern<TEventArgs>(
            FromEventPatternSeq<TEventArgs> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<EventPattern<TEventArgs>>>(FromEventPatternImpl<TEventArgs>,
                    seq.AddHandler, seq.RemoveHandler)
            : _bridge.Run<Seq<EventPattern<TEventArgs>>>(FromEventPatternOnImpl<TEventArgs>,
                    seq.AddHandler, seq.RemoveHandler, Unwrap(seq.Scheduler));

    Realized<Seq<EventPattern<object>>> ISeqVisitor.FromEventPatternInstanceObject(
            FromEventPatternInstanceObjectSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<EventPattern<object>>>(FromEventPatternInstanceObjectImpl, seq.Target,
                    seq.EventName)
            : _bridge.Run<Seq<EventPattern<object>>>(FromEventPatternInstanceObjectOnImpl,
                    seq.Target, seq.EventName, Unwrap(seq.Scheduler));

    Realized<Seq<EventPattern<TEventArgs>>> ISeqVisitor.FromEventPatternInstance<TEventArgs>(
            FromEventPatternInstanceSeq<TEventArgs> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<EventPattern<TEventArgs>>>(FromEventPatternInstanceImpl<TEventArgs>,
                    seq.Target, seq.EventName)
            : _bridge.Run<Seq<EventPattern<TEventArgs>>>(FromEventPatternInstanceOnImpl<TEventArgs>,
                    seq.Target, seq.EventName, Unwrap(seq.Scheduler));

    Realized<Seq<EventPattern<TSender, TEventArgs>>> ISeqVisitor.FromEventPatternInstanceSender<
            TSender, TEventArgs>(FromEventPatternInstanceSenderSeq<TSender, TEventArgs> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<EventPattern<TSender, TEventArgs>>>(
                    FromEventPatternInstanceSenderImpl<TSender, TEventArgs>, seq.Target,
                    seq.EventName)
            : _bridge.Run<Seq<EventPattern<TSender, TEventArgs>>>(
                    FromEventPatternInstanceSenderOnImpl<TSender, TEventArgs>, seq.Target,
                    seq.EventName, Unwrap(seq.Scheduler));

    Realized<Seq<EventPattern<object>>> ISeqVisitor.FromEventPatternStaticObject(
            FromEventPatternStaticObjectSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<EventPattern<object>>>(FromEventPatternStaticObjectImpl, seq.Type,
                    seq.EventName)
            : _bridge.Run<Seq<EventPattern<object>>>(FromEventPatternStaticObjectOnImpl, seq.Type,
                    seq.EventName, Unwrap(seq.Scheduler));

    Realized<Seq<EventPattern<TEventArgs>>> ISeqVisitor.FromEventPatternStatic<TEventArgs>(
            FromEventPatternStaticSeq<TEventArgs> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<EventPattern<TEventArgs>>>(FromEventPatternStaticImpl<TEventArgs>,
                    seq.Type, seq.EventName)
            : _bridge.Run<Seq<EventPattern<TEventArgs>>>(FromEventPatternStaticOnImpl<TEventArgs>,
                    seq.Type, seq.EventName, Unwrap(seq.Scheduler));

    Realized<Seq<EventPattern<TSender, TEventArgs>>> ISeqVisitor.FromEventPatternStaticSender<
            TSender, TEventArgs>(FromEventPatternStaticSenderSeq<TSender, TEventArgs> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<EventPattern<TSender, TEventArgs>>>(FromEventPatternStaticSenderImpl<
                    TSender, TEventArgs>, seq.Type, seq.EventName)
            : _bridge.Run<Seq<EventPattern<TSender, TEventArgs>>>(
                    FromEventPatternStaticSenderOnImpl<TSender, TEventArgs>, seq.Type,
                    seq.EventName, Unwrap(seq.Scheduler));

    private static IObservable<EventPattern<object>> FromEventPatternHandlerImpl(
        Action<EventHandler> addHandler,
        Action<EventHandler> removeHandler) =>
        Observable.FromEventPattern(addHandler, removeHandler);

    private static IObservable<EventPattern<object>> FromEventPatternHandlerOnImpl(
        Action<EventHandler> addHandler,
        Action<EventHandler> removeHandler,
        IScheduler scheduler) =>
        Observable.FromEventPattern(addHandler, removeHandler, scheduler);

    private static IObservable<EventPattern<TEventArgs>> FromEventPatternDelegateImpl<TDelegate,
            TEventArgs>(
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler) =>
        Observable.FromEventPattern<TDelegate, TEventArgs>(addHandler, removeHandler);

    private static IObservable<EventPattern<TEventArgs>> FromEventPatternDelegateOnImpl<TDelegate,
            TEventArgs>(
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler,
        IScheduler scheduler) =>
        Observable.FromEventPattern<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);

    private static IObservable<EventPattern<TEventArgs>> FromEventPatternConversionImpl<TDelegate,
            TEventArgs>(
        Func<EventHandler<TEventArgs>, TDelegate> conversion,
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler) =>
        Observable.FromEventPattern<TDelegate, TEventArgs>(conversion, addHandler, removeHandler);

    private static IObservable<EventPattern<TEventArgs>> FromEventPatternConversionOnImpl<TDelegate,
            TEventArgs>(
        Func<EventHandler<TEventArgs>, TDelegate> conversion,
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler,
        IScheduler scheduler) =>
        Observable.FromEventPattern<TDelegate, TEventArgs>(conversion, addHandler, removeHandler,
                scheduler);

    private static IObservable<EventPattern<TSender, TEventArgs>> FromEventPatternSenderImpl<
            TDelegate, TSender, TEventArgs>(
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler) =>
        Observable.FromEventPattern<TDelegate, TSender, TEventArgs>(addHandler, removeHandler);

    private static IObservable<EventPattern<TSender, TEventArgs>> FromEventPatternSenderOnImpl<
            TDelegate, TSender, TEventArgs>(
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler,
        IScheduler scheduler) =>
        Observable.FromEventPattern<TDelegate, TSender, TEventArgs>(addHandler, removeHandler,
                scheduler);

    private static IObservable<EventPattern<TEventArgs>> FromEventPatternImpl<TEventArgs>(
        Action<EventHandler<TEventArgs>> addHandler,
        Action<EventHandler<TEventArgs>> removeHandler) =>
        Observable.FromEventPattern<TEventArgs>(addHandler, removeHandler);

    private static IObservable<EventPattern<TEventArgs>> FromEventPatternOnImpl<TEventArgs>(
        Action<EventHandler<TEventArgs>> addHandler,
        Action<EventHandler<TEventArgs>> removeHandler,
        IScheduler scheduler) =>
        Observable.FromEventPattern<TEventArgs>(addHandler, removeHandler, scheduler);

    private static IObservable<EventPattern<object>> FromEventPatternInstanceObjectImpl(
        object target,
        string eventName) =>
        Observable.FromEventPattern(target, eventName);

    private static IObservable<EventPattern<object>> FromEventPatternInstanceObjectOnImpl(
        object target,
        string eventName,
        IScheduler scheduler) =>
        Observable.FromEventPattern(target, eventName, scheduler);

    private static IObservable<EventPattern<TEventArgs>> FromEventPatternInstanceImpl<TEventArgs>(
        object target,
        string eventName) =>
        Observable.FromEventPattern<TEventArgs>(target, eventName);

    private static IObservable<EventPattern<TEventArgs>> FromEventPatternInstanceOnImpl<TEventArgs>(
        object target,
        string eventName,
        IScheduler scheduler) =>
        Observable.FromEventPattern<TEventArgs>(target, eventName, scheduler);

    private static IObservable<EventPattern<TSender,
            TEventArgs>> FromEventPatternInstanceSenderImpl<TSender, TEventArgs>(
        object target,
        string eventName) =>
        Observable.FromEventPattern<TSender, TEventArgs>(target, eventName);

    private static IObservable<EventPattern<TSender,
            TEventArgs>> FromEventPatternInstanceSenderOnImpl<TSender, TEventArgs>(
        object target,
        string eventName,
        IScheduler scheduler) =>
        Observable.FromEventPattern<TSender, TEventArgs>(target, eventName, scheduler);

    private static IObservable<EventPattern<object>> FromEventPatternStaticObjectImpl(
        Type type,
        string eventName) =>
        Observable.FromEventPattern(type, eventName);

    private static IObservable<EventPattern<object>> FromEventPatternStaticObjectOnImpl(
        Type type,
        string eventName,
        IScheduler scheduler) =>
        Observable.FromEventPattern(type, eventName, scheduler);

    private static IObservable<EventPattern<TEventArgs>> FromEventPatternStaticImpl<TEventArgs>(
        Type type,
        string eventName) =>
        Observable.FromEventPattern<TEventArgs>(type, eventName);

    private static IObservable<EventPattern<TEventArgs>> FromEventPatternStaticOnImpl<TEventArgs>(
        Type type,
        string eventName,
        IScheduler scheduler) =>
        Observable.FromEventPattern<TEventArgs>(type, eventName, scheduler);

    private static IObservable<EventPattern<TSender, TEventArgs>> FromEventPatternStaticSenderImpl<
            TSender, TEventArgs>(
        Type type,
        string eventName) =>
        Observable.FromEventPattern<TSender, TEventArgs>(type, eventName);

    private static IObservable<EventPattern<TSender,
            TEventArgs>> FromEventPatternStaticSenderOnImpl<TSender, TEventArgs>(
        Type type,
        string eventName,
        IScheduler scheduler) =>
        Observable.FromEventPattern<TSender, TEventArgs>(type, eventName, scheduler);
}
