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
    public class ToDictionary : AsyncEnumerableTests
    {
        [Fact]
        public async Task ToDictionary_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(default, x => 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary(Return42, default(Func<int, int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(default, x => 0, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary(Return42, default(Func<int, int>), EqualityComparer<int>.Default));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(default, x => 0, x => 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(Return42, default, x => 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(Return42, x => 0, default));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(default, x => 0, x => 0, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary(Return42, default, x => 0, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(Return42, x => 0, default, EqualityComparer<int>.Default));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(default, x => 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary(Return42, default(Func<int, int>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(default, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary(Return42, default(Func<int, int>), EqualityComparer<int>.Default, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(default, x => 0, x => 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(Return42, default, x => 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(Return42, x => 0, default, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(default, x => 0, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary(Return42, default, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(Return42, x => 0, default, EqualityComparer<int>.Default, CancellationToken.None));
        }

        [Fact]
        public async Task ToDictionary1Async()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = await xs.ToDictionary(x => x % 2);
            Assert.True(res[0] == 4);
            Assert.True(res[1] == 1);
        }

        [Fact]
        public void ToDictionary2()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            AssertThrowsAsync<ArgumentException>(xs.ToDictionary(x => x % 2));
        }

        [Fact]
        public async Task ToDictionary3Async()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = await xs.ToDictionary(x => x % 2, x => x + 1);
            Assert.True(res[0] == 5);
            Assert.True(res[1] == 2);
        }

        [Fact]
        public void ToDictionary4()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            AssertThrowsAsync<ArgumentException>(xs.ToDictionary(x => x % 2, x => x + 1));
        }

        [Fact]
        public async Task ToDictionary5Async()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = await xs.ToDictionary(x => x % 2, new Eq());
            Assert.True(res[0] == 4);
            Assert.True(res[1] == 1);
        }

        [Fact]
        public void ToDictionary6()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            AssertThrowsAsync<ArgumentException>(xs.ToDictionary(x => x % 2, new Eq()));
        }

        [Fact]
        public async Task ToDictionary7Async()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = await xs.ToDictionary(x => x % 2, x => x, new Eq());
            Assert.True(res[0] == 4);
            Assert.True(res[1] == 1);
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
