// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;

namespace ReactiveTests.Dummies
{
    internal static class DummyFunc<T>
    {
        public static readonly Func<T> Instance = () => { throw new NotImplementedException(); };
    }

    internal static class DummyFunc<T, U>
    {
        public static readonly Func<T, U> Instance = t => { throw new NotImplementedException(); };
    }

    internal static class DummyFunc<T, U, V>
    {
        public static readonly Func<T, U, V> Instance = (t, u) => { throw new NotImplementedException(); };
    }

    internal static class DummyFunc<T, U, V, W>
    {
        public static readonly Func<T, U, V, W> Instance = (t, u, v) => { throw new NotImplementedException(); };
    }

    internal static class DummyFunc<T, U, V, W, X>
    {
        public static readonly Func<T, U, V, W, X> Instance = (t, u, v, w) => { throw new NotImplementedException(); };
    }

    internal static class DummyAction
    {
        public static readonly Action Instance = () => { throw new NotImplementedException(); };
    }

    internal static class DummyAction<T>
    {
        public static readonly Action<T> Instance = t => { throw new NotImplementedException(); };
    }

    internal static class DummyAction<T, U>
    {
        public static readonly Action<T, U> Instance = (t, u) => { throw new NotImplementedException(); };
    }
}
