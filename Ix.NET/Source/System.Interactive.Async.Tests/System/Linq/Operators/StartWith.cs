// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using Xunit;

namespace Tests
{
    public class StartWith : AsyncEnumerableExTests
    {
        [Fact]
        public void StartWith_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.StartWith(default, new[] { 1 }));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.StartWith(Return42, default));
        }

        [Fact]
        public void StartWith1()
        {
            var xs = AsyncEnumerable.Empty<int>().StartWith(1, 2);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void StartWith2()
        {
            var xs = Return42.StartWith(40, 41);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 40);
            HasNext(e, 41);
            HasNext(e, 42);
            NoNext(e);
        }

        [Fact]
        public void StartWith3()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex).StartWith(1, 2);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }
    }
}
