// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace ReactiveTests.Dummies
{
    class DummyEnumerable<T> : IEnumerable<T>
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

    class NullEnumeratorEnumerable<T> : IEnumerable<T>
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
