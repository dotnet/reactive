// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    internal static class TaskHelpers
    {
        private const int MaxDelay = int.MaxValue;

        public static Task Delay(TimeSpan delay, CancellationToken token)
        {
            var milliseconds = (long)delay.TotalMilliseconds;

            if (milliseconds > MaxDelay)
            {
                var remainder = delay - TimeSpan.FromMilliseconds(MaxDelay);

                return
                    Task.Delay(MaxDelay, token)
                        .ContinueWith(_ => Delay(remainder, token), TaskContinuationOptions.ExecuteSynchronously)
                        .Unwrap();
            }

            return Task.Delay(delay, token);
        }

        public static Exception GetSingleException(this Task t)
        {
            Debug.Assert(t.IsFaulted && t.Exception != null);

            if (t.Exception!.InnerException != null)
            {
                return t.Exception.InnerException;
            }

            return t.Exception;
        }
    }
}
