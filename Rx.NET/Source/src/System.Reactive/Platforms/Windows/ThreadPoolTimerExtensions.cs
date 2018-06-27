// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if WINDOWS
using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
    internal static class ThreadPoolTimerExtensions
    {
        public static IDisposable AsDisposable(this global::Windows.System.Threading.ThreadPoolTimer threadPoolTimer)
        {
            return Disposable.Create(threadPoolTimer, _ => _.Cancel());
        }

        public static IDisposable AsDisposable(this global::Windows.Foundation.IAsyncInfo asyncInfo)
        {
            return Disposable.Create(asyncInfo, _ => _.Cancel());
        }
    }
}
#endif
