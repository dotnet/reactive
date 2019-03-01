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
    public class ToArray : AsyncEnumerableTests
    {
        [Fact]
        public async Task ToArray_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToArrayAsync<int>(default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToArrayAsync<int>(default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task ToArray_IAsyncIListProvider_Simple()
        {
            var xs = new[] { 42, 25, 39 };
            var res = xs.ToAsyncEnumerable().ToArrayAsync();
            Assert.True((await res).SequenceEqual(xs));
        }

        [Fact]
        public async Task ToArray_IAsyncIListProvider_Empty1()
        {
            var xs = new int[0];
            var res = xs.ToAsyncEnumerable().ToArrayAsync();
            Assert.True((await res).SequenceEqual(xs));
        }

        [Fact]
        public async Task ToArray_IAsyncIListProvider_Empty2()
        {
            var xs = new HashSet<int>();
            var res = xs.ToAsyncEnumerable().ToArrayAsync();
            Assert.True((await res).SequenceEqual(xs));
        }

        [Fact]
        public async Task ToArray_Empty()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.ToArrayAsync();
            Assert.True((await res).Length == 0);
        }

        [Fact]
        public async Task ToArray_Throw()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).ToArrayAsync();
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task ToArray_Query()
        {
            var xs = await AsyncEnumerable.Range(5, 50).Take(10).ToArrayAsync();
            var ex = new[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

            Assert.True(ex.SequenceEqual(xs));
        }

        [Fact]
        public async Task ToArray_Set()
        {
            var res = new[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            var xs = new HashSet<int>(res);

            var arr = await xs.ToAsyncEnumerable().ToArrayAsync();

            Assert.True(res.SequenceEqual(arr));
        }
    }
}
