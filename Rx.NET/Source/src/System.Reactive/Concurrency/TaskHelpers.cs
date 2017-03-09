// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if !NO_TPL && !NO_TASK_DELAY
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    static class TaskHelpers
    {
        private const int MAX_DELAY = int.MaxValue;

        public static Task Delay(TimeSpan delay, CancellationToken token)
        {
            var milliseconds = (long)delay.TotalMilliseconds;

            if (milliseconds > MAX_DELAY)
            {
                var remainder = delay - TimeSpan.FromMilliseconds(MAX_DELAY);

                return
#if USE_TASKEX
                    TaskEx.Delay(MAX_DELAY, token)
#else
                    Task.Delay(MAX_DELAY, token)
#endif
                        .ContinueWith(_ => Delay(remainder, token), TaskContinuationOptions.ExecuteSynchronously)
                        .Unwrap();
            }

#if USE_TASKEX
            return TaskEx.Delay(delay, token);
#else
            return Task.Delay(delay, token);
#endif
        }
    }
}
#endif