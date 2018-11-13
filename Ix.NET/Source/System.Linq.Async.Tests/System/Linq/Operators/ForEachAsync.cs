// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if !HAS_AWAIT_FOREACH
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ForEachAsync : AsyncEnumerableTests
    {
        [Fact]
        public async Task ForEachAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(default, x => { }));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync(Return42, default(Action<int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(default, (x, i) => { }));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync(Return42, default(Action<int, int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(default, x => { }, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync(Return42, default(Action<int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(default, (x, i) => { }, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync(Return42, default(Action<int, int>), CancellationToken.None));
        }

        [Fact]
        public async Task ForEachAsync1()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            await xs.ForEachAsync(x => sum += x);
            Assert.Equal(10, sum);
        }

        [Fact]
        public async Task ForEachAsync2()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            await xs.ForEachAsync((x, i) => sum += x * i);
            Assert.Equal(1 * 0 + 2 * 1 + 3 * 2 + 4 * 3, sum);
        }

        [Fact]
        public async Task ForEachAsync3Async()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            await AssertThrowsAsync(xs.ForEachAsync(x => { throw ex; }), ex);
        }

        [Fact]
        public async Task ForEachAsync4Async()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            await AssertThrowsAsync(xs.ForEachAsync((x, i) => { throw ex; }), ex);
        }

        [Fact]
        public async Task ForEachAsync5Async()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            await AssertThrowsAsync(xs.ForEachAsync(x => { throw ex; }), ex);
        }

        [Fact]
        public async Task ForEachAsync6Async()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            await AssertThrowsAsync(xs.ForEachAsync((x, i) => { throw ex; }), ex);
        }
    }
}
#endif
