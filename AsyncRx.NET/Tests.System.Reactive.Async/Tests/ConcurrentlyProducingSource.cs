// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace Tests.System.Reactive.Async.Tests;

/// <summary>
/// A source that pushes two runs of values into its observer from two tasks at once.
/// </summary>
/// <param name="first">The values the first task pushes.</param>
/// <param name="second">The values the second task pushes.</param>
/// <param name="secondStartsAfter">
/// If not null, the second task waits for this before pushing, so that a test can place its
/// first push at a chosen moment of the first task's delivery.
/// </param>
/// <remarks>
/// Rx.NET's <c>Synchronize_BadObservable</c> builds such a source in its test with two
/// threads; this is its AsyncRx.NET counterpart, written by hand because the library gives no
/// help in breaking the observer grammar on purpose. It implements <see cref="IAsyncObservable{T}"/>
/// itself rather than deriving from the library's base, whose subscription would wrap the
/// observer in one that rejects a notification arriving while another is in progress, which is
/// what the tests over it are about. Each task records the first exception its pushes raise in
/// <see cref="Failures"/> and stops; when both have finished, the source completes the observer
/// if neither failed, and leaves it alone otherwise, since an observer that rejected a value
/// would reject the completion too.
/// </remarks>
internal sealed class ConcurrentlyProducingSource(
    IEnumerable<int> first,
    IEnumerable<int> second,
    Task? secondStartsAfter = null)
    : IAsyncObservable<int>
{
    /// <summary>The first exception each task's pushes raised, if any.</summary>
    public List<Exception> Failures { get; } = [];

    /// <summary>Completes when both tasks have finished pushing.</summary>
    public Task Done => _done.Task;

    /// <summary>Completes with the first exception a push raised, when one does.</summary>
    public Task<Exception> FirstFailure => _firstFailure.Task;

    private readonly TaskCompletionSource _done =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    private readonly TaskCompletionSource<Exception> _firstFailure =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    public async ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<int> observer)
    {
        async Task Push(IEnumerable<int> values, Task? after)
        {
            if (after is not null)
            {
                await after;
            }

            try
            {
                foreach (var value in values)
                {
                    await observer.OnNextAsync(value);
                }
            }
            catch (Exception ex)
            {
                lock (Failures)
                {
                    Failures.Add(ex);
                }

                _firstFailure.TrySetResult(ex);
            }
        }

        var t1 = Task.Run(() => Push(first, null));
        var t2 = Task.Run(() => Push(second, secondStartsAfter));

        _ = Task.Run(async () =>
        {
            await Task.WhenAll(t1, t2);

            if (Failures.Count == 0)
            {
                await observer.OnCompletedAsync();
            }

            _done.TrySetResult();
        });

        return AsyncDisposable.Nop;
    }
}
