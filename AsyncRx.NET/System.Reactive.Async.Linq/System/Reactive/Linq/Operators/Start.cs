// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Start<TSource>(Func<TSource> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return ToAsync(function)();
        }

        public static IAsyncObservable<TSource> Start<TSource>(Func<TSource> function, IAsyncScheduler scheduler)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return ToAsync(function, scheduler)();
        }

        public static IAsyncObservable<Unit> Start(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ToAsync(action)();
        }

        public static IAsyncObservable<Unit> Start(Action action, IAsyncScheduler scheduler)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return ToAsync(action, scheduler)();
        }
    }
}
