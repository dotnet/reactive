// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

#if WINDOWS
using System.Reactive.Disposables;
using Windows.System.Threading;
using Windows.Foundation;

namespace System.Reactive.Concurrency
{
    internal static class ThreadPoolTimerExtensions
    {
        public static IDisposable AsDisposable(this ThreadPoolTimer threadPoolTimer)
        {
            return Disposable.Create(threadPoolTimer, static t => t!.Cancel());
        }

        public static IDisposable AsDisposable(this IAsyncInfo asyncInfo)
        {
            return Disposable.Create(asyncInfo, static i => i!.Cancel());
        }
    }
}
#endif
