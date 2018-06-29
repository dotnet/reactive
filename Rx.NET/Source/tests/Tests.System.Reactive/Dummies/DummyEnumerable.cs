// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;

namespace ReactiveTests.Dummies
{
    internal class DummyEnumerable<T> : IEnumerable<T>
    {
        public static readonly DummyEnumerable<T> Instance = new DummyEnumerable<T>();

        private DummyEnumerable()
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    internal class NullEnumeratorEnumerable<T> : IEnumerable<T>
    {
        public static readonly NullEnumeratorEnumerable<T> Instance = new NullEnumeratorEnumerable<T>();

        private NullEnumeratorEnumerable()
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            return null;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
