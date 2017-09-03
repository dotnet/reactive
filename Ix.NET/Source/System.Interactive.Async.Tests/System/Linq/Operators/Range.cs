// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Range : AsyncEnumerableExTests
    {
        [Fact]
        public void Range_Null()
        {
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.Range(0, -1));
        }

        [Fact]
        public void Range1()
        {
            var xs = AsyncEnumerable.Range(2, 5);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            NoNext(e);
        }

        [Fact]
        public void Range2()
        {
            var xs = AsyncEnumerable.Range(2, 0);

            var e = xs.GetAsyncEnumerator();
            NoNext(e);
        }
    }
}
