// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Reactive.Concurrency
{
#if !NO_THREAD
    internal static class TimerStubs
    {
        public static readonly System.Threading.Timer Never = new System.Threading.Timer(_ => { });
    }
#endif
}
