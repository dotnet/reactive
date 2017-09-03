// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class AsAsyncEnumerable : AsyncEnumerableTests
    {
        [Fact]
        public void AsAsyncEnumerable_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.AsAsyncEnumerable(default(IAsyncEnumerable<int>)));
        }

        [Fact]
        public void AsAsyncEnumerable1()
        {
            var xs = AsyncEnumerable.Return(42);
            var ys = xs.AsAsyncEnumerable();

            Assert.NotSame(xs, ys);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 42);
            NoNext(e);
        }
    }
}
