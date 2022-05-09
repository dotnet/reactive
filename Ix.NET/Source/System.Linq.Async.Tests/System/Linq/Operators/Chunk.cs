// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace Tests
{
    public class Chunk : AsyncEnumerableTests
    {
        [Fact]
        public async Task Chunk_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                IAsyncEnumerable<int>? xs = null;
                var ys = xs!.ChunkAsync(24);

                var e = ys.GetAsyncEnumerator();
                await e.MoveNextAsync();
            });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Chunk_NonPositiveSize(int size)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>("size", async () =>
            {
                var xs = new[] { 24 }.ToAsyncEnumerable();
                var ys = xs.ChunkAsync(size);

                var e = ys.GetAsyncEnumerator();
                await e.MoveNextAsync();
            });
        }

        [Fact]
        public async Task Chunk_Simple_Evenly()
        {
            var xs = new[] { 1, 1, 4, 5, 1, 4 }.ToAsyncEnumerable();
            var ys = xs.ChunkAsync(3);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, new[] { 1, 1, 4 });
            await HasNextAsync(e, new[] { 5, 1, 4 });
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Chunk_Simple_Unevenly()
        {
            var xs = new[] { 1, 9, 1, 9, 8, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.ChunkAsync(4);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, new[] { 1, 9, 1, 9 });
            await HasNextAsync(e, new[] { 8, 1, 0 });
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Chunk_SourceSmallerThanChunkSize()
        {
            var xs = new[] { 8, 9, 3 }.ToAsyncEnumerable();
            var ys = xs.ChunkAsync(4);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, new[] { 8, 9, 3 });
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Chunk_EmptySource()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.ChunkAsync(24);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }
    }
}
