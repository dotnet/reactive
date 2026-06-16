// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

#if BUILDING_REFERENCE_ASSEMBLY
using System.Reactive.Disposables;
#else
extern alias SystemReactive;
using SystemReactive::System.Reactive.Disposables;
#endif

using Windows.System.Threading;

namespace System.Reactive.Uwp
{
    internal static class ThreadPoolTimerExtensions
    {
        public static IDisposable AsDisposable(this ThreadPoolTimer threadPoolTimer)
        {
            return Disposable.Create(threadPoolTimer, static t => t!.Cancel());
        }
    }
}
