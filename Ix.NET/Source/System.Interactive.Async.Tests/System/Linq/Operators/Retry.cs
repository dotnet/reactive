// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Retry : AsyncEnumerableExTests
    {
        [Fact]
        public void Retry_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Retry<int>(default(IAsyncEnumerable<int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Retry<int>(default(IAsyncEnumerable<int>), 1));
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerableEx.Retry<int>(AsyncEnumerable.Return(42), -1));
        }

        [Fact]
        public void Retry1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Retry();

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            NoNext(e);
        }

        [Fact]
        public void Retry2()
        {
            var ex = new InvalidOperationException("Bang!");

            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(AsyncEnumerable.Throw<int>(ex));

            var res = xs.Retry();

            var e = res.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
        }
    }
}
