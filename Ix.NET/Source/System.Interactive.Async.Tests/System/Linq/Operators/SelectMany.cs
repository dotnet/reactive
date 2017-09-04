// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class SelectMany : AsyncEnumerableExTests
    {
        [Fact]
        public void SelectMany_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.SelectMany<int, int>(default(IAsyncEnumerable<int>), Return42));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.SelectMany<int, int>(Return42, default(IAsyncEnumerable<int>)));
        }

        [Fact]
        public void SelectMany1()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 4 }.ToAsyncEnumerable();

            var res = xs.SelectMany(ys);

            var e = res.GetAsyncEnumerator();
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }
    }
}
