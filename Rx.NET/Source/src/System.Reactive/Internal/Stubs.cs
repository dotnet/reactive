// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    internal static class Stubs<T>
    {
        public static readonly Action<T> Ignore = _ => { };
        public static readonly Func<T, T> I = _ => _;
    }

    internal static class Stubs
    {
        public static readonly Action Nop = () => { };
        public static readonly Action<Exception> Throw = ex => { ex.Throw(); };
    }

#if !NO_THREAD
    internal static class TimerStubs
    {
#if NETSTANDARD1_3
        public static readonly System.Threading.Timer Never = new System.Threading.Timer(_ => { }, null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
#else
        public static readonly System.Threading.Timer Never = new System.Threading.Timer(_ => { });
#endif
    }
#endif
}

