// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

namespace Benchmarks.System.Reactive.Async.Infrastructure
{
    /// <summary>
    /// An <see cref="IAsyncResult"/> that is complete when it is created: the synchronous APM shim used by the
    /// <c>FromAsyncPattern</c> benchmarks, so only the AsyncRx wrapper is measured.
    /// </summary>
    public sealed class ImmediateAsyncResult : IAsyncResult
    {
        private ManualResetEvent? _handle;

        public ImmediateAsyncResult(object? state) => AsyncState = state;

        public object? AsyncState { get; }

        public WaitHandle AsyncWaitHandle => _handle ??= new ManualResetEvent(true);

        public bool CompletedSynchronously => true;

        public bool IsCompleted => true;

        /// <summary>A <c>Begin</c> method that completes at once: it creates the result and calls the callback inline.</summary>
        public static IAsyncResult BeginSynchronously(AsyncCallback callback, object? state)
        {
            var result = new ImmediateAsyncResult(state);
            callback?.Invoke(result);
            return result;
        }
    }
}
