// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

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
