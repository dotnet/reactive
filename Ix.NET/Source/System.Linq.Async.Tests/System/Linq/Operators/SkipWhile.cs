// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class SkipWhile : AsyncEnumerableTests
    {
        [Fact]
        public void SkipWhile_Sync_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhile<int>(default, x => true));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhile<int>(default, (x, i) => true));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhile(Return42, default(Func<int, bool>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhile(Return42, default(Func<int, int, bool>)));
        }

        [Fact]
        public async Task SkipWhile_Sync_Simple1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => x < 3);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhile_Sync_Simple2()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 2, 1 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => x < 3);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 1);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhile_Sync_False()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => false);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhile_Sync_True()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => true);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhile_Sync_Throws_Predicate()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(new Func<int, bool>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SkipWhile_Sync_Indexed()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => i < 2);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhile_Sync_Indexed_False()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => false);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhile_Sync_Indexed_True()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => true);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhile_Sync_Indexed_Throws_Predicate()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(new Func<int, int, bool>((x, i) => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SkipWhile_Sync_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => x < 3);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task SkipWhile_Sync_Indexed_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => false);

            await SequenceIdentity(ys);
        }
    }
}
