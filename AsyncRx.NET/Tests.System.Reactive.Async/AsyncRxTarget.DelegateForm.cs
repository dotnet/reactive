// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reflection;

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

/// <summary>The <see cref="DelegateForm"/> axis: the Impl pairs and their delegates.</summary>
/// <remarks>
/// <para>
/// An operator with a <see cref="ValueTask"/> delegate overload has two <c>*Impl</c> methods,
/// one per overload, each mirroring its own library call. A visitor passes the pair to
/// <see cref="RunByForm{TDescription}"/> or <see cref="CallByForm{TResult}"/> with the
/// scenario's arguments, and the form chooses: under <see cref="DelegateForm.Synchronous"/>
/// the first runs with the arguments as they are; under
/// <see cref="DelegateForm.Asynchronous"/> the second runs, with every delegate argument whose
/// parameter in that Impl wants a <see cref="ValueTask"/> result passed through
/// <see cref="ThroughYieldPoint(Delegate)"/>. Which arguments that is comes from the
/// asynchronous Impl's own signature, so a library overload that keeps one delegate
/// synchronous (<c>GroupByUntil</c>'s duration selector) is honoured without the visitor
/// saying so.
/// </para>
/// <para>
/// The wrapped delegate returns the scenario's result through a <see cref="ValueTask"/> after
/// awaiting the test scheduler's yield point, so that the library's await of it completes
/// synchronously or suspends as the <see cref="ExecutionShape"/> says. Off the pump, in a
/// scenario running in real time, the delegate completes synchronously. A null delegate stays
/// null, so that the library's own argument check still sees it.
/// </para>
/// </remarks>
public sealed partial class AsyncRxTarget
{
    private Realized<TDescription> RunByForm<TDescription>(
        Delegate synchronousImpl,
        Delegate asynchronousImpl,
        params object?[] args) =>
        _delegates == DelegateForm.Asynchronous
            ? _bridge.Run<TDescription>(
                asynchronousImpl, ThroughYieldPoints(asynchronousImpl, args))
            : _bridge.Run<TDescription>(synchronousImpl, args);

    private TResult CallByForm<TResult>(
        Delegate synchronousImpl,
        Delegate asynchronousImpl,
        params object?[] args) =>
        _delegates == DelegateForm.Asynchronous
            ? _bridge.Call<TResult>(
                asynchronousImpl, ThroughYieldPoints(asynchronousImpl, args))
            : _bridge.Call<TResult>(synchronousImpl, args);

    // The arguments for the asynchronous Impl: each delegate whose parameter wants a ValueTask
    // result, and does not already return one, wrapped.
    private object?[] ThroughYieldPoints(Delegate asynchronousImpl, object?[] args)
    {
        var parameters = asynchronousImpl.Method.GetParameters();
        var result = (object?[])args.Clone();
        for (var i = 0; i < args.Length; i++)
        {
            if (args[i] is Delegate f
                && ReturnsValueTask(parameters[i].ParameterType)
                && !ReturnsValueTask(f.GetType()))
            {
                result[i] = ThroughYieldPoint(f);
            }
        }

        return result;
    }

    private static bool ReturnsValueTask(Type delegateType)
    {
        if (!delegateType.IsGenericType
            || !delegateType.GetGenericTypeDefinition().FullName!.StartsWith(
                "System.Func`", StringComparison.Ordinal))
        {
            return false;
        }

        var result = delegateType.GetGenericArguments()[^1];
        return result == typeof(ValueTask)
            || (result.IsGenericType && result.GetGenericTypeDefinition() == typeof(ValueTask<>));
    }

    // The generic overload below for this delegate's shape: a Func over its type arguments, or
    // an Action over them.
    private Delegate ThroughYieldPoint(Delegate f)
    {
        var type = f.GetType();
        var typeArguments = type.IsGenericType ? type.GetGenericArguments() : [];
        var kind = type.IsGenericType ? type.GetGenericTypeDefinition().Name : type.Name;
        var isFunc = kind.StartsWith("Func", StringComparison.Ordinal);
        var overload = typeof(AsyncRxTarget)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Single(m =>
                m.Name == nameof(ThroughYieldPoint)
                && m.GetGenericArguments().Length == typeArguments.Length
                && m.GetParameters()[0].ParameterType.Name.StartsWith(
                    isFunc ? "Func" : "Action", StringComparison.Ordinal));
        if (typeArguments.Length > 0)
        {
            overload = overload.MakeGenericMethod(typeArguments);
        }

        return (Delegate)overload.Invoke(this, [f])!;
    }

    // The scheduler's yield point when the delegate runs under the pump; completed otherwise,
    // since a continuation handed to a pump that is not draining would never run (a scenario
    // in real time on another scheduler, or one that never created a test scheduler).
    private ValueTask YieldPoint()
    {
        if (_scheduler is not { IsOnPumpThread: true })
        {
            return default;
        }

        return Yield(_scheduler);

        static async ValueTask Yield(TestAsyncScheduler scheduler) => await scheduler.YieldPoint();
    }

    private Func<ValueTask<TResult>>?
        ThroughYieldPoint<TResult>(
        Func<TResult>? f) =>
        f is null
            ? null
            : async () =>
            {
                await YieldPoint();
                return f();
            };

