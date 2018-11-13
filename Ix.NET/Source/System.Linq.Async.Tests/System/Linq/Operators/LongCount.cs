// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class LongCount : AsyncEnumerableTests
    {
        [Fact]
        public async Task LongCount_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(default, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(default, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public async Task LongCount1()
        {
            Assert.Equal(0, await new int[0].ToAsyncEnumerable().LongCount());
            Assert.Equal(3, await new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCount());
        }

        [Fact]
        public async Task LongCount2()
        {
            Assert.Equal(0, await new int[0].ToAsyncEnumerable().LongCount(x => x < 3));
            Assert.Equal(2, await new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCount(x => x < 3));
        }

        [Fact]
        public async Task LongCount3Async()
        {
            var ex = new Exception("Bang!");
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCount(new Func<int, bool>(x => { throw ex; }));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task LongCount4Async()
        {
            var ex = new Exception("Bang!");
            await AssertThrowsAsync(Throw<int>(ex).LongCount(), ex);
        }

        [Fact]
        public async Task LongCount5Async()
        {
            var ex = new Exception("Bang!");
            await AssertThrowsAsync(Throw<int>(ex).LongCount(x => x < 3), ex);
        }
    }
}
