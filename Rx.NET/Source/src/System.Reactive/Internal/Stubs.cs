// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    internal static class Stubs<T>
    {
        public static readonly Action<T> Ignore = static _ => { };
        public static readonly Func<T, T> I = static _ => _;
    }

    internal static class Stubs
    {
        public static readonly Action Nop = static () => { };
        public static readonly Action<Exception> Throw = static ex => { ex.Throw(); };
    }

    internal static class TimerStubs
    {
        public static readonly System.Threading.Timer Never = new(static _ => { });
    }
}