    private Func<T1, ValueTask<TResult>>?
        ThroughYieldPoint<T1, TResult>(
        Func<T1, TResult>? f) =>
        f is null
            ? null
            : async x1 =>
            {
                await YieldPoint();
                return f(x1);
            };

    private Func<T1, T2, ValueTask<TResult>>?
        ThroughYieldPoint<T1, T2, TResult>(
        Func<T1, T2, TResult>? f) =>
        f is null
            ? null
            : async (x1, x2) =>
            {
                await YieldPoint();
                return f(x1, x2);
            };

    private Func<T1, T2, T3, ValueTask<TResult>>?
        ThroughYieldPoint<T1, T2, T3, TResult>(
        Func<T1, T2, T3, TResult>? f) =>
        f is null
            ? null
            : async (x1, x2, x3) =>
            {
                await YieldPoint();
                return f(x1, x2, x3);
            };

    private Func<T1, T2, T3, T4, ValueTask<TResult>>?
        ThroughYieldPoint<T1, T2, T3, T4, TResult>(
        Func<T1, T2, T3, T4, TResult>? f) =>
        f is null
            ? null
            : async (x1, x2, x3, x4) =>
            {
                await YieldPoint();
                return f(x1, x2, x3, x4);
            };

    private Func<T1, T2, T3, T4, T5, ValueTask<TResult>>?
        ThroughYieldPoint<T1, T2, T3, T4, T5, TResult>(
        Func<T1, T2, T3, T4, T5, TResult>? f) =>
        f is null
            ? null
            : async (x1, x2, x3, x4, x5) =>
            {
                await YieldPoint();
                return f(x1, x2, x3, x4, x5);
            };

    private Func<T1, T2, T3, T4, T5, T6, ValueTask<TResult>>?
        ThroughYieldPoint<T1, T2, T3, T4, T5, T6, TResult>(
        Func<T1, T2, T3, T4, T5, T6, TResult>? f) =>
        f is null
            ? null
            : async (x1, x2, x3, x4, x5, x6) =>
            {
                await YieldPoint();
                return f(x1, x2, x3, x4, x5, x6);
            };

    private Func<T1, T2, T3, T4, T5, T6, T7, ValueTask<TResult>>?
        ThroughYieldPoint<T1, T2, T3, T4, T5, T6, T7, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, TResult>? f) =>
        f is null
            ? null
            : async (x1, x2, x3, x4, x5, x6, x7) =>
            {
                await YieldPoint();
                return f(x1, x2, x3, x4, x5, x6, x7);
            };

    private Func<T1, T2, T3, T4, T5, T6, T7, T8, ValueTask<TResult>>?
        ThroughYieldPoint<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>? f) =>
        f is null
            ? null
            : async (x1, x2, x3, x4, x5, x6, x7, x8) =>
            {
                await YieldPoint();
                return f(x1, x2, x3, x4, x5, x6, x7, x8);
            };

    private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, ValueTask<TResult>>?
        ThroughYieldPoint<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>? f) =>
        f is null
            ? null
            : async (x1, x2, x3, x4, x5, x6, x7, x8, x9) =>
            {
                await YieldPoint();
                return f(x1, x2, x3, x4, x5, x6, x7, x8, x9);
            };

    private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, ValueTask<TResult>>?
        ThroughYieldPoint<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>? f) =>
        f is null
            ? null
            : async (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) =>
            {
                await YieldPoint();
                return f(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10);
            };

    private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, ValueTask<TResult>>?
        ThroughYieldPoint<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>? f) =>
        f is null
            ? null
            : async (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11) =>
            {
                await YieldPoint();
                return f(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11);
            };

    private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, ValueTask<TResult>>?
        ThroughYieldPoint<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>? f) =>
        f is null
            ? null
            : async (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12) =>
            {
                await YieldPoint();
                return f(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12);
            };

    private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, ValueTask<TResult>>?
        ThroughYieldPoint<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>? f) =>
        f is null
            ? null
            : async (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13) =>
            {
                await YieldPoint();
                return f(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13);
            };

    private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, ValueTask<TResult>>?
        ThroughYieldPoint<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>? f) =>
        f is null
            ? null
            : async (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14) =>
            {
                await YieldPoint();
                return f(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14);
            };

    private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
        ValueTask<TResult>>?
        ThroughYieldPoint<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>? f) =>
        f is null
            ? null
            : async (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15) =>
            {
                await YieldPoint();
                return f(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15);
            };

    private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
        ValueTask<TResult>>?
        ThroughYieldPoint<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
            TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>? f) =>
        f is null
            ? null
            : async (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15, x16) =>
            {
                await YieldPoint();
                return f(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15, x16);
            };

    private Func<ValueTask>? ThroughYieldPoint(Action? f) =>
        f is null
            ? null
            : async () =>
            {
                await YieldPoint();
                f();
            };

    private Func<T1, ValueTask>? ThroughYieldPoint<T1>(Action<T1>? f) =>
        f is null
            ? null
            : async x1 =>
            {
                await YieldPoint();
                f(x1);
            };
}
