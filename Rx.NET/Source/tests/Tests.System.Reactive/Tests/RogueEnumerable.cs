// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;

namespace ReactiveTests.Tests
{
    internal class RogueEnumerable<T> : IEnumerable<T>
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
