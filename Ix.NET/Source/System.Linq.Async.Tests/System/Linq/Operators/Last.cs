// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
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
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(default, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last<int>(default, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Last(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public void Last1()
        {
            var res = AsyncEnumerable.Empty<int>().Last();
            AssertThrowsAsync<InvalidOperationException>(res);
        }

        [Fact]
        public void Last2()
        {
            var res = AsyncEnumerable.Empty<int>().Last(x => true);
            AssertThrowsAsync<InvalidOperationException>(res);
        }

        [Fact]
        public void Last3()
        {
            var res = Return42.Last(x => x % 2 != 0);
            AssertThrowsAsync<InvalidOperationException>(res);
        }

        [Fact]
        public async Task Last4Async()
        {
            var res = Return42.Last();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task Last5Async()
        {
            var res = Return42.Last(x => x % 2 == 0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public void Last6()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).Last();
            AssertThrowsAsync(res, ex);
        }

        [Fact]
        public void Last7()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).Last(x => true);
            AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task Last8Async()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().Last();
            Assert.Equal(90, await res);
        }

        [Fact]
        public async Task Last9Async()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().Last(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }
    }
}
