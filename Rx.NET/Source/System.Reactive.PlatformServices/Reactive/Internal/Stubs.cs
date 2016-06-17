// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Concurrency
{
#if !NO_THREAD
    internal static class TimerStubs
    {
#if NETCOREAPP1_0
        public static readonly System.Threading.Timer Never = new System.Threading.Timer(_ => { }, null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
#else
        public static readonly System.Threading.Timer Never = new System.Threading.Timer(_ => { });
#endif
    }
#endif
}
