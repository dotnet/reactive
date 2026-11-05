// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// Rx.NET's <c>TestLongRunningScheduler</c>, for the tests written against Rx.NET.
/// </summary>
/// <remarks>
/// The original's own test double, as it stands in <c>Tests.System.Reactive</c>: a long-running
/// scheduler whose one unit of work runs on a new task, handing back the events it sets at
/// start and at end and the exception it threw. Named for the runner because the kit's shared
/// <see cref="Scenarios.TestLongRunningScheduler"/> keeps the original's name.
/// </remarks>
internal sealed class RxTestLongRunningScheduler
    : IScheduler, ISchedulerLongRunning, IServiceProvider
{
    private readonly Action<ManualResetEvent> _setStart;
    private readonly Action<ManualResetEvent> _setEnd;
    private readonly Action<Exception>? _setException;

    public RxTestLongRunningScheduler(
        Action<ManualResetEvent> setStart,
        Action<ManualResetEvent> setEnd)
        : this(setStart, setEnd, null)
    {
    }

    public RxTestLongRunningScheduler(
        Action<ManualResetEvent> setStart,
        Action<ManualResetEvent> setEnd,
        Action<Exception>? setException)
    {
        _setStart = setStart;
        _setEnd = setEnd;
        _setException = setException;
    }

    public DateTimeOffset Now => DateTimeOffset.Now;

    public IDisposable Schedule<TState>(
        TState state,
        Func<IScheduler, TState, IDisposable> action) =>
        throw new NotImplementedException();

    public IDisposable Schedule<TState>(
        TState state,
        TimeSpan dueTime,
        Func<IScheduler, TState, IDisposable> action) =>
        throw new NotImplementedException();

    public IDisposable Schedule<TState>(
        TState state,
        DateTimeOffset dueTime,
        Func<IScheduler, TState, IDisposable> action) =>
        throw new NotImplementedException();

    public IDisposable ScheduleLongRunning<TState>(
        TState state,
        Action<TState, ICancelable> action)
    {
        var d = new BooleanDisposable();

        var eb = new ManualResetEvent(false);
        _setStart(eb);

        var ee = new ManualResetEvent(false);
        _setEnd(ee);

        Task.Run(() =>
        {
            eb.Set();

            try
            {
                action(state, d);
            }
            catch (Exception ex)
            {
                if (_setException == null)
                {
                    throw;
                }

                _setException(ex);
            }
            finally
            {
                ee.Set();
            }
        });

        return d;
    }

    object? IServiceProvider.GetService(Type serviceType) =>
        serviceType == typeof(ISchedulerLongRunning) ? this : null;
}
