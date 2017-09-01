// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class TakeLast : AsyncEnumerableTests
    {
        [Fact]
        public void TakeLast_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.TakeLast(default(IAsyncEnumerable<int>), 5));
        }

        [Fact]
        public void TakeLast0()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(-2);

            var e = xs.GetAsyncEnumerator();
            NoNext(e);
        }

        [Fact]
        public void TakeLast1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(2);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void TakeLast2()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(5);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public async Task TakeLast3()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(2);

            await SequenceIdentity(xs);
        }

        [Fact]
        public void TakeLast_BugFix_TakeLast_Zero_TakesForever()
        {
            var isSet = false;
            new int[] { 1, 2, 3, 4 }.ToAsyncEnumerable()
                .TakeLast(0)
                .ForEachAsync(_ => { isSet = true; })
                .Wait(WaitTimeoutMs);

            Assert.False(isSet);

            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(0);

            var e = xs.GetAsyncEnumerator();

            NoNext(e);
        }
    }
}
