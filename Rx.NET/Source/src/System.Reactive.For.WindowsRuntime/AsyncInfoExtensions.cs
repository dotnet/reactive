// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

#if LEGACY_SYSTEM_REACTIVE_FACADE
extern alias SystemReactiveNet;
using SystemReactiveNet::System.Reactive.Disposables;
#else
using System.Reactive.Disposables;
#endif
using Windows.Foundation;

namespace System.Reactive.WindowsRuntime
{
    internal static class AsyncInfoExtensions
    {
        public static IDisposable AsDisposable(this IAsyncInfo asyncInfo)
        {
            return Disposable.Create(asyncInfo, static i => i!.Cancel());
        }
    }
}
