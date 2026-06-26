// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Threading;

/// <summary>
/// Extension methods for <see cref="IAsyncGate"/>.
/// </summary>
public static class AsyncGateExtensions
{
    /// <summary>
    /// Acquires an <see cref="IAsyncGate"/> in a way enables the gate to be released with a <see langword="using" />
    /// statement or declaration.
    /// </summary>
    /// <param name="gate">The gate to lock.</param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}"/> that produces a <see cref="DisposableGateReleaser"/> that will call
    /// <see cref="IAsyncGateReleaser.Release"/> when disposed.
    /// </returns>
    public static ValueTask<DisposableGateReleaser> LockAsync(this IAsyncGate gate)
    {
        // Note, we are avoiding async/await here because we MUST NOT create a new child ExecutionContext
        // (The AsyncGate.LockAsync method does not use async/await either, and for the same reason.)
        //
        // IAsyncGate implementations are allowed to require that their LockAsync method is called from the same
        // execution context as Release will be called. For example, AsyncGate uses an AsyncLocal<int> to track
        // the recursion count, and when you update an AsyncLocal<T>'s value, that modified value is visible only
        // in the current ExecutionContext and its descendants. An async method effectively introduces a new child
        // context, so any AsyncLocal<T> value changes are lost when an async method returns, but we need the
        // recursion count to live in our caller's context, which is why we must make sure we don't introduce a
        // new child context here. That's why this needs to be old-school manual task management, and not async/await.
        ValueTask<IAsyncGateReleaser> releaserValueTask = gate.AcquireAsync();
        if (releaserValueTask.IsCompleted)
        {
            return new ValueTask<DisposableGateReleaser>(new DisposableGateReleaser(releaserValueTask.Result));
        }

        return new ValueTask<DisposableGateReleaser>(releaserValueTask.AsTask().ContinueWith(t => new DisposableGateReleaser(t.Result)));
    }
}
