// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace ReactiveTests.Tests
{
    class RogueEnumerable<T> : IEnumerable<T>
    {
        private readonly Exception _ex;

        public RogueEnumerable(Exception ex)
        {
            _ex = ex;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw _ex;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
