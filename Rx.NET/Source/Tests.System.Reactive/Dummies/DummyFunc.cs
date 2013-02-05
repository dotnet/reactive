// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;

namespace ReactiveTests.Dummies
{
    static class DummyFunc<T>
    {
        public static readonly Func<T> Instance = () => { throw new NotImplementedException(); };
    }

    static class DummyFunc<T, U>
    {
        public static readonly Func<T, U> Instance = t => { throw new NotImplementedException(); };
    }

    static class DummyFunc<T, U, V>
    {
        public static readonly Func<T, U, V> Instance = (t, u) => { throw new NotImplementedException(); };
    }

    static class DummyFunc<T, U, V, W>
    {
        public static readonly Func<T, U, V, W> Instance = (t, u, v) => { throw new NotImplementedException(); };
    }

    static class DummyFunc<T, U, V, W, X>
    {
        public static readonly Func<T, U, V, W, X> Instance = (t, u, v, w) => { throw new NotImplementedException(); };
    }

    static class DummyAction
    {
        public static readonly Action Instance = () => { throw new NotImplementedException(); };
    }

    static class DummyAction<T>
    {
        public static readonly Action<T> Instance = t => { throw new NotImplementedException(); };
    }

    static class DummyAction<T, U>
    {
        public static readonly Action<T, U> Instance = (t, u) => { throw new NotImplementedException(); };
    }
}
