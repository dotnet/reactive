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
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(default, x => 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(Return42, default(Func<int, int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(default, x => 0, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(Return42, default(Func<int, int>), EqualityComparer<int>.Default));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(default, x => 0, x => 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(Return42, default, x => 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(Return42, x => 0, default));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(default, x => 0, x => 0, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(Return42, default, x => 0, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(Return42, x => 0, default, EqualityComparer<int>.Default));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(default, x => 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(Return42, default(Func<int, int>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(default, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(Return42, default(Func<int, int>), EqualityComparer<int>.Default, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(default, x => 0, x => 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(Return42, default, x => 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(Return42, x => 0, default, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(default, x => 0, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(Return42, default, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(Return42, x => 0, default, EqualityComparer<int>.Default, CancellationToken.None));
        }

        [Fact]
        public void ToLookup1()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2).Result;
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.Contains(4, res[0]);
            Assert.Contains(1, res[1]);
            Assert.True(res.Count == 2);
        }

        [Fact]
        public void ToLookup2()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2).Result;
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.Contains(4, res[0]);
            Assert.Contains(2, res[0]);
            Assert.Contains(1, res[1]);
            Assert.True(res.Count == 2);
        }

        [Fact]
        public void ToLookup3()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2, x => x + 1).Result;
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.Contains(5, res[0]);
            Assert.Contains(2, res[1]);
            Assert.True(res.Count == 2);
        }

        [Fact]
        public void ToLookup4()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2, x => x + 1).Result;
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.Contains(5, res[0]);
            Assert.Contains(3, res[0]);
            Assert.Contains(2, res[1]);
            Assert.True(res.Count == 2);
        }

        [Fact]
        public void ToLookup5()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2, new Eq()).Result;
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.Contains(4, res[0]);
            Assert.Contains(1, res[1]);
            Assert.True(res.Count == 2);
        }

        [Fact]
        public void ToLookup6()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2, new Eq()).Result;
            Assert.True(res.Contains(0));
            Assert.True(res.Contains(1));
            Assert.Contains(4, res[0]);
            Assert.Contains(2, res[0]);
            Assert.Contains(1, res[1]);
            Assert.True(res.Count == 2);
        }

        [Fact]
        public void ToLookup7()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2).Result;
            foreach (var g in res)
                Assert.True(g.Key == 0 || g.Key == 1);
        }

        [Fact]
        public void ToLookup8()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2).Result;
#pragma warning disable IDE0007 // Use implicit type
            foreach (IGrouping<int, int> g in (IEnumerable)res)
                Assert.True(g.Key == 0 || g.Key == 1);
#pragma warning restore IDE0007 // Use implicit type
        }

        [Fact]
        public void ToLookup9()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2, x => x, new Eq()).Result;
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
