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
    public class Last : AsyncEnumerableTests
    {
        [Fact]
        public async Task Last_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAsync<int>(default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAsync<int>(default, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAsync(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAsync<int>(default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAsync<int>(default, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAsync(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public async Task Last1Async()
        {
            var res = AsyncEnumerable.Empty<int>().LastAsync();
            await AssertThrowsAsync<InvalidOperationException>(res);
        }

        [Fact]
        public async Task Last2Async()
        {
            var res = AsyncEnumerable.Empty<int>().LastAsync(x => true);
            await AssertThrowsAsync<InvalidOperationException>(res);
        }

        [Fact]
        public async Task Last3Async()
        {
            var res = Return42.LastAsync(x => x % 2 != 0);
            await AssertThrowsAsync<InvalidOperationException>(res);
        }

        [Fact]
        public async Task Last4Async()
        {
            var res = Return42.LastAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task Last5Async()
        {
            var res = Return42.LastAsync(x => x % 2 == 0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task Last6Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).LastAsync();
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task Last7Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).LastAsync(x => true);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task Last8Async()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().LastAsync();
            Assert.Equal(90, await res);
        }

        [Fact]
        public async Task Last9Async()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().LastAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }
    }
}
