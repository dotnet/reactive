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
    public class Scan : AsyncEnumerableExTests
    {
        [Fact]
        public void Scan_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Scan(default(IAsyncEnumerable<int>), 3, (x, y) => x + y));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Scan(Return42, 3, default(Func<int, int, int>)));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Scan(default(IAsyncEnumerable<int>), (x, y) => x + y));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Scan(Return42, default(Func<int, int, int>)));
        }

        [Fact]
        public async Task Scan1Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan(8, (x, y) => x + y);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 9);
            await HasNextAsync(e, 11);
            await HasNextAsync(e, 14);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Scan2Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan((x, y) => x + y);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Scan3()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan(8, new Func<int, int, int>((x, y) => { throw ex; }));

            var e = xs.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Scan4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan(new Func<int, int, int>((x, y) => { throw ex; }));

            var e = xs.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Scan5()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan(8, (x, y) => x + y);

            await SequenceIdentity(xs);
        }

        [Fact]
        public async Task Scan6()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan((x, y) => x + y);

            await SequenceIdentity(xs);
        }
    }
}
