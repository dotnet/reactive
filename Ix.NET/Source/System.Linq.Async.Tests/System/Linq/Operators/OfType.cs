// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class OfType : AsyncEnumerableTests
    {
        [Fact]
        public void OfType_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OfType<int>(default));
        }

        [Fact]
        public void OfType1()
        {
            var xs = new object[] { 1, 1.2, true, 4, "" }.ToAsyncEnumerable();
            var ys = xs.OfType<int>();

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 4);
            NoNext(e);
        }
    }
}
