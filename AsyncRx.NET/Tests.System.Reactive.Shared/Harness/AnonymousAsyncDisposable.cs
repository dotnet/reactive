// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>An async disposable that runs an action once, on first disposal.</summary>
/// <param name="dispose">The action to run.</param>
/// <remarks>
/// What a scenario builds where the Rx.NET test builds a <c>CompositeDisposable</c> or a
/// <c>Disposable.Create</c> whose work is now asynchronous, such as disposing a subscription
/// it holds and then setting a flag.
/// </remarks>
public sealed class AnonymousAsyncDisposable(Func<ValueTask> dispose) : IAsyncDisposable
{
    private Func<ValueTask>? _dispose = dispose ?? throw new ArgumentNullException(nameof(dispose));

    /// <summary>Runs the action if it has not run yet.</summary>
    public ValueTask DisposeAsync()
    {
        var dispose = Interlocked.Exchange(ref _dispose, null);
        return dispose is null ? default : dispose();
    }
}
