// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TResult> FromAsync<TResult>(Func<Task<TResult>> functionAsync)
        {
            if (functionAsync == null)
                throw new ArgumentNullException(nameof(functionAsync));

            return Defer(() => StartAsync(functionAsync));
        }

        public static IAsyncObservable<TResult> FromAsync<TResult>(Func<Task<TResult>> functionAsync, IAsyncScheduler scheduler)
        {
            if (functionAsync == null)
                throw new ArgumentNullException(nameof(functionAsync));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Defer(() => StartAsync(functionAsync, scheduler));
        }

        public static IAsyncObservable<TResult> FromAsync<TResult>(Func<CancellationToken, Task<TResult>> functionAsync)
        {
            if (functionAsync == null)
                throw new ArgumentNullException(nameof(functionAsync));

            return Defer(() => StartAsync(functionAsync));
        }

        public static IAsyncObservable<TResult> FromAsync<TResult>(Func<CancellationToken, Task<TResult>> functionAsync, IAsyncScheduler scheduler)
        {
            if (functionAsync == null)
                throw new ArgumentNullException(nameof(functionAsync));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Defer(() => StartAsync(functionAsync, scheduler));
        }

        public static IAsyncObservable<Unit> FromAsync(Func<Task> actionAsync)
        {
            if (actionAsync == null)
                throw new ArgumentNullException(nameof(actionAsync));

            return Defer(() => StartAsync(actionAsync));
        }

        public static IAsyncObservable<Unit> FromAsync(Func<Task> actionAsync, IAsyncScheduler scheduler)
        {
            if (actionAsync == null)
                throw new ArgumentNullException(nameof(actionAsync));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Defer(() => StartAsync(actionAsync, scheduler));
        }

        public static IAsyncObservable<Unit> FromAsync(Func<CancellationToken, Task> actionAsync)
        {
            if (actionAsync == null)
                throw new ArgumentNullException(nameof(actionAsync));

            return Defer(() => StartAsync(actionAsync));
        }

        public static IAsyncObservable<Unit> FromAsync(Func<CancellationToken, Task> actionAsync, IAsyncScheduler scheduler)
        {
            if (actionAsync == null)
                throw new ArgumentNullException(nameof(actionAsync));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Defer(() => StartAsync(actionAsync, scheduler));
        }
    }
}
