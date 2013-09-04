// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

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
        public static readonly System.Threading.Timer Never = new System.Threading.Timer(_ => { });
    }
#endif
}
