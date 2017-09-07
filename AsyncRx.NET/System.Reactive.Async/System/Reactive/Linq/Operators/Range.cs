// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<int> Range(int start, int count)
        {
            var max = ((long)start) + count - 1;

            if (count < 0 || max > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(count));

            throw new NotImplementedException();
        }

        public static IAsyncObservable<int> Range(int start, int count, IAsyncScheduler scheduler)
        {
            var max = ((long)start) + count - 1;

            if (count < 0 || max > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            throw new NotImplementedException();
        }
    }
}
