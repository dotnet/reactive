// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

using Windows.System.Threading;

namespace System.Reactive.Integration.Uwp
{
    internal static class ThreadPoolTimerExtensions
    {
        public static IDisposable AsDisposable(this ThreadPoolTimer threadPoolTimer)
        {
            return Disposable.Create(threadPoolTimer, static t => t!.Cancel());
        }
    }
}
