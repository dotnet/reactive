// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ToLookup : AsyncEnumerableTests
    {
        [Fact]
        public async Task ToLookup_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync<int, int>(default, x => 0).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync(Return42, default(Func<int, int>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync<int, int>(default, x => 0, EqualityComparer<int>.Default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync(Return42, default, EqualityComparer<int>.Default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync<int, int, int>(default, x => 0, x => 0).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync<int, int, int>(Return42, default, x => 0).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync<int, int, int>(Return42, x => 0, default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync<int, int, int>(default, x => 0, x => 0, EqualityComparer<int>.Default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync(Return42, default, x => 0, EqualityComparer<int>.Default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync<int, int, int>(Return42, x => 0, default, EqualityComparer<int>.Default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync<int, int>(default, x => 0, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync(Return42, default(Func<int, int>), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync<int, int>(default, x => 0, EqualityComparer<int>.Default, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync(Return42, default, EqualityComparer<int>.Default, CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync<int, int, int>(default, x => 0, x => 0, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync<int, int, int>(Return42, default, x => 0, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync<int, int, int>(Return42, x => 0, default, CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync<int, int, int>(default, x => 0, x => 0, EqualityComparer<int>.Default, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync(Return42, default, x => 0, EqualityComparer<int>.Default, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookupAsync<int, int, int>(Return42, x => 0, default, EqualityComparer<int>.Default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task ToLookup1Async()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = await xs.ToLookupAsync(x => x % 2);
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.Contains(4, res[0]);
            Assert.Contains(1, res[1]);
            Assert.True(res.Count == 2);
        }

        [Fact]
        public async Task ToLookup2Async()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = await xs.ToLookupAsync(x => x % 2);
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.Contains(4, res[0]);
            Assert.Contains(2, res[0]);
            Assert.Contains(1, res[1]);
            Assert.True(res.Count == 2);
        }

        [Fact]
        public async Task ToLookup3Async()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = await xs.ToLookupAsync(x => x % 2, x => x + 1);
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.Contains(5, res[0]);
            Assert.Contains(2, res[1]);
            Assert.True(res.Count == 2);
        }

        [Fact]
        public async Task ToLookup4Async()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = await xs.ToLookupAsync(x => x % 2, x => x + 1);
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.Contains(5, res[0]);
            Assert.Contains(3, res[0]);
            Assert.Contains(2, res[1]);
            Assert.True(res.Count == 2);
        }

        [Fact]
        public async Task ToLookup5Async()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = await xs.ToLookupAsync(x => x % 2, new Eq());
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.Contains(4, res[0]);
            Assert.Contains(1, res[1]);
            Assert.True(res.Count == 2);
        }

        [Fact]
        public async Task ToLookup6Async()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = await xs.ToLookupAsync(x => x % 2, new Eq());
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.Contains(4, res[0]);
            Assert.Contains(2, res[0]);
            Assert.Contains(1, res[1]);
            Assert.True(res.Count == 2);
        }

        [Fact]
        public async Task ToLookup7Async()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = await xs.ToLookupAsync(x => x % 2);
            foreach (var g in res)
                Assert.True(g.Key == 0 || g.Key == 1);
        }

        [Fact]
        public async Task ToLookup8Async()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = await xs.ToLookupAsync(x => x % 2);
#pragma warning disable IDE0007 // Use implicit type
            foreach (IGrouping<int, int>? g in (IEnumerable)res)
            {
                Assert.NotNull(g);
                Assert.True(g!.Key == 0 || g!.Key == 1);
            }
#pragma warning restore IDE0007 // Use implicit type
        }

        [Fact]
        public async Task ToLookup9Async()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = await xs.ToLookupAsync(x => x % 2, x => x, new Eq());
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.Contains(4, res[0]);
            Assert.Contains(2, res[0]);
            Assert.Contains(1, res[1]);
            Assert.True(res.Count == 2);
        }

        private sealed class Eq : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(Math.Abs(x), Math.Abs(y));
            }

            public int GetHashCode(int obj)
            {
                return EqualityComparer<int>.Default.GetHashCode(Math.Abs(obj));
            }
        }
    }
}
