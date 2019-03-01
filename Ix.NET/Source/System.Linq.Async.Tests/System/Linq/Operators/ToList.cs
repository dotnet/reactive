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
    public class ToList : AsyncEnumerableTests
    {
        [Fact]
        public async Task ToList_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToListAsync<int>(default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToListAsync<int>(default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task ToList_Simple()
        {
            var xs = new[] { 42, 25, 39 };
            var res = xs.ToAsyncEnumerable().ToListAsync();
            Assert.True((await res).SequenceEqual(xs));
        }

        [Fact]
        public async Task ToList_Empty()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.ToListAsync();
            Assert.True((await res).Count == 0);
        }

        [Fact]
        public async Task ToList_Throw()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).ToListAsync();
            await AssertThrowsAsync(res, ex);
        }
    }
}
