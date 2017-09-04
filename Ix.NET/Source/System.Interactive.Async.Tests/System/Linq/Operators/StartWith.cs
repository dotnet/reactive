// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class StartWith : AsyncEnumerableExTests
    {
        [Fact]
        public void StartWith_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.StartWith(default(IAsyncEnumerable<int>), new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.StartWith(Return42, default(int[])));
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
            var xs = AsyncEnumerable.Return<int>(0).StartWith(1, 2);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 0);
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
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }
    }
}
